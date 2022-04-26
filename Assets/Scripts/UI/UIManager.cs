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

    public bool isOpenPanel;

    [Header("---------- UI 변수")]
    public GameObject mainPanel;
    public QuestionWindow questionWindow;
    public GameObject clickObjWindow;
    public GetRewardWindow getRewardWindow;
    public PuzzleUI puzzlePanel;
    public GameObject snowPanel;
    public GameObject citizenPanel;

    public GameObject store;

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

            if (store.activeSelf) store.SetActive(false);
        }
    }

    /// <summary>
    /// 오브젝트 정보 창 숨김
    /// </summary>
    public void HideClickObjWindow()
    {
        clickObjWindow.SetActive(false);
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
