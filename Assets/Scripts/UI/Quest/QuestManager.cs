/**
 * @details csv를 파싱하여 퀘스트 목록을 가져옴
 * @author 김미성
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class QuestManager : MonoBehaviour
{
    #region 변수
    // 싱글톤
    private static QuestManager instance;
    public static QuestManager Instance
    {
        get { return instance; }
    }

    [SerializeField]
    private GameObject questObj;            // UI에 배치될 오브젝트 (프리팹)
    [SerializeField]
    private GameObject parent;              // 오브젝트의 부모 (스크롤뷰의 Content)
    public GameObject notificationImage;   // 보상을 받을 퀘스트가 있음을 알리는 이미지

    // UI 배치에 필요한 변수
    private Vector3 startPos = new Vector3(0, 190, 0);
    private Vector2 startParentSize = new Vector2(0, 480);
    private float nextYPos = -75;
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;

    // 퀘스트의 내용을 가진 리스트
    public List<Quest> questList = new List<Quest>();

    // 퀘스트의 UI를 가진 리스트
    private List<QuestObject> questObjectList = new List<QuestObject>();

    // CSV 파일 이름
    private string csvFileName = "DailyQuestData";

    public bool isAllSuccess;

    #endregion

    #region 유니티 함수
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
        isAllSuccess = false;   // 추후 저장 필요

        StartCoroutine(Init());     // 익일 초기화
    }

    //앱의 활성화 상태를 저장하는 변수
    bool isPaused = false;

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            isPaused = true;

            SaveData();         // 앱이 비활성화되었을 때 데이터 저장
        }

        else
        {
            if (isPaused)
            {
                isPaused = false;
                /* 앱이 활성화 되었을 때 처리 */
            }
        }
    }

    void OnApplicationQuit()
    {
        SaveData();         // 앱 종료 시 데이터 저장
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
                 data[i]["수량"].ToString(),
                 false,
                 false);

            questList.Add(dailyQuest);

            AchivementInstance(dailyQuest);     // UI에 퀘스트 목록 생성 후 배치
        }
    }

    /// <summary>
    /// 데이터 저장
    /// </summary>
    void SaveData()
    {
        string jdata = JsonUtility.ToJson(new Serialization<Quest>(questList));
        File.WriteAllText(Application.persistentDataPath + "/QuestData.json", jdata);
    }

    /// <summary>
    /// 데이터 로드
    /// </summary>
    /// <returns>불러오기 성공 여부</returns>
    public bool LoadData()
    {
        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/QuestData.json");
        if (fileInfo.Exists)
        {
            string jdata = File.ReadAllText(Application.persistentDataPath + "/QuestData.json");

            questList = JsonUtility.FromJson<Serialization<Quest>>(jdata).target;
            for (int i = 0; i < questList.Count; i++)
            {
                AchivementInstance(questList[i]);     // UI에 퀘스트 목록 생성 후 배치
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// 퀘스트 인스턴스를 생성하여 UI 배치
    /// </summary>
    void AchivementInstance(Quest newQuest)
    {
        QuestObject questObject = GameObject.Instantiate(questObj, parent.transform).GetComponent<QuestObject>();
        questObject.Init(newQuest);
        questObject.transform.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;
        questObjectList.Add(questObject);

        rectTransform.anchoredPosition += new Vector2(0, nextYPos);         // 다음 오브젝트를 스크롤뷰의 알맞은 위치에 넣기 위해 RectTransform을 조정

       // parentRectTransform.sizeDelta += new Vector2(0, rectTransform.sizeDelta.y);        // 스크롤뷰의 Content의 크기를 늘려줌
    }

    /// <summary>
    /// 퀘스트 성공
    /// </summary>
    /// <param name="id">퀘스트의 id</param>
    public virtual void Success(int id)
    {
        if (!isAllSuccess)
        {
            if (questList[id].count < questList[id].maxCount)
            {
                questList[id].count++;
                questObjectList[id].QuestCount++;

                if (questList[id].count == questList[id].maxCount)      // 퀘스트를 완료했을 때
                {
                    notificationImage.SetActive(true);
                }

            }

            // 일일 퀘스트를 모두 완료했는지 탐색
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
    /// 다음 날이 되면 퀘스트 초기화
    /// </summary>
    IEnumerator Init()
    {
        while (true)
        {
            if (GameManager.Instance.initQuestDate != DateTime.Now.ToString("yyyy.MM.dd"))
            {
                // 목록의 모든 퀘스트를 초기화
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
