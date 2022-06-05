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
    [SerializeField]
    private Image[] balls;

    [SerializeField]
    private Image ballImage;        // 뽑기에서 나온 공

    [SerializeField]
    private GameObject lever;

    private Vector3 leverRot = new Vector3(0, 0, -45);

    [SerializeField]
    private Transform dropBallPos;

    [SerializeField]
    private Animator anim;

    // 캐싱
    private GameManager gameManager;
    private GiftManager giftManager;
    private SoundManager soundManager;
    private QuestManager questManager;
    private WaitForSeconds waitFallingBall = new WaitForSeconds(0.3f);

    private int questID = 1;


    #endregion

    #region 유니티 함수
    private void Awake()
    {
        gameManager = GameManager.Instance;
        giftManager = GiftManager.Instance;
        soundManager = SoundManager.Instance;
        questManager = QuestManager.Instance;

        InitPos();
    }

    #endregion

    #region 코루틴
    /// <summary>
    /// 선물을 뽑기 위해 오브젝트들이 움직임
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveObject()
    {
        yield return null;

        soundManager.PlaySoundEffect(ESoundEffectType.giftShopLever);  // 효과음 실행

        // 레버를 돌림
        float distance = lever.transform.localEulerAngles.z - leverRot.z;
        while (distance == 45 || distance > 360)
        {
            distance = lever.transform.localEulerAngles.z - leverRot.z;

            lever.transform.localEulerAngles = Vector3.Lerp(lever.transform.localEulerAngles, leverRot, Time.deltaTime * 1.5f);

            yield return null;
        }

        yield return waitFallingBall;

        // 공을 떨어뜨림
        while (Vector3.Distance(ballImage.transform.localPosition, dropBallPos.localPosition) > 1f)
        {
            ballImage.transform.localPosition = Vector3.Lerp(ballImage.transform.localPosition, dropBallPos.localPosition, Time.deltaTime * 5f);

            yield return null;
        }

        GetRandomGift();        // 선물 획득
    }
    #endregion

    
    #region 함수
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
        gameManager.MyGold -= GoldManager.UnitToBigInteger("10.0A");              // 뽑기 비용 지불

        RandBall();

        StartCoroutine(MoveObject());
    }

    /// <summary>
    /// 레버와 공 이미지의 위치 초기 설정
    /// </summary>
    void InitPos()
    {
        lever.transform.localEulerAngles = new Vector3(0, 0, 0);

        ballImage.transform.localPosition = new Vector3(0, -300, 0);
    }

    /// <summary>
    /// 랜덤 선물을 인벤토리에 넣기
    /// </summary>
    void GetRandomGift()
    {
        questManager.Success(questID);        // 퀘스트 성공

        giftManager.ReceiveRandomGift();

        InitPos();
    }
    #endregion
}
