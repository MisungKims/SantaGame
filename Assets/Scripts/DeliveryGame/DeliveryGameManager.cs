/**
 * @brief ���� ���� ����
 * @author ��̼�
 * @date 22-05-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Chimney preChimney = null;      // ������ ���� ����

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
        isEnd = true;

        if (!isClear)       // ���� �ð� �� ������ ���ؼ� �׾��ٸ� GameOver ȿ���� ����
            soundManager.PlaySoundEffect(ESoundEffectType.deliveryGameOver);
        else
            soundManager.PlaySoundEffect(ESoundEffectType.deliveryGameClear);   // �ƴϸ� GameClear ȿ���� ����

        yield return twoSec;

        // ����� isMove�� false �����Ͽ� ����� �������� ����
        background.isMove = false;
        cloud.isMove = false;

        soundManager.PlaySoundEffect(ESoundEffectType.uiButton);

        santa.gameObject.SetActive(false);
        resultWindow.gameObject.SetActive(true);        // ���â�� ���
    }
    DeliveryGameObject deliveryGameObject;
    /// <summary>
    /// ��ֹ� ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator CreateObstacle()
    {
        while (!isEnd)
        {
            yield return twoSec;

            //if(obstacle != null) preChimney = obstacle.GetComponent<Chimney>();


            EDeliveryFlag rand = EDeliveryFlag.chimney;

            //Ȯ���� ���� ��ֹ� ����
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
        TimeCount = 60;

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
    #endregion
}
