using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region 변수
    [Header("Parse CSV")]
    public ParseCSV storeParseCSV;

    [Header("메인 오브젝트")]
    public Slider gaugeSlider;        // 게이지를 나타내는 슬라이더
    public Text lvText;               // 레벨을 나타내는 텍스트
    public Text moneyText;            // 돈을 나타내는 텍스트
    public Text dateText;             // 날짜를 나타내는 텍스트
    public Text timeText;             // 시간을 나타내는 텍스트

    [Header("상점 오브젝트")]
    public GameObject storePanel;     // 상점 패널
    public Text moneyTextStore;            // 돈을 나타내는 텍스트
    public GameObject storeBuildingObject;      // 복제가 될 상점의 건물 오브젝트

    private List<StoreObjectSc> BuildingList = new List<StoreObjectSc>();
 

    [Header("변수")]
    public float gauge;
    public int level = 1;
    public int myMoney = 0;

    #endregion

    #region 함수

    /// <summary>
    /// 1000 단위 마다 콤마를 붙여주는 함수
    /// </summary>
    string GetCommaText(int i)
    {
        return string.Format("{0: #,###; -#,###;0}", i);
    }

    public void LevelUp()
    {
        level++;
        lvText.text = level.ToString();
    }

    public void IncreaseGauge(float amount)
    {
        gauge += amount;
        gaugeSlider.value = gauge;
    }

    public void IncreaseMoney(int amount)
    {
        myMoney += amount;
        moneyText.text = GetCommaText(myMoney);
        moneyTextStore.text = GetCommaText(myMoney);
    }

    public void DecreaseMoney(int amount)
    {
        myMoney -= amount;
        moneyText.text = GetCommaText(myMoney);
        moneyTextStore.text = GetCommaText(myMoney);
    }


    public void DoIncreaseMoney(int second, int incrementMoney)
    {
        StartCoroutine(IncreaseMoney(second, incrementMoney));
    }

    /// <summary>
    /// 정해진 시간(second)마다 돈 증가
    /// </summary>
    /// <returns></returns>
    IEnumerator IncreaseMoney(int second, int incrementMoney)
    {
        while (true)
        {
            yield return new WaitForSeconds(second);

            IncreaseMoney(incrementMoney);
        }
    }


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


    #endregion


    // Start is called before the first frame update
    void Start()
    {
        lvText.text = level.ToString();
        storePanel.SetActive(false);
        
        // 상점의 건물 리스트 생성
        for(int i = 0; i < storeParseCSV.GetCount(); i++)
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

    // Update is called once per frame
    void Update()
    {
        gaugeSlider.value = gauge;
        moneyText.text = GetCommaText(myMoney);
        moneyTextStore.text = GetCommaText(myMoney);
    }
}
