/**
 * @details ī�޶��� �������� ����
 * @author ��̼�
 * @date 22-06-06
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum EChaseState { noChase, chaseSanta, chaseBuilding, endChase };         // ī�޶��� ����

public class CameraMovement : MonoBehaviour
{
    #region ���� ����
    private static CameraMovement instance = null;
    public static CameraMovement Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField]
    private Transform distanceTransform;
    private Vector3 distance;                // �׻� �������־�� �� �Ÿ�

    [Header("---------- Chase")]
    private Transform chasingTarget = null;          // ������ Ÿ��+
    private Transform buildingDistance;

    [SerializeField]
    private float basicFieldOfView = 60f;            // ī�޶��� �⺻  field of view
    [SerializeField]
    private float chasingSantaFieldOfView = 10f;          // ��Ÿ ���� �� ī�޶��� field of view
    [SerializeField]
    private float basicRotateX = 30f;               // ī�޶��� �⺻  x��
    [SerializeField]
    private float chasingRotateX = 23f;         // ���� �� ī�޶��� x��

    Vector3 chasingCamAngles;
    Vector3 basicCamAngles;

    bool isSantaAngleStart = false;


    public EChaseState chaseState = EChaseState.noChase;

    [Header("---------- Move")]
    private float moveSpeed = 1.2f;
    private Vector2 currentPos, previousPos;
    private Vector3 movePos;
    public bool canMove = true;


    [Header("---------- Zoom")]
    public Camera cam;
    private float perspectiveZoomSpeed = 5f;  //����,�ܾƿ��Ҷ� �ӵ�   
    private float zoomMinValue = 6f;
    private float zoomMaxValue = 70f;
    private float sensitive = 1f;

    Vector2 touchZeroPrevPos;
    Vector2 touchOnePrevPos;

    float prevTouchDeltaMag;
    float touchDeltaMag;

    float deltaMagnitudeDiff;

    float zoomSpeed = 0.2f;

    Touch touchZero;
    Touch touchOne;

    // ĳ��
    private UIManager uiManager;

    #endregion

    #region ����Ƽ �Լ�

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }

        uiManager = UIManager.Instance;

        chaseState = EChaseState.noChase;                       // ī�޶��� ���� ����   

        cam.fieldOfView = basicFieldOfView;                     // ī�޶��� �⺻ �� ����

        basicCamAngles = new Vector3(basicRotateX, 0, 0);       // ī�޶��� �⺻ ���� ����
        cam.transform.eulerAngles = basicCamAngles;

        chasingCamAngles = new Vector3(chasingRotateX, 0, 0);   // ���� �� ī�޶��� ���� ����

        distance = distanceTransform.localPosition;             // ī�޶��� �⺻ ��ġ ����
        cam.transform.position = distance;
    }

    private void Start()
    {
        StartCoroutine(Angles());
        StartCoroutine(Position());
        StartCoroutine(FieldOfView());
    }

    private void LateUpdate()
    {
        CameraMove();       // ī�޶��� ������
    }
    #endregion

    #region �Լ�
    #region ī�޶� ������

    /// <summary>
    /// ��ġ�� ī�޶� ������
    /// </summary>
    void CamMove()
    {
        if (Input.touchCount == 2)          // �� �հ��� ��ġ ���̸� return
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            previousPos = Input.mousePosition;      // ��ġ�� ���� ���� �� ��ġ ����
        }
        else if (Input.GetMouseButton(0))
        {
            currentPos = Input.mousePosition;

            movePos = (Vector3)(previousPos - currentPos) * Time.deltaTime * moveSpeed;

            Vector3 pos = cam.transform.position + movePos;

            if (pos.x < -15f)
            {
                Vector3 newPos = cam.transform.position;
                newPos.x = -15f;
                cam.transform.position = newPos;

                return;
            }
            else if (pos.x > 30f)
            {
                Vector3 newPos = cam.transform.position;
                newPos.x = 30f;
                cam.transform.position = newPos;

                return;
            }
            else if (pos.y < 10f)
            {
                Vector3 newPos = cam.transform.position;
                newPos.y = 10f;
                cam.transform.position = newPos;

                return;
            }
            else if (pos.y > 20f)
            {
                Vector3 newPos = cam.transform.position;
                newPos.y = 20f;
                cam.transform.position = newPos;

                return;
            }
            else if (pos.z > -10f)
            {
                Vector3 newPos = cam.transform.position;
                newPos.z = -10f;
                cam.transform.position = newPos;

                return;
            }
            else if (pos.z < -31f)
            {
                Vector3 newPos = cam.transform.position;
                newPos.z = -31f;
                cam.transform.position = newPos;

                return;
            }
            else
            {
                cam.transform.Translate(movePos);                      // ī�޶� �̵� ��Ŵ
            }

            previousPos = Input.mousePosition;
        }
    }

    /// <summary>
    /// ��ġ�� ī�޶� �� ��/�� �ƿ�
    /// </summary>
    void Zoom()
    {
        touchZero = Input.GetTouch(0); //ù��° �հ��� ��ġ�� ����
        touchOne = Input.GetTouch(1); //�ι�° �հ��� ��ġ�� ����

        //��ġ�� ���� ���� ��ġ���� ���� ������
        //ó�� ��ġ�� ��ġ(touchZero.position)���� ���� �����ӿ����� ��ġ ��ġ�� �̹� �����ӿ��� ��ġ ��ġ�� ���̸� ��
        touchZeroPrevPos = touchZero.position - touchZero.deltaPosition; //deltaPosition�� �̵����� ������ �� ���
        touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        // �� �����ӿ��� ��ġ ������ ���� �Ÿ� ����
        prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude; //magnitude�� �� ������ �Ÿ� ��(����)
        touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        // �Ÿ� ���� ����(�Ÿ��� �������� ũ��(���̳ʽ��� ������)�հ����� ���� ����_���� ����)
        deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, cam.fieldOfView + deltaMagnitudeDiff * perspectiveZoomSpeed, sensitive * Time.deltaTime * zoomSpeed);
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, zoomMinValue, zoomMaxValue);
    }

    void CameraMove()
    {
        if (canMove)
        {
            if (Input.GetMouseButton(0))  // �� �հ������� ��ġ �� 
            {
                if (chaseState == EChaseState.endChase)
                {
                    chaseState = EChaseState.noChase;
                }

                if (chaseState == EChaseState.noChase) CamMove();        // ���� ���� �ƴϸ� ī�޶� �̵�                                              
            }



#if UNITY_ANDROID && !UNITY_EDITOR
            if (Input.touchCount == 2) // �� �հ������� ��ġ ��
            {
                if (chaseState == EChaseState.endChase)
                {
                    chaseState = EChaseState.noChase;
                }

                Zoom();                 // �� ��/�ƿ�
            }
            else
            {
                if (Input.GetMouseButton(0))  // �� �հ������� ��ġ �� 
                {
                    if (chaseState == EChaseState.endChase)
                    {
                        chaseState = EChaseState.noChase;
                    }

                    if (chaseState == EChaseState.noChase)          // ���� ���� �ƴϸ�
                        CamMove();                                 // ī�޶� �̵�
                }
            }
#endif
        }
    }
#endregion

#region Ÿ�� ����
   
    /// <summary>
    /// ī�޶� ��Ÿ�� ����ٴ�
    /// </summary>
    /// <param name="obj">Ÿ���� Transform</param>
    public void ChaseSanta(Transform obj)
    {
        chaseState = EChaseState.chaseSanta;    // ī�޶��� ���� ����

        chasingTarget = obj;

        isSantaAngleStart = true;
    }

    /// <summary>
    /// ī�޶� ������ ī�޶� ��ġ�� �̵�
    /// </summary>
    /// <param name="obj">Ÿ���� Transform</param>
    /// <param name="distance">ī�޶���� �Ÿ�</param>
    public void ChaseBuilding(Transform obj, Transform distance)
    {
        chaseState = EChaseState.chaseBuilding;    // ī�޶��� ���� ����

        chasingTarget = obj;
        buildingDistance = distance;
    }

    /// <summary>
    /// Ÿ�� ���� ���� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void EndChaseTarget()
    {
        chaseState = EChaseState.endChase;                    // ī�޶��� ���� ���� 

        uiManager.HideClickObjWindow();                     // Ŭ�� ������Ʈ â ���ֱ�
    }

    /// <summary>
    /// ī�޶��� �ޱ��� ����
    /// </summary>
    /// <returns></returns>
    IEnumerator Angles()
    {
        while (true)
        {
            if (isSantaAngleStart)       // ��Ÿ ���� �� 
            {
                while (Vector3.Distance(cam.transform.eulerAngles, chasingCamAngles) > 0.05f 
                    && chaseState == EChaseState.chaseSanta
                    && isSantaAngleStart)
                {
                    cam.transform.eulerAngles = Vector3.Lerp(cam.transform.eulerAngles, chasingCamAngles, Time.deltaTime);

                    yield return null;
                }
                isSantaAngleStart = false;
            }

            else if (chaseState == EChaseState.endChase)     // ���� ���� �� �⺻ ī�޶� �ޱ۷� �̵�
            {
                while (Vector3.Distance(cam.transform.eulerAngles, basicCamAngles) > 0.05f && chaseState == EChaseState.endChase)
                {
                    cam.transform.eulerAngles = Vector3.Lerp(cam.transform.eulerAngles, basicCamAngles, Time.deltaTime);

                    yield return null;
                }
                if (chaseState == EChaseState.endChase) chaseState = EChaseState.noChase;
            }

            yield return null;
        }
    }

    /// <summary>
    /// ī�޶��� ��ġ�� ����
    /// </summary>
    /// <returns></returns>
    IEnumerator Position()
    {
        Vector3 targetPos;
        while (true)
        {
            if (chaseState == EChaseState.chaseSanta)       // ��Ÿ�� ������ �� ī�޶� ��Ÿ�� ����ٴ�
            {
                while (chaseState == EChaseState.chaseSanta)
                {
                    targetPos = distance + chasingTarget.position;
                    cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, Time.deltaTime);

                    yield return null;
                }
            }
            else if (chaseState == EChaseState.chaseBuilding)       // �ǹ��� ������ �� �ǹ��� cameraPos ��ġ�� �̵�
            {
                targetPos = chasingTarget.position + buildingDistance.localPosition;

                while (Vector3.Distance(cam.transform.position, targetPos) > 0.05f && chaseState == EChaseState.chaseBuilding)
                {
                    targetPos = chasingTarget.position + buildingDistance.localPosition;

                    cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, Time.deltaTime * 1.6f);

                    yield return null;
                }
                if (chaseState == EChaseState.chaseBuilding) chaseState = EChaseState.noChase;
            }

            else if (chaseState == EChaseState.endChase)         // ���� ���� �� �⺻ ī�޶� ��ġ�� �̵�
            {
                while (Vector3.Distance(cam.transform.position, distance) > 0.05f && chaseState == EChaseState.endChase)
                {
                    cam.transform.position = Vector3.Lerp(cam.transform.position, distance, Time.deltaTime * 1.6f);
                    //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime);

                    yield return null;
                }
                //transform.rotation = Quaternion.Euler(0, 0, 0);
                if (chaseState == EChaseState.endChase) chaseState = EChaseState.noChase;
            }

            yield return null;
        }
    }

    /// <summary>
    /// ī�޶��� FieldOfView�� ����
    /// </summary>
    /// <returns></returns>
    IEnumerator FieldOfView()
    {
        while (true)
        {
            if (chaseState == EChaseState.chaseSanta)           // ��Ÿ ���� �� ��Ÿ�� ���� Zoom In
            {
                while (Mathf.Abs(cam.fieldOfView - chasingSantaFieldOfView) > 0.05f && chaseState == EChaseState.chaseSanta)
                {
                    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, chasingSantaFieldOfView, Time.deltaTime * 1.6f);

                    yield return null;
                }
            }
            else if (chaseState == EChaseState.endChase)         // ���� ���� �� Zoom Out
            {
                while (Mathf.Abs(cam.fieldOfView - basicFieldOfView) > 0.05f && chaseState == EChaseState.endChase)
                {
                    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, basicFieldOfView, Time.deltaTime * 1.6f);

                    yield return null;
                }
                if (chaseState == EChaseState.endChase) chaseState = EChaseState.noChase;
            }
            yield return null;
        }
    }

#endregion

#endregion
}

