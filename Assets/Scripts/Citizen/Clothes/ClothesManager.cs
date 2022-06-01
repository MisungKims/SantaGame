/**
 * @brief �䳢 �ֹ��� ���� ����
 * @author ��̼�
 * @date 22-06-01
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class ClothesManager : MonoBehaviour
{
    #region ����
    // �̱���
    private static ClothesManager instance;
    public static ClothesManager Instance
    {
        get { return instance; }
    }

    // ����Ʈ
    public List<Clothes> clothesList = new List<Clothes>();             // �� ����Ʈ

    [SerializeField]
    private List<Sprite> clothesImageList = new List<Sprite>();      // ���� �̹��� ����Ʈ

    [SerializeField]
    private List<GameObject> clothesPrefabList = new List<GameObject>();      // ���� ������ ����Ʈ

    // UI ����
    [Header("-------- Citizen Panel Slot")]
    [SerializeField]
    private Transform clothesScrollView;

    [SerializeField]
    private ClothesSlot clothesSlot;

    public List<ClothesSlot> clothesSlots = new List<ClothesSlot>();    // ���� UI ����

    [Header("-------- Clothes Store")]
    public List<ClotesStoreSlot> clotesStoreSlots = new List<ClotesStoreSlot>();

    [SerializeField]
    private GameObject store;

    [Header("-------- Clothes Inventory")]
    public List<ClothesInventorySlot> clotesInventorySlots = new List<ClothesInventorySlot>();

    [SerializeField]
    private GameObject inventory;



    // ĳ��
    private GetRewardWindow getRewardWindow;
    #endregion

    #region ����Ƽ �Լ�
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

    #region �Լ�
    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="clothes"></param>
    public void GetClothes(Clothes clothes)
    {
        if (clothes.totalAmount <= 0)       // ���� ���� ���� ��
        {
            AddClothesSlot(clothes);
        }

        clothes.totalAmount++;

        getRewardWindow.OpenWindow(clothes.clothesName, clothes.image);      // ���� ȹ��â ������

        /// TODO : �κ��丮�� ���� �����̸�
        RefreshInventory();
    }

    /// <summary>
    /// �������� ���� ����
    /// </summary>
    public void GetRandomClothes()
    {
        int rand = Random.Range(0, clothesList.Count);

        GetClothes(clothesList[rand]);
    }

    /// <summary>
    /// UI ������ �߰�
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
    /// ���� �ʱ�ȭ
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
    /// �κ��丮 ���ΰ�ħ
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
    /// ������ ��
    /// </summary>
    public void OpenStore()
    {
        store.SetActive(true);
        inventory.SetActive(false);
    }
    
    /// <summary>
    /// �κ��丮�� ��
    /// </summary>
    public void OpenInventory()
    {
        store.SetActive(false);
        inventory.SetActive(true);
    }

    /// <summary>
    /// ���� ���� ���� ���� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void ShowAdAndGetReward()
    {
        /// TODO : ���� ���� ���� (���̾� 10��?)
    }

    #endregion


    //���� Ȱ��ȭ ���¸� �����ϴ� ����
    bool isPaused = false;

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
                /* ���� Ȱ��ȭ �Ǿ��� �� ó�� */
            }
        }
    }

    void OnApplicationQuit()
    {
        SaveData();         // �� ���� �� ������ ����
    }

    /// <summary>
    /// ������ ����
    /// </summary>
    void SaveData()
    {
        string jdata = JsonUtility.ToJson(new Serialization<Clothes>(clothesList));
        File.WriteAllText(Application.persistentDataPath + "/ClothesData.json", jdata);
    }

    /// <summary>
    /// ������ �ε�
    /// </summary>
    /// <returns>�ҷ����� ���� ����</returns>
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
