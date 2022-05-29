/**
 * @brief ��Ÿ �ֹ��� ����
 * @author ��̼�
 * @date 22-04-22
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
    public Transform pos;     // �ǹ����� �ֹ��� �� �� �ִ� ��ġ
    public Transform lookAtPos;     // �ٶ�����ϴ� ��ġ
    public bool isUse;          // �ٸ� �ֹ��� �̹� ��� ���ΰ�?
}

[System.Serializable]
public class Citizen
{
    public Material material;
    public Vector3 pos;

    public Citizen(Material material, Vector3 pos)
    {
        this.material = material;
        this.pos = pos;
    }
}

public class CitizenRabbitManager : MonoBehaviour
{

    public List<goal> goalPositions = new List<goal>();       // �ֹ��� �� �� �ִ� �ǹ��� ��ġ ����Ʈ

    public List<Citizen> citizenList = new List<Citizen>();

    public List<RabbitCitizen> rabbitCitizens = new List<RabbitCitizen>();

    // �̱���
    private static CitizenRabbitManager instance;
    public static CitizenRabbitManager Instance
    {
        get { return instance; }
    }

    public Material[] materials;

    public GameObject rabbit;
    public GameObject rabbitGroup;

    public Transform zeroPos;

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

    //���� Ȱ��ȭ ���¸� �����ϴ� ����
    //bool isPaused = false;

    //void OnApplicationPause(bool pause)
    //{
    //    if (pause)
    //    {
    //        isPaused = true;

    //        //SaveData();         // ���� ��Ȱ��ȭ�Ǿ��� �� ������ ����
    //    }

    //    else
    //    {
    //        if (isPaused)
    //        {
    //            isPaused = false;
    //            /* ���� Ȱ��ȭ �Ǿ��� �� ó�� */
    //        }
    //    }
    //}

    void OnApplicationQuit()
    {
        SaveData();         // �� ���� �� ������ ����
    }

    /// <summary>
    /// ������ ����
    /// </summary>
    void SaveData()
    {
        for (int i = 0; i < citizenList.Count; i++)
        {
            citizenList[i].pos = rabbitCitizens[i].transform.position;
        }

        string jdata = JsonUtility.ToJson(new Serialization<Citizen>(citizenList));
        File.WriteAllText(Application.dataPath + "/Resources/CitizenData.json", jdata);
    }

    /// <summary>
    /// ������ �ε�
    /// </summary>
    /// <returns>�ҷ����� ���� ����</returns>
    public bool LoadData()
    {
        FileInfo fileInfo = new FileInfo(Application.dataPath + "/Resources/CitizenData.json");
        if (fileInfo.Exists)
        {
            string jdata = File.ReadAllText(Application.dataPath + "/Resources/CitizenData.json");

            citizenList = JsonUtility.FromJson<Serialization<Citizen>>(jdata).target;
            for (int i = 0; i < citizenList.Count; i++)
            {
                RabbitCitizen rabbitCitizen = GameObject.Instantiate(rabbit, citizenList[i].pos, Quaternion.identity, rabbitGroup.transform).GetComponent<RabbitCitizen>();
                rabbitCitizen.rabbitMat.material = citizenList[i].material;
                rabbitCitizens.Add(rabbitCitizen);
            }

            return true;
        }

        return false;
    }

}
