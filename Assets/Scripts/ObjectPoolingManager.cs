/**
 * @brief 오브젝트 풀링
 * @author 김미성
 * @date 22-04-27
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EObjectFlag // 배열이나 리스트의 순서
{
    getCarrotButton,
    post
}

public enum EDeliveryFlag // 배열이나 리스트의 순서
{
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
    #endregion

    #region 유니티 함수
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
           // DontDestroyOnLoad(gameObject);      // 씬 전환 시에도 파괴되지 않음
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }

        for (int i = 0; i < poolingList.Count; i++)     // poolingList를 탐색해 각 오브젝트를 미리 생성
        {
            Init(i);
        }
    }
    #endregion

    #region 함수
    /// <summary>
    /// 초기에 initCount 만큼 생성
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
    /// 오브젝트를 반환 (선물 전달 게임 씬에서 사용)
    /// </summary>
    public GameObject Get(EDeliveryFlag flag)
    {
        int index = (int)flag;
        GameObject tempGb;

        if (poolingList[index].queue.Count > 0)             // 큐에 게임 오브젝트가 남아 있을 때
        {
            tempGb = poolingList[index].queue.Dequeue();
        }
        else         // 큐에 더이상 없으면 새로 생성
        {
            
            tempGb = GameObject.Instantiate(poolingList[index].copyObj, poolingList[index].parent.transform);
        }

        tempGb.SetActive(true);
        return tempGb;
    }


    /// <summary>
    /// 다 쓴 오브젝트를 큐에 돌려줌 (선물 전달 게임 씬에서 사용)
    /// </summary>
    public void Set(GameObject gb, EDeliveryFlag flag)
    {
        int index = (int)flag;
        gb.SetActive(false);

        poolingList[index].queue.Enqueue(gb);
    }
    #endregion

}
