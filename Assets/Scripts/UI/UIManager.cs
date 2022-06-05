/**
 * @brief UI�� ����
 * @author ��̼�
 * @date 22-06-04
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    #region ����
    public Text text;

    private static UIManager instance;
    public static UIManager Instance
    {
        get { return instance; }
    }

    public bool isOpenPanel;        // �г� Ȥ�� â�� �����ִ���

    [Header("---------- UI ����")]
    public GameObject mainPanel;
    public GameObject alwaysVisiblePanel;
    public GameObject cameraPanel;               // ī�޶� �г�
    public GameObject storePanel;                // ���� �г�
    public GameObject giftShopPanel;             // ���� ���� �г�
    public GameObject inventoryPanel;            // �κ��丮 �г�
    public GameObject postOfficePanel;           // ��ü�� �г�
    public GameObject clothesStoreObject;        // �ʰ��� ������Ʈ
    public GameObject clothesStorePanel;         // �ʰ��� �г�
    public GetOfflineGoldWindow getOfflineGoldWindow;
    public PuzzleUI puzzlePanel;                // ���� �г�
    public CitizenPanel citizenPanel;           // �ֹ� �г�

    public GameObject clickObjWindow;           // Ŭ�� ������Ʈ â
    public GameObject InviteRabbitWindow;       // �䳢 �ʴ� â
    public GetRewardWindow getRewardWindow;     // ���� ȹ�� â


    [Header("---------- ���� �Ŵ��� UI ����")]
    public Slider gaugeSlider;
    public Text gaugeText;
    public Text lvText;
    public Text goldText;
    public Text carrotsText;
    public Text diaText;
    public Text citizenCountText;
    public Text dateText;
    public GameObject gaugeBellImage;

    // ĳ��
    SoundManager soundManager;
    CameraMovement cameraMovement;
    GameManager gameManager;
    Inventory inventory;
    #endregion

    #region ����Ƽ �Լ�
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

    #region �Լ�
    

    /// <summary>
    /// ������Ʈ ���� â ������
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
    /// ������Ʈ ���� â ����
    /// </summary>
    public void HideClickObjWindow()
    {
        clickObjWindow.SetActive(false);
    }

    /// <summary>
    /// �ֹ� �г� ������
    /// </summary>
    public void ShowCitizenPanel(RabbitCitizen rabbitCitizen)
    {
        mainPanel.SetActive(false);
        citizenPanel.rabbitCitizen = rabbitCitizen;
        citizenPanel.gameObject.SetActive(true);

        if (InviteRabbitWindow.activeSelf) InviteRabbitWindow.SetActive(false);
    }

    /// <summary>
    /// �ֹ� �г��� ����
    /// </summary>
    public void HideCitizenPanel()
    {
        mainPanel.SetActive(true);
        citizenPanel.rabbitCitizen = null;
        citizenPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// ī�޶� �г��� ������
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
    /// ī�޶� �г��� ����
    /// </summary>
    public void HideCameraPanel()
    {
        cameraPanel.SetActive(false);
        mainPanel.SetActive(true);
        alwaysVisiblePanel.SetActive(true);
    }

    /// <summary>
    /// ���� �г� ������
    /// </summary>
    public void ShowStorePanel()
    {
        SetisOpenPanel(true);

        storePanel.SetActive(true);

        if (InviteRabbitWindow.activeSelf) InviteRabbitWindow.SetActive(false);
        if (clickObjWindow.activeSelf) clickObjWindow.SetActive(false);
    }

    /// <summary>
    /// ���� �г� ����
    /// </summary>
    public void HideStorePanel()
    {
        SetisOpenPanel(false);

        storePanel.SetActive(false);
    }

    /// <summary>
    /// �ʰ��� �г� ������
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
    /// �ʰ��� �г� ����
    /// </summary>
    public void HideClothesStore()
    {
        SetisOpenPanel(false);
        clothesStoreObject.SetActive(false);
        clothesStorePanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    /// <summary>
    /// ���� ���� �г��� ������
    /// </summary>
    public void ShowGiftShopPanel()
    {
        SetisOpenPanel(true);

        giftShopPanel.SetActive(true);

        if (InviteRabbitWindow.activeSelf) InviteRabbitWindow.SetActive(false);
        if (citizenPanel.gameObject.activeSelf) citizenPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// ���� ���� �г��� ����
    /// </summary>
    public void HideGiftShopPanel()
    {
        SetisOpenPanel(false);

        giftShopPanel.SetActive(false);
    }

    /// <summary>
    /// �κ��丮 �г��� ������
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
            soundManager.PlaySoundEffect(ESoundEffectType.uiButton);       // ȿ���� ����
        }
        else
        {
            CameraMovementInstance().canMove = true;
        }
        
        isOpenPanel = value;
    }

    /// <summary>
    /// CameraMovement �ν��Ͻ� ��ȯ
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
    /// Inventory �ν��Ͻ� ��ȯ
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

    /////////////////// �׽�Ʈ
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
