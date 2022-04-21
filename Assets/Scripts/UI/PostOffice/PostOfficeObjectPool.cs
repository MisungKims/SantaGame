/**
 * @brief ���� UI ������Ʈ Ǯ��
 * @author ��̼�
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostOfficeObjectPool : MonoBehaviour
{
    #region ����
    // �̱���
    private static PostOfficeObjectPool instance;
    public static PostOfficeObjectPool Instance
    {
        get { return instance; }
    }

    public GameObject cpyPostObject;     // ������ ������Ʈ (������)
    public Queue<PostObject> que = new Queue<PostObject>(); // ���� ť

    private Transform parent;
    #endregion

    #region ����Ƽ �Լ�
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

    #region �Լ�
    /// <summary>
    /// �ʱ⿡ Maximum��ŭ ����
    /// </summary>
    /// <param name="count">������ Maximum</param>
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
    /// ť���� �� �� �ִ� ������Ʈ�� ������ �װ��� ��ȯ
    /// </summary>
    /// <param name="postStruct">��ȯ�� ��</param>
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
    /// �� �� ������Ʈ�� ť�� ������
    /// </summary>
    /// <param name="post">������ ��</param>
    public void Set(PostObject post)
    {
        post.gameObject.SetActive(false);
        que.Enqueue(post);
    }
    #endregion
}
