/**
 * @brief 광고 보기 버튼
 * @author 김미성
 * @date 22-06-06
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using System.IO;

public class AdsButton : MonoBehaviour
{
    class SaveAdData
    {
        public string date;     // 광고를 본 날짜
        public int count;       // 광고를 본 횟수

        public SaveAdData(string date, int count)
        {
            this.date = date;
            this.count = count;
        }
    }

    #region 변수
    [SerializeField]
    private Button adsButton;       // 광고 보기 버튼
    [SerializeField]
    private Text adsCountText;       // 광고 카운트

    int adsCount;       // 하루동안 광고를 본 횟수

    //앱의 활성화 상태를 저장하는 변수
    bool isPaused = false;

    // 퀘스트 아이디
    int questId = 5;

    // 캐싱
    WaitForSeconds wait1f = new WaitForSeconds(1f);

    StringBuilder sb = new StringBuilder();

    QuestManager questManager;
    #endregion

    private void Awake()
    {
        questManager = QuestManager.Instance;

        if (!LoadData())
        {
            adsCount = 0;
        }
       
        SetText();
    }

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
            }
        }
    }

    void OnApplicationQuit()
    {
        SaveData();         // 앱 종료 시 데이터 저장
    }

    #region 함수
    /// <summary>
    /// 광고 버튼의 Interactable을 설정
    /// </summary>
    public void SetAdsButton()
    {
        QuestManagerInstance().Success(questId);        // 광고 보기 퀘스트 성공

        adsCount++;
        SetText();
    }

    /// <summary>
    /// 광고 카운트 텍스트 설정
    /// </summary>
    void SetText()
    {
        sb.Clear();
        sb.Append(adsCount);
        sb.Append(" / 10");
        adsCountText.text = sb.ToString();

        if (adsCount >= 10)
        {
            adsButton.interactable = false;
        }
    }

    /// <summary>
    /// 데이터 저장
    /// </summary>
    void SaveData()
    {
        SaveAdData saveAdData = new SaveAdData(DateTime.Now.ToString("yyyy.MM.dd"), adsCount);
        File.WriteAllText(Application.persistentDataPath + "/" + this.name + ".json", JsonUtility.ToJson(saveAdData));
    }

    /// <summary>
    /// 데이터 로드
    /// </summary>
    /// <returns></returns>
    public bool LoadData()
    {
        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/" + this.name + ".json");

        if (fileInfo.Exists)
        {
            string dataStr = File.ReadAllText(Application.persistentDataPath + "/" + this.name + ".json");

            SaveAdData data = JsonUtility.FromJson<SaveAdData>(dataStr);
            
            // 마지막으로 광고를 본 날짜와 현재 날짜가 다르면 카운트를 0으로 변경
            if (!data.date.Equals(DateTime.Now.ToString("yyyy.MM.dd")))
            {
                adsCount = 0;
            }
            else
            {
                adsCount = data.count;
            }

            return true;
        }

        return false;
    }

    QuestManager QuestManagerInstance()
    {
        if (!questManager)
        {
            questManager = QuestManager.Instance;
        }

        return questManager;
    }
    #endregion
}
