/**
 * @brief ���� �κ��丮 ����
 * @author ��̼�
 * @date 22-05-04
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[System.Serializable]
public class Slot : MonoBehaviour
{
    #region ����
    // UI ����
    [SerializeField]
    private Image gradeImage;
    [SerializeField]
    private Image giftimage;
    [SerializeField]
    private Text gradeText;
    [SerializeField]
    private Text amountText;

    // ������Ƽ
    private EGiftType giftType;         // ������ Ÿ��
    public EGiftType GiftType
    {
        get { return giftType; }
        set
        {
            giftType = value;

            giftimage.sprite = GiftManager.Instance.giftList[(int)giftType].giftImage;      // ������ �̹��� ����
        }
    }

    private EGiftGrade giftGrade;       // ������ ���
    public EGiftGrade GiftGrade
    {
        get { return giftGrade; }
        set
        {
            giftGrade = value;

            gradeText.text = Enum.GetName(typeof(EGiftGrade), (int)giftGrade);
            gradeImage.sprite = Inventory.Instance.gradeSprite[(int)giftGrade];         // ��޺� �̹��� ����
        }
    }

    private int amount;                 // ������ ����
    public int Amount
    {
        get { return amount; }
        set
        {
            amount = value;

            amountText.text = amount.ToString();
        }
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// ���Կ� �� ����
    /// </summary>
    /// <param name="item">���Կ� ���� ���� ������</param>
    public void SetSlot(GiftItem item)
    {
        Amount = item.amount;

        if (Amount <= 0)        // ������ 0���� ���ٸ� �� ��������
        {
            SetEmpty();
        }
        else
        {
            SlotActive(true);
            GiftType = item.gift.giftType;
            GiftGrade = item.gift.giftGrade;
        }
    }

    /// <summary>
    /// �� �������� �����
    /// </summary>
    public void SetEmpty()
    {
        SlotActive(false);
        gradeImage.sprite = Inventory.Instance.gradeSprite[5];      // �� ���� �̹����� ����
    }

    /// <summary>
    /// ���� UI�� Ȱ��ȭ ����
    /// </summary>
    /// <param name="value">Ȱ��ȭ ����</param>
    public void SlotActive(bool value)
    {
        giftimage.gameObject.SetActive(value);
        gradeText.gameObject.SetActive(value);
        amountText.gameObject.SetActive(value);
    }
    #endregion
}
