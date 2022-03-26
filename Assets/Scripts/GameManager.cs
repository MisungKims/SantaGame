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
    #region ����
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

    [Header("---------- ���� ������Ʈ")]
    [SerializeField]
    private Slider gaugeSlider;        // �������� ��Ÿ���� �����̴�
    [SerializeField]
    private Text gaugeText;            // �������� ��Ÿ���� �ؽ�Ʈ
    [SerializeField]
    private Text lvText;               // ������ ��Ÿ���� �ؽ�Ʈ
    [SerializeField]
    private Text goldText;             // ���� ��Ÿ���� �ؽ�Ʈ
    [SerializeField]
    private Text carrotsText;          // ����� ��Ÿ���� �ؽ�Ʈ
    [SerializeField]
    private Text diaText;              // ���̾Ƹ� ��Ÿ���� �ؽ�Ʈ
    [SerializeField]
    private Text santaCountText;              // ���̾Ƹ� ��Ÿ���� �ؽ�Ʈ
    [SerializeField]
    private Text dateText;             // ��¥�� ��Ÿ���� �ؽ�Ʈ

    public Text text;             // ��¥�� ��Ÿ���� �ؽ�Ʈ

    [Header("---------- �г�")]
    public GameObject mainPanel;     // ���� �г�
    public GameObject storePanel;     // ���� �г�
    public GameObject clickObjWindow;     // Ŭ�� ������Ʈ �г�

    [Header("---------- �÷��̾� ��")]
    [SerializeField]
    private float gauge;
    [SerializeField]
    private int level = 1;
    [SerializeField]
    private int santaCount =0;

    private BigInteger myGold = 3560000;
    private BigInteger myCarrots = 13000;
    private BigInteger myDia = 36000;

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

    public BigInteger MyDia
    {
        get { return myDia; }
        set
        {
            myDia = value;
            diaText.text = GoldManager.ExpressUnitOfGold(myDia);
        }
    }
    public int SantaCount
    {
        get { return santaCount; }
        set
        {
            santaCount = value;
            santaCountText.text = santaCount.ToString();
        }
    }


    // ĳ��
    private CameraMovement cameraMovement;

    #endregion

    #region �Լ�

    // ������Ʈ ���� â ������
    public void ShowClickObjWindow()
    {
        if(!clickObjWindow.activeSelf)
        {
            mainPanel.SetActive(false);
            clickObjWindow.SetActive(true);
        }
    }

    // ������Ʈ ���� â ����
    public void HideClickObjWindow()
    {
        mainPanel.SetActive(true);
        clickObjWindow.SetActive(false);
    }

    // Store Panel�� ������
    public void ShowStorePanel()
    {
        storePanel.SetActive(true);

        cameraMovement.SetCanMove(false);      // ī�޶� ������ �� ����
    }

    // Store Panel�� ����
    public void HideStorePanel()
    {
        storePanel.SetActive(false);

        cameraMovement.SetCanMove(true);       // ī�޶� ������ �� �ְ�
    }

    #endregion


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);      // �� ��ȯ �ÿ��� �ı����� ����
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
