using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

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

    private Building buildingInstant;
    private Santa santaInstant;

    // 캐싱
    private StorePanel storeInstance;
    private GameManager gameManagerInstance;
    private Transform thisTransform;

    StringBuilder sb = new StringBuilder();
   
    #endregion


    #region 유니티 메소드

    void Start()
    {
        storeInstance = StorePanel.Instance;
        gameManagerInstance = GameManager.Instance;
        thisTransform = this.transform;


        backgroundButton = thisTransform.GetChild(0).GetComponent<Button>();

        buildingNameText = thisTransform.GetChild(1).GetComponent<Text>();
        buildingNameText.text = buildingName;

        unlockText = thisTransform.GetChild(2).GetComponent<Text>();
        sb.Append("Lv.");
        sb.Append(unlockLevel.ToString());
        unlockText.text = sb.ToString();

        unlockImage = thisTransform.GetChild(3).gameObject;


        //descText = this.transform.GetChild(0).GetChild(1).GetComponent<Text>();
        //descText.text = desc;

    }

    void Update()
    {
        SetButtonInteractable();
    }

    #endregion


    // 목록에 있는 버튼 클릭 시
    public void ButtonClick()
    {
        storeInstance.selectedObject = this;       // 선택된 Object를 자신으로 변경
        storeInstance.SelectStoreObject();
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
        if (gameManagerInstance.MyGold >= buildingPrice)           // 플레이어가 가진 돈이 건물의 가격보다 높을 때
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

        gameManagerInstance.MyGold -= buildingPrice;                       // 건물 비용 지불

        gameManagerInstance.DoIncreaseGold(second, incrementGold);        // 정해진 시간마다 돈 증가하기 시작

        NewBuilding();      // 건물 오브젝트 생성
    }

    // 새로운 건물 생성
    void NewBuilding()
    {
        gameManagerInstance.HideStorePanel();          // 상점 창 숨기기

        buildingInstant = buildingGroup.transform.GetChild(index).GetComponent<Building>();
        buildingInstant.InitBuilding(index, buildingName, multiplyBuildingPrice, buildingPrice, multiplyGold, incrementGold);
    }

    /// <summary>
    /// 건물을 업그레이드
    /// </summary>
    void UpgradeBuilding()
    {
        if(buildingInstant)
        {
            buildingInstant.Upgrade();
        }

        storeInstance.BuildingPrice = buildingPrice;
        storeInstance.IncrementGold = incrementGold;
    }


    public void SantaButtonClick()
    {
        if (gameManagerInstance.MyGold >= santaPrice)            // 플레이어가 가진 돈이 건물의 가격보다 높을 때
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

        gameManagerInstance.MyGold -= santaPrice;      // 산타 비용 지불

        CreateNewSanta();      // 산타 오브젝트 생성
    }

    /// <summary>
    /// 새로운 산타 생성
    /// </summary>
    void CreateNewSanta()
    {
        gameManagerInstance.HideStorePanel();          // 상점 창 숨기기

        santaInstant = santaGroup.transform.GetChild(index).GetComponent<Santa>();

        santaInstant.InitSanta(index, santaName, multiplySantaPrice, santaPrice, multiplyAmountObtained, amountObtained, buildingInstant);
    }

    /// <summary>
    /// 산타를 업그레이드
    /// </summary>
    void UpgradeSanta()
    {
        if (santaInstant)
        {
            santaInstant.Upgrade();
        }

        storeInstance.SantaPrice = santaPrice;
        storeInstance.IncrementAmount = amountObtained;
    }


    /// <summary>
    /// 플레이어의 레벨에 따른 버튼의 Interactable 설정
    /// </summary>
    void SetButtonInteractable()
    {
        if (gameManagerInstance.Level >= unlockLevel)        // 플레이어의 레벨이 잠금 해제 가능 레벨보다 크고 가진 돈이 건물의 가격보다 클 때
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
}
