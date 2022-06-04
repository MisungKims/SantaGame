/**
 * @brief ¿Ê º¸°üÇÔÀÇ UI ½½·Ô
 * @author ±è¹Ì¼º
 * @date 22-06-04
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ClothesInventorySlot : Closet
{
    #region º¯¼ö
    [SerializeField]
    private Text amountText;

    private int amount;                 // ¿ÊÀÇ °¡°Ý
    public int Amount
    {
        set
        {
            amount = value;
            amountText.text = amount.ToString();
        }
    }
    #endregion

    #region ÇÔ¼ö
    /// <summary>
    /// ½½·Ô ¼³Á¤
    /// </summary>
    /// <param name="clothes">¼³Á¤ÇÒ ¿Ê</param>
    public override void SetClothes(Clothes clothes)
    {
        base.SetClothes(clothes);

        Amount = clothes.clothesInfo.totalAmount;
    }
    #endregion
}
