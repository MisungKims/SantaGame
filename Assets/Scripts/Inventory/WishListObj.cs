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

    void Start()
    {
        gift = GiftManager.Instance.giftList[index];

        giftImg.sprite = gift.giftImage;
        giftName.text = gift.giftName;
        wishCount.text = gift.wishCount.ToString();
    }

    void Update()
    {
        wishCount.text = gift.wishCount.ToString();
    }

}
