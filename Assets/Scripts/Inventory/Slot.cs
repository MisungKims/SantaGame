/**
 * @brief 선물 인벤토리 슬롯
 * @author 김미성
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
    [SerializeField]
    private Image gradeImage;
    [SerializeField]
    private Image giftimage;
    [SerializeField]
    private Text gradeText;
    //[SerializeField]
    //private Text wishCountText;
    [SerializeField]
    private Text amountText;


    private EGiftType giftType;
    public EGiftType GiftType
    {
        get { return giftType; }
        set
        {
            giftType = value;

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

            amountText.text = amount.ToString();
        }
    }

    //private int wishCount;
    //public int WishCount
    //{
    //    get { return wishCount; }
    //    set
    //    {
    //        wishCount = value;

    //        wishCountText.text = "/" + wishCount.ToString();
    //    }
    //}

    //private void Awake()
    //{
    //    gradeImage = this.GetComponent<Image>();
    //    giftimage = this.transform.GetChild(0).GetComponent<Image>();
    //    gradeText = this.transform.GetChild(1).GetComponent<Text>();
    //    amountText = this.transform.GetChild(2).GetComponent<Text>();
    //}


    /// <summary>
    /// 슬롯에 값 대입
    /// </summary>
    /// <param name="item">슬롯에 넣을 선물 아이템</param>
    public void SetSlot(GiftItem item)
    {
        Amount = item.amount;

        if (Amount <= 0)        // 수량이 0보다 적다면
        {
            SetEmpty();
        }
        else
        {
            SlotActive(true);
            GiftType = item.gift.giftType;
            GiftGrade = item.gift.giftGrade;
           // WishCount = item.gift.wishCount;
        }
    }

    /// <summary>
    /// 빈 슬롯으로 만들기
    /// </summary>
    public void SetEmpty()
    {
        SlotActive(false);
        gradeImage.sprite = Inventory.Instance.gradeSprite[5];      // 빈 슬롯 이미지로 변경
    }
    
    public void SlotActive(bool value)
    {
        giftimage.gameObject.SetActive(value);
        gradeText.gameObject.SetActive(value);
        amountText.gameObject.SetActive(value);
    }
}
