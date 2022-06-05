/**
 * @brief 선물을 관리
 * @author 김미성
 * @date 22-06-02
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GiftManager : MonoBehaviour
{
    #region 변수
    public List<Gift> giftList = new List<Gift>();                  // 선물 리스트

    [HideInInspector]
    public List<GiftInfo> giftInfoList = new List<GiftInfo>();      // 저장할 데이터

    int totalWeight = 0;

    // 데이터 저장
    bool isPaused = false;      //앱의 활성화 상태

    // 캐싱
    private GetRewardWindow getRewardWindow;
    private Inventory inventory;

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
        inventory = Inventory.Instance;

        if (!LoadData())
        {
            // 데이터 로드에 실패 시 새로운 데이터 생성
            NewData();
        }

        SetTotalWeight();
    }

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
            }
        }
    }

    void OnApplicationQuit()
    {
        SaveData();         // 앱 종료 시 데이터 저장
    }
    #endregion

    #region 함수
    /// <summary>
    /// 총 가중치 설정
    /// </summary>
    public void SetTotalWeight()
    {
        for (int i = 0; i < giftList.Count; i++)
        {
            totalWeight += Gift.GetWeight(giftList[i].giftGrade);
        }
    }

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
        InventoryInstance().AddItem(gift);       // 인벤토리에 저장
    }

    /// <summary>
    /// 인벤토리 싱글톤 반환
    /// </summary>
    /// <returns></returns>
    Inventory InventoryInstance()
    {
        inventory = Inventory.Instance;
        
        return inventory;
    }

    /// <summary>
    /// 데이터 저장
    /// </summary>
    void SaveData()
    {
        string jdata = JsonUtility.ToJson(new Serialization<GiftInfo>(giftInfoList));
        File.WriteAllText(Application.persistentDataPath + "/GiftData.json", jdata);
    }

    /// <summary>
    /// 데이터 로드
    /// </summary>
    /// <returns>불러오기 성공 여부</returns>
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
    /// 데이터 생성
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
