/**
 * @details �⼮ ���� ȹ��
 * @author ��̼�
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
    /// ������ ���� �� �ִ��� üũ
    /// </summary>
    /// <returns>������ ���� �� ������ true</returns>
    public bool CheckCanGetReward()
    {
        // ���� �޾ƾ��ϴ� ������ ���� �ʾ��� �� false ����
        if (day != "1")
        {
            if (!attendance.rewardList[int.Parse(day) - 2].isGet)
                return false;
        }

        // �ش� ������ �̹� �޾Ҵٸ� false ����
        if (isGet)
            return false;

        // ���� ������ �޾��� �� false ����
        if (gameManager.getAttendanceRewardDate == DateTime.Now.ToString("yyyy.MM.dd"))
            return false;

        return true;
    }

    /// <summary>
    /// �⼮ ���� ���� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void GetReward()
    {
        if (CheckCanGetReward())
        {
            RewardManager.GetReward(rewardType, rewardAmount);

            gameManager.getAttendanceRewardDate = DateTime.Now.ToString("yyyy.MM.dd");      // ������ �⼮ ���� ���ɳ�¥�� ���� ��¥�� ����
            uiManager.HideAttendanceNotiImage();

            isGet = true;

            /// TODO : ���� ȹ�� �Ϸ� ���� ����
        }
    }
}
