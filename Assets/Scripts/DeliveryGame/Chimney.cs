/**
 * @brief 선물 전달 게임의 굴뚝
 * @author 김미성
 * @date 22-05-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chimney : Obstacle
{
    #region 변수
    public Image giftImage;
    public Gift gift;

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

        gift = inventory.RandomGet();      // 인벤토리에 있는 선물을 가져옴
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
