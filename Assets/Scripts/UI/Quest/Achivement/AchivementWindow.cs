using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchivementWindow : MonoBehaviour
{
    [SerializeField]
    private GameObject achivementObj;
    [SerializeField]
    private GameObject achivementObjParent;

    private float margin = -60;

    private RectTransform rectTransform;
    private RectTransform parentRectTransform;

    private void Awake()
    {
        rectTransform = achivementObj.transform.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector3(0, 120, 0);

        parentRectTransform = achivementObjParent.transform.GetComponent<RectTransform>();
        parentRectTransform.sizeDelta = new Vector2(0, 52);

        ReadCSV();
    }

    void ReadCSV()
    { 
        // csv 리더를 통해 파일 가져오기
        List<Dictionary<string, object>> data = CSVReader.Read("AchivementData");

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
        QuestObject instant = GameObject.Instantiate(achivementObj, achivementObjParent.transform).GetComponent<QuestObject>();
        instant.transform.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;

        instant.QuestName = name;
        instant.QuestMaxCount = count;
        instant.QuestGold = gold;
        instant.eQuestObj = EQuestObj.achivement;

        rectTransform.anchoredPosition += new Vector2(0, margin);

        parentRectTransform.sizeDelta += new Vector2(0, rectTransform.sizeDelta.y + 25);
    }
}
