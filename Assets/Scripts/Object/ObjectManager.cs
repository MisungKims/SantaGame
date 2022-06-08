/**
 * @details CSV ������ �Ľ��Ͽ� ������Ʈ(�ǹ�, ��Ÿ) ����Ʈ ����
 * @author ��̼�
 * @date 22-06-02
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Numerics;
using Random = UnityEngine.Random;
using System.Text;


public class ObjectManager : MonoBehaviour
{
    #region ����

    //public int unlockCount = 1;     // ��������� �ǹ��� ��
    
    // ����Ʈ
    public List<Building> buildingList = new List<Building>();
    
    public List<Santa> santaList = new List<Santa>();
    
    public List<Object> objectList = new List<Object>();    // ������Ʈ�� ��� ������ ���� ����Ʈ

    public Sprite[] buildingSprites;
    public Sprite[] santaSprites;

    public bool isWatchingAds = false;

    // �̱���
    private static ObjectManager instance;
    public static ObjectManager Instance
    {
        get { return instance; }
    }

    //���� Ȱ��ȭ ���¸� �����ϴ� ����
    bool isPaused = false;
    #endregion

    #region ����Ƽ �Լ�
    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
        }

        if (!LoadData())
        {
            // ������ �ε忡 ���� �� CSV���Ͽ��� �ҷ���
            ReadCSV();
        }

        SetBuilidng();

        SetSanta();
    }

    private void Start()
    {
        OfflineTime();
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

                // ���� ���� �ö��� �ƴ϶�� �������� ����
                if (!isWatchingAds)
                {
                    OfflineTime();
                }
                else
                {
                    isWatchingAds = false;
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        SaveData();         // �� ���� �� ������ ����
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// csv ������ ���� StoreData ���� ��������
    /// </summary>
    void ReadCSV()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("StoreData");

        for (int i = 0; i < data.Count; i++)         // ������ �������� StoreObject ����
        {
            // ���� ���� ����
            string prerequisites;
            bool isGetPrerequisites;
            if (i == 0)
            {
                isGetPrerequisites = true;
                prerequisites = null;
            }
            else
            {
                isGetPrerequisites = false;
                prerequisites = data[i - 1]["�̸�"].ToString();
            }

            Object newObject = new Object(
                data[i]["Desc"].ToString(),
                (int)data[i]["��� ���� ����"],
                (int)data[i]["��"],
                data[i]["�̸�"].ToString(),
                0,
                data[i]["�ǹ� ����"].ToString(),
                (float)data[i]["�ǹ� ���� ���"],
                data[i]["��� ������"].ToString(),
                buildingSprites[i],
                data[i]["��Ÿ �̸�"].ToString(),
                0,
                data[i]["��Ÿ ����"].ToString(),
                (int)data[i]["��Ÿ ���� ���"],
                (int)data[i]["�˹� ȿ�� ����"],
                santaSprites[i],
                prerequisites,
                isGetPrerequisites
                );

            objectList.Add(newObject);

            if (objectList[i].santaSprite == null)
            {
                objectList[i].santaSprite = santaSprites[i];
            }
        }
    }
    /// <summary>
    /// �ǹ��� �� �ʱ�ȭ
    /// </summary>
    void SetBuilidng()
    {
        for (int i = 0; i < buildingList.Count; i++)
        {
            buildingList[i].buildingObj = objectList[i];
            buildingList[i].index = i;

            if (objectList[i].buildingLevel > 0)    // ��������� �ǹ��̶�� ���̰�
            {
                buildingList[i].Init();
            }
            else
            {
                buildingList[i].gameObject.SetActive(false);
            }

        }
    }

    /// <summary>
    /// ��Ÿ�� �� �ʱ�ȭ
    /// </summary>
    void SetSanta()
    {
        for (int i = 0; i < santaList.Count; i++)
        {
            santaList[i].santaObj = objectList[i];
            santaList[i].index = i;

            if (objectList[i].santaLevel > 0)    // ������ ��Ÿ��� ���̰�
            {
                santaList[i].Init();
            }
            else
            {
                santaList[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// �������� �ð� ���� ���� ������ ���
    /// </summary>
    public void OfflineTime()
    {
        GameManager gameManager = GameManager.Instance;
        UIManager uIManager = UIManager.Instance;
        GetOfflineGoldWindow getOfflineGoldWindow = uIManager.getOfflineGoldWindow;

        string sLastTime = gameManager.lastConnectionTime;
        if (sLastTime.Equals("")) return;

        // ȹ���� ������ ���
        BigInteger goldAmount = GetGold(gameManager.diffTotalSeconds);
        string gold = GoldManager.ExpressUnitOfGold(goldAmount);

        BigInteger carrotAmount = GetCarrot(gameManager.diffTotalSeconds);
        string carrot = GoldManager.ExpressUnitOfGold(carrotAmount);

        // ������ ���� �������� return
        if (gold.Equals("0.0 ") && carrot.Equals("0.0 "))        
        {
            return;            
        }


        // ��� ȹ��
        gameManager.MyGold += goldAmount;
        getOfflineGoldWindow.goldText.text = gold;

        // ��� ȹ��
        gameManager.MyCarrots += carrotAmount;
        getOfflineGoldWindow.carrotText.text = carrot;


        // �������� �ð��� Text�� ������
        getOfflineGoldWindow.timeText.text = OfflinesString(gameManager.diffTotalSeconds);


        // �������� ���� ȹ��â Ȱ��ȭ
        getOfflineGoldWindow.gameObject.SetActive(true);
        uIManager.SetisOpenPanel(true);
        CameraMovement.Instance.canMove = false;
    }

    /// <summary>
    /// �������� �ð��� "0 min" ���·� ��ȯ
    /// </summary>
    /// <returns></returns>
    String OfflinesString(float diffTotalSeconds)
    {
        StringBuilder sb = new StringBuilder();
        if (diffTotalSeconds < 60)
        {
            sb = new StringBuilder();
            sb.Append(((int)diffTotalSeconds).ToString());
            sb.Append(" sec");
        }
        else
        {
            sb = new StringBuilder();
            sb.Append((diffTotalSeconds / 60).ToString("F0"));
            sb.Append(" min");
        }

        return sb.ToString();
    }

    /// <summary>
    /// ȹ���� ��带 ���
    /// </summary>
    /// <param name="diffTotalSeconds">�������� �ð�</param>
    /// <returns></returns>
    BigInteger GetGold(float diffTotalSeconds)
    {
        BigInteger amount = 0;

        for (int i = 0; i < objectList.Count; i++)
        {
            if (objectList[i].santaLevel > 0)       // ��� ���� �ڵ�ȭ�� �ǹ��̶��
            {
                int count = (int)(diffTotalSeconds / objectList[i].second);      // �������� �ð����� ��� ��带 ȹ���ߴ��� ���
                string multiple = GoldManager.MultiplyUnit(objectList[i].incrementGold, count);

                amount += GoldManager.UnitToBigInteger(multiple);
            }
        }

        return amount;
    }

    /// <summary>
    /// ȹ���� ����� ���
    /// </summary>
    /// <param name="diffTotalSeconds">�������� �ð�</param>
    /// <returns></returns>
    BigInteger GetCarrot(float diffTotalSeconds)
    {
        BigInteger amount = 0;

        int citizenCount = CitizenRabbitManager.Instance.rabbitCitizens.Count;
        for (int i = 0; i < citizenCount; i++)                  // ������ �䳢 �ֹ� �� ��ŭ
        {
            int count = (int)(diffTotalSeconds / Random.Range(60.0f, 120.0f));      // �������� �ð����� ��� ����� ȹ���ߴ��� ���
            string multiple = GoldManager.MultiplyUnit("100.0A", count);

            amount += GoldManager.UnitToBigInteger(multiple);
        }

        return amount;
    }

    /// <summary>
    /// ������ ����
    /// </summary>
    void SaveData()
    {
        string jdata = JsonUtility.ToJson(new Serialization<Object>(objectList));
        File.WriteAllText(Application.persistentDataPath + "/ObjectData.json", jdata);
    }

    /// <summary>
    /// ������ �ε�
    /// </summary>
    /// <returns>�ҷ����� ���� ����</returns>
    public bool LoadData()
    {
        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/ObjectData.json");
        if (fileInfo.Exists)
        {
            string jdata = File.ReadAllText(Application.persistentDataPath + "/ObjectData.json");

            objectList = JsonUtility.FromJson<Serialization<Object>>(jdata).target;
            for (int i = 0; i < objectList.Count; i++)
            {
                objectList[i].buildingSprite = buildingSprites[i];
                objectList[i].santaSprite = santaSprites[i];
            }
            return true;
        }

        return false;
    }
    #endregion
}
