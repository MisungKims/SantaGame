/**
 * @details ��Ÿ Ȥ�� �ǹ��� Ŭ������ �� ���̴� UI
 * @author ��̼�
 * @date 22-04-20
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

    [Header("----------- ������Ʈ �̹���")]
    [SerializeField]
    private GameObject[] buildingImages;
    [SerializeField]
    private GameObject[] santaImages;

    private GameObject ObjImg;

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

    public Object clickedObj;
    #endregion

    #region �Լ�

    /// <summary>
    /// ������ ������ ������ UI�� Set
    /// </summary>
    public void SetBuildingInfo()
    {
        ObjName = clickedObj.buildingName;
        ObjLevel = clickedObj.buildingLevel;
        ObjPrice = clickedObj.buildingPrice;

        goldSb.Clear();
        goldSb.Append("+ ");
        goldSb.Append(clickedObj.incrementGold);
        ObjAmount = goldSb.ToString();

        ObjImg = buildingImages[building.index].gameObject;
        ObjImg.SetActive(true);
    }

    /// <summary>
    /// ��Ÿ�� ������ ������ UI�� Set
    /// </summary>
    public void SetSantaInfo()
    {
        ObjName = clickedObj.santaName;

        ObjLevel = clickedObj.santaLevel;
        ObjPrice = clickedObj.santaPrice;

        goldSb.Clear();
        goldSb.Append("�˹� ȿ�� ");
        goldSb.Append(clickedObj.santaEfficiency.ToString());
        goldSb.Append("% ����");
        ObjAmount = goldSb.ToString();

        ObjImg = santaImages[santa.index].gameObject;
        ObjImg.SetActive(true);
    }

    /// <summary>
    /// UI ���ΰ�ħ
    /// </summary>
    void Refresh()
    {
        if (building && building.Upgrade())
        {
            SetBuildingInfo();
        }
        else if (santa && santa.Upgrade())
        {
            SetSantaInfo();
        }

        SetButtonInteractable();
    }

    /// <summary>
    /// ���׷��̵� ��ư Ŭ�� �� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void UpgradeButtonClick()
    {
        Refresh();
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
        Refresh();
    }

    private void OnDisable()
    {
        if (building) building = null;
        if (santa) santa = null;

        if (ObjImg)
        {
            ObjImg.SetActive(false);
        }
    }
    #endregion
}
