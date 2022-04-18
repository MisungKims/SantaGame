using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickScale : MonoBehaviour
{
    #region ����

    private Transform tr;       // �ڽ��� Ʈ������

    private Vector3 startScale; // �۾��� ���� �ٽ� ������ ������

    public float speed = 0.9f;  // �۾����� �ӵ�

    public float size = 0.8f;   // �۾����� ����(ũ��)

    #endregion

    #region �ڷ�ƾ
    IEnumerator ScaleDown()
    {
        while (tr.localScale.x > startScale.x * size)
        {
            tr.localScale *= speed;     // ������ 0.9�� �����Ϸ� ����

            yield return null;
        }
    }


    IEnumerator ScaleUp()
    {
        while (tr.localScale.x < startScale.x)
        {
           
            tr.localScale *= 2 - speed;     // �پ��� �ӵ��� �����ϰ� ����

            yield return null;


            //if (tr.localScale.x > startScale.x)
            //{
            //    tr.localScale = startScale;             // ���� �����Ϸ�
            //    break;
            //}
        }
    }


    #endregion

    #region �Լ�

    public void Up()
    {
        StartCoroutine(ScaleUp());
    }

    public void Down()
    {
        StartCoroutine(ScaleDown());
    }

    void OnPointerDown(PointerEventData eventData)      // ���콺�� ������ ��
    {
       
        Down();                     // �������� �۾�����
    }

    void OnPointerUp(PointerEventData eventData)        // ���콺�� �տ��� ���� ��
    {
         Up();
    }

    #endregion

    #region ����Ƽ �޼ҵ�
    IEnumerator Start()
    {
        yield return null;          // ��ũ�Ѻ信 ����Ʈ�� �ڵ����� ��ġ�� ����ִ� ���� ��ٸ�

        tr = this.transform;
        startScale = tr.localScale;

        //if (isCoin)
        //{
        //    yield break;            // �ڷ�ƾ �ߴ�
        //}

        //btn = GetComponent<Button>();

        //GameObject canvas = GameObject.Find("Canvas");
        //panel = canvas.transform.GetChild(canvas.transform.childCount - 1).gameObject;

        //rt = tr.GetComponent<RectTransform>();


        /* �̺�Ʈ Ʈ����*/
        EventTrigger et = this.gameObject.AddComponent<EventTrigger>();     // Ʈ���� �߰�

        EventTrigger.Entry entry_pointerDown = new EventTrigger.Entry();    // ��Ʈ�� ����
        entry_pointerDown.eventID = EventTriggerType.PointerDown;           // � ���� ������ ���ΰ�
        entry_pointerDown.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });   // ��Ʈ���� �̺�Ʈ�� �־���
        et.triggers.Add(entry_pointerDown);     // �������� �߰�

        EventTrigger.Entry entry_pointerUp = new EventTrigger.Entry();
        entry_pointerUp.eventID = EventTriggerType.PointerUp;
        entry_pointerUp.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
        et.triggers.Add(entry_pointerUp);
    }

    #endregion

}
