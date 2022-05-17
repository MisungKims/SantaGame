using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chimney : Obstacle
{
    public Image giftImage;
    public Gift gift;

    protected override void OnEnable()
    {
        base.OnEnable();

        gift = GiftManager.Instance.RandomGift();
        giftImage.sprite = gift.giftImage;
    }
}
