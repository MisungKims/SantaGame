using System.Collections;
using System.Collections.Generic;
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
    private Text gaugeText;        // 게이지를 나타내는 텍스트
    [SerializeField]
    private Text lvText;               // 레벨을 나타내는 텍스트
    [SerializeField]
    private Text goldText;            // 돈을 나타내는 텍스트
    [SerializeField]
    private Text dateText;             // 날짜를 나타내는 텍스트
    
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

    public double MyGold
    {
        get { return myGold; }
        set
        {
            myGold = value;
            goldText.text = GoldManager.ExpressUnitOfGold(myGold);
        }
    }

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

        cameraMovement = CameraMovement.Instance;
    }
}
