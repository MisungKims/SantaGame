/**
 * @brief 퀘스트 UI 및 완료 시 보상 획득
 * @author 김미성
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

    public bool isSuccess = false;      // 퀘스트를 모두 완료했는지
    private bool isGetReward = false;     // 보상을 받았는지

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
    /// 퀘스트 완료 버튼 클릭 (인스펙터에서 호출)
    /// </summary>
    public void ClickSuccessButton()
    {
        if (isSuccess && !isGetReward)
        {
            rewardText.text = "완료";

            RewardManager.GetReward(rewardType, questRewardAmount);

            if (questType == EQuestType.daily)                   // 일일미션이라면 
                DailyQuestWindow.Instance.CheckAllSuccess();    //  모두 완료했는지 체크
        }
    }
}
