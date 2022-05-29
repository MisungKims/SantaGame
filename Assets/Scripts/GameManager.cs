/**
 * @brief 플레이어의 전반적인 것을 관리
 * @author 김미성
 * @date 22-04-20
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

    public string attendanceDate;
    public string initQuestDate;

    public string lastSaveDate;
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

    // 데이터가 저장될 경로
   // string path = Application.dataPath + "/Resources/MyData.json";

    //[Header("---------- UI 변수")]
    //[SerializeField]
    //private Slider gaugeSlider;
    //[SerializeField]
    //private Text gaugeText;
    //[SerializeField]
    //private Text lvText;
    //[SerializeField]
    //private Text goldText;
    //[SerializeField]
    //private Text carrotsText;
    //[SerializeField]
    //private Text diaText;
    //[SerializeField]
    //private Text citizenCountText;
    //[SerializeField]
    //private Text dateText;
    //[SerializeField]
    //private GameObject gaugeBellImage;

    private Animator gaugeAnim;

    public Text text;   // 나중에 지워야함


    // 플레이어의 값
    StringBuilder gaugeSb = new StringBuilder();
    private float gauge;
    public float Gauge
    {
        get{ return gauge; }
        set
        {
            gauge = value;

            if (GameLoadManager.CurrentScene().name == "SantaVillage")
            {
                UIManager.Instance.gaugeSlider.value = gauge;

                gaugeSb.Clear();
                gaugeSb.Append(gauge.ToString("N0"));
                gaugeSb.Append("%");

                UIManager.Instance.gaugeText.text = gaugeSb.ToString();
            }
        }
    }

    private int level = 1;
    public int Level
    {
        get { return level; }
        set
        {
            level = value;
            UIManager.Instance.lvText.text = string.Format("{0:D2}", level);
        }
    }

    private BigInteger myGold;
    public BigInteger MyGold
    {
        get { return myGold; }
        set
        {
            myGold = value;
            UIManager.Instance.goldText.text = GoldManager.ExpressUnitOfGold(myGold);
        }
    }

    private BigInteger myCarrots;
    public BigInteger MyCarrots
    {
        get { return myCarrots; }
        set
        {
            myCarrots = value;
            UIManager.Instance.carrotsText.text = GoldManager.ExpressUnitOfGold(MyCarrots);
        }
    }

  

    private int myDia;
    public int MyDia
    {
        get { return myDia; }
        set
        {
            myDia = value;
            UIManager.Instance.diaText.text = myDia.ToString();
        }
    }

    private int citizenCount = 0;
    public int CitizenCount
    {
        get { return citizenCount; }
        set
        {
            citizenCount = value;
            UIManager.Instance.citizenCountText.text = citizenCount.ToString();

            if (citizenCount != 0 && citizenCount % 5 == 0)      // 주민의 수가 5의 배수일 때 게이지 증가
            {
                IncreaseGauge(5);
            }
        }
    }

    private int lastDay;
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

            if (GameLoadManager.CurrentScene().name == "SantaVillage")
            {
                UIManager.Instance.dateText.text = String.Format("{0}년 {1}월 {2}일", year, month, day);
            }
        }
    }

    public float dayCount = 600f;        // 게임 속 에서 몇초마다 다음 날이 될 지

    public float goldEfficiency = 1.0f;         // 토끼 주민 초대 시 증가할 효율

    public string attendanceDate;              // 마지막으로 출석 보상을 받은 날짜

    public string initQuestDate;            // 퀘스트를 초기화한 날짜

    private WaitForSeconds waitForSeconds;      // 캐싱

    [SerializeField]
    private GameObject deliveryGame;

    [SerializeField]
    private GameObject vaillage;


    #endregion

    /* public void SuccessQuest()
    {
        AchivementManager.Instance.Success(questid);
    }*/

    #region 함수
    /// <summary>
    /// 레벨 업
    /// </summary>
    private void LevelUp()
    {
        Level++;

        float remain = gauge - 100.0f;
        Gauge = remain;

        for (int i = 0; i < StoreManager.Instance.storeObjectList.Count; i++)
        {
            StoreManager.Instance.storeObjectList[i].Check();
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
        Gauge = gauge + amount;

        if (gauge >= 100.0f)
            LevelUp();
    }

    public void GetCarrot(BigInteger amount)
    {
        myCarrots += amount;
    }

    public void StartDeliveryGame()
    {
        deliveryGame.SetActive(true);
        vaillage.SetActive(false);
    }

    public void EndDeliveryGame()
    {
        vaillage.SetActive(true);
        deliveryGame.SetActive(false);
    }

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
        data.day = day;
        data.attendanceDate = attendanceDate;
        data.initQuestDate = initQuestDate;
        data.lastSaveDate = DateTime.Now.ToString("yyyy.MM.dd");

        File.WriteAllText(Application.dataPath + "/Resources/MyData.json", JsonUtility.ToJson(data));
    }
    
    public bool LoadData()
    {
        FileInfo fileInfo = new FileInfo(Application.dataPath + "/Resources/MyData.json");
        if (fileInfo.Exists)
        {
            string dataStr = File.ReadAllText(Application.dataPath + "/Resources/MyData.json");
            SaveData data = JsonUtility.FromJson<SaveData>(dataStr);
            Level = data.level;
            Gauge = data.gauge;
            MyGold = GoldManager.UnitToBigInteger(data.gold);
            MyCarrots = GoldManager.UnitToBigInteger(data.carrot);
            MyDia = data.dia;
            CitizenCount = data.citizenCount;
            year = data.year;
            month = data.month;
            day = data.day;
            attendanceDate = data.attendanceDate;
            initQuestDate = data.initQuestDate;

            return true;
        }

        return false;
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
        //SoundManager.Instance.PlaySoundEffect(ESoundEffectType.getGoldButton);      // 효과음 실행

        gaugeAnim.SetBool("isIncrease", true);      // SantaVillage 씬에서만 게이지 상승 애니메이션 실행

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



    #region 유니티 함수

    #endregion
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
            // 로드 실패시 초기값 설정
            Level = 1;
            Gauge = 0;
            CitizenCount = 0;
            Day = 1;
            MyGold = 1000;
            MyCarrots = 1000;
            MyDia = 0;
            year = 0;
            month = 1;
            day = 1;
        }
       

        waitForSeconds = new WaitForSeconds(dayCount);

        gaugeAnim = UIManager.Instance.gaugeBellImage.GetComponent<Animator>();
    }

    void Start()
    {
        StartCoroutine(DateCounting());

        SoundManager.Instance.PlayBGM(EBgmType.main);

        //BigInteger start = 100;
        //string value = start.ToString();
        //float sf = 1.7f;

        //Debug.Log(value);

        //for (int i = 0; i < 100; i++)
        //{
        //    value = GoldManager.MultiplyUnit(value, sf);
        //    sf += 0.5f;
        //    text.text += value + "\n";
        //}


        //Debug.Log(BigIntegerManager.UnitToValue(BigInteger.Pow(1000, 702)));

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
}
