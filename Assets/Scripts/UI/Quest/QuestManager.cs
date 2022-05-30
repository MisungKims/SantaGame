/**
 * @details csv�� �Ľ��Ͽ� ����Ʈ ����� ������
 * @author ��̼�
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class QuestManager : MonoBehaviour
{
    #region ����
    // �̱���
    private static QuestManager instance;
    public static QuestManager Instance
    {
        get { return instance; }
    }

    [SerializeField]
    private GameObject questObj;            // UI�� ��ġ�� ������Ʈ (������)
    [SerializeField]
    private GameObject parent;              // ������Ʈ�� �θ� (��ũ�Ѻ��� Content)
    public GameObject notificationImage;   // ������ ���� ����Ʈ�� ������ �˸��� �̹���

    // UI ��ġ�� �ʿ��� ����
    private Vector3 startPos = new Vector3(0, 190, 0);
    private Vector2 startParentSize = new Vector2(0, 480);
    private float nextYPos = -75;
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;

    // ����Ʈ�� ������ ���� ����Ʈ
    public List<Quest> questList = new List<Quest>();

    // ����Ʈ�� UI�� ���� ����Ʈ
    private List<QuestObject> questObjectList = new List<QuestObject>();

    // CSV ���� �̸�
    private string csvFileName = "DailyQuestData";

    public bool isAllSuccess;

    #endregion

    #region ����Ƽ �Լ�
    protected virtual void Awake()
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

        //csvFileName = "DailyQuestData";
        //startPos = new Vector3(0, 190, 0);
        //startParentSize = new Vector2(0, 480);
        //nextYPos = -75;

        SetTransform();

        if (!LoadData())
        {
            ReadCSV();
        }
    }

    void Start()
    {
        isAllSuccess = false;   // ���� ���� �ʿ�

        StartCoroutine(Init());     // ���� �ʱ�ȭ
    }

    //���� Ȱ��ȭ ���¸� �����ϴ� ����
    bool isPaused = false;

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            isPaused = true;

            SaveData();         // ���� ��Ȱ��ȭ�Ǿ��� �� ������ ����
        }

        else
        {
            if (isPaused)
            {
                isPaused = false;
                /* ���� Ȱ��ȭ �Ǿ��� �� ó�� */
            }
        }
    }

    void OnApplicationQuit()
    {
        SaveData();         // �� ���� �� ������ ����
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
                 data[i]["����"].ToString(),
                 false,
                 false);

            questList.Add(dailyQuest);

            AchivementInstance(dailyQuest);     // UI�� ����Ʈ ��� ���� �� ��ġ
        }
    }

    /// <summary>
    /// ������ ����
    /// </summary>
    void SaveData()
    {
        string jdata = JsonUtility.ToJson(new Serialization<Quest>(questList));
        File.WriteAllText(Application.persistentDataPath + "/QuestData.json", jdata);
    }

    /// <summary>
    /// ������ �ε�
    /// </summary>
    /// <returns>�ҷ����� ���� ����</returns>
    public bool LoadData()
    {
        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/QuestData.json");
        if (fileInfo.Exists)
        {
            string jdata = File.ReadAllText(Application.persistentDataPath + "/QuestData.json");

            questList = JsonUtility.FromJson<Serialization<Quest>>(jdata).target;
            for (int i = 0; i < questList.Count; i++)
            {
                AchivementInstance(questList[i]);     // UI�� ����Ʈ ��� ���� �� ��ġ
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// ����Ʈ �ν��Ͻ��� �����Ͽ� UI ��ġ
    /// </summary>
    void AchivementInstance(Quest newQuest)
    {
        QuestObject questObject = GameObject.Instantiate(questObj, parent.transform).GetComponent<QuestObject>();
        questObject.Init(newQuest);
        questObject.transform.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;
        questObjectList.Add(questObject);

        rectTransform.anchoredPosition += new Vector2(0, nextYPos);         // ���� ������Ʈ�� ��ũ�Ѻ��� �˸��� ��ġ�� �ֱ� ���� RectTransform�� ����

       // parentRectTransform.sizeDelta += new Vector2(0, rectTransform.sizeDelta.y);        // ��ũ�Ѻ��� Content�� ũ�⸦ �÷���
    }

    /// <summary>
    /// ����Ʈ ����
    /// </summary>
    /// <param name="id">����Ʈ�� id</param>
    public virtual void Success(int id)
    {
        if (!isAllSuccess)
        {
            if (questList[id].count < questList[id].maxCount)
            {
                questList[id].count++;
                questObjectList[id].QuestCount++;

                if (questList[id].count == questList[id].maxCount)      // ����Ʈ�� �Ϸ����� ��
                {
                    notificationImage.SetActive(true);
                }

            }

            // ���� ����Ʈ�� ��� �Ϸ��ߴ��� Ž��
            for (int i = 0; i < questList.Count; i++)
            {
                if (questList[i].count == questList[i].maxCount)
                {
                    isAllSuccess = false;
                    return;
                }
            }

            isAllSuccess = true;
        }
        
    }
    /// <summary>
    /// ���� ���� �Ǹ� ����Ʈ �ʱ�ȭ
    /// </summary>
    IEnumerator Init()
    {
        while (true)
        {
            if (GameManager.Instance.initQuestDate != DateTime.Now.ToString("yyyy.MM.dd"))
            {
                // ����� ��� ����Ʈ�� �ʱ�ȭ
                for (int i = 0; i < questList.Count; i++)
                {
                    questList[i].count = 0;
                    questObjectList[i].NextDay();
                }

                GameManager.Instance.initQuestDate = DateTime.Now.ToString("yyyy.MM.dd");
            }

            yield return null;
        }
    }
    #endregion
}
