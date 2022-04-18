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

    [Header("---------- �г�")]
    public GameObject mainPanel;
    public GameObject questionWindow;
    public GameObject clickObjWindow;

    public GameObject store;

    [Header("---------- �⼮ ����")]
    [SerializeField]
    private GameObject attendanceNotificationImage;     // �⼮ ���� �˸� �̹���      

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

            if (store.activeSelf) store.SetActive(false);
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
    /// ���� �⼮ ������ ���� �� �˷��ִ� �̹���
    /// </summary>
    public void SetNotificationImage()
    {
        // ������ �⼮ ���� ���� ��¥�� ���� ��¥�� �ٸ��� ���� ���� �⼮ ������ �����Ƿ�,
        if (GameManager.Instance.getAttendanceRewardDate != DateTime.Now.ToString("yyyy.MM.dd"))
        {
            attendanceNotificationImage.SetActive(true);        // UI�� �˷���
        }
    }

    /// <summary>
    /// �⼮ ���� �˸� �̹����� ����
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
