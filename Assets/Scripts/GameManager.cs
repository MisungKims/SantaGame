/**
 * @brief �÷��̾��� �������� ���� ����
 * @author ��̼�
 * @date 22-06-04
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

    public string inviteRabbitPrice;

    public string attendanceDate;
    public string initQuestDate;

    public string lastConnectionTime;
}

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

    private Animator gaugeAnim;


    // �÷��̾��� ��
    StringBuilder gaugeSb = new StringBuilder();
    private float gauge;
    public float Gauge
    {
        get{ return gauge; }
        set
        {
            gauge = value;

            UIManagerInstance().gaugeSlider.value = gauge;

            gaugeSb.Clear();
            gaugeSb.Append(gauge.ToString("N0"));
            gaugeSb.Append("%");

            UIManagerInstance().gaugeText.text = gaugeSb.ToString();
        }
    }


    private int level = 1;
    public int Level
    {
        get { return level; }
        set
        {
            level = value;
            UIManagerInstance().lvText.text = level.ToString("D2");
            
        }
    }

    private BigInteger myGold;
    public BigInteger MyGold
    {
        get { return myGold; }
        set
        {
            myGold = value;
            UIManagerInstance().goldText.text = GoldManager.ExpressUnitOfGold(myGold);
        }
    }

    private BigInteger myCarrots;
    public BigInteger MyCarrots
    {
        get { return myCarrots; }
        set
        {
            myCarrots = value;
            UIManagerInstance().carrotsText.text = GoldManager.ExpressUnitOfGold(MyCarrots);
        }
    }

  

    private int myDia;
    public int MyDia
    {
        get { return myDia; }
        set
        {
            myDia = value;
            UIManagerInstance().diaText.text = myDia.ToString();
        }
    }

    private int citizenCount = 0;
    public int CitizenCount
    {
        get { return citizenCount; }
        set
        {
            citizenCount = value;
            UIManagerInstance().citizenCountText.text = citizenCount.ToString();

            if (citizenCount != 0 && citizenCount % 5 == 0)      // �ֹ��� ���� 5�� ����� �� ������ ����
            {
                IncreaseGauge(10);
            }
        }
    }

    StringBuilder daySb = new StringBuilder();
    public int lastDay;
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

            daySb.Clear();
            daySb.Append(year);
            daySb.Append("�� ");
            daySb.Append(month);
            daySb.Append("�� ");
            daySb.Append(day);
            daySb.Append("��");
            UIManagerInstance().dateText.text = daySb.ToString();

            // 12�� 25�Ͽ��� �������޹�ư�� ���̵���
            if (month == 12 && day == 25)
            {
                deliveryButtonObj.SetActive(true);
            }
            else if(deliveryButtonObj.activeSelf)
            {
                deliveryButtonObj.SetActive(false);
            }
        }
    }

    public float dayCount = 600f;        // ���� �� ���� ���ʸ��� ���� ���� �� ��

    public float goldEfficiency = 1.0f;         // �䳢 �ֹ� �ʴ� �� ������ ȿ��

    public string attendanceDate;              // ���������� �⼮ ������ ���� ��¥

    public string initQuestDate;            // ����Ʈ�� �ʱ�ȭ�� ��¥

    public string lastConnectionTime;       // ���������� ������ �ð�

    public string inviteRabbitPrice;        // �䳢 �ֹ� �ʴ� ���


    // ��������
    public int goneMonth = 0;       // �������� �ð����� ������ ��
    public float diffTotalSeconds;  // �������� �ð� (��)


    //���� Ȱ��ȭ ���¸� �����ϴ� ����
    bool isPaused = false;


    // ĳ��
    private WaitForSeconds waitForSeconds;
    private UIManager uIManager;
    private SoundManager soundManager;
    private StoreManager storeManager;

    
    // ���� ���� ����
    [SerializeField]
    private GameObject deliveryButtonObj;

    [SerializeField]
    private GameObject deliveryGame;
    #endregion

    #region ����Ƽ �Լ�
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

        // ������ �ε�
        if (!LoadData())
        {
            InitData();         // �ε� ���н� �ʱⰪ ����
        }

        waitForSeconds = new WaitForSeconds(dayCount);

        gaugeAnim = UIManagerInstance().gaugeBellImage.GetComponent<Animator>();
    }

    void Start()
    {
        StartCoroutine(DateCounting());     // ��¥ ���� ����

        SoundManagerInstance().PlayBGM(EBgmType.main);
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            isPaused = true;

            SaveData();         // ���� ��Ȱ��ȭ�Ǿ��� �� ������ ����
        }

        else
        {
            if (isPaused)
            {
                isPaused = false;
                OfflineTime();
            }
        }
    }

    void OnApplicationQuit()
    {
        SaveData();         // �� ���� �� ������ ����
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
        gaugeAnim.SetBool("isIncrease", true);      // ������ ��� �ִϸ��̼� ����

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

    #region �Լ�
    /// <summary>
    /// ���� ��
    /// </summary>
    private void LevelUp()
    {
        Level++;

        float remain = Gauge - 100.0f;
        Gauge = remain;

        // ���� ������Ʈ�� ������ üũ
        for (int i = 0; i < StoreManagerInstance().storeObjectList.Count; i++)
        {
            StoreManagerInstance().storeObjectList[i].Check();
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
        Gauge += amount;

        if (Gauge >= 100.0f) LevelUp();
    }

    /// <summary>
    /// ���� ���� ���� ����
    /// </summary>
    public void StartDeliveryGame()
    {
        deliveryGame.SetActive(true);
        UIManagerInstance().StartDeliveryGame();
    }

    /// <summary>
    /// ���� ���� ���� ����
    /// </summary>
    public void EndDeliveryGame()
    {
        UIManagerInstance().EndDeliveryGame();
        deliveryGame.SetActive(false);
    }

    /// <summary>
    /// ������ ����
    /// </summary>
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
        data.day = Day;
        data.inviteRabbitPrice = inviteRabbitPrice;
        data.attendanceDate = attendanceDate;
        data.initQuestDate = initQuestDate;
        data.lastConnectionTime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

        File.WriteAllText(Application.persistentDataPath + "/MyData.json", JsonUtility.ToJson(data));
    }
    
    /// <summary>
    /// ������ �ε�
    /// </summary>
    /// <returns></returns>
    public bool LoadData()
    {
        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/MyData.json");

        if (fileInfo.Exists)
        {
            string dataStr = File.ReadAllText(Application.persistentDataPath + "/MyData.json");

            SaveData data = JsonUtility.FromJson<SaveData>(dataStr);
            Level = data.level;
            Gauge = data.gauge;
            MyGold = GoldManager.UnitToBigInteger(data.gold);
            MyCarrots = GoldManager.UnitToBigInteger(data.carrot);
            MyDia = data.dia;
            CitizenCount = data.citizenCount;
            year = data.year;
            month = data.month;
            Day = data.day;
            inviteRabbitPrice = data.inviteRabbitPrice;
            attendanceDate = data.attendanceDate;
            initQuestDate = data.initQuestDate;
            lastConnectionTime = data.lastConnectionTime;

            OfflineTime();

            return true;
        }

        return false;
    }

    /// <summary>
    /// �� �ʱ�ȭ
    /// </summary>
    void InitData()
    {
        Level = 1;
        Gauge = 0;
        CitizenCount = 0;
        Day = 1;
        MyGold = 1000;
        MyCarrots = 1000;
        MyDia = 0;
        year = 0;
        month = 1;
        inviteRabbitPrice = "100";
        lastConnectionTime = "";
    }

    /// <summary>
    /// �������� �ð� ���� ���� ������ ���
    /// </summary>
    public void OfflineTime()
    {
        goneMonth = 0;

        if (lastConnectionTime.Equals("")) return;

        // ���� �� ��Ȱ��ȭ �ð��� ������
        DateTime lastConnection = DateTime.ParseExact(lastConnectionTime, "yyyy-MM-dd-HH-mm-ss", System.Globalization.CultureInfo.InvariantCulture);

        // �������� �ð��� �ʷ� ���
        TimeSpan timeDiff = DateTime.Now - lastConnection;
        diffTotalSeconds = (float)timeDiff.TotalSeconds;

        if (diffTotalSeconds > 21600f)       // �ִ� 360��
        {
            diffTotalSeconds = 21600f;
        }

        int goneDay = (int)(diffTotalSeconds / dayCount);       // ������ ��¥
        int lastMonth = month;
        for (int i = 0; i < goneDay; i++)
        {
            Day++;

            if (lastMonth - month < 0)
            {
                goneMonth++;
            }

            lastMonth = month;
        }
    }

    /// <summary>
    /// UIManager �ν��Ͻ� ��ȯ
    /// </summary>
    /// <returns></returns>
    UIManager UIManagerInstance()
    {
        if (!uIManager)
        {
            uIManager = UIManager.Instance;
        }

        return uIManager;
    }

    /// <summary>
    /// SoundManager �ν��Ͻ� ��ȯ
    /// </summary>
    /// <returns></returns>
    SoundManager SoundManagerInstance()
    {
        if (!soundManager)
        {
            soundManager = SoundManager.Instance;
        }

        return soundManager;
    }

    /// <summary>
    /// UIManager �ν��Ͻ� ��ȯ
    /// </summary>
    /// <returns></returns>
    StoreManager StoreManagerInstance()
    {
        if (!storeManager)
        {
            storeManager = StoreManager.Instance;
        }

        return storeManager;
    }
    #endregion
}
