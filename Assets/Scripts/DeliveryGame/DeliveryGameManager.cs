/**
 * @brief 선물 전달 게임
 * @author 김미성
 * @date 22-05-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryGameManager : MonoBehaviour
{
    #region 변수
    [SerializeField]
    private GameObject startWindow;         // 시작 창

    [SerializeField]
    private GameObject resultWindow;        // 결과 창

    [SerializeField]
    private BackgroundMove background;

    [SerializeField]
    private BackgroundMove cloud;

    [SerializeField]
    private DeliverySanta santa;

    [SerializeField]
    private Image[] lifeImages;

    public bool isEnd;

    private int life;
    public int Life
    {
        get { return life; }
        set
        {
            life = value;

            if (life == 3)
            {
                for (int i = 0; i < lifeImages.Length; i++)
                {
                    lifeImages[i].gameObject.SetActive(true);
                }
            }
            else
            {
                lifeImages[life].gameObject.SetActive(false);
            }

            if (life <= 0)
            {
                End(false);
            }
        }
    }

    [SerializeField]
    private Text giftCountText;

    private int giftCount = 10;
    public int GiftCount
    {
        get { return giftCount; }
        set
        {
            giftCount = value;
            giftCountText.text = giftCount.ToString();
        }
    }

    [SerializeField]
    private Text satisfiedCountText;

    private int satisfiedCount = 0;
    public int SatisfiedCount
    {
        get { return satisfiedCount; }
        set
        {
            satisfiedCount = value;
            satisfiedCountText.text = satisfiedCount.ToString();
        }
    }

    [SerializeField]
    private Text puzzleCountText;

    private int puzzleCount = 0;
    public int PuzzleCount
    {
        get { return puzzleCount; }
        set
        {
            puzzleCount = value;
            puzzleCountText.text = puzzleCount.ToString();
        }
    }

    [SerializeField]
    private Text carrotCountText;

    private int carrotCount = 0;
    public int CarrotCount
    {
        get { return carrotCount; }
        set
        {
            carrotCount = value;
            carrotCountText.text = carrotCount.ToString();
        }
    }

    [SerializeField]
    private Text timeCountText;
    private int timeCount;
    public int TimeCount
    {
        get { return timeCount; }
        set
        {
            timeCount = value;
            timeCountText.text = string.Format("{0:D2}", timeCount);
        }
    }

    public Chimney preChimney = null;      // 이전에 나온 굴뚝

    // 싱글톤
    private static DeliveryGameManager instance;
    public static DeliveryGameManager Instance
    {
        get { return instance; }
    }

    // 캐싱
    private WaitForSeconds oneSec = new WaitForSeconds(1f);
    private WaitForSeconds twoSec = new WaitForSeconds(2f);

    private Inventory inventory;

    private SoundManager soundManager;

    private ObjectPoolingManager objectPoolingManager;
    #endregion

    #region 유니티 함수
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        inventory = Inventory.Instance;
        soundManager = SoundManager.Instance;
        objectPoolingManager = ObjectPoolingManager.Instance;
    }

    void Start()
    {
        startWindow.gameObject.SetActive(true);     // 시작 창 보여줌
        santa.gameObject.SetActive(false);

        GiftCount = inventory.count;
    }
    #endregion

    #region 코루틴
    /// <summary>
    /// 타임 카운트 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator StartTimeCount()
    {
        while (TimeCount > 0 && !isEnd)
        {
            yield return oneSec;

            TimeCount--;
        }

        if (!isEnd)
        {
            End(true);
        }
    }

    /// <summary>
    /// 게임 오버 코루틴
    /// </summary>
    IEnumerator GameOver(bool isClear)
    {
        isEnd = true;

        if (!isClear)       // 제한 시간 내 생명이 다해서 죽었다면 GameOver 효과음 실행
            soundManager.PlaySoundEffect(ESoundEffectType.deliveryGameOver);
        else
            soundManager.PlaySoundEffect(ESoundEffectType.deliveryGameClear);   // 아니면 GameClear 효과음 실행

        yield return twoSec;

        // 배경의 isMove를 false 변경하여 배경의 움직임을 멈춤
        background.isMove = false;
        cloud.isMove = false;

        soundManager.PlaySoundEffect(ESoundEffectType.uiButton);

        santa.gameObject.SetActive(false);
        resultWindow.gameObject.SetActive(true);        // 결과창을 띄움
    }
    DeliveryGameObject deliveryGameObject;
    /// <summary>
    /// 장애물 생성 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator CreateObstacle()
    {
        while (!isEnd)
        {
            yield return twoSec;

            //if(obstacle != null) preChimney = obstacle.GetComponent<Chimney>();


            EDeliveryFlag rand = EDeliveryFlag.chimney;

            //확률에 따라 장애물 생성
            int randInt = Random.Range(0, 100);
            if (randInt <= 45)
            {
                rand = EDeliveryFlag.chimney;             // 45%
            }
            else if (randInt <= 75)
            {
                rand = EDeliveryFlag.utilityPole;        // 30%
            }
            else if (randInt <= 100)
            {
                rand = EDeliveryFlag.bird;              // 25%
            }

            deliveryGameObject = objectPoolingManager.Get(rand).GetComponent<DeliveryGameObject>();
            deliveryGameObject.flag = rand;
           
        }
    }
    #endregion

    #region 함수
    /// <summary>
    /// 게임 시작시 초기화
    /// </summary>
    public void GameStart()
    {
        startWindow.gameObject.SetActive(false);        // 시작 창 닫기
        santa.gameObject.SetActive(true);

        soundManager.PlayBGM(EBgmType.game);           // BGM 플레이

        // 배경의 isMove를 true로 변경하여 배경을 움직이게 함
        background.isMove = true;
        cloud.isMove = true;

        Life = 3;
        TimeCount = 60;

        isEnd = false;

        StartCoroutine(StartTimeCount());
        StartCoroutine(CreateObstacle());
    }

    /// <summary>
    /// 게임 종료시 호출
    /// </summary>
    // 1. 시간이 다했을 때 (isClear = true)
    // 2. 인벤토리에 아이템이 없을 때 (isClear = true)
    // 3. 생명이 모두 다했을 때 (isClear = false)
    public void End(bool isClear)
    {
        StartCoroutine(GameOver(isClear));
    }
    #endregion
}
