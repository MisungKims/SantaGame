/**
 * @brief ���� ����ü
 * @author ��̼�
 * @date 22-04-24
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EGiftGrade
{
    SS,
    S,
    A,
    B,
    C
}

/// <summary>
/// ���� ����
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
    stationery,  // �п�ǰ
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

    public GiftInfo giftInfo;

    //public int wishCount;
    //public int inventoryIndex = -1;

    public Gift(Gift gift)
    {
        this.giftType = gift.giftType;
        this.giftGrade = gift.giftGrade;
        this.giftName = gift.giftName;
        this.giftImage = gift.giftImage;
        this.giftInfo = gift.giftInfo;

        //this.wishCount = gift.wishCount;
        //this.inventoryIndex = gift.inventoryIndex;
    }

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


