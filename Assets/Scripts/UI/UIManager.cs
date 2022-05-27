/**
 * @brief UI�� ����
 * @author ��̼�
 * @date 22-05-01
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    #region ����
    private static UIManager instance;
    public static UIManager Instance
    {
        get { return instance; }
    }

    public bool isOpenPanel;        // �г� Ȥ�� â�� �����ִ���

    [Header("---------- UI ����")]
    public GameObject mainPanel;
    public GameObject alwaysVisiblePanel;
    public GameObject cameraPanel;          // ī�޶� �г�
    public GameObject citizenPanel;         // �ֹ� �г�
    public GameObject storePanel;        // ���� �г�
    public GameObject giftShopPanel;        // ���� ���� �г�
    public GameObject inventoryPanel;       // �κ��丮 �г�
    public GameObject postOfficePanel;       // ��ü�� �г�
    public PuzzleUI puzzlePanel;            // ���� �г�
    
    public GameObject clickObjWindow;       // Ŭ�� ������Ʈ â
    public GameObject InviteRabbitWindow;  // �䳢 �ʴ� â
    public QuestionWindow questionWindow;
    public GetRewardWindow getRewardWindow; // ���� ȹ�� â

    

    // public GameObject panel;        // ���� �� ���� �Ѿ��ϴ� â���� ���� ������Ʈ

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

    //[Header("---------- �κ��丮 UI ����")]
    //public Slot[] slots;        // �κ��丮 UI ����

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
    public void ShowCitizenPanel()
    {
        mainPanel.SetActive(false);
        citizenPanel.SetActive(true);

        if (InviteRabbitWindow.activeSelf) InviteRabbitWindow.SetActive(false);
    }

    /// <summary>
    /// �ֹ� �г��� ����
    /// </summary>
    public void HideCitizenPanel()
    {
        mainPanel.SetActive(true);
        citizenPanel.SetActive(false);
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
        if (citizenPanel.activeSelf) citizenPanel.SetActive(false);
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
    /// ���� ���� �г��� ������
    /// </summary>
    public void ShowGiftShopPanel()
    {
        SetisOpenPanel(true);

        giftShopPanel.SetActive(true);
        //mainPanel.SetActive(false);
        //alwaysVisiblePanel.SetActive(false);

        if (InviteRabbitWindow.activeSelf) InviteRabbitWindow.SetActive(false);
        if (citizenPanel.activeSelf) citizenPanel.SetActive(false);
    }

    /// <summary>
    /// ���� ���� �г��� ����
    /// </summary>
    public void HideGiftShopPanel()
    {
        SetisOpenPanel(false);

        giftShopPanel.SetActive(false);
        //mainPanel.SetActive(true);
        //alwaysVisiblePanel.SetActive(true);
    }

    /// <summary>
    /// �κ��丮 �г��� ������
    /// </summary>
    public void ShowInventoryPanel()
    {
        SetisOpenPanel(true);
        inventoryPanel.SetActive(true);

        Inventory.Instance.RefreshInventory();

    }

    public void SetisOpenPanel(bool value)
    {
        if (value)
        {
            SoundManager.Instance.PlaySoundEffect(ESoundEffectType.uiButton);       // ȿ���� ����
        }
        
        isOpenPanel = value;
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

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }
}
