/**
 * @brief 선물 인벤토리
 * @author 김미성
 * @date 22-05-01
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
    #region 변수
    public List<GiftItem> giftItems = new List<GiftItem>();     // 선물 인벤토리 리스트

    public Slot[] slots;        // 인벤토리 UI 슬롯

    public Sprite[] gradeSprite;    // 등급별 이미지

    // 싱글톤
    private static Inventory instance;
    public static Inventory Instance
    {
        get { return instance; }
    }
    #endregion

    #region 유니티 함수
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    private void OnEnable()
    {
        Debug.Log("REFRESH");
        RefreshInventory();
    }

    #endregion

    #region 함수
    /// <summary>
    /// 인벤토리에 아이템 추가
    /// </summary>
    /// <param name="gift">추가할 선물</param>
    public void AddItem(Gift gift)
    {
        int giftInvIndex = gift.inventoryIndex;

        if (giftInvIndex > -1)          // 이미 인벤토리에 있는 선물이라면
        {
            giftItems[giftInvIndex].amount++;              // 카운트를 늘림
            slots[giftInvIndex].SetSlot(giftItems[giftInvIndex]);     // 슬롯에 저장
        }
        else
        {
            gift.inventoryIndex = giftItems.Count;
            giftItems.Add(new GiftItem(gift, 1));       // 인벤토리에 없다면 새로 생성
            slots[giftItems.Count - 1].SetSlot(giftItems[giftItems.Count - 1]);     // 슬롯에 저장
        }
    }

    /// <summary>
    /// 인벤토리에서 아이템 제거
    /// </summary>
    /// <param name="gift">제거할 아이템</param>
    public void RemoveItem(Gift gift)
    {
        int giftInvIndex = gift.inventoryIndex;

        if (giftInvIndex > -1)                          // 인벤토리에 아이템이 있을 때
        {
            giftItems[giftInvIndex].amount--;          // 수량을 줄임
            slots[giftInvIndex].SetSlot(giftItems[giftInvIndex]);

            if (giftItems[giftInvIndex].amount <= 0)    // 수량이 0 이하이면 인벤토리에서 완전 제거
            {
                giftItems.RemoveAt(giftInvIndex);
                gift.inventoryIndex = -1;

                if (giftItems.Count > 0)    // 제거 후 인벤토리에 다른 아이템이 있으면 UI 재배치
                {
                   // Debug.Log(giftInvIndex);
                    for (int i = giftInvIndex; i < giftItems.Count + giftInvIndex; i++)
                    {
                       // Debug.Log(giftItems[i].gift.giftName + " " + giftItems[i].gift.inventoryIndex);
                       // slots[giftItems[i].gift.inventoryIndex].SetEmpty();
                        giftItems[i].gift.inventoryIndex -= 1;
                        

                    }
                }

               
            }
        }
    }

    /// <summary>
    /// 인벤토리 새로 고침
    /// </summary>
    public void RefreshInventory()
    {
        for (int i = 0; i < giftItems.Count; i++)
        {
            Debug.Log(giftItems[i].gift.giftName);
            slots[i].SetSlot(giftItems[i]);
        }
    }
    #endregion
}
