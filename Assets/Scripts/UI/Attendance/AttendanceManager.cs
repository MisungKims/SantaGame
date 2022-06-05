/**
 * @brief �⼮ ������ ����
 * @author ��̼�
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

#region ����ü

[System.Serializable]
public class AttendanceStruct
{
    public string rewardType;
    public string amount;
    public bool isGet;    // �⼮ ������ �޾����� true

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
    #region ����

    // UI ����
    [SerializeField]
    private GameObject notificationImage;     // �⼮ ���� �˸� �̹���      

    public AttendanceObject[] attendanceObjects;       // UI ������Ʈ�� ���� �迭

    public List<AttendanceStruct> attendanceList = new List<AttendanceStruct>();   // CSV ���Ͽ��� ������ �⼮ ���� ����Ʈ

    // �̱���
    private static AttendanceManager instance;
    public static AttendanceManager Instance
    {
        get { return instance; }
    }

    // ĳ��
    private GameManager gameManager;

    //���� Ȱ��ȭ ���¸� �����ϴ� ����
    bool isPaused = false;

    #endregion

    #region ����Ƽ �Լ�
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
            // �ε� ���� �� CSV ������ �о��
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

            SaveData();         // ���� ��Ȱ��ȭ�Ǿ��� �� ������ ����
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
        SaveData();         // �� ���� �� ������ ����
    }
    #endregion

    #region �ڷ�ƾ
    /// <summary>
    /// ���� �⼮ ������ ���� �� �˷���
    /// </summary>
    IEnumerator SetNotificationImage()
    {
        while (true)
        {
            // ������ �⼮ ���� ���� ��¥�� ���� ��¥�� �ٸ��� ���� ���� �⼮ ������ �����Ƿ�,
            if (gameManager.attendanceDate != DateTime.Now.ToString("yyyy.MM.dd"))
            {
                Init();         // �⼮�� �� �޾Ҵٸ� �ʱ�ȭ
                notificationImage.SetActive(true);        // UI�� �˷���
            }
            yield return null;
        }
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// ������ ����
    /// </summary>
    void SaveData()
    {
        string jdata = JsonUtility.ToJson(new Serialization<AttendanceStruct>(attendanceList));
        File.WriteAllText(Application.persistentDataPath + "/AttendacnceData.json", jdata);
    }

    /// <summary>
    /// ������ �ε�
    /// </summary>
    /// <returns>�ҷ����� ���� ����</returns>
    public bool LoadData()
    {
        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/AttendacnceData.json");
        if (fileInfo.Exists)
        {
            string jdata = File.ReadAllText(Application.persistentDataPath + "/AttendacnceData.json");

            // ������ �����ͷ� UI ����
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
    /// csv ������ ���� ���� ��������
    /// </summary>
    void ReadCSV()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("AttendanceRewardData");

        for (int i = 0; i < data.Count; i++)
        {
            AttendanceStruct attendance = new AttendanceStruct(
                 data[i]["����"].ToString(),
                 data[i]["��"].ToString(),
                 false
                );

            attendanceList.Add(attendance);

            AttendanceInstance(i, attendance);
        }
    }

    /// <summary>
    /// �⼮ ���� UI�� ���� �־���
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
    /// �⼮ ���� ȹ��
    /// </summary>
    /// <param name="day">���� ��¥</param>
    public bool GetReward(string day)
    {
        if (CheckCanGetReward(day))
        {
            AttendanceStruct attendance = attendanceList[int.Parse(day) - 1];
            attendance.isGet = true;

            RewardManager.GetReward(RewardManager.StringToRewardType(attendance.rewardType), attendance.amount);

            gameManager.attendanceDate = DateTime.Now.ToString("yyyy.MM.dd");      // ������ �⼮ ���� ���ɳ�¥�� ���� ��¥�� ����
            notificationImage.SetActive(false);

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
        if (gameManager.attendanceDate == DateTime.Now.ToString("yyyy.MM.dd"))
            return false;

        return true;
    }

    /// <summary>
    /// �������� ���� �⼮ ������ �� �޾Ҵٸ� �ʱ�ȭ
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
