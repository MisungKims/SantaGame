/**
 * @brief 위시 리스트창
 * @author 김미성
 * @date 22-06-04
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WishList : MonoBehaviour
{
    #region 변수
    // 싱글톤
    private static WishList instance;
    public static WishList Instance
    {
        get { return instance; }
    }

    [SerializeField]
    private WishListSlot[] wishListSlots;   // 슬롯 배열

    [SerializeField]
    private GameObject inventoryWindow;     // 인벤토리 창

    [SerializeField]
    private GameObject wishListWindow;      // 위시리스트 창

    [SerializeField]
    private GameObject slide;               // 슬라이드 버튼
    [SerializeField]
    private Text slideText;

    // 위시리스트 슬라이드할 때 필요한 변수
    private Vector3 originInvenPos = new Vector3(0, -30, 0);
    private Vector3 slideInvenPos = new Vector3(-185, -30, 0);

    private Vector3 originListPos = new Vector3(648, 244, 0);
    private Vector3 slideListPos = new Vector3(817, 244, 0);

    private EMoveType moveType;         // 위시리스트를 열고 닫는 상태

    bool isOpen = false;

    // 캐싱
    private SoundManager soundManager;
    #endregion

    #region 유니티 함수
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

    #region 코루틴
    /// <summary>
    /// 인벤토리를 슬라이드
    /// </summary>
    /// <returns></returns>
    IEnumerator SlideInventory()
    {
        while (true)
        {
            // 인벤토리를 왼쪽으로
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
            // 인벤토리를 오른쪽으로
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
    /// 위시리스트를 슬라이드
    /// </summary>
    /// <returns></returns>
    IEnumerator SlideWishList()
    {
        while (true)
        {
            // 위시리스트를 엶
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
            // 위시리스트를 닫음
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

    #region 함수
    /// <summary>
    /// 위시 리스트를 오픈하거나 닫기 (인스펙터에서 호출)
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
