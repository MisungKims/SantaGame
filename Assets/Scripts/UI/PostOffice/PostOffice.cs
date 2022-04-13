using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostOffice : MonoBehaviour
{
    public static PostOffice instance;

    [SerializeField]
    private GameObject postObj;
    public WritingPad writingPad;

    private RectTransform rectTransform;
    private RectTransform parentRectTransform;

    private float margin = -70;

    private void Awake()
    {
        if (instance == null)
            instance = this; // �ν��Ͻ��� �ڱ� �ڽ��� �־��
        else
            Destroy(gameObject);

    rectTransform = postObj.transform.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector3(0, 0, 0);

        parentRectTransform = this.transform.GetComponent<RectTransform>();
        parentRectTransform.sizeDelta = new Vector2(0, 80);

        ReadCSV();
    }

    void ReadCSV()
    {
        // csv ������ ���� ���� ��������
        List<Dictionary<string, object>> data = CSVReader.Read("PostOfficeData");

        for (int i = 0; i < data.Count; i++)
        {
            AttendanceInstant(
                i,
                data[i]["����"].ToString(),
                  data[i]["����"].ToString()
                  );
        }
    }

    void AttendanceInstant(int i, string name, string content)
    {
        PostObject instant = GameObject.Instantiate(postObj, this.transform).GetComponent<PostObject>();

        instant.transform.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;

        instant.name = i.ToString();
        instant.PostName = name;
        instant.PostConent = content;

        rectTransform.anchoredPosition += new Vector2(0, margin);

        parentRectTransform.sizeDelta += new Vector2(0, rectTransform.sizeDelta.y);
    }
}
