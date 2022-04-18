/**
 * @details csv�� �Ľ��Ͽ� �⼮ ���� UI ����
 * @author ��̼�
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
    private RectTransform[] objRectTransforms;          // �� �ν��Ͻ��� ��ġ�� ���� �迭

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
    /// csv ������ ���� ���� ��������
    /// </summary>
    void ReadCSV()
    {
        
        List<Dictionary<string, object>> data = CSVReader.Read("AttendanceRewardData");

        for (int i = 0; i < data.Count; i++)
        {
            AttendanceInstance(
                i,
                data[i]["����"].ToString(),
                data[i]["��"].ToString()
                );
        }
    }

    /// <summary>
    /// �⼮ ���� �ν��Ͻ� ����
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
