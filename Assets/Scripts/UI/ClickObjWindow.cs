/**
 * @details ��Ÿ�� Ŭ������ �� ���̴� UI
 * @author ��̼�
 * @date 22-04-27
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class ClickObjWindow : MonoBehaviour
{
    #region ����
    // UI ����
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


    // ��Ʈ�� ����
    StringBuilder levelSb = new StringBuilder();
    StringBuilder goldSb = new StringBuilder();

    // ������Ƽ 
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

    #region �Լ�

    ///// <summary>
    ///// ������ ������ ������ UI�� Set
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
    /// ��Ÿ�� ������ ������ UI�� Set
    /// </summary>
    public void SetSantaInfo()
    {
        ObjName = santa.SantaName;
        objImg.sprite = santa.SantaSprite;

        ObjLevel = santa.Level;
        ObjPrice = santa.SantaPrice;

        goldSb.Clear();
        goldSb.Append("�˹� ȿ�� ");
        goldSb.Append(santa.SantaEfficiency.ToString());
        goldSb.Append("% ����");
        ObjAmount = goldSb.ToString();
    }

    /// <summary>
    /// ���׷��̵� ��ư Ŭ�� �� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void UpgradeButtonClick()
    {
        Refresh();
    }

    /// <summary>
    /// UI ���ΰ�ħ
    /// </summary>
    void Refresh()
    {
        //if (building && building.Upgrade())     // ���� ���׷��̵�
        //{
        //    SetBuildingInfo();
        //}
        if (santa.Upgrade())      // ��Ÿ ���׷��̵�
        {
            ObjLevel = santa.Level;
            ObjPrice = santa.SantaPrice;

            goldSb.Clear();
            goldSb.Append("�˹� ȿ�� ");
            goldSb.Append(santa.SantaEfficiency.ToString());
            goldSb.Append("% ����");
            ObjAmount = goldSb.ToString();
        }

        SetButtonInteractable();
    }

  
    /// <summary>
    /// ���׷��̵� ��ư�� Interactable ����
    /// </summary>
    void SetButtonInteractable()
    {
        if (GoldManager.CompareBigintAndUnit(GameManager.Instance.MyGold, ObjPrice))        //���� ���� ������Ʈ�� ���ݺ��� Ŭ ��
            UpgradeButton.interactable = true;
        else UpgradeButton.interactable = false;
    }


    #endregion

    #region ����Ƽ �Լ�
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
