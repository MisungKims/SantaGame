/**
 * @brief 위시 리스트 슬롯
 * @author 김미성
 * @date 22-06-04
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WishListSlot : MonoBehaviour
{
    #region 변수
    // UI 변수
    [SerializeField]
    private Image giftImg;
    [SerializeField]
    private Text giftName;
    [SerializeField]
    private Text wishCount;

    public int index;       // 슬롯의 인덱스

    private Gift gift;      // 슬롯이 어떤 선물인지
    #endregion

    #region 유니티 함수
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
    #endregion
}
