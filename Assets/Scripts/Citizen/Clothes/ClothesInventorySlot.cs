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

    private string clothesName;         // ���� �̸�
    public string ClothesName
    {
        set
        {
            clothesName = value;
            clothesNameText.text = clothesName;
        }
    }

    private int amount;                 // ���� ����
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

}
