/**
 * @details ���Ϲ̼� ��� �Ϸ� ���� Ȯ���Ͽ� ���� ����
 * @author ��̼�
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
            allSuccessButton.interactable = value;      // ��� �Ϸ� ���ο� ���� ��ư�� interactable ����
        }
    }

    private List<QuestObject> dailyQuestList;


    /// TODO : ���� �� �ʱ�ȭ

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
    /// ���Ϲ̼��� ��� �Ϸ��ߴ��� üũ
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
    /// ���� ���� �Ǹ� �̼� �ʱ�ȭ
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
