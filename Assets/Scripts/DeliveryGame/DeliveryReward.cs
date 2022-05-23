using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryReward : MonoBehaviour
{
    public ERewardType rewardType;

    [SerializeField]
    private Sprite[] rewardImages;   // ���� �̹����� ����ִ� �迭
    [SerializeField]
    private Image image;        // ���� �̹���

    private void OnEnable()
    {
        int rand = Random.Range(0, 2);
        image.sprite = rewardImages[rand];

        if (rand == 0)
            rewardType = ERewardType.carrot;
        else if (rand == 1)
            rewardType = ERewardType.puzzle;
    }
}
