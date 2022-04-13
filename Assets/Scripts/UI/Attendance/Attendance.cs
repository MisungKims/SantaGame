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
        // csv ������ ���� ���� ��������
        List<Dictionary<string, object>> data = CSVReader.Read("AttendanceRewardData");

        for (int i = 0; i < data.Count; i++)
        {
            AttendanceInstant(
                i,
                data[i]["����"].ToString(),
                  data[i]["��"].ToString()
                  );
        }
    }

    void AttendanceInstant(int i, string reward, string amount)
    {
        switch (reward)
        {
            case "���":
                eReward = EReward.gold;
                break;
            case "����":
                eReward = EReward.dia;
                break;
            case "���� ����":
                eReward = EReward.puzzle;
                break;
            case "���� �ǻ� ����":
                eReward = EReward.clothesBox;
                break;
            case "���":
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
