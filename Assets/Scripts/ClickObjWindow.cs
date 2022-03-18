using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickObjWindow : MonoBehaviour
{
   
    public Image ObjImage;
    public Text NameText;
    public Text LevelText;
    public Text DescText;
    public Button UpgradeButton;
    public Text PriceText;

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
            LevelText.text = "Lv." + objLevel.ToString();
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

    public void Set(string name, int level, int price, string desc)
    {
        ObjName = name;
        ObjLevel = level;
        ObjPrice = price;
        ObjAmount = desc;
    }
}
