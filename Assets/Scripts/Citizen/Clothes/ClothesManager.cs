/**
 * @brief Åä³¢ ÁÖ¹ÎÀÇ ¿ÊÀ» °ü¸®
 * @author ±è¹Ì¼º
 * @date 22-06-01
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ClothesManager : MonoBehaviour
{
    #region º¯¼ö
    // ½Ì±ÛÅæ
    private static ClothesManager instance;
    public static ClothesManager Instance
    {
        get { return instance; }
    }

    public List<Clothes> clothesList = new List<Clothes>();         // ¿Ê ¸®½ºÆ®

    public List<Sprite> clothesImageList = new List<Sprite>();      // ¿ÊÀÇ ÀÌ¹ÌÁö ¸®½ºÆ®

    // UI º¯¼ö
    [SerializeField]
    private Transform clothesScrollView;

    [SerializeField]
    private ClothesSlot clothesSlot;

    public List<ClothesSlot> clothesSlots = new List<ClothesSlot>();    // ¿ÊÀÇ UI ½½·Ô

    // Ä³½Ì
    private GetRewardWindow getRewardWindow;
    #endregion

    #region À¯´ÏÆ¼ ÇÔ¼ö
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

    #region ÇÔ¼ö
    public void GetClothes(Clothes clothes)
    {
        if (clothes.clothesInfo.totalAmount <= 0)       // »õ·Î ¹ÞÀº ¿ÊÀÏ ¶§
        {
            AddClothesSlot(clothes);
        }

        clothes.clothesInfo.totalAmount++;
    }

    public void GetRandomClothes()
    {
        int rand = Random.Range(0, clothesList.Count);

        GetClothes(clothesList[rand]);

        getRewardWindow.OpenWindow(clothesList[rand].clothesName, clothesList[rand].image);      // º¸»ó È¹µæÃ¢ º¸¿©ÁÜ
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
