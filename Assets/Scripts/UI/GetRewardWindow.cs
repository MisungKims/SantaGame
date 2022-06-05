/**
 * @brief 보상 획득 UI
 * @author 김미성
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GetRewardWindow : MonoBehaviour
{
    #region 변수
    [SerializeField]
    private Text RewardName;
    [SerializeField]
    private Text RewardGrade;
    [SerializeField]
    private Image RewardImg;

    public bool isTouch;        // UI를 터치했는지?
    #endregion

    #region 유니티 함수
    private void OnEnable()
    {
        isTouch = false;
    }
    #endregion

    #region 함수
    /// <summary>
    /// 선물 획득 창을 열 때 초기 설정
    /// </summary>
    /// <param name="gift">획득할 선물</param>
    public void OpenWindow(Gift gift)
    {
        this.gameObject.SetActive(true);
        RewardName.text = gift.giftName;
        RewardGrade.text = Enum.GetName(typeof(EGiftGrade), (int)gift.giftGrade);
        RewardImg.sprite = gift.giftImage;
    }

    /// <summary>
    /// 보상 획득 창을 열 때 초기 설정
    /// </summary>
    /// <param name="name">보여줄 이름</param>
    /// <param name="sprite">보여줄 이미지</param>
    public void OpenWindow(string name, Sprite sprite)
    {
        this.gameObject.SetActive(true);
        RewardName.text = name;
        RewardGrade.text = "";
        RewardImg.sprite = sprite;
    }

    /// <summary>
    /// 보상 획득 창을 닫음 (인스펙터에서 호출)
    /// </summary>
    public void CloseWindow()
    {
        isTouch = true;
        this.gameObject.SetActive(false);
    }
    #endregion
}
