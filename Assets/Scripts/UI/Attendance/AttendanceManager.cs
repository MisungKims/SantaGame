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
    private GameObject rewardObj;                    // ������ ������Ʈ(������)
    //[SerializeField]
    //private Image[] rewardImages;                   // ���� �̹���
    [SerializeField]
    private RectTransform[] objRectTransforms;          // �� �ν��Ͻ��� ��ġ�� ���� �迭
    [SerializeField]
    private GameObject parent;                          // ������Ʈ�� �θ�
    [SerializeField]
    private GameObject notificationImage;     // �⼮ ���� �˸� �̹���      

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
    /// �⼮ ���� �ν��Ͻ� ����
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
    /// ���� ȹ��
    /// </summary>
    /// <param name="day">���� ��¥</param>
    public void GetReward(string day)
    {
        if (CheckCanGetReward(day))
        {
            AttendanceStruct attendance = attendanceList[int.Parse(day) - 1];

            RewardManager.GetReward(RewardManager.StringToRewardType(attendance.rewardType), attendance.amount);

            getRewardDate = DateTime.Now.ToString("yyyy.MM.dd");      // ������ �⼮ ���� ���ɳ�¥�� ���� ��¥�� ����
            notificationImage.SetActive(false);

            AttendanceStruct newAttandance = attendance;
            newAttandance.isGet = true;

            attendance = newAttandance;

            /// TODO : ���� ȹ�� �Ϸ� ���� ����
        }
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
