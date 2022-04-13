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
            contentText.text = postContent;
        }
    }
}
