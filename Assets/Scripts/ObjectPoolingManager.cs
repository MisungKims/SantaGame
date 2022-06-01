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

    //public List<ObjectPool> clothesPoolingList = new List<ObjectPool>();

    // Clothes�� ���� ������

    [SerializeField]
    private Transform clothesParent;

    public List<Queue<GameObject>> clothesQueue = new List<Queue<GameObject>>();

    
    //public Queue<GameObject> clothesQueue = new Queue<GameObject>();   // ������Ʈ���� ���� ť

    #endregion

    #region ����Ƽ �Լ�
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
           // DontDestroyOnLoad(gameObject);      // �� ��ȯ �ÿ��� �ı����� ����
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }

        for (int i = 0; i < poolingList.Count; i++)     // poolingList�� Ž���� �� ������Ʈ�� �̸� ����
        {
            Init(i);
        }

        
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// �ʱ⿡ initCount ��ŭ ����
    /// </summary>
    private void Init(int index)
    {
        for (int i = 0; i < poolingList[index].initCount; i++)
        {
            GameObject tempGb = GameObject.Instantiate(poolingList[index].copyObj, poolingList[index].parent.transform);
            tempGb.name = i.ToString();
            tempGb.gameObject.SetActive(false);
            poolingList[index].queue.Enqueue(tempGb);
        }
    }

    /// <summary>
    /// �ʱ⿡ initCount ��ŭ ����
    /// </summary>
    public void InitClothes()
    {
        for (int index = 0; index < ClothesManager.Instance.clothesList.Count; index++)
        {
            clothesQueue.Add(new Queue<GameObject>());
            for (int i = 0; i < 5; i++)
            {
                GameObject tempGb = GameObject.Instantiate(ClothesManager.Instance.clothesList[index].clothesPrefabs, clothesParent);
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
            tempGb = GameObject.Instantiate(ClothesManager.Instance.clothesList[index].clothesPrefabs, parent);
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

    ///// <summary>
    ///// ������Ʈ�� ��ȯ (���� ���� ���� ������ ���)
    ///// </summary>
    //public GameObject Get(EObjectFlag flag)
    //{
    //    int index = (int)flag;
    //    GameObject tempGb;

    //    if (poolingList[index].queue.Count > 0)             // ť�� ���� ������Ʈ�� ���� ���� ��
    //    {
    //        tempGb = poolingList[index].queue.Dequeue();
    //    }
    //    else         // ť�� ���̻� ������ ���� ����
    //    {

    //        tempGb = GameObject.Instantiate(poolingList[index].copyObj, poolingList[index].parent.transform);
    //    }

    //    tempGb.SetActive(true);
    //    return tempGb;
    //}


    ///// <summary>
    ///// �� �� ������Ʈ�� ť�� ������ (���� ���� ���� ������ ���)
    ///// </summary>
    //public void Set(GameObject gb, EDeliveryFlag flag)
    //{
    //    int index = (int)flag;
    //    gb.SetActive(false);

    //    poolingList[index].queue.Enqueue(gb);
    //}
    #endregion

}
