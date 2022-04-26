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

    public bool isOpenPanel;

    [Header("---------- UI ����")]
    public GameObject mainPanel;
    public QuestionWindow questionWindow;
    public GameObject clickObjWindow;
    public GetRewardWindow getRewardWindow;
    public PuzzleUI puzzlePanel;
    public GameObject snowPanel;
    public GameObject citizenPanel;

    public GameObject store;

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
