/**
 * @details 산타를 클릭했을 때 보이는 UI
 * @author 김미성
 * @date 22-04-27
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class ClickObjWindow : MonoBehaviour
{
    #region 변수
    // UI 변수
    [SerializeField]
    private Text NameText;
    [SerializeField]
    private Text LevelText;
    [SerializeField]
    private Text DescText;
    [SerializeField]
    private Text PriceText;
    [SerializeField]
    private Button UpgradeButton;
    //[SerializeField]
    //private Image buildingImg;
    [SerializeField]
    private Image objImg;


    // 스트링 빌더
    StringBuilder levelSb = new StringBuilder();
    StringBuilder goldSb = new StringBuilder();

    // 프로퍼티 
    public string ObjName
    {
        set
        {
            NameText.text = value;
        }
    }

    public int ObjLevel
    {
        set
        {
            levelSb.Clear();
            levelSb.Append("Lv.");
            levelSb.Append(value.ToString());
            LevelText.text = levelSb.ToString();
        }
    }

    public string ObjAmount
    {
        set
        {
            DescText.text = value;
        }
    }

    public string ObjPrice
    {
        get { return PriceText.text; }
        set
        {
            PriceText.text = value;
        }
    }

    //private Building building;
    //public Building Builidng
    //{
    //    set { building = value; }
    //}

    private Santa santa;
    public Santa Santa
    {
        set { santa = value; }
    }

    //public Object clickedObj;
    #endregion

    #region 함수

    ///// <summary>
    ///// 빌딩의 정보를 가져와 UI에 Set
    ///// </summary>
    //public void SetBuildingInfo()
    //{
    //    ObjName = clickedObj.buildingName;
    //    ObjLevel = clickedObj.buildingLevel;
    //    ObjPrice = clickedObj.buildingPrice;

    //    goldSb.Clear();
    //    goldSb.Append("+ ");
    //    goldSb.Append(clickedObj.incrementGold);
    //    ObjAmount = goldSb.ToString();

    //    buildingImg.sprite = clickedObj.buildingSprite;
    //    buildingImg.gameObject.SetActive(true);
    //}

    /// <summary>
    /// 산타의 정보를 가져와 UI에 Set
    /// </summary>
    public void SetSantaInfo()
    {
        ObjName = santa.SantaName;
        objImg.sprite = santa.SantaSprite;

        ObjLevel = santa.Level;
        ObjPrice = santa.SantaPrice;

        goldSb.Clear();
        goldSb.Append("알바 효율 ");
        goldSb.Append(santa.SantaEfficiency.ToString());
        goldSb.Append("% 증가");
        ObjAmount = goldSb.ToString();
    }

    /// <summary>
    /// 업그레이드 버튼 클릭 시 (인스펙터에서 호출)
    /// </summary>
    public void UpgradeButtonClick()
    {
        Refresh();
    }

    /// <summary>
    /// UI 새로고침
    /// </summary>
    void Refresh()
    {
        //if (building && building.Upgrade())     // 빌딩 업그레이드
        //{
        //    SetBuildingInfo();
        //}
        if (santa.Upgrade())      // 산타 업그레이드
        {
            ObjLevel = santa.Level;
            ObjPrice = santa.SantaPrice;

            goldSb.Clear();
            goldSb.Append("알바 효율 ");
            goldSb.Append(santa.SantaEfficiency.ToString());
            goldSb.Append("% 증가");
            ObjAmount = goldSb.ToString();
        }

        SetButtonInteractable();
    }

  
    /// <summary>
    /// 업그레이드 버튼의 Interactable 설정
    /// </summary>
    void SetButtonInteractable()
    {
        if (GoldManager.CompareBigintAndUnit(GameManager.Instance.MyGold, ObjPrice))        //가진 돈이 오브젝트의 가격보다 클 때
            UpgradeButton.interactable = true;
        else UpgradeButton.interactable = false;
    }


    #endregion

    #region 유니티 함수
    private void OnEnable()
    {
        SetSantaInfo();
    }

    //private void OnDisable()
    //{
    //    if (building)
    //    {
    //        building = null;
    //        buildingImg.gameObject.SetActive(false);
    //    }
    //    if (santa)
    //    {
    //        santa = null;
    //        santaImg.gameObject.SetActive(false);
    //    }
    //}
    #endregion
}
