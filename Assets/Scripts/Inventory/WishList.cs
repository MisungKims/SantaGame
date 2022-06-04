/**
 * @brief ���� ����Ʈâ
 * @author ��̼�
 * @date 22-06-04
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WishList : MonoBehaviour
{
    #region ����
    // �̱���
    private static WishList instance;
    public static WishList Instance
    {
        get { return instance; }
    }

    [SerializeField]
    private WishListSlot[] wishListSlots;   // ���� �迭

    [SerializeField]
    private GameObject inventoryWindow;     // �κ��丮 â

    [SerializeField]
    private GameObject wishListWindow;      // ���ø���Ʈ â

    [SerializeField]
    private GameObject slide;               // �����̵� ��ư
    [SerializeField]
    private Text slideText;

    // ���ø���Ʈ �����̵��� �� �ʿ��� ����
    private Vector3 originInvenPos = new Vector3(0, -30, 0);
    private Vector3 slideInvenPos = new Vector3(-185, -30, 0);

    private Vector3 originListPos = new Vector3(648, 244, 0);
    private Vector3 slideListPos = new Vector3(817, 244, 0);

    private EMoveType moveType;         // ���ø���Ʈ�� ���� �ݴ� ����

    bool isOpen = false;

    // ĳ��
    private SoundManager soundManager;
    #endregion

    #region ����Ƽ �Լ�
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        for (int i = 0; i < wishListSlots.Length; i++)
        {
            wishListSlots[i].index = i;
        }

        soundManager = SoundManager.Instance;

        isOpen = false;
        slideText.text = ">";
        inventoryWindow.transform.localPosition = originInvenPos;
        slide.transform.localPosition = originListPos;

        moveType = EMoveType.none;
        StartCoroutine(SlideInventory());
        StartCoroutine(SlideWishList());
    }
    #endregion

    #region �ڷ�ƾ
    /// <summary>
    /// �κ��丮�� �����̵�
    /// </summary>
    /// <returns></returns>
    IEnumerator SlideInventory()
    {
        while (true)
        {
            // �κ��丮�� ��������
            if (moveType.Equals(EMoveType.opening))
            {
                while (Vector3.Distance(inventoryWindow.transform.localPosition, slideInvenPos) > 0.05f && moveType.Equals(EMoveType.opening))
                {
                    inventoryWindow.transform.localPosition = Vector3.Lerp(inventoryWindow.transform.localPosition, slideInvenPos, Time.deltaTime * 2.5f);

                    yield return null;
                }

                if (moveType.Equals(EMoveType.opening))
                {
                    inventoryWindow.transform.localPosition = slideInvenPos;
                }
            }
            // �κ��丮�� ����������
            else if (moveType.Equals(EMoveType.closing))
            {
                while (Vector3.Distance(inventoryWindow.transform.localPosition, originInvenPos) > 0.05f && moveType.Equals(EMoveType.closing))
                {
                    inventoryWindow.transform.localPosition = Vector3.Lerp(inventoryWindow.transform.localPosition, originInvenPos, Time.deltaTime * 2.5f);

                    yield return null;
                }

                if (moveType.Equals(EMoveType.closing))
                {
                    inventoryWindow.transform.localPosition = originInvenPos;
                }
            }

            yield return null;
        }
    }
    /// <summary>
    /// ���ø���Ʈ�� �����̵�
    /// </summary>
    /// <returns></returns>
    IEnumerator SlideWishList()
    {
        while (true)
        {
            // ���ø���Ʈ�� ��
            if (moveType.Equals(EMoveType.opening))
            {
                wishListWindow.SetActive(true);

                while (Vector3.Distance(slide.transform.localPosition, slideListPos) > 0.05f && moveType.Equals(EMoveType.opening))
                {
                    slide.transform.localPosition = Vector3.Lerp(slide.transform.localPosition, slideListPos, Time.deltaTime * 2.5f);

                    yield return null;
                }

                if (moveType.Equals(EMoveType.opening))
                {
                    slide.transform.localPosition = slideListPos;
                    moveType = EMoveType.none;
                }
            }
            // ���ø���Ʈ�� ����
            else if (moveType.Equals(EMoveType.closing))
            {
                while (Vector3.Distance(slide.transform.localPosition, originListPos) > 0.05f && !isOpen)
                {
                    slide.transform.localPosition = Vector3.Lerp(slide.transform.localPosition, originListPos, Time.deltaTime * 2.5f);

                    yield return null;
                }

                if (moveType.Equals(EMoveType.closing))
                {
                    slide.transform.localPosition = originListPos;
                    wishListWindow.SetActive(false);
                    moveType = EMoveType.none;
                }
            }

            yield return null;
        }
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// ���� ����Ʈ�� �����ϰų� �ݱ� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void OpenWishList()
    {
        soundManager.PlaySoundEffect(ESoundEffectType.uiButton);

        if (isOpen)
        {
            isOpen = false;
            slideText.text = ">";

            moveType = EMoveType.closing;
        }
        else
        {
            isOpen = true;
            slideText.text = "<";

            moveType = EMoveType.opening;
        }
    }
    #endregion
}
