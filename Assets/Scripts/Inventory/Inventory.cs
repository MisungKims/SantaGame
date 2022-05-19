/**
 * @brief 선물 인벤토리
 * @author 김미성
 * @date 22-05-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public int count = 0;   // 가지고 있는 선물의 총 갯수

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

    #region 함수
    /// <summary>
    /// 인벤토리에 아이템 추가
    /// </summary>
    /// <param name="gift">추가할 아이템</param>
    public void AddItem(Gift gift)
    {
        count++;

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
        count--;
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
                    for (int i = giftInvIndex; i < giftItems.Count; i++)
                    {
                        giftItems[i].gift.inventoryIndex -= 1;          // 제거한 슬롯의 뒤에 있는 슬롯을 앞으로 한 칸씩 당김
                        slots[giftItems[i].gift.inventoryIndex].SetSlot(giftItems[giftItems[i].gift.inventoryIndex]);
                    }
                    slots[giftItems.Count].SetEmpty();          // 한 칸씩 당기면 맨 뒤의 슬롯은 필요없으므로 비워둠
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

        if (giftInvIndex > -1)                          // 인벤토리에 아이템이 있을 때
        {
           // Debug.Log(giftItems[giftInvIndex].gift.giftName + " : " + giftInvIndex);

            //Debug.Log(giftItems[giftInvIndex].amount);

            giftItems[giftInvIndex].amount--;          // 수량을 줄임
            if (giftItems[giftInvIndex].amount <= 0)    // 수량이 0 이하이면 인벤토리에서 완전 제거
            {
               // Debug.Log("Remove  " + giftItems[giftInvIndex].gift.giftName + " : " + giftItems[giftInvIndex].amount);

                giftItems.RemoveAt(giftInvIndex);
                gift.inventoryIndex = -1;

                if (giftItems.Count > 0)    // 제거 후 인벤토리에 다른 아이템이 있으면 UI 재배치
                {
                    for (int i = giftInvIndex; i < giftItems.Count; i++)
                    {
                        giftItems[i].gift.inventoryIndex -= 1;          // 제거한 슬롯의 뒤에 있는 슬롯을 앞으로 한 칸씩 당김
                    }
                }
            }
        }
    }

    /// <summary>
    /// 인벤토리에서 랜덤으로 선물을 꺼내어 반환
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
    ///// 인벤토리에서 특정한 아이템을 제외하고 랜덤으로 선물을 꺼내어 반환
    ///// </summary>
    //public Gift RandomGet(int idx)
    //{
    //    if (giftItems.Count <= 0)
    //    {
    //        return null;
    //    }
    //    else
    //    {
    //        int[] arr = { };
    //        arr[0] = idx;
    //        int rand = GetRandomNumber(0, giftItems.Count, arr);
    //        return giftItems[rand].gift;
    //    }
    //}

    ///// <summary>
    ///// 특정 값을 제외한 랜덤한 값 구하기
    ///// </summary>
    ///// <param name="min"></param>
    ///// <param name="max"></param>
    ///// <param name="notContainVal"></param>
    ///// <returns></returns>
    //private int GetRandomNumber(int min, int max, int[] notContainVal)
    //{
    //    var exclude = new HashSet<int>();
    //    for (int i = 0; i < notContainVal.Length; i++)
    //    {
    //        exclude.Add(notContainVal[i]);
    //    }

    //    var range = Enumerable.Range(min, max).Where(i => !exclude.Contains(i));
    //    var rand = new System.Random();
    //    int index = rand.Next(min, max - exclude.Count);

    //    return range.ElementAt(index);
    //}

    ///// <summary>
    ///// 인벤토리 새로 고침
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
