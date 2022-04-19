/**
 * @details 일일미션 모두 완료 여부 확인하여 보상 제공
 * @author 김미성
 * @date 22-04-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DailyQuestWindow : QuestWindow
{
    private static DailyQuestWindow instance;
    public static DailyQuestWindow Instance
    {
        get { return instance; }
    }

    [SerializeField]
    private Button allSuccessButton;

    private bool isAllSuccess = false;
    public bool IsAllSuccess
    {
        set
        {
            isAllSuccess = value;
            allSuccessButton.interactable = value;      // 모두 완료 여부에 따라 버튼의 interactable 설정
        }
    }

    private List<QuestObject> dailyQuestList;


    /// TODO : 다음 날 초기화

    protected override void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);

        IsAllSuccess = false;

        base.Awake();

        StartCoroutine(Init());
    }

    public void Start()
    {
        dailyQuestList = GameManager.Instance.dailyQuestList;
    }

    /// <summary>
    /// 일일미션을 모두 완료했는지 체크
    /// </summary>
    public void CheckAllSuccess()
    {
        for (int i = 0; i < dailyQuestList.Count; i++)
        {
            if (!dailyQuestList[i].isSuccess)
            {
                IsAllSuccess = false;
                return;
            }
        }

        IsAllSuccess = true;
    }

  
    /// <summary>
    /// 다음 날이 되면 미션 초기화
    /// </summary>
    IEnumerator Init()
    {
        while (true)
        {
            if (GameManager.Instance.getDailyQuestRewardDate != DateTime.Now.ToString("yyyy.MM.dd"))
            {
                for (int i = 0; i < dailyQuestList.Count; i++)
                {
                    if (dailyQuestList[i].isSuccess)
                    {
                        dailyQuestList[i].isSuccess = false;
                    }
                }
            }

            yield return null;
        }
    }
}
