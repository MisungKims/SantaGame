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

    public bool isAlreadyGet;       // ������ ���� ��������?

    // ĳ��
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

        isAlreadyGet = false;

        giftImage.transform.parent.gameObject.SetActive(true);

        gift = inventory.RandomGet();      // �κ��丮�� �ִ� ������ �������� ������
        if (gift == null)
        {
            if(deliveryGameManager != null) deliveryGameManager.End(true);
        }
        else
        {
            giftImage.sprite = gift.giftImage;
        }
    }
    #endregion
}
