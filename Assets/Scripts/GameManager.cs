/**
 * @brief �÷��̾��� �������� ���� ����
 * @author ��̼�
 * @date 22-04-18
 */

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
    private Slider gaugeSlider;
    [SerializeField]
    private Text gaugeText;
    [SerializeField]
    private Text lvText;
    [SerializeField]
    private Text goldText;
    [SerializeField]
    private Text carrotsText;
    [SerializeField]
    private Text diaText;
    [SerializeField]
    private Text citizenCountText;
    [SerializeField]
    private Text dateText;
        
    public Text text;   // ���߿� ��������

    public string getAttendanceRewardDate;              // ���������� �⼮ ������ ���� ��¥
   
    StringBuilder gaugeSb = new StringBuilder();
    private float gauge;
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

    private int level = 1;
    public int Level
    {
        get { return level; }
        set
        {
            level = value;
            lvText.text = string.Format("{0:D2}", level);
        }
    }

    private BigInteger myGold = 3560000;
    public BigInteger MyGold
    {
        get { return myGold; }
        set
        {
            myGold = value;
            goldText.text = GoldManager.ExpressUnitOfGold(myGold);
        }
    }

    private BigInteger myCarrots = 13000;
    public BigInteger MyCarrots
    {
        get { return myCarrots; }
        set
        {
            myCarrots = value;
            carrotsText.text = GoldManager.ExpressUnitOfGold(MyCarrots);
        }
    }

    private int myDia = 0;
    public int MyDia
    {
        get { return myDia; }
        set
        {
            myDia = value;
            diaText.text = myDia.ToString();
        }
    }

    private int citizenCount = 0;
    public int CitizenCount
    {
        get { return citizenCount; }
        set
        {
            citizenCount = value;
            citizenCountText.text = citizenCount.ToString();
        }
    }


    public float goldEfficiency = 1.0f;         // �䳢 �ֹ� �ʴ� �� ������ ȿ��

    public List<QuestObject> dailyQuestList = new List<QuestObject>();

   
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
        CitizenCount = 0;
        MyGold = myGold;
        MyCarrots = myCarrots;
        MyDia = myDia;
     
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

}
