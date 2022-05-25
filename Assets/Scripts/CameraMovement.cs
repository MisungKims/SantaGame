/**
 * @details ī�޶��� �������� ����
 * @author ��̼�
 * @date 22-04-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    enum EChaseState { noChase, chaseSanta, chaseBuilding, endChase };         // ī�޶��� ����
    EChaseState chaseState = EChaseState.noChase;

    [Header("---------- Move")]
    private float moveSpeed = 0.06f;
    private Vector2 currentPos, previousPos;
    private Vector3 movePos;
    private bool canMove = true;

    [Header("---------- Rotate")]
    private float rotateSpeed = 1;

    private float minRotateX = -10f;                // ȸ�� �� x���� Min ��
    private float maxRotateX = 20f;                 // ȸ�� �� x���� Max ��

    private float minRotateY = -55f;                // ȸ�� �� y���� Min ��
    private float maxRotateY = 55f;                 // ȸ�� �� y���� Max ��

    //private float mouseX;
    //private float mouseY;
    private float touchX;
    private float touchY;

    //Vector3 FirstPoint;
    //Vector3 SecondPoint;
    //float xAngle;
    //float yAngle;
    //float xAngleTemp;
    //float yAngleTemp;


    [Header("---------- Zoom")]
    public Camera cam;
    private float perspectiveZoomSpeed = 5f;  //����,�ܾƿ��Ҷ� �ӵ�   
    private float zoomMinValue = 6f;
    private float zoomMaxValue = 70f;
    private float sensitive = 1f;


    private GameManager gameManager;
    private UIManager uiManager;


    Vector3 targetPos;

    #endregion

    #region ����� ���� �Լ�


    #region ī�޶� ������


    /// <summary>
    /// ��ġ�� ī�޶� ������
    /// </summary>
    void CamMove()
    {
        chaseState = EChaseState.noChase;

        Touch touch = Input.GetTouch(0);        // �հ��� ��ġ
        if (touch.phase == TouchPhase.Began)    // ������ ��
        {
            previousPos = touch.position - touch.deltaPosition;     // ��ġ�� ���� ���� �� ��ġ ����
        }
        else if (touch.phase == TouchPhase.Moved)   // ������ ��
        {
            currentPos = touch.position - touch.deltaPosition;      // ��ġ�� ���� ���� �� ��ġ ����

            movePos = (Vector3)(previousPos - currentPos) * moveSpeed;  // ���� ���� ���� ���� ���� �Ÿ��� ���Ͽ�

            cam.transform.Translate(movePos);                      // ī�޶� �̵� ��Ŵ
            //cam.transform.position = Vector3.Lerp(cam.transform.position, movePos, Time.deltaTime);

            previousPos = touch.position - touch.deltaPosition;
        }
        else if (touch.phase == TouchPhase.Ended)
        {

        }
    }

    /// <summary>
    /// ��ġ�� ī�޶� ȸ��
    /// </summary>
    void CamRotate()
    {
        //chaseState = EChaseState.noChase;

        //// ���콺�� ȸ��
        //if (Input.GetMouseButton(0))
        //{
        //    mouseX += Input.GetAxis("Mouse X");
        //    mouseY += Input.GetAxis("Mouse Y") * -1;

        //    Debug.Log("X : " + mouseX);

        //    float xRot = Mathf.Clamp(transform.rotation.x + mouseY, minRotateX, maxRotateX);
        //    float yRot = Mathf.Clamp(transform.rotation.y + mouseX, minRotateY, maxRotateY);

        //    transform.rotation = Quaternion.Euler(new Vector3(xRot, yRot, 0) * rotateSpeed);
        //}
        //if (chaseState == ChaseState.chaseStart) chaseState = ChaseState.chasing;

        //����Ͽ��� ��ġ�� ȸ��
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                touchX += touch.deltaPosition.x;
                touchY += touch.deltaPosition.y * -1;

                float xRot = Mathf.Clamp(transform.rotation.x + touchY, minRotateX, maxRotateX);
                float yRot = Mathf.Clamp(transform.rotation.y + touchX, minRotateY, maxRotateY);

                transform.rotation = Quaternion.Euler(new Vector3(xRot, yRot, 0) * rotateSpeed);
                //transform.RotateAround(chasingTarget.transform.position, new Vector3(xRot, yRot, 0), 10f);
            }

        }
    }

    Vector2 touchZeroPrevPos;
    Vector2 touchOnePrevPos;

    float prevTouchDeltaMag;
    float touchDeltaMag;

    float deltaMagnitudeDiff;
    /// <summary>
    /// ��ġ�� ī�޶� �� ��/�� �ƿ�
    /// </summary>
    void Zoom()
    {
        chaseState = EChaseState.noChase;

        Touch touchZero = Input.GetTouch(0); //ù��° �հ��� ��ġ�� ����
        Touch touchOne = Input.GetTouch(1); //�ι�° �հ��� ��ġ�� ����

        //��ġ�� ���� ���� ��ġ���� ���� ������
        //ó�� ��ġ�� ��ġ(touchZero.position)���� ���� �����ӿ����� ��ġ ��ġ�� �̹� �����ӿ��� ��ġ ��ġ�� ���̸� ��
        touchZeroPrevPos = touchZero.position - touchZero.deltaPosition; //deltaPosition�� �̵����� ������ �� ���
        touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        // �� �����ӿ��� ��ġ ������ ���� �Ÿ� ����
        prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude; //magnitude�� �� ������ �Ÿ� ��(����)
        touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        // �Ÿ� ���� ����(�Ÿ��� �������� ũ��(���̳ʽ��� ������)�հ����� ���� ����_���� ����)
        deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, cam.fieldOfView + deltaMagnitudeDiff * perspectiveZoomSpeed, sensitive * Time.deltaTime);
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, zoomMinValue, zoomMaxValue);
    }

    void CameraMove()
    {
        if (canMove)
        {
            if (Input.touchCount == 1)  // �� �հ������� ��ġ �� 
            {
                if (chaseState == EChaseState.noChase)          // ���� ���� �ƴϸ�
                    CamMove();                                 // ī�޶� �̵�
                else if (chaseState == EChaseState.chaseSanta)      // ���� ���̸�
                    CamRotate();                               // ������Ʈ�� �߽����� ī�޶� ȸ��
            }

            else if (Input.touchCount == 2) // �� �հ������� ��ġ ��
            {
                Zoom();                 // �� ��/�ƿ�
            }

            //else  // PC���� Ȯ���ϱ� ����
            //{
            //    if (chaseState == ChaseState.chasing)          // ������ Ÿ���� ������
            //        CamRotate();                    // ������Ʈ�� �߽����� ī�޶� ȸ��
            //}
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
            if (chaseState == EChaseState.chaseSanta)       // ��Ÿ ���� �� 
            {
                while (Vector3.Distance(cam.transform.eulerAngles, chasingCamAngles) > 0.05f && chaseState == EChaseState.chaseSanta)
                {
                    cam.transform.eulerAngles = Vector3.Lerp(cam.transform.eulerAngles, chasingCamAngles, Time.deltaTime);

                    yield return null;
                }
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
                //targetPos = cam.transform.position;
                //targetPos.x = chasingTarget.position.x + buildingDistance.localPosition.x;
                //targetPos.z = chasingTarget.position.z + buildingDistance.localPosition.z;
                while (Vector3.Distance(cam.transform.position, targetPos) > 0.05f && chaseState == EChaseState.chaseBuilding)
                {
                    targetPos = chasingTarget.position + buildingDistance.localPosition;
                    //targetPos = cam.transform.position;
                    //targetPos.x = chasingTarget.position.x + buildingDistance.localPosition.x;
                    //targetPos.z = chasingTarget.position.z + buildingDistance.localPosition.z;
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

                    yield return null;
                }
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

    /// <summary>
    /// ī�޶��� FieldOfView�� �����Ͽ� Ÿ���� ���� �� ��/�� �ƿ�
    /// </summary>
    /// <param name="fieldOfView">������ Field Of View</param>
    /// <returns></returns>
    //IEnumerator ChangeFieldOfView(float fieldOfView, EChaseState state)
    //{
    //    while (Mathf.Abs(cam.fieldOfView - fieldOfView) > 0.05f && chaseState == state)
    //    {
    //        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fieldOfView, Time.deltaTime * 1.6f);          // Ÿ���� ���� ����

    //        yield return null;
    //    }
    //}

    ///// <summary>
    ///// ī�޶��� �ޱ��� ����
    ///// </summary>
    ///// <param name="angle">������ ����</param>
    ///// <returns></returns>
    //IEnumerator ChangeAngles(Vector3 angle, EChaseState state)
    //{
    //    while (Vector3.Distance(cam.transform.eulerAngles, angle) > 0.05f && chaseState == state)
    //    {
    //        cam.transform.eulerAngles = Vector3.Lerp(cam.transform.eulerAngles, angle, Time.deltaTime);

    //        yield return null;
    //    }
    //}

    ///// <summary>
    ///// ī�޶��� ��ġ�� ����
    ///// </summary>
    ///// <returns></returns>
    //IEnumerator SetCamPos(Vector3 targetPos, bool isFix, EChaseState state)
    //{
    //    // ī�޶� Ÿ���� ���� �������� ���� ��
    //    if(isFix)
    //    {
    //        while (Vector3.Distance(cam.transform.position, targetPos) > 0.05f && chaseState == state)
    //        {
    //            cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, Time.deltaTime * 1.6f);

    //            yield return null;
    //        }
    //    }
    //    // ī�޶� Ÿ���� ���� ������ ��
    //    else
    //    {
    //        while (chaseState == EChaseState.chasing)
    //        {
    //            cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos + chasingTarget.position, Time.deltaTime);

    //            yield return null;
    //        }
    //    }
    //}

    #endregion

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
    }

    private void Start()
    {
        chaseState = EChaseState.noChase;                       // ī�޶��� ���� ����   

        cam.fieldOfView = basicFieldOfView;                     // ī�޶��� �⺻ �� ����

        basicCamAngles = new Vector3(basicRotateX, 0, 0);       // ī�޶��� �⺻ ���� ����
        cam.transform.eulerAngles = basicCamAngles;

        chasingCamAngles = new Vector3(chasingRotateX, 0, 0);   // ���� �� ī�޶��� ���� ����

        distance = distanceTransform.localPosition;             // ī�޶��� �⺻ ��ġ ����
        cam.transform.position = distance;


        StartCoroutine(Angles());
        StartCoroutine(Position());
        StartCoroutine(FieldOfView());
    }

    private void LateUpdate()
    {
        CameraMove();       // ī�޶��� ������
    }
    #endregion
}

