using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreObjectSc : MonoBehaviour
{
    #region ����
    Text buildingNameText;                  // �ǹ��� �̸� Text
    Text descText;                          // ���� Text
    Text unlockText;                        // Unlock ���� Text

    Text buildingPriceText;                 // �ǹ��� ���� Text
    Text incrementGoldText;                 
    GameObject unlockBuildingImage;

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
    public int santaPrice;                 // ��Ÿ ���� 

    public int buildingLevel;              // �ش� �ǹ��� ����
    public int santaLevel;

    public string desc;                    // �ǹ��� ����

    [Header("---------- ������Ʈ")]
    public GameObject santaObject;
    Santa santaInstant;

    #endregion

    // ��Ͽ� �ִ� ��ư Ŭ�� ��
    public void ButtonClick()
    {
        storePanel.selectedObject = this;       // ���õ� Object�� �ڽ����� ����
        storePanel.SelectStoreObject();
    }

    /// <summary>
    /// �ǹ� ��ư Ŭ�� �� ��� ���� Ȥ�� �ǹ� ���׷��̵�
    /// </summary>
    public void BuildingButtonClick()
    {
        if (GameManager.Instance.MyGold >= buildingPrice)           // �÷��̾ ���� ���� �ǹ��� ���ݺ��� ���� ��
        {
            if (!isBuyBuilding) 
                UnlockBuilding();                                   // ���� ���� �ǹ��̸� ��� ����
            else UpgradeBuilding();                                 // �� �ǹ��̸� ���׷��̵�
        }
    }

    /// <summary>
    /// �� �ǹ��� ����� ����
    /// </summary>
    void UnlockBuilding()
    {
       isBuyBuilding = true;

        GameManager.Instance.MyGold -= buildingPrice;

        GameManager.Instance.DoIncreaseGold(second, incrementGold);        // ������ �ð����� �� �����ϱ� ����

        unlockBuildingImage.SetActive(false);                              // ��� �̹����� ����

       
    }

    /// <summary>
    /// �ǹ��� ���׷��̵�
    /// </summary>
    void UpgradeBuilding()
    {
        GameManager.Instance.MyGold -= buildingPrice;

        buildingPrice = (int)(buildingPrice * multiplyBuildingPrice);    // ����� ������ŭ ����
        buildingPriceText.text = GetCommaText(buildingPrice);

        incrementGold = (int)(incrementGold * multiplyGold);            // ���� �������� ������ŭ ����
        incrementGoldText.text = "���� ������ : " + GetCommaText(incrementGold);

        buildingLevel++;                                                // �ǹ��� ���� ��
    }


    public void SantaButtonClick()
    {
        if (GameManager.Instance.MyGold >= santaPrice)            // �÷��̾ ���� ���� �ǹ��� ���ݺ��� ���� ��
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

        GameManager.Instance.MyGold -= santaPrice;

        unlockSantaImage.SetActive(false);           // ��� �̹����� ����

        GetNewSanta();      // ��Ÿ ������Ʈ ����
    }

    /// <summary>
    /// ���ο� ��Ÿ ����
    /// </summary>
    void GetNewSanta()
    {
        GameManager.Instance.HideStorePanel();

        // ��Ÿ ������Ʈ ����
        GameObject instant = GameObject.Instantiate(santaObject, santaObject.transform.position, santaObject.transform.rotation, santaObject.transform.parent);
        santaInstant = instant.transform.GetComponent<Santa>();

        santaInstant.InitSanta(santaName);
    }

    /// <summary>
    /// ��Ÿ�� ���׷��̵�
    /// </summary>
    void UpgradeSanta()
    {
        GameManager.Instance.MyGold -= santaPrice;

        santaPrice = (int)(santaPrice * multiplySantaPrice);    // ����� ������ŭ ����
        santaPriceText.text = GetCommaText(santaPrice);

        santaLevel++;                       // ��Ÿ�� ���� ��

        if (santaInstant) santaInstant.Level++;
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
       
        if (GameManager.Instance.Level >= unlockLevel)        // �÷��̾��� ������ ��� ���� ���� �������� ũ�� ���� ���� �ǹ��� ���ݺ��� Ŭ ��
        {
            backgroundButton.interactable = true;   //  Interactable�� True�� ����
            unlockBuildingImage.SetActive(false);
        }
        else
        {
            backgroundButton.interactable = false;
            unlockBuildingImage.SetActive(true);
        }
    }


    void Start()
    {
        backgroundButton = this.transform.GetChild(0).GetComponent<Button>();

        buildingNameText = this.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        buildingNameText.text = buildingName;

      
        descText = this.transform.GetChild(0).GetChild(1).GetComponent<Text>();
        descText.text = desc;

        unlockText = this.transform.GetChild(0).GetChild(2).GetComponent<Text>();
        unlockText.text = "Lv." + unlockLevel.ToString();

        unlockBuildingImage = this.transform.GetChild(0).GetChild(3).gameObject;

    }

    void Update()
    {
        SetButtonInteractable();
    }
}
