/**
 * @details csv를 파싱하여 출석 보상 UI 생성
 * @author 김미성
 * @date 22-04-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Attendance : MonoBehaviour
{
    private static Attendance instance;
    public static Attendance Instance
    {
        get { return instance; }
    }

    [SerializeField]
    private GameObject[] rewardObjs;
    [SerializeField]
    private RectTransform[] objRectTransforms;          // 각 인스턴스의 위치를 담은 배열

    public List<AttendanceObject> rewardList = new List<AttendanceObject>();

    private ERewardType rewardType;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        
        ReadCSV();
    }

    /// <summary>
    /// csv 리더를 통해 파일 가져오기
    /// </summary>
    void ReadCSV()
    {
        
        List<Dictionary<string, object>> data = CSVReader.Read("AttendanceRewardData");

        for (int i = 0; i < data.Count; i++)
        {
            AttendanceInstance(
                i,
                data[i]["보상"].ToString(),
                data[i]["양"].ToString()
                );
        }
    }

    /// <summary>
    /// 출석 보상 인스턴스 생성
    /// </summary>
    void AttendanceInstance(int i, string reward, string amount)
    {
        rewardType = RewardManager.StringToRewardType(reward);

        int index = (int)rewardType;

        AttendanceObject instant = GameObject.Instantiate(rewardObjs[index], this.transform).GetComponent<AttendanceObject>();

        instant.name = (i + 1).ToString();
        instant.transform.GetComponent<RectTransform>().anchoredPosition = objRectTransforms[i].anchoredPosition;

        instant.rewardType = rewardType;
        instant.Day = (i + 1).ToString();
        instant.RewardAmount = amount;

        rewardList.Add(instant);
    }
}
