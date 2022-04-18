/**
 * @brief ����Ʈ UI �� �Ϸ� �� ���� ȹ��
 * @author ��̼�
 * @date 22-04-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuestObject : MonoBehaviour
{
    [SerializeField]
    private Text questNameText;
    [SerializeField]
    private Slider questCountSlider;
    [SerializeField]
    private Text maxCountText;
    [SerializeField]
    private Text countText;
    [SerializeField]
    private Text rewardText;

    public bool isSuccess = false;      // ����Ʈ�� ��� �Ϸ��ߴ���
    private bool isGetReward = false;     // ������ �޾Ҵ���

    public EQuestType questType;

    public ERewardType rewardType;

    private string questName;
    public string QuestName
    {
        set
        {
            questName = value;
            questNameText.text = questName;
        }
    }

    private int questMaxCount;
    public int QuestMaxCount
    {
        set
        {
            questMaxCount = value;
            maxCountText.text = questMaxCount.ToString();
            questCountSlider.maxValue = questMaxCount;
        }
    }

    private int questCount;
    public int QuestCount
    {
        set
        {
            questCount = value;
            countText.text = questCount.ToString();
            questCountSlider.value = questCount;

            if (questCount >= questMaxCount)
                isSuccess = true;
        }
    }

    private string questRewardAmount;
    public string QuestRewardAmount
    {
        set
        {
            questRewardAmount = value;
            rewardText.text = questRewardAmount;
        }
    }

    /// <summary>
    /// ����Ʈ �Ϸ� ��ư Ŭ�� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void ClickSuccessButton()
    {
        if (isSuccess && !isGetReward)
        {
            rewardText.text = "�Ϸ�";

            RewardManager.GetReward(rewardType, questRewardAmount);

            if (questType == EQuestType.daily)                   // ���Ϲ̼��̶�� 
                DailyQuestWindow.Instance.CheckAllSuccess();    //  ��� �Ϸ��ߴ��� üũ
        }
    }
}
