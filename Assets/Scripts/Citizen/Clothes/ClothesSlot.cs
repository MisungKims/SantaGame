/**
 * @brief ����ڰ� ���� ���� UI ����
 * @author ��̼�
 * @date 22-06-01
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClothesSlot : MonoBehaviour
{
    #region ����
    [SerializeField]
    private Image clothesImage;             // ������ �� �̹���

    [SerializeField]
    private GameObject checkImage;          // ���� ���� ���θ� �����ִ� �̹���

    [SerializeField]
    private Button button;                  // ������ ��ư

    private bool isWearing;                 // ���� ���� ����
    public bool IsWearing
    {
        set
        {
            isWearing = value;
            checkImage.SetActive(isWearing);
        }
    }

    public RabbitCitizen rabbitCitizen;     // ���� �䳢 �ֹ�â�� �䳢

    public Clothes clothes;                 // �ش� ������ ��
    #endregion

    #region ����Ƽ �Լ�
    private void OnEnable()
    {
        CheckWearing();

        SetInteractable();
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// ���� UI �ʱ�ȭ
    /// </summary>
    /// <param name="myClothes">�ش� ������ ��</param>
    public void Init(Clothes myClothes)
    {
        clothes = myClothes;
        clothesImage.sprite = myClothes.image;
    }

    /// <summary>
    /// �� ������ ���� �԰��ִ���?
    /// </summary>
    /// <returns></returns>
    bool CheckWearing()
    {
        if (!rabbitCitizen) return false;

        if (rabbitCitizen.clothes == clothes)       // �ش� ���� �԰� ���� ��
        {
            IsWearing = true;
            return true;
        }
        else    //���� �԰����� ���� ��
        {
            IsWearing = false;
            return false;
        }
    }

    /// <summary>
    /// ��ư�� Interactable ����
    /// </summary>
    void SetInteractable()
    {
        // ���� ���� �� ���� �䳢�� ������ ���� �԰� ���� ���� �� ��ư�� ���� �� ����
        if (clothes.totalAmount <= clothes.wearingCount && rabbitCitizen.clothes != clothes)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }


    /// <summary>
    /// �䳢 �ֹο��� ���� �����ų� ����
    /// </summary>
    public void PutOnOrOffRabbit()
    {
        if (!rabbitCitizen) return;

        if (CheckWearing())     // �Ȱ��� ���� �԰����� ���� ����
        {
            PutOff();
        }
        else   // �Ȱ��� ���� �԰����� ���� ���� ����
        {
            if (clothes.totalAmount > clothes.wearingCount)
            {
                if (rabbitCitizen.clothes != null)      // �̹� �ٸ� ���� �԰� ���� ������
                {
                    for (int i = 0; i < ClothesManager.Instance.clothesSlots.Count; i++)
                    {
                        if (ClothesManager.Instance.clothesSlots[i].clothes == rabbitCitizen.clothes)
                        {
                            ClothesManager.Instance.clothesSlots[i].PutOff();           // �ش� ���� ����
                            break;
                        }
                    }
                }

                PutOn();
            }
        }

        SetInteractable();
    }

    /// <summary>
    /// �䳢���� ���� ����
    /// </summary>
    void PutOn()
    {
        if (rabbitCitizen.PutOn(clothes))
        {
            clothes.wearingCount++;
            IsWearing = true;
        }
    }

    /// <summary>
    /// �䳢���� ���� ����
    /// </summary>
    void PutOff()
    {
        rabbitCitizen.PutOff();
        clothes.wearingCount--;

        IsWearing = false;
    }
    #endregion
}
