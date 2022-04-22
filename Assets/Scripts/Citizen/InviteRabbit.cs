/**
 * @details �䳢�� �ʴ�
 * @author ��̼�
 * @date 22-04-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InviteRabbit : MonoBehaviour
{
    #region ����
    // UI ����
    [SerializeField]
    private Text priceText;
    [SerializeField]
    private GameObject rabbit;
    [SerializeField]
    private GameObject rabbitGroup;

    // ������Ƽ
    private string price = "100";
    public string Price
    {
        set
        {
            price = value;
            priceText.text = price;
        }
    }

    // �䳢 �ʴ� ��� ���� �� �ʿ�
    private float magnification = 1.7f;

    // ĳ��
    private GameManager gameManager;
    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        gameManager = GameManager.Instance;
        priceText.text = price;
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// �䳢�� �ʴ� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void Invite()
    {
        if (!GoldManager.CompareBigintAndUnit(gameManager.MyCarrots, price))    // ������ ����� ���ٸ� return
        {
            return;
        }

        GameObject.Instantiate(rabbit, rabbitGroup.transform);          // �䳢 �ֹ� ����

        gameManager.MyCarrots -= GoldManager.UnitToBigInteger(price);  // �ʴ� ��� ����

        gameManager.CitizenCount++;

        gameManager.goldEfficiency *= 1.5f;     // ȿ�� ����

        Price = GoldManager.MultiplyUnit(price, magnification);
        magnification += 0.5f;
    }
    #endregion
}
