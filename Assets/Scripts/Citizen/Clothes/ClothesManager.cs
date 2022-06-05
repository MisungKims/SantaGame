/**
 * @brief �䳢 �ֹ��� ���� ����
 * @author ��̼�
 * @date 22-06-04
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
    public List<Clothes> clothesList = new List<Clothes>();                 // �� ����Ʈ

    public List<ClothesInfo> clothesInfoList = new List<ClothesInfo>();     // ���� ������ ���� ����Ʈ

    [Header("-------- Citizen Panel Slot")]
    public List<ClothesSlot> clothesSlotList = new List<ClothesSlot>();

    [HideInInspector]
    public int clothesSlotCount = 0;        // ���� ���� ���� ��


    [Header("-------- Clothes Store")]
    public List<ClotesStoreSlot> clotesStoreSlots = new List<ClotesStoreSlot>();        // �� ���� ���� ����Ʈ

    [SerializeField]
    private GameObject store;

    [Header("-------- Clothes Inventory")]
    public List<ClothesInventorySlot> clotesInventorySlots = new List<ClothesInventorySlot>();      // �� ������ ���� ����Ʈ

    [SerializeField]
    private GameObject inventory;


    // ������ ����
    bool isPaused = false;      //���� Ȱ��ȭ ����


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

        getRewardWindow = UIManager.Instance.getRewardWindow;   // ĳ��

        // �����͸� �ε�
        if (!LoadData())
        {
            NewData();                  // ������ �ε忡 ���н� ���ο� ������ ����
        }
        
        // ������Ʈ Ǯ������ �̸� �� ������Ʈ ����
        ObjectPoolingManager.Instance.InitClothes();            

        InitStore();
        RefreshInventory();
    }

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
            }
        }
    }

    void OnApplicationQuit()
    {
        SaveData();         // �� ���� �� ������ ����
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// ���� ȹ��
    /// </summary>
    /// <param name="clothes">ȹ���� ��</param>
    public void GetClothes(Clothes clothes)
    {
        if (clothes.clothesInfo.totalAmount <= 0)       // ���� ���� ���� ��
        {
            AddClothesSlot(clothes);                    // ���� ���Կ��� �߰�
        }

        clothes.clothesInfo.totalAmount++;

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
    /// ���� UI ������ �߰�
    /// </summary>
    /// <param name="clothes"></param>
    public void AddClothesSlot(Clothes clothes)
    {
        clothesSlotList[clothesSlotCount].SetClothes(clothes);
        clothesSlotCount++;
    }

    /// <summary>
    /// ���� �ʱ�ȭ
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
    /// �κ��丮 ���ΰ�ħ
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

        // �������� �ʾƵ� �Ǵ� ������ active�� ��
        for (int i = index; i < clotesInventorySlots.Count; i++)
        {
            clotesInventorySlots[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ������ �� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void OpenStore()
    {
        store.SetActive(true);
        inventory.SetActive(false);
    }

    /// <summary>
    /// �κ��丮�� �� (�ν����Ϳ��� ȣ��)
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

    /// <summary>
    /// ������ ����
    /// </summary>
    void SaveData()
    {
        string jdata = JsonUtility.ToJson(new Serialization<ClothesInfo>(clothesInfoList));
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

            clothesInfoList = JsonUtility.FromJson<Serialization<ClothesInfo>>(jdata).target;

            // �ֹ� �г��� ���� ���� UI �߰�
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
    /// ���ο� ������ ����
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
