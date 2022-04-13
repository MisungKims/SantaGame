using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyQuestWindow : MonoBehaviour
{
    private static DailyQuestWindow instance;
    public static DailyQuestWindow Instance
    {
        get { return instance; }
    }

    [SerializeField]
    private GameObject questObj;
    [SerializeField]
    private GameObject questObjParent;
    [SerializeField]
    private Button allSuccessButton;

    private float margin = -50;

    private RectTransform rectTransform;
    private RectTransform parentRectTransform;

    private List<QuestObject> dailyQuestList = new List<QuestObject>();

    private bool isAllSuccess = true;

    // TODO : 다음 날 초기화

    private void Awake()
    {
        instance = this;

        allSuccessButton.interactable = false;

        rectTransform = questObj.transform.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector3(0, 102, 0);

        parentRectTransform = questObjParent.transform.GetComponent<RectTransform>();
        parentRectTransform.sizeDelta = new Vector2(0, 52);

        ReadCSV();
    }

    // csv 리더를 통해 파일 가져오기
    void ReadCSV()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("DailyQuestData");

        for (int i = 0; i < data.Count; i++)
        {
            AchivementInstant(
                data[i]["퀘스트 이름"].ToString(),
                 (int)data[i]["최대 카운트"],
                 data[i]["골드"].ToString()
                 );
        }
    }

    void AchivementInstant(string name, int count, string gold)
    {
        QuestObject instant = GameObject.Instantiate(questObj, questObjParent.transform).GetComponent<QuestObject>();
        instant.transform.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;

        instant.QuestName = name;
        instant.QuestMaxCount = count;
        instant.QuestGold = gold;
        instant.eQuestObj = EQuestObj.daily;

        rectTransform.anchoredPosition += new Vector2(0, margin);
        parentRectTransform.sizeDelta += new Vector2(0, rectTransform.sizeDelta.y + 20);

        dailyQuestList.Add(instant);
    }

    // 일일미션을 모두 완료했는지 체크
    public void CheckAllSuccess()
    {
        for (int i = 0; i < dailyQuestList.Count; i++)
        {
            if (!dailyQuestList[i].isSuccess)
            {
                isAllSuccess = false;
                break;
            }
            else isAllSuccess = true;
        }

        if (isAllSuccess)
        {
            allSuccessButton.interactable = true;
        }
        else
        {
            allSuccessButton.interactable = false;
        }
    }
    
}
