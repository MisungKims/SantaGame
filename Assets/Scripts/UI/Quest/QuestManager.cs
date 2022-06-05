/**
 * @details csv�� �Ľ��Ͽ� ����Ʈ ����� ������
 * @author ��̼�
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EQuestType
{
    daily,
    achivement
}

public class QuestManager : MonoBehaviour
{
    #region ����
    [SerializeField]
    private GameObject questObj;            // UI�� ��ġ�� ������Ʈ (������)
    [SerializeField]
    private GameObject parent;              // ������Ʈ�� �θ� (��ũ�Ѻ��� Content)
    public GameObject notificationImage;   // ������ ���� ����Ʈ�� ������ �˸��� �̹���
    
    // UI ��ġ�� �ʿ��� ����
    protected Vector3 startPos = new Vector3(0, 102, 0);
    protected Vector2 startParentSize = new Vector2(0, 52);
    protected float nextYPos;
    protected float increaseParentYSize = 20;
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;

    // ����Ʈ�� ������ ���� ����Ʈ
    public List<Quest> questList = new List<Quest>();

    // ����Ʈ�� UI�� ���� ����Ʈ
    protected List<QuestObject> questObjectList = new List<QuestObject>();

    // CSV ���� �̸�
    protected string csvFileName;

    // ����Ʈ�� ����
    protected EQuestType questType;

    #endregion

    #region ����Ƽ �Լ�
    protected virtual void Awake()
    {
        SetTransform();

        ReadCSV();
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// ������ ������Ʈ�� Rect Transform �ʱ� ����
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
            Quest dailyQuest = new Quest(
                i,
                data[i]["����Ʈ �̸�"].ToString(),
                0,
                (int)data[i]["�ִ� ī��Ʈ"],
                 data[i]["���� ����"].ToString(),
                 data[i]["����"].ToString());

            questList.Add(dailyQuest);

            AchivementInstance(dailyQuest);     // UI�� ����Ʈ ��� ���� �� ��ġ
        }
    }

    /// <summary>
    /// ����Ʈ �ν��Ͻ��� �����Ͽ� UI ��ġ
    /// </summary>
    void AchivementInstance(Quest newQuest)
    {
        QuestObject questObject = GameObject.Instantiate(questObj, parent.transform).GetComponent<QuestObject>();
        questObject.Init(newQuest);
        questObject.questType = questType;
        questObject.transform.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;
        questObjectList.Add(questObject);

        rectTransform.anchoredPosition += new Vector2(0, nextYPos);         // ���� ������Ʈ�� ��ũ�Ѻ��� �˸��� ��ġ�� �ֱ� ���� RectTransform�� ����

        parentRectTransform.sizeDelta += new Vector2(0, rectTransform.sizeDelta.y + increaseParentYSize);        // ��ũ�Ѻ��� Content�� ũ�⸦ �÷���
    }

    /// <summary>
    /// ����Ʈ ����
    /// </summary>
    /// <param name="id">����Ʈ�� id</param>
    public virtual void Success(int id)
    {
        if (questList[id].count < questList[id].maxCount)
        {
            questList[id].count++;
            questObjectList[id].QuestCount++;

            if (questList[id].count == questList[id].maxCount)      // ����Ʈ�� �Ϸ����� ��
            {
                notificationImage.SetActive(true);
            }

            /// TODO : �� ����Ʈ�� ID�� ���ؼ� �ֱ�
        }
    }

    #endregion
}
