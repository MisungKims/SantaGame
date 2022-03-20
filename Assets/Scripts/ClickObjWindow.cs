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

    private int objPrice;
    public int ObjPrice
    {
        set
        {
            objPrice = value;
            PriceText.text = GoldManager.ExpressUnitOfGold(objPrice);
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

    public void SetBuildingInfo()
    {
        ObjName = building.BuilidingName;

        ObjLevel = building.Level;
        ObjPrice = building.BuildingPrice;

        goldSb.Clear();
        goldSb.Append("+ ");
        goldSb.Append(GoldManager.ExpressUnitOfGold(building.IncrementGold));
        
        ObjAmount = goldSb.ToString();

        ObjImg = buildingImages.transform.GetChild(building.Index).gameObject;
        ObjImg.SetActive(true);
    }

    public void SetSantaInfo()
    {
        ObjName = santa.SantaName;

        ObjLevel = santa.Level;
        ObjPrice = santa.SantaPrice;

        goldSb.Clear();
        goldSb.Append("∞ÒµÂ »πµÊ∑Æ ");
        goldSb.Append(santa.AmountObtained.ToString());
        goldSb.Append("% ¡ı∞°");

        ObjAmount = goldSb.ToString();

        ObjImg = santaImages.transform.GetChild(santa.Index).gameObject;
        ObjImg.SetActive(true);
    }


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

    private void OnDisable()
    {
        if (building)
        {
            ObjImg.SetActive(false);
            building = null;
        }
        else if (santa)
        {
            ObjImg.SetActive(false);
            santa = null;
        }
    }
}
