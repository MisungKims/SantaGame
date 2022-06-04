/**
 * @details �⼮ ���� UI
 * @author ��̼�
 * @date 22-04-22
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AttendanceObject : MonoBehaviour
{
    #region ����
    // UI ����
    [SerializeField]
    private Text RewardAmountText;
    [SerializeField]
    private Text DayText;
    [SerializeField]
    public Image RewardImage;
    [SerializeField]
    public GameObject CheckImage;

    // ������Ƽ
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

    // ���� ����
    public ERewardType rewardType;

    // ĳ��
    private AttendanceManager attendanceManager;
    #endregion


    #region ����Ƽ �Լ�
    private void Awake()
    {
        attendanceManager = AttendanceManager.Instance;
    }

    private void OnEnable()
    {
        SetCheckImage();
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// ���� ���� ���ο� ���� Check Image �����ִ� ���� ����
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
    /// �⼮ ���� ���� (�ν����Ϳ��� ȣ��)
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
