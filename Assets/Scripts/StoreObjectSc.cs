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
    GameObject unlockImage;

    Button backgroundButton;

    [Header("---------- ����")]
    public int unlockLevel;                // ��� ���� ���� ����
    public int second;                     // �� �� ���� ������ ������
    public string desc;                    // �ǹ��� ����

    private bool isBuyBuilding = false;     // �ǹ��� �� ������ �ƴ���
    public string buildingName;            // �ǹ� �̸�

    public float multiplyBuildingPrice;    // ���׷��̵� �� �ǹ� ���� ���� ����
    public int buildingPrice;              // �ǹ� ���� 
    public float multiplyGold;             // ���׷��̵� �� �÷��̾� �� ���� ����
    public int incrementGold;              // �÷��̾��� �� ������

    public bool isBuySanta = false;        // ��Ÿ�� �� ������ �ƴ���
    public string santaName;               // ��Ÿ �̸�

    public float multiplySantaPrice;       // ���׷��̵� �� ��Ÿ ���� ���� ����
    public int santaPrice;                 // ��Ÿ ���� 
    public float multiplyAmountObtained;   // ���׷��̵� �� ȹ�淮 ���� ����
    public float amountObtained;           // ȹ�淮 ����


    [Header("---------- ������Ʈ")]
    public GameObject santaObject;
    Santa santaInstant;

    #endregion

    // ��Ͽ� �ִ� ��ư Ŭ�� ��
    public void ButtonClick()
    {
        StorePanel.Instance.selectedObject = this;       // ���õ� Object�� �ڽ����� ����
        StorePanel.Instance.SelectStoreObject();
    }

    void Unlock()
    {
        backgroundButton.interactable = true;   //  Interactable�� True�� ����
        unlockImage.SetActive(false);
    }

    /// <summary>
    /// �ǹ� ��� ��ư Ŭ�� �� ��� ���� Ȥ�� �ǹ� ���׷��̵�
    /// </summary>
    public void BuildingButtonClick()
    {
        if (GameManager.Instance.MyGold >= buildingPrice)           // �÷��̾ ���� ���� �ǹ��� ���ݺ��� ���� ��
        {
            if (!isBuyBuilding)
                BuyNewBuilding();                                   // ���� ���� �ǹ��̸� ��� ����
            else UpgradeBuilding();                                 // �� �ǹ��̸� ���׷��̵�
        }
    }

    /// <summary>
    /// �� �ǹ��� ����� ����
    /// </summary>
    void BuyNewBuilding()
    {
       isBuyBuilding = true;

        GameManager.Instance.MyGold -= buildingPrice;

        GameManager.Instance.DoIncreaseGold(second, incrementGold);        // ������ �ð����� �� �����ϱ� ����
    }

    /// <summary>
    /// �ǹ��� ���׷��̵�
    /// </summary>
    void UpgradeBuilding()
    {
        GameManager.Instance.MyGold -= buildingPrice;

        buildingPrice = (int)(buildingPrice * multiplyBuildingPrice);    // ����� ������ŭ ����
        StorePanel.Instance.BuildingPrice = buildingPrice;

        incrementGold = (int)(incrementGold * multiplyGold);            // ���� �������� ������ŭ ����
        StorePanel.Instance.IncrementGold = incrementGold;

        //buildingLevel++;                                                // �ǹ��� ���� ��
    }


    public void SantaButtonClick()
    {
        if (GameManager.Instance.MyGold >= santaPrice)            // �÷��̾ ���� ���� �ǹ��� ���ݺ��� ���� ��
        {
            if (!isBuySanta) BuyNewSanta();                           // ���� ���� �ǹ��̸� ��� ����
            else UpgradeSanta();                                 // �� �ǹ��̸� ���׷��̵�
        }
    }

    /// <summary>
    /// ��Ÿ�� ����� ����
    /// </summary>
    void BuyNewSanta()
    {
        isBuySanta = true;

        GameManager.Instance.MyGold -= santaPrice;

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
        StorePanel.Instance.SantaPrice = santaPrice;

        amountObtained = (float)(amountObtained * multiplyAmountObtained);  // ȹ�淮�� ������ŭ ����
        StorePanel.Instance.IncrementAmount = amountObtained;

        if (santaInstant) 
            santaInstant.Level++;
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
            unlockImage.SetActive(false);
        }
        else
        {
            backgroundButton.interactable = false;
            unlockImage.SetActive(true);
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

        unlockImage = this.transform.GetChild(0).GetChild(3).gameObject;


        
    }

    void Update()
    {
        SetButtonInteractable();
    }
}
