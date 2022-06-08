/**
 * @details CSV 파일을 파싱하여 오브젝트(건물, 산타) 리스트 생성
 * @author 김미성
 * @date 22-06-02
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Numerics;
using Random = UnityEngine.Random;
using System.Text;


public class ObjectManager : MonoBehaviour
{
    #region 변수

    //public int unlockCount = 1;     // 잠금해제된 건물의 수
    
    // 리스트
    public List<Building> buildingList = new List<Building>();
    
    public List<Santa> santaList = new List<Santa>();
    
    public List<Object> objectList = new List<Object>();    // 오브젝트의 모든 정보를 가진 리스트

    public Sprite[] buildingSprites;
    public Sprite[] santaSprites;

    public bool isWatchingAds = false;

    // 싱글톤
    private static ObjectManager instance;
    public static ObjectManager Instance
    {
        get { return instance; }
    }

    //앱의 활성화 상태를 저장하는 변수
    bool isPaused = false;
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

        if (!LoadData())
        {
            // 데이터 로드에 실패 시 CSV파일에서 불러옴
            ReadCSV();
        }

        SetBuilidng();

        SetSanta();
    }

    private void Start()
    {
        OfflineTime();
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

                // 광고를 보고 올때가 아니라면 오프라인 보상
                if (!isWatchingAds)
                {
                    OfflineTime();
                }
                else
                {
                    isWatchingAds = false;
                }
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
    /// csv 리더를 통해 StoreData 파일 가져오기
    /// </summary>
    void ReadCSV()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("StoreData");

        for (int i = 0; i < data.Count; i++)         // 가져온 내용으로 StoreObject 생성
        {
            // 선행 조건 설정
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

            if (objectList[i].santaSprite == null)
            {
                objectList[i].santaSprite = santaSprites[i];
            }
        }
    }
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
            else
            {
                buildingList[i].gameObject.SetActive(false);
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
            else
            {
                santaList[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 오프라인 시간 동안 얻은 보상을 계산
    /// </summary>
    public void OfflineTime()
    {
        GameManager gameManager = GameManager.Instance;
        UIManager uIManager = UIManager.Instance;
        GetOfflineGoldWindow getOfflineGoldWindow = uIManager.getOfflineGoldWindow;

        string sLastTime = gameManager.lastConnectionTime;
        if (sLastTime.Equals("")) return;

        // 획득할 보상을 계산
        BigInteger goldAmount = GetGold(gameManager.diffTotalSeconds);
        string gold = GoldManager.ExpressUnitOfGold(goldAmount);

        BigInteger carrotAmount = GetCarrot(gameManager.diffTotalSeconds);
        string carrot = GoldManager.ExpressUnitOfGold(carrotAmount);

        // 보상을 얻지 못했으면 return
        if (gold.Equals("0.0 ") && carrot.Equals("0.0 "))        
        {
            return;            
        }


        // 골드 획득
        gameManager.MyGold += goldAmount;
        getOfflineGoldWindow.goldText.text = gold;

        // 당근 획득
        gameManager.MyCarrots += carrotAmount;
        getOfflineGoldWindow.carrotText.text = carrot;


        // 오프라인 시간을 Text에 보여줌
        getOfflineGoldWindow.timeText.text = OfflinesString(gameManager.diffTotalSeconds);


        // 오프라인 보상 획득창 활성화
        getOfflineGoldWindow.gameObject.SetActive(true);
        uIManager.SetisOpenPanel(true);
        CameraMovement.Instance.canMove = false;
    }

    /// <summary>
    /// 오프라인 시간을 "0 min" 형태로 반환
    /// </summary>
    /// <returns></returns>
    String OfflinesString(float diffTotalSeconds)
    {
        StringBuilder sb = new StringBuilder();
        if (diffTotalSeconds < 60)
        {
            sb = new StringBuilder();
            sb.Append(((int)diffTotalSeconds).ToString());
            sb.Append(" sec");
        }
        else
        {
            sb = new StringBuilder();
            sb.Append((diffTotalSeconds / 60).ToString("F0"));
            sb.Append(" min");
        }

        return sb.ToString();
    }

    /// <summary>
    /// 획득할 골드를 계산
    /// </summary>
    /// <param name="diffTotalSeconds">오프라인 시간</param>
    /// <returns></returns>
    BigInteger GetGold(float diffTotalSeconds)
    {
        BigInteger amount = 0;

        for (int i = 0; i < objectList.Count; i++)
        {
            if (objectList[i].santaLevel > 0)       // 골드 생산 자동화된 건물이라면
            {
                int count = (int)(diffTotalSeconds / objectList[i].second);      // 오프라인 시간동안 몇번 골드를 획득했는지 계산
                string multiple = GoldManager.MultiplyUnit(objectList[i].incrementGold, count);

                amount += GoldManager.UnitToBigInteger(multiple);
            }
        }

        return amount;
    }

    /// <summary>
    /// 획득할 당근을 계산
    /// </summary>
    /// <param name="diffTotalSeconds">오프라인 시간</param>
    /// <returns></returns>
    BigInteger GetCarrot(float diffTotalSeconds)
    {
        BigInteger amount = 0;

        int citizenCount = CitizenRabbitManager.Instance.rabbitCitizens.Count;
        for (int i = 0; i < citizenCount; i++)                  // 생성된 토끼 주민 수 만큼
        {
            int count = (int)(diffTotalSeconds / Random.Range(60.0f, 120.0f));      // 오프라인 시간동안 몇번 당근을 획득했는지 계산
            string multiple = GoldManager.MultiplyUnit("100.0A", count);

            amount += GoldManager.UnitToBigInteger(multiple);
        }

        return amount;
    }

    /// <summary>
    /// 데이터 저장
    /// </summary>
    void SaveData()
    {
        string jdata = JsonUtility.ToJson(new Serialization<Object>(objectList));
        File.WriteAllText(Application.persistentDataPath + "/ObjectData.json", jdata);
    }

    /// <summary>
    /// 데이터 로드
    /// </summary>
    /// <returns>불러오기 성공 여부</returns>
    public bool LoadData()
    {
        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/ObjectData.json");
        if (fileInfo.Exists)
        {
            string jdata = File.ReadAllText(Application.persistentDataPath + "/ObjectData.json");

            objectList = JsonUtility.FromJson<Serialization<Object>>(jdata).target;
            for (int i = 0; i < objectList.Count; i++)
            {
                objectList[i].buildingSprite = buildingSprites[i];
                objectList[i].santaSprite = santaSprites[i];
            }
            return true;
        }

        return false;
    }
    #endregion
}
