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

 
    public int index;

    [Header("---------- ����")]
    public int unlockLevel;                // ��� ���� ���� ����
    public int second;                     // �� �� ���� ������ ������
    public string desc;                    // �ǹ��� ����

    public bool isBuyBuilding = false;     // �ǹ��� �� ������ �ƴ���
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
    public GameObject santaObject;        // ������ ��Ÿ ������
    public GameObject santaGroup;
    public GameObject buildingGroup;
   
    public List<Building> BuildingList = new List<Building>();
    public List<Santa> SantaList = new List<Santa>();

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

        GameManager.Instance.MyGold -= buildingPrice;                       // �ǹ� ��� ����

        GameManager.Instance.DoIncreaseGold(second, incrementGold);        // ������ �ð����� �� �����ϱ� ����

        NewBuilding();      // �ǹ� ������Ʈ ����
    }

    // ���ο� �ǹ� ����
    void NewBuilding()
    {
        GameManager.Instance.HideStorePanel();          // ���� â �����

        BuildingList[index] = buildingGroup.transform.GetChild(index).GetComponent<Building>();
        BuildingList[index].InitBuilding(index, buildingName, multiplyBuildingPrice, buildingPrice, multiplyGold, incrementGold);
    }

    /// <summary>
    /// �ǹ��� ���׷��̵�
    /// </summary>
    void UpgradeBuilding()
    {
        if(BuildingList[index])
        {
            BuildingList[index].Upgrade();
        }

        StorePanel.Instance.BuildingPrice = buildingPrice;
        StorePanel.Instance.IncrementGold = incrementGold;
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

        GameManager.Instance.MyGold -= santaPrice;      // ��Ÿ ��� ����

        CreateNewSanta();      // ��Ÿ ������Ʈ ����
    }

    /// <summary>
    /// ���ο� ��Ÿ ����
    /// </summary>
    void CreateNewSanta()
    {
        GameManager.Instance.HideStorePanel();          // ���� â �����

        
        // ��Ÿ ������Ʈ ����
        GameObject instant = GameObject.Instantiate(
            santaObject, 
            BuildingList[index].transform.GetChild(1).position, 
            BuildingList[index].transform.GetChild(1).rotation, 
            santaGroup.transform);

        SantaList[index] = instant.transform.GetComponent<Santa>();

        SantaList[index].InitSanta(index, santaName, multiplySantaPrice, santaPrice, multiplyAmountObtained, amountObtained);

        SantaList[index].building = BuildingList[index];
    }

    /// <summary>
    /// ��Ÿ�� ���׷��̵�
    /// </summary>
    void UpgradeSanta()
    {
        if (SantaList[index])
        {
            SantaList[index].Upgrade();
        }

        StorePanel.Instance.SantaPrice = santaPrice;
        StorePanel.Instance.IncrementAmount = amountObtained;
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

        BuildingList = new List<Building>(new Building[StorePanel.Instance.ObjectList.Count]);
        SantaList = new List<Santa>(new Santa[StorePanel.Instance.ObjectList.Count]);

    }

    void Update()
    {
        SetButtonInteractable();
    }
}
