/**
 * @brief ���� UI ������Ʈ Ǯ��
 * @author ��̼�
 * @date 22-04-20
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostOfficeObjectPool : MonoBehaviour
{
    public static PostOfficeObjectPool Instance;

    public GameObject cpyPostObject;     // ������ ������Ʈ
    public Queue<PostObject> que = new Queue<PostObject>(); // ���� ť

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

        if (que.Count > 0) // �ش��ϴ� ť�� ���� ������Ʈ�� ���� ������ �װ��� ��ȯ
        {
            tempGb = que.Dequeue(); //��ť�� ���ؼ� ���������� ��ȯ

            tempGb.PostName = postStruct.name;
            tempGb.PostConent = postStruct.content;
            tempGb.gameObject.SetActive(true);

            return tempGb;
        }

        //else // ť�� ���̻� ������ ���� ����
        //{
        //    //Set(que.)
        //    tempGb = GameObject.Instantiate(cpyPostObject, parent).GetComponent<PostObject>();
        //}

        return null;
    }

    public void Set(PostObject gb) // �� �ٽ�� ť�� �����ٰ�
    {
        gb.gameObject.SetActive(false);
        que.Enqueue(gb);
    }
}
