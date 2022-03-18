using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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
    public Slider gaugeSlider;        // 게이지를 나타내는 슬라이더
    public Text lvText;               // 레벨을 나타내는 텍스트
    public Text goldText;            // 돈을 나타내는 텍스트
    public Text dateText;             // 날짜를 나타내는 텍스트
    
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
    private double myGold = 10000;
   
    public float Gauge
    {
        get{ return gauge; }
        set
        {
            gauge = value;
            gaugeSlider.value = gauge;
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

    
    public double MyGold
    {
        get { return myGold; }
        set
        {
            myGold = value;
            goldText.text = GoldManager.ExpressUnitOfGold(myGold);
        }
    }

    #endregion

    #region 함수

    public void DoIncreaseGold(int second, int incrementGold)
    {
        //StartCoroutine(IncreaseGold(second, incrementGold));
    }


    public void ShowClickObjWindow()
    {
        if(!clickObjWindow.activeSelf)
        {
            mainPanel.SetActive(false);
            clickObjWindow.SetActive(true);
        }
    }

    public void HideClickObjWindow()
    {
        mainPanel.SetActive(true);
        clickObjWindow.SetActive(false);
    }

    // Store Panel을 보여주기
    public void ShowStorePanel()
    {
        storePanel.SetActive(true);                 

        CameraMovement.Instance.SetCanMove(false);      // 카메라 움직일 수 없게
    }

    // Store Panel을 숨기기
    public void HideStorePanel()
    {
        storePanel.SetActive(false);

        CameraMovement.Instance.SetCanMove(true);       // 카메라 움직일 수 있게
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

    // Start is called before the first frame update
    void Start()
    {
        Level = 1;
        Gauge = 0;
        MyGold = myGold;
        

        storePanel.SetActive(false);
        clickObjWindow.SetActive(false);
    }

    
}
