/**
 * @brief 편지지
 * @author 김미성
 * @date 22-04-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WritingPad : MonoBehaviour
{
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text contentText;

    private string postName;        // 수신인
    public string PostName
    {
        set
        {
            postName = value;
            nameText.text = postName;
        }
    }

    private string postContent;     // 편지의 내용
    public string PostConent
    {
        set
        {
            postContent = value;
            contentText.text = postContent;
        }
    }
}
