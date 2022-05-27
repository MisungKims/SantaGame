/**
 * @brief 선물 전달 게임
 * @author 김미성
 * @date 22-05-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

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

    [SerializeField]
    private GameObject deliveryGame;

    [SerializeField]
    private Canvas villiageCanvas;

    [SerializeField]
    private Canvas villiageWorldCanvas;


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
    private Text scoreText;

    private int score = 0;
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            scoreText.text = score.ToString();
        }
    }

    //[SerializeField]
    //private Text wishCountText;

    public int wishCount = 0;
    //public int WishCount
    //{
    //    get { return wishCount; }
    //    set
    //    {
    //        wishCount = value;
    //        wishCountText.text = wishCount.ToString();
    //    }
    //}

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

    public int carrotCount = 0;
    //public int CarrotCount
    //{
    //    get { return carrotCount; }
    //    set
    //    {
    //        carrotCount = value;
    //    }
    //}

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

    
    [SerializeField]
    private Text gaugeAmountText;

    public bool isStart;    // 게임이 시작되었는지?
    public bool isEnd;      // 게임이 종료되었는지?

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
        resultWindow.gameObject.SetActive(false);
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
        isStart = false;
        isEnd = true;

        if (!isClear)       // 제한 시간 내 생명이 다해서 죽었다면 GameOver 효과음 실행
            soundManager.PlaySoundEffect(ESoundEffectType.deliveryGameOver);
        else
            soundManager.PlaySoundEffect(ESoundEffectType.deliveryGameClear);   // 아니면 GameClear 효과음 실행

        yield return twoSec;

        // 배경의 isMove를 false 변경하여 배경의 움직임을 멈춤
        background.isMove = false;
        cloud.isMove = false;

        GetReward();

        //soundManager.PlaySoundEffect(ESoundEffectType.uiButton);

        santa.gameObject.SetActive(false);

        // 결과창을 띄우고 차례대로 값을 보여주게끔
        scoreText.gameObject.SetActive(false);
        //wishCountText.gameObject.SetActive(false);
        puzzleCountText.gameObject.SetActive(false);
        carrotCountText.gameObject.SetActive(false);
        gaugeAmountText.gameObject.SetActive(false);

        resultWindow.gameObject.SetActive(true);        
        yield return new WaitForSeconds(0.5f);

        soundManager.PlaySoundEffect(ESoundEffectType.uiButton);
        scoreText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        //soundManager.PlaySoundEffect(ESoundEffectType.uiButton);
        //wishCountText.gameObject.SetActive(true);
        //yield return new WaitForSeconds(0.5f);

        soundManager.PlaySoundEffect(ESoundEffectType.uiButton);
        puzzleCountText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        soundManager.PlaySoundEffect(ESoundEffectType.uiButton);
        carrotCountText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.7f);

        soundManager.PlaySoundEffect(ESoundEffectType.uiButton);
        gaugeAmountText.gameObject.SetActive(true);

        
    }

    /// <summary>
    /// 장애물 생성 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator CreateObstacle()
    {
        while (!isEnd)
        {
            yield return twoSec;

            EObjectFlag rand = EObjectFlag.chimney;

            //확률에 따라 장애물 생성
            int randInt = Random.Range(0, 100);
            if (randInt <= 50)
            {
                rand = EObjectFlag.chimney;             // 50%
            }
            else if (randInt <= 80)
            {
                rand = EObjectFlag.utilityPole;        // 30%
            }
            else if (randInt <= 100)
            {
                rand = EObjectFlag.bird;              // 20%
            }

            DeliveryGameObject deliveryGameObject = objectPoolingManager.Get(rand).GetComponent<DeliveryGameObject>();
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

        Score = 0;
        //WishCount = 0;
        PuzzleCount = 0;
        carrotCount = 0;

        TimeCount = 60;

        isStart = true;
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

    /// <summary>
    /// 게임 종료 후 보상 획득
    /// </summary>
    public void GetReward()
    {
        // 신뢰도 획득
        int gaugeAmount = Score + wishCount;
        GameManager.Instance.IncreaseGaugeNotAnim(gaugeAmount);

        // 결과창에 신뢰도 결과 띄우기
        StringBuilder sb = new StringBuilder();
        sb.Append("신뢰도  ");
        sb.Append(gaugeAmount);
        sb.Append("%  증가");
        gaugeAmountText.text = sb.ToString();

        // 당근 획득
        int carrotAmount = carrotCount * 10000;
        carrotCountText.text = GoldManager.ExpressUnitOfGold(carrotAmount);
        GameManager.Instance.GetCarrot(carrotAmount);

        // 퍼즐 획득
        PuzzleManager.Instance.GetManyRandomPiece(puzzleCount);
    }


    /// <summary>
    /// DeliveryGame 창을 닫음 (인스펙터에서 호출)
    /// </summary>
    public void CloseDeliverGame()
    {
        soundManager.PlayBGM(EBgmType.main);

        UIManager.Instance.SetisOpenPanel(false);

        UIManager.Instance.mainPanel.SetActive(true);
        deliveryGame.SetActive(false);
        villiageCanvas.enabled = true;
        villiageWorldCanvas.enabled = false;
    }

    ///// <summary>
    ///// 씬을 변경 (인스펙터에서 호출)
    ///// </summary>
    //public void ChangeScene()
    //{
    //    soundManager.StopBGM();           // BGM 종료
    //    GameLoadManager.LoadScene("SantaVillage");
    //}
    #endregion
}
