/**
 * @brief 상점을 관리
 * @author 김미성
 * @date 22-04-26
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    #region 변수
    // 싱글톤
    private static StoreManager instance;
    public static StoreManager Instance
    {
        get { return instance; }
    }

    [Header("---------- 상점 오브젝트")]
    public GameObject storeObj;           // 복제가 될 상점의 UI 오브젝트 (프리팹)
    public GameObject storeObjParent;     // UI 오브젝트의 부모

    [HideInInspector]
    public List<StoreObject> storeObjectList = new List<StoreObject>();


    // UI 배치
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    private float nextXPos = 690;

    // 캐싱
    private ObjectManager objectManager;

    #endregion

    #region 유니티 함수
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
        }

        objectManager = ObjectManager.Instance;

        SetTransform();

        GetObject();
    }
    #endregion

    #region 함수
    /// <summary>
    /// 초기 위치와 사이즈 설정
    /// </summary>
    void SetTransform()
    {
        rectTransform = storeObj.transform.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector3(0, 0, 0);

        parentRectTransform = storeObjParent.transform.GetComponent<RectTransform>();
        parentRectTransform.sizeDelta = new Vector2(780, 298);
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
        StoreObject copiedStoreObject = GameObject.Instantiate(storeObj, storeObjParent.transform).transform.GetComponent<StoreObject>();
        copiedStoreObject.transform.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition += new Vector2(nextXPos, 0);
        parentRectTransform.sizeDelta += new Vector2(620, 0);

        copiedStoreObject.index = i;
        copiedStoreObject.storeObject = newObject;

        // 상점 오브젝트의 이미지 설정
        copiedStoreObject.buildingImage.sprite = newObject.buildingSprite;      //오류 이렇게 하지말기 (오브젝트 매니저에서 가져오기)
        copiedStoreObject.santaImage.sprite = newObject.santaSprite;

        if (newObject.buildingLevel > 0)
        {
            copiedStoreObject.isBuyBuilding = true;
        }

        if (newObject.santaLevel > 0)
        {
            copiedStoreObject.isBuySanta = true;
        }

        storeObjectList.Add(copiedStoreObject);
    }
    #endregion
}
