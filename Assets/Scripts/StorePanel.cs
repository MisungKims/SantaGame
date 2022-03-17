using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorePanel : MonoBehaviour
{
    #region ����
    public static StorePanel Instance;

    [Header("---------- ���� ������Ʈ")]
    public GameObject storeObject;              // ������ �� ������ �ǹ� ������Ʈ

    public StoreObjectSc selectedObject;

    public Button buyBuildingButton;                 // �ǹ� ��� ��ư
    public Text selectedBuildingName;                // ������ ���� ������Ʈ�� �ǹ� �̸�
    public Text selectedBuildingPrice;               // ������ ���� ������Ʈ�� �ǹ� ����
    public Text incrementGoldText;                   // ������ ���� ������Ʈ �ǹ��� ��� ������

    public Button buySantaButton;                   // ��Ÿ ��� ��ư
    public Text selectedSantaName;                  // ������ ���� ������Ʈ�� ��Ÿ �̸�
    public Text selectedSantaPrice;                 // ������ ���� ������Ʈ�� ��Ÿ ����
    public Text incrementAmountText;                // ������ ���� ������Ʈ ��Ÿ�� ȹ�淮 ������


    private int buildingPrice;
    public int BuildingPrice
    {
        set
        {
            selectedBuildingPrice.text = GoldManager.ExpressUnitOfGold(value);
        }
    }

    private int incrementGold;
    public int IncrementGold
    {
        set
        {
            incrementGoldText.text = "+" + value.ToString();
        }
    }

    private int santaPrice;
    public int SantaPrice
    {   
        set
        {
            selectedSantaPrice.text = GoldManager.ExpressUnitOfGold(value);
        }
    }

    private float incrementAmount;
    public float IncrementAmount
    {
        set
        {
            incrementAmountText.text = value.ToString() + "%";
        }
    }


    public List<StoreObjectSc> BuildingList = new List<StoreObjectSc>();

    #endregion

    void Awake()    // ���� �Ŵ����� Start���� ���� ����
    {
        Instance = this;
        
        List<Dictionary<string, object>> data = CSVReader.Read("StoreData");       // csv ������ ���� StoreData ���� ��������

        // ������ �������� StoreObject �����ϱ�
        for (int i = 0; i < data.Count; i++)
        {
            StoreInstant(
                i,
                data[i]["�̸�"].ToString(),
                (int)data[i]["��� ���� ����"],
                (int)data[i]["��"],
                (float)data[i]["�ǹ� ���� ���"],
                (int)data[i]["�ǹ� ����"],
                 (float)data[i]["��� ���� ���"],
                (int)data[i]["��� ������"],
                data[i]["��Ÿ �̸�"].ToString(),
                (float)data[i]["��Ÿ ���� ���"],
                (int)data[i]["��Ÿ ����"],
                data[i]["Desc"].ToString());
        }

        selectedObject = BuildingList[0];
    }

    /// <summary>
    /// ���� ������Ʈ ����
    /// </summary>
    void StoreInstant(int index, string buildingName, int unlockLevel, int second, float multiplyBuildingPrice, int buildingPrice, float multiplyGold, int incrementGold, string santaName, float multiplySantaPrice, int santaPrice, string desc)
    {
        GameObject instant = GameObject.Instantiate(storeObject, storeObject.transform.position, Quaternion.identity, storeObject.transform.parent);

        // csv������ ������ copiedStoreObject�� �־���
        StoreObjectSc copiedStoreObject = instant.transform.GetComponent<StoreObjectSc>();

        copiedStoreObject.index = index;
        copiedStoreObject.buildingName = buildingName;                      // �ǹ� �̸�
        copiedStoreObject.unlockLevel = unlockLevel;                   // ��� ���� ���� ����
        copiedStoreObject.second = second;                                    // �� �� ���� ������ ������
        copiedStoreObject.multiplyBuildingPrice = multiplyBuildingPrice;       // ���׷��̵� �� �ǹ� ���� ���� ����
        copiedStoreObject.buildingPrice = buildingPrice;                      // �ǹ� ���� 
        copiedStoreObject.multiplyGold = multiplyGold;                // ���׷��̵� �� �÷��̾� �� ���� ����
        copiedStoreObject.incrementGold = incrementGold;                    // �÷��̾��� �� ������
        copiedStoreObject.santaName = santaName;                    // ��Ÿ �̸�
        copiedStoreObject.multiplySantaPrice = multiplySantaPrice;          // ���׷��̵� �� �ǹ� ���� ���� ����
        copiedStoreObject.santaPrice = santaPrice;                         // �ǹ� ���� 
        copiedStoreObject.desc = desc;                              // �ǹ��� ����

        copiedStoreObject.gameObject.SetActive(true);
        copiedStoreObject.gameObject.name = buildingName;

        BuildingList.Add(copiedStoreObject);


    }

    /// <summary>
    /// ��ư�� Interactable ����
    /// </summary>
    void SetButtonInteractable()
    {
        if (GameManager.Instance.MyGold >= selectedObject.buildingPrice)        // �÷��̾��� ������ ��� ���� ���� �������� ũ�� ���� ���� �ǹ��� ���ݺ��� Ŭ ��
            buyBuildingButton.interactable = true;                                             //  Interactable�� True�� ����
        else buyBuildingButton.interactable = false;

        if (GameManager.Instance.MyGold >= selectedObject.santaPrice)        // �÷��̾��� ������ ��� ���� ���� �������� ũ�� ���� ���� �ǹ��� ���ݺ��� Ŭ ��
            buySantaButton.interactable = true;                                             //  Interactable�� True�� ����
        else buySantaButton.interactable = false;

    }

    private void Start()
    {
        SelectStoreObject();
    }

    /// <summary>
    /// ���� ����� ��ư�� �������� �� ���õ� ������Ʈ�� ������ �̸�, �̹���, ���� ���� ����
    /// </summary>
    public void SelectStoreObject()
    {
        selectedBuildingName.text = selectedObject.buildingName;
        BuildingPrice = selectedObject.buildingPrice;
        IncrementGold = selectedObject.incrementGold;

        selectedSantaName.text = selectedObject.santaName;
        SantaPrice = selectedObject.santaPrice;
        IncrementAmount = selectedObject.amountObtained;
    }

    // ���� ���׷��̵� ��ư Ŭ�� ��
    public void BuildingUpgradeButton()
    {
        selectedObject.BuildingButtonClick();
    }

    // ��Ÿ ���׷��̵� ��ư Ŭ�� ��
    public void SantaUpgradeButton()
    {
        selectedObject.SantaButtonClick();
    }


    void Update()
    {
        SetButtonInteractable();
    }
}
