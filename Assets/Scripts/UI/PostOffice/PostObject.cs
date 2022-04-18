/**
 * @details 편지의 내용을 가진 오브젝트
 * @author 김미성
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
    /// 새로온 편지가 있음을 알려주는 이미지
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

            if (postContent.Length > 17)    // 편지의 내용이 길다면 18자 까지만 보여줌
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
    /// 편지를 확인 (인스펙터에서 호출)
    /// </summary>
    public void Read()
    {
        
        if (!isRead)        // 편지를 처음 읽었을 때
        {
            notificationImage.SetActive(false);
            isRead = true;
        }

        writingPad.gameObject.SetActive(true);
        writingPad.PostName = postName;
        writingPad.PostConent = postContent;
    }
}
