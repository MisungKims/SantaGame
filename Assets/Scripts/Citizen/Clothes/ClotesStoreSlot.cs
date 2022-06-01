using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClotesStoreSlot : MonoBehaviour
{
    [SerializeField]
    private Text clothesNameText;
    [SerializeField]
    private Image clothesImage;
    [SerializeField]
    private Text priceText;

    private string clothesName;         // 옷의 이름
    public string ClothesName
    {
        set
        {
            clothesName = value;
            clothesNameText.text = clothesName;
        }
    }
    Button button;
   
    private int price;                 // 옷의 가격
    public int Price
    {
        set
        {
            price = value;
            priceText.text = price.ToString();
        }
    }

    public Clothes clothes;

    [SerializeField]
    private RabbitModel model;


    public void Init(Clothes clothes)
    {
        this.clothes = clothes;
        clothesName = clothes.clothesName;
        clothesImage.sprite = clothes.image;
        price = clothes.price;

    }

    /// <summary>
    /// 모델에게 옷을 입혀봄 (인스펙터에서 호출)
    /// </summary>
    public void Wear()
    {
        if (model.clothes != clothes)       // 옷을 입지 않거나 다른 옷을 입고 있을 때에는 옷을 입힘
        {
            if (model.clothes.clothesName != null && !model.clothes.clothesName.Equals(""))
            {
                model.PutOff();
            }
            model.PutOn(clothes);
        }
        else         // 해당 슬롯의 옷을 입고 있었다면 옷을 벗김
        {
            model.PutOff();         
        }
    }

    /// <summary>
    /// 옷을 구입함 (인스펙터에서 호출)
    /// </summary>
    public void Buy()
    {
        if (GameManager.Instance.MyDia >= price)
        {
            GameManager.Instance.MyDia -= price;

            ClothesManager.Instance.GetClothes(clothes);
        }
        
    }
}
