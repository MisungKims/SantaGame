/**
 * @details 출석 보상 UI
 * @author 김미성
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AttendanceObject : MonoBehaviour
{
    [SerializeField]
    private Text RewardAmountText;
    [SerializeField]
    private Text DayText;
    [SerializeField]
    public Image RewardImage;

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


    public ERewardType rewardType;

    // 캐싱
    private AttendanceManager attendanceManager;


    private void Start()
    {
        attendanceManager = AttendanceManager.Instance;
    }

   
    /// <summary>
    /// 보상을 받을 수 있는지 체크
    /// </summary>
    /// <returns>보상을 받을 수 있으면 true</returns>
    public bool CheckCanGetReward()
    {
        // 먼저 받아야하는 보상을 받지 않았을 때 false 리턴
        if (day != "1")
        {
            /*attendance.rewardList[int.Parse(day) - 2].isGet*/
            if (!attendanceManager.attendanceList[int.Parse(day) - 2].isGet)
                return false;
        }

        // 해당 보상을 이미 받았다면 false 리턴
        if (attendanceManager.attendanceList[int.Parse(day) - 2].isGet)
            return false;

        // 오늘 보상을 받았을 때 false 리턴
        if (attendanceManager.getRewardDate == DateTime.Now.ToString("yyyy.MM.dd"))
            return false;

        return true;
    }

    /// <summary>
    /// 출석 보상 수령 (인스펙터에서 호출)
    /// </summary>
    public void GetReward()
    {
        attendanceManager.GetReward(day);
    }
}
