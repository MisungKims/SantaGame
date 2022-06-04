/**
 * @brief 선물 전달 게임의 굴뚝
 * @author 김미성
 * @date 22-05-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chimney : DeliveryGameObject
{
    #region 변수
    public Image giftImage;
    public Gift gift;

    public bool isAlreadyGet;       // 선물을 받은 굴뚝인지?

    // 캐싱
    private DeliveryGameManager deliveryGameManager;
    private Inventory inventory;
    #endregion

    #region 유니티 함수
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

        gift = inventory.RandomGet();      // 인벤토리에 있는 선물을 랜덤으로 가져옴
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
