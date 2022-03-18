using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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
    public Slider gaugeSlider;        // �������� ��Ÿ���� �����̴�
    public Text lvText;               // ������ ��Ÿ���� �ؽ�Ʈ
    public Text goldText;            // ���� ��Ÿ���� �ؽ�Ʈ
    public Text dateText;             // ��¥�� ��Ÿ���� �ؽ�Ʈ
    
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

    #region �Լ�

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

    // Store Panel�� �����ֱ�
    public void ShowStorePanel()
    {
        storePanel.SetActive(true);                 

        CameraMovement.Instance.SetCanMove(false);      // ī�޶� ������ �� ����
    }

    // Store Panel�� �����
    public void HideStorePanel()
    {
        storePanel.SetActive(false);

        CameraMovement.Instance.SetCanMove(true);       // ī�޶� ������ �� �ְ�
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
