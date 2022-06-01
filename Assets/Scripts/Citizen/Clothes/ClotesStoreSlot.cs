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

    private string clothesName;         // ���� �̸�
    public string ClothesName
    {
        set
        {
            clothesName = value;
            clothesNameText.text = clothesName;
        }
    }
    Button button;
   
    private int price;                 // ���� ����
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
    /// �𵨿��� ���� ������ (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void Wear()
    {
        if (model.clothes != clothes)       // ���� ���� �ʰų� �ٸ� ���� �԰� ���� ������ ���� ����
        {
            if (model.clothes.clothesName != null && !model.clothes.clothesName.Equals(""))
            {
                model.PutOff();
            }
            model.PutOn(clothes);
        }
        else         // �ش� ������ ���� �԰� �־��ٸ� ���� ����
        {
            model.PutOff();         
        }
    }

    /// <summary>
    /// ���� ������ (�ν����Ϳ��� ȣ��)
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
