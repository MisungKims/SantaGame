using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

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

    public bool isBuyBuilding = false;
   
    public string buildingName;            // �ǹ� �̸�

    public float multiplyBuildingPrice;    // ���׷��̵� �� �ǹ� ���� ���� ����
    public int buildingPrice;              // �ǹ� ���� 
    public float multiplyGold;             // ���׷��̵� �� �÷��̾� �� ���� ����
    public int incrementGold;              // �÷��̾��� �� ������

    public string santaName;               // ��Ÿ �̸�
    private bool isBuySanta = false;        // ��Ÿ�� �����ߴ��� ���ߴ���

    public float multiplySantaPrice;       // ���׷��̵� �� ��Ÿ ���� ���� ����
    public int santaPrice;                 // ��Ÿ ���� 
    public float multiplyAmountObtained;   // ���׷��̵� �� ȹ�淮 ���� ����
    public float amountObtained;           // ȹ�淮 ����


    [Header("---------- ������Ʈ")]
    public GameObject santaObject;        // ������ ��Ÿ ������
    public GameObject santaGroup;
    public GameObject buildingGroup;

    private Building buildingInstant;
    public Building BuildingInstant
    {
        get { return BuildingInstant; }
    }

    private Santa santaInstant;

    // ĳ��
    private StorePanel storeInstance;
    private GameManager gameManagerInstance;
    private Transform thisTransform;

    StringBuilder sb = new StringBuilder();
   
    #endregion


    #region ����Ƽ �޼ҵ�

    void Start()
    {
        storeInstance = StorePanel.Instance;
        gameManagerInstance = GameManager.Instance;
        thisTransform = this.transform;


        backgroundButton = thisTransform.GetChild(0).GetComponent<Button>();

        buildingNameText = thisTransform.GetChild(1).GetComponent<Text>();
        buildingNameText.text = buildingName;

        unlockText = thisTransform.GetChild(2).GetComponent<Text>();
        sb.Append("Lv.");
        sb.Append(unlockLevel.ToString());
        unlockText.text = sb.ToString();

        unlockImage = thisTransform.GetChild(3).gameObject;


        //descText = this.transform.GetChild(0).GetChild(1).GetComponent<Text>();
        //descText.text = desc;

    }

    void Update()
    {
        SetButtonInteractable();
    }

    #endregion


    // ��Ͽ� �ִ� ��ư Ŭ�� ��
    public void ButtonClick()
    {
        storeInstance.selectedObject = this;       // ���õ� Object�� �ڽ����� ����
        storeInstance.SelectStoreObject();
    }

    void Unlock()
    {
        backgroundButton.interactable = true;   //  Interactable�� True�� ����
        unlockImage.SetActive(false);
    }


    // �ǹ� ��� ��ư Ŭ�� �� �ǹ� ���� Ȥ�� ���׷��̵�
    public void BuildingButtonClick()
    {
        if (gameManagerInstance.MyGold >= buildingPrice)           // �÷��̾ ���� ���� �ǹ��� ���ݺ��� ���� ��
        {
            if (!isBuyBuilding)
                BuyNewBuilding();                                   // ���� ���� �ǹ��̸� ���� ����
            else UpgradeBuilding();                                 // �� �ǹ��̸� ���׷��̵�
        }
    }

    
    // ���ο� �ǹ� ����
    void BuyNewBuilding()
    {
       isBuyBuilding = true;

       gameManagerInstance.MyGold -= buildingPrice;                       // �ǹ� ��� ����

        NewBuilding();      // �ǹ� ������Ʈ ����
    }

    // ���ο� �ǹ� ����
    void NewBuilding()
    {
        gameManagerInstance.HideStorePanel();          // ���� â �����

        buildingInstant = buildingGroup.transform.GetChild(index).GetComponent<Building>();
        buildingInstant.InitBuilding(index, buildingName, multiplyBuildingPrice, buildingPrice, multiplyGold, incrementGold);
    }

    // �ǹ��� ���׷��̵�
    void UpgradeBuilding()
    {
        buildingInstant.Upgrade();
       
        storeInstance.BuildingPrice = buildingPrice;
        storeInstance.IncrementGold = incrementGold;
    }

    // ��Ÿ ��� ��ư Ŭ�� �� ��Ÿ ���� Ȥ�� ���׷��̵�
    public void SantaButtonClick()
    {
        if (gameManagerInstance.MyGold >= santaPrice)            // �÷��̾ ���� ���� �ǹ��� ���ݺ��� ���� ��
        {
            if (!isBuySanta) BuyNewSanta();                    // ���� ���� �ǹ��̸� ���� ����
            else UpgradeSanta();                                 // �� �ǹ��̸� ���׷��̵�
        }
    }

    // ���ο� ��Ÿ ����
    void BuyNewSanta()
    {
        isBuySanta = true;

        gameManagerInstance.MyGold -= santaPrice;      // ��Ÿ ��� ����

        CreateNewSanta();      // ��Ÿ ������Ʈ ����
    }

    // ���ο� ��Ÿ ����
    void CreateNewSanta()
    {
        gameManagerInstance.HideStorePanel();          // ���� â �����

        santaInstant = santaGroup.transform.GetChild(index).GetComponent<Santa>();

        santaInstant.InitSanta(index, santaName, multiplySantaPrice, santaPrice, multiplyAmountObtained, amountObtained, buildingInstant);
    }

    // ��Ÿ�� ���׷��̵�
    void UpgradeSanta()
    {
        santaInstant.Upgrade();
       
        storeInstance.SantaPrice = santaPrice;
        storeInstance.IncrementAmount = amountObtained;
    }

    // �÷��̾��� ������ ���� ��ư�� Interactable ����
    void SetButtonInteractable()
    {
        if (gameManagerInstance.Level >= unlockLevel)        // �÷��̾��� ������ ��� ���� ���� �������� ũ�� ���� ���� �ǹ��� ���ݺ��� Ŭ ��
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
}
