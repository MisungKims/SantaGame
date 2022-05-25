/**
 * @brief �÷��̾��� �������� ���� ����
 * @author ��̼�
 * @date 22-04-20
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

    //[Header("---------- UI ����")]
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

    public Text text;   // ���߿� ��������


    // �÷��̾��� ��
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

    private BigInteger myGold = 3560000;
    public BigInteger MyGold
    {
        get { return myGold; }
        set
        {
            myGold = value;
            UIManager.Instance.goldText.text = GoldManager.ExpressUnitOfGold(myGold);
        }
    }

    private BigInteger myCarrots = 13000;
    public BigInteger MyCarrots
    {
        get { return myCarrots; }
        set
        {
            myCarrots = value;
            UIManager.Instance.carrotsText.text = GoldManager.ExpressUnitOfGold(MyCarrots);
        }
    }

  

    private int myDia = 0;
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
                UIManager.Instance.dateText.text = String.Format("{0}�� {1}�� {2}��", year, month, day);
            }
        }
    }

    public float dayCount = 600f;        // ���� �� ���� ���ʸ��� ���� ���� �� ��

    public float goldEfficiency = 1.0f;         // �䳢 �ֹ� �ʴ� �� ������ ȿ��

    
    private WaitForSeconds waitForSeconds;      // ĳ��

    public int questid;     // ���߿� ������ �� ��

  
    #endregion

    /* public void SuccessQuest()
    {
        AchivementManager.Instance.Success(questid);
    }*/

    #region �Լ�
    /// <summary>
    /// ���� ��
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
    /// ������ ��� ����
    /// </summary>
    /// <param name="amount">ȹ���� ������</param>
    public void IncreaseGauge(float amount)
    {
        StartCoroutine(IncreaseGaugeCorou(amount));
    }

    /// <summary>
    /// ������ ���(�ִϸ��̼� �������� ����)
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
    #endregion

    #region �ڷ�ƾ
    /// <summary>
    /// ��¥ ����
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
    /// ������ ���
    /// </summary>
    /// <param name="amount">ȹ���� ������</param>
    IEnumerator IncreaseGaugeCorou(float amount)
    {
        /// TODO : ȿ���� ����
         gaugeAnim.SetBool("isIncrease", true);      // SantaVillage �������� ������ ��� �ִϸ��̼� ����

        float goalGuage = gauge + amount;

        while (gauge < goalGuage)
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



    #region ����Ƽ �Լ�

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

        Level = 20;
        Gauge = 10;
        CitizenCount = 0;
        Day = 1;
        MyGold = myGold;
        MyCarrots = myCarrots;
        MyDia = myDia;

        waitForSeconds = new WaitForSeconds(dayCount);

        gaugeAnim = UIManager.Instance.gaugeBellImage.GetComponent<Animator>();
    }

    void Start()
    {
        StartCoroutine(DateCounting());

        StartCoroutine(IncreaseGaugeCorou(20f));

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

}
