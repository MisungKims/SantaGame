/**
 * @brief 산타 주민을 관리
 * @author 김미성
 * @date 22-04-22
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class goal
{
    public goalObject[] goalObjects;
}

[System.Serializable]
public class goalObject
{
    public Transform pos;     // 건물에서 주민이 갈 수 있는 위치
    public bool isUse;          // 다른 주민이 이미 사용 중인가?
}

public class CitizenRabbitManager : MonoBehaviour
{
    public List<goal> goalPositions = new List<goal>();       // 주민이 갈 수 있는 건물의 위치 리스트

    // 싱글톤
    private static CitizenRabbitManager instance;
    public static CitizenRabbitManager Instance
    {
        get { return instance; }
    }

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
        }
    }

}
