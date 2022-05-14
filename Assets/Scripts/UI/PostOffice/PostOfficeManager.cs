/**
 * @details CSV ������ ������ ���� �߼�
 * @author ��̼�
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// TODO : �ν����Ϳ��� ��ü������ �̵��ϱ�

#region ����ü
public struct PostStruct
{
    public string name;
    public string content;
    public int giftIndex;
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

    // ���� UI ����
    [SerializeField]
    private GameObject postObj;         // ������ ���� UI (������)
    [SerializeField]
    public GameObject parent;          // ���� ������Ʈ�� �θ�

    
    private List<PostObject> postUIList = new List<PostObject>();    // ���� UI ����Ʈ
    public List<Vector2> UITransformList = new List<Vector2>();      // ���� UI�� ��ġ ����Ʈ
    public List<Vector2> parentSizeList = new List<Vector2>();       // �θ�(��ũ�Ѻ��� content)�� ũ�� ����Ʈ

    // UI ��ġ
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    private float nextYPos = -90;

    // ĳ��
    WaitForSeconds waitForSeconds;

    // ��ũ��Ʈ
    public WritingPad writingPad;

    // �� �� ����
    public int maximum;        // ������ �ƽø�

    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        waitForSeconds = new WaitForSeconds(5f);     // ���� ���� �� ������ ������ �����ϱ� ����

        SetTransform();

        ReadCSV();
    }

    private void Start()
    {
        maximum = ObjectPoolingManager.Instance.poolingList[(int)EObjectFlag.post].initCount;

        StartCoroutine(SendPost());
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
            PostStruct newPost;
            newPost.name = data[i]["����"].ToString();
            newPost.content = data[i]["����"].ToString();
            newPost.giftIndex = (int)data[i]["���� �ε���"];

            postList.Add(newPost);
        }
    }


    /// <summary>
    /// ������ �ð����� ������ ���� �߼�
    /// </summary>
    IEnumerator SendPost()
    {
        while (true)
        {
            yield return waitForSeconds;

            int randIndex = Random.Range(0, postList.Count);        // �������� ���� ������ ����
            if (postUIList.Count < maximum)          // �������� ���� �ʾ��� ���� ����
            {
                if (GameLoadManager.CurrentScene().name == "SantaVillage")
                {
                    PostOfficeInstance(postList[randIndex]);
                }
            }
        }
    }

    /// <summary>
    /// ���� �ν��Ͻ� ����
    /// </summary>
    void PostOfficeInstance(PostStruct post)
    {
        PostObject newObj = ObjectPoolingManager.Instance.Get(EObjectFlag.post).GetComponent<PostObject>();
        
        newObj.PostName = post.name;
        newObj.PostConent = post.content;
        newObj.transform.GetComponent<RectTransform>().anchoredPosition = UITransformList[0];       // ���ο� ������ �� ������� ������
        newObj.index = postUIList.Count;

        for (int i = 0; i < postUIList.Count; i++)          // ������ ������ ������ ������ UI�� �ٽ� ��ġ
        {
            postUIList[i].RefreshTransform(UITransformList[postUIList.Count - i]);
        }

        parentRectTransform.sizeDelta = parentSizeList[postUIList.Count];

        postUIList.Add(newObj);

        GiftManager.Instance.giftList[post.giftIndex].wishCount++;      // ���� ���ø���Ʈ�� �߰�
    }

    /// <summary>
    /// Post Object ���� ��
    /// </summary>
    /// <param name="index"></param>
    public void Refresh(int index)
    {
        ObjectPoolingManager.Instance.Set(postUIList[index].gameObject, EObjectFlag.post);        // ������Ʈ Ǯ�� ������

        postUIList.RemoveAt(index);

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
    #endregion
}
