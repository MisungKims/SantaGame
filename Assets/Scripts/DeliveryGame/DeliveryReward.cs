/**
 * @brief 선물 전달 게임의 보상 (퍼즐, 당근)
 * @author 김미성
 * @date 22-06-04
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryReward : MonoBehaviour
{
    #region 변수
    public ERewardType rewardType;

    [SerializeField]
    private Sprite[] rewardImages;   // 보상 이미지가 들어있는 배열
    [SerializeField]
    private Image image;        // 보상 이미지
    #endregion

    #region 유니티 함수
    private void OnEnable()
    {
        // 퍼즐, 당근 중 랜덤으로 보상을 정함
        int rand = Random.Range(0, 2);
        image.sprite = rewardImages[rand];

        if (rand == 0)
            rewardType = ERewardType.carrot;
        else if (rand == 1)
            rewardType = ERewardType.puzzle;
    }
    #endregion
}
