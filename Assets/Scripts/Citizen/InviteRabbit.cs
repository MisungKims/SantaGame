using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InviteRabbit : MonoBehaviour
{
    [SerializeField]
    private Text priceText;
    [SerializeField]
    private GameObject rabbit;
    [SerializeField]
    private GameObject rabbitGroup;

    private string price = "100";
    public string Price
    {
        set
        {
            price = value;
            priceText.text = price;
        }
    }

    private float startF = 1.7f;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        priceText.text = price;
    }

    public void Invite()
    {
        // 지불할 당근이 없다면 return
        if (!GoldManager.CompareBigintAndUnit(gameManager.MyCarrots, price))
        {
            return;
        }

        GameObject.Instantiate(rabbit, rabbitGroup.transform);          // 토끼 주민 생성

        gameManager.MyCarrots -= GoldManager.UnitToBigInteger(price);  // 초대 비용 지불

        gameManager.CitizenCount++;

        gameManager.goldEfficiency *= 1.5f;

        Price = GoldManager.MultiplyUnit(price, startF);
        startF += 0.5f;
    }
}
