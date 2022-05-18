/**
 * @brief ���� ���� ������ ��Ÿ(�÷��̾�)
 * @author ��̼�
 * @date 22-05-18
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverySanta : MonoBehaviour
{
    #region ����
    private Rigidbody rigid;

    [SerializeField]
    private float jumpPower = 50f;
    [SerializeField]
    private float doubleJumpPower = 40f;
    [SerializeField]
    private Vector3 gravity = new Vector3(0, -50f, 0);

    private int jumpCnt = 0;

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
        if (other.gameObject.CompareTag("Obstacle"))    // ��ֹ��� �ε����� ���� ����
        {
            soundManager.PlaySoundEffect(ESoundEffectType.deliveryObstacle);     // ȿ���� ����
            deliveryGameManager.Life--;
        }
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// ���� ���� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void Jump()
    {
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
        if (deliveryGameManager.GiftCount > 0)
        {
            soundManager.PlaySoundEffect(ESoundEffectType.uiButton);

            objectPoolingManager.Get(EDeliveryFlag.gift);
            deliveryGameManager.GiftCount--;
        }
    }
    #endregion
}
