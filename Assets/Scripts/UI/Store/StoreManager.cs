/**
 * @brief 상점을 관리
 * @author 김미성
 * @date 22-04-26
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        copiedStoreObject.buildingImage.sprite = newObject.buildingSprite;
        copiedStoreObject.santaImage.sprite = newObject.santaSprite;

        //RectTransform objRect = copiedStoreObject.buildingImage.transform.GetComponent<RectTransform>();
        //RectTransform ImgRect = buildingImages[i].transform.GetComponent<RectTransform>();
       // objRect.sizeDelta = ImgRect.sizeDelta;

        

       // objRect = copiedStoreObject.santaImage.transform.GetComponent<RectTransform>();
       // ImgRect = santaImages[i].transform.GetComponent<RectTransform>();
       // objRect.sizeDelta = ImgRect.sizeDelta;


        storeObjectList.Add(copiedStoreObject);
    }
}
