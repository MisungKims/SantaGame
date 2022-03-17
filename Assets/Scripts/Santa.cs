using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Santa : MonoBehaviour
{
    #region ����
    [SerializeField]
    private int level = 1;
    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    Animator anim;
    #endregion

    #region �Լ�

    /// <summary>
    /// ��Ÿ �ʱ�ȭ
    /// </summary>
    /// <param name="santaName">��Ÿ�� �̸�</param>
    public void InitSanta(string santaName)
    {
        name += " " + santaName;            // ��Ÿ �̸� ����

        SetCamTargetThis();                 // ī�޶� ��Ÿ�� ����ٴϵ���

        gameObject.SetActive(true);         // ��Ÿ�� ���̵���

        anim.SetInteger("SantaIndex", Random.Range(0, 11));     // � ��Ÿ�� �ҷ��ð���
    }

    /// <summary>
    /// ī�޶� �ش� ��Ÿ�� ����ٴ�
    /// </summary>
    public void SetCamTargetThis()
    {
        CameraMovement.Instance.StartChaseTarget();
        CameraMovement.Instance.chasingSanta = this.transform;
    }

    /// <summary>
    /// ��Ÿ ��ġ �� ī�޶��� Target�� �ش� ��Ÿ�� set
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
