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

    [Header("---------- 패널")]
    public GameObject mainPanel;
    public GameObject questionWindow;
    public GameObject clickObjWindow;

    public GameObject store;

    [Header("---------- 출석 보상")]
    [SerializeField]
    private GameObject attendanceNotificationImage;     // 출석 보상 알림 이미지      

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

    /// <summary>
    /// 
    /// </summary>
    public void ShowQuestionWindow()
    {
        if (!clickObjWindow.activeSelf)
        {
            questionWindow.SetActive(true);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void HideQuestionWindow()
    {
        questionWindow.SetActive(false);
    }


    /// <summary>
    /// 받을 출석 보상이 있을 때 알려주는 이미지
    /// </summary>
    public void SetNotificationImage()
    {
        // 마지막 출석 보상 수령 날짜가 오늘 날짜와 다르면 현재 받을 출석 보상이 있으므로,
        if (GameManager.Instance.getAttendanceRewardDate != DateTime.Now.ToString("yyyy.MM.dd"))
        {
            attendanceNotificationImage.SetActive(true);        // UI로 알려줌
        }
    }

    /// <summary>
    /// 출석 보상 알림 이미지를 숨김
    /// </summary>
    public void HideAttendanceNotiImage()
    {
        attendanceNotificationImage.SetActive(false);
    }



    #endregion

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        SetNotificationImage();
    }
}
