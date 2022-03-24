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
    Text secondText;                        // 초 Text
    Text incrementAmountText;               // 골드 증가량 Text
    GameObject infoText;

    GameObject unlockImage;                 // 자물쇠 이미지

    Button backgroundButton;

 
    public int index;

    [Header("---------- 변수")]
    public int unlockLevel;                // 잠금 해제 가능 레벨
    public int second;                     // 몇 초 마다 증가할 것인지
    public string desc;                    // 건물의 설명

    public bool isBuyBuilding = false;
    
   
    public string buildingName;                 // 건물 이름

    public float multiplyBuildingPrice;         // 업그레이드 후 건물 가격 증가 배율

    private string buildingPrice;              // 건물 가격 
    public string BuildingPrice
    {
        get { return buildingPrice; }
        set 
        {
            buildingPrice = value;
            if(storeInstance) storeInstance.BuildingPrice = buildingPrice;
        }
    }

   
    private int buildingLevel = 1;
    public int BuildingLevel
    {
        get { return buildingLevel; }
        set 
        {
            buildingLevel = value;
            if (storeInstance) storeInstance.BuildingLevel = buildingLevel;
        }
    }

    public string santaName;               // 산타 이름
    private bool isBuySanta = false;        // 산타를 구매했는지 안했는지

    public float multiplySantaPrice;       // 업그레이드 후 산타 가격 증가 배율

    public string santaPrice;               // 산타 가격 
    public string SantaPrice
    {
        get { return santaPrice; }
        set
        {
            santaPrice = value;
            if (storeInstance) storeInstance.SantaPrice = santaPrice;
        }
    }

 
    private int santaLevel;
    public int SantaLevel
    {
        get { return santaLevel; }
        set
        {
            santaLevel = value;
            if (storeInstance) storeInstance.SantaLevel = santaLevel;
        }
    }

    private string incrementGold;              // 돈 증가량
    public string IncrementGold
    {
        get { return incrementGold; }
        set
        {
            incrementGold = value;
            if (incrementAmountText) incrementAmountText.text = incrementGold;
            if (storeInstance) storeInstance.IncrementGold = incrementGold;
        }
    }

    public int santaEfficiency;            // 알바 효율


    [Header("---------- 오브젝트")]
    //public GameObject santaObject;        // 복제될 산타 프리팹
    public GameObject buildingGroup;

    private Building buildingInstant;
    public Building BuildingInstant
    {
        get { return buildingInstant; }
    }

    private Santa santaInstant;

    // 캐싱
    private StorePanel storeInstance;
    private GameManager gameManager;
    private Transform thisTransform;

    StringBuilder sb = new StringBuilder();
    StringBuilder secondSb = new StringBuilder();

    #endregion


    #region 유니티 메소드
    void OnEnable()
    {
        if (buildingInstant)
        {
            BuildingPrice = buildingInstant.BuildingPrice;
            IncrementGold = buildingInstant.IncrementGold;
            BuildingLevel = BuildingInstant.Level;
        }

        if (santaInstant)
        {
            SantaPrice = santaInstant.SantaPrice;
            SantaLevel = santaInstant.Level;
        }
    }

    void Start()
    {
        storeInstance = StorePanel.Instance;
        gameManager = GameManager.Instance;
        thisTransform = this.transform;

        InitUI();
    }

    void Update()
    {
        SetButtonInteractable();
    }

    #endregion

    // UI 초기 설정
    void InitUI()
    {
        backgroundButton = thisTransform.GetChild(0).GetComponent<Button>();

        buildingNameText = thisTransform.GetChild(1).GetComponent<Text>();
        buildingNameText.text = buildingName;

        unlockText = thisTransform.GetChild(2).GetComponent<Text>();
        sb.Append("Lv.");
        sb.Append(unlockLevel.ToString());
        sb.Append(" 에 잠금 해제");
        unlockText.text = sb.ToString();

        unlockImage = thisTransform.GetChild(3).gameObject;

        infoText = thisTransform.GetChild(4).gameObject;

        incrementAmountText = infoText.transform.GetChild(0).GetComponent<Text>();
        incrementAmountText.text = incrementGold;

        secondText = infoText.transform.GetChild(1).GetComponent<Text>();
        secondSb.Append(second.ToString());
        secondSb.Append("초");
        secondText.text = secondSb.ToString();
    }

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


    // 건물 사기 버튼 클릭 시 건물 구매 혹은 업그레이드
    public void BuildingButtonClick()
    {
        if (GoldManager.CompareBigintAndUnit(gameManager.MyGold, buildingPrice))           // 플레이어가 가진 돈이 건물의 가격보다 높을 때
        {
            if (!isBuyBuilding)
                BuyNewBuilding();                                   // 사지 않은 건물이면 새로 구매
            else UpgradeBuilding();                                 // 산 건물이면 업그레이드
        }
    }

    
    // 새로운 건물 구매
    void BuyNewBuilding()
    {
       isBuyBuilding = true;

       gameManager.MyGold -= GoldManager.UnitToBigInteger(buildingPrice);        // 건물 비용 지불
    
        NewBuilding();      // 건물 오브젝트 생성
    }

    // 새로운 건물 생성
    void NewBuilding()
    {
        gameManager.HideStorePanel();          // 상점 창 숨기기

        buildingInstant = buildingGroup.transform.GetChild(index).GetComponent<Building>();
        buildingInstant.InitBuilding(index, buildingName, multiplyBuildingPrice, buildingPrice, incrementGold);
    }

    // 건물을 업그레이드
    void UpgradeBuilding()
    {
        buildingInstant.Upgrade();

        BuildingPrice = buildingInstant.BuildingPrice;
        IncrementGold = buildingInstant.IncrementGold;
        BuildingLevel = buildingLevel + 1;
    }

    // 산타 사기 버튼 클릭 시 산타 구매 혹은 업그레이드
    public void SantaButtonClick()
    {
        if (GoldManager.CompareBigintAndUnit(gameManager.MyGold, santaPrice))            // 플레이어가 가진 돈이 건물의 가격보다 높을 때
        {
            if (!isBuySanta) BuyNewSanta();                    // 사지 않은 건물이면 새로 구매
            else UpgradeSanta();                                 // 산 건물이면 업그레이드
        }
    }

    // 새로운 산타 구입
    void BuyNewSanta()
    {
        isBuySanta = true;

        gameManager.MyCarrots -= GoldManager.UnitToBigInteger(santaPrice);      // 산타 비용 지불

        CreateNewSanta();      // 산타 오브젝트 생성
    }

    // 새로운 산타 생성
    void CreateNewSanta()
    {
        gameManager.HideStorePanel();          // 상점 창 숨기기

        santaInstant = buildingGroup.transform.GetChild(index).GetChild(1).GetComponent<Santa>();

        santaInstant.InitSanta(index, santaName, (float)second, multiplySantaPrice, santaPrice, santaEfficiency, buildingInstant);
    }

    // 산타를 업그레이드
    void UpgradeSanta()
    {
        santaInstant.Upgrade();

        SantaPrice = santaInstant.SantaPrice;
        IncrementGold = buildingInstant.IncrementGold;
        SantaLevel = santaLevel + 1;
    }

    // 플레이어의 레벨에 따른 버튼의 Interactable 설정
    void SetButtonInteractable()
    {
        if (gameManager.Level >= unlockLevel)        // 플레이어의 레벨이 잠금 해제 가능 레벨보다 크고 가진 돈이 건물의 가격보다 클 때
        {
            backgroundButton.interactable = true;   //  Interactable을 True로 설정
            infoText.SetActive(true);
            unlockImage.SetActive(false);
            unlockText.gameObject.SetActive(false);
        }
        else
        {
            backgroundButton.interactable = false;
            infoText.SetActive(false);
            unlockImage.SetActive(true);
            unlockText.gameObject.SetActive(true);
        }
    }

   
}
