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

            if (postContent.Length > 17)    // 내용이 길다면 18자 까지만 보여줌
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
        // 편지를 처음 읽었을 때 notification 이미지의 active를 false로 변경
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
