using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Santa : MonoBehaviour
{
   
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))
            {
                if(hit.collider.name == this.name)
                {
                    CameraMovement.Instance.chasingTarget = this.transform;
                    CameraMovement.Instance.StartChaseTarget();
                }
                Debug.Log(hit.collider.name);
            }
        }
    }
}
