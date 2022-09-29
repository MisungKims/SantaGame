using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum EReward
{
    gold,
    dia,
    puzzle,
    clothesBox,
    carrot
}

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
    private RectTransform[] objRectTransforms;

    public List<AttendanceObject> rewardList = new List<AttendanceObject>();

    private EReward eReward;

    private void Awake()
    {
        instance = this;

        ReadCSV();
    }

    void ReadCSV()
    {
        // csv 리더를 통해 파일 가져오기
        List<Dictionary<string, object>> data = CSVReader.Read("AttendanceRewardData");

        for (int i = 0; i < data.Count; i++)
        {
            AttendanceInstant(
                i,
                data[i]["보상"].ToString(),
                  data[i]["양"].ToString()
                  );
        }
    }

    void AttendanceInstant(int i, string reward, string amount)
    {
        switch (reward)
        {
            case "골드":
                eReward = EReward.gold;
                break;
            case "보석":
                eReward = EReward.dia;
                break;
            case "퍼즐 조각":
                eReward = EReward.puzzle;
                break;
            case "랜덤 의상 상자":
                eReward = EReward.clothesBox;
                break;
            case "당근":
                eReward = EReward.carrot;
                break;
        }

        int index = (int)eReward;

        AttendanceObject instant = GameObject.Instantiate(rewardObjs[index], this.transform).GetComponent<AttendanceObject>();
        instant.name = (i + 1).ToString();
        instant.transform.GetComponent<RectTransform>().anchoredPosition = objRectTransforms[i].anchoredPosition;

        instant.eReward = eReward;
        instant.Day = (i + 1).ToString();
        instant.RewardAmount = amount;

        rewardList.Add(instant);
    }
}
