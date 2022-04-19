/**
 * @details ������ UI�� ����
 * @author ��̼�
 * @date 22-04-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorePanel : MonoBehaviour
{
    #region ����
    public static StorePanel instance;

    [Header("---------- ���� ������Ʈ")]
    public GameObject storeObject;           // ������ �� ������ �ǹ� ������Ʈ
    public GameObject storeObjectParent;     // �ǹ� ������Ʈ�� �θ�

    [HideInInspector]
    public List<StoreObjectSc> ObjectList = new List<StoreObjectSc>();

    private string prerequisites = null;    // ���� �����ؾ� �� �ǹ�

    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    private float nextXPos = 410;

    #endregion

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }

        SetTransform();

        ReadCSV();
    }

    /// <summary>
    /// �ʱ� ��ġ�� ������ ����
    /// </summary>
    void SetTransform()
    {
        rectTransform = storeObject.transform.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector3(206, 300, 0);

        parentRectTransform = storeObjectParent.transform.GetComponent<RectTransform>();
        parentRectTransform.sizeDelta = new Vector2(513, 298);
    }

    /// <summary>
    /// csv ������ ���� StoreData ���� ��������
    /// </summary>
    void ReadCSV()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("StoreData");

        for (int i = 0; i < data.Count; i++)         // ������ �������� StoreObject ����
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
    }

    /// <summary>
    /// ���� ������Ʈ ����
    /// </summary>
    void StoreInstant(int index, string buildingName, int unlockLevel, int second, float multiplyBuildingPrice, string buildingPrice, string incrementGold, string santaName, float multiplySantaPrice, string santaPrice, int efficiency, string desc)
    {
        // csv������ ������ copiedStoreObject�� �־���

        StoreObjectSc copiedStoreObject = GameObject.Instantiate(storeObject, storeObjectParent.transform).transform.GetComponent<StoreObjectSc>();
        copiedStoreObject.transform.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition += new Vector2(nextXPos, 0);
        parentRectTransform.sizeDelta += new Vector2(375, 0);

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

        copiedStoreObject.Prerequisites = prerequisites;
        prerequisites = buildingName;

        //if (prerequisites == null)
        //{
          
        //}
        //else
        //{
        //    copiedStoreObject.Prerequisites = prerequisites;
        //    prerequisites = buildingName;
        //}

        ObjectList.Add(copiedStoreObject);
    }

}
