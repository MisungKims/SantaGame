/**
 * @details ������ ������ ���� ������Ʈ
 * @author ��̼�
 * @date 22-04-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PostObject : MonoBehaviour
{
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text contentPreviewText;

    /// <summary>
    /// ���ο� ������ ������ �˷��ִ� �̹���
    /// </summary>
    [SerializeField]
    private GameObject notificationImage;

    private WritingPad writingPad;

    private bool isRead;

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
                contentPreviewText.text = string.Format("{0}...", postContent.Substring(0, 18));
            }
            else
            {
                contentPreviewText.text = postContent;
            }
        }
    }

    public void Awake()
    {
        if (isRead)
        {
            notificationImage.SetActive(false);
        }

        writingPad = PostOffice.Instance.writingPad;
    }

    /// <summary>
    /// ������ Ȯ�� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void Read()
    {
        
        if (!isRead)        // ������ ó�� �о��� ��
        {
            notificationImage.SetActive(false);
            isRead = true;
        }

        writingPad.gameObject.SetActive(true);
        writingPad.PostName = postName;
        writingPad.PostConent = postContent;
    }
}
