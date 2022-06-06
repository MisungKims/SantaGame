/**
 * @details ������ ������ ���� ������Ʈ
 * @author ��̼�
 * @date 22-04-27
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class PostObject : MonoBehaviour
{
    #region ����
    //UI ����
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text contentPreviewText;

    public GameObject notificationImage;   // ���ο� ������ ������ �˷��ִ� �̹���

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

            if (postContent.Length > 17)    // ������ ������ ��ٸ� 18�� ������ ������
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


    private WritingPad writingPad;      // ������ ������Ʈ

    public int index;           // ���� ���â������ �ε���

    public Gift gift;

    private int questID = 4;

    // ĳ��
    GameManager gameManager;
    PostOfficeManager postOfficeManager;
    QuestManager questManager;
    #endregion

    #region ����Ƽ �Լ�
    public void Awake()
    {
        gameManager = GameManager.Instance;

        postOfficeManager = PostOfficeManager.Instance;
        writingPad = postOfficeManager.writingPad;

        questManager = QuestManager.Instance;
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// �˸� �̹��� ���ΰ�ħ
    /// </summary>
    public void RefreshNotification()
    {
        if (PostOfficeManagerInstance().havePostList.Count > index)
        {
            // ���� ������ �˸� �̹����� ������ �ʵ���
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
    /// ������ Ȯ�� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void Read()
    {
        if (!postOfficeManager.havePostList[index].isRead)        // ������ ó�� �о��� ��
        {
            gift.giftInfo.wishCount++;        // ������ ���ø���Ʈ�� �߰�

            questManager.Success(questID);        // ����Ʈ ����

            gameManager.IncreaseGauge(5);      // ������ ����

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
    /// ������ ���� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void Discard()
    {
        postOfficeManager.Refresh(index);
    }

    /// <summary>
    /// �ش� UI�� ��ġ�� ���� ��ħ
    /// </summary>
    /// <param name="vector">������ ��ġ</param>
    public void RefreshTransform(Vector2 vector)
    {
        this.transform.GetComponent<RectTransform>().anchoredPosition = vector;
    }

    /// <summary>
    /// PostOfficeManager �ν��Ͻ� ��ȯ
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
