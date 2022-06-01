/**
 * @brief 산타 주민을 관리
 * @author 김미성
 * @date 22-04-22
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
    public Transform pos;     // 건물에서 주민이 갈 수 있는 위치
    public Transform lookAtPos;     // 바라봐야하는 위치
    public bool isUse;          // 다른 주민이 이미 사용 중인가?
}

[System.Serializable]
public class Citizen
{
    public string name;
    public Material material;
    public Vector3 pos;

    public Citizen(string name, Material material, Vector3 pos)
    {
        this.name = name;
        this.material = material;
        this.pos = pos;
    }
}

public class CitizenRabbitManager : MonoBehaviour
{

    public List<goal> goalPositions = new List<goal>();       // 주민이 갈 수 있는 건물의 위치 리스트

    public List<Citizen> citizenList = new List<Citizen>();

    public List<RabbitCitizen> rabbitCitizens = new List<RabbitCitizen>();

    // 싱글톤
    private static CitizenRabbitManager instance;
    public static CitizenRabbitManager Instance
    {
        get { return instance; }
    }

    public Material[] materials;

    public GameObject rabbit;
    public GameObject rabbitGroup;

    public Transform zeroPos;

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

    //앱의 활성화 상태를 저장하는 변수
    bool isPaused = false;

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

    /// <summary>
    /// 데이터 저장
    /// </summary>
    void SaveData()
    {
        for (int i = 0; i < citizenList.Count; i++)
        {
            citizenList[i].name = rabbitCitizens[i].name;
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
            string jdata = File.ReadAllText(Application.persistentDataPath + "/CitizenData.json");

            citizenList = JsonUtility.FromJson<Serialization<Citizen>>(jdata).target;
            for (int i = 0; i < citizenList.Count; i++)
            {
                RabbitCitizen rabbitCitizen = GameObject.Instantiate(rabbit, citizenList[i].pos, Quaternion.identity, rabbitGroup.transform).GetComponent<RabbitCitizen>();
                if (citizenList[i].material == null)
                {
                    int rand = Random.Range(0, 12);
                    citizenList[i].material = materials[rand];
                }
                rabbitCitizen.rabbitMat.material = citizenList[i].material;
                rabbitCitizen.name = citizenList[i].name;
                rabbitCitizens.Add(rabbitCitizen);
            }

            return true;
        }

        return false;
    }

}
