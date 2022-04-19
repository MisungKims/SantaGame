/**
 * @details ������ ������ ���� ������Ʈ
 * @author ��̼�
 * @date 22-04-19
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostObject : MonoBehaviour
{
    #region ����
    //UI ����
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text contentPreviewText;
    [SerializeField]
    private GameObject notificationImage;   // ���ο� ������ ������ �˷��ִ� �̹���

    private string postName;
    public string PostName
    {
        set
        {
            postName = value;
            nameText.text = postName;
        }
    }

    private string postContent;
    public string PostConent
    {
        set
        {
            postContent = value;

            if (postContent.Length > 17)    // ������ ������ ��ٸ� 18�� ������ ������
            {
                contentPreviewText.text = string.Format("{0}...", postContent.Substring(0, 18));    /// TODO : ���� ���� ��Ʈ�� ������
            }
            else
            {
                contentPreviewText.text = postContent;
            }
        }
    }

    // ��ũ��Ʈ
    private WritingPad writingPad;

    // �� �� ����
    private bool isRead;    // ���� �����̸� true
    public bool IsRead
    {
        set
        {
            isRead = value;
            notificationImage.SetActive(!isRead);
        }
    }

    #endregion

    #region ����Ƽ �Լ�
    public void Awake()
    {
        if (isRead)
        {
            notificationImage.SetActive(false);
        }

        writingPad = PostOfficeManager.Instance.writingPad;
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// ������ Ȯ�� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void Read()
    {
        if (!isRead)        // ������ ó�� �о��� ��
            IsRead = true;

        writingPad.gameObject.SetActive(true);
        writingPad.PostName = postName;
        writingPad.PostConent = postContent;
    }
    #endregion
}
