using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : MonoBehaviour
{
    [Header("---------- Parse CSV")]
    public ParseCSV storeParseCSV;

    [Header("---------- ���� ������Ʈ")]
    public Text moneyTextStore;                 // ���� ��Ÿ���� �ؽ�Ʈ
    public GameObject storeBuildingObject;      // ������ �� ������ �ǹ� ������Ʈ

    private List<StoreObjectSc> BuildingList = new List<StoreObjectSc>();


    /// <summary>
    /// �ǹ� ����Ʈ ����
    /// </summary>
    /// <param name="gameObject">������ Object</param>
    /// <param name="buildingName">�ǹ��� �̸�</param>
    /// <param name="multiply">�ǹ��� ��������</param>
    /// <param name="cost">�ǹ��� ����</param>
    /// <param name="incrementAmount">�÷��̾� Money�� ������</param>
    /// <param name="unlockLevel">��� ���� ���� ����</param>
    void GetStoreInstant(GameObject gameObject, string buildingName, float multiplyCost, int cost, float multiplyMoney, int incrementMoney, int unlockLevel, int second, string desc)
    {
        GameObject instant = GameObject.Instantiate(gameObject, gameObject.transform.position, Quaternion.identity, gameObject.transform.parent);

        // csv������ ������ storeObject�� �־���
        StoreObjectSc storeObject = instant.transform.GetComponent<StoreObjectSc>();
        storeObject.buildingName = buildingName;
        storeObject.multiplyCost = multiplyCost;
        storeObject.cost = cost;
        storeObject.multiplyMoney = multiplyMoney;
        storeObject.incrementMoney = incrementMoney;
        storeObject.unlockLevel = unlockLevel;
        storeObject.second = second;
        storeObject.desc = desc;

        storeObject.gameObject.SetActive(true);
        storeObject.gameObject.name = buildingName;

        BuildingList.Add(storeObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        // ������ �ǹ� ����Ʈ ����
        for (int i = 0; i < storeParseCSV.GetCount(); i++)
        {
            GetStoreInstant(storeBuildingObject,
                storeParseCSV.nameList[i],
                storeParseCSV.multiplyCostList[i],
                storeParseCSV.costList[i],
                storeParseCSV.multiplyMoneyList[i],
                storeParseCSV.incrementMoneyList[i],
                storeParseCSV.unlockLevelList[i],
                storeParseCSV.secondList[i],
                storeParseCSV.descList[i]);
        }
    }

    void Update()
    {
        moneyTextStore.text = GameManager.Instance.myMoney.ToString();
    }

}
