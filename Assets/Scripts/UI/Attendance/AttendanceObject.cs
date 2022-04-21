/**
 * @details �⼮ ���� UI
 * @author ��̼�
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

    // ĳ��
    private AttendanceManager attendanceManager;


    private void Start()
    {
        attendanceManager = AttendanceManager.Instance;
    }

   
    /// <summary>
    /// ������ ���� �� �ִ��� üũ
    /// </summary>
    /// <returns>������ ���� �� ������ true</returns>
    public bool CheckCanGetReward()
    {
        // ���� �޾ƾ��ϴ� ������ ���� �ʾ��� �� false ����
        if (day != "1")
        {
            /*attendance.rewardList[int.Parse(day) - 2].isGet*/
            if (!attendanceManager.attendanceList[int.Parse(day) - 2].isGet)
                return false;
        }

        // �ش� ������ �̹� �޾Ҵٸ� false ����
        if (attendanceManager.attendanceList[int.Parse(day) - 2].isGet)
            return false;

        // ���� ������ �޾��� �� false ����
        if (attendanceManager.getRewardDate == DateTime.Now.ToString("yyyy.MM.dd"))
            return false;

        return true;
    }

    /// <summary>
    /// �⼮ ���� ���� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void GetReward()
    {
        attendanceManager.GetReward(day);
    }
}
