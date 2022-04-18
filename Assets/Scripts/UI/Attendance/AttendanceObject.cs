/**
 * @details 출석 보상 획득
 * @author 김미성
 * @date 22-04-18
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

    private bool isGet;

    private GameManager gameManager;
    private Attendance attendance;
    private UIManager uiManager;


    private void Start()
    {
        gameManager = GameManager.Instance;
        attendance = Attendance.Instance;
        uiManager = UIManager.Instance;
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
            if (!attendance.rewardList[int.Parse(day) - 2].isGet)
                return false;
        }

        // 해당 보상을 이미 받았다면 false 리턴
        if (isGet)
            return false;

        // 오늘 보상을 받았을 때 false 리턴
        if (gameManager.getAttendanceRewardDate == DateTime.Now.ToString("yyyy.MM.dd"))
            return false;

        return true;
    }

    /// <summary>
    /// 출석 보상 수령 (인스펙터에서 호출)
    /// </summary>
    public void GetReward()
    {
        if (CheckCanGetReward())
        {
            RewardManager.GetReward(rewardType, rewardAmount);

            gameManager.getAttendanceRewardDate = DateTime.Now.ToString("yyyy.MM.dd");      // 마지막 출석 보상 수령날짜를 오늘 날짜로 변경
            uiManager.HideAttendanceNotiImage();

            isGet = true;

            /// TODO : 보상 획득 완료 도장 생성
        }
    }
}
