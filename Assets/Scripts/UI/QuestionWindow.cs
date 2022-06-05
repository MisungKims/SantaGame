using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionWindow : MonoBehaviour
{
    [SerializeField]
    private Text titleText;
    [SerializeField]
    private Text contentText;
    [SerializeField]
    private Text buttonText;

    public Button enterButton;

    private string title;
    public string Title
    {
        set
        {
            titleText.text = value;
        }
    }

    private string content;
    public string Content
    {
        set
        {
            contentText.text = value;
        }
    }

    private string button;
    public string Button
    {
        set
        {
            buttonText.text = value;
        }
    }
}
