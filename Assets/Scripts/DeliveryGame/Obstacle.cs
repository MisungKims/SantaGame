/**
 * @brief 선물 전달 게임의 장애물
 * @author 김미성
 * @date 22-06-04
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : DeliveryGameObject
{
    #region 변수
    [SerializeField]
    private Transform rewardPos;

    private GameObject reward;      // 장애물의 머리 위에있는 퍼즐이나 당근
    #endregion

    #region 유니티 함수
    protected override void OnEnable()
    {
        base.OnEnable();

        // 10%의 확률로 보상 아이템 Spawn
        int rand = Random.Range(0, 100);
        if (rand <= 10)     
        {
            reward = objectPoolingManager.Get(EObjectFlag.reward);
            reward.transform.position = rewardPos.position;
        }
    }

    protected override void Update()
    {
        if (!deliveryGameManager.isEnd)
        {
            this.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            if (reward) reward.transform.position = rewardPos.position;
        }
        else
        {
            // 게임 종료시 게임오브젝트를 오브젝트 풀에 반환
            if (reward)
            {
                objectPoolingManager.Set(reward, EObjectFlag.reward);
                reward = null;
            }

            objectPoolingManager.Set(this.gameObject, flag);
        }
    }
    #endregion
}
