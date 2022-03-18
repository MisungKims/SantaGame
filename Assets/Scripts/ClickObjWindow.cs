using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class ClickObjWindow : MonoBehaviour
{
    public GameObject buildingImage;
    public GameObject santaImage;


    public Text NameText;
    public Text LevelText;
    public Text DescText;
    public Button UpgradeButton;
    public Text PriceText;


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

    public Building building;
    public Santa santa;

   
    public void SetBuildingInfo()
    {
        ObjName = building.BuilidingName;

        ObjLevel = building.Level;
        ObjPrice = building.BuildingPrice;

        goldSb.Clear();
        goldSb.Append("+ ");
        goldSb.Append(building.IncrementGold.ToString());

        ObjAmount = goldSb.ToString();

        buildingImage.transform.GetChild(building.Index).gameObject.SetActive(true);
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

        santaImage.transform.GetChild(santa.Index).gameObject.SetActive(true);
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
            buildingImage.transform.GetChild(building.Index).gameObject.SetActive(false);
            building = null;
            
        }
        else if (santa)
        {
            santaImage.transform.GetChild(santa.Index).gameObject.SetActive(false);
            santa = null;
        }
    }
}
