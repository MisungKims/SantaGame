/**
 * @brief 편지 UI 오브젝트 풀링
 * @author 김미성
 * @date 22-04-20
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostOfficeObjectPool : MonoBehaviour
{
    public static PostOfficeObjectPool Instance;

    public GameObject cpyPostObject;     // 복제할 오브젝트
    public Queue<PostObject> que = new Queue<PostObject>(); // 담을 큐

    private Transform parent;

    private void init(int count)
    {
        for (int i = 0; i < count; i++)
        {
            PostObject tempGb = GameObject.Instantiate(cpyPostObject, parent).GetComponent<PostObject>();
            tempGb.gameObject.SetActive(false);

            que.Enqueue(tempGb);
        }
    }

    void Awake()
    {
        Instance = this;
        parent = PostOfficeManager.Instance.parent.transform;

        init(20);
    }

    public PostObject Get(PostStruct postStruct)
    {
        PostObject tempGb;

        if (que.Count > 0) // 해당하는 큐에 게임 오브젝트가 남아 있으면 그것을 반환
        {
            tempGb = que.Dequeue(); //디큐를 통해서 실질적으로 반환

            tempGb.PostName = postStruct.name;
            tempGb.PostConent = postStruct.content;
            tempGb.gameObject.SetActive(true);

            return tempGb;
        }

        //else // 큐에 더이상 없으면 새로 생성
        //{
        //    //Set(que.)
        //    tempGb = GameObject.Instantiate(cpyPostObject, parent).GetComponent<PostObject>();
        //}

        return null;
    }

    public void Set(PostObject gb) // 나 다썼어 큐에 돌려줄게
    {
        gb.gameObject.SetActive(false);
        que.Enqueue(gb);
    }
}
