/**
 * @brief ������ ����
 * @author ��̼�
 * @date 22-04-19
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    #region ����
    public static StoreManager instance;

    [Header("---------- ���� ������Ʈ")]
    public GameObject storeObject;           // ������ �� ������ UI ������Ʈ (������)
    public GameObject storeObjectParent;     // UI ������Ʈ�� �θ�

    [HideInInspector]
    public List<StoreObject> storeObjectList = new List<StoreObject>();

    // UI ��ġ
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    private float nextXPos = 410;

    // ĳ��
    private ObjectManager objectManager;

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

        objectManager = ObjectManager.Instance;

        SetTransform();

        GetObject();
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
    /// ������Ʈ �Ŵ����� ����Ʈ�� ������ ���� UI ����
    /// </summary>
   void GetObject()
    {
        for (int i = 0; i < objectManager.objectList.Count; i++)
        {
            StoreInstance(i, objectManager.objectList[i]);
        }
    }

    /// <summary>
    /// ���� �ν��Ͻ��� �����Ͽ� UI ��ġ
    /// </summary>
    void StoreInstance(int i, Object newObject)
    {
        StoreObject copiedStoreObject = GameObject.Instantiate(storeObject, storeObjectParent.transform).transform.GetComponent<StoreObject>();
        copiedStoreObject.transform.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition += new Vector2(nextXPos, 0);
        parentRectTransform.sizeDelta += new Vector2(375, 0);

        copiedStoreObject.index = i;
        copiedStoreObject.storeObject = newObject;

        storeObjectList.Add(copiedStoreObject);
    }

    ///// <summary>
    ///// ���� ������Ʈ ����
    ///// </summary>
    //void StoreInstant(int index, string buildingName, int unlockLevel, int second, float multiplyBuildingPrice, string buildingPrice, string incrementGold, string santaName, float multiplySantaPrice, string santaPrice, int efficiency, string desc)
    //{
    //    // csv������ ������ copiedStoreObject�� �־���

    //    StoreObjectSc copiedStoreObject = GameObject.Instantiate(storeObject, storeObjectParent.transform).transform.GetComponent<StoreObjectSc>();
    //    copiedStoreObject.transform.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;
    //    rectTransform.anchoredPosition += new Vector2(nextXPos, 0);
    //    parentRectTransform.sizeDelta += new Vector2(375, 0);

    //    copiedStoreObject.index = index;
    //    copiedStoreObject.BuildingName = buildingName;                      // �ǹ� �̸�
    //    copiedStoreObject.UnlockLevel = unlockLevel;                        // ��� ���� ���� ����
    //    copiedStoreObject.Second = second;                                    // �� �� ���� ������ ������
    //    copiedStoreObject.multiplyBuildingPrice = multiplyBuildingPrice;       // ���׷��̵� �� �ǹ� ���� ���� ����
    //    copiedStoreObject.BuildingPrice = buildingPrice;                      // �ǹ� ���� 
    //    copiedStoreObject.IncrementGold = incrementGold;                    // �÷��̾��� �� ������
    //    copiedStoreObject.SantaName = santaName;                    // ��Ÿ �̸�
    //    copiedStoreObject.multiplySantaPrice = multiplySantaPrice;          // ���׷��̵� �� �ǹ� ���� ���� ����
    //    copiedStoreObject.SantaPrice = santaPrice;                         // �ǹ� ���� 
    //    copiedStoreObject.SantaEfficiency = efficiency;
    //    copiedStoreObject.Desc = desc;                              // �ǹ��� ����

    //    copiedStoreObject.gameObject.SetActive(true);
    //    copiedStoreObject.gameObject.name = buildingName;

    //    copiedStoreObject.Prerequisites = prerequisites;
    //    prerequisites = buildingName;

    //    //if (prerequisites == null)
    //    //{

    //    //}
    //    //else
    //    //{
    //    //    copiedStoreObject.Prerequisites = prerequisites;
    //    //    prerequisites = buildingName;
    //    //}

    //    ObjectList.Add(copiedStoreObject);
    //}
}
