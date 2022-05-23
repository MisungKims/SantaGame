/**
 * @brief 선물 전달 게임의 산타(플레이어)
 * @author 김미성
 * @date 22-05-18
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverySanta : MonoBehaviour
{
    #region 변수
    private Rigidbody rigid;

    [SerializeField]
    private float jumpPower = 50f;
    [SerializeField]
    private float doubleJumpPower = 40f;
    [SerializeField]
    private Vector3 gravity = new Vector3(0, -50f, 0);

    private int jumpCnt = 0;

    public static GameObject giftPos;      // 선물을 떨어뜨릴 위치

    // 캐싱
    private DeliveryGameManager deliveryGameManager;

    private ObjectPoolingManager objectPoolingManager;

    private SoundManager soundManager;
    #endregion

    #region 유니티 함수
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        Physics.gravity = gravity;

        deliveryGameManager = DeliveryGameManager.Instance;
        objectPoolingManager = ObjectPoolingManager.Instance;
        soundManager = SoundManager.Instance;

        giftPos = this.transform.GetChild(1).gameObject;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))       // Floor에 닿으면 jumpCnt를 초기화
        {
            jumpCnt = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))    // 장애물에 부딪히면 생명 감소
        {
            soundManager.PlaySoundEffect(ESoundEffectType.deliveryObstacle);     // 효과음 실행
            deliveryGameManager.Life--;
        }
        else if (other.gameObject.CompareTag("Reward"))
        {
            soundManager.PlaySoundEffect(ESoundEffectType.deliveryGetGift);     // 효과음 실행

            DeliveryReward reward = other.transform.GetComponent<DeliveryReward>();
            ObjectPoolingManager.Instance.Set(reward.gameObject, EDeliveryFlag.reward);     // 오브젝트 풀에 반환

            if (reward.rewardType.Equals(ERewardType.carrot))           // 보상이 당근일 때
            {
                deliveryGameManager.carrotCount++;
            }
            else if (reward.rewardType.Equals(ERewardType.puzzle))           // 보상이 퍼즐 조각일 때
            {
                deliveryGameManager.PuzzleCount++;
            }
        }
    }
    #endregion

    #region 함수
    /// <summary>
    /// 더블 점프 (인스펙터에서 호출)
    /// </summary>
    public void Jump()
    {
        if (!deliveryGameManager.isStart)
        {
            return;
        }

        if (jumpCnt == 0)
        {
            jumpCnt++;
            soundManager.PlaySoundEffect(ESoundEffectType.getGoldButton);     // 효과음 실행
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
        else if (jumpCnt == 1)
        {
            jumpCnt++;
            soundManager.PlaySoundEffect(ESoundEffectType.getGoldButton);     // 효과음 실행
            rigid.AddForce(Vector3.up * doubleJumpPower, ForceMode.Impulse);
        }
        else return;
    }

    /// <summary>
    /// Drop 버튼 클릭 시 선물을 떨어뜨림 (인스펙터에서 호출)
    /// </summary>
    public void Drop()
    {
        if (!deliveryGameManager.isStart)
        {
            return;
        }

        if (deliveryGameManager.GiftCount > 0)
        {
            soundManager.PlaySoundEffect(ESoundEffectType.uiButton);

            objectPoolingManager.Get(EDeliveryFlag.gift);
            deliveryGameManager.GiftCount--;
        }
    }
    #endregion
}
