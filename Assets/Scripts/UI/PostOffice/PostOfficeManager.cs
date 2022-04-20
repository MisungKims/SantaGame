/**
 * @details CSV ������ ������ ���� �߼�
 * @author ��̼�
 * @date 22-04-20
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float nextYPos = -75;

    // ĳ��
    WaitForSeconds waitForSeconds;

    // ��ũ��Ʈ
    public WritingPad writingPad;

    // �� �� ����
    [SerializeField]
    float second = 10f;             // ������ �������� �߼�����

    int maximum = 20;        // ������ �ƽø�

    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        waitForSeconds = new WaitForSeconds(GameManager.Instance.dayCount);     // ���� ���� �� ������ ���� ����

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
        PostObject newObj = PostOfficeObjectPool.Instance.Get(post);

        if (newObj == null)     // �������� �� á�� ��
        {
            return;
        }

        newObj.transform.GetComponent<RectTransform>().anchoredPosition = UITransformList[postUIList.Count];
        newObj.index = postUIList.Count;

        parentRectTransform.sizeDelta = parentSizeList[postUIList.Count];

        postUIList.Add(newObj);
    }

    /// <summary>
    /// Post Object ���� ��
    /// </summary>
    /// <param name="index"></param>
    public void Refresh(int index)
    {
        PostOfficeObjectPool.Instance.Set(postUIList[index]);
        
        // ����Ʈ ������
        for (int i = index; i < postUIList.Count - 1; i++)
        {
            postUIList[i] = postUIList[i + 1];
        }
       
        postUIList.RemoveAt(postUIList.Count - 1);

        // UI ��ġ �ٽ�
        for (int i = 0; i < postUIList.Count; i++)
        {
            postUIList[i].RefreshTransform(UITransformList[i]);
        }

        parentRectTransform.sizeDelta = parentSizeList[postUIList.Count];
    }
    #endregion
}
