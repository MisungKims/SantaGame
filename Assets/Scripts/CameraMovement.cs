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

    public Vector3 distance;                // �׻� �������־�� �� �Ÿ�

    [Header("---------- Chase")]

    [SerializeField]
    public Transform chasingSanta = null;          // ������ Ÿ��

    public Transform chasingBuilding = null;

    public Vector3 buildingDistance;

    [SerializeField]
    private float basicFieldOfView = 60f;            // ī�޶��� �⺻  field of view
    [SerializeField]
    private float chasingSantaFieldOfView = 10f;          // ��Ÿ ���� �� ī�޶��� field of view
    [SerializeField]
    private float basicRotateX = 30f;               // ī�޶��� �⺻  x��
    [SerializeField]
    private float chasingRotateX = 19.7f;         // ���� �� ī�޶��� x��

    enum ChaseState { chaseStart, chasing, chaseEnd, noChase };         // ī�޶��� ����
    ChaseState chaseState = ChaseState.noChase;

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


    Vector3 targetPos;

    #endregion

    #region ����� ���� �Լ�


    #region ī�޶� ������
    public void SetCanMove(bool move)
    {
        canMove = move;
    }

    // ī�޶� ������
    void CamMove()
    {
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

    // ī�޶� ȸ��
    void CamRotate()
    {
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

    // ī�޶� �� ��/�� �ƿ�
    void Zoom()
    {
        if (chaseState == ChaseState.chaseStart) chaseState = ChaseState.chasing;
        else if (chaseState == ChaseState.chaseEnd) chaseState = ChaseState.noChase;

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
                if (chaseState == ChaseState.noChase)          // ���� ���� �ƴϸ�
                    CamMove();                                 // ī�޶� �̵�
                else if (chaseState == ChaseState.chasing)      // ���� ���̸�
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
    // 10�� �� ChaseState ����
    IEnumerator ChangeChaseState(ChaseState cs)
    {
        yield return new WaitForSeconds(10f);

        chaseState = cs;
    }

    // Ÿ�� ���� ����
    public void StartChaseTarget()
    {
        chaseState = ChaseState.chaseStart;                     // ī�޶��� ���� ����

        StartCoroutine(ChangeChaseState(ChaseState.chasing));   // 10�� �� chasing ���·� ����
    }

    // Ÿ�� ���� ����
    public void EndChaseTarget()
    {
        gameManager.HideClickObjWindow();             // Ŭ�� ������Ʈ â ���ֱ�

        if (chasingSanta) chasingSanta = null;
        else if (chasingBuilding) chasingBuilding = null;

        chaseState = ChaseState.chaseEnd;                     // ī�޶��� ���� ���� 

        StartCoroutine(ChangeChaseState(ChaseState.noChase));       // 10�� �� noChase ���·� ����
    }

    // Ÿ�� ���� ���� �� ī�޶� ���� (��Ÿ�� Ÿ���� ��)
    void SetStartChaseCam()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, chasingSantaFieldOfView, Time.deltaTime * 1.6f);          // Ÿ���� ���� ����

        // ī�޶��� x���� ����
        Quaternion chasingCamRot = Quaternion.Euler(new Vector3(chasingRotateX, cam.transform.rotation.y, cam.transform.rotation.z));    
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, chasingCamRot, Time.deltaTime);
    }

    // �ǹ��� Ÿ���� ���� �ǹ��� ���� ����
    void SetZoomInBuilding()
    {
        // ī�޶��� ��ġ ����
        targetPos = chasingBuilding.position + buildingDistance;
        cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, Time.deltaTime * 1.6f);


        //Quaternion chasingCamRot = Quaternion.Euler(new Vector3(chasingRotateX, cam.transform.rotation.y, cam.transform.rotation.z));
        //cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, chasingCamRot, Time.deltaTime);

        // ī�޶��� x���� ����
        Vector3 chasingCamAngles = new Vector3(chasingRotateX, cam.transform.rotation.y, cam.transform.rotation.z);
        cam.transform.eulerAngles = Vector3.Lerp(cam.transform.eulerAngles, chasingCamAngles, Time.deltaTime);
    }

    // Ÿ�� ���� ���� �� ī�޶� �⺻������ ����
    void SetEndChaseCam()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, basicFieldOfView, Time.deltaTime);            // �� �ƿ�

        // ī�޶��� x���� �������
        Quaternion chasingCamRot = Quaternion.Euler(new Vector3(basicRotateX, 0, 0));
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, chasingCamRot, Time.deltaTime);

        cam.transform.position = Vector3.Lerp(cam.transform.position, distance, Time.deltaTime);
    }

    void SetChaseCam()
    {
        if (chaseState == ChaseState.chaseStart)
        {
            if (chasingSanta != null)
            {
                SetStartChaseCam();     // ��Ÿ ���� ���� �� ī�޶� ����
            }
            else if(chasingBuilding != null)
            {
                SetZoomInBuilding();    // �ǹ��� ����
            }         
        }
        else if (chaseState == ChaseState.chaseEnd)
        {
            SetEndChaseCam();               // Ÿ�� ���� ���� �� ī�޶� ����
        }
    }

    void FollowSanta()
    {
        // ���� ���� �� ��Ÿ�� ����ٴ�
        if (chasingSanta != null)
        {
            targetPos = chasingSanta.position + distance;
            cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, Time.deltaTime);
        }

        //if (chasingBuilding != null)
        //{
        //    targetPos = chasingBuilding.position + chasingBuilding.GetChild(0).localPosition;
        //    cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, Time.deltaTime * 1.6f);
        //}
    }
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
    }

    private void Start()
    {
        gameManager = GameManager.Instance;

        // ī�޶��� ���� ����
        chaseState = ChaseState.noChase;            

        // ī�޶��� �⺻ �� ����
        cam.fieldOfView = basicFieldOfView;

        // ī�޶��� �⺻ ���� ����
        cam.transform.rotation = Quaternion.Euler(new Vector3(basicRotateX, 0, 0));

        // ī�޶��� �⺻ ��ġ ����
        distance = this.transform.GetChild(1).localPosition;
        cam.transform.position = distance;
    }


    private void LateUpdate()
    {
        CameraMove();       // ī�޶��� ������

        SetChaseCam();      // Ÿ�� ���� ����/���� �� ī�޶� ����

        FollowSanta();     // ���� ���� �� ������Ʈ�� ����ٴ�
    }
    #endregion
}

