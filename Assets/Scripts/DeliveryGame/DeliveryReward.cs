using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryReward : MonoBehaviour
{
    public ERewardType rewardType;

    [SerializeField]
    private Sprite[] rewardImages;   // 보상 이미지가 들어있는 배열
    [SerializeField]
    private Image image;        // 보상 이미지

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
