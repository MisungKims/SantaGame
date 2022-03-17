using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Santa : MonoBehaviour
{
    #region ����
    
    private int level = 1;
    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    Animator anim;
    #endregion

    #region �Լ�

    // ��Ÿ �ʱ�ȭ
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
                    SetCamTargetThis();
                }
            }
        }
    }
    #endregion

    #region ����Ƽ �޼ҵ�
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
