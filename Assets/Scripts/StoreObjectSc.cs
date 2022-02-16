using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreObjectSc : MonoBehaviour
{
    #region ����
    Text buidingNameText;
    Text buildingCostText;
    Text incrementMoneyText;
    Text unlockText;
    Text descText;
    GameObject unlockImage;

    public GameManager gameManager;

    [Header("����")]
    public bool isBuy = false;             // �ǹ��� �� ������ �ƴ���
    public string buildingName;            // �ǹ� �̸�
    public float multiplyCost;             // ���׷��̵� �� �ǹ� ���� ���� ����
    public int cost;                       // �ǹ� ���� 
    public float multiplyMoney;            // ���׷��̵� �� �÷��̾� �� ���� ����
    public int incrementMoney;             // �÷��̾��� �� ������
    public int unlockLevel;                // ��� ���� ���� ����
    public int buildingLevel;              // �ش� �ǹ��� ����
    public int second;                     // �� �� ���� ������ ������
    public string desc;                    // �ǹ��� ����

    Button button;

    #endregion

    /// <summary>
    /// �ǹ� ��ư Ŭ�� �� ��� ���� Ȥ�� �ǹ� ���׷��̵�
    /// </summary>
    public void Upgrade()
    {
        if (gameManager.myMoney >= cost)        // �÷��̾ ���� ���� �ǹ��� ���ݺ��� ���� ��
        {
            if (!isBuy) Unlock();               // ���� ���� �ǹ��̸� ��� ����
            else                                // �� �ǹ��̸� ���׷��̵�
            {
                gameManager.DecreaseMoney(cost);

                cost = (int)(cost * multiplyCost);    // ����� ������ŭ ����
                buildingCostText.text = GetCommaText(cost);

                incrementMoney = (int)(incrementMoney * multiplyMoney);     // ���� �������� ������ŭ ����
                incrementMoneyText.text = "���� ������ : " + GetCommaText(incrementMoney);

                buildingLevel++;                       // �ǹ��� ���� ��
            }
        }
    }

    /// <summary>
    /// 1000 ���� ���� �޸��� �ٿ��ִ� �Լ�
    /// </summary>
    string GetCommaText(int i)
    {
        return string.Format("{0: #,###; -#,###;0}", i);
    }

    /// <summary>
    /// �÷��̾��� ������ ���� ��ư�� Interactable ����
    /// </summary>
    void SetButtonInteractable()
    {
        if (gameManager.level >= unlockLevel && gameManager.myMoney >= cost)        // �÷��̾��� ������ ��� ���� ���� �������� ũ�� ���� ���� �ǹ��� ���ݺ��� Ŭ ��
            button.interactable = true;                                             //  Interactable�� True�� ����
        else button.interactable = false;
    }


    /// <summary>
    /// �� �ǹ��� ����� ����
    /// </summary>
    void Unlock()
    {
        isBuy = true;

        gameManager.DecreaseMoney(cost);

        gameManager.DoIncreaseMoney(second, incrementMoney);        // ������ �ð����� �� �����ϱ� ����

        unlockImage.SetActive(false);           // ��� �̹����� ����

        // �ؽ�Ʈ�� Active�� true��
        incrementMoneyText.gameObject.SetActive(true);
        descText.gameObject.SetActive(true);
    }


    void Start()
    {
        button = this.transform.GetChild(0).GetComponent<Button>();

        buidingNameText = this.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        buidingNameText.text = buildingName;

        buildingCostText = this.transform.GetChild(0).GetChild(1).GetComponent<Text>();
        buildingCostText.text = GetCommaText(cost);

        incrementMoneyText = this.transform.GetChild(0).GetChild(2).GetComponent<Text>();
        incrementMoneyText.text = "���� ������ : " + GetCommaText(incrementMoney);

        unlockText = this.transform.GetChild(0).GetChild(3).GetComponent<Text>();
        unlockText.text = "Lv." + unlockLevel.ToString();

        descText = this.transform.GetChild(0).GetChild(4).GetComponent<Text>();
        descText.text = desc;

        unlockImage = this.transform.GetChild(0).GetChild(5).gameObject;

        // ���� ���� �ǹ��� ��
        if (!isBuy)
        {
            unlockImage.SetActive(true);       // ��� �̹����� ������

            // �������� ���� �ؽ�Ʈ�� ����
            incrementMoneyText.gameObject.SetActive(false);
            descText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        SetButtonInteractable();
    }
}
