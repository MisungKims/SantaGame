/**
 * @brief ���� ���� ������ ����
 * @author ��̼�
 * @date 22-05-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chimney : DeliveryGameObject
{
    #region ����
    public Image giftImage;
    public Gift gift;

    public Chimney preChimney;

    private DeliveryGameManager deliveryGameManager;
    private Inventory inventory;

    public bool isAlreadyGet;       // ������ ���� ��������?
    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        deliveryGameManager = DeliveryGameManager.Instance;
        inventory = Inventory.Instance;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        isAlreadyGet = false;

        gift = inventory.RandomGet();      // �κ��丮�� �ִ� ������ �������� ������

        //if (deliveryGameManager && deliveryGameManager.preChimney)
        //{
        //    preChimney = deliveryGameManager.preChimney;

        //    // �������� ������ �ϳ� �������� �������� ������ �ʵ��� ��
        //    GiftItem invItem = inventory.giftItems[preChimney.gift.inventoryIndex];
        //    if (invItem.gift == gift && invItem.amount <= 1)
        //    {
        //        gift = inventory.RandomGet(preChimney.gift.inventoryIndex);
        //    }
        //}
        
        if (gift == null)
        {
            deliveryGameManager.End(true);
        }
        else
        {
            giftImage.sprite = gift.giftImage;
        }
    }
    #endregion
}
