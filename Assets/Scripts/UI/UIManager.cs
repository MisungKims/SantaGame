/**
 * @brief UI�� ����
 * @author ��̼�
 * @date 22-04-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public GameObject snowPanel;            // �� �г�
    public GameObject cameraPanel;          // ī�޶� �г�
    public GameObject citizenPanel;         // �ֹ� �г�
    public GameObject storePanel;        // ���� �г�
    public PuzzleUI puzzlePanel;            // ���� �г�

    public GameObject clickObjWindow;       // Ŭ�� ������Ʈ â
    public GameObject InviteRabbitWindow;  // �䳢 �ʴ� â
    public QuestionWindow questionWindow;
    public GetRewardWindow getRewardWindow; // ���� ȹ�� â
   
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

    public void SetisOpenPanel(bool value)
    {
        isOpenPanel = value;
    }
    #endregion


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    //private void Start()
    //{
    //    SetNotificationImage();
    //}
}
