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

    Text buildingPriceText;                 // 건물의 가격 Text
    Text incrementGoldText;                 
    GameObject unlockBuildingImage;

    Text santaPriceText;
    GameObject unlockSantaImage;

    Button backgroundButton;

    public StorePanel storePanel;

    [Header("---------- 변수")]
    public bool isBuyBuilding = false;             // 건물을 산 것인지 아닌지
    public string buildingName;            // 건물 이름
    public int unlockLevel;                // 잠금 해제 가능 레벨
    public int second;                     // 몇 초 마다 증가할 것인지

    public float multiplyBuildingPrice;    // 업그레이드 후 건물 가격 증가 배율
    public int buildingPrice;              // 건물 가격 
    public float multiplyGold;             // 업그레이드 후 플레이어 돈 증가 배율
    public int incrementGold;              // 플레이어의 돈 증가량

    public bool isBuySanta = false;
    public string santaName;               // 산타 이름
    public float multiplySantaPrice;       // 업그레이드 후 건물 가격 증가 배율
    public int santaPrice;                 // 산타 가격 

    public int buildingLevel;              // 해당 건물의 레벨
    public int santaLevel;

    public string desc;                    // 건물의 설명

    [Header("---------- 오브젝트")]
    public GameObject santaObject;
    Santa santaInstant;

    #endregion

    // 목록에 있는 버튼 클릭 시
    public void ButtonClick()
    {
        storePanel.selectedObject = this;       // 선택된 Object를 자신으로 변경
        storePanel.SelectStoreObject();
    }

    /// <summary>
    /// 건물 버튼 클릭 시 잠금 해제 혹은 건물 업그레이드
    /// </summary>
    public void BuildingButtonClick()
    {
        if (GameManager.Instance.MyGold >= buildingPrice)           // 플레이어가 가진 돈이 건물의 가격보다 높을 때
        {
            if (!isBuyBuilding) 
                UnlockBuilding();                                   // 사지 않은 건물이면 잠금 해제
            else UpgradeBuilding();                                 // 산 건물이면 업그레이드
        }
    }

    /// <summary>
    /// 산 건물의 잠금을 해제
    /// </summary>
    void UnlockBuilding()
    {
       isBuyBuilding = true;

        GameManager.Instance.MyGold -= buildingPrice;

        GameManager.Instance.DoIncreaseGold(second, incrementGold);        // 정해진 시간마다 돈 증가하기 시작

        unlockBuildingImage.SetActive(false);                              // 잠금 이미지를 숨김

       
    }

    /// <summary>
    /// 건물을 업그레이드
    /// </summary>
    void UpgradeBuilding()
    {
        GameManager.Instance.MyGold -= buildingPrice;

        buildingPrice = (int)(buildingPrice * multiplyBuildingPrice);    // 비용을 배율만큼 증가
        buildingPriceText.text = GetCommaText(buildingPrice);

        incrementGold = (int)(incrementGold * multiplyGold);            // 코인 증가량을 배율만큼 증가
        incrementGoldText.text = "코인 증가량 : " + GetCommaText(incrementGold);

        buildingLevel++;                                                // 건물의 레벨 업
    }


    public void SantaButtonClick()
    {
        if (GameManager.Instance.MyGold >= santaPrice)            // 플레이어가 가진 돈이 건물의 가격보다 높을 때
        {
            if (!isBuySanta) UnlockSanta();                           // 사지 않은 건물이면 잠금 해제
            else UpgradeSanta();                                 // 산 건물이면 업그레이드
        }
    }

    /// <summary>
    /// 산타의 잠금을 해제
    /// </summary>
    void UnlockSanta()
    {
        isBuySanta = true;

        GameManager.Instance.MyGold -= santaPrice;

        unlockSantaImage.SetActive(false);           // 잠금 이미지를 숨김

        GetNewSanta();      // 산타 오브젝트 생성
    }

    /// <summary>
    /// 새로운 산타 생성
    /// </summary>
    void GetNewSanta()
    {
        GameManager.Instance.HideStorePanel();

        // 산타 오브젝트 생성
        GameObject instant = GameObject.Instantiate(santaObject, santaObject.transform.position, santaObject.transform.rotation, santaObject.transform.parent);
        santaInstant = instant.transform.GetComponent<Santa>();

        santaInstant.InitSanta(santaName);
    }

    /// <summary>
    /// 산타를 업그레이드
    /// </summary>
    void UpgradeSanta()
    {
        GameManager.Instance.MyGold -= santaPrice;

        santaPrice = (int)(santaPrice * multiplySantaPrice);    // 비용을 배율만큼 증가
        santaPriceText.text = GetCommaText(santaPrice);

        santaLevel++;                       // 산타의 레벨 업

        if (santaInstant) santaInstant.Level++;
    }

    /// <summary>
    /// 1000 단위 마다 콤마를 붙여주는 함수
    /// </summary>
    string GetCommaText(int i)
    {
        return string.Format("{0: #,###; -#,###;0}", i);
    }

    /// <summary>
    /// 플레이어의 레벨에 따른 버튼의 Interactable 설정
    /// </summary>
    void SetButtonInteractable()
    {
       
        if (GameManager.Instance.Level >= unlockLevel)        // 플레이어의 레벨이 잠금 해제 가능 레벨보다 크고 가진 돈이 건물의 가격보다 클 때
        {
            backgroundButton.interactable = true;   //  Interactable을 True로 설정
            unlockBuildingImage.SetActive(false);
        }
        else
        {
            backgroundButton.interactable = false;
            unlockBuildingImage.SetActive(true);
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

        unlockBuildingImage = this.transform.GetChild(0).GetChild(3).gameObject;

    }

    void Update()
    {
        SetButtonInteractable();
    }
}
