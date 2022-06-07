/**
 * @details CSV ������ ������ ���� �߼�
 * @author ��̼�
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#region ����ü
[System.Serializable]
public class PostStruct
{
    public string name;
    public string content;
    public int giftIndex;
    public bool isRead;

    public PostStruct(string name, string content, int giftIndex, bool isRead)
    {
        this.name = name;
        this.content = content;
        this.giftIndex = giftIndex;
        this.isRead = isRead;
    }
}
#endregion

public class PostOfficeManager : MonoBehaviour
{
    #region ����
    // �̱���
    private static PostOfficeManager instance;
    public static PostOfficeManager Instance
    {
        get { return instance; }
    }

    // CSV ���Ͽ��� ������ ��ü ���� ����Ʈ
    private List<PostStruct> postList = new List<PostStruct>();

    public List<PostStruct> havePostList = new List<PostStruct>();  // ��ü�뿡 �ִ� ���� ����Ʈ

    // ���� UI ����
    [SerializeField]
    private GameObject postObj;         // ������ ���� UI (������)
    [SerializeField]
    public GameObject parent;          // ���� ������Ʈ�� �θ�
    public WritingPad writingPad;


    private List<PostObject> postUIList = new List<PostObject>();    // ���� UI ����Ʈ
    public List<Vector2> UITransformList = new List<Vector2>();      // ���� UI�� ��ġ ����Ʈ
    public List<Vector2> parentSizeList = new List<Vector2>();       // �θ�(��ũ�Ѻ��� content)�� ũ�� ����Ʈ

    // UI ��ġ
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    private float nextYPos = -90;

   
    public int maximum;        // ������ �ƽø�

    // ĳ��
    private UIManager uIManager;
    private Inventory inventory;
    private ObjectPoolingManager objectPoolingManager;

    //���� Ȱ��ȭ ���¸� �����ϴ� ����
    bool isPaused = false;
    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        uIManager = UIManager.Instance;
        inventory = Inventory.Instance;
        objectPoolingManager = ObjectPoolingManager.Instance;


        SetTransform();

        ReadCSV();

        LoadData();
    }

    private void Start()
    {
        OfflineTime();

        maximum = objectPoolingManager.poolingList[(int)EObjectFlag.post].initCount;

        StartCoroutine(SendPost());
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            isPaused = true;

            SaveData();         // ���� ��Ȱ��ȭ�Ǿ��� �� ������ ����
        }

        else
        {
            if (isPaused)
            {
                isPaused = false;
                OfflineTime();
            }
        }
    }

    void OnApplicationQuit()
    {
        SaveData();         // �� ���� �� ������ ����
    }
    #endregion

    #region �ڷ�ƾ
    /// <summary>
    /// �Ѵ޸��� ������ ���� �߼�
    /// </summary>
    IEnumerator SendPost()
    {
        while (true)
        {
            NewPost();

            yield return new WaitForSeconds(GameManager.Instance.dayCount * (GameManager.Instance.lastDay - 1));        // ���� ���� �� ������ ���� ����
        }
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// �ʱ� ��ġ�� ������ ����
    /// </summary>
    void SetTransform()
    {
        // UI ������Ʈ�� ������ ���� ��ġ���� ����Ʈ�� �ֱ�
        rectTransform = postObj.transform.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector2.zero;

        Vector2 tempPos = Vector2.zero;
        tempPos.y = nextYPos;

        for (int i = 0; i < maximum; i++)
        {
            UITransformList.Add(rectTransform.anchoredPosition);
            rectTransform.anchoredPosition += tempPos;
        }

        // UI�� ������ ���� �θ��� ũ�⸦ ����Ʈ�� �ֱ�
        parentRectTransform = parent.transform.GetComponent<RectTransform>();
        parentRectTransform.sizeDelta = new Vector2(0, 80);

        Vector2 tempSize = Vector2.zero;
        tempSize.y = rectTransform.sizeDelta.y;

        Vector2 size = parentRectTransform.sizeDelta;
        for (int i = 0; i < maximum; i++)
        {
            parentSizeList.Add(size);
            size += tempSize;
        }
    }

    /// <summary>
    /// csv ������ ���� ���� ��������
    /// </summary>
    void ReadCSV()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("PostOfficeData");

        for (int i = 0; i < data.Count; i++)
        {
            PostStruct newPost = new PostStruct
            (data[i]["����"].ToString(),
            data[i]["����"].ToString(),
            (int)data[i]["���� �ε���"],
            false);
           
            postList.Add(newPost);
        }
    }

    public void NewPost()
    {
        int randIndex = Random.Range(0, postList.Count);        // �������� ���� ������ ����
        if (postUIList.Count < maximum)          // �������� ���� �ʾ��� ���� ����
        {
            PostStruct postStruct = new PostStruct(
                postList[randIndex].name,
                postList[randIndex].content,
                postList[randIndex].giftIndex,
                false);

            havePostList.Add(postStruct);

            PostOfficeInstance(postStruct);
        }
    }    

    /// <summary>
    /// ���� �ν��Ͻ� ����
    /// </summary>
    void PostOfficeInstance(PostStruct post)
    {
        PostObject newObj = objectPoolingManager.Get(EObjectFlag.post).GetComponent<PostObject>();
        
        newObj.PostName = post.name;
        newObj.PostConent = post.content;
        newObj.transform.GetComponent<RectTransform>().anchoredPosition = UITransformList[0];       // ���ο� ������ �� ������� ������

        Gift gift = GiftManager.Instance.giftList[post.giftIndex];
        newObj.gift = gift;

        newObj.index = postUIList.Count;
        for (int i = 0; i < postUIList.Count; i++)          // ������ ������ ������ ������ UI�� �ٽ� ��ġ
        {
            postUIList[i].RefreshTransform(UITransformList[postUIList.Count - i]);
        }

        parentRectTransform.sizeDelta = parentSizeList[postUIList.Count];

        newObj.RefreshNotification();

        postUIList.Add(newObj);
       
        // �κ��丮�� �����ְ�, ���ø���Ʈ�� �߰��� ������ �κ��丮�� �־����� �κ��丮 ���ΰ�ħ
        if (UIManagerInstance().inventoryPanel.activeSelf && gift.giftInfo.inventoryIndex > -1)
        {
            InventoryInstance().RefreshInventory();
        }
    }

    /// <summary>
    /// Post Object ���� ��
    /// </summary>
    /// <param name="index"></param>
    public void Refresh(int index)
    {
        objectPoolingManager.Set(postUIList[index].gameObject, EObjectFlag.post);        // ������Ʈ Ǯ�� ������

        postUIList.RemoveAt(index);
        havePostList.RemoveAt(index);

        // UI ��ġ�� �ٽ�
        for (int i = 0; i < postUIList.Count; i++)
        {
            postUIList[i].index = i;
            postUIList[i].RefreshTransform(UITransformList[postUIList.Count - 1 - i]);
        }

        if (postUIList.Count != 0)
        {
            parentRectTransform.sizeDelta = parentSizeList[postUIList.Count - 1];
        }
    }

    /// <summary>
    /// ������ ����
    /// </summary>
    void SaveData()
    {
        string jdata = JsonUtility.ToJson(new Serialization<PostStruct>(havePostList));
        File.WriteAllText(Application.persistentDataPath + "/PostOfficeListData.json", jdata);
    }

    /// <summary>
    /// ������ �ε�
    /// </summary>
    /// <returns>�ҷ����� ���� ����</returns>
    public bool LoadData()
    {
        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/PostOfficeListData.json");
        if (fileInfo.Exists)
        {
            string jdata = File.ReadAllText(Application.persistentDataPath + "/PostOfficeListData.json");

            havePostList = JsonUtility.FromJson<Serialization<PostStruct>>(jdata).target;
            for (int i = 0; i < havePostList.Count; i++)
            {
                PostOfficeInstance(havePostList[i]);
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// �������� �ð����� ������ �ð����� �޾��� ������ �޵���
    /// </summary>
    void OfflineTime()
    {
        int goneMonth = GameManager.Instance.goneMonth;
        for (int i = 0; i < goneMonth; i++)
        {
            NewPost();
        }
    }

    /// <summary>
    /// UIManager �ν��Ͻ� ��ȯ
    /// </summary>
    /// <returns></returns>
    UIManager UIManagerInstance()
    {
        if (!uIManager)
        {
            uIManager = UIManager.Instance;
        }

        return uIManager;
    }

    /// <summary>
    /// Inventory �ν��Ͻ� ��ȯ
    /// </summary>
    /// <returns></returns>
    Inventory InventoryInstance()
    {
        if (!inventory)
        {
            inventory = Inventory.Instance;
        }

        return inventory;
    }
    #endregion
}
