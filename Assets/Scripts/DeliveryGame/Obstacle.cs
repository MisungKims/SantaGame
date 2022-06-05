/**
 * @brief ���� ���� ������ ��ֹ�
 * @author ��̼�
 * @date 22-06-04
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : DeliveryGameObject
{
    #region ����
    [SerializeField]
    private Transform rewardPos;

    private GameObject reward;      // ��ֹ��� �Ӹ� �����ִ� �����̳� ���
    #endregion

    #region ����Ƽ �Լ�
    protected override void OnEnable()
    {
        base.OnEnable();

        // 10%�� Ȯ���� ���� ������ Spawn
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
            // ���� ����� ���ӿ�����Ʈ�� ������Ʈ Ǯ�� ��ȯ
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
