/**
 * @brief 플레이어의 전반적인 것을 관리
 * @author 김미성
 * @date 22-06-04
 */

using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using System.IO;

[System.Serializable]
public class Serialization<T>
{
    public Serialization(List<T> _target) => target = _target;
    public List<T> target;
}

public class SaveData
{
    public int level;
    public float gauge;
    public string gold;
    public string carrot;
    public int dia;
    public int citizenCount;
    public int day;
    public int month;
    public int year;

    public string inviteRabbitPrice;

    public string attendanceDate;
    public string initQuestDate;

    public string lastConnectionTime;
}

public class GameManager : MonoBehaviour
{
    #region 변수
    // 싱글톤
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    private Animator gaugeAnim;


    // 플레이어의 값
    StringBuilder gaugeSb = new StringBuilder();
    private float gauge;
    public float Gauge
    {
        get{ return gauge; }
        set
        {
            gauge = value;

            UIManagerInstance().gaugeSlider.value = gauge;

            gaugeSb.Clear();
            gaugeSb.Append(gauge.ToString("N0"));
            gaugeSb.Append("%");

            UIManagerInstance().gaugeText.text = gaugeSb.ToString();
        }
    }


    private int level = 1;
    public int Level
    {
        get { return level; }
        set
        {
            level = value;
            UIManagerInstance().lvText.text = level.ToString("D2");
            
        }
    }

    private BigInteger myGold;
    public BigInteger MyGold
    {
        get { return myGold; }
        set
        {
            myGold = value;
            UIManagerInstance().goldText.text = GoldManager.ExpressUnitOfGold(myGold);
        }
    }

    private BigInteger myCarrots;
    public BigInteger MyCarrots
    {
        get { return myCarrots; }
        set
        {
            myCarrots = value;
            UIManagerInstance().carrotsText.text = GoldManager.ExpressUnitOfGold(MyCarrots);
        }
    }

  

    private int myDia;
    public int MyDia
    {
        get { return myDia; }
        set
        {
            myDia = value;
            UIManagerInstance().diaText.text = myDia.ToString();
        }
    }

    private int citizenCount = 0;
    public int CitizenCount
    {
        get { return citizenCount; }
        set
        {
            citizenCount = value;
            UIManagerInstance().citizenCountText.text = citizenCount.ToString();

            if (citizenCount != 0 && citizenCount % 5 == 0)      // 주민의 수가 5의 배수일 때 게이지 증가
            {
                IncreaseGauge(10);
            }
        }
    }

    StringBuilder daySb = new StringBuilder();
    public int lastDay;
    private int month = 1, year = 0;
    private int day = 1;
    public int Day
    {
        get { return day; }
        set
        {
            day = value;

            if (month == 1 && month == 3 && month == 5 && month == 7 && month == 8 && month == 10 && month == 12)
                lastDay = 32;
            else
                lastDay = 31;

            if (day > lastDay)
            {
                day = 1;
                if (month == 12)
                {
                    year++;
                    month = 1;
                }
                else month++;
            }

            daySb.Clear();
            daySb.Append(year);
            daySb.Append("년 ");
            daySb.Append(month);
            daySb.Append("월 ");
            daySb.Append(day);
            daySb.Append("일");
            UIManagerInstance().dateText.text = daySb.ToString();

            // 12월 25일에만 선물전달버튼이 보이도록
            if (month == 12 && day == 25)
            {
                deliveryButtonObj.SetActive(true);
            }
            else if(deliveryButtonObj.activeSelf)
            {
                deliveryButtonObj.SetActive(false);
            }
        }
    }

    public float dayCount = 600f;        // 게임 속 에서 몇초마다 다음 날이 될 지

    public float goldEfficiency = 1.0f;         // 토끼 주민 초대 시 증가할 효율

    public string attendanceDate;              // 마지막으로 출석 보상을 받은 날짜

    public string initQuestDate;            // 퀘스트를 초기화한 날짜

    public string lastConnectionTime;       // 마지막으로 접속한 시간

    public string inviteRabbitPrice;        // 토끼 주민 초대 비용


    // 오프라인
    public int goneMonth = 0;       // 오프라인 시간동안 지나간 달
    public float diffTotalSeconds;  // 오프라인 시간 (초)


    //앱의 활성화 상태를 저장하는 변수
    bool isPaused = false;


    // 캐싱
    private WaitForSeconds waitForSeconds;
    private UIManager uIManager;
    private SoundManager soundManager;
    private StoreManager storeManager;

    
    // 선물 전달 게임
    [SerializeField]
    private GameObject deliveryButtonObj;

    [SerializeField]
    private GameObject deliveryGame;
    #endregion

    #region 유니티 함수
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);      // 씬 전환 시에도 파괴되지 않음
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }

        // 데이터 로드
        if (!LoadData())
        {
            InitData();         // 로드 실패시 초기값 설정
        }

        waitForSeconds = new WaitForSeconds(dayCount);

        gaugeAnim = UIManagerInstance().gaugeBellImage.GetComponent<Animator>();
    }

    void Start()
    {
        StartCoroutine(DateCounting());     // 날짜 세기 시작

        SoundManagerInstance().PlayBGM(EBgmType.main);
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
                OfflineTime();
            }
        }
    }

    void OnApplicationQuit()
    {
        SaveData();         // 앱 종료 시 데이터 저장
    }
    #endregion

    #region 코루틴
    /// <summary>
    /// 날짜 세기
    /// </summary>
    /// <returns></returns>
    IEnumerator DateCounting()
    {
        while (true)
        {
            yield return waitForSeconds;

            Day++;
        }
    }

    /// <summary>
    /// 게이지 상승
    /// </summary>
    /// <param name="amount">획득할 게이지</param>
    IEnumerator IncreaseGaugeCorou(float amount)
    {
        gaugeAnim.SetBool("isIncrease", true);      // 게이지 상승 애니메이션 실행

        float goalGuage = gauge + amount;

        while (gauge < goalGuage && gauge < 100)
        {
            Gauge += 0.1f;

            yield return new WaitForSeconds(0.001f);
        }

        Gauge = goalGuage;

        gaugeAnim.SetBool("isIncrease", false);

        if (gauge >= 100.0f)
            LevelUp();
    }

    #endregion

    #region 함수
    /// <summary>
    /// 레벨 업
    /// </summary>
    private void LevelUp()
    {
        Level++;

        float remain = Gauge - 100.0f;
        Gauge = remain;

        // 상점 오브젝트의 조건을 체크
        for (int i = 0; i < StoreManagerInstance().storeObjectList.Count; i++)
        {
            StoreManagerInstance().storeObjectList[i].Check();
        }
    }

    /// <summary>
    /// 게이지 상승 실행
    /// </summary>
    /// <param name="amount">획득할 게이지</param>
    public void IncreaseGauge(float amount)
    {
        StartCoroutine(IncreaseGaugeCorou(amount));
    }

    /// <summary>
    /// 게이지 상승(애니메이션 실행하지 않음)
    /// </summary>
    /// <param name="amount"></param>
    public void IncreaseGaugeNotAnim(float amount)
    {
        Gauge += amount;

        if (Gauge >= 100.0f) LevelUp();
    }

    /// <summary>
    /// 선물 전달 게임 시작
    /// </summary>
    public void StartDeliveryGame()
    {
        deliveryGame.SetActive(true);
        UIManagerInstance().StartDeliveryGame();
    }

    /// <summary>
    /// 선물 전달 게임 종료
    /// </summary>
    public void EndDeliveryGame()
    {
        UIManagerInstance().EndDeliveryGame();
        deliveryGame.SetActive(false);
    }

    /// <summary>
    /// 데이터 저장
    /// </summary>
    public void SaveData()
    {
        SaveData data = new SaveData();
        data.level = Level;
        data.gauge = Gauge;
        data.gold = GoldManager.ExpressUnitOfGold(MyGold);
        data.carrot = GoldManager.ExpressUnitOfGold(MyCarrots);
        data.dia = MyDia;
        data.citizenCount = citizenCount;
        data.year = year;
        data.month = month;
        data.day = Day;
        data.inviteRabbitPrice = inviteRabbitPrice;
        data.attendanceDate = attendanceDate;
        data.initQuestDate = initQuestDate;
        data.lastConnectionTime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

        File.WriteAllText(Application.persistentDataPath + "/MyData.json", JsonUtility.ToJson(data));
    }
    
    /// <summary>
    /// 데이터 로드
    /// </summary>
    /// <returns></returns>
    public bool LoadData()
    {
        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/MyData.json");

        if (fileInfo.Exists)
        {
            string dataStr = File.ReadAllText(Application.persistentDataPath + "/MyData.json");

            SaveData data = JsonUtility.FromJson<SaveData>(dataStr);
            Level = data.level;
            Gauge = data.gauge;
            MyGold = GoldManager.UnitToBigInteger(data.gold);
            MyCarrots = GoldManager.UnitToBigInteger(data.carrot);
            MyDia = data.dia;
            CitizenCount = data.citizenCount;
            year = data.year;
            month = data.month;
            Day = data.day;
            inviteRabbitPrice = data.inviteRabbitPrice;
            attendanceDate = data.attendanceDate;
            initQuestDate = data.initQuestDate;
            lastConnectionTime = data.lastConnectionTime;

            OfflineTime();

            return true;
        }

        return false;
    }

    /// <summary>
    /// 값 초기화
    /// </summary>
    void InitData()
    {
        Level = 1;
        Gauge = 0;
        CitizenCount = 0;
        Day = 1;
        MyGold = 1000;
        MyCarrots = 1000;
        MyDia = 0;
        year = 0;
        month = 1;
        inviteRabbitPrice = "100";
        lastConnectionTime = "";
    }

    /// <summary>
    /// 오프라인 시간 동안 얻은 보상을 계산
    /// </summary>
    public void OfflineTime()
    {
        goneMonth = 0;

        if (lastConnectionTime.Equals("")) return;

        // 지난 앱 비활성화 시간을 가져와
        DateTime lastConnection = DateTime.ParseExact(lastConnectionTime, "yyyy-MM-dd-HH-mm-ss", System.Globalization.CultureInfo.InvariantCulture);

        // 오프라인 시간을 초로 계산
        TimeSpan timeDiff = DateTime.Now - lastConnection;
        diffTotalSeconds = (float)timeDiff.TotalSeconds;

        if (diffTotalSeconds > 21600f)       // 최대 360분
        {
            diffTotalSeconds = 21600f;
        }

        int goneDay = (int)(diffTotalSeconds / dayCount);       // 지나간 날짜
        int lastMonth = month;
        for (int i = 0; i < goneDay; i++)
        {
            Day++;

            if (lastMonth - month < 0)
            {
                goneMonth++;
            }

            lastMonth = month;
        }
    }

    /// <summary>
    /// UIManager 인스턴스 반환
    /// </summary>
    /// <returns></returns>
    UIManager UIManagerInstance()
    {
        if (!uIManager)
        {
            uIManager = UIManager.Instance;
        }

        return uIManager;
    }

    /// <summary>
    /// SoundManager 인스턴스 반환
    /// </summary>
    /// <returns></returns>
    SoundManager SoundManagerInstance()
    {
        if (!soundManager)
        {
            soundManager = SoundManager.Instance;
        }

        return soundManager;
    }

    /// <summary>
    /// UIManager 인스턴스 반환
    /// </summary>
    /// <returns></returns>
    StoreManager StoreManagerInstance()
    {
        if (!storeManager)
        {
            storeManager = StoreManager.Instance;
        }

        return storeManager;
    }
    #endregion
}
