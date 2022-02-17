using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreObjectSc : MonoBehaviour
{
    #region ����
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

    [Header("---------- ����")]
    public bool isBuyBuilding = false;             // �ǹ��� �� ������ �ƴ���
    public string buildingName;            // �ǹ� �̸�
    public int unlockLevel;                // ��� ���� ���� ����
    public int second;                     // �� �� ���� ������ ������

    public float multiplyBuildingPrice;    // ���׷��̵� �� �ǹ� ���� ���� ����
    public int buildingPrice;              // �ǹ� ���� 
    public float multiplyGold;             // ���׷��̵� �� �÷��̾� �� ���� ����
    public int incrementGold;              // �÷��̾��� �� ������

    public bool isBuySanta = false;
    public string santaName;               // ��Ÿ �̸�
    public float multiplySantaPrice;       // ���׷��̵� �� �ǹ� ���� ���� ����
    public int santaPrice;                 // �ǹ� ���� 

    public int buildingLevel;              // �ش� �ǹ��� ����
    public int santaLevel;

    public string desc;                    // �ǹ��� ����

  
    #endregion

    public void ButtonClick()
    {
        storePanel.selectedObject = this;
    }

    /// <summary>
    /// �ǹ� ��ư Ŭ�� �� ��� ���� Ȥ�� �ǹ� ���׷��̵�
    /// </summary>
    public void BuildingButtonClick()
    {
        if (GameManager.Instance.myGold >= buildingPrice)            // �÷��̾ ���� ���� �ǹ��� ���ݺ��� ���� ��
        {
            if (!isBuyBuilding) UnlockBuilding();                           // ���� ���� �ǹ��̸� ��� ����
            else UpgradeBuilding();                                 // �� �ǹ��̸� ���׷��̵�
        }
    }

    /// <summary>
    /// �� �ǹ��� ����� ����
    /// </summary>
    void UnlockBuilding()
    {
       isBuyBuilding = true;

        GameManager.Instance.DecreaseGold(buildingPrice);

        GameManager.Instance.DoIncreaseGold(second, incrementGold);        // ������ �ð����� �� �����ϱ� ����

        unlockBuildingImage.SetActive(false);           // ��� �̹����� ����

        // �ؽ�Ʈ�� Active�� true��
        incrementGoldText.gameObject.SetActive(true);
    }

    /// <summary>
    /// �ǹ��� ���׷��̵�
    /// </summary>
    void UpgradeBuilding()
    {
        GameManager.Instance.DecreaseGold(buildingPrice);

        buildingPrice = (int)(buildingPrice * multiplyBuildingPrice);    // ����� ������ŭ ����
        buildingPriceText.text = GetCommaText(buildingPrice);

        incrementGold = (int)(incrementGold * multiplyGold);     // ���� �������� ������ŭ ����
        incrementGoldText.text = "���� ������ : " + GetCommaText(incrementGold);

        buildingLevel++;                       // �ǹ��� ���� ��
    }


    public void SantaButtonClick()
    {
        if (GameManager.Instance.myGold >= santaPrice)            // �÷��̾ ���� ���� �ǹ��� ���ݺ��� ���� ��
        {
            if (!isBuySanta) UnlockSanta();                           // ���� ���� �ǹ��̸� ��� ����
            else UpgradeSanta();                                 // �� �ǹ��̸� ���׷��̵�
        }
    }

    /// <summary>
    /// ��Ÿ�� ����� ����
    /// </summary>
    void UnlockSanta()
    {
        isBuySanta = true;

        GameManager.Instance.DecreaseGold(santaPrice);

        unlockSantaImage.SetActive(false);           // ��� �̹����� ����

    }

    /// <summary>
    /// ��Ÿ�� ���׷��̵�
    /// </summary>
    void UpgradeSanta()
    {
        GameManager.Instance.DecreaseGold(santaPrice);

        santaPrice = (int)(santaPrice * multiplySantaPrice);    // ����� ������ŭ ����
        santaPriceText.text = GetCommaText(santaPrice);

        santaLevel++;                       // �ǹ��� ���� ��
    }

    /// <summary>
    /// 1000 ���� ���� �޸��� �ٿ��ִ� �Լ�
    /// </summary>
    string GetCommaText(int i)
    {
        return string.Format("{0: #,###; -#,###;0}", i);
    }

    /// <summary>
    /// �÷��̾��� ������ ���� ��ư�� Interactable ����
    /// </summary>
    void SetButtonInteractable()
    {
        if (GameManager.Instance.level >= unlockLevel && GameManager.Instance.myGold >= buildingPrice)        // �÷��̾��� ������ ��� ���� ���� �������� ũ�� ���� ���� �ǹ��� ���ݺ��� Ŭ ��
            buildingButton.interactable = true;                                             //  Interactable�� True�� ����
        else buildingButton.interactable = false;

        if (GameManager.Instance.level >= unlockLevel && GameManager.Instance.myGold >= santaPrice)        // �÷��̾��� ������ ��� ���� ���� �������� ũ�� ���� ���� �ǹ��� ���ݺ��� Ŭ ��
            santaButton.interactable = true;                                             //  Interactable�� True�� ����
        else santaButton.interactable = false;

        if (GameManager.Instance.level >= unlockLevel)        // �÷��̾��� ������ ��� ���� ���� �������� ũ�� ���� ���� �ǹ��� ���ݺ��� Ŭ ��
            backgroundButton.interactable = true;                                             //  Interactable�� True�� ����
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
        incrementGoldText.text = "��� ������ : " + GetCommaText(incrementGold);

        unlockBuildingImage = buildingButton.transform.GetChild(2).gameObject;

        // ���� ���� �ǹ��� ��
        if (!isBuyBuilding)
        {
            unlockBuildingImage.SetActive(true);       // ��� �̹����� ������

            // �������� ���� �ؽ�Ʈ�� ����
            incrementGoldText.gameObject.SetActive(false);
        }

        santaButton = this.transform.GetChild(0).GetChild(4).GetComponent<Button>();
        
        santaPriceText = santaButton.transform.GetChild(0).GetComponent<Text>();
        santaPriceText.text = GetCommaText(santaPrice);

        unlockSantaImage = santaButton.transform.GetChild(2).gameObject;

        santaNameText = santaButton.transform.GetChild(3).GetComponent<Text>();
        santaNameText.text = santaName;

        // ���� ���� ��Ÿ�� ��
        if (!isBuySanta)
        {
            unlockSantaImage.SetActive(true);       // ��� �̹����� ������
        }
    }

    void Update()
    {
        SetButtonInteractable();
    }
}
