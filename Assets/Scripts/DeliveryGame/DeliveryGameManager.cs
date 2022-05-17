/**
 * @brief ���� ���� ����
 * @author ��̼�
 * @date 22-05-14
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryGameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject startWindow;

    [SerializeField]
    private GameObject resultWindow;

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
                End();
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
            SatisfiedCount = value;
            satisfiedCountText.text = SatisfiedCount.ToString();
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


    // �̱���
    private static DeliveryGameManager instance;
    public static DeliveryGameManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    void Start()
    {
        startWindow.gameObject.SetActive(true);
        santa.gameObject.SetActive(false);

        GiftCount = Inventory.Instance.count;
    }

    public void GameStart()
    {
        SoundManager.Instance.PlayBGM(EBgmType.game);

        startWindow.gameObject.SetActive(false);
        santa.gameObject.SetActive(true);

        // ����� isMove�� true�� �����Ͽ� ����� �����̰� ��
        background.isMove = true;
        cloud.isMove = true;

        Life = 3;

        isEnd = false;
        StartCoroutine(CreateObstacle());
    }

    public void End()
    {
        santa.gameObject.SetActive(false);
        resultWindow.gameObject.SetActive(true);

        // ����� isMove�� true�� �����Ͽ� ����� �����̰� ��
        background.isMove = false;
        cloud.isMove = false;

        isEnd = true;
    }

    /// <summary>
    /// ��ֹ� ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator CreateObstacle()
    {
        while (!isEnd)
        {
            yield return new WaitForSeconds(2f);

            EDeliveryFlag rand = (EDeliveryFlag)Random.Range(1, 1);
            Obstacle obstacle = ObjectPoolingManager.Instance.Get(rand).GetComponent<Obstacle>();
            obstacle.flag = rand;
        }
    }
}
