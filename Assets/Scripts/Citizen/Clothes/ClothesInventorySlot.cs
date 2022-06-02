using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ClothesInventorySlot : MonoBehaviour
{
    [SerializeField]
    private Text clothesNameText;
    [SerializeField]
    private Image clothesImage;
    [SerializeField]
    private Text amountText;

    private string clothesName;         // ø ¿« ¿Ã∏ß
    public string ClothesName
    {
        set
        {
            clothesName = value;
            clothesNameText.text = clothesName;
        }
    }

    private int amount;                 // ø ¿« ∞°∞›
    public int Amount
    {
        set
        {
            amount = value;
            amountText.text = amount.ToString();
        }
    }

    public Clothes clothes;

    [SerializeField]
    private RabbitModel model;


    public void Init(Clothes clothes)
    {
        this.clothes = clothes;
        ClothesName = clothes.clothesName;
        clothesImage.sprite = clothes.image;
        Amount = clothes.clothesInfo.totalAmount;
    }

    /// <summary>
    /// ∏µ®ø°∞‘ ø ¿ª ¿‘«Ù∫Ω (¿ŒΩ∫∆Â≈Õø°º≠ »£√‚)
    /// </summary>
    public void Wear()
    {
        if (model.clothes != clothes)       // ø ¿ª ¿‘¡ˆ æ ∞≈≥™ ¥Ÿ∏• ø ¿ª ¿‘∞Ì ¿÷¿ª ∂ßø°¥¬ ø ¿ª ¿‘»˚
        {
            if (model.clothes.clothesName != null && !model.clothes.clothesName.Equals(""))
            {
                model.PutOff();
            }
            
            model.PutOn(clothes);
        }
        else         // «ÿ¥Á ΩΩ∑‘¿« ø ¿ª ¿‘∞Ì ¿÷æ˙¥Ÿ∏È ø ¿ª π˛±Ë
        {
            model.PutOff();
        }
    }

}
