/**
 * @brief ����Ʈ UI, �Ϸ� �� ���� ȹ��
 * @author ��̼�
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestObject : MonoBehaviour
{
    #region ����
    // UI ����
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
                isSuccess = true;
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

    private bool isSuccess = false;      // ����Ʈ�� ��� �Ϸ��ߴ���
    private bool isGetReward = false;     // ������ �޾Ҵ���

    public EQuestType questType;
    public ERewardType rewardType;
    #endregion


    #region ����Ƽ �Լ�
    void Awake()
    {
        if (!isSuccess)
            getRewardButton.interactable = false;
    }
    #endregion


    #region �Լ�
    /// <summary>
    /// UI ������Ʈ�� ������ Set
    /// </summary>
    public void Init(Quest quest)
    {
        this.name = quest.name;

        questNameText.text = name;

        QuestMaxCount = quest.maxCount;
        QuestCount = 0;

        rewardType = RewardManager.StringToRewardType(quest.rewardType);
        rewardImage.sprite = RewardManager.Instance.rewardImages[(int)rewardType].sprite;

        QuestRewardAmount = quest.amount;
    }


    /// <summary>
    /// ����Ʈ �Ϸ� ��ư Ŭ�� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void ClickSuccessButton()
    {
        if (isSuccess && !isGetReward)
        {
            rewardText.text = "�Ϸ�";
            getRewardButton.interactable = false;

            RewardManager.GetReward(rewardType, questRewardAmount);

            // ���� ������ �ִٴ� �� �˷��ִ� notification Image�� ����
            if (questType == EQuestType.achivement)
            {
                AchivementManager.Instance.notificationImage.SetActive(false);
            }
            else
            {
                DailyQuestManager.Instance.notificationImage.SetActive(false);
            }
        }
    }

    /// <summary>
    /// ���� ���� �Ǿ� �ʱ�ȭ�� ��
    /// </summary>
    public void NextDay()
    {
        isSuccess = false;
        isGetReward = false;

        getRewardButton.interactable = false;

        rewardText.text = questRewardAmount;

        QuestCount = 0;
    }
    #endregion
}
