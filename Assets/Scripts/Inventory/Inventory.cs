/**
 * @brief ���� �κ��丮
 * @author ��̼�
 * @date 22-05-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Slot[] slots;        // �κ��丮 UI ����

    public Sprite[] gradeSprite;    // ��޺� �̹���

    public int count = 0;   // ������ �ִ� ������ �� ����

    // �̱���
    private static Inventory instance;
    public static Inventory Instance
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
    }
    private void OnEnable()
    {
        RefreshInventory();
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

        int giftInvIndex = gift.inventoryIndex;

        if (giftInvIndex > -1)          // �̹� �κ��丮�� �ִ� �����̶��
        {
            giftItems[giftInvIndex].amount++;              // ī��Ʈ�� �ø�
            slots[giftInvIndex].SetSlot(giftItems[giftInvIndex]);     // ���Կ� ����
        }
        else
        {
            gift.inventoryIndex = giftItems.Count;
            giftItems.Add(new GiftItem(gift, 1));       // �κ��丮�� ���ٸ� ���� ����
            slots[giftItems.Count - 1].SetSlot(giftItems[giftItems.Count - 1]);     // ���Կ� ����
        }
    }

    /// <summary>
    /// �κ��丮���� ������ ����
    /// </summary>
    /// <param name="gift">������ ������</param>
    public void RemoveItem(Gift gift)
    {
        count--;
        int giftInvIndex = gift.inventoryIndex;

        if (giftInvIndex > -1)                          // �κ��丮�� �������� ���� ��
        {
            giftItems[giftInvIndex].amount--;          // ������ ����
            slots[giftInvIndex].SetSlot(giftItems[giftInvIndex]);

            if (giftItems[giftInvIndex].amount <= 0)    // ������ 0 �����̸� �κ��丮���� ���� ����
            {
                giftItems.RemoveAt(giftInvIndex);
                gift.inventoryIndex = -1;

                if (giftItems.Count > 0)    // ���� �� �κ��丮�� �ٸ� �������� ������ UI ���ġ
                {
                    for (int i = giftInvIndex; i < giftItems.Count; i++)
                    {
                        giftItems[i].gift.inventoryIndex -= 1;          // ������ ������ �ڿ� �ִ� ������ ������ �� ĭ�� ���
                        slots[giftItems[i].gift.inventoryIndex].SetSlot(giftItems[giftItems[i].gift.inventoryIndex]);
                    }
                    slots[giftItems.Count].SetEmpty();          // �� ĭ�� ���� �� ���� ������ �ʿ�����Ƿ� �����
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gift"></param>
    public void RemoveItem2(Gift gift)
    {
        count--;
        int giftInvIndex = gift.inventoryIndex;
        Debug.Log(gift.giftName +" : " + giftInvIndex);

        if (giftInvIndex > -1)                          // �κ��丮�� �������� ���� ��
        {
            //Debug.Log(giftItems[giftInvIndex].amount);
            giftItems[giftInvIndex].amount--;          // ������ ����
            
            if (giftItems[giftInvIndex].amount <= 0)    // ������ 0 �����̸� �κ��丮���� ���� ����
            {
                giftItems.RemoveAt(giftInvIndex);
                gift.inventoryIndex = -1;
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
    #endregion
}
