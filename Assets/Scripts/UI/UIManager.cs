/**
 * @brief UI를 관리
 * @author 김미성
 * @date 22-04-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager : MonoBehaviour
{
    #region 변수
    private static UIManager instance;
    public static UIManager Instance
    {
        get { return instance; }
    }

    public bool isOpenPanel;        // 패널 혹은 창이 열려있는지

    [Header("---------- UI 변수")]
    public GameObject mainPanel;
    public GameObject alwaysVisiblePanel;
    public GameObject snowPanel;            // 눈 패널
    public GameObject cameraPanel;          // 카메라 패널
    public GameObject citizenPanel;         // 주민 패널
    public GameObject storePanel;        // 상점 패널
    public PuzzleUI puzzlePanel;            // 퍼즐 패널

    public GameObject clickObjWindow;       // 클릭 오브젝트 창
    public GameObject InviteRabbitWindow;  // 토끼 초대 창
    public QuestionWindow questionWindow;
    public GetRewardWindow getRewardWindow; // 보상 획득 창
   
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
    public void ShowCitizenPanel()
    {
        mainPanel.SetActive(false);
        citizenPanel.SetActive(true);

        if (InviteRabbitWindow.activeSelf) InviteRabbitWindow.SetActive(false);
    }

    /// <summary>
    /// 주민 패널을 숨김
    /// </summary>
    public void HideCitizenPanel()
    {
        mainPanel.SetActive(true);
        citizenPanel.SetActive(false);
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
        if (citizenPanel.activeSelf) citizenPanel.SetActive(false);
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
