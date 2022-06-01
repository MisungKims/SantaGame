/**
 * @brief �䳢 �ֹ��� ���� ����
 * @author ��̼�
 * @date 22-06-01
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ClothesManager : MonoBehaviour
{
    #region ����
    // �̱���
    private static ClothesManager instance;
    public static ClothesManager Instance
    {
        get { return instance; }
    }

    public List<Clothes> clothesList = new List<Clothes>();         // �� ����Ʈ

    public List<Sprite> clothesImageList = new List<Sprite>();      // ���� �̹��� ����Ʈ

    // UI ����
    [SerializeField]
    private Transform clothesScrollView;

    [SerializeField]
    private ClothesSlot clothesSlot;

    public List<ClothesSlot> clothesSlots = new List<ClothesSlot>();    // ���� UI ����

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

        for (int i = 0; i < clothesList.Count; i++)
        {
            clothesList[i].clothesInfo.totalAmount = 0;
            clothesList[i].clothesInfo.wearingCount = 0;
        }

        for (int i = 0; i < clothesImageList.Count; i++)
        {
            clothesList[i].image = clothesImageList[i];
        }

        getRewardWindow = UIManager.Instance.getRewardWindow;

        ObjectPoolingManager.Instance.InitClothes();
    }

    public void Start()
    {
        GetRandomClothes();
        GetRandomClothes();
    }
    #endregion

    #region �Լ�
    public void GetClothes(Clothes clothes)
    {
        if (clothes.clothesInfo.totalAmount <= 0)       // ���� ���� ���� ��
        {
            AddClothesSlot(clothes);
        }

        clothes.clothesInfo.totalAmount++;
    }

    public void GetRandomClothes()
    {
        int rand = Random.Range(0, clothesList.Count);

        GetClothes(clothesList[rand]);

        getRewardWindow.OpenWindow(clothesList[rand].clothesName, clothesList[rand].image);      // ���� ȹ��â ������
    }

    public void AddClothesSlot(Clothes clothes)
    {
        ClothesSlot newSlot = ClothesSlot.Instantiate(clothesSlot, clothesScrollView);

        newSlot.Init(clothes);

        RectTransform rect = newSlot.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2((rect.anchoredPosition.x + 147) * clothesSlots.Count - 228, 0);

        clothesSlots.Add(newSlot);
    }
    #endregion




}
