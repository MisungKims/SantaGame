using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickScale : MonoBehaviour
{
    #region 변수

    private Transform tr;       // 자신의 트랜스폼

    private Vector3 startScale; // 작아진 이후 다시 복구할 스케일

    public float speed = 0.9f;  // 작아지는 속도

    public float size = 0.8f;   // 작아지는 비율(크기)

    #endregion

    #region 코루틴
    IEnumerator ScaleDown()
    {
        while (tr.localScale.x > startScale.x * size)
        {
            tr.localScale *= speed;     // 지금의 0.9배 스케일로 변경

            yield return null;
        }
    }


    IEnumerator ScaleUp()
    {
        while (tr.localScale.x < startScale.x)
        {
           
            tr.localScale *= 2 - speed;     // 줄어드는 속도와 동일하게 만듦

            yield return null;


            //if (tr.localScale.x > startScale.x)
            //{
            //    tr.localScale = startScale;             // 원래 스케일로
            //    break;
            //}
        }
    }


    #endregion

    #region 함수

    public void Up()
    {
        StartCoroutine(ScaleUp());
    }

    public void Down()
    {
        StartCoroutine(ScaleDown());
    }

    void OnPointerDown(PointerEventData eventData)      // 마우스를 눌렀을 때
    {
       
        Down();                     // 스케일을 작아지게
    }

    void OnPointerUp(PointerEventData eventData)        // 마우스를 손에서 뗐을 때
    {
         Up();
    }

    #endregion

    #region 유니티 메소드
    IEnumerator Start()
    {
        yield return null;          // 스크롤뷰에 컨텐트가 자동으로 위치를 잡아주는 것을 기다림

        tr = this.transform;
        startScale = tr.localScale;

        //if (isCoin)
        //{
        //    yield break;            // 코루틴 중단
        //}

        //btn = GetComponent<Button>();

        //GameObject canvas = GameObject.Find("Canvas");
        //panel = canvas.transform.GetChild(canvas.transform.childCount - 1).gameObject;

        //rt = tr.GetComponent<RectTransform>();


        /* 이벤트 트리거*/
        EventTrigger et = this.gameObject.AddComponent<EventTrigger>();     // 트리거 추가

        EventTrigger.Entry entry_pointerDown = new EventTrigger.Entry();    // 엔트리 생성
        entry_pointerDown.eventID = EventTriggerType.PointerDown;           // 어떤 때에 실행할 것인가
        entry_pointerDown.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });   // 엔트리에 이벤트를 넣어줌
        et.triggers.Add(entry_pointerDown);     // 최종으로 추가

        EventTrigger.Entry entry_pointerUp = new EventTrigger.Entry();
        entry_pointerUp.eventID = EventTriggerType.PointerUp;
        entry_pointerUp.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
        et.triggers.Add(entry_pointerUp);
    }

    #endregion

}
