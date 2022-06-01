/**
 * @details CSV ������ �Ľ��Ͽ� ������Ʈ(�ǹ�, ��Ÿ) ����Ʈ ����
 * @author ��̼�
 * @date 22-04-26
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Numerics;
using Random = UnityEngine.Random;

public class ObjectManager : MonoBehaviour
{
    #region ����

    public int unlockCount = 1;     // ��������� �ǹ��� ��
    
    // ����Ʈ
    public List<Building> buildingList = new List<Building>();
    
    public List<Santa> santaList = new List<Santa>();
    
    public List<Object> objectList = new List<Object>();    // ������Ʈ�� ��� ������ ���� ����Ʈ

    public Sprite[] buildingSprites;
    public Sprite[] santaSprites;


    // �̱���
    private static ObjectManager instance;
    public static ObjectManager Instance
    {
        get { return instance; }
    }
    #endregion


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
            ReadCSV();
        }
        

        SetBuilidng();

        SetSanta();
    }

    private void Start()
    {
        OfflineTime();
    }

    /// <summary>
    /// csv ������ ���� StoreData ���� ��������
    /// </summary>
    void ReadCSV()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("StoreData");

        for (int i = 0; i < data.Count; i++)         // ������ �������� StoreObject ����
        {
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

           
        }
    }

    /// <summary>
    /// �������� �ð� ���� ���� ������ ���
    /// </summary>
    public void OfflineTime()
    {
        if (GameManager.Instance.lastConnectionTime.Equals(""))
        {
            return;
        }

        DateTime lastConnectionTime = DateTime.ParseExact(GameManager.Instance.lastConnectionTime, "yyyy-MM-dd-HH-mm-ss",
                    System.Globalization.CultureInfo.InvariantCulture); // DateTime ���� ��ȯ

        TimeSpan timeDiff = DateTime.Now - lastConnectionTime;
        int diffSec = timeDiff.Seconds;
        double diffTotalSeconds = timeDiff.TotalSeconds;

        //Debug.Log(DateTime.Now);
        //Debug.Log(lastConnectionTime);
        //Debug.Log(diffTotalSeconds);

        if (diffTotalSeconds > 21600)       // �ִ� 360��
        {
            diffTotalSeconds = 21600;
        }

        if (diffTotalSeconds < 60)
        {
            UIManager.Instance.getOfflineGoldWindow.timeText.text = ((int)diffTotalSeconds).ToString() + " sec";
        }
        else
        {
            UIManager.Instance.getOfflineGoldWindow.timeText.text = (diffTotalSeconds / 60).ToString("F0") + " min";
        }


        // ��� ȹ��
        BigInteger goldAmount = 0;
        for (int i = 0; i < objectList.Count; i++)
        {
            if (objectList[i].santaLevel > 0)       // ��� ���� �ڵ�ȭ�� �ǹ��̶��
            {
                int count = (int)((float)diffTotalSeconds / objectList[i].second);      // �������� �ð����� ��� ��带 ȹ���ߴ��� ���
                string multiple = GoldManager.MultiplyUnit(objectList[i].incrementGold, count);

                goldAmount += GoldManager.UnitToBigInteger(multiple);
            }
        }

        GameManager.Instance.MyGold += goldAmount;
        UIManager.Instance.getOfflineGoldWindow.goldText.text = GoldManager.ExpressUnitOfGold(goldAmount);

        // ��� ȹ��
        BigInteger carrotAmount = 0;
        carrotAmount = 0;
        for (int i = 0; i < CitizenRabbitManager.Instance.rabbitCitizens.Count; i++)            // ������ �䳢 �ֹ� �� ��ŭ
        {
            int count = (int)((float)diffTotalSeconds / Random.Range(50.0f, 70.0f));      // �������� �ð����� ��� ����� ȹ���ߴ��� ���
            string multiple = GoldManager.MultiplyUnit("100.0A", count);

            carrotAmount += GoldManager.UnitToBigInteger(multiple);
        }
        GameManager.Instance.MyCarrots += carrotAmount;
        UIManager.Instance.getOfflineGoldWindow.carrotText.text = GoldManager.ExpressUnitOfGold(carrotAmount);

        // ������ ���� �������� return
        if (GoldManager.ExpressUnitOfGold(goldAmount).Equals("0.0 ") && GoldManager.ExpressUnitOfGold(carrotAmount).Equals("0.0 "))
        {
            return;
        }

        UIManager.Instance.getOfflineGoldWindow.gameObject.SetActive(true);
        UIManager.Instance.SetisOpenPanel(true);
        CameraMovement.Instance.canMove = false;
    }

    //���� Ȱ��ȭ ���¸� �����ϴ� ����
    bool isPaused = false;

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

                /* ���� Ȱ��ȭ �Ǿ��� �� ó�� */
                OfflineTime();
            }
        }
    }

    void OnApplicationQuit()
    {
        SaveData();         // �� ���� �� ������ ����
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
}
