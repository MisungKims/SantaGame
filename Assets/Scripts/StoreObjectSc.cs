using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class StoreObjectSc : MonoBehaviour
{
    #region 변수
    [Header("---------- 오브젝트")]
    [SerializeField]
    Text descText;                          // 설명 Text
    [SerializeField]
    Text secondText;                        // 초 Text
    [SerializeField]
    Text incrementAmountText;               // 골드 증가량 Text
    [SerializeField]
    GameObject unlockingObject;

    [SerializeField]
    Text objectNameText;               // 골드 증가량 Text
    [SerializeField]
    Text lockingLevelText;                        // Unlock 레벨 Text
    [SerializeField]
    GameObject lockingImage;                 // 자물쇠 이미지
    [SerializeField]
    Text PrerequisitesText;
    [SerializeField]
    GameObject unlockButton;                 // 자물쇠 이미지

    [Header("----------------- 빌딩")]

    [SerializeField]
    Text buildingNameText;                  // 건물의 이름 Text
    [SerializeField]
    Text buildingLevelText;                  // 건물의 이름 Text
    [SerializeField]
    Button upgradeBuildingButton;                  // 건물의 이름 Text
    [SerializeField]
    Text buildingPriceText;                  // 건물의 이름 Text
    [SerializeField]
    Text incrementGoldText;                  // 건물의 이름 Text
    [SerializeField]
    GameObject buildingImageGroup;                  // 건물의 이름 Text

    [Header("----------------- 산타")]

    [SerializeField]
    Text santaNameText;                  // 건물의 이름 Text
    [SerializeField]
    Text santaLevelText;                  // 건물의 이름 Text
    [SerializeField]
    Button upgradeSantaButton;                  // 건물의 이름 Text
    [SerializeField]
    Text santaPriceText;                  // 건물의 이름 Text
    [SerializeField]
    Text efficiencyText;                  // 건물의 이름 Text
    [SerializeField]
    GameObject santaImageGroup;                  // 건물의 이름 Text


    Button backgroundButton;

 
    public int index;

    private string desc;                    // 건물의 설명
    public string Desc
    {
        get { return desc; }
        set
        {
            desc = value;
            descText.text = desc;
        }
    }

    private int unlockLevel;                // 잠금 해제 가능 레벨
    public int UnlockLevel
    {
        get { return unlockLevel; }
        set
        {
            unlockLevel = value;

            sb.Append("Lv.");
            sb.Append(unlockLevel.ToString());
            sb.Append(" 에 잠금 해제 가능");
            lockingLevelText.text = sb.ToString();
        }
    }

    private int second;                     // 몇 초 마다 증가할 것인지
    public int Second
    {
        get { return second; }
        set
        {
            second = value;
            secondText.text = string.Format("{0}초", second);
        }
    }

    private string incrementGold;              // 돈 증가량
    public string IncrementGold
    {
        get { return incrementGold; }
        set
        {
            incrementGold = value;
            incrementAmountText.text = incrementGold;
            incrementGoldText.text = string.Format("+ {0}", GoldManager.MultiplyUnit(incrementGold, 0.1f)); 
        }
    }

    private string buildingName;                 // 건물 이름
    public string BuildingName
    {
        set
        {
            buildingName = value;
            buildingNameText.text = buildingName;
        }
    }

  
    private string buildingPrice;              // 건물 가격 
    public string BuildingPrice
    {
        get { return buildingPrice; }
        set 
        {
            buildingPrice = value;
            buildingPriceText.text = buildingPrice;
        }
    }

    private int buildingLevel = 1;
    public int BuildingLevel
    {
        get { return buildingLevel; }
        set 
        {
            buildingLevel = value;
            buildingLevelText.text = string.Format("Lv. {0}", buildingLevel);
        }
    }

    private string santaName;               // 산타 이름
    public string SantaName
    {
        set
        {
            santaName = value;
            santaNameText.text = santaName;
        }
    }

    private string santaPrice;               // 산타 가격 
    public string SantaPrice
    {
        get { return santaPrice; }
        set
        {
            santaPrice = value;
            santaPriceText.text = santaPrice.ToString();
        }
    }

 
    private int santaLevel;
    public int SantaLevel
    {
        get { return santaLevel; }
        set
        {
            santaLevel = value;
            santaLevelText.text = string.Format("Lv. {0}", santaLevel);
        }
    }

    private int santaEfficiency;            // 알바 효율
    public int SantaEfficiency
    {
        get { return santaEfficiency; }
        set
        {
            santaEfficiency = value;
            efficiencyText.text = string.Format("{0}%", santaEfficiency.ToString());
        }
    }


    public bool isBuyBuilding = false;       // 건물을 구매했는지 안했는지 (잠금해제를 했는지)
    public float multiplyBuildingPrice;         // 업그레이드 후 건물 가격 증가 배율

    private bool isBuySanta = false;        // 산타를 구매했는지 안했는지
    public float multiplySantaPrice;       // 업그레이드 후 산타 가격 증가 배율


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

        objectNameText.text = name;

        //PrerequisitesText.text = string.Format("! {0} 필요", storeInstance.ObjectList[index - 1].name);
    }

    void Update()
    {
        SetObjectActive();

    }

    #endregion


    // 목록에 있는 버튼 클릭 시
    public void ButtonClick()
    {
        storeInstance.selectedObject = this;       // 선택된 Object를 자신으로 변경
        //storeInstance.SelectStoreObject();
    }

    public void Unlock()    // 잠금 해제
    {
        lockingImage.SetActive(false);
        unlockingObject.SetActive(true);

        if (!isBuyBuilding)
            BuyNewBuilding();                                   // 사지 않은 건물이면 새로 구매
    }

    // 새로운 건물 생성
    void BuyNewBuilding()
    {
        isBuyBuilding = true;

        gameManager.MyGold -= GoldManager.UnitToBigInteger(buildingPrice);        // 건물 비용 지불

        NewBuilding();      // 건물 오브젝트 생성
    }

    // 건물 사기 버튼 클릭 시 건물 구매 혹은 업그레이드
    public void BuildingButtonClick()
    {
        if (GoldManager.CompareBigintAndUnit(gameManager.MyGold, buildingPrice))           // 플레이어가 가진 돈이 건물의 가격보다 높을 때
        {
            //if (!isBuyBuilding)
            //    BuyNewBuilding();                                   // 사지 않은 건물이면 새로 구매
            UpgradeBuilding();                                 // 산 건물이면 업그레이드
        }
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

    // 플레이어의 레벨에 따른 오브젝트의 active 설정
    void SetObjectActive()
    {
        if (!isBuyBuilding)      // 건물을 사지 않았을 때 (잠금해제 안했을 때)
        {
            lockingImage.SetActive(true);
            unlockingObject.SetActive(false);

            if (gameManager.Level >= unlockLevel)        // 플레이어의 레벨이 잠금 해제 가능 레벨보다 클 때
            {
                unlockButton.SetActive(true);
                //if (index != 0 && storeInstance.ObjectList[index - 1].isBuyBuilding)      // 선행 조건으로 필요한 건물을 샀을 때
                //{
                //    unlockButton.SetActive(true);
                //    PrerequisitesText.gameObject.SetActive(false);
                //}
                //else
                //{
                //    unlockButton.SetActive(false);
                //    PrerequisitesText.gameObject.SetActive(true);
                //}
            }
            else
            {
                unlockButton.SetActive(false);
            }
        }
    }
}
