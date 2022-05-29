/**
 * @brief 선물을 관리
 * @author 김미성
 * @date 22-04-24
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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

        if (!LoadData())
        {
            for (int i = 0; i < giftList.Count; i++)
            {
                giftList[i].inventoryIndex = -1;
            }
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

    ///// <summary>
    ///// 위시 리스트에 추가된 선물을 랜덤으로 반환
    ///// </summary>
    ///// <returns></returns>
    //public Gift RandomWishListGift()
    //{
    //    if (!IsHaveWishList())       // 위시리스트가 비어있을 때
    //    {
    //        return null;
    //    }

    //    Gift gift = RandomGift();
    //    if (gift.wishCount > 0)     // 랜덤으로 가져온 선물이 위시리스트에 있을 때 반환
    //    {
    //        return gift;
    //    }
    //    else
    //    {
    //        return RandomWishListGift();
    //    }
    //}

    ///// <summary>
    ///// 위시리스트에 모든 선물이 없으면 false 반환 
    ///// </summary>
    ///// <returns>위시리스트에 선물이 하나라도 있으면 true</returns>
    //public bool IsHaveWishList()
    //{
    //    bool returnVal = false;

    //    for (int i = 0; i < giftList.Count; i++)
    //    {
    //        if (giftList[i].wishCount > 0)
    //        {
    //            returnVal = true;
    //            break;
    //        }
    //    }

    //    return returnVal;
    //}

    /// <summary>
    /// 랜덤으로 선물을 받음
    /// </summary>
    public void ReceiveRandomGift()
    {
        ReceiveGift(RandomGift().giftType);
    }

    /// <summary>
    /// 받은 선물을 인벤토리에 저장
    /// </summary>
    public void ReceiveGift(EGiftType giftType)
    {
        Gift gift = giftList[(int)giftType];

        getRewardWindow.OpenWindow(gift);       // 보상 획득창 보여줌
        Inventory.Instance.AddItem(gift);       // 인벤토리에 저장
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
        string jdata = JsonUtility.ToJson(new Serialization<Gift>(giftList));
        File.WriteAllText(Application.dataPath + "/Resources/GiftData.json", jdata);
    }

    /// <summary>
    /// 데이터 로드
    /// </summary>
    /// <returns>불러오기 성공 여부</returns>
    public bool LoadData()
    {
        FileInfo fileInfo = new FileInfo(Application.dataPath + "/Resources/GiftData.json");
        if (fileInfo.Exists)
        {
            string jdata = File.ReadAllText(Application.dataPath + "/Resources/GiftData.json");

            giftList = JsonUtility.FromJson<Serialization<Gift>>(jdata).target;
            
            return true;
        }

        return false;
    }
}
