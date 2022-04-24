/**
 * @brief ������ �������� �̱�
 * @author ��̼�
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
    private Image[] balls;          // �̱� �ȿ� �ִ� ����

    [SerializeField]
    private Image ballImage;        // �̱⿡�� ���� ��

    int count;      // ������ ���� Ƚ��

    [SerializeField]
    private Animator anim;

    // ĳ��
    private GetRewardWindow getRewardWindow;

    private void Awake()
    {
        getRewardWindow = UIManager.Instance.getRewardWindow;
    }

    private void OnEnable()
    {
        count = -1;
    }


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
        count++;
        anim.SetInteger("Animation", count);

        StartCoroutine(IsEndAnim());

        RandBall();
    }

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
        OpenGetRewardUI();
    }

    void OpenGetRewardUI()
    {

        Debug.Log("open");
        //UIManager.Instance.getRewardWindow.OpenWindow();      // ���� ȹ��â ������
    }    
}
