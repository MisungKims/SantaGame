/**
 * @brief ���� ����Ʈ�� ����
 * @author ��̼�
 * @date 22-04-19
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DailyQuestManager : QuestManager
{
    #region ����
    [SerializeField]
    private Button allSuccessButton;        // ��� �Ϸ� ��ư

    private bool isAllSuccess;
    public bool IsAllSuccess
    {
        set
        {
            isAllSuccess = value;
            allSuccessButton.interactable = value;      // ��� �Ϸ� ���ο� ���� ��ư�� interactable ����
        }
    }

    private string initQuestDate;       // ����Ʈ�� ���������� �ʱ�ȭ�� ��¥

    // �̱���
    private static DailyQuestManager instance;
    public static DailyQuestManager Instance
    {
        get { return instance; }
    }
    #endregion

    #region ����Ƽ �Լ�
    protected override void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
        }
            
        csvFileName = "DailyQuestData";
        questType = EQuestType.daily;

        // UI ��ġ �� �ʿ�
        startPos = new Vector3(0, 102, 0);
        startParentSize = new Vector2(0, 52);
        nextYPos = -75;
        increaseParentYSize = 20;

        base.Awake();
    }

    void Start()
    {
        IsAllSuccess = false;   // ���� ���� �ʿ�

        StartCoroutine(Init());     // ���� �ʱ�ȭ
    }

    #endregion

    #region �Լ�
    public override void Success(int id)
    {
        base.Success(id);

        // ���� ����Ʈ�� ��� �Ϸ��ߴ��� Ž��
        for (int i = 0; i < questList.Count; i++)
        {
            if (questList[i].count == questList[i].maxCount)
            {
                IsAllSuccess = false;
                return;
            }
        }

        IsAllSuccess = true;
    }

    /// <summary>
    /// ���� ���� �Ǹ� ����Ʈ �ʱ�ȭ
    /// </summary>
    IEnumerator Init()
    {
        while (true)
        {
            if (initQuestDate != DateTime.Now.ToString("yyyy.MM.dd"))
            {
                // ����� ��� ����Ʈ�� �ʱ�ȭ
                for (int i = 0; i < questList.Count; i++)
                {
                    questList[i].count = 0;
                    questObjectList[i].NextDay();
                }

                initQuestDate = DateTime.Now.ToString("yyyy.MM.dd");
            }

            yield return null;
        }
    }
    #endregion
}
