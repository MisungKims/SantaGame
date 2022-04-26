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

    
    private List<PostObject> postUIList = new List<PostObject>();       // ���� UI ����Ʈ
    public List<Vector2> UITransformList = new List<Vector2>();       // ���� UI�� ��ġ ����Ʈ
    public List<Vector2> parentSizeList = new List<Vector2>();       // ���� UI�� ��ġ ����Ʈ

    // UI ��ġ
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    private float nextYPos = -90;

    // ĳ��
    WaitForSeconds waitForSeconds;

    // ��ũ��Ʈ
    public WritingPad writingPad;

    // �� �� ����
    public int maximum = 20;        // ������ �ƽø�

    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        waitForSeconds = new WaitForSeconds(GameManager.Instance.dayCount);     // ���� ���� �� ������ ������ �����ϱ� ����

        SetTransform();

        ReadCSV();
    }

    private void Start()
    {
        StartCoroutine(SendPost());
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// �ʱ� ��ġ�� ������ ����
    /// </summary>
    void SetTransform()
    {
        rectTransform = postObj.transform.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector3(0, 0, 0);

        for (int i = 0; i < maximum; i++)
        {
            UITransformList.Add(rectTransform.anchoredPosition);
            rectTransform.anchoredPosition += new Vector2(0, nextYPos);
        }

        parentRectTransform = parent.transform.GetComponent<RectTransform>();
        parentRectTransform.sizeDelta = new Vector2(0, 80);

        Vector2 temp = parentRectTransform.sizeDelta;
        for (int i = 0; i < maximum; i++)
        {
            parentSizeList.Add(temp);
            temp += new Vector2(0, rectTransform.sizeDelta.y);
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

            int randIndex = Random.Range(0, postList.Count);
           
            PostOfficeInstance(postList[randIndex]);
        }
    }

    /// <summary>
    /// ���� �ν��Ͻ� ����
    /// </summary>
    void PostOfficeInstance(PostStruct post)
    {
        PostObject newObj = PostOfficeObjectPool.Instance.Get(post);           // ������Ʈ Ǯ���� ����

        if (newObj == null)     // �������� �� á�� ������ �������� ����
        {
            return;
        }

        newObj.transform.GetComponent<RectTransform>().anchoredPosition = UITransformList[0];       // ���ο� ������ �� ������� ������
        newObj.index = postUIList.Count;

        for (int i = 0; i < postUIList.Count; i++)          // ������ ������ ������ ������ UI�� �ٽ� ��ġ
        {
            postUIList[i].RefreshTransform(UITransformList[postUIList.Count - i]);
        }

        parentRectTransform.sizeDelta = parentSizeList[postUIList.Count];

        postUIList.Add(newObj);
    }

    /// <summary>
    /// Post Object ���� ��
    /// </summary>
    /// <param name="index"></param>
    public void Refresh(int index)
    {
        PostOfficeObjectPool.Instance.Set(postUIList[index]);           // ������Ʈ Ǯ�� ������
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
