/**
 * @brief 토끼 주민의 옷을 관리
 * @author 김미성
 * @date 22-06-04
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class ClothesManager : MonoBehaviour
{
    #region 변수
    // 싱글톤
    private static ClothesManager instance;
    public static ClothesManager Instance
    {
        get { return instance; }
    }


    // 리스트
    public List<Clothes> clothesList = new List<Clothes>();                 // 옷 리스트

    public List<ClothesInfo> clothesInfoList = new List<ClothesInfo>();     // 옷의 정보를 담은 리스트

    [Header("-------- Citizen Panel Slot")]
    public List<ClothesSlot> clothesSlotList = new List<ClothesSlot>();

    [HideInInspector]
    public int clothesSlotCount = 0;        // 현재 가진 옷의 수


    [Header("-------- Clothes Store")]
    public List<ClotesStoreSlot> clotesStoreSlots = new List<ClotesStoreSlot>();        // 옷 가게 슬롯 리스트

    [SerializeField]
    private GameObject store;

    [Header("-------- Clothes Inventory")]
    public List<ClothesInventorySlot> clotesInventorySlots = new List<ClothesInventorySlot>();      // 옷 보관함 슬롯 리스트

    [SerializeField]
    private GameObject inventory;


    // 데이터 저장
    bool isPaused = false;      //앱의 활성화 상태


    // 캐싱
    private GetRewardWindow getRewardWindow;
    #endregion

    #region 유니티 함수
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        getRewardWindow = UIManager.Instance.getRewardWindow;   // 캐싱

        // 데이터를 로드
        if (!LoadData())
        {
            NewData();                  // 데이터 로드에 실패시 새로운 데이터 생성
        }
        
        // 오브젝트 풀링으로 미리 옷 오브젝트 생성
        ObjectPoolingManager.Instance.InitClothes();            

        InitStore();
        RefreshInventory();
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
    /// 옷을 획득
    /// </summary>
    /// <param name="clothes">획득할 옷</param>
    public void GetClothes(Clothes clothes)
    {
        if (clothes.clothesInfo.totalAmount <= 0)       // 새로 받은 옷일 때
        {
            AddClothesSlot(clothes);                    // 옷장 슬롯에도 추가
        }

        clothes.clothesInfo.totalAmount++;

        getRewardWindow.OpenWindow(clothes.clothesName, clothes.image);      // 보상 획득창 보여줌

        /// TODO : 인벤토리가 열린 상태이면
        RefreshInventory();
    }

    /// <summary>
    /// 랜덤으로 옷을 얻음
    /// </summary>
    public void GetRandomClothes()
    {
        int rand = Random.Range(0, clothesList.Count);

        GetClothes(clothesList[rand]);
    }

    /// <summary>
    /// 옷장 UI 슬롯을 추가
    /// </summary>
    /// <param name="clothes"></param>
    public void AddClothesSlot(Clothes clothes)
    {
        clothesSlotList[clothesSlotCount].SetClothes(clothes);
        clothesSlotCount++;
    }

    /// <summary>
    /// 상점 초기화
    /// </summary>
    public void InitStore()
    {
        for (int i = 0; i < clothesList.Count; i++)             
        {
            clotesStoreSlots[i].gameObject.SetActive(true);
            clotesStoreSlots[i].SetSlot(clothesList[i]);
        }
    }

    /// <summary>
    /// 인벤토리 새로고침
    /// </summary>
    public void RefreshInventory()
    {
        int index = 0;
        for (int i = 0; i < clothesList.Count; i++)
        {
            if (clothesList[i].clothesInfo.totalAmount > 0)
            {
                clotesInventorySlots[index].gameObject.SetActive(true);
                clotesInventorySlots[index].SetClothes(clothesList[i]);

                index++;
            }
        }

        // 보여지지 않아도 되는 슬롯은 active를 끔
        for (int i = index; i < clotesInventorySlots.Count; i++)
        {
            clotesInventorySlots[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 상점을 엶 (인스펙터에서 호출)
    /// </summary>
    public void OpenStore()
    {
        store.SetActive(true);
        inventory.SetActive(false);
    }

    /// <summary>
    /// 인벤토리를 엶 (인스펙터에서 호출)
    /// </summary>
    public void OpenInventory()
    {
        store.SetActive(false);
        inventory.SetActive(true);
    }

    /// <summary>
    /// 광고를 보고 보상 지급 (인스펙터에서 호출)
    /// </summary>
    public void ShowAdAndGetReward()
    {
        /// TODO : 광고 보기 구현 (다이아 10개?)
    }

    /// <summary>
    /// 데이터 저장
    /// </summary>
    void SaveData()
    {
        string jdata = JsonUtility.ToJson(new Serialization<ClothesInfo>(clothesInfoList));
        File.WriteAllText(Application.persistentDataPath + "/ClothesData.json", jdata);
    }

    /// <summary>
    /// 데이터 로드
    /// </summary>
    /// <returns>불러오기 성공 여부</returns>
    public bool LoadData()
    {
        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/ClothesData.json");
        if (fileInfo.Exists)
        {
            string jdata = File.ReadAllText(Application.persistentDataPath + "/ClothesData.json");

            clothesInfoList = JsonUtility.FromJson<Serialization<ClothesInfo>>(jdata).target;

            // 주민 패널의 옷장 슬롯 UI 추가
            for (int i = 0; i < clothesInfoList.Count; i++)
            {
                clothesList[i].clothesInfo = clothesInfoList[i];

                if (clothesList[i].clothesInfo.totalAmount > 0)
                {
                    AddClothesSlot(clothesList[i]);
                }
            }

            for (int i = clothesSlotCount; i < clothesSlotList.Count; i++)
            {
                clothesSlotList[i].Reset();
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// 새로운 데이터 생성
    /// </summary>
    public void NewData()
    {
        for (int i = 0; i < clothesList.Count; i++)             
        {
            ClothesInfo info = new ClothesInfo(0, 0);
            clothesInfoList.Add(info);
            clothesList[i].clothesInfo = info;
        }
    }
    #endregion
}
