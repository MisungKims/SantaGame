/**
 * @brief 오브젝트 풀링
 * @author 김미성
 * @date 22-06-01
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EObjectFlag // 배열이나 리스트의 순서
{
    getCarrotButton,
    post,
    gift,
    utilityPole,     // 전봇대
    chimney,        // 굴뚝
    bird,
    reward
}

public class ObjectPoolingManager : MonoBehaviour
{
    #region 변수
    // 싱글톤
    private static ObjectPoolingManager instance;
    public static ObjectPoolingManager Instance
    {
        get { return instance; }
    }

    public List<ObjectPool> poolingList = new List<ObjectPool>();

    // Clothes를 위한 변수들
    [SerializeField]
    private Transform clothesParent;

    public List<Queue<GameObject>> clothesQueue = new List<Queue<GameObject>>();

    private ClothesManager clothesManager;
    #endregion

    #region 유니티 함수
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

    #region 함수
    /// <summary>
    /// 초기에 initCount 만큼 생성
    /// </summary>
    private void Init()
    {
        for (int i = 0; i < poolingList.Count; i++)     // poolingList를 탐색해 각 오브젝트를 미리 생성
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
    /// 초기에 initCount 만큼 생성
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
    /// 오브젝트를 반환
    /// </summary>
    public GameObject Get(EObjectFlag flag)
    {
        int index = (int)flag;
        GameObject tempGb;

        if (poolingList[index].queue.Count > 0)             // 큐에 게임 오브젝트가 남아 있을 때
        {
            tempGb = poolingList[index].queue.Dequeue();
            tempGb.SetActive(true);
        }
        else         // 큐에 더이상 없으면 새로 생성
        {
            tempGb = GameObject.Instantiate(poolingList[index].copyObj, poolingList[index].parent.transform);
        }

        return tempGb;
    }


    /// <summary>
    /// 다 쓴 오브젝트를 큐에 돌려줌
    /// </summary>
    public void Set(GameObject gb, EObjectFlag flag)
    {
        int index = (int)flag;
        gb.SetActive(false);

        poolingList[index].queue.Enqueue(gb);
    }

    /// <summary>
    /// 오브젝트를 반환
    /// </summary>
    public GameObject Get(EClothesFlag flag, Transform parent)
    {
        int index = (int)flag;
        GameObject tempGb;

        if (clothesQueue[index].Count > 0)             // 큐에 게임 오브젝트가 남아 있을 때
        {
            tempGb = clothesQueue[index].Dequeue();
            tempGb.transform.SetParent(parent);
            tempGb.gameObject.SetActive(true);
        }
        else         // 큐에 더이상 없으면 새로 생성
        {
            tempGb = GameObject.Instantiate(ClothesManagerInstance().clothesList[index].clothesPrefabs, parent);
        }

        return tempGb;
    }

    /// <summary>
    /// 다 쓴 오브젝트를 큐에 돌려줌
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
