/**
 * @details csv�� �Ľ��Ͽ� ����Ʈ(���Ϲ̼�, ����) UI ����
 * @author ��̼�
 * @date 22-04-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����Ʈ�� ����
/// </summary>
public enum EQuestType
{
    daily,
    achivement
}
public class QuestWindow : MonoBehaviour
{
    [SerializeField]
    private string csvFileName;
    [SerializeField]
    EQuestType questType;
    [SerializeField]
    private GameObject questObj;
    [SerializeField]
    private GameObject parent;         // ������Ʈ�� �θ� (��ũ�Ѻ��� Content)
    [SerializeField]
    private GameObject notificationImage;         // ������ ���� ����Ʈ�� ������ �˸��� �̹���

    [Header("--------------Transform")]
    [SerializeField]
    private Vector3 startPos;
    [SerializeField]
    private Vector2 startParentSize;
    [SerializeField]
    private float nextYPos;
    [SerializeField]
    private float increaseParentYSize;
   

    private RectTransform rectTransform;
    private RectTransform parentRectTransform;

    

    protected virtual void Awake()
    {
        SetTransform();

        ReadCSV();
    }

    /// <summary>
    /// ������Ʈ�� Rect Transform �ʱ� ����
    /// </summary>
    void SetTransform()
    {
        rectTransform = questObj.transform.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = startPos;

        parentRectTransform = parent.transform.GetComponent<RectTransform>();
        parentRectTransform.sizeDelta = startParentSize;
    }

    /// <summary>
    /// csv ������ ���� ���� ��������
    /// </summary>
    void ReadCSV()
    {
        List<Dictionary<string, object>> data = CSVReader.Read(csvFileName);

        for (int i = 0; i < data.Count; i++)
        {
            AchivementInstance(
                data[i]["����Ʈ �̸�"].ToString(),
                 (int)data[i]["�ִ� ī��Ʈ"],
                 data[i]["���� ����"].ToString(),
                 data[i]["����"].ToString()
                 );
        }
    }

    /// <summary>
    /// ����Ʈ �ν��Ͻ� ����
    /// </summary>
    void AchivementInstance(string name, int count, string rewardType, string amount)
    {
        QuestObject newQuest = GameObject.Instantiate(questObj, parent.transform).GetComponent<QuestObject>();

        newQuest.name = name;
        newQuest.QuestName = name;
        newQuest.QuestMaxCount = count;
        newQuest.rewardType = RewardManager.StringToRewardType(rewardType);
        newQuest.QuestRewardAmount = amount;
        newQuest.questType = questType;

        newQuest.transform.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;

        if (questType == EQuestType.daily)
            GameManager.Instance.dailyQuestList.Add(newQuest);

        rectTransform.anchoredPosition += new Vector2(0, nextYPos);         // ���� ������Ʈ�� ��ũ�Ѻ��� �˸��� ��ġ�� �ֱ� ���� RectTransform�� ����

        parentRectTransform.sizeDelta += new Vector2(0, rectTransform.sizeDelta.y + increaseParentYSize);        // ��ũ�Ѻ��� Content�� ũ�⸦ �÷���
    }
}
