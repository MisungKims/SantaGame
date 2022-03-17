using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Santa : MonoBehaviour
{
    #region 변수
    [SerializeField]
    private int level = 1;
    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    Animator anim;
    #endregion

    #region 함수

    /// <summary>
    /// 산타 초기화
    /// </summary>
    /// <param name="santaName">산타의 이름</param>
    public void InitSanta(string santaName)
    {
        name += " " + santaName;            // 산타 이름 설정

        SetCamTargetThis();                 // 카메라가 산타를 따라다니도록

        gameObject.SetActive(true);         // 산타가 보이도록

        anim.SetInteger("SantaIndex", Random.Range(0, 11));     // 어떤 산타를 불러올건지
    }

    /// <summary>
    /// 카메라가 해당 산타를 따라다님
    /// </summary>
    public void SetCamTargetThis()
    {
        CameraMovement.Instance.StartChaseTarget();
        CameraMovement.Instance.chasingSanta = this.transform;
    }

    /// <summary>
    /// 산타 터치 시 카메라의 Target을 해당 산타로 set
    /// </summary>
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
