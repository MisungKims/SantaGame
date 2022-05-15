/**
 * @brief 선물 전달 게임의 산타
 * @author 김미성
 * @date 22-05-14
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverySanta : MonoBehaviour
{
    private Rigidbody rigid;

    [SerializeField]
    private float jumpPower = 50f;
    [SerializeField]
    private float doubleJumpPower = 40f;
    [SerializeField]
    private Vector3 gravity = new Vector3(0, -50f, 0);

    private int jumpCnt = 0;

    [SerializeField]
    private GameObject[] gifts;
    

    public void Jump()
    {
        if (jumpCnt == 0)
        {
            jumpCnt++;
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
        else if (jumpCnt == 1)
        {
            jumpCnt++;
            rigid.AddForce(Vector3.up * doubleJumpPower, ForceMode.Impulse);
        }
        else return;
    }

    public void Drop()
    {
        if (DeliveryGameManager.Instance.GiftCount > 0)
        {
            DeliveryGameManager.Instance.GiftCount--;

            int rand = Random.Range(0, 1);
            ObjectPoolingManager.Instance.Get((EDeliveryFlag)rand);

            if (DeliveryGameManager.Instance.GiftCount == 0)
            {
                DeliveryGameManager.Instance.End();
            }
        }
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        Physics.gravity = gravity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))       // 땅에 닿으면 jumpCnt를 초기화
        {
            jumpCnt = 0;
        }
    }
}
