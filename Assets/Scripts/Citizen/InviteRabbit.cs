/**
 * @details �䳢�� �ʴ�
 * @author ��̼�
 * @date 22-06-04
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

    // ������Ƽ
    public string Price     // �䳢 �ֹ� �ʴ� ����
    {
        get { return gameManager.inviteRabbitPrice; }
        set
        {
            gameManager.inviteRabbitPrice = value;
            priceText.text = gameManager.inviteRabbitPrice;
        }
    }

    public int Count        // (Count) ��° �ֹ� �߰�
    {
        set
        {
            citizenCountText.text = value.ToString();
        }
    }


    public GameObject rabbit;           // �䳢 �ֹ� ������
    public GameObject rabbitGroup;      // �䳢 �ֹ��� ���̶�Ű �θ�

    // �䳢 �ʴ� ��� ���� �� �ʿ�
    private float magnification = 1.7f;

    // ĳ��
    private GameManager gameManager;
    private CitizenRabbitManager citizenRabbitManager;
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
        Price = gameManager.inviteRabbitPrice;

        citizenRabbitManager = CitizenRabbitManager.Instance;
    }

    private void OnEnable()
    {
        Count = (gameManager.CitizenCount + 1);
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// �䳢�� �ʴ� (���ο� �䳢 ����) (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void Invite()
    {
        // ������ ����� ���ٸ� return
        if (!GoldManager.CompareBigintAndUnit(gameManager.MyCarrots, Price))    
        {
            return;
        }

        // �䳢 �ʴ�â �ݱ�
        this.gameObject.SetActive(false);   

        // �䳢 �ֹ� ���� �� ī�޶��� Ÿ������ ����
        RabbitCitizen rabbitCitizen = GameObject.Instantiate(rabbit, rabbitGroup.transform).GetComponent<RabbitCitizen>();
        citizenRabbitManager.rabbitCitizens.Add(rabbitCitizen);
        rabbitCitizen.name = citizenRabbitManager.rabbitCitizens.Count.ToString();
        rabbitCitizen.SetCamTargetThis();

        // �ʴ� ��� ����
        gameManager.MyCarrots -= GoldManager.UnitToBigInteger(Price);  

        // �䳢 �ֹ� �� ����
        Count = ++gameManager.CitizenCount + 1;         

        // ȿ�� ����
        gameManager.goldEfficiency *= 1.5f;             

        // �䳢 �ʴ� ���� ����
        Price = GoldManager.MultiplyUnit(Price, magnification);     
        magnification += 0.5f;

        // �䳢�� Material�� �������� ����
        int rand = Random.Range(0, 12);
        rabbitCitizen.rabbitMat.material = citizenRabbitManager.materials[rand];       

        // �䳢�� ������ ������ �� �ν��Ͻ� ����
        Citizen citizen = new Citizen(rabbitCitizen.name, rand, -1, rabbitCitizen.transform.position);
        citizenRabbitManager.citizenList.Add(citizen);
    }

    #endregion
}
