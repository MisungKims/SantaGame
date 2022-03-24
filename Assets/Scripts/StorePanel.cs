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

    //[Header("-------------------- �ǹ�")]
    //[SerializeField]
    //private Button buyBuildingButton;                 // �ǹ� ��� ��ư
    //[SerializeField]
    //private Text selectedBuildingName;                // ������ ���� ������Ʈ�� �ǹ� �̸�
    //[SerializeField]
    //private Text selectedBuildingLevel;                // ������ ���� ������Ʈ�� �ǹ� ����
    //[SerializeField]
    //private Text selectedBuildingPrice;               // ������ ���� ������Ʈ�� �ǹ� ����
    //[SerializeField]
    //private Text incrementGoldText;                   // ������ ���� ������Ʈ �ǹ��� ��� ������
    //[SerializeField]
    //private GameObject buildingImageGroup;            // �ǹ��� �̹��� �׷�

    //private GameObject buildingImg;

    //[Header("-------------------- ��Ÿ")]
    //[SerializeField]
    //private Button buySantaButton;                   // ��Ÿ ��� ��ư
    //[SerializeField]
    //private Text selectedSantaName;                  // ������ ���� ������Ʈ�� ��Ÿ �̸�
    //[SerializeField]
    //private Text selectedSantaLevel;                  // ������ ���� ������Ʈ�� ��Ÿ ����
    //[SerializeField]
    //private Text selectedSantaPrice;                 // ������ ���� ������Ʈ�� ��Ÿ ����
    //[SerializeField]
    //private Text santaEfficiencyText;                // ������ ���� ������Ʈ ��Ÿ�� �˹� ȿ�� ����
    //[SerializeField]
    //private GameObject santaImageGroup;            // ��Ÿ�� �̹��� �׷�

    //private GameObject santaImg;

    //[Header("-------------------- ����")]
    //[SerializeField]
    //private Text incrementAmountText;                   // ������ ������Ʈ�� �� ��� ������
    //[SerializeField]
    //private Text secondText;                            // ������ ������Ʈ�� ��
   

    [HideInInspector]
    public StoreObjectSc selectedObject;

    StringBuilder goldSb = new StringBuilder();
    StringBuilder amountSb = new StringBuilder();

    //private string buildingPrice;
    //public string BuildingPrice
    //{
    //    set
    //    {
    //        selectedBuildingPrice.text = value;
    //    }
    //}

    //private string santaPrice;
    //public string SantaPrice
    //{
    //    set
    //    {
    //        selectedSantaPrice.text = value;
    //    }
    //}


    //private string incrementGold;
    //public string IncrementGold
    //{
    //    set
    //    {
    //        goldSb.Clear();
    //        goldSb.Append("+");
    //        goldSb.Append(value);
    //        incrementGoldText.text = goldSb.ToString();

    //        incrementAmountText.text = value;
    //    }
    //}

    //private float santaEfficiency;
    //public float SantaEfficiency
    //{
    //    set
    //    {
    //        santaEfficiency = value;

    //        amountSb.Clear();
    //        amountSb.Append(santaEfficiency);
    //        amountSb.Append("%");

    //        santaEfficiencyText.text = amountSb.ToString();
    //    }
    //}

    //private int second;
    //public int Second
    //{
    //    set
    //    {
    //        secondText.text = value.ToString();
    //    }
    //}

    //private int buildingLevel;
    //public int BuildingLevel
    //{
    //    set
    //    {
    //        selectedBuildingLevel.text = string.Format("LV. {0}", value);
    //    }
    //}

    //private int santaLevel;
    //public int SantaLevel
    //{
    //    set
    //    {
    //        selectedSantaLevel.text = string.Format("LV. {0}", value);
    //    }
    //}


    public List<StoreObjectSc> ObjectList = new List<StoreObjectSc>();

    // ĳ��
    private GameManager gameManager;

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
                 data[i]["�ǹ� ����"].ToString(),
                 data[i]["��� ������"].ToString(),
                 data[i]["��Ÿ �̸�"].ToString(),
                 (int)data[i]["��Ÿ ���� ���"],
                 data[i]["��Ÿ ����"].ToString(),
                 (int)data[i]["�˹� ȿ�� ����"],
                 data[i]["Desc"].ToString()
                 );
        }

        selectedObject = ObjectList[0];
    }

    /// <summary>
    /// ���� ������Ʈ ����
    /// </summary>
    void StoreInstant(int index, string buildingName, int unlockLevel, int second, float multiplyBuildingPrice, string buildingPrice, string incrementGold, string santaName, float multiplySantaPrice, string santaPrice, int efficiency, string desc)
    {
        GameObject instant = GameObject.Instantiate(storeObject, storeObject.transform.position, Quaternion.identity, storeObject.transform.parent);

        // csv������ ������ copiedStoreObject�� �־���
        StoreObjectSc copiedStoreObject = instant.transform.GetComponent<StoreObjectSc>();

        copiedStoreObject.index = index;
        copiedStoreObject.BuildingName = buildingName;                      // �ǹ� �̸�
        copiedStoreObject.UnlockLevel = unlockLevel;                        // ��� ���� ���� ����
        copiedStoreObject.Second = second;                                    // �� �� ���� ������ ������
        copiedStoreObject.multiplyBuildingPrice = multiplyBuildingPrice;       // ���׷��̵� �� �ǹ� ���� ���� ����
        copiedStoreObject.BuildingPrice = buildingPrice;                      // �ǹ� ���� 
        copiedStoreObject.IncrementGold = incrementGold;                    // �÷��̾��� �� ������
        copiedStoreObject.SantaName = santaName;                    // ��Ÿ �̸�
        copiedStoreObject.multiplySantaPrice = multiplySantaPrice;          // ���׷��̵� �� �ǹ� ���� ���� ����
        copiedStoreObject.SantaPrice = santaPrice;                         // �ǹ� ���� 
        copiedStoreObject.SantaEfficiency = efficiency;
        copiedStoreObject.Desc = desc;                              // �ǹ��� ����

        copiedStoreObject.gameObject.SetActive(true);
        copiedStoreObject.gameObject.name = buildingName;

        ObjectList.Add(copiedStoreObject);
    }

    // ��ư�� Interactable ����
    //void SetButtonInteractable()
    //{
    //    //���� ���� �ǹ��� ���ݺ��� Ŭ ��
    //    if (GoldManager.CompareBigintAndUnit(gameManager.MyGold, selectedObject.BuildingPrice))
    //        buyBuildingButton.interactable = true;                //  Interactable�� True�� ����
    //    else buyBuildingButton.interactable = false;

    //    // ��Ÿ ������Ʈ�� �ǹ��� ���� ��, ���� ���� ��Ÿ�� ���ݺ��� Ŭ ��
    //    if (selectedObject.isBuyBuilding && GoldManager.CompareBigintAndUnit(gameManager.MyGold, selectedObject.santaPrice))
    //        buySantaButton.interactable = true;                 //  Interactable�� True�� ����
    //    else buySantaButton.interactable = false;
    //}

    //// ���� ����� ��ư�� �������� �� ���õ� ������Ʈ�� ������ �̸�, �̹���, ���� ���� ����
    //public void SelectStoreObject()
    //{
    //    if (buildingImg)
    //        buildingImg.SetActive(false);
    //    if (santaImg)
    //        santaImg.SetActive(false);

    //    selectedBuildingName.text = selectedObject.buildingName;
    //    BuildingLevel = selectedObject.BuildingLevel;
    //    BuildingPrice = selectedObject.BuildingPrice;
    //    buildingImg = buildingImageGroup.transform.GetChild(selectedObject.index).gameObject;
    //    buildingImg.SetActive(true);

    //    selectedSantaName.text = selectedObject.santaName;
    //    santaLevel = selectedObject.SantaLevel;
    //    SantaPrice = selectedObject.santaPrice;
    //    santaImg = santaImageGroup.transform.GetChild(selectedObject.index).gameObject;
    //    santaImg.SetActive(true);

    //    SantaEfficiency = selectedObject.santaEfficiency;
    //    IncrementGold = selectedObject.IncrementGold;
    //    second = selectedObject.second;
    //}

    //// ���� ���׷��̵� ��ư Ŭ�� ��
    //public void BuildingUpgradeButton()
    //{
    //    selectedObject.BuildingButtonClick();
    //}

    //// ��Ÿ ���׷��̵� ��ư Ŭ�� ��
    //public void SantaUpgradeButton()
    //{
    //    selectedObject.SantaButtonClick();
    //}
    private void Start()
    {
       // SelectStoreObject();

        gameManager = GameManager.Instance;
    }

    //void Update()
    //{
    //    SetButtonInteractable();
    //}

    //private void OnEnable()
    //{
    //    selectedObject = ObjectList[0];

    //    SelectStoreObject();
    //}
}
