/**
 * @brief 선물을 랜덤으로 뽑기
 * @author 김미성
 * @date 22-04-24
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GiftShop : MonoBehaviour
{
    #region 변수
    enum EShopState
    {
        idle,
        clickLever,
        moveBalls,
        dropBall
    }

    EShopState shopState;

    [SerializeField]
    private Image[] balls;          

    //[SerializeField]
    //private GameObject ballsObj;        // 뽑기 안에 있는 공들

    //private Vector3 originBallsPos = new Vector3(9, -61, 0);
    //private Vector3 ballsPos1 = new Vector3(-85, 33, 0);
    //private Vector3 ballsPos2 = new Vector3(5, 100, 0);
    //private Vector3 ballsPos3 = new Vector3(74, 14, 0);

    //private Vector3 originBallsRot = new Vector3(0, 0, 0);
    //private Vector3 ballsRot1 = new Vector3(0, 0, -90);
    //private Vector3 ballsRot2 = new Vector3(0, 0, -180);
    //private Vector3 ballsRot3 = new Vector3(0, 0, -270);
    //private Vector3 ballsRot4 = new Vector3(0, 0, -360);

    [SerializeField]
    private Image ballImage;        // 뽑기에서 나온 공

    [SerializeField]
    private GameObject lever;

    private Vector3 leverRot = new Vector3(0, 0, -45);

    [SerializeField]
    private Transform dropBallPos;

    int count;      // 선물을 뽑은 횟수

    [SerializeField]
    private Animator anim;

    // 캐싱
    private GiftManager giftManager;

    private int questID = 1;
    #endregion



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
        count++;
        //anim.SetInteger("Animation", count);

        InitPos();
        StartCoroutine(MoveObject());

        //StartCoroutine(IsEndAnim());

        RandBall();
    }

    void InitPos()
    {
        lever.transform.localEulerAngles = new Vector3(0, 0, 0);

        ballImage.transform.localPosition = new Vector3(0, -300, 0);
    }


    IEnumerator MoveObject()
    {
        yield return null;

        SoundManager.Instance.PlaySoundEffect(ESoundEffectType.giftShopLever);  // 효과음 실행

        float distance = lever.transform.localEulerAngles.z - leverRot.z;
        while (distance == 45 || distance > 360)
        {
            distance = lever.transform.localEulerAngles.z - leverRot.z;

            lever.transform.localEulerAngles = Vector3.Lerp(lever.transform.localEulerAngles, leverRot, Time.deltaTime * 1.5f);

            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        while (Vector3.Distance(ballImage.transform.localPosition, dropBallPos.localPosition) > 1f)
        {
            ballImage.transform.localPosition = Vector3.Lerp(ballImage.transform.localPosition, dropBallPos.localPosition, Time.deltaTime * 5f);
           
            yield return null;
        }

        GetRandomGift();


        ////distance = Vector3.Distance(ballsObj.transform.localPosition, ballsPos1);
        //while (Vector3.Distance(ballsObj.transform.localPosition, ballsPos1) > 2f)
        //{
        //    ballsObj.transform.localPosition = Vector3.Lerp(ballsObj.transform.localPosition, ballsPos1, Time.deltaTime * 10f);
        //    //ballsObj.transform.localEulerAngles = Vector3.Lerp(ballsObj.transform.localEulerAngles, ballsRot1, Time.deltaTime * 1f);
        //    Debug.Log(ballsObj.transform.localEulerAngles);

        //    yield return null;
        //}

        //while (Vector3.Distance(ballsObj.transform.localPosition, ballsPos2) > 2f)
        //{
        //    ballsObj.transform.localPosition = Vector3.Lerp(ballsObj.transform.localPosition, ballsPos2, Time.deltaTime * 10f);

        //    yield return null;
        //}

        //while (Vector3.Distance(ballsObj.transform.localPosition, ballsPos3) > 2f)
        //{
        //    ballsObj.transform.localPosition = Vector3.Lerp(ballsObj.transform.localPosition, ballsPos3, Time.deltaTime * 10f);

        //    yield return null;
        //}

        //while (Vector3.Distance(ballsObj.transform.localPosition, originBallsPos) > 2f)
        //{
        //    ballsObj.transform.localPosition = Vector3.Lerp(ballsObj.transform.localPosition, originBallsPos, Time.deltaTime * 10f);

        //    yield return null;
        //}


    }

    /// <summary>
    /// 공 굴리기 애니메이션이 끝났으면 선물 받기
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
        QuestManager.Instance.Success(questID);        // 퀘스트 성공

        giftManager.ReceiveRandomGift();
    }
}
