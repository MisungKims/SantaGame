/**
 * @details csv를 파싱하여 퀘스트(일일미션, 업적) UI 생성
 * @author 김미성
 * @date 22-04-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 퀘스트의 종류
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
    private GameObject parent;         // 오브젝트의 부모 (스크롤뷰의 Content)
    [SerializeField]
    private GameObject notificationImage;         // 보상을 받을 퀘스트가 있음을 알리는 이미지

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
    /// 오브젝트의 Rect Transform 초기 설정
    /// </summary>
    void SetTransform()
    {
        rectTransform = questObj.transform.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = startPos;

        parentRectTransform = parent.transform.GetComponent<RectTransform>();
        parentRectTransform.sizeDelta = startParentSize;
    }

    /// <summary>
    /// csv 리더를 통해 파일 가져오기
    /// </summary>
    void ReadCSV()
    {
        List<Dictionary<string, object>> data = CSVReader.Read(csvFileName);

        for (int i = 0; i < data.Count; i++)
        {
            AchivementInstance(
                data[i]["퀘스트 이름"].ToString(),
                 (int)data[i]["최대 카운트"],
                 data[i]["보상 종류"].ToString(),
                 data[i]["수량"].ToString()
                 );
        }
    }

    /// <summary>
    /// 퀘스트 인스턴스 생성
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

        rectTransform.anchoredPosition += new Vector2(0, nextYPos);         // 다음 오브젝트를 스크롤뷰의 알맞은 위치에 넣기 위해 RectTransform을 조정

        parentRectTransform.sizeDelta += new Vector2(0, rectTransform.sizeDelta.y + increaseParentYSize);        // 스크롤뷰의 Content의 크기를 늘려줌
    }
}
