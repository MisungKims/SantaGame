/**
 * @brief 상점을 관리
 * @author 김미성
 * @date 22-04-19
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    #region 변수
    public static StoreManager instance;

    [Header("---------- 상점 오브젝트")]
    public GameObject storeObject;           // 복제가 될 상점의 UI 오브젝트 (프리팹)
    public GameObject storeObjectParent;     // UI 오브젝트의 부모

    [HideInInspector]
    public List<StoreObject> storeObjectList = new List<StoreObject>();

    // UI 배치
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    private float nextXPos = 410;

    // 캐싱
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
    /// 초기 위치와 사이즈 설정
    /// </summary>
    void SetTransform()
    {
        rectTransform = storeObject.transform.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector3(206, 300, 0);

        parentRectTransform = storeObjectParent.transform.GetComponent<RectTransform>();
        parentRectTransform.sizeDelta = new Vector2(513, 298);
    }

    /// <summary>
    /// 오브젝트 매니저의 리스트를 가져와 상점 UI 생성
    /// </summary>
   void GetObject()
    {
        for (int i = 0; i < objectManager.objectList.Count; i++)
        {
            StoreInstance(i, objectManager.objectList[i]);
        }
    }

    /// <summary>
    /// 상점 인스턴스를 생성하여 UI 배치
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
    ///// 상점 오브젝트 생성
    ///// </summary>
    //void StoreInstant(int index, string buildingName, int unlockLevel, int second, float multiplyBuildingPrice, string buildingPrice, string incrementGold, string santaName, float multiplySantaPrice, string santaPrice, int efficiency, string desc)
    //{
    //    // csv파일의 내용을 copiedStoreObject에 넣어줌

    //    StoreObjectSc copiedStoreObject = GameObject.Instantiate(storeObject, storeObjectParent.transform).transform.GetComponent<StoreObjectSc>();
    //    copiedStoreObject.transform.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;
    //    rectTransform.anchoredPosition += new Vector2(nextXPos, 0);
    //    parentRectTransform.sizeDelta += new Vector2(375, 0);

    //    copiedStoreObject.index = index;
    //    copiedStoreObject.BuildingName = buildingName;                      // 건물 이름
    //    copiedStoreObject.UnlockLevel = unlockLevel;                        // 잠금 해제 가능 레벨
    //    copiedStoreObject.Second = second;                                    // 몇 초 마다 증가할 것인지
    //    copiedStoreObject.multiplyBuildingPrice = multiplyBuildingPrice;       // 업그레이드 후 건물 가격 증가 배율
    //    copiedStoreObject.BuildingPrice = buildingPrice;                      // 건물 가격 
    //    copiedStoreObject.IncrementGold = incrementGold;                    // 플레이어의 돈 증가량
    //    copiedStoreObject.SantaName = santaName;                    // 산타 이름
    //    copiedStoreObject.multiplySantaPrice = multiplySantaPrice;          // 업그레이드 후 건물 가격 증가 배율
    //    copiedStoreObject.SantaPrice = santaPrice;                         // 건물 가격 
    //    copiedStoreObject.SantaEfficiency = efficiency;
    //    copiedStoreObject.Desc = desc;                              // 건물의 설명

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
