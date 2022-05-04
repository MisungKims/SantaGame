/**
 * @brief UI를 관리
 * @author 김미성
 * @date 22-05-01
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
  //  public GameObject snowPanel;            // 눈 패널
 //   public RainPuzzle rainPuzzle;
    public GameObject cameraPanel;          // 카메라 패널
    public GameObject citizenPanel;         // 주민 패널
    public GameObject storePanel;        // 상점 패널
    public PuzzleUI puzzlePanel;            // 퍼즐 패널

    public GameObject clickObjWindow;       // 클릭 오브젝트 창
    public GameObject InviteRabbitWindow;  // 토끼 초대 창
    public QuestionWindow questionWindow;
    public GetRewardWindow getRewardWindow; // 보상 획득 창

   // public GameObject panel;        // 시작 전 껐다 켜야하는 창들을 담은 오브젝트

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
        Inventory.Instance.RemoveItem(GiftManager.Instance.giftList[13]);
    }
    #endregion



    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    //private IEnumerator Start()
    //{
        

    //    yield return null;

    //    // 켰다가 다시 끄기
    //    for (int i = 0; i < panel.transform.childCount; i++)
    //    {
    //        panel.transform.GetChild(i).gameObject.SetActive(true);
    //        panel.transform.GetChild(i).gameObject.SetActive(false);
    //    }
    //}

    /// <summary>
    /// 눈 내리는 패널 보이는 타이머
    /// </summary>
    /// <returns></returns>
    //private IEnumerator SnowTimer()
    //{
    //    yield return new WaitForSeconds(30f);

    //    snowPanel.SetActive(true);

    //    yield return StartCoroutine(rainPuzzle.ClickPuzzleCoru());      // 퍼즐이 클릭될 때까지 대기
    //    yield return new WaitForSeconds(1f);

    //    snowPanel.SetActive(false);
    //}
}
