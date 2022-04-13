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

            if (postContent.Length > 17)    // ������ ��ٸ� 18�� ������ ������
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

        writingPad = PostOffice.instance.writingPad;
    }

    public void Read()
    {
        // ������ ó�� �о��� �� notification �̹����� active�� false�� ����
        if (!isRead)
        {
            notificationImage.SetActive(false);
            isRead = true;
        }

        writingPad.gameObject.SetActive(true);
        writingPad.PostName = postName;
        writingPad.PostConent = postContent;
    }
}
