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

    private CapsuleCollider capsuleCollider;
    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        deliveryGameManager = DeliveryGameManager.Instance;

        inventory = Inventory.Instance;

        capsuleCollider = this.GetComponent<CapsuleCollider>();
    }

    private void OnEnable()
    {
        capsuleCollider.enabled = true;
        isRemove = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���� ��ó�� ������ ����(�������� ���� ���� ����)��
        // ��ó�� ������ ������ ������ �κ��丮���� ����

        if (other.gameObject.CompareTag("Chimney"))
        {
            Chimney chimney = other.GetComponent<Chimney>();

            inventory.RemoveItem(chimney.gift, false);
            isRemove = true;

            capsuleCollider.enabled = false;
        }
    }
    #endregion
}
