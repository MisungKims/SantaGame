/**
 * @brief 선물 인벤토리
 * @author 김미성
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
    #region 변수
    public List<GiftItem> giftItems = new List<GiftItem>();     // 선물 인벤토리 리스트


    public Slot[] slots;        // 인벤토리 UI 슬롯 프리팹

    public Sprite[] gradeSprite;    // 등급별 이미지

    public int count = 0;   // 가지고 있는 선물의 총 갯수

    // 싱글톤
    private static Inventory instance;
    public static Inventory Instance
    {
        get { return instance; }
    }

    // 캐싱
    UIManager uIManager;
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

        uIManager = UIManager.Instance;

        LoadData();
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
            //UIManager.Instance.slots[giftInvIndex].SetSlot(giftItems[giftInvIndex]);     // 슬롯에 저장
        }
        else
        {
            gift.inventoryIndex = giftItems.Count;
            giftItems.Add(new GiftItem(gift, 1));       // 인벤토리에 없다면 새로 생성
            //UIManager.Instance.slots[giftItems.Count - 1].SetSlot(giftItems[giftItems.Count - 1]);     // 슬롯에 저장
        }
    }

    /// <summary>
    /// 인벤토리에서 아이템 제거
    /// </summary>
    /// <param name="gift">제거할 아이템</param>
    /// <param name="isUseSlot">슬롯 UI를 조정할 것인지?</param>
    public void RemoveItem(Gift gift, bool isUseSlot)
    {
        count--;
        int giftInvIndex = gift.inventoryIndex;

        if (giftInvIndex > -1)                          // 인벤토리에 아이템이 있을 때
        {
            giftItems[giftInvIndex].amount--;          // 수량을 줄임
           // UIManager.Instance.slots[giftInvIndex].SetSlot(giftItems[giftInvIndex]);

            if (giftItems[giftInvIndex].amount <= 0)    // 수량이 0 이하이면 인벤토리에서 완전 제거
            {
                giftItems.RemoveAt(giftInvIndex);
                gift.inventoryIndex = -1;

                if (giftItems.Count > 0)    // 제거 후 인벤토리에 다른 아이템이 있으면 UI 재배치
                {
                    for (int i = giftInvIndex; i < giftItems.Count; i++)
                    {
                        giftItems[i].gift.inventoryIndex -= 1;          // 제거한 슬롯의 뒤에 있는 슬롯을 앞으로 한 칸씩 당김
                        //UIManager.Instance.slots[giftItems[i].gift.inventoryIndex].SetSlot(giftItems[giftItems[i].gift.inventoryIndex]);
                    }
                   if(isUseSlot) slots[giftItems.Count].SetEmpty();          // 한 칸씩 당기면 맨 뒤의 슬롯은 필요없으므로 비워둠
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

    //앱의 활성화 상태를 저장하는 변수
    bool isPaused = false;

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            isPaused = true;

            SaveData();         // 앱이 비활성화되었을 때 데이터 저장
        }

        else
        {
            if (isPaused)
            {
                isPaused = false;
                /* 앱이 활성화 되었을 때 처리 */
            }
        }
    }

    void OnApplicationQuit()
    {
        SaveData();         // 앱 종료 시 데이터 저장
    }

    /// <summary>
    /// 데이터 저장
    /// </summary>
    void SaveData()
    {
        string jdata = JsonUtility.ToJson(new Serialization<GiftItem>(giftItems));
        File.WriteAllText(Application.dataPath + "/Resources/InventoryData.json", jdata);
    }

    /// <summary>
    /// 데이터 로드
    /// </summary>
    /// <returns>불러오기 성공 여부</returns>
    public bool LoadData()
    {
        FileInfo fileInfo = new FileInfo(Application.dataPath + "/Resources/InventoryData.json");
        if (fileInfo.Exists)
        {
            string jdata = File.ReadAllText(Application.dataPath + "/Resources/InventoryData.json");

            giftItems = JsonUtility.FromJson<Serialization<GiftItem>>(jdata).target;
            
            return true;
        }

        return false;
    }
}
