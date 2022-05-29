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
    // �̱���
    private static InviteRabbit instance;
    public static InviteRabbit Instance
    {
        get { return instance; }
    }

    // UI ����
    [SerializeField]
    private Text priceText;
    [SerializeField]
    private Text citizenCountText;
    public GameObject rabbit;
    public GameObject rabbitGroup;

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
        if (instance == null)
            instance = this;
        else
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
        }

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

        // �䳢 �ֹ� ���� �� ī�޶��� Ÿ������ ����
        RabbitCitizen rabbitCitizen = GameObject.Instantiate(rabbit, rabbitGroup.transform).GetComponent<RabbitCitizen>();
        CitizenRabbitManager.Instance.rabbitCitizens.Add(rabbitCitizen);

        rabbitCitizen.SetCamTargetThis();

        gameManager.MyCarrots -= GoldManager.UnitToBigInteger(price);  // �ʴ� ��� ����

        Count = ++gameManager.CitizenCount + 1;

        gameManager.goldEfficiency *= 1.5f;     // ȿ�� ����

        Price = GoldManager.MultiplyUnit(price, magnification);
        magnification += 0.5f;

        int rand = Random.Range(0, 12);
        rabbitCitizen.rabbitMat.material = CitizenRabbitManager.Instance.materials[rand];       // �䳢�� Material�� �������� ����


        Citizen citizen = new Citizen(CitizenRabbitManager.Instance.materials[rand], rabbitCitizen.transform.position);
        CitizenRabbitManager.Instance.citizenList.Add(citizen);
    }

    #endregion
}
