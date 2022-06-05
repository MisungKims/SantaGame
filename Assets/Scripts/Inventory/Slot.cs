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
    #region 변수
    // UI 변수
    [SerializeField]
    private Image gradeImage;
    [SerializeField]
    private Image giftimage;
    [SerializeField]
    private Text gradeText;
    [SerializeField]
    private Text amountText;

    // 프로퍼티
    private EGiftType giftType;         // 선물의 타입
    public EGiftType GiftType
    {
        get { return giftType; }
        set
        {
            giftType = value;

            giftimage.sprite = GiftManager.Instance.giftList[(int)giftType].giftImage;      // 선물의 이미지 설정
        }
    }

    private EGiftGrade giftGrade;       // 선물의 등급
    public EGiftGrade GiftGrade
    {
        get { return giftGrade; }
        set
        {
            giftGrade = value;

            gradeText.text = Enum.GetName(typeof(EGiftGrade), (int)giftGrade);
            gradeImage.sprite = Inventory.Instance.gradeSprite[(int)giftGrade];         // 등급별 이미지 설정
        }
    }

    private int amount;                 // 선물의 수량
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

    #region 함수
    /// <summary>
    /// 슬롯에 값 대입
    /// </summary>
    /// <param name="item">슬롯에 넣을 선물 아이템</param>
    public void SetSlot(GiftItem item)
    {
        Amount = item.amount;

        if (Amount <= 0)        // 수량이 0보다 적다면 빈 슬롯으로
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
    /// 빈 슬롯으로 만들기
    /// </summary>
    public void SetEmpty()
    {
        SlotActive(false);
        gradeImage.sprite = Inventory.Instance.gradeSprite[5];      // 빈 슬롯 이미지로 변경
    }

    /// <summary>
    /// 슬롯 UI의 활성화 설정
    /// </summary>
    /// <param name="value">활성화 여부</param>
    public void SlotActive(bool value)
    {
        giftimage.gameObject.SetActive(value);
        gradeText.gameObject.SetActive(value);
        amountText.gameObject.SetActive(value);
    }
    #endregion
}
