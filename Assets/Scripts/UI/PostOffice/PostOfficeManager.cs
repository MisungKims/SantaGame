/**
 * @details CSV 파일을 가져와 편지 발송
 * @author 김미성
 * @date 22-04-19
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostOfficeManager : MonoBehaviour
{
    #region 구조체
    struct PostStruct
    {
        public string name;
        public string content;
    }
    #endregion


    #region 변수
    // 싱글톤
    private static PostOfficeManager instance;
    public static PostOfficeManager Instance
    {
        get { return instance; }
    }

    // CSV 파일에서 가져올 전체 편지 리스트
    private List<PostStruct> postList = new List<PostStruct>();     

    // 편지 오브젝트 생성
    [SerializeField]
    private GameObject postObj;         // 생성될 편지 오브젝트 (프리팹)
    [SerializeField]
    private GameObject parent;          // 편지 오브젝트의 부모

    // UI 배치
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    private float nextYPos = -70;

    // 캐싱
    WaitForSeconds waitForSeconds;

    // 스크립트
    public WritingPad writingPad;

    // 그 외 변수
    [SerializeField]
    float second = 10f;             // 편지를 언제마다 발송할지


    #endregion

    #region 유니티 함수
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

    #region 함수
    /// <summary>
    /// 초기 위치와 사이즈 설정
    /// </summary>
    void SetTransform()
    {
        rectTransform = postObj.transform.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector3(0, 0, 0);

        parentRectTransform = parent.transform.GetComponent<RectTransform>();
        parentRectTransform.sizeDelta = new Vector2(0, 80);
    }

    /// <summary>
    /// csv 리더를 통해 파일 가져오기
    /// </summary>
    void ReadCSV()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("PostOfficeData");

        for (int i = 0; i < data.Count; i++)
        {
            //PostStruct newPost = new PostStruct(data[i]["수신"].ToString(), data[i]["내용"].ToString());
            PostStruct newPost;
            newPost.name = data[i]["수신"].ToString();
            newPost.content = data[i]["내용"].ToString();

            postList.Add(newPost);
        }
    }


    /// <summary>
    /// 랜덤한 시간마다 랜덤한 편지 발송
    /// </summary>
    IEnumerator SendPost()
    {
        while (true)
        {
            yield return waitForSeconds;

            int randIndex = Random.Range(0, postList.Count);
           
            /// TODO : 오브젝트 풀링으로 
            PostOfficeInstance(postList[randIndex].name, postList[randIndex].content);
        }
    }

    /// <summary>
    /// 편지 인스턴스 생성
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
