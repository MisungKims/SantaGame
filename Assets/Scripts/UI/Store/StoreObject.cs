/**
 * @details 오브젝트 구입 및 업그레이드
 * @author 김미성
 * @date 22-04-19
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class StoreObject : MonoBehaviour
{
    #region 변수
    // UI 변수
    [Header("---------- UI 변수")]
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
    //[SerializeField]
    //Button upgradeBuildingButton;                  // 건물의 이름 Text
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
    //[SerializeField]
    //Button upgradeSantaButton;                  // 건물의 이름 Text
    [SerializeField]
    Text santaPriceText;                  // 건물의 이름 Text
    [SerializeField]
    Text efficiencyText;                  // 건물의 이름 Text
    [SerializeField]
    GameObject santaImageGroup;                  // 건물의 이름 Text


    Button backgroundButton;

    public Object storeObject;


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

            sb.Clear();
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
            objectNameText.text = buildingName;
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


    private string prerequisites;
    public string Prerequisites
    {
        set
        {
            prerequisites = value;
            PrerequisitesText.text = string.Format("! {0} 필요", prerequisites);
        }
    }


    public bool isBuyBuilding = false;       // 건물을 구매했는지 안했는지 (잠금해제를 했는지)
    public float multiplyBuildingPrice;         // 업그레이드 후 건물 가격 증가 배율

    private bool isBuySanta = false;        // 산타를 구매했는지 안했는지
    public float multiplySantaPrice;       // 업그레이드 후 산타 가격 증가 배율


    private bool isGetPrerequisites = false;


    [Header("---------- 오브젝트")]
    //public GameObject santaObject;        // 복제될 산타 프리팹
    public GameObject buildingGroup;

    private Building buildingInstance;
    public Building BuildingInstant
    {
        get { return buildingInstance; }
    }

    private Santa santaInstance;

    // 캐싱
    private StorePanel storeInstance;
    private GameManager gameManager;

    private GameObject prerequisitesGb;

    StringBuilder sb = new StringBuilder();
    #endregion


    #region 유니티 메소드

    private void Awake()
    {
        // 값 세팅

        BuildingName = storeObject.buildingName;                      // 건물 이름
        UnlockLevel = storeObject.unlockLevel;                        // 잠금 해제 가능 레벨
        Second = storeObject.second;                                    // 몇 초 마다 증가할 것인지
        multiplyBuildingPrice = storeObject.multiplyBuildingPrice;       // 업그레이드 후 건물 가격 증가 배율
        BuildingPrice = storeObject.buildingPrice;                      // 건물 가격 
        IncrementGold = storeObject.incrementGold;                    // 플레이어의 돈 증가량
        SantaName = storeObject.santaName;                    // 산타 이름
        multiplySantaPrice = storeObject.multiplySantaPrice;          // 업그레이드 후 건물 가격 증가 배율
        SantaPrice = storeObject.santaPrice;                         // 건물 가격 
        SantaEfficiency = storeObject.santaEfficiency;
        Desc = storeObject.desc;                              // 건물의 설명
        Prerequisites = storeObject.prerequisites;
    }

    void OnEnable()
    {
        if (buildingInstance)
        {
            BuildingPrice = buildingInstance.BuildingPrice;
            IncrementGold = buildingInstance.IncrementGold;
            BuildingLevel = BuildingInstant.Level;
        }

        if (santaInstance)
        {
            SantaPrice = santaInstance.SantaPrice;
            SantaLevel = santaInstance.Level;
        }

    }

    void Start()
    {
        storeInstance = StorePanel.instance;
        gameManager = GameManager.Instance;
        prerequisitesGb = PrerequisitesText.gameObject;

        if (index == 0)
        {
            isGetPrerequisites = true;
        }

        if (!isBuyBuilding)      // 건물을 사지 않았을 때 (잠금해제 안했을 때)
        {
            lockingImage.SetActive(true);
            unlockingObject.SetActive(false);
        }
    }

    void Update()
    {
        SetObjectActive();
    }
    #endregion

    /// <summary>
    /// 플레이어의 레벨에 따른 버튼의 active 설정
    /// </summary>
    void SetObjectActive()
    {
        if (!isBuyBuilding)                                         // 건물을 사지 않았고 (잠금해제 안했을 때)
        {
            if (gameManager.Level < unlockLevel)                    // 잠금해제 불가능한 레벨일 때
            {
                unlockButton.SetActive(false);                      // 잠금해제 버튼과 선행조건 텍스트를 숨김
                prerequisitesGb.SetActive(false);
            }
            else                                                    // 잠금해제 가능 레벨일 때
            {
                if (isGetPrerequisites)                             // 선행 조건을 만족 했으면
                {
                    unlockButton.SetActive(true);                   // 잠금해제 버튼만 보여줌
                    prerequisitesGb.SetActive(false);
                }
                else                                                 // 선행 조건을 만족하지 않았으면
                {
                    unlockButton.SetActive(false);
                    prerequisitesGb.SetActive(true);                 // 선행조건 텍스트만 보여줌
                }
            }
        }
    }

    /// <summary>
    /// 건물의 잠금 해제 (인스펙터에서 호출)
    /// </summary>
    public void Unlock()
    {
        lockingImage.SetActive(false);
        unlockingObject.SetActive(true);

        StoreManager.instance.storeObjectList[index + 1].isGetPrerequisites = true;      // 다음 건물의 선행조건을 만족시킴

        if (!isBuyBuilding) BuyNewBuilding();
    }

    /// <summary>
    /// 새로운 건물 생성
    /// </summary>
    void BuyNewBuilding()
    {
        isBuyBuilding = true;

        gameManager.MyGold -= GoldManager.UnitToBigInteger(buildingPrice);        // 건물 비용 지불

       // 건물 오브젝트 생성
        buildingInstance = ObjectManager.Instance.buildingList[index].GetComponent<Building>();

        buildingInstance.NewBuilding();
    }

    /// <summary>
    /// 건물 업그레이드 버튼 클릭 (인스펙터에서 호출)
    /// </summary>
    public void BuildingButtonClick()
    {
        if (GoldManager.CompareBigintAndUnit(gameManager.MyGold, buildingPrice))    // 플레이어가 가진 돈으로 업그레이드가 가능할 때
        {
            buildingInstance.Upgrade();

            BuildingPrice = buildingInstance.BuildingPrice;
            IncrementGold = buildingInstance.IncrementGold;
            BuildingLevel = buildingInstance.Level;
        }
    }


    /// <summary>
    /// 산타 사기 버튼 클릭 시 산타 구매 혹은 업그레이드 (인스펙터에서 호출)
    /// </summary>
    public void SantaButtonClick()
    {
        if (GoldManager.CompareBigintAndUnit(gameManager.MyGold, santaPrice))     // 플레이어가 가진 돈으로 업그레이드가 가능할 때
        {
            if (!isBuySanta) BuyNewSanta();                    // 사지 않은 건물이면 새로 구매
            else UpgradeSanta();                                 // 산 건물이면 업그레이드
        }
    }

    /// <summary>
    /// 새로운 산타 구입
    /// </summary>
    void BuyNewSanta()
    {
        isBuySanta = true;

        gameManager.MyCarrots -= GoldManager.UnitToBigInteger(santaPrice);      // 산타 비용 지불

        santaInstance = ObjectManager.Instance.santaList[index].GetComponent<Santa>();
        santaInstance.NewSanta();
    }

    /// <summary>
    /// 산타를 업그레이드
    /// </summary>
    void UpgradeSanta()
    {
        santaInstance.Upgrade();

        SantaPrice = santaInstance.SantaPrice;
        IncrementGold = buildingInstance.IncrementGold;
        SantaLevel = santaInstance.Level;
    }


}
