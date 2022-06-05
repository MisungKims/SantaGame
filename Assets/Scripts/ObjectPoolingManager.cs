/**
 * @brief ������Ʈ Ǯ��
 * @author ��̼�
 * @date 22-06-01
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EObjectFlag // �迭�̳� ����Ʈ�� ����
{
    getCarrotButton,
    post,
    gift,
    utilityPole,     // ������
    chimney,        // ����
    bird,
    reward
}

public class ObjectPoolingManager : MonoBehaviour
{
    #region ����
    // �̱���
    private static ObjectPoolingManager instance;
    public static ObjectPoolingManager Instance
    {
        get { return instance; }
    }

    public List<ObjectPool> poolingList = new List<ObjectPool>();

    // Clothes�� ���� ������
    [SerializeField]
    private Transform clothesParent;

    public List<Queue<GameObject>> clothesQueue = new List<Queue<GameObject>>();

    private ClothesManager clothesManager;
    #endregion

    #region ����Ƽ �Լ�
    void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }

        clothesManager = ClothesManager.Instance;

        Init();
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// �ʱ⿡ initCount ��ŭ ����
    /// </summary>
    private void Init()
    {
        for (int i = 0; i < poolingList.Count; i++)     // poolingList�� Ž���� �� ������Ʈ�� �̸� ����
        {
            for (int j = 0; j < poolingList[i].initCount; j++)
            {
                GameObject tempGb = GameObject.Instantiate(poolingList[i].copyObj, poolingList[i].parent.transform);
                tempGb.name = j.ToString();
                tempGb.gameObject.SetActive(false);
                poolingList[i].queue.Enqueue(tempGb);
            }
        }
    }

    /// <summary>
    /// �ʱ⿡ initCount ��ŭ ����
    /// </summary>
    public void InitClothes()
    {
        for (int index = 0; index < ClothesManagerInstance().clothesList.Count; index++)
        {
            clothesQueue.Add(new Queue<GameObject>());
            for (int i = 0; i < 5; i++)
            {
                GameObject tempGb = GameObject.Instantiate(ClothesManagerInstance().clothesList[index].clothesPrefabs, clothesParent);
                tempGb.name = i.ToString();
                tempGb.gameObject.SetActive(false);
                clothesQueue[index].Enqueue(tempGb);
            }
        }
    }

    /// <summary>
    /// ������Ʈ�� ��ȯ
    /// </summary>
    public GameObject Get(EObjectFlag flag)
    {
        int index = (int)flag;
        GameObject tempGb;

        if (poolingList[index].queue.Count > 0)             // ť�� ���� ������Ʈ�� ���� ���� ��
        {
            tempGb = poolingList[index].queue.Dequeue();
            tempGb.SetActive(true);
        }
        else         // ť�� ���̻� ������ ���� ����
        {
            tempGb = GameObject.Instantiate(poolingList[index].copyObj, poolingList[index].parent.transform);
        }

        return tempGb;
    }


    /// <summary>
    /// �� �� ������Ʈ�� ť�� ������
    /// </summary>
    public void Set(GameObject gb, EObjectFlag flag)
    {
        int index = (int)flag;
        gb.SetActive(false);

        poolingList[index].queue.Enqueue(gb);
    }

    /// <summary>
    /// ������Ʈ�� ��ȯ
    /// </summary>
    public GameObject Get(EClothesFlag flag, Transform parent)
    {
        int index = (int)flag;
        GameObject tempGb;

        if (clothesQueue[index].Count > 0)             // ť�� ���� ������Ʈ�� ���� ���� ��
        {
            tempGb = clothesQueue[index].Dequeue();
            tempGb.transform.SetParent(parent);
            tempGb.gameObject.SetActive(true);
        }
        else         // ť�� ���̻� ������ ���� ����
        {
            tempGb = GameObject.Instantiate(ClothesManagerInstance().clothesList[index].clothesPrefabs, parent);
        }

        return tempGb;
    }

    /// <summary>
    /// �� �� ������Ʈ�� ť�� ������
    /// </summary>
    public void Set(GameObject gb, EClothesFlag flag)
    {
        int index = (int)flag;
        gb.SetActive(false);
        gb.transform.SetParent(clothesParent);

        clothesQueue[index].Enqueue(gb);
    }

    ClothesManager ClothesManagerInstance()
    {
        if (!clothesManager)
        {
            clothesManager = ClothesManager.Instance;
        }

        return clothesManager;
    }
    #endregion
}
