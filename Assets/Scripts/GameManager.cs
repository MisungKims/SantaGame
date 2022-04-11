using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;


public class GameManager : MonoBehaviour
{
    #region 변수
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

    [Header("---------- 메인 오브젝트")]
    [SerializeField]
    private Slider gaugeSlider;        // 게이지를 나타내는 슬라이더
    [SerializeField]
    private Text gaugeText;            // 게이지를 나타내는 텍스트
    [SerializeField]
    private Text lvText;               // 레벨을 나타내는 텍스트
    [SerializeField]
    private Text goldText;             // 돈을 나타내는 텍스트
    [SerializeField]
    private Text carrotsText;          // 당근을 나타내는 텍스트
    [SerializeField]
    private Text diaText;              // 다이아를 나타내는 텍스트
    [SerializeField]
    private Text santaCountText;              // 다이아를 나타내는 텍스트
    [SerializeField]
    private Text dateText;             // 날짜를 나타내는 텍스트

    public Text text;             // 날짜를 나타내는 텍스트

    public string getAttendanceRewardDate;      // 마지막으로 출석 보상을 받은 날짜
    [SerializeField]
    private GameObject attendanceNotificationImage;     // 출석 보상 알림 이미지      


    [Header("---------- 패널")]
    public GameObject mainPanel;     // 상점 패널
    public GameObject storePanel;     // 상점 패널
    public GameObject clickObjWindow;     // 클릭 오브젝트 패널

    [Header("---------- 플레이어 값")]
    [SerializeField]
    private float gauge;
    [SerializeField]
    private int level = 1;
    [SerializeField]
    private int santaCount = 0;

  

    private BigInteger myGold = 3560000;
    private BigInteger myCarrots = 13000;
    private int myDia = 0;
    //private BigInteger myDia = 36000;

    StringBuilder gaugeSb = new StringBuilder();

    public float Gauge
    {
        get{ return gauge; }
        set
        {
            gauge = value;
            gaugeSlider.value = gauge;

            gaugeSb.Clear();
            gaugeSb.Append(gauge.ToString());
            gaugeSb.Append("%");
            gaugeText.text = gaugeSb.ToString();
        }
    }

    public int Level
    {
        get { return level; }
        set
        {
            level = value;
            lvText.text = string.Format("{0:D2}", level);
        }
    }

    public BigInteger MyGold
    {
        get { return myGold; }
        set
        {
            myGold = value;
            goldText.text = GoldManager.ExpressUnitOfGold(myGold);
        }
    }

    public BigInteger MyCarrots
    {
        get { return myCarrots; }
        set
        {
            myCarrots = value;
            carrotsText.text = GoldManager.ExpressUnitOfGold(MyCarrots);
        }
    }

    public int MyDia
    {
        get { return myDia; }
        set
        {
            myDia = value;
            diaText.text = myDia.ToString();
        }
    }

    //public BigInteger MyDia
    //{
    //    get { return myDia; }
    //    set
    //    {
    //        myDia = value;
    //        diaText.text = GoldManager.ExpressUnitOfGold(myDia);
    //    }
    //}
    public int SantaCount
    {
        get { return santaCount; }
        set
        {
            santaCount = value;
            santaCountText.text = santaCount.ToString();
        }
    }

    public float goldEfficiency = 1.0f;         // 토끼 주민 초대 시 증가할 효율

  

    // 캐싱
    private CameraMovement cameraMovement;

    #endregion

    #region 함수

    // 오브젝트 정보 창 보여줌
    public void ShowClickObjWindow()
    {
        if(!clickObjWindow.activeSelf)
        {
            mainPanel.SetActive(false);
            clickObjWindow.SetActive(true);
        }
    }

    // 오브젝트 정보 창 숨김
    public void HideClickObjWindow()
    {
        mainPanel.SetActive(true);
        clickObjWindow.SetActive(false);
    }

    // Store Panel을 보여줌
    public void ShowStorePanel()
    {
        storePanel.SetActive(true);

        cameraMovement.SetCanMove(false);      // 카메라 움직일 수 없게
    }

    // Store Panel을 숨김
    public void HideStorePanel()
    {
        storePanel.SetActive(false);

        cameraMovement.SetCanMove(true);       // 카메라 움직일 수 있게
    }


    // 받을 출석 보상이 있을 때 알려주는 UI
    public void SetNotificationImage()
    {
        // 마지막 출석 보상 수령 날짜가 오늘 날짜와 다르면 받을 출석 보상이 있으므로,
        // UI로 알려줌
        if (getAttendanceRewardDate != DateTime.Now.ToString("yyyy.MM.dd"))
        {
            attendanceNotificationImage.SetActive(true);
        }
    }

    // 출석 보상을 받음
    public void GetAttendanceReward()
    {
        // 마지막 출석 보상 수령날짜를 오늘 날짜로 변경
        getAttendanceRewardDate = DateTime.Now.ToString("yyyy.MM.dd");

        // 더이상 받을 출석 보상이 없을 때 알림 이미지의 Active를 false로
        attendanceNotificationImage.SetActive(false);
    }

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
    }

    void Start()
    {
        Level = 1;
        Gauge = 0;
        SantaCount = 0;
        MyGold = myGold;
        MyCarrots = myCarrots;
        MyDia = myDia;
        

        storePanel.SetActive(false);
        clickObjWindow.SetActive(false);

        cameraMovement = CameraMovement.Instance;

        SetNotificationImage();

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

    //void Update()
    //{
    //    MyGold = myGold;
    //}
}
