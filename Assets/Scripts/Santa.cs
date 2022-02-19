using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Santa : MonoBehaviour
{

    // ��Ÿ ��ġ �� ī�޶��� Target�� �ش� ��Ÿ�� set
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
