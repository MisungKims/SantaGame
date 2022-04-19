/**
 * @details CSV ������ �Ľ��Ͽ� ������Ʈ(�ǹ�, ��Ÿ) ����Ʈ ����
 * @author ��̼�
 * @date 22-04-19
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    // �̱���
    private static ObjectManager instance;
    public static ObjectManager Instance
    {
        get { return instance; }
    }

    public List<Building> buildingList = new List<Building>();

    public List<Santa> santaList = new List<Santa>();

    public List<Object> objectList = new List<Object>();    // CSV ���Ͽ��� �Ľ��� ������ ���� ����Ʈ (���� �ʱ� ���� �� ���)


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

        ReadCSV();

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
            if (i == 0)
                prerequisites = null;
            else prerequisites = data[i-1]["�̸�"].ToString();

            Object newObject = new Object(
                data[i]["Desc"].ToString(),
                (int)data[i]["��� ���� ����"],
                (int)data[i]["��"],
                data[i]["�̸�"].ToString(),
                1,
                data[i]["�ǹ� ����"].ToString(),
                (float)data[i]["�ǹ� ���� ���"],
                data[i]["��� ������"].ToString(),
                data[i]["��Ÿ �̸�"].ToString(),
                1,
                data[i]["��Ÿ ����"].ToString(),
                (int)data[i]["��Ÿ ���� ���"],
                (int)data[i]["�˹� ȿ�� ����"],
                prerequisites
                );

            objectList.Add(newObject);
        }
    }

    /// <summary>
    /// �ǹ��� �� �ʱ�ȭ
    /// </summary>
    void SetBuilidng()
    {
        for (int i = 0; i < buildingList.Count; i++)
        {
            buildingList[i].InitBuilding(
                i,
                objectList[i].buildingName,
                objectList[i].multiplyBuildingPrice,
                objectList[i].buildingPrice,
                objectList[i].incrementGold,
                objectList[i].second);
        }
    }

    /// <summary>
    /// ��Ÿ�� �� �ʱ�ȭ
    /// </summary>
    void SetSanta()
    {
        for (int i = 0; i < santaList.Count; i++)
        {
            santaList[i].InitSanta(
                i,
                objectList[i].santaName,
                objectList[i].multiplySantaPrice,
                objectList[i].santaPrice,
                objectList[i].santaEfficiency,
                buildingList[i].GetComponent<Building>()
                );
        }
    }
}
