using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WishListObj : MonoBehaviour
{
    [SerializeField]
    private Image giftImg;
    [SerializeField]
    private Text giftName;
    [SerializeField]
    private Text wishCount;

    public int index;

    private Gift gift;

    void Awake()
    {
        gift = GiftManager.Instance.giftList[index];

        giftImg.sprite = gift.giftImage;
        giftName.text = gift.giftName;
    }

    void OnEnable()
    {
        wishCount.text = gift.giftInfo.wishCount.ToString();
    }
}
