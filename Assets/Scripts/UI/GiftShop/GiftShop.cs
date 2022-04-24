/**
 * @brief ¼±¹°À» ·£´ýÀ¸·Î »Ì±â
 * @author ±è¹Ì¼º
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
    private Image[] balls;          // »Ì±â ¾È¿¡ ÀÖ´Â °øµé

    [SerializeField]
    private Image ballImage;        // »Ì±â¿¡¼­ ³ª¿Â °ø

    int count;      // ¼±¹°À» »ÌÀº È½¼ö

    [SerializeField]
    private Animator anim;

    // Ä³½Ì
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
    /// »ÌÈù °øÀÇ »öÀ» ·£´ýÀ¸·Î Á¤ÇÔ
    /// </summary>
    void RandBall()
    {
        int randBall = Random.Range(0, balls.Length);

        ballImage.sprite = balls[randBall].sprite;
    }

    /// <summary>
    /// ¼±¹° »Ì±â ·¹¹ö¸¦ µ¹·ÈÀ» ¶§ (ÀÎ½ºÆåÅÍ¿¡¼­ È£Ãâ)
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
        //UIManager.Instance.getRewardWindow.OpenWindow();      // º¸»ó È¹µæÃ¢ º¸¿©ÁÜ
    }    
}
