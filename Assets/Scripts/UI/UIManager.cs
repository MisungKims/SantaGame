/**
 * @brief UI를 관리
 * @author 김미성
 * @date 22-06-04
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
    public GameObject cameraPanel;               // 카메라 패널
    public GameObject storePanel;                // 상점 패널
    public GameObject giftShopPanel;             // 선물 가게 패널
    public GameObject inventoryPanel;            // 인벤토리 패널
    public GameObject postOfficePanel;           // 우체국 패널
    public GameObject clothesStoreObject;        // 옷가게 오브젝트
    public GameObject clothesStorePanel;         // 옷가게 패널
    public GetOfflineGoldWindow getOfflineGoldWindow;
    public PuzzleUI puzzlePanel;                // 퍼즐 패널
    public CitizenPanel citizenPanel;           // 주민 패널

    public GameObject clickObjWindow;           // 클릭 오브젝트 창
    public GameObject InviteRabbitWindow;       // 토끼 초대 창
    public GetRewardWindow getRewardWindow;     // 보상 획득 창


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

    // 캐싱
    SoundManager soundManager;
    CameraMovement cameraMovement;
    GameManager gameManager;
    Inventory inventory;
    #endregion

    #region 유니티 함수
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);


        soundManager = SoundManager.Instance;
        cameraMovement = CameraMovement.Instance;
        inventory = Inventory.Instance;
    }
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

    /// <summary>
    /// 상점 패널 보여줌
    /// </summary>
    public void ShowStorePanel()
    {
        SetisOpenPanel(true);

        storePanel.SetActive(true);

        if (InviteRabbitWindow.activeSelf) InviteRabbitWindow.SetActive(false);
        if (clickObjWindow.activeSelf) clickObjWindow.SetActive(false);
    }

    /// <summary>
    /// 상점 패널 숨김
    /// </summary>
    public void HideStorePanel()
    {
        SetisOpenPanel(false);

        storePanel.SetActive(false);
    }

    /// <summary>
    /// 옷가게 패널 보여줌
    /// </summary>
    public void ShowClothesStore()
    {
        SetisOpenPanel(true);
        clothesStoreObject.SetActive(true);
        clothesStorePanel.SetActive(true);
        mainPanel.SetActive(false);

        if (InviteRabbitWindow.activeSelf) InviteRabbitWindow.SetActive(false);
    }

    /// <summary>
    /// 옷가게 패널 숨김
    /// </summary>
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
        SetisOpenPanel(true);

        giftShopPanel.SetActive(true);

        if (InviteRabbitWindow.activeSelf) InviteRabbitWindow.SetActive(false);
        if (citizenPanel.gameObject.activeSelf) citizenPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// 선물 가게 패널을 숨김
    /// </summary>
    public void HideGiftShopPanel()
    {
        SetisOpenPanel(false);

        giftShopPanel.SetActive(false);
    }

    /// <summary>
    /// 인벤토리 패널을 보여줌
    /// </summary>
    public void ShowInventoryPanel()
    {
        SetisOpenPanel(true);
        inventoryPanel.SetActive(true);

        InventoryInstance().RefreshInventory();
    }

    public void StartDeliveryGame()
    {
        mainPanel.SetActive(false);
        alwaysVisiblePanel.SetActive(false);
       SetisOpenPanel(true);
    }

    public void EndDeliveryGame()
    {
        mainPanel.SetActive(true);
        alwaysVisiblePanel.SetActive(true);
        SetisOpenPanel(false);
    }

    public void SetisOpenPanel(bool value)
    {
        if (value)
        {
            CameraMovementInstance().canMove = false;
            soundManager.PlaySoundEffect(ESoundEffectType.uiButton);       // 효과음 실행
        }
        else
        {
            CameraMovementInstance().canMove = true;
        }
        
        isOpenPanel = value;
    }

    /// <summary>
    /// CameraMovement 인스턴스 반환
    /// </summary>
    /// <returns></returns>
    CameraMovement CameraMovementInstance()
    {
        if (!cameraMovement)
        {
            cameraMovement = CameraMovement.Instance;
        }

        return cameraMovement;
    }

    /// <summary>
    /// Inventory 인스턴스 반환
    /// </summary>
    /// <returns></returns>
    Inventory InventoryInstance()
    {
        if (!inventory)
        {
            inventory = Inventory.Instance;
        }

        return inventory;
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

    

}
