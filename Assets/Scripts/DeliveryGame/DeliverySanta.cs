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


    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        Physics.gravity = gravity;
    }


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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            jumpCnt = 0;
        }
    }
}
