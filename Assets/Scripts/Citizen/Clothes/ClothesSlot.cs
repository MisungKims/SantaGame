using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClothesSlot : MonoBehaviour
{
    [SerializeField]
    private Image clothesImage;

    [SerializeField]
    private GameObject checkImage;

    private bool isWearing;
    public bool IsWearing
    {
        set
        {
            isWearing = value;
            checkImage.SetActive(isWearing);
        }
    }

    public RabbitCitizen rabbitCitizen;
   
    public Clothes clothes;

    public void Init(Clothes myClothes)
    {
        clothes = myClothes;
        clothesImage.sprite = myClothes.image;
    }

    private void OnEnable()
    {
        CheckWearing();
    }

    /// <summary>
    /// ≈‰≥¢ ¡÷πŒø°∞‘ ø ¿ª ¿‘»˜∞≈≥™ π˛±Ë
    /// </summary>
    public void PutOnOrOffRabbit()
    {
        if (!rabbitCitizen) return;

        if (CheckWearing())     // ø ¿ª ¿‘∞Ì¿÷¿ª ∂ß¥¬ π˛±Ë
        {
            rabbitCitizen.PutOff();
            clothes.clothesInfo.wearingCount--;

            IsWearing = false;
        }
        else   // ø ¿ª ¿‘∞Ì¿÷¡ˆ æ ¿ª ∂ß¥¬ ¿‘»˚
        {
            if (clothes.clothesInfo.totalAmount > clothes.clothesInfo.wearingCount)
            {
                clothes.clothesInfo.wearingCount++;
                rabbitCitizen.PutOn(clothes);

                IsWearing = true;
            }
        }
    }

    bool CheckWearing()
    {
        if (!rabbitCitizen) return false;

        if (rabbitCitizen.clothes == clothes)       // ø ¿ª ¿‘∞Ì ¿÷¿ª ∂ß
        {
            IsWearing = true;
            return true;
        }
        else    //ø ¿ª ¿‘∞Ì¿÷¡ˆ æ ¿ª ∂ß
        {
            IsWearing = false;
            return false;
        }
    }

}
