/**
 * @details ������Ʈ ���� �� ���׷��̵�
 * @author ��̼�
 * @date 22-04-19
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class StoreObject : MonoBehaviour
{
    #region ����
    // UI ����
    [Header("---------- UI ����")]
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
    //[SerializeField]
    //Button upgradeBuildingButton;                  // �ǹ��� �̸� Text
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
    //[SerializeField]
    //Button upgradeSantaButton;                  // �ǹ��� �̸� Text
    [SerializeField]
    Text santaPriceText;                  // �ǹ��� �̸� Text
    [SerializeField]
    Text efficiencyText;                  // �ǹ��� �̸� Text
    [SerializeField]
    GameObject santaImageGroup;                  // �ǹ��� �̸� Text


    Button backgroundButton;

    public Object storeObject;


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

            sb.Clear();
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
            objectNameText.text = buildingName;
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


    private string prerequisites;
    public string Prerequisites
    {
        set
        {
            prerequisites = value;
            PrerequisitesText.text = string.Format("! {0} �ʿ�", prerequisites);
        }
    }


    public bool isBuyBuilding = false;       // �ǹ��� �����ߴ��� ���ߴ��� (��������� �ߴ���)
    public float multiplyBuildingPrice;         // ���׷��̵� �� �ǹ� ���� ���� ����

    private bool isBuySanta = false;        // ��Ÿ�� �����ߴ��� ���ߴ���
    public float multiplySantaPrice;       // ���׷��̵� �� ��Ÿ ���� ���� ����


    private bool isGetPrerequisites = false;


    [Header("---------- ������Ʈ")]
    //public GameObject santaObject;        // ������ ��Ÿ ������
    public GameObject buildingGroup;

    private Building buildingInstance;
    public Building BuildingInstant
    {
        get { return buildingInstance; }
    }

    private Santa santaInstance;

    // ĳ��
    private StorePanel storeInstance;
    private GameManager gameManager;

    private GameObject prerequisitesGb;

    StringBuilder sb = new StringBuilder();
    #endregion


    #region ����Ƽ �޼ҵ�

    private void Awake()
    {
        // �� ����

        BuildingName = storeObject.buildingName;                      // �ǹ� �̸�
        UnlockLevel = storeObject.unlockLevel;                        // ��� ���� ���� ����
        Second = storeObject.second;                                    // �� �� ���� ������ ������
        multiplyBuildingPrice = storeObject.multiplyBuildingPrice;       // ���׷��̵� �� �ǹ� ���� ���� ����
        BuildingPrice = storeObject.buildingPrice;                      // �ǹ� ���� 
        IncrementGold = storeObject.incrementGold;                    // �÷��̾��� �� ������
        SantaName = storeObject.santaName;                    // ��Ÿ �̸�
        multiplySantaPrice = storeObject.multiplySantaPrice;          // ���׷��̵� �� �ǹ� ���� ���� ����
        SantaPrice = storeObject.santaPrice;                         // �ǹ� ���� 
        SantaEfficiency = storeObject.santaEfficiency;
        Desc = storeObject.desc;                              // �ǹ��� ����
        Prerequisites = storeObject.prerequisites;
    }

    void OnEnable()
    {
        if (buildingInstance)
        {
            BuildingPrice = buildingInstance.BuildingPrice;
            IncrementGold = buildingInstance.IncrementGold;
            BuildingLevel = BuildingInstant.Level;
        }

        if (santaInstance)
        {
            SantaPrice = santaInstance.SantaPrice;
            SantaLevel = santaInstance.Level;
        }

    }

    void Start()
    {
        storeInstance = StorePanel.instance;
        gameManager = GameManager.Instance;
        prerequisitesGb = PrerequisitesText.gameObject;

        if (index == 0)
        {
            isGetPrerequisites = true;
        }

        if (!isBuyBuilding)      // �ǹ��� ���� �ʾ��� �� (������� ������ ��)
        {
            lockingImage.SetActive(true);
            unlockingObject.SetActive(false);
        }
    }

    void Update()
    {
        SetObjectActive();
    }
    #endregion

    /// <summary>
    /// �÷��̾��� ������ ���� ��ư�� active ����
    /// </summary>
    void SetObjectActive()
    {
        if (!isBuyBuilding)                                         // �ǹ��� ���� �ʾҰ� (������� ������ ��)
        {
            if (gameManager.Level < unlockLevel)                    // ������� �Ұ����� ������ ��
            {
                unlockButton.SetActive(false);                      // ������� ��ư�� �������� �ؽ�Ʈ�� ����
                prerequisitesGb.SetActive(false);
            }
            else                                                    // ������� ���� ������ ��
            {
                if (isGetPrerequisites)                             // ���� ������ ���� ������
                {
                    unlockButton.SetActive(true);                   // ������� ��ư�� ������
                    prerequisitesGb.SetActive(false);
                }
                else                                                 // ���� ������ �������� �ʾ�����
                {
                    unlockButton.SetActive(false);
                    prerequisitesGb.SetActive(true);                 // �������� �ؽ�Ʈ�� ������
                }
            }
        }
    }

    /// <summary>
    /// �ǹ��� ��� ���� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void Unlock()
    {
        lockingImage.SetActive(false);
        unlockingObject.SetActive(true);

        StoreManager.instance.storeObjectList[index + 1].isGetPrerequisites = true;      // ���� �ǹ��� ���������� ������Ŵ

        if (!isBuyBuilding) BuyNewBuilding();
    }

    /// <summary>
    /// ���ο� �ǹ� ����
    /// </summary>
    void BuyNewBuilding()
    {
        isBuyBuilding = true;

        gameManager.MyGold -= GoldManager.UnitToBigInteger(buildingPrice);        // �ǹ� ��� ����

       // �ǹ� ������Ʈ ����
        buildingInstance = ObjectManager.Instance.buildingList[index].GetComponent<Building>();

        buildingInstance.NewBuilding();
    }

    /// <summary>
    /// �ǹ� ���׷��̵� ��ư Ŭ�� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void BuildingButtonClick()
    {
        if (GoldManager.CompareBigintAndUnit(gameManager.MyGold, buildingPrice))    // �÷��̾ ���� ������ ���׷��̵尡 ������ ��
        {
            buildingInstance.Upgrade();

            BuildingPrice = buildingInstance.BuildingPrice;
            IncrementGold = buildingInstance.IncrementGold;
            BuildingLevel = buildingInstance.Level;
        }
    }


    /// <summary>
    /// ��Ÿ ��� ��ư Ŭ�� �� ��Ÿ ���� Ȥ�� ���׷��̵� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void SantaButtonClick()
    {
        if (GoldManager.CompareBigintAndUnit(gameManager.MyGold, santaPrice))     // �÷��̾ ���� ������ ���׷��̵尡 ������ ��
        {
            if (!isBuySanta) BuyNewSanta();                    // ���� ���� �ǹ��̸� ���� ����
            else UpgradeSanta();                                 // �� �ǹ��̸� ���׷��̵�
        }
    }

    /// <summary>
    /// ���ο� ��Ÿ ����
    /// </summary>
    void BuyNewSanta()
    {
        isBuySanta = true;

        gameManager.MyCarrots -= GoldManager.UnitToBigInteger(santaPrice);      // ��Ÿ ��� ����

        santaInstance = ObjectManager.Instance.santaList[index].GetComponent<Santa>();
        santaInstance.NewSanta();
    }

    /// <summary>
    /// ��Ÿ�� ���׷��̵�
    /// </summary>
    void UpgradeSanta()
    {
        santaInstance.Upgrade();

        SantaPrice = santaInstance.SantaPrice;
        IncrementGold = buildingInstance.IncrementGold;
        SantaLevel = santaInstance.Level;
    }


}
