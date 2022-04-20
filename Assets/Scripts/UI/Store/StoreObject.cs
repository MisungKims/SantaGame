/**
 * @details ������Ʈ ���� �� ���׷��̵�
 * @author ��̼�
 * @date 22-04-20
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
    Text santaPriceText;                  // �ǹ��� �̸� Text
    [SerializeField]
    Text efficiencyText;                  // �ǹ��� �̸� Text
    [SerializeField]
    GameObject santaImageGroup;                  // �ǹ��� �̸� Text

    public Object storeObject;
    public int index;
 
    public string Desc                  // �ǹ��� ����
    {
        set { descText.text = value; }
    }

    public int UnlockLevel          // ��� ���� ���� ����
    {
        get { return storeObject.unlockLevel; }
        set
        {
            sb.Clear();
            sb.Append("Lv.");
            sb.Append(value.ToString());
            sb.Append(" �� ��� ���� ����");
            lockingLevelText.text = sb.ToString();
        }
    }

    public int Second                    // �� �� ���� ������ ������
    {
        set{  secondText.text = $"{value}��"; }
    }

    public string IncrementGold          // ��� ������
    {
        set
        {
            incrementAmountText.text = value;
            incrementGoldText.text = $"+ {GoldManager.MultiplyUnit(value, 0.1f)}";
            
        }
    }

    public string BuildingName
    {
        set
        {
            buildingNameText.text = value;
            objectNameText.text = value;
        }
    }

    public string BuildingPrice
    {
        get { return storeObject.buildingPrice; }
        set {  buildingPriceText.text = value; }
    }

    public int BuildingLevel
    {
        set { buildingLevelText.text = $"LV. {value}"; }
    }

    public string SantaName
    {
        set { santaNameText.text = value; }
    }

    public string SantaPrice
    {
        get { return storeObject.santaPrice; }
        set { santaPriceText.text = value.ToString();  }
    }

    public int SantaLevel
    {
        set { santaLevelText.text = $"Lv. {value}"; }
    }

    public int SantaEfficiency
    {
        set { efficiencyText.text = $"{value}%"; }
    }

    private string prerequisites;
    public string Prerequisites
    {
        set
        {
            prerequisites = value;
            PrerequisitesText.text = $"! {prerequisites} �ʿ�";
        }
    }

    public bool isBuyBuilding = false;       // �ǹ��� �����ߴ��� ���ߴ��� (��������� �ߴ���)
  
    private bool isBuySanta = false;        // ��Ÿ�� �����ߴ��� ���ߴ���

    private bool isGetPrerequisites = false;    // ���� ���� �ǹ��� ������� �Ǿ�����


    [Header("---------- ������Ʈ")]
    private GameObject prerequisitesGb;

    public GameObject buildingGroup;

    private Building buildingInstance;

    private Santa santaInstance;

    // ĳ��
    private GameManager gameManager;

    StringBuilder sb = new StringBuilder();
    #endregion

    #region ����Ƽ �޼ҵ�

    private void Awake()
    {
        gameManager = GameManager.Instance;
        prerequisitesGb = PrerequisitesText.gameObject;

        RefreshAll();

        if (index == 0)
            isGetPrerequisites = true;

        if (!isBuyBuilding)      // �ǹ��� ���� �ʾ��� �� (������� ������ ��)
        {
            lockingImage.SetActive(true);
            unlockingObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        RefreshBuildingInfo();
        RefreshSantaInfo();
    }

    void Start()
    {
        Check();
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// ��� UI ���ΰ�ħ
    /// </summary>
    void RefreshAll()
    {
        Desc = storeObject.desc;
        BuildingName = storeObject.buildingName;
        SantaName = storeObject.santaName;
        UnlockLevel = storeObject.unlockLevel;
        Second = storeObject.second;
        Prerequisites = storeObject.prerequisites;

        RefreshBuildingInfo();
        RefreshSantaInfo();
    }

    /// <summary>
    /// �ǹ��� UI ���� ��ħ
    /// </summary>
    void RefreshBuildingInfo()
    {
        BuildingPrice = storeObject.buildingPrice;
        IncrementGold = storeObject.incrementGold;
        BuildingLevel = storeObject.buildingLevel;
    }

    /// <summary>
    /// ��Ÿ�� UI ���� ��ħ
    /// </summary>
    void RefreshSantaInfo()
    {
        SantaPrice = storeObject.santaPrice;
        SantaEfficiency = storeObject.santaEfficiency;
        SantaLevel = storeObject.santaLevel;
    }

    /// <summary>
    /// ���� �� ���� �� Ȥ�� �������� �ǹ��� ������� ���� �� üũ�Ͽ� ��ư�� active ����
    /// </summary>
    public void Check()
    {
        if (!isBuyBuilding)                                         // �ǹ��� ���� �ʾҰ� (������� ������ ��)
        {
            if (gameManager.Level < UnlockLevel)                    // ������� �Ұ����� ������ ��
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

    #region �ǹ�
    /// <summary>
    /// �ǹ��� ��� ���� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void Unlock()
    {
        if (gameManager.Level < UnlockLevel)   // ��� ���� �Ұ����ϴٸ� return
        {
            return;
        }

        lockingImage.SetActive(false);
        unlockingObject.SetActive(true);

        BuyNewBuilding();
    }

    /// <summary>
    /// ���ο� �ǹ� ����
    /// </summary>
    void BuyNewBuilding()
    {
        isBuyBuilding = true;

        buildingInstance = ObjectManager.Instance.buildingList[index].GetComponent<Building>();
        buildingInstance.NewBuilding();

        StoreObject nextObject = StoreManager.Instance.storeObjectList[index + 1];
        if (index != StoreManager.Instance.storeObjectList.Count - 1)
        {
            nextObject.isGetPrerequisites = true;      // ���� �ǹ��� ���������� ������Ŵ
            nextObject.Check();
        }
    }

    /// <summary>
    /// �ǹ� ���׷��̵� ��ư Ŭ�� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void BuildingButtonClick()
    {
        if (buildingInstance.Upgrade())
        {
            RefreshBuildingInfo();
        }
    }
    #endregion

    #region ��Ÿ
    /// <summary>
    /// ��Ÿ ��� ��ư Ŭ�� �� ��Ÿ ���� Ȥ�� ���׷��̵� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void SantaButtonClick()
    {
        if (GoldManager.CompareBigintAndUnit(gameManager.MyGold, SantaPrice))     // �÷��̾ ���� ������ ���׷��̵尡 ������ ��
        {
            if (!isBuySanta) BuyNewSanta();                    // ���� ���� ��Ÿ�� ���� ����
            else UpgradeSanta();                               // �� �ǹ��̸� ���׷��̵�
        }
    }

    /// <summary>
    /// ���ο� ��Ÿ ����
    /// </summary>
    void BuyNewSanta()
    {
        isBuySanta = true;

        gameManager.MyCarrots -= GoldManager.UnitToBigInteger(SantaPrice);      // ��Ÿ ��� ����

        santaInstance = ObjectManager.Instance.santaList[index].GetComponent<Santa>();
        santaInstance.NewSanta();
    }

    /// <summary>
    /// ��Ÿ�� ���׷��̵�
    /// </summary>
    void UpgradeSanta()
    {
        if (santaInstance.Upgrade())
        {
            RefreshSantaInfo();
        }
    }
    #endregion
    #endregion
}
