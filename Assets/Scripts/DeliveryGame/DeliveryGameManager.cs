/**
 * @brief 선물 전달 게임
 * @author 김미성
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


    // 싱글톤
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

        //GiftCount = Inventory.Instance.count;
    }

    public void GameStart()
    {
       // SoundManager.Instance.PlayBGM(EBgmType.game);

        startWindow.gameObject.SetActive(false);
        santa.gameObject.SetActive(true);

        // 배경의 isMove를 true로 변경하여 배경을 움직이게 함
        background.isMove = true;
        cloud.isMove = true;

        Life = 3;
        TimeCount = 60;

        isEnd = false;

        StartCoroutine(StartTimeCount());
        StartCoroutine(CreateObstacle());
    }

    public void End()
    {
        StartCoroutine(GameOver());
    }

    IEnumerator StartTimeCount()
    {
        while (TimeCount > 0)
        {
            yield return new WaitForSeconds(1f);

            TimeCount--;
        }

        End();
    }

    IEnumerator GameOver()
    {
        // 배경의 isMove를 false 변경하여 배경의 움직임을 멈춤
        background.isMove = false;
        cloud.isMove = false;

        isEnd = true;

        yield return new WaitForSeconds(1f);

        santa.gameObject.SetActive(false);
        resultWindow.gameObject.SetActive(true);
    }

    /// <summary>
    /// 장애물 생성 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator CreateObstacle()
    {
        while (!isEnd)
        {
            yield return new WaitForSeconds(2f);

            EDeliveryFlag rand = EDeliveryFlag.chimney;

            // 장애물이 나올 확률
            int randInt = Random.Range(1, 100);
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

            Obstacle obstacle = ObjectPoolingManager.Instance.Get(rand).GetComponent<Obstacle>();
            obstacle.flag = rand;
        }
    }
}
