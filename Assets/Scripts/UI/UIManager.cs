/**
 * @brief UI를 관리
 * @author 김미성
 * @date 22-05-01
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    #region 변수
    public Text text;

    private static UIManager instance;
    public static UIManager Instance
    {
        get { return instance; }
    }

    public bool isOpenPanel;        // 패널 혹은 창이 열려있는지

    [Header("---------- UI 변수")]
    public GameObject mainPanel;
    public GameObject alwaysVisiblePanel;
    public GameObject cameraPanel;          // 카메라 패널
    public GameObject storePanel;        // 상점 패널
    public GameObject giftShopPanel;        // 선물 가게 패널
    public GameObject inventoryPanel;       // 인벤토리 패널
    public GameObject postOfficePanel;       // 우체국 패널
    public GameObject clothesStoreObject;       // 옷가게 오브젝트
    public GameObject clothesStorePanel;         // 옷가게 패널
    public GetOfflineGoldWindow getOfflineGoldWindow;
    public PuzzleUI puzzlePanel;            // 퍼즐 패널
    public CitizenPanel citizenPanel;         // 주민 패널

    public GameObject clickObjWindow;       // 클릭 오브젝트 창
    public GameObject InviteRabbitWindow;  // 토끼 초대 창
    public QuestionWindow questionWindow;
    public GetRewardWindow getRewardWindow; // 보상 획득 창



    

    // public GameObject panel;        // 시작 전 껐다 켜야하는 창들을 담은 오브젝트

    [Header("---------- 게임 매니저 UI 변수")]
    public Slider gaugeSlider;
    public Text gaugeText;
    public Text lvText;
    public Text goldText;
    public Text carrotsText;
    public Text diaText;
    public Text citizenCountText;
    public Text dateText;
    public GameObject gaugeBellImage;

    //[Header("---------- 인벤토리 UI 변수")]
    //public Slot[] slots;        // 인벤토리 UI 슬롯

    #endregion

    #region 함수
    /// <summary>
    /// 오브젝트 정보 창 보여줌
    /// </summary>
    public void ShowClickObjWindow()
    {
        if (!clickObjWindow.activeSelf)
        {
            clickObjWindow.SetActive(true);

            if (storePanel.activeSelf) storePanel.SetActive(false);
            if (InviteRabbitWindow.activeSelf) InviteRabbitWindow.SetActive(false);
        }
    }

    /// <summary>
    /// 오브젝트 정보 창 숨김
    /// </summary>
    public void HideClickObjWindow()
    {
        clickObjWindow.SetActive(false);
    }

    /// <summary>
    /// 주민 패널 보여줌
    /// </summary>
    public void ShowCitizenPanel(RabbitCitizen rabbitCitizen)
    {
        mainPanel.SetActive(false);
        citizenPanel.rabbitCitizen = rabbitCitizen;
        citizenPanel.gameObject.SetActive(true);

        if (InviteRabbitWindow.activeSelf) InviteRabbitWindow.SetActive(false);
    }

    /// <summary>
    /// 주민 패널을 숨김
    /// </summary>
    public void HideCitizenPanel()
    {
        mainPanel.SetActive(true);
        citizenPanel.rabbitCitizen = null;
        citizenPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// 카메라 패널을 보여줌
    /// </summary>
    public void ShowCameraPanel()
    {
        cameraPanel.SetActive(true);
        mainPanel.SetActive(false);
        alwaysVisiblePanel.SetActive(false);

        if (InviteRabbitWindow.activeSelf) InviteRabbitWindow.SetActive(false);
        if (citizenPanel.gameObject.activeSelf) citizenPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// 카메라 패널을 숨김
    /// </summary>
    public void HideCameraPanel()
    {
        cameraPanel.SetActive(false);
        mainPanel.SetActive(true);
        alwaysVisiblePanel.SetActive(true);
    }

    public void ShowStorePanel()
    {
        SetisOpenPanel(true);

        storePanel.SetActive(true);

        if (InviteRabbitWindow.activeSelf) InviteRabbitWindow.SetActive(false);
        if (clickObjWindow.activeSelf) clickObjWindow.SetActive(false);
    }

    public void HideStorePanel()
    {
        SetisOpenPanel(false);

        storePanel.SetActive(false);
    }

    public void ShowClothesStore()
    {
        SetisOpenPanel(true);
        clothesStoreObject.SetActive(true);
        clothesStorePanel.SetActive(true);
        mainPanel.SetActive(false);

        if (InviteRabbitWindow.activeSelf) InviteRabbitWindow.SetActive(false);
    }

    public void HideClothesStore()
    {
        SetisOpenPanel(false);
        clothesStoreObject.SetActive(false);
        clothesStorePanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    /// <summary>
    /// 선물 가게 패널을 보여줌
    /// </summary>
    public void ShowGiftShopPanel()
    {
        CameraMovement.Instance.canMove = false;

        SetisOpenPanel(true);

        giftShopPanel.SetActive(true);
        //mainPanel.SetActive(false);
        //alwaysVisiblePanel.SetActive(false);

        if (InviteRabbitWindow.activeSelf) InviteRabbitWindow.SetActive(false);
        if (citizenPanel.gameObject.activeSelf) citizenPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// 선물 가게 패널을 숨김
    /// </summary>
    public void HideGiftShopPanel()
    {
        CameraMovement.Instance.canMove = true;

        SetisOpenPanel(false);

        giftShopPanel.SetActive(false);
        //mainPanel.SetActive(true);
        //alwaysVisiblePanel.SetActive(true);
    }

    /// <summary>
    /// 인벤토리 패널을 보여줌
    /// </summary>
    public void ShowInventoryPanel()
    {
        CameraMovement.Instance.canMove = false;

        SetisOpenPanel(true);
        inventoryPanel.SetActive(true);

        Inventory.Instance.RefreshInventory();
    }

    public void SetisOpenPanel(bool value)
    {
        if (value)
        {
            CameraMovement.Instance.canMove = false;
            SoundManager.Instance.PlaySoundEffect(ESoundEffectType.uiButton);       // 효과음 실행
        }
        else
        {
            CameraMovement.Instance.canMove = true;
        }
        
        isOpenPanel = value;
    }


 /////////////////// 테스트
    public void TestNewItem()
    {
        Inventory.Instance.AddItem(GiftManager.Instance.giftList[2]);
    }

    public void TestNewItem2()
    {
        Inventory.Instance.AddItem(GiftManager.Instance.giftList[13]);
    }

    public void TestNewItem3()
    {
        Inventory.Instance.AddItem(GiftManager.Instance.giftList[3]);
    }

    public void TestRemoveItem()
    {
        Inventory.Instance.RemoveItem(GiftManager.Instance.giftList[13], true);
    }
    #endregion

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }
}
