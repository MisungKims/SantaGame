/**
 * @brief 출석 보상을 관리
 * @author 김미성
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

#region 구조체

[System.Serializable]
public class AttendanceStruct
{
    public string rewardType;
    public string amount;
    public bool isGet;    // 출석 보상을 받았으면 true

    public AttendanceStruct(string rewardType, string amount, bool isGet)
    {
        this.rewardType = rewardType;
        this.amount = amount;
        this.isGet = isGet;
    }
}
#endregion

public class AttendanceManager : MonoBehaviour
{
    #region 변수

    // UI 변수
    [SerializeField]
    private GameObject notificationImage;     // 출석 보상 알림 이미지      

    public AttendanceObject[] attendanceObjects;       // UI 오브젝트를 담은 배열

    public List<AttendanceStruct> attendanceList = new List<AttendanceStruct>();   // CSV 파일에서 가져올 출석 보상 리스트

    // 싱글톤
    private static AttendanceManager instance;
    public static AttendanceManager Instance
    {
        get { return instance; }
    }

    // 캐싱
    private GameManager gameManager;

    //앱의 활성화 상태를 저장하는 변수
    bool isPaused = false;

    #endregion

    #region 유니티 함수
    private void Awake()
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

        gameManager = GameManager.Instance;

        if (!LoadData())
        {
            // 로드 실패 시 CSV 파일을 읽어옴
            ReadCSV();
        }
    }

    private void Start()
    {
        StartCoroutine(SetNotificationImage());
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
    #endregion

    #region 코루틴
    /// <summary>
    /// 받을 출석 보상이 있을 때 알려줌
    /// </summary>
    IEnumerator SetNotificationImage()
    {
        while (true)
        {
            // 마지막 출석 보상 수령 날짜가 오늘 날짜와 다르면 현재 받을 출석 보상이 있으므로,
            if (gameManager.attendanceDate != DateTime.Now.ToString("yyyy.MM.dd"))
            {
                Init();         // 출석을 다 받았다면 초기화
                notificationImage.SetActive(true);        // UI로 알려줌
            }
            yield return null;
        }
    }
    #endregion

    #region 함수
    /// <summary>
    /// 데이터 저장
    /// </summary>
    void SaveData()
    {
        string jdata = JsonUtility.ToJson(new Serialization<AttendanceStruct>(attendanceList));
        File.WriteAllText(Application.persistentDataPath + "/AttendacnceData.json", jdata);
    }

    /// <summary>
    /// 데이터 로드
    /// </summary>
    /// <returns>불러오기 성공 여부</returns>
    public bool LoadData()
    {
        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/AttendacnceData.json");
        if (fileInfo.Exists)
        {
            string jdata = File.ReadAllText(Application.persistentDataPath + "/AttendacnceData.json");

            // 가져온 데이터로 UI 생성
            attendanceList = JsonUtility.FromJson<Serialization<AttendanceStruct>>(jdata).target;
            for (int i = 0; i < attendanceList.Count; i++)
            {
                AttendanceInstance(i, attendanceList[i]);
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// csv 리더를 통해 파일 가져오기
    /// </summary>
    void ReadCSV()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("AttendanceRewardData");

        for (int i = 0; i < data.Count; i++)
        {
            AttendanceStruct attendance = new AttendanceStruct(
                 data[i]["보상"].ToString(),
                 data[i]["양"].ToString(),
                 false
                );

            attendanceList.Add(attendance);

            AttendanceInstance(i, attendance);
        }
    }

    /// <summary>
    /// 출석 보상 UI에 값을 넣어줌
    /// </summary>
    void AttendanceInstance(int i, AttendanceStruct attendance)
    {
        ERewardType rewardType = RewardManager.StringToRewardType(attendance.rewardType);

        attendanceObjects[i].rewardType = rewardType;
        attendanceObjects[i].RewardAmount = attendance.amount;
        attendanceObjects[i].Day = (i + 1).ToString();
        attendanceObjects[i].RewardImage.sprite = RewardManager.Instance.rewardImages[(int)rewardType];
    }

    /// <summary>
    /// 출석 보상 획득
    /// </summary>
    /// <param name="day">받을 날짜</param>
    public bool GetReward(string day)
    {
        if (CheckCanGetReward(day))
        {
            AttendanceStruct attendance = attendanceList[int.Parse(day) - 1];
            attendance.isGet = true;

            RewardManager.GetReward(RewardManager.StringToRewardType(attendance.rewardType), attendance.amount);

            gameManager.attendanceDate = DateTime.Now.ToString("yyyy.MM.dd");      // 마지막 출석 보상 수령날짜를 오늘 날짜로 변경
            notificationImage.SetActive(false);

            return true;
        }
        else return false;
    }

    /// <summary>
    /// 보상을 받을 수 있는지 체크
    /// </summary>
    /// <returns>보상을 받을 수 있으면 true</returns>
    public bool CheckCanGetReward(string day)
    {
        // 먼저 받아야하는 보상을 받지 않았을 때 false 리턴
        if (day != "1")
        {
            if (!attendanceList[int.Parse(day) - 2].isGet)
                return false;
        }

        // 해당 보상을 이미 받았다면 false 리턴
        if (attendanceList[int.Parse(day) - 1].isGet)
            return false;

        // 오늘 보상을 받았을 때 false 리턴
        if (gameManager.attendanceDate == DateTime.Now.ToString("yyyy.MM.dd"))
            return false;

        return true;
    }

    /// <summary>
    /// 마지막날 까지 출석 보상을 다 받았다면 초기화
    /// </summary>
    private void Init()
    {
        if (attendanceList[6].isGet)
        {
            for (int i = 0; i < attendanceList.Count; i++)
            {
                attendanceList[i].isGet = false;
            }
        }
    }
    #endregion
}
