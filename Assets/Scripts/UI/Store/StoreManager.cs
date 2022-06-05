/**
 * @brief ������ ����
 * @author ��̼�
 * @date 22-04-26
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    #region ����
    // �̱���
    private static StoreManager instance;
    public static StoreManager Instance
    {
        get { return instance; }
    }

    [Header("---------- ���� ������Ʈ")]
    public GameObject storeObj;           // ������ �� ������ UI ������Ʈ (������)
    public GameObject storeObjParent;     // UI ������Ʈ�� �θ�

    [HideInInspector]
    public List<StoreObject> storeObjectList = new List<StoreObject>();

    // UI ��ġ
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    private float nextXPos = 690;

    // ĳ��
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
    /// �ʱ� ��ġ�� ������ ����
    /// </summary>
    void SetTransform()
    {
        rectTransform = storeObj.transform.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector3(0, 0, 0);

        parentRectTransform = storeObjParent.transform.GetComponent<RectTransform>();
        parentRectTransform.sizeDelta = new Vector2(780, 298);
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
        StoreObject copiedStoreObject = GameObject.Instantiate(storeObj, storeObjParent.transform).transform.GetComponent<StoreObject>();
        copiedStoreObject.transform.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition += new Vector2(nextXPos, 0);
        parentRectTransform.sizeDelta += new Vector2(620, 0);

        copiedStoreObject.index = i;
        copiedStoreObject.storeObject = newObject;

        // ���� ������Ʈ�� �̹��� ����
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
