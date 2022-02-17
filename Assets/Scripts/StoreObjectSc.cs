using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreObjectSc : MonoBehaviour
{
    #region 변수
    Text buidingNameText;
    Text descText;
    Text unlockText;

    Button buildingButton;
    Text buildingPriceText;
    Text incrementGoldText;
    GameObject unlockBuildingImage;

    Button santaButton;
    Text santaNameText;
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
    public int santaPrice;                 // 건물 가격 

    public int buildingLevel;              // 해당 건물의 레벨
    public int santaLevel;

    public string desc;                    // 건물의 설명

  
    #endregion

    public void ButtonClick()
    {
        storePanel.selectedObject = this;
    }

    /// <summary>
    /// 건물 버튼 클릭 시 잠금 해제 혹은 건물 업그레이드
    /// </summary>
    public void BuildingButtonClick()
    {
        if (GameManager.Instance.myGold >= buildingPrice)            // 플레이어가 가진 돈이 건물의 가격보다 높을 때
        {
            if (!isBuyBuilding) UnlockBuilding();                           // 사지 않은 건물이면 잠금 해제
            else UpgradeBuilding();                                 // 산 건물이면 업그레이드
        }
    }

    /// <summary>
    /// 산 건물의 잠금을 해제
    /// </summary>
    void UnlockBuilding()
    {
       isBuyBuilding = true;

        GameManager.Instance.DecreaseGold(buildingPrice);

        GameManager.Instance.DoIncreaseGold(second, incrementGold);        // 정해진 시간마다 돈 증가하기 시작

        unlockBuildingImage.SetActive(false);           // 잠금 이미지를 숨김

        // 텍스트의 Active를 true로
        incrementGoldText.gameObject.SetActive(true);
    }

    /// <summary>
    /// 건물을 업그레이드
    /// </summary>
    void UpgradeBuilding()
    {
        GameManager.Instance.DecreaseGold(buildingPrice);

        buildingPrice = (int)(buildingPrice * multiplyBuildingPrice);    // 비용을 배율만큼 증가
        buildingPriceText.text = GetCommaText(buildingPrice);

        incrementGold = (int)(incrementGold * multiplyGold);     // 코인 증가량을 배율만큼 증가
        incrementGoldText.text = "코인 증가량 : " + GetCommaText(incrementGold);

        buildingLevel++;                       // 건물의 레벨 업
    }


    public void SantaButtonClick()
    {
        if (GameManager.Instance.myGold >= santaPrice)            // 플레이어가 가진 돈이 건물의 가격보다 높을 때
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

        GameManager.Instance.DecreaseGold(santaPrice);

        unlockSantaImage.SetActive(false);           // 잠금 이미지를 숨김

    }

    /// <summary>
    /// 산타를 업그레이드
    /// </summary>
    void UpgradeSanta()
    {
        GameManager.Instance.DecreaseGold(santaPrice);

        santaPrice = (int)(santaPrice * multiplySantaPrice);    // 비용을 배율만큼 증가
        santaPriceText.text = GetCommaText(santaPrice);

        santaLevel++;                       // 건물의 레벨 업
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
        if (GameManager.Instance.level >= unlockLevel && GameManager.Instance.myGold >= buildingPrice)        // 플레이어의 레벨이 잠금 해제 가능 레벨보다 크고 가진 돈이 건물의 가격보다 클 때
            buildingButton.interactable = true;                                             //  Interactable을 True로 설정
        else buildingButton.interactable = false;

        if (GameManager.Instance.level >= unlockLevel && GameManager.Instance.myGold >= santaPrice)        // 플레이어의 레벨이 잠금 해제 가능 레벨보다 크고 가진 돈이 건물의 가격보다 클 때
            santaButton.interactable = true;                                             //  Interactable을 True로 설정
        else santaButton.interactable = false;

        if (GameManager.Instance.level >= unlockLevel)        // 플레이어의 레벨이 잠금 해제 가능 레벨보다 크고 가진 돈이 건물의 가격보다 클 때
            backgroundButton.interactable = true;                                             //  Interactable을 True로 설정
        else backgroundButton.interactable = false;
    }


    void Start()
    {
        backgroundButton = this.transform.GetChild(0).GetComponent<Button>();

        buidingNameText = this.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        buidingNameText.text = buildingName;

        descText = this.transform.GetChild(0).GetChild(1).GetComponent<Text>();
        descText.text = desc;

        unlockText = this.transform.GetChild(0).GetChild(2).GetComponent<Text>();
        unlockText.text = "Lv." + unlockLevel.ToString();

        buildingButton = this.transform.GetChild(0).GetChild(3).GetComponent<Button>();

        buildingPriceText = buildingButton.transform.GetChild(0).GetComponent<Text>();
        buildingPriceText.text = GetCommaText(buildingPrice);

        incrementGoldText = buildingButton.transform.GetChild(1).GetComponent<Text>();
        incrementGoldText.text = "골드 증가량 : " + GetCommaText(incrementGold);

        unlockBuildingImage = buildingButton.transform.GetChild(2).gameObject;

        // 사지 않은 건물일 때
        if (!isBuyBuilding)
        {
            unlockBuildingImage.SetActive(true);       // 잠금 이미지를 보여줌

            // 증가량과 설명 텍스트를 숨김
            incrementGoldText.gameObject.SetActive(false);
        }

        santaButton = this.transform.GetChild(0).GetChild(4).GetComponent<Button>();
        
        santaPriceText = santaButton.transform.GetChild(0).GetComponent<Text>();
        santaPriceText.text = GetCommaText(santaPrice);

        unlockSantaImage = santaButton.transform.GetChild(2).gameObject;

        santaNameText = santaButton.transform.GetChild(3).GetComponent<Text>();
        santaNameText.text = santaName;

        // 사지 않은 산타일 때
        if (!isBuySanta)
        {
            unlockSantaImage.SetActive(true);       // 잠금 이미지를 보여줌
        }
    }

    void Update()
    {
        SetButtonInteractable();
    }
}
