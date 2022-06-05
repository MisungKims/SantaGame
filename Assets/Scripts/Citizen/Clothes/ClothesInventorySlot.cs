/**
 * @brief �� �������� UI ����
 * @author ��̼�
 * @date 22-06-04
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ClothesInventorySlot : Closet
{
    #region ����
    [SerializeField]
    private Text amountText;

    private int amount;                 // ���� ����
    public int Amount
    {
        set
        {
            amount = value;
            amountText.text = amount.ToString();
        }
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="clothes">������ ��</param>
    public override void SetClothes(Clothes clothes)
    {
        base.SetClothes(clothes);

        Amount = clothes.clothesInfo.totalAmount;
    }
    #endregion
}
