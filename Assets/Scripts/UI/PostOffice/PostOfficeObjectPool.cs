/**
 * @brief 편지 UI 오브젝트 풀링
 * @author 김미성
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostOfficeObjectPool : MonoBehaviour
{
    #region 변수
    // 싱글톤
    private static PostOfficeObjectPool instance;
    public static PostOfficeObjectPool Instance
    {
        get { return instance; }
    }

    public GameObject cpyPostObject;     // 복제할 오브젝트 (프리팹)
    public Queue<PostObject> que = new Queue<PostObject>(); // 담을 큐

    private Transform parent;
    #endregion

    #region 유니티 함수
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        parent = PostOfficeManager.Instance.parent.transform;

        Init(PostOfficeManager.Instance.maximum);
    }
    #endregion

    #region 함수
    /// <summary>
    /// 초기에 Maximum만큼 생성
    /// </summary>
    /// <param name="count">편지함 Maximum</param>
    private void Init(int count)
    {
        for (int i = 0; i < count; i++)
        {
            PostObject tempGb = GameObject.Instantiate(cpyPostObject, parent).GetComponent<PostObject>();
            tempGb.gameObject.SetActive(false);

            que.Enqueue(tempGb);
        }
    }

    /// <summary>
    /// 큐에서 쓸 수 있는 오브젝트가 있으면 그것을 반환
    /// </summary>
    /// <param name="postStruct">반환할 것</param>
    public PostObject Get(PostStruct postStruct)
    {
        PostObject tempGb;

        if (que.Count > 0)
        {
            tempGb = que.Dequeue();

            tempGb.PostName = postStruct.name;
            tempGb.PostConent = postStruct.content;
            tempGb.gameObject.SetActive(true);

            return tempGb;
        }

        return null;
    }

    /// <summary>
    /// 다 쓴 오브젝트를 큐에 돌려줌
    /// </summary>
    /// <param name="post">돌려줄 것</param>
    public void Set(PostObject post)
    {
        post.gameObject.SetActive(false);
        que.Enqueue(post);
    }
    #endregion
}
