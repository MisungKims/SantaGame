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

    // ��ũ��Ʈ
    private WritingPad writingPad;

    //// �� �� ����
    //private bool isRead;    // ���� �����̸� true
    //public bool IsRead
    //{
    //    set
    //    {
    //        isRead = value;
    //        notificationImage.SetActive(!isRead);
    //    }
    //}

    public int index;           // ���� ���â������ �ε���

   // public int listIndex;       // ���� ����Ʈ������ �ε���

    public Gift gift;

    private int questID = 4;

    #endregion

    #region ����Ƽ �Լ�
    public void Awake()
    {
        writingPad = PostOfficeManager.Instance.writingPad;
    }

    #endregion

    #region �Լ�
    /// <summary>
    /// �˸� �̹��� ���ΰ�ħ
    /// </summary>
    public void RefreshNotification()
    {
        if (PostOfficeManager.Instance.havePostList.Count > index)
        {
            // ���� ������ �˸� �̹����� ������ �ʵ���
            if (PostOfficeManager.Instance.havePostList[index].isRead)
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
        if (!PostOfficeManager.Instance.havePostList[index].isRead)        // ������ ó�� �о��� ��
        {
            //IsRead = true;

            gift.giftInfo.wishCount++;        // ������ ���ø���Ʈ�� �߰�

            QuestManager.Instance.Success(questID);        // ����Ʈ ����

            GameManager.Instance.IncreaseGauge(3);      // ������ ����

            PostOfficeManager.Instance.havePostList[index].isRead = true;

            notificationImage.SetActive(false);

            writingPad.canMove = true;
            writingPad.giftSprite = gift.giftImage;
        }
        else writingPad.canMove = false;

        writingPad.PostName = postName;
        writingPad.PostConent = postContent;
        writingPad.gameObject.SetActive(true);
    }

    /// <summary>
    /// ������ ���� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void Discard()
    {
        PostOfficeManager.Instance.Refresh(index);
    }

    /// <summary>
    /// �ش� UI�� ��ġ�� ���� ��ħ
    /// </summary>
    /// <param name="vector">������ ��ġ</param>
    public void RefreshTransform(Vector2 vector)
    {
        this.transform.GetComponent<RectTransform>().anchoredPosition = vector;
    }
    #endregion
}
