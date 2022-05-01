/**
 * @brief 선물을 관리
 * @author 김미성
 * @date 22-04-24
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftManager : MonoBehaviour
{
    #region 변수
    public List<Gift> giftList = new List<Gift>();

    int totalWeight = 0;

    // 캐싱
    private GetRewardWindow getRewardWindow;

    // 싱글톤
    private static GiftManager instance;
    public static GiftManager Instance
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

        getRewardWindow = UIManager.Instance.getRewardWindow;

        for (int i = 0; i < giftList.Count; i++)
        {
            giftList[i].inventoryIndex = -1;
        }
    }

    private void Start()
    {
        for (int i = 0; i < giftList.Count; i++)
        {
            totalWeight += Gift.GetWeight(giftList[i].giftGrade);
        }
    }
    #endregion

    #region 함수
    /// <summary>
    /// 랜덤 선물 반환
    /// </summary>
    public Gift RandomGift()
    {
        // 가중치 랜덤 사용
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
    /// 받은 선물을 인벤토리에 저장
    /// </summary>
    public void ReceiveGift(Gift gift)
    {
        getRewardWindow.OpenWindow(gift);     // 보상 획득창 보여줌

        Inventory.Instance.AddItem(gift);   // 인벤토리에 저장
    }
    #endregion

}
