using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Santa : MonoBehaviour
{

    public int level = 1;

    Animator anim;

    public void InitSanta(string santaName)
    {
        level = 1;
        
        name += " " + santaName;

        SetCamTargetThis();

        gameObject.SetActive(true);

        anim.SetInteger("SantaIndex", Random.Range(0, 11));
    }

   
    public void SetCamTargetThis()
    {
        CameraMovement.Instance.StartChaseTarget(this.transform);
    }

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
                    SetCamTargetThis();
                }
            }
        }
    }

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        TouchSanta();
    }
}
