/**
 * @brief ���� �κ��丮
 * @author ��̼�
 * @date 22-05-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

[System.Serializable]
public class GiftItem
{
    public Gift gift;
    public int amount;

    public GiftItem(Gift gift, int amount)
    {
        this.gift = gift;
        this.amount = amount;
    }
}

public class Inventory : MonoBehaviour
{
    #region ����
    public List<GiftItem> giftItems = new List<GiftItem>();     // ���� �κ��丮 ����Ʈ

    public Slot[] slots;        // �κ��丮 UI ���� ������

    public Sprite[] gradeSprite;    // ��޺� �̹���

    public int count = 0;   // ������ �ִ� ������ �� ����

    // �̱���
    private static Inventory instance;
    public static Inventory Instance
    {
        get { return instance; }
    }

    
    bool isPaused = false;      //���� Ȱ��ȭ ����
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

        LoadData();
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
    /// �κ��丮�� ������ �߰�
    /// </summary>
    /// <param name="gift">�߰��� ������</param>
    public void AddItem(Gift gift)
    {
        count++;

        int giftInvIndex = gift.giftInfo.inventoryIndex;

        if (giftInvIndex > -1)          // �̹� �κ��丮�� �ִ� �����̶��
        {
            giftItems[giftInvIndex].amount++;              // ī��Ʈ�� �ø�
        }
        else
        {
            gift.giftInfo.inventoryIndex = giftItems.Count;
            giftItems.Add(new GiftItem(gift, 1));       // �κ��丮�� ���ٸ� ���� ����
        }
    }

    /// <summary>
    /// �κ��丮���� ������ ����
    /// </summary>
    /// <param name="gift">������ ������</param>
    /// <param name="isUseSlot">���� UI�� ������ ������?</param>
    public void RemoveItem(Gift gift, bool isUseSlot)
    {
        count--;
        int giftInvIndex = gift.giftInfo.inventoryIndex;

        if (giftInvIndex > -1)                          // �κ��丮�� �������� ���� ��
        {
            giftItems[giftInvIndex].amount--;          // ������ ����

            if (giftItems[giftInvIndex].amount <= 0)    // ������ 0 �����̸� �κ��丮���� ���� ����
            {
                giftItems.RemoveAt(giftInvIndex);
                gift.giftInfo.inventoryIndex = -1;

                // ���� �� �κ��丮�� �ٸ� �������� ������ UI ���ġ
                if (giftItems.Count > 0)   
                {
                    for (int i = giftInvIndex; i < giftItems.Count; i++)
                    {
                        giftItems[i].gift.giftInfo.inventoryIndex -= 1;          // ������ ������ �ڿ� �ִ� ������ ������ �� ĭ�� ���
                    }
                   if(isUseSlot) slots[giftItems.Count].SetEmpty();          // �� ĭ�� ���� �� ���� ������ �ʿ�����Ƿ� �����
                }
            }
        }
    }


    /// <summary>
    /// �κ��丮���� �������� ������ ������ ��ȯ
    /// </summary>
    public Gift RandomGet()
    {
        if (giftItems.Count <= 0)
        {
            return null;
        }
        else
        {
            int rand = Random.Range(0, giftItems.Count);
            return giftItems[rand].gift;
        }
    }

    ///// <summary>
    ///// �κ��丮 ���� ��ħ
    ///// </summary>
    public void RefreshInventory()
    {
        for (int i = 0; i < giftItems.Count; i++)
        {
            slots[i].SetSlot(giftItems[i]);
        }
    }

    /// <summary>
    /// ������ ����
    /// </summary>
    void SaveData()
    {
        string jdata = JsonUtility.ToJson(new Serialization<GiftItem>(giftItems));
        File.WriteAllText(Application.persistentDataPath + "/InventoryData.json", jdata);
    }

    /// <summary>
    /// ������ �ε�
    /// </summary>
    /// <returns>�ҷ����� ���� ����</returns>
    public bool LoadData()
    {
        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/InventoryData.json");
        if (fileInfo.Exists)
        {
            string jdata = File.ReadAllText(Application.persistentDataPath + "/InventoryData.json");

            giftItems = JsonUtility.FromJson<Serialization<GiftItem>>(jdata).target;
            for (int i = 0; i < giftItems.Count; i++)
            {
                giftItems[i].gift = GiftManager.Instance.giftList[(int)giftItems[i].gift.giftType];

                count += giftItems[i].amount;
            }

            return true;
        }

        return false;
    }
    #endregion
}
