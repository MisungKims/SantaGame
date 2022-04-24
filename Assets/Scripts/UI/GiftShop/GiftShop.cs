/**
 * @brief 선물을 랜덤으로 뽑기
 * @author 김미성
 * @date 22-04-24
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EPresentType
{
    RCcar
}

public class GiftShop : MonoBehaviour
{
    [SerializeField]
    private Image[] balls;          // 뽑기 안에 있는 공들

    [SerializeField]
    private Image ballImage;        // 뽑기에서 나온 공

    int count;      // 선물을 뽑은 횟수

    [SerializeField]
    private Animator anim;

    // 캐싱
    private GiftManager giftManager;

    private void Awake()
    {
        giftManager = GiftManager.Instance;
    }

    private void OnEnable()
    {
        count = -1;
    }


    /// <summary>
    /// 뽑힌 공의 색을 랜덤으로 정함
    /// </summary>
    void RandBall()
    {
        int randBall = Random.Range(0, balls.Length);

        ballImage.sprite = balls[randBall].sprite;
    }

    /// <summary>
    /// 선물 뽑기 레버를 돌렸을 때 (인스펙터에서 호출)
    /// </summary>
    public void ClickLever()
    {
        Debug.Log("click");
        count++;
        anim.SetInteger("Animation", count);

        StartCoroutine(IsEndAnim());

        RandBall();
    }

    /// <summary>
    /// 애니메이션이 끝났으면 선물 받기
    /// </summary>
    /// <returns></returns>
    IEnumerator IsEndAnim()
    {
        while (true)
        {
            yield return null;

            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Zoom") || anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.No Zoom"))
                {
                    yield return new WaitForSeconds(1f);

                    break;
                }
            }
        }

        anim.SetInteger("Animation", -1);
        GetRandomGift();
    }

    /// <summary>
    /// 랜덤 선물을 인벤토리에 넣기
    /// </summary>
    void GetRandomGift()
    {
        Gift randomGift = giftManager.RandomGift();
        
        giftManager.ReceiveGift(randomGift);        // 인벤토리에 넣기
    }
}
