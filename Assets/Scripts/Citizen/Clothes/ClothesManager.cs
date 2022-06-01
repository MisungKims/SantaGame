/**
 * @brief 토끼 주민의 옷을 관리
 * @author 김미성
 * @date 22-06-01
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
    public List<Clothes> clothesList = new List<Clothes>();             // 옷 리스트

    [SerializeField]
    private List<Sprite> clothesImageList = new List<Sprite>();      // 옷의 이미지 리스트

    [SerializeField]
    private List<GameObject> clothesPrefabList = new List<GameObject>();      // 옷의 프리팹 리스트

    // UI 변수
    [Header("-------- Citizen Panel Slot")]
    [SerializeField]
    private Transform clothesScrollView;

    [SerializeField]
    private ClothesSlot clothesSlot;

    public List<ClothesSlot> clothesSlots = new List<ClothesSlot>();    // 옷의 UI 슬롯

    [Header("-------- Clothes Store")]
    public List<ClotesStoreSlot> clotesStoreSlots = new List<ClotesStoreSlot>();

    [SerializeField]
    private GameObject store;

    [Header("-------- Clothes Inventory")]
    public List<ClothesInventorySlot> clotesInventorySlots = new List<ClothesInventorySlot>();

    [SerializeField]
    private GameObject inventory;



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

        if (!LoadData())
        {
            for (int i = 0; i < clothesList.Count; i++)
            {
                clothesList[i].totalAmount = 0;
                clothesList[i].wearingCount = 0;
            }
        }
        

        //for (int i = 0; i < clothesList.Count; i++)
        //{
        //    clothesList[i].image = clothesImageList[i];
        //    clothesList[i].clothesPrefabs = clothesPrefabList[i];
        //}

      

        getRewardWindow = UIManager.Instance.getRewardWindow;

        ObjectPoolingManager.Instance.InitClothes();

        InitStore();
        RefreshInventory();
    }

    //public void Start()
    //{
    //    GetClothes(clothesList[0]);
    //    GetClothes(clothesList[1]);
    //}
    #endregion

    #region 함수
    /// <summary>
    /// 옷을 얻음
    /// </summary>
    /// <param name="clothes"></param>
    public void GetClothes(Clothes clothes)
    {
        if (clothes.totalAmount <= 0)       // 새로 받은 옷일 때
        {
            AddClothesSlot(clothes);
        }

        clothes.totalAmount++;

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
    /// UI 슬롯을 추가
    /// </summary>
    /// <param name="clothes"></param>
    public void AddClothesSlot(Clothes clothes)
    {
        ClothesSlot newSlot = ClothesSlot.Instantiate(clothesSlot, clothesScrollView);

        newSlot.Init(clothes);

        RectTransform rect = newSlot.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(137 * clothesSlots.Count - 228, 0);

        clothesSlots.Add(newSlot);
    }

    /// <summary>
    /// 상점 초기화
    /// </summary>
    public void InitStore()
    {
        for (int i = 0; i < clothesList.Count; i++)
        {
            clotesStoreSlots[i].gameObject.SetActive(true);
            clotesStoreSlots[i].Init(clothesList[i]);
        }

        for (int i = clothesList.Count; i < clotesStoreSlots.Count; i++)
        {
            clotesStoreSlots[i].gameObject.SetActive(false);
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
            if (clothesList[i].totalAmount > 0)
            {
                clotesInventorySlots[index].gameObject.SetActive(true);
                clotesInventorySlots[index].Init(clothesList[i]);

                index++;
            }
        }

        for (int i = index; i < clotesInventorySlots.Count; i++)
        {
            clotesInventorySlots[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 상점을 엶
    /// </summary>
    public void OpenStore()
    {
        store.SetActive(true);
        inventory.SetActive(false);
    }
    
    /// <summary>
    /// 인벤토리를 엶
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
        string jdata = JsonUtility.ToJson(new Serialization<Clothes>(clothesList));
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

            clothesList = JsonUtility.FromJson<Serialization<Clothes>>(jdata).target;
            for (int i = 0; i < clothesList.Count; i++)
            {
                if (clothesList[i].totalAmount > 0)
                {
                    AddClothesSlot(clothesList[i]);
                }

                clothesList[i].image = clothesImageList[i];
                clothesList[i].clothesPrefabs = clothesPrefabList[i];
            }

            return true;
        }

        return false;
    }

}
