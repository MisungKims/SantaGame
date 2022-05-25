/**
 * @brief 일일 퀘스트를 관리
 * @author 김미성
 * @date 22-04-19
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DailyQuestManager : QuestManager
{
    #region 변수
    //[SerializeField]
    //private Button allSuccessButton;        // 모두 완료 버튼

    public bool isAllSuccess;
    //public bool IsAllSuccess
    //{
    //    set
    //    {
    //        isAllSuccess = value;
    //        //allSuccessButton.interactable = value;      // 모두 완료 여부에 따라 버튼의 interactable 설정
    //    }
    //}

    private string initQuestDate;       // 퀘스트를 마지막으로 초기화한 날짜

    // 싱글톤
    private static DailyQuestManager instance;
    public static DailyQuestManager Instance
    {
        get { return instance; }
    }
    #endregion

    #region 유니티 함수
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

        // UI 배치 시 필요
        startPos = new Vector3(0, 190, 0);
        startParentSize = new Vector2(0, 480);
        nextYPos = -75;

        base.Awake();
    }

    void Start()
    {
        isAllSuccess = false;   // 추후 저장 필요

        StartCoroutine(Init());     // 익일 초기화
    }

    #endregion

    #region 함수
    public override void Success(int id)
    {
        if (!isAllSuccess)
        {
            base.Success(id);

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
            if (initQuestDate != DateTime.Now.ToString("yyyy.MM.dd"))
            {
                // 목록의 모든 퀘스트를 초기화
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
