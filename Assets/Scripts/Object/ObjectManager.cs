/**
 * @details CSV 파일을 파싱하여 오브젝트(건물, 산타) 리스트 생성
 * @author 김미성
 * @date 22-04-26
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ObjectManager : MonoBehaviour
{
    #region 변수

    public int unlockCount = 1;     // 잠금해제된 건물의 수
    
    // 리스트
    public List<Building> buildingList = new List<Building>();
    
    public List<Santa> santaList = new List<Santa>();
    
    public List<Object> objectList = new List<Object>();    // 오브젝트의 모든 정보를 가진 리스트

    public Sprite[] buildingSprites;
    public Sprite[] santaSprites;


    // 싱글톤
    private static ObjectManager instance;
    public static ObjectManager Instance
    {
        get { return instance; }
    }
    #endregion


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

        if (!LoadData())
        {
            ReadCSV();
        }
        

        SetBuilidng();

        SetSanta();
    }

    /// <summary>
    /// csv 리더를 통해 StoreData 파일 가져오기
    /// </summary>
    void ReadCSV()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("StoreData");

        for (int i = 0; i < data.Count; i++)         // 가져온 내용으로 StoreObject 생성
        {
            string prerequisites;
            bool isGetPrerequisites;
            if (i == 0)
            {
                isGetPrerequisites = true;
                prerequisites = null;
            }
            else
            {
                isGetPrerequisites = false;
                prerequisites = data[i - 1]["이름"].ToString();
            }        

            Object newObject = new Object(
                data[i]["Desc"].ToString(),
                (int)data[i]["잠금 해제 레벨"],
                (int)data[i]["초"],
                data[i]["이름"].ToString(),
                0,
                data[i]["건물 가격"].ToString(),
                (float)data[i]["건물 가격 배수"],
                data[i]["골드 증가량"].ToString(),
                buildingSprites[i],
                data[i]["산타 이름"].ToString(),
                0,
                data[i]["산타 가격"].ToString(),
                (int)data[i]["산타 가격 배수"],
                (int)data[i]["알바 효율 증가"],
                santaSprites[i],
                prerequisites,
                isGetPrerequisites
                );

            objectList.Add(newObject);

            //buildingList[i].buildingObj = objectList[i];
        }
    }
    /// TODO : 하이라키에서 다 셋팅 후에 밑에꺼 위로 올리기

    /// <summary>
    /// 건물의 값 초기화
    /// </summary>
    void SetBuilidng()
    {
        for (int i = 0; i < buildingList.Count; i++)
        {
            buildingList[i].buildingObj = objectList[i];
            buildingList[i].index = i;

            if (objectList[i].buildingLevel > 0)    // 잠금해제된 건물이라면 보이게
            {
                buildingList[i].Init();
            }
        }
    }

    /// <summary>
    /// 산타의 값 초기화
    /// </summary>
    void SetSanta()
    {
        for (int i = 0; i < santaList.Count; i++)
        {
            santaList[i].santaObj = objectList[i];
            santaList[i].index = i;

            if (objectList[i].santaLevel > 0)    // 구입한 산타라면 보이게
            {
                santaList[i].Init();
            }
        }
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
        string jdata = JsonUtility.ToJson(new Serialization<Object>(objectList));
        File.WriteAllText(Application.dataPath + "/Resources/ObjectData.json", jdata);
    }

    /// <summary>
    /// 데이터 로드
    /// </summary>
    /// <returns>불러오기 성공 여부</returns>
    public bool LoadData()
    {
        FileInfo fileInfo = new FileInfo(Application.dataPath + "/Resources/ObjectData.json");
        if (fileInfo.Exists)
        {
            string jdata = File.ReadAllText(Application.dataPath + "/Resources/ObjectData.json");

            objectList = JsonUtility.FromJson<Serialization<Object>>(jdata).target;

            return true;
        }

        return false;
    }
}
