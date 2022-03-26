using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class StorePanel : MonoBehaviour
{
    #region ����
    public static StorePanel Instance;

    [Header("---------- ���� ������Ʈ")]
    public GameObject storeObject;              // ������ �� ������ �ǹ� ������Ʈ

    [HideInInspector]
    public List<StoreObjectSc> ObjectList = new List<StoreObjectSc>();

    private string prerequisites = null;

    #endregion

    void Awake()    // ���� �Ŵ����� Start���� ���� ����
    {
        Instance = this;
        
        List<Dictionary<string, object>> data = CSVReader.Read("StoreData");       // csv ������ ���� StoreData ���� ��������

        // ������ �������� StoreObject �����ϱ�
        for (int i = 0; i < data.Count; i++)
        {
            StoreInstant(
                 i,
                 data[i]["�̸�"].ToString(),
                 (int)data[i]["��� ���� ����"],
                 (int)data[i]["��"],
                 (float)data[i]["�ǹ� ���� ���"],
                 data[i]["�ǹ� ����"].ToString(),
                 data[i]["��� ������"].ToString(),
                 data[i]["��Ÿ �̸�"].ToString(),
                 (int)data[i]["��Ÿ ���� ���"],
                 data[i]["��Ÿ ����"].ToString(),
                 (int)data[i]["�˹� ȿ�� ����"],
                 data[i]["Desc"].ToString()
                 );
        }
    }

    /// <summary>
    /// ���� ������Ʈ ����
    /// </summary>
    void StoreInstant(int index, string buildingName, int unlockLevel, int second, float multiplyBuildingPrice, string buildingPrice, string incrementGold, string santaName, float multiplySantaPrice, string santaPrice, int efficiency, string desc)
    {
        GameObject instant = GameObject.Instantiate(storeObject, storeObject.transform.position, Quaternion.identity, storeObject.transform.parent);

        // csv������ ������ copiedStoreObject�� �־���
        StoreObjectSc copiedStoreObject = instant.transform.GetComponent<StoreObjectSc>();

        copiedStoreObject.index = index;
        copiedStoreObject.BuildingName = buildingName;                      // �ǹ� �̸�
        copiedStoreObject.UnlockLevel = unlockLevel;                        // ��� ���� ���� ����
        copiedStoreObject.Second = second;                                    // �� �� ���� ������ ������
        copiedStoreObject.multiplyBuildingPrice = multiplyBuildingPrice;       // ���׷��̵� �� �ǹ� ���� ���� ����
        copiedStoreObject.BuildingPrice = buildingPrice;                      // �ǹ� ���� 
        copiedStoreObject.IncrementGold = incrementGold;                    // �÷��̾��� �� ������
        copiedStoreObject.SantaName = santaName;                    // ��Ÿ �̸�
        copiedStoreObject.multiplySantaPrice = multiplySantaPrice;          // ���׷��̵� �� �ǹ� ���� ���� ����
        copiedStoreObject.SantaPrice = santaPrice;                         // �ǹ� ���� 
        copiedStoreObject.SantaEfficiency = efficiency;
        copiedStoreObject.Desc = desc;                              // �ǹ��� ����

        copiedStoreObject.gameObject.SetActive(true);
        copiedStoreObject.gameObject.name = buildingName;

        if (prerequisites == null)
        {
            copiedStoreObject.Prerequisites = prerequisites;
            prerequisites = buildingName;
        }
        else
        {
            copiedStoreObject.Prerequisites = prerequisites;
            prerequisites = buildingName;
        }

        ObjectList.Add(copiedStoreObject);
    }

}
