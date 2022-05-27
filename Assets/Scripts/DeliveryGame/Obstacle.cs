using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : DeliveryGameObject
{
    [SerializeField]
    private Transform rewardPos;

    GameObject reward;      // �����̳� ���

    protected override void OnEnable()
    {
        base.OnEnable();

        int rand = Random.Range(0, 100);
        if (rand <= 10)     // 10%�� Ȯ���� ���� ������ Spawn
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
            // ���� ����� ���ӿ�����Ʈ�� ������Ʈ Ǯ�� ��ȯ
            ObjectPoolingManager.Instance.Set(this.gameObject, flag);

            if (reward)
            {
                ObjectPoolingManager.Instance.Set(reward, EObjectFlag.reward);
                reward = null;
            }
        }
    }

}
