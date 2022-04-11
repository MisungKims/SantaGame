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


    public EReward eReward;

    private bool isGet;

    private GameManager gameManager;
    private Attendance attendance;

    private void Start()
    {
        gameManager = GameManager.Instance;
        attendance = Attendance.Instance;
    }

    // �⼮ ���� ����
    public void GetReward()
    {
        // ���� �޾ƾ��ϴ� ������ ���� �ʾ��� �� return
        if (day != "1")
        {
            if (!attendance.rewardList[int.Parse(day) - 2].isGet)
                return;
        }

        // ���� ������ �޾Ұų�, �ش� ������ �̹� �޾Ҵٸ� return
        if (isGet || gameManager.getAttendanceRewardDate == DateTime.Now.ToString("yyyy.MM.dd"))
            return;

        // �� ������ ������ ���� ����
        switch (eReward)
        {
            case EReward.gold:
                gameManager.MyGold += GoldManager.UnitToBigInteger(rewardAmount);
                break;
            case EReward.dia:
                gameManager.MyDia += int.Parse(rewardAmount);
                break;
            case EReward.puzzle:
                break;
            case EReward.clothesBox:
                break;
            case EReward.carrot:
                gameManager.MyCarrots += GoldManager.UnitToBigInteger(rewardAmount);
                break;
            default:
                break;
        }

        gameManager.GetAttendanceReward();

        isGet = true;
    }
}
