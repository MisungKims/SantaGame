/**
 * @details CSV ������ �Ľ��Ͽ� ������Ʈ(�ǹ�, ��Ÿ) ����Ʈ ����
 * @author ��̼�
 * @date 22-04-26
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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

            //buildingList[i].buildingObj = objectList[i];
        }
    }
    /// TODO : ���̶�Ű���� �� ���� �Ŀ� �ؿ��� ���� �ø���

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
        File.WriteAllText(Application.dataPath + "/Resources/ObjectData.json", jdata);
    }

    /// <summary>
    /// ������ �ε�
    /// </summary>
    /// <returns>�ҷ����� ���� ����</returns>
    public bool LoadData()
    {
        FileInfo fileInfo = new FileInfo(Application.dataPath + "/Resources/ObjectData.json");
        if (fileInfo.Exists)
        {
            string jdata = File.ReadAllText(Application.dataPath + "/Resources/ObjectData.json");

            objectList = JsonUtility.FromJson<Serialization<Object>>(jdata).target;

            return true;
        }

        return false;
    }
}
