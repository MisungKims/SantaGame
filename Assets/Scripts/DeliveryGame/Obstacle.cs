using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : DeliveryGameObject
{
    [SerializeField]
    private Transform rewardPos;

    protected override void OnEnable()
    {
        base.OnEnable();

        //int rand = Random.Range(0, 100);

        //if (rand <= 3)
        //{
        //    ObjectPoolingManager.Instance.Get(EDeliveryFlag.reward);
        //}

        ObjectPoolingManager.Instance.Get(EDeliveryFlag.reward).transform.position = rewardPos.localPosition;
    }
}
