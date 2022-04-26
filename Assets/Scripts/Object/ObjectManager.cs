/**
 * @details CSV ������ �Ľ��Ͽ� ������Ʈ(�ǹ�, ��Ÿ) ����Ʈ ����
 * @author ��̼�
 * @date 22-04-26
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                prerequisites
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
            
            //santaList[i].InitSanta(
            //    i,
            //    objectList[i].santaName,
            //    objectList[i].multiplySantaPrice,
            //    objectList[i].santaPrice,
            //    objectList[i].santaEfficiency,
            //    buildingList[i].GetComponent<Building>()
            //    );
        }
    }
}
