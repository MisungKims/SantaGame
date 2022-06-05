/**
 * @details 이벤트 트리거로 scale 변경
 * @author 김미성
 * @date 22-04-27
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScale : ClickScale
{
    void OnPointerDown(PointerEventData eventData)      // 마우스를 눌렀을 때
    {
        StartCoroutine(ScaleDown());                     // 스케일을 작아지게
    }

    protected override IEnumerator Start()
    {
        yield return StartCoroutine(base.Start()); // 베이스 호출

        /* 이벤트 트리거*/
        EventTrigger et = this.gameObject.AddComponent<EventTrigger>();     // 트리거 추가

        EventTrigger.Entry entry_pointerDown = new EventTrigger.Entry();    // 엔트리 생성
        entry_pointerDown.eventID = EventTriggerType.PointerDown;           // 어떤 때에 실행할 것인가
        entry_pointerDown.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });   // 엔트리에 이벤트를 넣어줌
        et.triggers.Add(entry_pointerDown);     // 최종으로 추가
    }

    
}
