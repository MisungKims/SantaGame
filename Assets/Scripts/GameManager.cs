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
    private Text gaugeText;        // �������� ��Ÿ���� �ؽ�Ʈ
    [SerializeField]
    private Text lvText;               // ������ ��Ÿ���� �ؽ�Ʈ
    [SerializeField]
    private Text goldText;            // ���� ��Ÿ���� �ؽ�Ʈ
    [SerializeField]
    private Text dateText;             // ��¥�� ��Ÿ���� �ؽ�Ʈ
    
    [Header("---------- �г�")]
    public GameObject mainPanel;     // ���� �г�
    public GameObject storePanel;     // ���� �г�
    public GameObject clickObjWindow;     // Ŭ�� ������Ʈ �г�

    [Header("---------- �÷��̾� ��")]
    [SerializeField]
    private float gauge;
    [SerializeField]
    private int level = 1;

    private BigInteger myGold = 1200;

    StringBuilder gaugeSb = new StringBuilder();

    [SerializeField]
    private Text text;             // ��¥�� ��Ÿ���� �ؽ�Ʈ

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

            //goldText.text = BigIntegerManager.GetUnit(myGold);
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
        MyGold = myGold;
        

        storePanel.SetActive(false);
        clickObjWindow.SetActive(false);

        cameraMovement = CameraMovement.Instance;


        //BigInteger[] arr = new BigInteger[15];

        //int santaHouse = 130;
        //BigInteger pri = santaHouse;

        //BigInteger j = (BigInteger)11;

        //for (int i = 0; i < 15; i++)
        //{
        //    pri *= j;
            
        //    arr[i] = pri;
        //}

        //text.text += GoldManager.ExpressUnitOfGold(arr[0] * 3) + "\n";
        //text.text += GoldManager.ExpressUnitOfGold(arr[1] * 5) + "\n";
        //text.text += GoldManager.ExpressUnitOfGold(arr[2] * 7) + "\n";
        //text.text += GoldManager.ExpressUnitOfGold(arr[3] * 9) + "\n";
        //text.text += GoldManager.ExpressUnitOfGold(arr[4] * 11) + "\n";
        //text.text += GoldManager.ExpressUnitOfGold(arr[5] * 13) + "\n";
        //text.text += GoldManager.ExpressUnitOfGold(arr[6] * 15) + "\n";
        //text.text += GoldManager.ExpressUnitOfGold(arr[7] * 17) + "\n";
        //text.text += GoldManager.ExpressUnitOfGold(arr[8] * 19) + "\n";
        //text.text += GoldManager.ExpressUnitOfGold(arr[9] * 21) + "\n";
        //text.text += GoldManager.ExpressUnitOfGold(arr[10] * 23) + "\n";
        //text.text += GoldManager.ExpressUnitOfGold(arr[11] * 25) + "\n";
        //text.text += GoldManager.ExpressUnitOfGold(arr[12] * 27) + "\n";
        //text.text += GoldManager.ExpressUnitOfGold(arr[13] * 29) + "\n";
    }

    //void Update()
    //{
    //    MyGold = myGold;
    //}
}
