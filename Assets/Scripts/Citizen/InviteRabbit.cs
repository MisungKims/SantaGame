/**
 * @details �䳢�� �ʴ�
 * @author ��̼�
 * @date 22-04-23
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
    private Text citizenCountText;
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

    private int count;
    public int Count
    {
        set
        {
            citizenCountText.text = value.ToString();
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

    private void OnEnable()
    {
        Count = (gameManager.CitizenCount + 1);
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

        this.gameObject.SetActive(false);   // �䳢 �ʴ�â �ݱ�

        GameObject.Instantiate(rabbit, rabbitGroup.transform).GetComponent<RabbitCitizen>().SetCamTargetThis();    // �䳢 �ֹ� ���� �� ī�޶��� Ÿ�� ����

        gameManager.MyCarrots -= GoldManager.UnitToBigInteger(price);  // �ʴ� ��� ����

        Count = ++gameManager.CitizenCount + 1;

        gameManager.goldEfficiency *= 1.5f;     // ȿ�� ����

        Price = GoldManager.MultiplyUnit(price, magnification);
        magnification += 0.5f;
    }
    #endregion
}
