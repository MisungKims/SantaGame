/**
 * @details 편지의 내용을 가진 오브젝트
 * @author 김미성
 * @date 22-04-27
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

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

    StringBuilder nameSb = new StringBuilder();

    private string postName;
    public string PostName
    {
        set
        {
            postName = value;

            //nameSb.Clear();
            //nameSb.Append("To. ");
            //nameSb.Append(postName);
            nameText.text = postName.ToString();
        }
    }

    StringBuilder postSb = new StringBuilder();

    private string postContent;
    public string PostConent
    {
        set
        {
            postContent = value;

            if (postContent.Length > 17)    // 편지의 내용이 길다면 18자 까지만 보여줌
            {
                postSb.Clear();
                postSb.Append(postContent.Substring(0, 18));
                postSb.Append("...");
                contentPreviewText.text = postSb.ToString();
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

    public int index;

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

    /// <summary>
    /// 편지를 버림 (인스펙터에서 호출)
    /// </summary>
    public void Discard()
    {
        PostOfficeManager.Instance.Refresh(index);
    }

    /// <summary>
    /// 해당 UI의 위치를 새로 고침
    /// </summary>
    /// <param name="vector">변경할 위치</param>
    public void RefreshTransform(Vector2 vector)
    {
        this.transform.GetComponent<RectTransform>().anchoredPosition = vector;
    }
    #endregion
}
