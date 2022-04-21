/**
 * @brief 출석 보상을 관리
 * @author 김미성
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

#region 구조체
public struct AttendanceStruct
{
    public string rewardType;
    public string amount;
    public bool isGet;    // 출석 보상을 받았으면 true
}
#endregion

public class AttendanceManager : MonoBehaviour
{
    #region 변수

    // UI 변수
    [SerializeField]
    private GameObject rewardObj;                    // 생성될 오브젝트(프리팹)
    //[SerializeField]
    //private Image[] rewardImages;                   // 보상 이미지
    [SerializeField]
    private RectTransform[] objRectTransforms;          // 각 인스턴스의 위치를 담은 배열
    [SerializeField]
    private GameObject parent;                          // 오브젝트의 부모
    [SerializeField]
    private GameObject notificationImage;     // 출석 보상 알림 이미지      

    // 그 외 변수
    public string getRewardDate;              // 마지막으로 출석 보상을 받은 날짜

    public List<AttendanceStruct> attendanceList = new List<AttendanceStruct>();   // CSV 파일에서 가져올 출석 보상 리스트

    // 싱글톤
    private static AttendanceManager instance;
    public static AttendanceManager Instance
    {
        get { return instance; }
    }
    #endregion


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        ReadCSV();
    }

    private void Start()
    {
       StartCoroutine(SetNotificationImage());
    }

    /// <summary>
    /// csv 리더를 통해 파일 가져오기
    /// </summary>
    void ReadCSV()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("AttendanceRewardData");

        for (int i = 0; i < data.Count; i++)
        {
            AttendanceStruct attendance;
            attendance.rewardType = data[i]["보상"].ToString();
            attendance.amount = data[i]["양"].ToString();
            attendance.isGet = false;

            attendanceList.Add(attendance);

            AttendanceInstance(i, attendance);
        }
    }

    /// <summary>
    /// 출석 보상 인스턴스 생성
    /// </summary>
    void AttendanceInstance(int i, AttendanceStruct attendance)
    {
        ERewardType rewardType = RewardManager.StringToRewardType(attendance.rewardType);

        int index = (int)rewardType;

        AttendanceObject attendanceObject = GameObject.Instantiate(rewardObj, parent.transform).GetComponent<AttendanceObject>();

        attendanceObject.transform.GetComponent<RectTransform>().anchoredPosition = objRectTransforms[i].anchoredPosition;

        attendanceObject.rewardType = rewardType;
        attendanceObject.RewardAmount = attendance.amount;
        attendanceObject.Day = (i + 1).ToString();
        attendanceObject.RewardImage.sprite = RewardManager.Instance.rewardImages[index].sprite;
    }

    /// <summary>
    /// 보상 획득
    /// </summary>
    /// <param name="day">받을 날짜</param>
    public void GetReward(string day)
    {
        if (CheckCanGetReward(day))
        {
            AttendanceStruct attendance = attendanceList[int.Parse(day) - 1];

            RewardManager.GetReward(RewardManager.StringToRewardType(attendance.rewardType), attendance.amount);

            getRewardDate = DateTime.Now.ToString("yyyy.MM.dd");      // 마지막 출석 보상 수령날짜를 오늘 날짜로 변경
            notificationImage.SetActive(false);

            AttendanceStruct newAttandance = attendance;
            newAttandance.isGet = true;

            attendance = newAttandance;

            /// TODO : 보상 획득 완료 도장 생성
        }
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
        if (getRewardDate == DateTime.Now.ToString("yyyy.MM.dd"))
            return false;

        return true;
    }

    /// <summary>
    /// 받을 출석 보상이 있을 때 알려줌
    /// </summary>
    IEnumerator SetNotificationImage()
    {
        while (true)
        {
            // 마지막 출석 보상 수령 날짜가 오늘 날짜와 다르면 현재 받을 출석 보상이 있으므로,
            if (getRewardDate != DateTime.Now.ToString("yyyy.MM.dd"))
            {
                notificationImage.SetActive(true);        // UI로 알려줌
            }

            yield return null;
        }
    }
}
