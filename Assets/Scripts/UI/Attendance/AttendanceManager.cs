/**
 * @brief �⼮ ������ ����
 * @author ��̼�
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

#region ����ü
public struct AttendanceStruct
{
    public string rewardType;
    public string amount;
    public bool isGet;    // �⼮ ������ �޾����� true
}
#endregion

public class AttendanceManager : MonoBehaviour
{
    #region ����

    // UI ����
    [SerializeField]
    private GameObject notificationImage;     // �⼮ ���� �˸� �̹���      

    [SerializeField]
    private AttendanceObject[] attendanceObjects;       // UI ������Ʈ�� ���� �迭

    // �� �� ����
    public string getRewardDate;              // ���������� �⼮ ������ ���� ��¥

    public List<AttendanceStruct> attendanceList = new List<AttendanceStruct>();   // CSV ���Ͽ��� ������ �⼮ ���� ����Ʈ

    // �̱���
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
    /// csv ������ ���� ���� ��������
    /// </summary>
    void ReadCSV()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("AttendanceRewardData");

        for (int i = 0; i < data.Count; i++)
        {
            AttendanceStruct attendance;
            attendance.rewardType = data[i]["����"].ToString();
            attendance.amount = data[i]["��"].ToString();
            attendance.isGet = false;

            attendanceList.Add(attendance);

            AttendanceInstance(i, attendance);
        }
    }

    /// <summary>
    /// �⼮ ���� UI�� csv ������ ���� �־���
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
    /// ���� ȹ��
    /// </summary>
    /// <param name="day">���� ��¥</param>
    public bool GetReward(string day)
    {
        if (CheckCanGetReward(day))
        {
            AttendanceStruct attendance = attendanceList[int.Parse(day) - 1];

            RewardManager.GetReward(RewardManager.StringToRewardType(attendance.rewardType), attendance.amount);

            getRewardDate = DateTime.Now.ToString("yyyy.MM.dd");      // ������ �⼮ ���� ���ɳ�¥�� ���� ��¥�� ����
            notificationImage.SetActive(false);

            AttendanceStruct newAttandance = attendance;
            newAttandance.isGet = true;

            attendanceList[int.Parse(day) - 1] = newAttandance;

            return true;
        }
        else return false;
    }

    /// <summary>
    /// ������ ���� �� �ִ��� üũ
    /// </summary>
    /// <returns>������ ���� �� ������ true</returns>
    public bool CheckCanGetReward(string day)
    {
        // ���� �޾ƾ��ϴ� ������ ���� �ʾ��� �� false ����
        if (day != "1")
        {
            if (!attendanceList[int.Parse(day) - 2].isGet)
                return false;
        }

        // �ش� ������ �̹� �޾Ҵٸ� false ����
        if (attendanceList[int.Parse(day) - 1].isGet)
            return false;

        // ���� ������ �޾��� �� false ����
        if (getRewardDate == DateTime.Now.ToString("yyyy.MM.dd"))
            return false;

        return true;
    }

    /// <summary>
    /// ���� �⼮ ������ ���� �� �˷���
    /// </summary>
    IEnumerator SetNotificationImage()
    {
        while (true)
        {
            // ������ �⼮ ���� ���� ��¥�� ���� ��¥�� �ٸ��� ���� ���� �⼮ ������ �����Ƿ�,
            if (getRewardDate != DateTime.Now.ToString("yyyy.MM.dd"))
            {
                notificationImage.SetActive(true);        // UI�� �˷���
            }

            yield return null;
        }
    }
}
