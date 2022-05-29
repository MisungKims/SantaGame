/**
 * @brief 퀘스트 UI, 완료 시 보상 획득
 * @author 김미성
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestObject : MonoBehaviour
{
    #region 변수
    // UI 변수
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
    [SerializeField]
    private Button getRewardButton;
    [SerializeField]
    private Image rewardImage;

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
        get { return questCount; }
        set
        {
            questCount = value;
            countText.text = questCount.ToString();
            questCountSlider.value = questCount;

            if (questCount >= questMaxCount)
            {
                quest.isSuccess = true;
                getRewardButton.interactable = true;
            }
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

    public bool isSuccess = false;      // 퀘스트를 모두 완료했는지
    public bool isGetReward = false;     // 보상을 받았는지

    //public EQuestType questType;
    public ERewardType rewardType;

    Quest quest;
    #endregion


    #region 유니티 함수
    void Awake()
    {
        if (!quest.isSuccess)
            getRewardButton.interactable = false;
    }
    #endregion


    #region 함수
    /// <summary>
    /// UI 오브젝트의 값들을 Set
    /// </summary>
    public void Init(Quest quest)
    {
        this.quest = quest;
        this.name = quest.name;

        questNameText.text = name;

        QuestMaxCount = quest.maxCount;
        QuestCount = quest.count;
       
        rewardType = RewardManager.StringToRewardType(quest.rewardType);
        rewardImage.sprite = RewardManager.Instance.rewardImages[(int)rewardType];   // 보상의 종류에 따라 이미지 set

        QuestRewardAmount = quest.amount;

        //Debug.Log(quest.name + " " + quest.isGetReward);

        if (quest.isGetReward)
        {
            rewardText.text = "완료";
            getRewardButton.interactable = false;
        }
        else if (quest.isSuccess)
        {
            getRewardButton.interactable = true;
        }
        

        //isSuccess = quest.isSuccess;
        //isGetReward = quest.isGetReward;
    }


    /// <summary>
    /// 퀘스트 완료 버튼 클릭 (인스펙터에서 호출)
    /// </summary>
    public void ClickSuccessButton()
    {
        if (quest.isSuccess && !quest.isGetReward)
        {
            rewardText.text = "완료";
            getRewardButton.interactable = false;
            quest.isGetReward = true;

            RewardManager.GetReward(rewardType, questRewardAmount);

            // 받을 보상이 있다는 걸 알려주는 notification Image를 숨김
            QuestManager.Instance.notificationImage.SetActive(false);
        }
    }

    /// <summary>
    /// 다음 날이 되어 초기화할 때
    /// </summary>
    public void NextDay()
    {
        quest.isSuccess = false;
        quest.isGetReward = false;

        getRewardButton.interactable = false;

        rewardText.text = questRewardAmount;

        QuestCount = 0;
    }
    #endregion
}
