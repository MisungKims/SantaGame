/**
 * @details CSV 파일을 가져와 편지 발송
 * @author 김미성
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// TODO : 인스펙터에서 우체국으로 이동하기

#region 구조체
public struct PostStruct
{
    public string name;
    public string content;
}
#endregion

public class PostOfficeManager : MonoBehaviour
{
    #region 변수
    // 싱글톤
    private static PostOfficeManager instance;
    public static PostOfficeManager Instance
    {
        get { return instance; }
    }

    // CSV 파일에서 가져올 전체 편지 리스트
    private List<PostStruct> postList = new List<PostStruct>();     

    // 편지 UI 생성
    [SerializeField]
    private GameObject postObj;         // 생성될 편지 UI (프리팹)
    [SerializeField]
    public GameObject parent;          // 편지 오브젝트의 부모

    
    private List<PostObject> postUIList = new List<PostObject>();       // 편지 UI 리스트
    public List<Vector2> UITransformList = new List<Vector2>();       // 편지 UI의 위치 리스트
    public List<Vector2> parentSizeList = new List<Vector2>();       // 편지 UI의 위치 리스트

    // UI 배치
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    private float nextYPos = -90;

    // 캐싱
    WaitForSeconds waitForSeconds;

    // 스크립트
    public WritingPad writingPad;

    // 그 외 변수
    public int maximum = 20;        // 편지함 맥시멈

    #endregion

    #region 유니티 함수
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        waitForSeconds = new WaitForSeconds(GameManager.Instance.dayCount);     // 다음 날이 될 때마다 편지를 전송하기 위함

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
    /// csv 리더를 통해 파일 가져오기
    /// </summary>
    void ReadCSV()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("PostOfficeData");

        for (int i = 0; i < data.Count; i++)
        {
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
           
            PostOfficeInstance(postList[randIndex]);
        }
    }

    /// <summary>
    /// 편지 인스턴스 생성
    /// </summary>
    void PostOfficeInstance(PostStruct post)
    {
        PostObject newObj = PostOfficeObjectPool.Instance.Get(post);           // 오브젝트 풀에서 꺼냄

        if (newObj == null)     // 편지함이 꽉 찼을 때에는 생성하지 않음
        {
            return;
        }

        newObj.transform.GetComponent<RectTransform>().anchoredPosition = UITransformList[0];       // 새로온 편지는 맨 상단으로 가도록
        newObj.index = postUIList.Count;

        for (int i = 0; i < postUIList.Count; i++)          // 오래된 편지가 밑으로 가도록 UI를 다시 배치
        {
            postUIList[i].RefreshTransform(UITransformList[postUIList.Count - i]);
        }

        parentRectTransform.sizeDelta = parentSizeList[postUIList.Count];

        postUIList.Add(newObj);
    }

    /// <summary>
    /// Post Object 제거 시
    /// </summary>
    /// <param name="index"></param>
    public void Refresh(int index)
    {
        PostOfficeObjectPool.Instance.Set(postUIList[index]);           // 오브젝트 풀에 돌려줌
        postUIList.RemoveAt(index);

        // UI 배치를 다시
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
