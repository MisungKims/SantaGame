using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : MonoBehaviour
{
    [Header("---------- Parse CSV")]
    public ParseCSV storeParseCSV;

    [Header("---------- 상점 오브젝트")]
    public Text moneyTextStore;                 // 돈을 나타내는 텍스트
    public GameObject storeBuildingObject;      // 복제가 될 상점의 건물 오브젝트

    private List<StoreObjectSc> BuildingList = new List<StoreObjectSc>();


    /// <summary>
    /// 건물 리스트 복제
    /// </summary>
    /// <param name="gameObject">복제될 Object</param>
    /// <param name="buildingName">건물의 이름</param>
    /// <param name="multiply">건물의 증가배율</param>
    /// <param name="cost">건물의 가격</param>
    /// <param name="incrementAmount">플레이어 Money의 증가량</param>
    /// <param name="unlockLevel">잠금 해제 가능 레벨</param>
    void GetStoreInstant(GameObject gameObject, string buildingName, float multiplyCost, int cost, float multiplyMoney, int incrementMoney, int unlockLevel, int second, string desc)
    {
        GameObject instant = GameObject.Instantiate(gameObject, gameObject.transform.position, Quaternion.identity, gameObject.transform.parent);

        // csv파일의 내용을 storeObject에 넣어줌
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
        // 상점의 건물 리스트 생성
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
