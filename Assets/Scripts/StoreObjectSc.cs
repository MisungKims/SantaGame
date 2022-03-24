using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class StoreObjectSc : MonoBehaviour
{
    #region ����
    [Header("---------- ������Ʈ")]
    [SerializeField]
    Text descText;                          // ���� Text
    [SerializeField]
    Text secondText;                        // �� Text
    [SerializeField]
    Text incrementAmountText;               // ��� ������ Text
    [SerializeField]
    GameObject unlockingObject;

    [SerializeField]
    Text objectNameText;               // ��� ������ Text
    [SerializeField]
    Text lockingLevelText;                        // Unlock ���� Text
    [SerializeField]
    GameObject lockingImage;                 // �ڹ��� �̹���
    [SerializeField]
    Text PrerequisitesText;
    [SerializeField]
    GameObject unlockButton;                 // �ڹ��� �̹���

    [Header("----------------- ����")]

    [SerializeField]
    Text buildingNameText;                  // �ǹ��� �̸� Text
    [SerializeField]
    Text buildingLevelText;                  // �ǹ��� �̸� Text
    [SerializeField]
    Button upgradeBuildingButton;                  // �ǹ��� �̸� Text
    [SerializeField]
    Text buildingPriceText;                  // �ǹ��� �̸� Text
    [SerializeField]
    Text incrementGoldText;                  // �ǹ��� �̸� Text
    [SerializeField]
    GameObject buildingImageGroup;                  // �ǹ��� �̸� Text

    [Header("----------------- ��Ÿ")]

    [SerializeField]
    Text santaNameText;                  // �ǹ��� �̸� Text
    [SerializeField]
    Text santaLevelText;                  // �ǹ��� �̸� Text
    [SerializeField]
    Button upgradeSantaButton;                  // �ǹ��� �̸� Text
    [SerializeField]
    Text santaPriceText;                  // �ǹ��� �̸� Text
    [SerializeField]
    Text efficiencyText;                  // �ǹ��� �̸� Text
    [SerializeField]
    GameObject santaImageGroup;                  // �ǹ��� �̸� Text


    Button backgroundButton;

 
    public int index;

    private string desc;                    // �ǹ��� ����
    public string Desc
    {
        get { return desc; }
        set
        {
            desc = value;
            descText.text = desc;
        }
    }

    private int unlockLevel;                // ��� ���� ���� ����
    public int UnlockLevel
    {
        get { return unlockLevel; }
        set
        {
            unlockLevel = value;

            sb.Append("Lv.");
            sb.Append(unlockLevel.ToString());
            sb.Append(" �� ��� ���� ����");
            lockingLevelText.text = sb.ToString();
        }
    }

    private int second;                     // �� �� ���� ������ ������
    public int Second
    {
        get { return second; }
        set
        {
            second = value;
            secondText.text = string.Format("{0}��", second);
        }
    }

    private string incrementGold;              // �� ������
    public string IncrementGold
    {
        get { return incrementGold; }
        set
        {
            incrementGold = value;
            incrementAmountText.text = incrementGold;
            incrementGoldText.text = string.Format("+ {0}", GoldManager.MultiplyUnit(incrementGold, 0.1f)); 
        }
    }

    private string buildingName;                 // �ǹ� �̸�
    public string BuildingName
    {
        set
        {
            buildingName = value;
            buildingNameText.text = buildingName;
        }
    }

  
    private string buildingPrice;              // �ǹ� ���� 
    public string BuildingPrice
    {
        get { return buildingPrice; }
        set 
        {
            buildingPrice = value;
            buildingPriceText.text = buildingPrice;
        }
    }

    private int buildingLevel = 1;
    public int BuildingLevel
    {
        get { return buildingLevel; }
        set 
        {
            buildingLevel = value;
            buildingLevelText.text = string.Format("Lv. {0}", buildingLevel);
        }
    }

    private string santaName;               // ��Ÿ �̸�
    public string SantaName
    {
        set
        {
            santaName = value;
            santaNameText.text = santaName;
        }
    }

    private string santaPrice;               // ��Ÿ ���� 
    public string SantaPrice
    {
        get { return santaPrice; }
        set
        {
            santaPrice = value;
            santaPriceText.text = santaPrice.ToString();
        }
    }

 
    private int santaLevel;
    public int SantaLevel
    {
        get { return santaLevel; }
        set
        {
            santaLevel = value;
            santaLevelText.text = string.Format("Lv. {0}", santaLevel);
        }
    }

    private int santaEfficiency;            // �˹� ȿ��
    public int SantaEfficiency
    {
        get { return santaEfficiency; }
        set
        {
            santaEfficiency = value;
            efficiencyText.text = string.Format("{0}%", santaEfficiency.ToString());
        }
    }


    public bool isBuyBuilding = false;       // �ǹ��� �����ߴ��� ���ߴ��� (��������� �ߴ���)
    public float multiplyBuildingPrice;         // ���׷��̵� �� �ǹ� ���� ���� ����

    private bool isBuySanta = false;        // ��Ÿ�� �����ߴ��� ���ߴ���
    public float multiplySantaPrice;       // ���׷��̵� �� ��Ÿ ���� ���� ����


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

        objectNameText.text = name;

        //PrerequisitesText.text = string.Format("! {0} �ʿ�", storeInstance.ObjectList[index - 1].name);
    }

    void Update()
    {
        SetObjectActive();

    }

    #endregion


    // ��Ͽ� �ִ� ��ư Ŭ�� ��
    public void ButtonClick()
    {
        storeInstance.selectedObject = this;       // ���õ� Object�� �ڽ����� ����
        //storeInstance.SelectStoreObject();
    }

    public void Unlock()    // ��� ����
    {
        lockingImage.SetActive(false);
        unlockingObject.SetActive(true);

        if (!isBuyBuilding)
            BuyNewBuilding();                                   // ���� ���� �ǹ��̸� ���� ����
    }

    // ���ο� �ǹ� ����
    void BuyNewBuilding()
    {
        isBuyBuilding = true;

        gameManager.MyGold -= GoldManager.UnitToBigInteger(buildingPrice);        // �ǹ� ��� ����

        NewBuilding();      // �ǹ� ������Ʈ ����
    }

    // �ǹ� ��� ��ư Ŭ�� �� �ǹ� ���� Ȥ�� ���׷��̵�
    public void BuildingButtonClick()
    {
        if (GoldManager.CompareBigintAndUnit(gameManager.MyGold, buildingPrice))           // �÷��̾ ���� ���� �ǹ��� ���ݺ��� ���� ��
        {
            //if (!isBuyBuilding)
            //    BuyNewBuilding();                                   // ���� ���� �ǹ��̸� ���� ����
            UpgradeBuilding();                                 // �� �ǹ��̸� ���׷��̵�
        }
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

    // �÷��̾��� ������ ���� ������Ʈ�� active ����
    void SetObjectActive()
    {
        if (!isBuyBuilding)      // �ǹ��� ���� �ʾ��� �� (������� ������ ��)
        {
            lockingImage.SetActive(true);
            unlockingObject.SetActive(false);

            if (gameManager.Level >= unlockLevel)        // �÷��̾��� ������ ��� ���� ���� �������� Ŭ ��
            {
                unlockButton.SetActive(true);
                //if (index != 0 && storeInstance.ObjectList[index - 1].isBuyBuilding)      // ���� �������� �ʿ��� �ǹ��� ���� ��
                //{
                //    unlockButton.SetActive(true);
                //    PrerequisitesText.gameObject.SetActive(false);
                //}
                //else
                //{
                //    unlockButton.SetActive(false);
                //    PrerequisitesText.gameObject.SetActive(true);
                //}
            }
            else
            {
                unlockButton.SetActive(false);
            }
        }
    }
}
