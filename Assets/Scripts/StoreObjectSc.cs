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
    Text secondText;                        // �� Text
    Text incrementAmountText;               // ��� ������ Text
    GameObject infoText;

    GameObject unlockImage;                 // �ڹ��� �̹���

    Button backgroundButton;

 
    public int index;

    [Header("---------- ����")]
    public int unlockLevel;                // ��� ���� ���� ����
    public int second;                     // �� �� ���� ������ ������
    public string desc;                    // �ǹ��� ����

    public bool isBuyBuilding = false;
    
   
    public string buildingName;                 // �ǹ� �̸�

    public float multiplyBuildingPrice;         // ���׷��̵� �� �ǹ� ���� ���� ����

    private string buildingPrice;              // �ǹ� ���� 
    public string BuildingPrice
    {
        get { return buildingPrice; }
        set 
        {
            buildingPrice = value;
            if(storeInstance) storeInstance.BuildingPrice = buildingPrice;
        }
    }

   
    private int buildingLevel = 1;
    public int BuildingLevel
    {
        get { return buildingLevel; }
        set 
        {
            buildingLevel = value;
            if (storeInstance) storeInstance.BuildingLevel = buildingLevel;
        }
    }

    public string santaName;               // ��Ÿ �̸�
    private bool isBuySanta = false;        // ��Ÿ�� �����ߴ��� ���ߴ���

    public float multiplySantaPrice;       // ���׷��̵� �� ��Ÿ ���� ���� ����

    public string santaPrice;               // ��Ÿ ���� 
    public string SantaPrice
    {
        get { return santaPrice; }
        set
        {
            santaPrice = value;
            if (storeInstance) storeInstance.SantaPrice = santaPrice;
        }
    }

 
    private int santaLevel;
    public int SantaLevel
    {
        get { return santaLevel; }
        set
        {
            santaLevel = value;
            if (storeInstance) storeInstance.SantaLevel = santaLevel;
        }
    }

    private string incrementGold;              // �� ������
    public string IncrementGold
    {
        get { return incrementGold; }
        set
        {
            incrementGold = value;
            if (incrementAmountText) incrementAmountText.text = incrementGold;
            if (storeInstance) storeInstance.IncrementGold = incrementGold;
        }
    }

    public int santaEfficiency;            // �˹� ȿ��


    [Header("---------- ������Ʈ")]
    //public GameObject santaObject;        // ������ ��Ÿ ������
    public GameObject buildingGroup;

    private Building buildingInstant;
    public Building BuildingInstant
    {
        get { return buildingInstant; }
    }

    private Santa santaInstant;

    // ĳ��
    private StorePanel storeInstance;
    private GameManager gameManager;
    private Transform thisTransform;

    StringBuilder sb = new StringBuilder();
    StringBuilder secondSb = new StringBuilder();

    #endregion


    #region ����Ƽ �޼ҵ�
    void OnEnable()
    {
        if (buildingInstant)
        {
            BuildingPrice = buildingInstant.BuildingPrice;
            IncrementGold = buildingInstant.IncrementGold;
            BuildingLevel = BuildingInstant.Level;
        }

        if (santaInstant)
        {
            SantaPrice = santaInstant.SantaPrice;
            SantaLevel = santaInstant.Level;
        }
    }

    void Start()
    {
        storeInstance = StorePanel.Instance;
        gameManager = GameManager.Instance;
        thisTransform = this.transform;

        InitUI();
    }

    void Update()
    {
        SetButtonInteractable();
    }

    #endregion

    // UI �ʱ� ����
    void InitUI()
    {
        backgroundButton = thisTransform.GetChild(0).GetComponent<Button>();

        buildingNameText = thisTransform.GetChild(1).GetComponent<Text>();
        buildingNameText.text = buildingName;

        unlockText = thisTransform.GetChild(2).GetComponent<Text>();
        sb.Append("Lv.");
        sb.Append(unlockLevel.ToString());
        sb.Append(" �� ��� ����");
        unlockText.text = sb.ToString();

        unlockImage = thisTransform.GetChild(3).gameObject;

        infoText = thisTransform.GetChild(4).gameObject;

        incrementAmountText = infoText.transform.GetChild(0).GetComponent<Text>();
        incrementAmountText.text = incrementGold;

        secondText = infoText.transform.GetChild(1).GetComponent<Text>();
        secondSb.Append(second.ToString());
        secondSb.Append("��");
        secondText.text = secondSb.ToString();
    }

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
        if (GoldManager.CompareBigintAndUnit(gameManager.MyGold, buildingPrice))           // �÷��̾ ���� ���� �ǹ��� ���ݺ��� ���� ��
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

       gameManager.MyGold -= GoldManager.UnitToBigInteger(buildingPrice);        // �ǹ� ��� ����
    
        NewBuilding();      // �ǹ� ������Ʈ ����
    }

    // ���ο� �ǹ� ����
    void NewBuilding()
    {
        gameManager.HideStorePanel();          // ���� â �����

        buildingInstant = buildingGroup.transform.GetChild(index).GetComponent<Building>();
        buildingInstant.InitBuilding(index, buildingName, multiplyBuildingPrice, buildingPrice, incrementGold);
    }

    // �ǹ��� ���׷��̵�
    void UpgradeBuilding()
    {
        buildingInstant.Upgrade();

        BuildingPrice = buildingInstant.BuildingPrice;
        IncrementGold = buildingInstant.IncrementGold;
        BuildingLevel = buildingLevel + 1;
    }

    // ��Ÿ ��� ��ư Ŭ�� �� ��Ÿ ���� Ȥ�� ���׷��̵�
    public void SantaButtonClick()
    {
        if (GoldManager.CompareBigintAndUnit(gameManager.MyGold, santaPrice))            // �÷��̾ ���� ���� �ǹ��� ���ݺ��� ���� ��
        {
            if (!isBuySanta) BuyNewSanta();                    // ���� ���� �ǹ��̸� ���� ����
            else UpgradeSanta();                                 // �� �ǹ��̸� ���׷��̵�
        }
    }

    // ���ο� ��Ÿ ����
    void BuyNewSanta()
    {
        isBuySanta = true;

        gameManager.MyCarrots -= GoldManager.UnitToBigInteger(santaPrice);      // ��Ÿ ��� ����

        CreateNewSanta();      // ��Ÿ ������Ʈ ����
    }

    // ���ο� ��Ÿ ����
    void CreateNewSanta()
    {
        gameManager.HideStorePanel();          // ���� â �����

        santaInstant = buildingGroup.transform.GetChild(index).GetChild(1).GetComponent<Santa>();

        santaInstant.InitSanta(index, santaName, (float)second, multiplySantaPrice, santaPrice, santaEfficiency, buildingInstant);
    }

    // ��Ÿ�� ���׷��̵�
    void UpgradeSanta()
    {
        santaInstant.Upgrade();

        SantaPrice = santaInstant.SantaPrice;
        IncrementGold = buildingInstant.IncrementGold;
        SantaLevel = santaLevel + 1;
    }

    // �÷��̾��� ������ ���� ��ư�� Interactable ����
    void SetButtonInteractable()
    {
        if (gameManager.Level >= unlockLevel)        // �÷��̾��� ������ ��� ���� ���� �������� ũ�� ���� ���� �ǹ��� ���ݺ��� Ŭ ��
        {
            backgroundButton.interactable = true;   //  Interactable�� True�� ����
            infoText.SetActive(true);
            unlockImage.SetActive(false);
            unlockText.gameObject.SetActive(false);
        }
        else
        {
            backgroundButton.interactable = false;
            infoText.SetActive(false);
            unlockImage.SetActive(true);
            unlockText.gameObject.SetActive(true);
        }
    }

   
}
