/**
 * @brief ��Ÿ �ֹ��� ����
 * @author ��̼�
 * @date 22-06-02
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class goal
{
    public goalObject[] goalObjects;
}

[System.Serializable]
public class goalObject
{
    public Transform pos;           // �ǹ����� �ֹ��� �� �� �ִ� ��ġ
    public Transform lookAtPos;     // �ٶ�����ϴ� ��ġ
    public bool isUse;              // �ٸ� �ֹ��� �̹� ��� ���ΰ�?
}


[System.Serializable]
public class Citizen            // �����Ϸ��� ������
{
    public string name;
    public int materiaIdx;
    public int clothesIdx;
    public Vector3 pos;

    public Citizen(string name, int materiaIdx, int clothesIdx, Vector3 pos)
    {
        this.name = name;
        this.materiaIdx = materiaIdx;
        this.clothesIdx = clothesIdx;
        this.pos = pos;
    }
}

public class CitizenRabbitManager : MonoBehaviour
{
    #region ����
    public List<goal> goalPositions = new List<goal>();       // �ֹ��� �� �� �ִ� �ǹ��� ��ġ ����Ʈ

    public List<Citizen> citizenList = new List<Citizen>();     // �ֹ��� ������ ����ִ� ����Ʈ

    [HideInInspector]
    public List<RabbitCitizen> rabbitCitizens = new List<RabbitCitizen>();
    
    // �̱���
    private static CitizenRabbitManager instance;
    public static CitizenRabbitManager Instance
    {
        get { return instance; }
    }

    public Material[] materials;        // �ֹ� ���͸��� �迭

    public RabbitCitizen rabbit;        // �䳢 �ֹ� ������
    public GameObject rabbitGroup;      // �䳢 �ֹ� ���̶�Ű�� �θ�

    public Transform zeroPos;

    // ������ ����
    bool isPaused = false;      //���� Ȱ��ȭ ����
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

        LoadData();
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
                /* ���� Ȱ��ȭ �Ǿ��� �� ó�� */
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
    /// ������ ����
    /// </summary>
    void SaveData()
    {
        for (int i = 0; i < citizenList.Count; i++)
        {
            // �ֹ��� �� ���� ����
            if (rabbitCitizens[i].isWearing)
            {
                citizenList[i].clothesIdx = (int)rabbitCitizens[i].clothes.flag;
            }
            else
            {
                citizenList[i].clothesIdx = -1;
            }

            // �ֹ��� ��ġ ����
            citizenList[i].pos = rabbitCitizens[i].transform.position;
        }

        string jdata = JsonUtility.ToJson(new Serialization<Citizen>(citizenList));
        File.WriteAllText(Application.persistentDataPath + "/CitizenData.json", jdata);
    }

    /// <summary>
    /// ������ �ε�
    /// </summary>
    /// <returns>�ҷ����� ���� ����</returns>
    public bool LoadData()
    {
        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/CitizenData.json");
        if (fileInfo.Exists)
        {
            GameManager gameManager = GameManager.Instance;

            string jdata = File.ReadAllText(Application.persistentDataPath + "/CitizenData.json");

            citizenList = JsonUtility.FromJson<Serialization<Citizen>>(jdata).target;
            for (int i = 0; i < citizenList.Count; i++)
            {
                // ����� �����͸� �ҷ��� �䳢 �ֹ� ����
                RabbitCitizen rabbitCitizen = RabbitCitizen.Instantiate(rabbit, citizenList[i].pos, Quaternion.identity, rabbitGroup.transform);

                rabbitCitizen.rabbitMat.material = materials[citizenList[i].materiaIdx];
                rabbitCitizen.name = citizenList[i].name;
                if (citizenList[i].clothesIdx > -1)
                {
                    rabbitCitizen.PutOn(ClothesManager.Instance.clothesList[citizenList[i].clothesIdx]);
                    gameManager.goldEfficiency *= 1.5f;
                }
                rabbitCitizens.Add(rabbitCitizen);
            }

            return true;
        }

        return false;
    }
    #endregion
}
