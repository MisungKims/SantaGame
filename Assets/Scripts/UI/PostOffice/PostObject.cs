/**
 * @details 편지의 내용을 가진 오브젝트
 * @author 김미성
 * @date 22-04-19
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostObject : MonoBehaviour
{
    #region 변수
    //UI 변수
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text contentPreviewText;
    [SerializeField]
    private GameObject notificationImage;   // 새로온 편지가 있음을 알려주는 이미지

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
                contentPreviewText.text = string.Format("{0}...", postContent.Substring(0, 18));    /// TODO : 포맷 말고 스트링 빌더로
            }
            else
            {
                contentPreviewText.text = postContent;
            }
        }
    }

    // 스크립트
    private WritingPad writingPad;

    // 그 외 변수
    private bool isRead;    // 읽은 편지이면 true
    public bool IsRead
    {
        set
        {
            isRead = value;
            notificationImage.SetActive(!isRead);
        }
    }

    #endregion

    #region 유니티 함수
    public void Awake()
    {
        if (isRead)
        {
            notificationImage.SetActive(false);
        }

        writingPad = PostOfficeManager.Instance.writingPad;
    }
    #endregion

    #region 함수
    /// <summary>
    /// 편지를 확인 (인스펙터에서 호출)
    /// </summary>
    public void Read()
    {
        if (!isRead)        // 편지를 처음 읽었을 때
            IsRead = true;

        writingPad.gameObject.SetActive(true);
        writingPad.PostName = postName;
        writingPad.PostConent = postContent;
    }
    #endregion
}
