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

    public GameObject notificationImage;   // 새로온 편지가 있음을 알려주는 이미지

    private string postName;
    public string PostName
    {
        set
        {
            postName = value;

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


    private WritingPad writingPad;      // 편지지 오브젝트

    public int index;           // 편지 목록창에서의 인덱스

    public Gift gift;

    private int questID = 4;

    // 캐싱
    GameManager gameManager;
    PostOfficeManager postOfficeManager;
    QuestManager questManager;
    #endregion

    #region 유니티 함수
    public void Awake()
    {
        gameManager = GameManager.Instance;

        postOfficeManager = PostOfficeManager.Instance;
        writingPad = postOfficeManager.writingPad;

        questManager = QuestManager.Instance;
    }
    #endregion

    #region 함수
    /// <summary>
    /// 알림 이미지 새로고침
    /// </summary>
    public void RefreshNotification()
    {
        if (PostOfficeManagerInstance().havePostList.Count > index)
        {
            // 읽은 편지는 알림 이미지를 보이지 않도록
            if (PostOfficeManagerInstance().havePostList[index].isRead)
            {
                notificationImage.SetActive(false);         
            }
            else
            {
                notificationImage.SetActive(true);
            }
        }
    }

    /// <summary>
    /// 편지를 확인 (인스펙터에서 호출)
    /// </summary>
    public void Read()
    {
        if (!postOfficeManager.havePostList[index].isRead)        // 편지를 처음 읽었을 때
        {
            gift.giftInfo.wishCount++;        // 선물을 위시리스트에 추가

            questManager.Success(questID);        // 퀘스트 성공

            gameManager.IncreaseGauge(5);      // 게이지 증가

            postOfficeManager.havePostList[index].isRead = true;

            notificationImage.SetActive(false);

            writingPad.canMove = true;
            writingPad.giftSprite = gift.giftImage;
        }
        else
        {
            writingPad.canMove = false;
        }

        writingPad.PostName = postName;
        writingPad.PostConent = postContent;
        writingPad.gameObject.SetActive(true);
    }

    /// <summary>
    /// 편지를 버림 (인스펙터에서 호출)
    /// </summary>
    public void Discard()
    {
        postOfficeManager.Refresh(index);
    }

    /// <summary>
    /// 해당 UI의 위치를 새로 고침
    /// </summary>
    /// <param name="vector">변경할 위치</param>
    public void RefreshTransform(Vector2 vector)
    {
        this.transform.GetComponent<RectTransform>().anchoredPosition = vector;
    }

    /// <summary>
    /// PostOfficeManager 인스턴스 반환
    /// </summary>
    /// <returns></returns>
    PostOfficeManager PostOfficeManagerInstance()
    {
        if (!postOfficeManager)
        {
            postOfficeManager = PostOfficeManager.Instance;
        }

        return postOfficeManager;
    }
    #endregion
}
