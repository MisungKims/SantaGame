/**
 * @details csv를 파싱하여 퀘스트 목록을 가져옴
 * @author 김미성
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
    #region 변수
    [SerializeField]
    private GameObject questObj;            // UI에 배치될 오브젝트 (프리팹)
    [SerializeField]
    private GameObject parent;              // 오브젝트의 부모 (스크롤뷰의 Content)
    public GameObject notificationImage;   // 보상을 받을 퀘스트가 있음을 알리는 이미지
    
    // UI 배치에 필요한 변수
    protected Vector3 startPos = new Vector3(0, 102, 0);
    protected Vector2 startParentSize = new Vector2(0, 52);
    protected float nextYPos;
    protected float increaseParentYSize = 20;
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;

    // 퀘스트의 내용을 가진 리스트
    public List<Quest> questList = new List<Quest>();

    // 퀘스트의 UI를 가진 리스트
    protected List<QuestObject> questObjectList = new List<QuestObject>();

    // CSV 파일 이름
    protected string csvFileName;

    // 퀘스트의 종류
    protected EQuestType questType;

    #endregion

    #region 유니티 함수
    protected virtual void Awake()
    {
        SetTransform();

        ReadCSV();
    }
    #endregion

    #region 함수
    /// <summary>
    /// 복제될 오브젝트의 Rect Transform 초기 설정
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
            Quest dailyQuest = new Quest(
                i,
                data[i]["퀘스트 이름"].ToString(),
                0,
                (int)data[i]["최대 카운트"],
                 data[i]["보상 종류"].ToString(),
                 data[i]["수량"].ToString());

            questList.Add(dailyQuest);

            AchivementInstance(dailyQuest);     // UI에 퀘스트 목록 생성 후 배치
        }
    }

    /// <summary>
    /// 퀘스트 인스턴스를 생성하여 UI 배치
    /// </summary>
    void AchivementInstance(Quest newQuest)
    {
        QuestObject questObject = GameObject.Instantiate(questObj, parent.transform).GetComponent<QuestObject>();
        questObject.Init(newQuest);
        questObject.questType = questType;
        questObject.transform.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;
        questObjectList.Add(questObject);

        rectTransform.anchoredPosition += new Vector2(0, nextYPos);         // 다음 오브젝트를 스크롤뷰의 알맞은 위치에 넣기 위해 RectTransform을 조정

        parentRectTransform.sizeDelta += new Vector2(0, rectTransform.sizeDelta.y + increaseParentYSize);        // 스크롤뷰의 Content의 크기를 늘려줌
    }

    /// <summary>
    /// 퀘스트 성공
    /// </summary>
    /// <param name="id">퀘스트의 id</param>
    public virtual void Success(int id)
    {
        if (questList[id].count < questList[id].maxCount)
        {
            questList[id].count++;
            questObjectList[id].QuestCount++;

            if (questList[id].count == questList[id].maxCount)      // 퀘스트를 완료했을 때
            {
                notificationImage.SetActive(true);
            }

            /// TODO : 각 퀘스트의 ID를 정해서 넣기
        }
    }

    #endregion
}
