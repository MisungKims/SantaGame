/**
 * @brief ���� ���� ��ư
 * @author ��̼�
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
        public string date;     // ���� �� ��¥
        public int count;       // ���� �� Ƚ��

        public SaveAdData(string date, int count)
        {
            this.date = date;
            this.count = count;
        }
    }

    #region ����
    [SerializeField]
    private Button adsButton;       // ���� ���� ��ư
    [SerializeField]
    private Text adsCountText;       // ���� ī��Ʈ

    int adsCount;       // �Ϸ絿�� ���� �� Ƚ��

    //���� Ȱ��ȭ ���¸� �����ϴ� ����
    bool isPaused = false;

    // ����Ʈ ���̵�
    int questId = 5;

    // ĳ��
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

    #region �Լ�
    /// <summary>
    /// ���� ��ư�� Interactable�� ����
    /// </summary>
    public void SetAdsButton()
    {
        QuestManagerInstance().Success(questId);        // ���� ���� ����Ʈ ����

        adsCount++;
        SetText();
    }

    /// <summary>
    /// ���� ī��Ʈ �ؽ�Ʈ ����
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
    /// ������ ����
    /// </summary>
    void SaveData()
    {
        SaveAdData saveAdData = new SaveAdData(DateTime.Now.ToString("yyyy.MM.dd"), adsCount);
        File.WriteAllText(Application.persistentDataPath + "/" + this.name + ".json", JsonUtility.ToJson(saveAdData));
    }

    /// <summary>
    /// ������ �ε�
    /// </summary>
    /// <returns></returns>
    public bool LoadData()
    {
        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/" + this.name + ".json");

        if (fileInfo.Exists)
        {
            string dataStr = File.ReadAllText(Application.persistentDataPath + "/" + this.name + ".json");

            SaveAdData data = JsonUtility.FromJson<SaveAdData>(dataStr);
            
            // ���������� ���� �� ��¥�� ���� ��¥�� �ٸ��� ī��Ʈ�� 0���� ����
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
