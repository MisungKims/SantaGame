using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WishList : MonoBehaviour
{
    // ½Ì±ÛÅæ
    private static WishList instance;
    public static WishList Instance
    {
        get { return instance; }
    }

    [SerializeField]
    private WishListObj[] wishListObjs;

    [SerializeField]
    private GameObject inventoryWindow;

    [SerializeField]
    private GameObject wishListWindow;

    [SerializeField]
    private GameObject slide;

    private Vector3 originInvenPos = new Vector3(0, -30, 0);
    private Vector3 slideInvenPos = new Vector3(-185, -30, 0);

    private Vector3 originListPos = new Vector3(648, 244, 0);
    private Vector3 slideListPos = new Vector3(817, 244, 0);

    [SerializeField]
    private Text slideText;

    bool isOpen = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        for (int i = 0; i < wishListObjs.Length; i++)
        {
            wishListObjs[i].index = i;
        }
    }

    void OnEnable()
    {
        if (isOpen)
        {
            isOpen = false;
            slideText.text = ">";

            inventoryWindow.transform.localPosition = originInvenPos;
            slide.transform.localPosition = originListPos;

            wishListWindow.SetActive(false);
        }
    }

    /// <summary>
    /// À§½Ã ¸®½ºÆ®¸¦ ¿ÀÇÂÇÏ°Å³ª ´Ý±â (ÀÎ½ºÆåÅÍ¿¡¼­ È£Ãâ)
    /// </summary>
    public void OpenWishList()
    {
        SoundManager.Instance.PlaySoundEffect(ESoundEffectType.uiButton);

        if (isOpen)
        {
            isOpen = false;
            slideText.text = ">";

            StartCoroutine(EndSlideInventory());
            StartCoroutine(EndSlideWishList());
        }
        else
        {
            isOpen = true;
            slideText.text = "<";

            StartCoroutine(StartSlideInventory());
            StartCoroutine(StartSlideWishList());
        }
    }

    IEnumerator StartSlideInventory()
    {
        while (Vector3.Distance(inventoryWindow.transform.localPosition, slideInvenPos) > 0.05f && isOpen)
        {
            inventoryWindow.transform.localPosition = Vector3.Lerp(inventoryWindow.transform.localPosition, slideInvenPos, Time.deltaTime * 2.5f);

            yield return null;
        }

        if(isOpen) inventoryWindow.transform.localPosition = slideInvenPos;
    }

    IEnumerator StartSlideWishList()
    {
        wishListWindow.SetActive(true);

        while (Vector3.Distance(slide.transform.localPosition, slideListPos) > 0.05f && isOpen)
        {
            slide.transform.localPosition = Vector3.Lerp(slide.transform.localPosition, slideListPos, Time.deltaTime * 2.5f);

            yield return null;
        }

        if (isOpen) slide.transform.localPosition = slideListPos;
    }

    IEnumerator EndSlideInventory()
    {
       
        while (Vector3.Distance(inventoryWindow.transform.localPosition, originInvenPos) > 0.05f && !isOpen)
        {
            inventoryWindow.transform.localPosition = Vector3.Lerp(inventoryWindow.transform.localPosition, originInvenPos, Time.deltaTime * 2.5f);

            yield return null;
        }

        if (!isOpen) inventoryWindow.transform.localPosition = originInvenPos;
    }

    IEnumerator EndSlideWishList()
    {
        while (Vector3.Distance(slide.transform.localPosition, originListPos) > 0.05f && !isOpen)
        {
            slide.transform.localPosition = Vector3.Lerp(slide.transform.localPosition, originListPos, Time.deltaTime * 2.5f);

            yield return null;
        }

        if (!isOpen)
        {
            slide.transform.localPosition = originListPos;
            wishListWindow.SetActive(false);
        }

    }
}
