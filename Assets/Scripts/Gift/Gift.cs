/**
 * @brief 선물 구조체
 * @author 김미성
 * @date 22-06-02
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 선물 등급
/// </summary>
public enum EGiftGrade
{
    SS,
    S,
    A,
    B,
    C
}

/// <summary>
/// 선물 종류
/// </summary>
public enum EGiftType
{
    RCcar,
    camera,
    bike,
    block,
    hospital,
    watch,
    fishing,
    train,
    phone,
    animalDoll,
    princessDoll,
    dinosaurdoll,
    teddybear,
    gloves,
    boardGame,
    snacks,
    stationery,  // 학용품
    crayon,
    comicBook,
    bubbles
}

[System.Serializable]
public class Gift
{
    public EGiftType giftType;
    public EGiftGrade giftGrade;
    public string giftName;
    public Sprite giftImage;

    public GiftInfo giftInfo;       // 저장해야할 변수

    public Gift(Gift gift)
    {
        this.giftType = gift.giftType;
        this.giftGrade = gift.giftGrade;
        this.giftName = gift.giftName;
        this.giftImage = gift.giftImage;
        this.giftInfo = gift.giftInfo;
    }

    /// <summary>
    /// 선물의 등급에 따른 가중치
    /// </summary>
    /// <param name="giftGrade"></param>
    /// <returns></returns>
    public static int GetWeight(EGiftGrade giftGrade)
    {
        switch (giftGrade)
        {
            case EGiftGrade.SS:
                return 1;
            case EGiftGrade.S:
                return 9;
            case EGiftGrade.A:
                return 15;
            case EGiftGrade.B:
                return 25;
            case EGiftGrade.C:
                return 50;
            default:
                return 0;
        }
    }
}

[System.Serializable]
public class GiftInfo
{
    public int wishCount;
    public int inventoryIndex = -1;

    public GiftInfo(int wishCount, int inventoryIndex)
    {
        this.wishCount = wishCount;
        this.inventoryIndex = inventoryIndex;
    }
}


