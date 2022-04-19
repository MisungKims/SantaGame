/**
 * @details CSV ������ ������ ���� �߼�
 * @author ��̼�
 * @date 22-04-19
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostOfficeManager : MonoBehaviour
{
    #region ����ü
    struct PostStruct
    {
        public string name;
        public string content;
    }
    #endregion


    #region ����
    // �̱���
    private static PostOfficeManager instance;
    public static PostOfficeManager Instance
    {
        get { return instance; }
    }

    // CSV ���Ͽ��� ������ ��ü ���� ����Ʈ
    private List<PostStruct> postList = new List<PostStruct>();     

    // ���� ������Ʈ ����
    [SerializeField]
    private GameObject postObj;         // ������ ���� ������Ʈ (������)
    [SerializeField]
    private GameObject parent;          // ���� ������Ʈ�� �θ�

    // UI ��ġ
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    private float nextYPos = -70;

    // ĳ��
    WaitForSeconds waitForSeconds;

    // ��ũ��Ʈ
    public WritingPad writingPad;

    // �� �� ����
    [SerializeField]
    float second = 10f;             // ������ �������� �߼�����


    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        waitForSeconds = new WaitForSeconds(second);

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

        parentRectTransform = parent.transform.GetComponent<RectTransform>();
        parentRectTransform.sizeDelta = new Vector2(0, 80);
    }

    /// <summary>
    /// csv ������ ���� ���� ��������
    /// </summary>
    void ReadCSV()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("PostOfficeData");

        for (int i = 0; i < data.Count; i++)
        {
            //PostStruct newPost = new PostStruct(data[i]["����"].ToString(), data[i]["����"].ToString());
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
           
            /// TODO : ������Ʈ Ǯ������ 
            PostOfficeInstance(postList[randIndex].name, postList[randIndex].content);
        }
    }

    /// <summary>
    /// ���� �ν��Ͻ� ����
    /// </summary>
    void PostOfficeInstance(string name, string content)
    {
        PostObject newObj = GameObject.Instantiate(postObj, parent.transform).GetComponent<PostObject>();

        newObj.transform.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;

        newObj.PostName = name;
        newObj.PostConent = content;

        rectTransform.anchoredPosition += new Vector2(0, nextYPos);

        parentRectTransform.sizeDelta += new Vector2(0, rectTransform.sizeDelta.y);
    }
    #endregion
}
