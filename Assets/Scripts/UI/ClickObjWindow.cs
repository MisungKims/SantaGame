/**
 * @details 산타 혹은 건물을 클릭했을 때 보이는 UI
 * @author 김미성
 * @date 22-04-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class ClickObjWindow : MonoBehaviour
{
    [SerializeField]
    private GameObject buildingImages;
    [SerializeField]
    private GameObject santaImages;

    private GameObject ObjImg;

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


    StringBuilder levelSb = new StringBuilder();
    StringBuilder goldSb = new StringBuilder();

    private string objName;
    public string ObjName
    {
        set 
        {
            objName = value;
            NameText.text = objName;
        }
    }

    private int objLevel;
    public int ObjLevel
    {
        set
        {
            objLevel = value;

            levelSb.Clear();
            levelSb.Append("Lv.");
            levelSb.Append(objLevel.ToString());
            LevelText.text = levelSb.ToString();
        }
    }

    private string objAmount;
    public string ObjAmount
    {
        set
        {
            objAmount = value;
            DescText.text = objAmount;
        }
    }

    private string objPrice;
    public string ObjPrice
    {
        set
        {
            objPrice = value;
            PriceText.text = objPrice;
        }
    }

    private Building building;
    public Building Builidng
    {
        set { building = value; }
    }

    private Santa santa;
    public Santa Santa
    {
        set { santa = value; }
    }

    /// <summary>
    /// 빌딩의 정보를 가져와 UI에 Set
    /// </summary>
    public void SetBuildingInfo()
    {
        ObjName = building.BuilidingName;

        ObjLevel = building.Level;
        ObjPrice = building.BuildingPrice;

        goldSb.Clear();
        goldSb.Append("+ ");
        goldSb.Append(building.IncrementGold);
        
        ObjAmount = goldSb.ToString();

        ObjImg = buildingImages.transform.GetChild(building.Index).gameObject;
        ObjImg.SetActive(true);
    }

    /// <summary>
    /// 산타의 정보를 가져와 UI에 Set
    /// </summary>
    public void SetSantaInfo()
    {
        ObjName = santa.SantaName;

        ObjLevel = santa.Level;
        ObjPrice = santa.SantaPrice;

        goldSb.Clear();
        goldSb.Append("알바 효율 ");
        goldSb.Append(santa.SantaEfficiency.ToString());
        goldSb.Append("% 증가");

        ObjAmount = goldSb.ToString();

        ObjImg = santaImages.transform.GetChild(santa.Index).gameObject;
        ObjImg.SetActive(true);
    }

    /// <summary>
    /// 업그레이드 버튼 클릭 시 (인스펙터에서 호출)
    /// </summary>
    public void UpgradeButtonClick()
    {
        if (building)
        {
            building.Upgrade();

            SetBuildingInfo();
        }
        else if (santa)
        {
            santa.Upgrade();

            SetSantaInfo();
        }
    }

    /// <summary>
    /// 업그레이드 버튼의 Interactable 설정
    /// </summary>
    void SetButtonInteractable()
    {
        if (GoldManager.CompareBigintAndUnit(GameManager.Instance.MyGold, objPrice))        //가진 돈이 오브젝트의 가격보다 클 때
            UpgradeButton.interactable = true;
        else UpgradeButton.interactable = false;
    }

   
    void Update()
    {
        SetButtonInteractable();
    }

    //private void OnDisable()
    //{
    //    if (building)
    //    {
    //        ObjImg.SetActive(false);
    //        building = null;
    //    }
    //    else if (santa)
    //    {
    //        ObjImg.SetActive(false);
    //        santa = null;
    //    }
    //}
}
