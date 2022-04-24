/**
 * @brief ���� ȹ�� UI
 * @author ��̼�
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GetRewardWindow : MonoBehaviour
{
    [SerializeField]
    private Text RewardName;
    [SerializeField]
    private Text RewardGrade;
    [SerializeField]
    private Image RewardImg;
    

    public bool isTouch;

    private void OnEnable()
    {
        isTouch = false;
    }

    /// <summary>
    /// ���� ȹ�� â�� �� �� �ʱ� ����
    /// </summary>
    /// <param name="sprite">������ �̹���</param>
    public void OpenWindow(Gift gift)
    {
        this.gameObject.SetActive(true);
        RewardName.text = gift.giftName;
        RewardGrade.text = Enum.GetName(typeof(EGiftGrade), (int)gift.giftGrade);
        RewardImg.sprite = gift.giftImage;
    }

    /// <summary>
    /// ���� ȹ�� â�� �� �� �ʱ� ����
    /// </summary>
    /// <param name="sprite">������ �̹���</param>
    public void OpenWindow(string name, Sprite sprite)
    {
        this.gameObject.SetActive(true);
        RewardName.text = name;
        RewardGrade.text = "";
        RewardImg.sprite = sprite;
    }

    /// <summary>
    /// ���� ȹ�� â�� ���� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void CloseWindow()
    {
        isTouch = true;
        this.gameObject.SetActive(false);
    }
}
