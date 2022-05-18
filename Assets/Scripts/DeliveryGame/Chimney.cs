/**
 * @brief ���� ���� ������ ����
 * @author ��̼�
 * @date 22-05-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chimney : Obstacle
{
    #region ����
    public Image giftImage;
    public Gift gift;

    private DeliveryGameManager deliveryGameManager;
    private Inventory inventory;
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

        gift = inventory.RandomGet();      // �κ��丮�� �ִ� ������ ������
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
