using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Santa : MonoBehaviour
{

    // 산타 터치 시 카메라의 Target을 해당 산타로 set
    void TouchSanta()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))
            {
                if (hit.collider.CompareTag("Santa"))
                {
                    CameraMovement.Instance.StartChaseTarget(this.transform);
                }
            }
        }
    }
   
    void Update()
    {
        TouchSanta();
    }
}
