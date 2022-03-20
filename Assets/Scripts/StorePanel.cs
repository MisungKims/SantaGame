using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class StorePanel : MonoBehaviour
{
    #region ����
    public static StorePanel Instance;

    [Header("---------- ���� ������Ʈ")]
    public GameObject storeObject;              // ������ �� ������ �ǹ� ������Ʈ

    [Header("-------------------- �ǹ�")]
    [SerializeField]
    private Button buyBuildingButton;                 // �ǹ� ��� ��ư
    [SerializeField]
    private Text selectedBuildingName;                // ������ ���� ������Ʈ�� �ǹ� �̸�
    [SerializeField]
    private Text selectedBuildingPrice;               // ������ ���� ������Ʈ�� �ǹ� ����
    [SerializeField]
    private Text incrementGoldText;                   // ������ ���� ������Ʈ �ǹ��� ��� ������
    [SerializeField]
    private GameObject buildingImageGroup;            // �ǹ��� �̹��� �׷�

    private GameObject buildingImg;

    [Header("-------------------- ��Ÿ")]
    [SerializeField]
    private Button buySantaButton;                   // ��Ÿ ��� ��ư
    [SerializeField]
    private Text selectedSantaName;                  // ������ ���� ������Ʈ�� ��Ÿ �̸�
    [SerializeField]
    private Text selectedSantaPrice;                 // ������ ���� ������Ʈ�� ��Ÿ ����
    [SerializeField]
    private Text incrementAmountText;                // ������ ���� ������Ʈ ��Ÿ�� ȹ�淮 ������
    [SerializeField]
    private GameObject santaImageGroup;            // ��Ÿ�� �̹��� �׷�

    private GameObject santaImg;

    [HideInInspector]
    public StoreObjectSc selectedObject;

    StringBuilder goldSb = new StringBuilder();
    StringBuilder amountSb = new StringBuilder();

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
            goldSb.Clear();
            goldSb.Append("+");
            goldSb.Append(GoldManager.ExpressUnitOfGold(value));
            incrementGoldText.text = goldSb.ToString();
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
            amountSb.Clear();
            amountSb.Append(value);
            amountSb.Append("%");

            incrementAmountText.text = amountSb.ToString();
        }
    }


    public List<StoreObjectSc> ObjectList = new List<StoreObjectSc>();

    // ĳ��
    private GameManager gameManagerInstance;

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

        selectedObject = ObjectList[0];
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

        ObjectList.Add(copiedStoreObject);
    }

    // ��ư�� Interactable ����
    void SetButtonInteractable()
    {
        // ���� ���� �ǹ��� ���ݺ��� Ŭ ��
        if (gameManagerInstance.MyGold >= selectedObject.buildingPrice)        
            buyBuildingButton.interactable = true;                //  Interactable�� True�� ����
        else buyBuildingButton.interactable = false;

        // ��Ÿ ������Ʈ�� �ǹ��� ���� ��, ���� ���� ��Ÿ�� ���ݺ��� Ŭ ��
        if (selectedObject.isBuyBuilding && gameManagerInstance.MyGold >= selectedObject.santaPrice)
            buySantaButton.interactable = true;                 //  Interactable�� True�� ����
        else buySantaButton.interactable = false;
    }

    // ���� ����� ��ư�� �������� �� ���õ� ������Ʈ�� ������ �̸�, �̹���, ���� ���� ����
    public void SelectStoreObject()
    {
        if (buildingImg)
            buildingImg.SetActive(false);
        if (santaImg)
            santaImg.SetActive(false);

        selectedBuildingName.text = selectedObject.buildingName;
        BuildingPrice = selectedObject.buildingPrice;
        IncrementGold = selectedObject.incrementGold;
        buildingImg = buildingImageGroup.transform.GetChild(selectedObject.index).gameObject;
        buildingImg.SetActive(true);

        selectedSantaName.text = selectedObject.santaName;
        SantaPrice = selectedObject.santaPrice;
        IncrementAmount = selectedObject.amountObtained;
        santaImg = santaImageGroup.transform.GetChild(selectedObject.index).gameObject;
        santaImg.SetActive(true);
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
    private void Start()
    {
        SelectStoreObject();

        gameManagerInstance = GameManager.Instance;
    }

    void Update()
    {
        SetButtonInteractable();
    }

    private void OnEnable()
    {
        selectedObject = ObjectList[0];

        SelectStoreObject();
    }
}
