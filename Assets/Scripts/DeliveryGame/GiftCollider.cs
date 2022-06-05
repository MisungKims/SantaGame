/**
 * @brief 선물 전달 게임의 선물의 캡슐 콜라이더
 * @author 김미성
 * @date 22-05-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftCollider : MonoBehaviour
{
    #region 변수
    public bool isRemove = false;    // 인벤토리에서 제거되었는지?

    // 캐싱
    private DeliveryGameManager deliveryGameManager;

    private Inventory inventory;

    private CapsuleCollider capsuleCollider;
    #endregion

    #region 유니티 함수
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
        // 굴뚝 근처에 떨어진 선물(굴뚝으로 들어가지 못한 선물)은
        // 근처의 굴뚝의 선물을 가져와 인벤토리에서 제거

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
