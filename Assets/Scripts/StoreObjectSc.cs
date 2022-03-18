using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreObjectSc : MonoBehaviour
{
    #region 변수
    Text buildingNameText;                  // 건물의 이름 Text
    Text descText;                          // 설명 Text
    Text unlockText;                        // Unlock 레벨 Text
    GameObject unlockImage;

    Button backgroundButton;

 
    public int index;

    [Header("---------- 변수")]
    public int unlockLevel;                // 잠금 해제 가능 레벨
    public int second;                     // 몇 초 마다 증가할 것인지
    public string desc;                    // 건물의 설명

    public bool isBuyBuilding = false;     // 건물을 산 것인지 아닌지
    public string buildingName;            // 건물 이름

    public float multiplyBuildingPrice;    // 업그레이드 후 건물 가격 증가 배율
    public int buildingPrice;              // 건물 가격 
    public float multiplyGold;             // 업그레이드 후 플레이어 돈 증가 배율
    public int incrementGold;              // 플레이어의 돈 증가량

    public bool isBuySanta = false;        // 산타를 산 것인지 아닌지
    public string santaName;               // 산타 이름

    public float multiplySantaPrice;       // 업그레이드 후 산타 가격 증가 배율
    public int santaPrice;                 // 산타 가격 
    public float multiplyAmountObtained;   // 업그레이드 후 획득량 증가 배율
    public float amountObtained;           // 획득량 증가


    [Header("---------- 오브젝트")]
    public GameObject santaObject;        // 복제될 산타 프리팹
    public GameObject santaGroup;
    public GameObject buildingGroup;
   
    public List<Building> BuildingList = new List<Building>();
    public List<Santa> SantaList = new List<Santa>();

    #endregion

    // 목록에 있는 버튼 클릭 시
    public void ButtonClick()
    {
        StorePanel.Instance.selectedObject = this;       // 선택된 Object를 자신으로 변경
        StorePanel.Instance.SelectStoreObject();
    }

    void Unlock()
    {
        backgroundButton.interactable = true;   //  Interactable을 True로 설정
        unlockImage.SetActive(false);
    }

    /// <summary>
    /// 건물 사기 버튼 클릭 시 잠금 해제 혹은 건물 업그레이드
    /// </summary>
    public void BuildingButtonClick()
    {
        if (GameManager.Instance.MyGold >= buildingPrice)           // 플레이어가 가진 돈이 건물의 가격보다 높을 때
        {
            if (!isBuyBuilding)
                BuyNewBuilding();                                   // 사지 않은 건물이면 잠금 해제
            else UpgradeBuilding();                                 // 산 건물이면 업그레이드
        }
    }

    /// <summary>
    /// 산 건물의 잠금을 해제
    /// </summary>
    void BuyNewBuilding()
    {
       isBuyBuilding = true;

        GameManager.Instance.MyGold -= buildingPrice;                       // 건물 비용 지불

        GameManager.Instance.DoIncreaseGold(second, incrementGold);        // 정해진 시간마다 돈 증가하기 시작

        NewBuilding();      // 건물 오브젝트 생성
    }

    // 새로운 건물 생성
    void NewBuilding()
    {
        GameManager.Instance.HideStorePanel();          // 상점 창 숨기기

        BuildingList[index] = buildingGroup.transform.GetChild(index).GetComponent<Building>();
        BuildingList[index].InitBuilding(index, buildingName, multiplyBuildingPrice, buildingPrice, multiplyGold, incrementGold);
    }

    /// <summary>
    /// 건물을 업그레이드
    /// </summary>
    void UpgradeBuilding()
    {
        if(BuildingList[index])
        {
            BuildingList[index].Upgrade();
        }

        StorePanel.Instance.BuildingPrice = buildingPrice;
        StorePanel.Instance.IncrementGold = incrementGold;
    }


    public void SantaButtonClick()
    {
        if (GameManager.Instance.MyGold >= santaPrice)            // 플레이어가 가진 돈이 건물의 가격보다 높을 때
        {
            if (!isBuySanta) BuyNewSanta();                           // 사지 않은 건물이면 잠금 해제
            else UpgradeSanta();                                 // 산 건물이면 업그레이드
        }
    }

    /// <summary>
    /// 산타의 잠금을 해제
    /// </summary>
    void BuyNewSanta()
    {
        isBuySanta = true;

        GameManager.Instance.MyGold -= santaPrice;      // 산타 비용 지불

        CreateNewSanta();      // 산타 오브젝트 생성
    }

    /// <summary>
    /// 새로운 산타 생성
    /// </summary>
    void CreateNewSanta()
    {
        GameManager.Instance.HideStorePanel();          // 상점 창 숨기기

        
        // 산타 오브젝트 생성
        GameObject instant = GameObject.Instantiate(
            santaObject, 
            BuildingList[index].transform.GetChild(1).position, 
            BuildingList[index].transform.GetChild(1).rotation, 
            santaGroup.transform);

        SantaList[index] = instant.transform.GetComponent<Santa>();

        SantaList[index].InitSanta(index, santaName, multiplySantaPrice, santaPrice, multiplyAmountObtained, amountObtained);

        SantaList[index].building = BuildingList[index];
    }

    /// <summary>
    /// 산타를 업그레이드
    /// </summary>
    void UpgradeSanta()
    {
        if (SantaList[index])
        {
            SantaList[index].Upgrade();
        }

        StorePanel.Instance.SantaPrice = santaPrice;
        StorePanel.Instance.IncrementAmount = amountObtained;
    }


    /// <summary>
    /// 플레이어의 레벨에 따른 버튼의 Interactable 설정
    /// </summary>
    void SetButtonInteractable()
    {
        if (GameManager.Instance.Level >= unlockLevel)        // 플레이어의 레벨이 잠금 해제 가능 레벨보다 크고 가진 돈이 건물의 가격보다 클 때
        {
            backgroundButton.interactable = true;   //  Interactable을 True로 설정
            unlockImage.SetActive(false);
        }
        else
        {
            backgroundButton.interactable = false;
            unlockImage.SetActive(true);
        }

    }


    void Start()
    {
        backgroundButton = this.transform.GetChild(0).GetComponent<Button>();

        buildingNameText = this.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        buildingNameText.text = buildingName;

      
        descText = this.transform.GetChild(0).GetChild(1).GetComponent<Text>();
        descText.text = desc;

        unlockText = this.transform.GetChild(0).GetChild(2).GetComponent<Text>();
        unlockText.text = "Lv." + unlockLevel.ToString();

        unlockImage = this.transform.GetChild(0).GetChild(3).gameObject;

        BuildingList = new List<Building>(new Building[StorePanel.Instance.ObjectList.Count]);
        SantaList = new List<Santa>(new Santa[StorePanel.Instance.ObjectList.Count]);

    }

    void Update()
    {
        SetButtonInteractable();
    }
}
