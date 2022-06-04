/**
 * @brief ���� ����Ʈ ����
 * @author ��̼�
 * @date 22-06-04
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WishListSlot : MonoBehaviour
{
    #region ����
    // UI ����
    [SerializeField]
    private Image giftImg;
    [SerializeField]
    private Text giftName;
    [SerializeField]
    private Text wishCount;

    public int index;       // ������ �ε���

    private Gift gift;      // ������ � ��������
    #endregion

    #region ����Ƽ �Լ�
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
