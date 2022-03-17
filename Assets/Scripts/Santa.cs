using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Santa : MonoBehaviour
{
    #region 변수
    
    private int level = 1;
    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    Animator anim;
    #endregion

    #region 함수

    // 산타 초기화
    public void InitSanta(string santaName)
    {
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
    #endregion

    #region 유니티 메소드
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        TouchSanta();
    }
    #endregion

}
