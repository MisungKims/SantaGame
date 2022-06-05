/**
 * @brief ���� ���� ������ ��Ÿ(�÷��̾�)
 * @author ��̼�
 * @date 22-06-04
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverySanta : MonoBehaviour
{
    #region ����
    private Rigidbody rigid;

    // ����
    [SerializeField]
    private float jumpPower = 700f;
    [SerializeField]
    private float doubleJumpPower = 450f;
    [SerializeField]
    private Vector3 gravity = new Vector3(0, -800f, 0);

    private int jumpCnt = 0;

    // ������ ��Ÿ�� ��ġ
    public static GameObject giftPos;      

    // ĳ��
    private DeliveryGameManager deliveryGameManager;

    private ObjectPoolingManager objectPoolingManager;

    private SoundManager soundManager;
    #endregion

    #region ����Ƽ �Լ�
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
        if (collision.gameObject.CompareTag("Floor"))       // Floor�� ������ jumpCnt�� �ʱ�ȭ
        {
            jumpCnt = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))        // ��ֹ��� �ε����� ���� ����
        {
            DecreaseLife();
        }
        else if (other.gameObject.CompareTag("Reward"))     // ���� �ε����� ������ ȹ��
        {
            GetReward(other.transform.GetComponent<DeliveryReward>());
        }
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// ���� ���� (�ν����Ϳ��� ȣ��)
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
            soundManager.PlaySoundEffect(ESoundEffectType.getGoldButton);     // ȿ���� ����
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
        else if (jumpCnt == 1)
        {
            jumpCnt++;
            soundManager.PlaySoundEffect(ESoundEffectType.getGoldButton);     // ȿ���� ����
            rigid.AddForce(Vector3.up * doubleJumpPower, ForceMode.Impulse);
        }
        else return;
    }

    /// <summary>
    /// Drop ��ư Ŭ�� �� ������ ����߸� (�ν����Ϳ��� ȣ��)
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

            objectPoolingManager.Get(EObjectFlag.gift);
            deliveryGameManager.GiftCount--;
        }
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    void DecreaseLife()
    {
        soundManager.PlaySoundEffect(ESoundEffectType.deliveryObstacle);     // ȿ���� ����
        deliveryGameManager.Life--;
    }

    /// <summary>
    /// ���� ȹ��
    /// </summary>
    void GetReward(DeliveryReward reward)
    {
        soundManager.PlaySoundEffect(ESoundEffectType.deliveryGetGift);     // ȿ���� ����

        objectPoolingManager.Set(reward.gameObject, EObjectFlag.reward);     // ������Ʈ Ǯ�� ��ȯ

        if (reward.rewardType.Equals(ERewardType.carrot))           // ������ ����� ��
        {
            deliveryGameManager.carrotCount++;
        }
        else if (reward.rewardType.Equals(ERewardType.puzzle))           // ������ ���� ������ ��
        {
            deliveryGameManager.PuzzleCount++;
        }
    }
    #endregion
}
