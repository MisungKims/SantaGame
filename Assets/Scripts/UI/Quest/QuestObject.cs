using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EQuestObj
{
    daily,
    achivement
}

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
    private Text goldText;

    public bool isSuccess = false;     // 퀘스트를 모두 완료했는지
    private bool isGetGold = false;     // 보상을 받았는지

    public EQuestObj eQuestObj;

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

    private string questGold;
    public string QuestGold
    {
        set
        {
            questGold = value;
            goldText.text = questGold;
        }
    }

    // 업적을 달성하고, 아직 골드를 획득하지 않았을 때 보상 획득
    public void ClickSuccessButton()
    {
        if (isSuccess && !isGetGold)
        {
            goldText.text = "완료";

            GameManager.Instance.MyGold += GoldManager.UnitToBigInteger(questGold); // 보상 획득

            // 일일미션이라면 모두 완료했는지 체크
            if (eQuestObj == EQuestObj.daily)
            {
                DailyQuestWindow.Instance.CheckAllSuccess();
            }
        }
    }
}
