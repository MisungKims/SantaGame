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
    //[SerializeField]
    private Image gradeImage;
    //[SerializeField]
    private Image giftimage;
    //[SerializeField]
    private Text gradeText;
   // [SerializeField]
    private Text wishCountText;
    //[SerializeField]
    private Text amountText;

    private EGiftType giftType;
    public EGiftType GiftType
    {
        get { return giftType; }
        set
        {
            giftType = value;

            //if (giftimage == null)
            //    InitVariable();
            giftimage.sprite = GiftManager.Instance.giftList[(int)giftType].giftImage;
        }
    }

    private EGiftGrade giftGrade;
    public EGiftGrade GiftGrade
    {
        get { return giftGrade; }
        set
        {
            giftGrade = value;

            //if (gradeText == null)
            //    InitVariable();

            gradeText.text = Enum.GetName(typeof(EGiftGrade), (int)giftGrade);
            gradeImage.sprite = Inventory.Instance.gradeSprite[(int)giftGrade];
        }
    }

    private int amount;
    public int Amount
    {
        get { return amount; }
        set
        {
            amount = value;

            //if (amountText == null)
            //    InitVariable();
            amountText.text = amount.ToString();
        }
    }

    private int wishCount;
    public int WishCount
    {
        get { return wishCount; }
        set
        {
            wishCount = value;

            //if (wishCountText == null)
            //    InitVariable();
            wishCountText.text = wishCount.ToString();
        }
    }

    private void Awake()
    {
        InitVariable();
    }

    private void InitVariable()
    {
        gradeImage = this.GetComponent<Image>();
        giftimage = this.transform.GetChild(0).GetComponent<Image>();
        gradeText = this.transform.GetChild(1).GetComponent<Text>();
        wishCountText = this.transform.GetChild(2).GetComponent<Text>();
        amountText = wishCountText.transform.GetChild(0).GetComponent<Text>();
    }

    /// <summary>
    /// ���Կ� �� ����
    /// </summary>
    /// <param name="item">���Կ� ���� ���� ������</param>
    public void SetSlot(GiftItem item)
    {
        // Debug.Log(this.name);

        Amount = item.amount;

        if (Amount <= 0)        // ������ 0���� ���ٸ�
        {
            SetEmpty();
        }
        else
        {
            SlotActive(true);
            GiftType = item.gift.giftType;
            GiftGrade = item.gift.giftGrade;
            WishCount = item.gift.wishCount;
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
    
    public void SlotActive(bool value)
    {
        giftimage.gameObject.SetActive(value);
        gradeText.gameObject.SetActive(value);
        wishCountText.gameObject.SetActive(value);
    }
}
