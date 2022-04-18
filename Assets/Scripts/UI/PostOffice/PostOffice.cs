/**
 * @details 우체국 UI 생성
 * @author 김미성
 * @date 22-04-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostOffice : MonoBehaviour
{
    private static PostOffice instance;
    public static PostOffice Instance
    {
        get { return instance; }
    }

    [SerializeField]
    private GameObject postObj;
    public WritingPad writingPad;

    private RectTransform rectTransform;
    private RectTransform parentRectTransform;

    private float nextYPos = -70;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        SetTransform();

        ReadCSV();
    }

    /// <summary>
    /// 초기 위치와 사이즈 설정
    /// </summary>
    void SetTransform()
    {
        rectTransform = postObj.transform.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector3(0, 0, 0);

        parentRectTransform = this.transform.GetComponent<RectTransform>();
        parentRectTransform.sizeDelta = new Vector2(0, 80);
    }

    /// <summary>
    /// csv 리더를 통해 파일 가져오기
    /// </summary>
    void ReadCSV()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("PostOfficeData");

        for (int i = 0; i < data.Count; i++)
        {
            PostOfficeInstance(
                i,
                data[i]["수신"].ToString(),
                data[i]["내용"].ToString()
                );
        }
    }

    /// <summary>
    /// 우체국 인스턴스 생성
    /// </summary>
    void PostOfficeInstance(int i, string name, string content)
    {
        PostObject instant = GameObject.Instantiate(postObj, this.transform).GetComponent<PostObject>();

        instant.transform.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;

        instant.name = i.ToString();
        instant.PostName = name;
        instant.PostConent = content;

        rectTransform.anchoredPosition += new Vector2(0, nextYPos);

        parentRectTransform.sizeDelta += new Vector2(0, rectTransform.sizeDelta.y);
    }
}
