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

    public bool isSuccess = false;     // ����Ʈ�� ��� �Ϸ��ߴ���
    private bool isGetGold = false;     // ������ �޾Ҵ���

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

    // ������ �޼��ϰ�, ���� ��带 ȹ������ �ʾ��� �� ���� ȹ��
    public void ClickSuccessButton()
    {
        if (isSuccess && !isGetGold)
        {
            goldText.text = "�Ϸ�";

            GameManager.Instance.MyGold += GoldManager.UnitToBigInteger(questGold); // ���� ȹ��

            // ���Ϲ̼��̶�� ��� �Ϸ��ߴ��� üũ
            if (eQuestObj == EQuestObj.daily)
            {
                DailyQuestWindow.Instance.CheckAllSuccess();
            }
        }
    }
}
