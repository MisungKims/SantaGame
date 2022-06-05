/**
 * @brief ������ ����
 * @author ��̼�
 * @date 22-06-02
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GiftManager : MonoBehaviour
{
    #region ����
    public List<Gift> giftList = new List<Gift>();                  // ���� ����Ʈ

    [HideInInspector]
    public List<GiftInfo> giftInfoList = new List<GiftInfo>();      // ������ ������

    int totalWeight = 0;

    // ������ ����
    bool isPaused = false;      //���� Ȱ��ȭ ����

    // ĳ��
    private GetRewardWindow getRewardWindow;
    private Inventory inventory;

    // �̱���
    private static GiftManager instance;
    public static GiftManager Instance
    {
        get { return instance; }
    }
    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }

        getRewardWindow = UIManager.Instance.getRewardWindow;
        inventory = Inventory.Instance;

        if (!LoadData())
        {
            // ������ �ε忡 ���� �� ���ο� ������ ����
            NewData();
        }

        SetTotalWeight();
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

    #region �Լ�
    /// <summary>
    /// �� ����ġ ����
    /// </summary>
    public void SetTotalWeight()
    {
        for (int i = 0; i < giftList.Count; i++)
        {
            totalWeight += Gift.GetWeight(giftList[i].giftGrade);
        }
    }

    /// <summary>
    /// ���� ���� ��ȯ
    /// </summary>
    public Gift RandomGift()
    {
        // ����ġ ���� ���
        int weight = 0;
        int select = Mathf.RoundToInt(totalWeight * Random.Range(0.0f, 1.0f));

        for (int i = 0; i < giftList.Count; i++)
        {
            weight += Gift.GetWeight(giftList[i].giftGrade);
            if (select <= weight)
            {
                return new Gift(giftList[i]);
            }
        }

        return null;
    }

    /// <summary>
    /// �������� ������ ����
    /// </summary>
    public void ReceiveRandomGift()
    {
        ReceiveGift(RandomGift().giftType);
    }

    /// <summary>
    /// ���� ������ �κ��丮�� ����
    /// </summary>
    public void ReceiveGift(EGiftType giftType)
    {
        Gift gift = giftList[(int)giftType];

        getRewardWindow.OpenWindow(gift);       // ���� ȹ��â ������
        InventoryInstance().AddItem(gift);       // �κ��丮�� ����
    }

    /// <summary>
    /// �κ��丮 �̱��� ��ȯ
    /// </summary>
    /// <returns></returns>
    Inventory InventoryInstance()
    {
        inventory = Inventory.Instance;
        
        return inventory;
    }

    /// <summary>
    /// ������ ����
    /// </summary>
    void SaveData()
    {
        string jdata = JsonUtility.ToJson(new Serialization<GiftInfo>(giftInfoList));
        File.WriteAllText(Application.persistentDataPath + "/GiftData.json", jdata);
    }

    /// <summary>
    /// ������ �ε�
    /// </summary>
    /// <returns>�ҷ����� ���� ����</returns>
    public bool LoadData()
    {
        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/GiftData.json");
        if (fileInfo.Exists)
        {
            string jdata = File.ReadAllText(Application.persistentDataPath + "/GiftData.json");

            giftInfoList = JsonUtility.FromJson<Serialization<GiftInfo>>(jdata).target;
            for (int i = 0; i < giftList.Count; i++)
            {
                giftList[i].giftInfo = giftInfoList[i];
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// ������ ����
    /// </summary>
    public void NewData()
    {
        for (int i = 0; i < giftList.Count; i++)
        {
            GiftInfo giftInfo = new GiftInfo(0, -1);
            giftInfoList.Add(giftInfo);
            giftList[i].giftInfo = giftInfo;
        }
    }
    #endregion
}
