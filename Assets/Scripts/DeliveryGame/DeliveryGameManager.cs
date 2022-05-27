/**
 * @brief ���� ���� ����
 * @author ��̼�
 * @date 22-05-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class DeliveryGameManager : MonoBehaviour
{
    #region ����
    [SerializeField]
    private GameObject startWindow;         // ���� â

    [SerializeField]
    private GameObject resultWindow;        // ��� â

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

    public bool isStart;    // ������ ���۵Ǿ�����?
    public bool isEnd;      // ������ ����Ǿ�����?

    // �̱���
    private static DeliveryGameManager instance;
    public static DeliveryGameManager Instance
    {
        get { return instance; }
    }

    // ĳ��
    private WaitForSeconds oneSec = new WaitForSeconds(1f);
    private WaitForSeconds twoSec = new WaitForSeconds(2f);

    private Inventory inventory;

    private SoundManager soundManager;

    private ObjectPoolingManager objectPoolingManager;
    #endregion

    #region ����Ƽ �Լ�
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
        startWindow.gameObject.SetActive(true);     // ���� â ������
        resultWindow.gameObject.SetActive(false);
        santa.gameObject.SetActive(false);

        GiftCount = inventory.count;
    }
    #endregion

    #region �ڷ�ƾ
    /// <summary>
    /// Ÿ�� ī��Ʈ �ڷ�ƾ
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
    /// ���� ���� �ڷ�ƾ
    /// </summary>
    IEnumerator GameOver(bool isClear)
    {
        isStart = false;
        isEnd = true;

        if (!isClear)       // ���� �ð� �� ������ ���ؼ� �׾��ٸ� GameOver ȿ���� ����
            soundManager.PlaySoundEffect(ESoundEffectType.deliveryGameOver);
        else
            soundManager.PlaySoundEffect(ESoundEffectType.deliveryGameClear);   // �ƴϸ� GameClear ȿ���� ����

        yield return twoSec;

        // ����� isMove�� false �����Ͽ� ����� �������� ����
        background.isMove = false;
        cloud.isMove = false;

        GetReward();

        //soundManager.PlaySoundEffect(ESoundEffectType.uiButton);

        santa.gameObject.SetActive(false);

        // ���â�� ���� ���ʴ�� ���� �����ְԲ�
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
    /// ��ֹ� ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator CreateObstacle()
    {
        while (!isEnd)
        {
            yield return twoSec;

            EObjectFlag rand = EObjectFlag.chimney;

            //Ȯ���� ���� ��ֹ� ����
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

    #region �Լ�
    /// <summary>
    /// ���� ���۽� �ʱ�ȭ
    /// </summary>
    public void GameStart()
    {
        startWindow.gameObject.SetActive(false);        // ���� â �ݱ�
        santa.gameObject.SetActive(true);

        soundManager.PlayBGM(EBgmType.game);           // BGM �÷���

        // ����� isMove�� true�� �����Ͽ� ����� �����̰� ��
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
    /// ���� ����� ȣ��
    /// </summary>
    // 1. �ð��� ������ �� (isClear = true)
    // 2. �κ��丮�� �������� ���� �� (isClear = true)
    // 3. ������ ��� ������ �� (isClear = false)
    public void End(bool isClear)
    {
        StartCoroutine(GameOver(isClear));
    }

    /// <summary>
    /// ���� ���� �� ���� ȹ��
    /// </summary>
    public void GetReward()
    {
        // �ŷڵ� ȹ��
        int gaugeAmount = Score + wishCount;
        GameManager.Instance.IncreaseGaugeNotAnim(gaugeAmount);

        // ���â�� �ŷڵ� ��� ����
        StringBuilder sb = new StringBuilder();
        sb.Append("�ŷڵ�  ");
        sb.Append(gaugeAmount);
        sb.Append("%  ����");
        gaugeAmountText.text = sb.ToString();

        // ��� ȹ��
        int carrotAmount = carrotCount * 10000;
        carrotCountText.text = GoldManager.ExpressUnitOfGold(carrotAmount);
        GameManager.Instance.GetCarrot(carrotAmount);

        // ���� ȹ��
        PuzzleManager.Instance.GetManyRandomPiece(puzzleCount);
    }


    /// <summary>
    /// DeliveryGame â�� ���� (�ν����Ϳ��� ȣ��)
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
    ///// ���� ���� (�ν����Ϳ��� ȣ��)
    ///// </summary>
    //public void ChangeScene()
    //{
    //    soundManager.StopBGM();           // BGM ����
    //    GameLoadManager.LoadScene("SantaVillage");
    //}
    #endregion
}
