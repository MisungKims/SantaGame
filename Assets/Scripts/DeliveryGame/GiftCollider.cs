/**
 * @brief ���� ���� ������ ������ ĸ�� �ݶ��̴�
 * @author ��̼�
 * @date 22-05-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftCollider : MonoBehaviour
{
    #region ����
    public bool isRemove = false;    // �κ��丮���� ���ŵǾ�����?

    // ĳ��
    private DeliveryGameManager deliveryGameManager;

    private Inventory inventory;

    private CapsuleCollider collider;
    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        deliveryGameManager = DeliveryGameManager.Instance;

        inventory = Inventory.Instance;

        collider = this.GetComponent<CapsuleCollider>();
    }

    private void OnEnable()
    {
        collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���� ��ó�� ������ ����(�������� ���� ���� ����)��
        // ��ó�� ������ ������ ������ �κ��丮���� ����

        if (other.gameObject.CompareTag("Chimney"))
        {
            Chimney chimney = other.GetComponent<Chimney>();
            Debug.Log(chimney.gift.giftName);

            inventory.RemoveItem2(chimney.gift);
            isRemove = true;

            deliveryGameManager.GiftCount = inventory.count;

            collider.enabled = false;
        }
    }
    #endregion
}
