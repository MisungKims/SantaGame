using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorePanel : MonoBehaviour
{
    #region 변수
    public static StorePanel Instance;

    [Header("---------- 상점 오브젝트")]
    public GameObject storeObject;              // 복제가 될 상점의 건물 오브젝트

    public StoreObjectSc selectedObject;

    public Button buyBuildingButton;                 // 건물 사기 버튼
    public Text selectedBuildingName;                // 선택한 상점 오브젝트의 건물 이름
    public Text selectedBuildingPrice;               // 선택한 상점 오브젝트의 건물 가격
    public Text incrementGoldText;                   // 선택한 상점 오브젝트 건물의 골드 증가량

    public Button buySantaButton;                   // 산타 사기 버튼
    public Text selectedSantaName;                  // 선택한 상점 오브젝트의 산타 이름
    public Text selectedSantaPrice;                 // 선택한 상점 오브젝트의 산타 가격
    public Text incrementAmountText;                // 선택한 상점 오브젝트 산타의 획득량 증가량


    private int buildingPrice;
    public int BuildingPrice
    {
        set
        {
            selectedBuildingPrice.text = GoldManager.ExpressUnitOfGold(value);
        }
    }

    private int incrementGold;
    public int IncrementGold
    {
        set
        {
            incrementGoldText.text = "+" + value.ToString();
        }
    }

    private int santaPrice;
    public int SantaPrice
    {   
        set
        {
            selectedSantaPrice.text = GoldManager.ExpressUnitOfGold(value);
        }
    }

    private float incrementAmount;
    public float IncrementAmount
    {
        set
        {
            incrementAmountText.text = value.ToString() + "%";
        }
    }


    public List<StoreObjectSc> BuildingList = new List<StoreObjectSc>();

    #endregion

    void Awake()    // 게임 매니저의 Start보다 먼저 실행
    {
        Instance = this;
        
        List<Dictionary<string, object>> data = CSVReader.Read("StoreData");       // csv 리더를 통해 StoreData 파일 가져오기

        // 가져온 내용으로 StoreObject 복제하기
        for (int i = 0; i < data.Count; i++)
        {
            StoreInstant(
                i,
                data[i]["이름"].ToString(),
                (int)data[i]["잠금 해제 레벨"],
                (int)data[i]["초"],
                (float)data[i]["건물 가격 배수"],
                (int)data[i]["건물 가격"],
                 (float)data[i]["골드 증가 배수"],
                (int)data[i]["골드 증가량"],
                data[i]["산타 이름"].ToString(),
                (float)data[i]["산타 가격 배수"],
                (int)data[i]["산타 가격"],
                data[i]["Desc"].ToString());
        }

        selectedObject = BuildingList[0];
    }

    /// <summary>
    /// 상점 오브젝트 복제
    /// </summary>
    void StoreInstant(int index, string buildingName, int unlockLevel, int second, float multiplyBuildingPrice, int buildingPrice, float multiplyGold, int incrementGold, string santaName, float multiplySantaPrice, int santaPrice, string desc)
    {
        GameObject instant = GameObject.Instantiate(storeObject, storeObject.transform.position, Quaternion.identity, storeObject.transform.parent);

        // csv파일의 내용을 copiedStoreObject에 넣어줌
        StoreObjectSc copiedStoreObject = instant.transform.GetComponent<StoreObjectSc>();

        copiedStoreObject.index = index;
        copiedStoreObject.buildingName = buildingName;                      // 건물 이름
        copiedStoreObject.unlockLevel = unlockLevel;                   // 잠금 해제 가능 레벨
        copiedStoreObject.second = second;                                    // 몇 초 마다 증가할 것인지
        copiedStoreObject.multiplyBuildingPrice = multiplyBuildingPrice;       // 업그레이드 후 건물 가격 증가 배율
        copiedStoreObject.buildingPrice = buildingPrice;                      // 건물 가격 
        copiedStoreObject.multiplyGold = multiplyGold;                // 업그레이드 후 플레이어 돈 증가 배율
        copiedStoreObject.incrementGold = incrementGold;                    // 플레이어의 돈 증가량
        copiedStoreObject.santaName = santaName;                    // 산타 이름
        copiedStoreObject.multiplySantaPrice = multiplySantaPrice;          // 업그레이드 후 건물 가격 증가 배율
        copiedStoreObject.santaPrice = santaPrice;                         // 건물 가격 
        copiedStoreObject.desc = desc;                              // 건물의 설명

        copiedStoreObject.gameObject.SetActive(true);
        copiedStoreObject.gameObject.name = buildingName;

        BuildingList.Add(copiedStoreObject);


    }

    /// <summary>
    /// 버튼의 Interactable 설정
    /// </summary>
    void SetButtonInteractable()
    {
        if (GameManager.Instance.MyGold >= selectedObject.buildingPrice)        // 플레이어의 레벨이 잠금 해제 가능 레벨보다 크고 가진 돈이 건물의 가격보다 클 때
            buyBuildingButton.interactable = true;                                             //  Interactable을 True로 설정
        else buyBuildingButton.interactable = false;

        if (GameManager.Instance.MyGold >= selectedObject.santaPrice)        // 플레이어의 레벨이 잠금 해제 가능 레벨보다 크고 가진 돈이 건물의 가격보다 클 때
            buySantaButton.interactable = true;                                             //  Interactable을 True로 설정
        else buySantaButton.interactable = false;

    }

    private void Start()
    {
        SelectStoreObject();
    }

    /// <summary>
    /// 상점 목록의 버튼을 선택했을 때 선택된 오브젝트의 것으로 이름, 이미지, 가격 등을 설정
    /// </summary>
    public void SelectStoreObject()
    {
        selectedBuildingName.text = selectedObject.buildingName;
        BuildingPrice = selectedObject.buildingPrice;
        IncrementGold = selectedObject.incrementGold;

        selectedSantaName.text = selectedObject.santaName;
        SantaPrice = selectedObject.santaPrice;
        IncrementAmount = selectedObject.amountObtained;
    }

    // 빌딩 업그레이드 버튼 클릭 시
    public void BuildingUpgradeButton()
    {
        selectedObject.BuildingButtonClick();
    }

    // 산타 업그레이드 버튼 클릭 시
    public void SantaUpgradeButton()
    {
        selectedObject.SantaButtonClick();
    }


    void Update()
    {
        SetButtonInteractable();
    }
}
