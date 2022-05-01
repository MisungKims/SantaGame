/**
 * @details �̺�Ʈ Ʈ���ŷ� scale ����
 * @author ��̼�
 * @date 22-04-27
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScale : ClickScale
{
    void OnPointerDown(PointerEventData eventData)      // ���콺�� ������ ��
    {
        StartCoroutine(ScaleDown());                     // �������� �۾�����
    }

    protected override IEnumerator Start()
    {
        yield return StartCoroutine(base.Start()); // ���̽� ȣ��

        /* �̺�Ʈ Ʈ����*/
        EventTrigger et = this.gameObject.AddComponent<EventTrigger>();     // Ʈ���� �߰�

        EventTrigger.Entry entry_pointerDown = new EventTrigger.Entry();    // ��Ʈ�� ����
        entry_pointerDown.eventID = EventTriggerType.PointerDown;           // � ���� ������ ���ΰ�
        entry_pointerDown.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });   // ��Ʈ���� �̺�Ʈ�� �־���
        et.triggers.Add(entry_pointerDown);     // �������� �߰�
    }

    
}
