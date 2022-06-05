/**
 * @brief ������ �������� �̱�
 * @author ��̼�
 * @date 22-04-24
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftShop : MonoBehaviour
{
    #region ����
    [SerializeField]
    private Image[] balls;

    [SerializeField]
    private Image ballImage;        // �̱⿡�� ���� ��

    [SerializeField]
    private GameObject lever;

    private Vector3 leverRot = new Vector3(0, 0, -45);

    [SerializeField]
    private Transform dropBallPos;

    [SerializeField]
    private Animator anim;

    // ĳ��
    private GameManager gameManager;
    private GiftManager giftManager;
    private SoundManager soundManager;
    private QuestManager questManager;
    private WaitForSeconds waitFallingBall = new WaitForSeconds(0.3f);

    private int questID = 1;


    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        gameManager = GameManager.Instance;
        giftManager = GiftManager.Instance;
        soundManager = SoundManager.Instance;
        questManager = QuestManager.Instance;

        InitPos();
    }

    #endregion

    #region �ڷ�ƾ
    /// <summary>
    /// ������ �̱� ���� ������Ʈ���� ������
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveObject()
    {
        yield return null;

        soundManager.PlaySoundEffect(ESoundEffectType.giftShopLever);  // ȿ���� ����

        // ������ ����
        float distance = lever.transform.localEulerAngles.z - leverRot.z;
        while (distance == 45 || distance > 360)
        {
            distance = lever.transform.localEulerAngles.z - leverRot.z;

            lever.transform.localEulerAngles = Vector3.Lerp(lever.transform.localEulerAngles, leverRot, Time.deltaTime * 1.5f);

            yield return null;
        }

        yield return waitFallingBall;

        // ���� ����߸�
        while (Vector3.Distance(ballImage.transform.localPosition, dropBallPos.localPosition) > 1f)
        {
            ballImage.transform.localPosition = Vector3.Lerp(ballImage.transform.localPosition, dropBallPos.localPosition, Time.deltaTime * 5f);

            yield return null;
        }

        GetRandomGift();        // ���� ȹ��
    }
    #endregion

    
    #region �Լ�
    /// <summary>
    /// ���� ���� ���� �������� ����
    /// </summary>
    void RandBall()
    {
        int randBall = Random.Range(0, balls.Length);

        ballImage.sprite = balls[randBall].sprite;
    }

    /// <summary>
    /// ���� �̱� ������ ������ �� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void ClickLever()
    {
        gameManager.MyGold -= GoldManager.UnitToBigInteger("10.0A");              // �̱� ��� ����

        RandBall();

        StartCoroutine(MoveObject());
    }

    /// <summary>
    /// ������ �� �̹����� ��ġ �ʱ� ����
    /// </summary>
    void InitPos()
    {
        lever.transform.localEulerAngles = new Vector3(0, 0, 0);

        ballImage.transform.localPosition = new Vector3(0, -300, 0);
    }

    /// <summary>
    /// ���� ������ �κ��丮�� �ֱ�
    /// </summary>
    void GetRandomGift()
    {
        questManager.Success(questID);        // ����Ʈ ����

        giftManager.ReceiveRandomGift();

        InitPos();
    }
    #endregion
}
