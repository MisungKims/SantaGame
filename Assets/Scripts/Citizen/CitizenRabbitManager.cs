/**
 * @brief 산타 주민을 관리
 * @author 김미성
 * @date 22-06-02
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class goal
{
    public goalObject[] goalObjects;
}

[System.Serializable]
public class goalObject
{
    public Transform pos;           // 건물에서 주민이 갈 수 있는 위치
    public Transform lookAtPos;     // 바라봐야하는 위치
    public bool isUse;              // 다른 주민이 이미 사용 중인가?
}


[System.Serializable]
public class Citizen            // 저장하려는 변수들
{
    public string name;
    public int materiaIdx;
    public int clothesIdx;
    public Vector3 pos;

    public Citizen(string name, int materiaIdx, int clothesIdx, Vector3 pos)
    {
        this.name = name;
        this.materiaIdx = materiaIdx;
        this.clothesIdx = clothesIdx;
        this.pos = pos;
    }
}

public class CitizenRabbitManager : MonoBehaviour
{
    #region 변수
    public List<goal> goalPositions = new List<goal>();       // 주민이 갈 수 있는 건물의 위치 리스트

    public List<Citizen> citizenList = new List<Citizen>();     // 주민의 정보를 담고있는 리스트

    [HideInInspector]
    public List<RabbitCitizen> rabbitCitizens = new List<RabbitCitizen>();
    
    // 싱글톤
    private static CitizenRabbitManager instance;
    public static CitizenRabbitManager Instance
    {
        get { return instance; }
    }

    public Material[] materials;        // 주민 머터리얼 배열

    public RabbitCitizen rabbit;        // 토끼 주민 프리팹
    public GameObject rabbitGroup;      // 토끼 주민 하이라키의 부모

    public Transform zeroPos;

    // 데이터 저장
    bool isPaused = false;      //앱의 활성화 상태
    #endregion

    #region 유니티 함수
    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
        }

        LoadData();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            isPaused = true;

            SaveData();         // 앱이 비활성화되었을 때 데이터 저장
        }

        else
        {
            if (isPaused)
            {
                isPaused = false;
                /* 앱이 활성화 되었을 때 처리 */
            }
        }
    }

    void OnApplicationQuit()
    {
        SaveData();         // 앱 종료 시 데이터 저장
    }
    #endregion

    #region 함수
    /// <summary>
    /// 데이터 저장
    /// </summary>
    void SaveData()
    {
        for (int i = 0; i < citizenList.Count; i++)
        {
            // 주민의 옷 정보 저장
            if (rabbitCitizens[i].isWearing)
            {
                citizenList[i].clothesIdx = (int)rabbitCitizens[i].clothes.flag;
            }
            else
            {
                citizenList[i].clothesIdx = -1;
            }

            // 주민의 위치 저장
            citizenList[i].pos = rabbitCitizens[i].transform.position;
        }

        string jdata = JsonUtility.ToJson(new Serialization<Citizen>(citizenList));
        File.WriteAllText(Application.persistentDataPath + "/CitizenData.json", jdata);
    }

    /// <summary>
    /// 데이터 로드
    /// </summary>
    /// <returns>불러오기 성공 여부</returns>
    public bool LoadData()
    {
        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/CitizenData.json");
        if (fileInfo.Exists)
        {
            GameManager gameManager = GameManager.Instance;

            string jdata = File.ReadAllText(Application.persistentDataPath + "/CitizenData.json");

            citizenList = JsonUtility.FromJson<Serialization<Citizen>>(jdata).target;
            for (int i = 0; i < citizenList.Count; i++)
            {
                // 저장된 데이터를 불러와 토끼 주민 생성
                RabbitCitizen rabbitCitizen = RabbitCitizen.Instantiate(rabbit, citizenList[i].pos, Quaternion.identity, rabbitGroup.transform);

                rabbitCitizen.rabbitMat.material = materials[citizenList[i].materiaIdx];
                rabbitCitizen.name = citizenList[i].name;
                if (citizenList[i].clothesIdx > -1)
                {
                    rabbitCitizen.PutOn(ClothesManager.Instance.clothesList[citizenList[i].clothesIdx]);
                    gameManager.goldEfficiency *= 1.5f;
                }
                rabbitCitizens.Add(rabbitCitizen);
            }

            return true;
        }

        return false;
    }
    #endregion
}
