using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : DeliveryGameObject
{
    [SerializeField]
    private Transform rewardPos;

    GameObject reward;      // 퍼즐이나 당근

    protected override void OnEnable()
    {
        base.OnEnable();

        int rand = Random.Range(0, 100);
        if (rand <= 10)     // 10%의 확률로 보상 아이템 Spawn
        {
            reward = ObjectPoolingManager.Instance.Get(EObjectFlag.reward);
            reward.transform.position = rewardPos.position;
        }
    }

    protected override void Update()
    {
        if (!DeliveryGameManager.Instance.isEnd)
        {
            this.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            if(reward) reward.transform.position = rewardPos.position;
        }
        else
        {
            // 게임 종료시 게임오브젝트를 오브젝트 풀에 반환
            ObjectPoolingManager.Instance.Set(this.gameObject, flag);

            if (reward)
            {
                ObjectPoolingManager.Instance.Set(reward, EObjectFlag.reward);
                reward = null;
            }
        }
    }

}
