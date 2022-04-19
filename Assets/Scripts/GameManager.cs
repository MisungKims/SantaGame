/**
 * @brief �÷��̾��� �������� ���� ����
 * @author ��̼�
 * @date 22-04-18
 */

using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;


public class GameManager : MonoBehaviour
{
    #region ����
    // �̱���
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

    [Header("---------- UI ����")]
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

   
   // public string getDailyQuestRewardDate;              // ���������� ���Ϲ̼� ������ ���� ��¥

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

            dateText.text = String.Format("{0}�� {1}�� {2}��", year, month, day);
        }
    }


    public float goldEfficiency = 1.0f;         // �䳢 �ֹ� �ʴ� �� ������ ȿ��

    [SerializeField]
    private float dayCount = 5f;
    private WaitForSeconds waitForSeconds;

    public int questid;

    #endregion

    /* public void SuccessQuest()
    {
        AchivementManager.Instance.Success(questid);
    }*/



    IEnumerator DateCounting()
    {
        while (true)
        {
            yield return waitForSeconds;

            Day++;
        }
    }

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

        Level = 10;
        Gauge = 0;
        CitizenCount = 0;
        Day = 1;
        MyGold = myGold;
        MyCarrots = myCarrots;
        MyDia = myDia;

        waitForSeconds = new WaitForSeconds(dayCount);
    }

    void Start()
    {
        StartCoroutine(DateCounting());

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
