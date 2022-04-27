/**
 * @details UI 오브젝트를 누르면 크기가 작아졌다가 커짐
 * @author 김미성
 * @date 22-04-27
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickScale : MonoBehaviour
{
    #region 변수

    protected Vector3 startScale; // 작아진 이후 다시 복구할 스케일

    public float speed = 0.9f;  // 작아지는 속도

    public float size = 0.8f;   // 작아지는 비율(크기)

    #endregion

    #region 코루틴
    protected IEnumerator ScaleDown()
    {
        while (transform.localScale.x > startScale.x * size)
        {
            transform.localScale *= speed;     // 지금의 0.9배 스케일로 변경

            yield return null;
        }

        yield return new WaitForSeconds(0.05f);

        StartCoroutine(ScaleUp());
    }


    protected IEnumerator ScaleUp()
    {
        while (transform.localScale.x < startScale.x)
        {
            transform.localScale *= 2 - speed;     // 줄어드는 속도와 동일하게 만듦

            yield return null;
        }

        transform.localScale = startScale;

        yield return null;
    }

    #endregion


    #region 유니티 메소드
    protected virtual IEnumerator Start()
    {
        yield return null;          // 스크롤뷰에 컨텐트가 자동으로 위치를 잡아주는 것을 기다림

        startScale = transform.localScale;
    }

    protected void OnEnable()
    {
        if (startScale != Vector3.zero)
        {
            transform.localScale = startScale;
        }
    }

    #endregion

}
