using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorePanel : MonoBehaviour
{
    [Header("---------- ���� ������Ʈ")]
    public GameObject storeObject;              // ������ �� ������ �ǹ� ������Ʈ

    public StoreObjectSc selectedObject;

    public Button buyBuildingButton;
    public Text selectedBuildingName;                // ������ ���� ������Ʈ�� �ǹ� �̸�
    public Text selectedBuildingPrice;               // ������ ���� ������Ʈ�� �ǹ� ����
    public Text incrementGoldText;                   // ������ ���� ������Ʈ �ǹ��� ��� ������

    public Button buySantaButton;
    public Text selectedSantaName;                  // ������ ���� ������Ʈ�� ��Ÿ �̸�
    public Text selectedSantaPrice;
    public Text incrementAmountText;

    public List<StoreObjectSc> BuildingList = new List<StoreObjectSc>();

    void Awake()    // ���� �Ŵ����� Start���� ���� ����
    {
        List<Dictionary<string, object>> data = CSVReader.Read("StoreData");       // csv ������ ���� StoreData ���� ��������

        // ������ �������� StoreObject �����ϱ�
        for (int i = 0; i < data.Count; i++)
        {
            StoreInstant(
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
    /// <param name="i">����Ʈ �ε���</param>
    void StoreInstant(string buildingName, int unlockLevel, int second, float multiplyBuildingPrice, int buildingPrice, float multiplyGold, int incrementGold, string santaName, float multiplySantaPrice, int santaPrice, string desc)
    {
        GameObject instant = GameObject.Instantiate(storeObject, storeObject.transform.position, Quaternion.identity, storeObject.transform.parent);

        // csv������ ������ copiedStoreObject�� �־���
        StoreObjectSc copiedStoreObject = instant.transform.GetComponent<StoreObjectSc>();

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
    /// ���� ����� ��ư�� �������� ��
    /// </summary>
    public void SelectStoreObject()
    {
        SetSelectedValue();
        SetButtonListner();
    }

    /// <summary>
    /// ���õ� ������Ʈ�� ������ �̸�, �̹���, ���� ���� ����
    /// </summary>
    private void SetSelectedValue()
    {
        selectedBuildingName.text = selectedObject.buildingName;
        selectedBuildingPrice.text = GoldManager.ExpressUnitOfGold(selectedObject.buildingPrice);
        incrementGoldText.text = selectedObject.incrementGold.ToString();

        selectedSantaName.text = selectedObject.santaName;
        selectedSantaPrice.text = GoldManager.ExpressUnitOfGold(selectedObject.santaPrice);
        //incrementAmountText.text = selectedObject.

    }

    /// <summary>
    /// ���õ� ������Ʈ�� ������ ��ư ������ ����
    /// </summary>
    public void SetButtonListner()
    {
        buyBuildingButton.onClick.AddListener(selectedObject.BuildingButtonClick);
        buySantaButton.onClick.AddListener(selectedObject.SantaButtonClick);
    }

    void Update()
    {
        SetButtonInteractable();
    }
}
