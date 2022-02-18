using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorePanel : MonoBehaviour
{
    [Header("---------- ���� ������Ʈ")]
    public Text goldTextStore;                  // ��带 ��Ÿ���� �ؽ�Ʈ
    public GameObject storeObject;              // ������ �� ������ �ǹ� ������Ʈ

    public StoreObjectSc selectedObject;

    public Button buyBuildingButton;
    public Text selectedBuildingName;                // ������ ���� ������Ʈ�� �ǹ� �̸�
    public Text selectedBuildingPrice;

    public Button buySantaButton;
    public Text selectedSantaName;                // ������ ���� ������Ʈ�� ��Ÿ �̸�
    public Text selectedSantaPrice;

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

    void SetButtonInteractable()
    {
        if (GameManager.myGold >= selectedObject.buildingPrice)        // �÷��̾��� ������ ��� ���� ���� �������� ũ�� ���� ���� �ǹ��� ���ݺ��� Ŭ ��
            buyBuildingButton.interactable = true;                                             //  Interactable�� True�� ����
        else buyBuildingButton.interactable = false;

        if (GameManager.myGold >= selectedObject.santaPrice)        // �÷��̾��� ������ ��� ���� ���� �������� ũ�� ���� ���� �ǹ��� ���ݺ��� Ŭ ��
            buySantaButton.interactable = true;                                             //  Interactable�� True�� ����
        else buySantaButton.interactable = false;

    }

    private void Start()
    {
        buyBuildingButton.onClick.AddListener(selectedObject.BuildingButtonClick);
        buySantaButton.onClick.AddListener(selectedObject.SantaButtonClick);
    }

    void Update()
    {
        goldTextStore.text = GameManager.myGold.ToString();

        selectedBuildingName.text = selectedObject.buildingName;
        selectedBuildingPrice.text = selectedObject.buildingPrice.ToString();
        
        selectedSantaName.text = selectedObject.santaName;
        selectedSantaPrice.text = selectedObject.santaPrice.ToString();
        
        SetButtonInteractable();
    }
}
