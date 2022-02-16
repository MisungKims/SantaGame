using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreObjectSc : MonoBehaviour
{
    #region 변수
    Text buidingNameText;
    Text buildingCostText;
    Text incrementMoneyText;
    Text unlockText;
    Text descText;
    GameObject unlockImage;

    public GameManager gameManager;

    [Header("변수")]
    public bool isBuy = false;             // 건물을 산 것인지 아닌지
    public string buildingName;            // 건물 이름
    public float multiplyCost;             // 업그레이드 후 건물 가격 증가 배율
    public int cost;                       // 건물 가격 
    public float multiplyMoney;            // 업그레이드 후 플레이어 돈 증가 배율
    public int incrementMoney;             // 플레이어의 돈 증가량
    public int unlockLevel;                // 잠금 해제 가능 레벨
    public int buildingLevel;              // 해당 건물의 레벨
    public int second;                     // 몇 초 마다 증가할 것인지
    public string desc;                    // 건물의 설명

    Button button;

    #endregion

    /// <summary>
    /// 건물 버튼 클릭 시 잠금 해제 혹은 건물 업그레이드
    /// </summary>
    public void Upgrade()
    {
        if (gameManager.myMoney >= cost)        // 플레이어가 가진 돈이 건물의 가격보다 높을 때
        {
            if (!isBuy) Unlock();               // 사지 않은 건물이면 잠금 해제
            else                                // 산 건물이면 업그레이드
            {
                gameManager.DecreaseMoney(cost);

                cost = (int)(cost * multiplyCost);    // 비용을 배율만큼 증가
                buildingCostText.text = GetCommaText(cost);

                incrementMoney = (int)(incrementMoney * multiplyMoney);     // 코인 증가량을 배율만큼 증가
                incrementMoneyText.text = "코인 증가량 : " + GetCommaText(incrementMoney);

                buildingLevel++;                       // 건물의 레벨 업
            }
        }
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
        if (gameManager.level >= unlockLevel && gameManager.myMoney >= cost)        // 플레이어의 레벨이 잠금 해제 가능 레벨보다 크고 가진 돈이 건물의 가격보다 클 때
            button.interactable = true;                                             //  Interactable을 True로 설정
        else button.interactable = false;
    }


    /// <summary>
    /// 산 건물의 잠금을 해제
    /// </summary>
    void Unlock()
    {
        isBuy = true;

        gameManager.DecreaseMoney(cost);

        gameManager.DoIncreaseMoney(second, incrementMoney);        // 정해진 시간마다 돈 증가하기 시작

        unlockImage.SetActive(false);           // 잠금 이미지를 숨김

        // 텍스트의 Active를 true로
        incrementMoneyText.gameObject.SetActive(true);
        descText.gameObject.SetActive(true);
    }


    void Start()
    {
        button = this.transform.GetChild(0).GetComponent<Button>();

        buidingNameText = this.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        buidingNameText.text = buildingName;

        buildingCostText = this.transform.GetChild(0).GetChild(1).GetComponent<Text>();
        buildingCostText.text = GetCommaText(cost);

        incrementMoneyText = this.transform.GetChild(0).GetChild(2).GetComponent<Text>();
        incrementMoneyText.text = "코인 증가량 : " + GetCommaText(incrementMoney);

        unlockText = this.transform.GetChild(0).GetChild(3).GetComponent<Text>();
        unlockText.text = "Lv." + unlockLevel.ToString();

        descText = this.transform.GetChild(0).GetChild(4).GetComponent<Text>();
        descText.text = desc;

        unlockImage = this.transform.GetChild(0).GetChild(5).gameObject;

        // 사지 않은 건물일 때
        if (!isBuy)
        {
            unlockImage.SetActive(true);       // 잠금 이미지를 보여줌

            // 증가량과 설명 텍스트를 숨김
            incrementMoneyText.gameObject.SetActive(false);
            descText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        SetButtonInteractable();
    }
}
