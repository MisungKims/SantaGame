/**
 * @details 오브젝트 구입 및 업그레이드
 * @author 김미성
 * @date 22-04-20
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
    Text santaPriceText;                  // 건물의 이름 Text
    [SerializeField]
    Text efficiencyText;                  // 건물의 이름 Text
    [SerializeField]
    GameObject santaImageGroup;                  // 건물의 이름 Text

    public Object storeObject;
    public int index;
 
    public string Desc                  // 건물의 설명
    {
        set { descText.text = value; }
    }

    public int UnlockLevel          // 잠금 해제 가능 레벨
    {
        get { return storeObject.unlockLevel; }
        set
        {
            sb.Clear();
            sb.Append("Lv.");
            sb.Append(value.ToString());
            sb.Append(" 에 잠금 해제 가능");
            lockingLevelText.text = sb.ToString();
        }
    }

    public int Second                    // 몇 초 마다 증가할 것인지
    {
        set{  secondText.text = $"{value}초"; }
    }

    public string IncrementGold          // 골드 증가량
    {
        set
        {
            incrementAmountText.text = value;
            incrementGoldText.text = $"+ {GoldManager.MultiplyUnit(value, 0.1f)}";
            
        }
    }

    public string BuildingName
    {
        set
        {
            buildingNameText.text = value;
            objectNameText.text = value;
        }
    }

    public string BuildingPrice
    {
        get { return storeObject.buildingPrice; }
        set {  buildingPriceText.text = value; }
    }

    public int BuildingLevel
    {
        set { buildingLevelText.text = $"LV. {value}"; }
    }

    public string SantaName
    {
        set { santaNameText.text = value; }
    }

    public string SantaPrice
    {
        get { return storeObject.santaPrice; }
        set { santaPriceText.text = value.ToString();  }
    }

    public int SantaLevel
    {
        set { santaLevelText.text = $"Lv. {value}"; }
    }

    public int SantaEfficiency
    {
        set { efficiencyText.text = $"{value}%"; }
    }

    private string prerequisites;
    public string Prerequisites
    {
        set
        {
            prerequisites = value;
            PrerequisitesText.text = $"! {prerequisites} 필요";
        }
    }

    public bool isBuyBuilding = false;       // 건물을 구매했는지 안했는지 (잠금해제를 했는지)
  
    private bool isBuySanta = false;        // 산타를 구매했는지 안했는지

    private bool isGetPrerequisites = false;    // 선행 조건 건물이 잠금해제 되었는지


    [Header("---------- 오브젝트")]
    private GameObject prerequisitesGb;

    public GameObject buildingGroup;

    private Building buildingInstance;

    private Santa santaInstance;

    // 캐싱
    private GameManager gameManager;

    StringBuilder sb = new StringBuilder();
    #endregion

    #region 유니티 메소드

    private void Awake()
    {
        gameManager = GameManager.Instance;
        prerequisitesGb = PrerequisitesText.gameObject;

        RefreshAll();

        if (index == 0)
            isGetPrerequisites = true;

        if (!isBuyBuilding)      // 건물을 사지 않았을 때 (잠금해제 안했을 때)
        {
            lockingImage.SetActive(true);
            unlockingObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        RefreshBuildingInfo();
        RefreshSantaInfo();
    }

    void Start()
    {
        Check();
    }
    #endregion

    #region 함수
    /// <summary>
    /// 모든 UI 새로고침
    /// </summary>
    void RefreshAll()
    {
        Desc = storeObject.desc;
        BuildingName = storeObject.buildingName;
        SantaName = storeObject.santaName;
        UnlockLevel = storeObject.unlockLevel;
        Second = storeObject.second;
        Prerequisites = storeObject.prerequisites;

        RefreshBuildingInfo();
        RefreshSantaInfo();
    }

    /// <summary>
    /// 건물의 UI 새로 고침
    /// </summary>
    void RefreshBuildingInfo()
    {
        BuildingPrice = storeObject.buildingPrice;
        IncrementGold = storeObject.incrementGold;
        BuildingLevel = storeObject.buildingLevel;
    }

    /// <summary>
    /// 산타의 UI 새로 고침
    /// </summary>
    void RefreshSantaInfo()
    {
        SantaPrice = storeObject.santaPrice;
        SantaEfficiency = storeObject.santaEfficiency;
        SantaLevel = storeObject.santaLevel;
    }

    /// <summary>
    /// 레벨 업 했을 때 혹은 선행조건 건물이 잠금해제 됐을 때 체크하여 버튼의 active 설정
    /// </summary>
    public void Check()
    {
        if (!isBuyBuilding)                                         // 건물을 사지 않았고 (잠금해제 안했을 때)
        {
            if (gameManager.Level < UnlockLevel)                    // 잠금해제 불가능한 레벨일 때
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

    #region 건물
    /// <summary>
    /// 건물의 잠금 해제 (인스펙터에서 호출)
    /// </summary>
    public void Unlock()
    {
        if (gameManager.Level < UnlockLevel)   // 잠금 해제 불가능하다면 return
        {
            return;
        }

        lockingImage.SetActive(false);
        unlockingObject.SetActive(true);

        BuyNewBuilding();
    }

    /// <summary>
    /// 새로운 건물 생성
    /// </summary>
    void BuyNewBuilding()
    {
        isBuyBuilding = true;

        buildingInstance = ObjectManager.Instance.buildingList[index].GetComponent<Building>();
        buildingInstance.NewBuilding();

        StoreObject nextObject = StoreManager.Instance.storeObjectList[index + 1];
        if (index != StoreManager.Instance.storeObjectList.Count - 1)
        {
            nextObject.isGetPrerequisites = true;      // 다음 건물의 선행조건을 만족시킴
            nextObject.Check();
        }
    }

    /// <summary>
    /// 건물 업그레이드 버튼 클릭 (인스펙터에서 호출)
    /// </summary>
    public void BuildingButtonClick()
    {
        if (buildingInstance.Upgrade())
        {
            RefreshBuildingInfo();
        }
    }
    #endregion

    #region 산타
    /// <summary>
    /// 산타 사기 버튼 클릭 시 산타 구매 혹은 업그레이드 (인스펙터에서 호출)
    /// </summary>
    public void SantaButtonClick()
    {
        if (GoldManager.CompareBigintAndUnit(gameManager.MyGold, SantaPrice))     // 플레이어가 가진 돈으로 업그레이드가 가능할 때
        {
            if (!isBuySanta) BuyNewSanta();                    // 사지 않은 산타면 새로 구매
            else UpgradeSanta();                               // 산 건물이면 업그레이드
        }
    }

    /// <summary>
    /// 새로운 산타 구입
    /// </summary>
    void BuyNewSanta()
    {
        isBuySanta = true;

        gameManager.MyCarrots -= GoldManager.UnitToBigInteger(SantaPrice);      // 산타 비용 지불

        santaInstance = ObjectManager.Instance.santaList[index].GetComponent<Santa>();
        santaInstance.NewSanta();
    }

    /// <summary>
    /// 산타를 업그레이드
    /// </summary>
    void UpgradeSanta()
    {
        if (santaInstance.Upgrade())
        {
            RefreshSantaInfo();
        }
    }
    #endregion
    #endregion
}
