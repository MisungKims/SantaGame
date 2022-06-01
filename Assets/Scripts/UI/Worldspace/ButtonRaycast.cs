/**
 * @brief UI를 레이캐스트를 사용해 터치했는지 확인
 * @author 김미성
 * @date 22-04-26
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRaycast : ClickScale
{
    
    protected virtual void Touched()
    {
        StartCoroutine(ScaleDown());
    }

    void DetectTouch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))
            {
                if (hit.collider.CompareTag("WorldSpaceUI") && hit.collider.name == this.name)
                {
                    Touched();
                }
            }
        }
    }

    protected override IEnumerator Start()
    {
        yield return StartCoroutine(base.Start()); // 베이스 호출
    }

    void Update()
    {
        DetectTouch();
    }
}
