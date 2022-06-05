/**
 * @brief ���� ���� ������ ����
 * @author ��̼�
 * @date 22-06-04
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
    private Inventory inventory;
    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        inventory = Inventory.Instance;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        isAlreadyGet = false;

        giftImage.transform.parent.gameObject.SetActive(true);      // ���ǳ�� ������Ʈ Ȱ��ȭ

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
