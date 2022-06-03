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
  
    private int price;                 // ���� ����
    public int Price
    {
        set
        {
            price = value;
            priceText.text = price.ToString();
        }
    }

    [SerializeField]
    private Button buyButton;

    public Clothes clothes;

    [SerializeField]
    private RabbitModel model;

    private void OnEnable()
    {
        SetInteractable();
    }

    public void Init(Clothes clothes)
    {
        this.clothes = clothes;
        ClothesName = clothes.clothesName;
        clothesImage.sprite = clothes.image;
        Price = clothes.price;

        SetInteractable();
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

            SetInteractable();
        }
    }

    /// <summary>
    /// ���� �� �� �ִ¸�ŭ�� ���̾ư� ���� �� ��ư�� Ŭ������ ���ϵ���
    /// </summary>
    public void SetInteractable()
    {
        if (GameManager.Instance.MyDia < price)
        {
            buyButton.interactable = false;
        }
        else
        {
            buyButton.interactable = true;
        }
    }
}
