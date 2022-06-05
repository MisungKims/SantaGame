/**
 * @details 출석 보상 UI
 * @author 김미성
 * @date 22-04-22
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AttendanceObject : MonoBehaviour
{
    #region 변수
    // UI 변수
    [SerializeField]
    private Text RewardAmountText;
    [SerializeField]
    private Text DayText;
    [SerializeField]
    public Image RewardImage;
    [SerializeField]
    public GameObject CheckImage;

    // 프로퍼티
    private string rewardAmount;
    public string RewardAmount
    {
        set
        {
            rewardAmount = value;
            RewardAmountText.text = rewardAmount;
        }
    }

    private string day;
    public string Day
    {
        set
        {
            day = value;
            DayText.text = day;
        }
    }

    // 보상 종류
    public ERewardType rewardType;

    // 캐싱
    private AttendanceManager attendanceManager;
    #endregion


    #region 유니티 함수
    private void Awake()
    {
        attendanceManager = AttendanceManager.Instance;
    }

    private void OnEnable()
    {
        SetCheckImage();
    }
    #endregion

    #region 함수
    /// <summary>
    /// 보상 수령 여부에 따라 Check Image 보여주는 여부 설정
    /// </summary>
    void SetCheckImage()
    {
        if (attendanceManager.attendanceList[int.Parse(day) - 1].isGet)
        {
            CheckImage.SetActive(true);
        }
        else
        {
            CheckImage.SetActive(false);
        }
    }


    /// <summary>
    /// 출석 보상 수령 (인스펙터에서 호출)
    /// </summary>
    public void GetReward()
    {
        if (attendanceManager.GetReward(day))
        {
            SoundManager.Instance.PlaySoundEffect(ESoundEffectType.stamp);
            CheckImage.SetActive(true);
        }
    }
    #endregion
}
