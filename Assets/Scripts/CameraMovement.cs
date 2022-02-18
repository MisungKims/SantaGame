using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    [Header("---------- Chase")]
    public Transform chasingTarget = null;

    public float basicFieldOfView = 60f;            // �⺻ ī�޶��� field of view
    public float chasingFieldOfView = 25f;          // ���� �� ī�޶��� field of view

    enum ChaseState{ chaseStart, chasing, chaseEnd, noChase };
    ChaseState chaseState = ChaseState.noChase;

    [Header("---------- Move")]
    public float moveSpeed = 0.06f;
    public Vector2 currentPos, previousPos;
    public Vector3 movePos;
    public bool canMove = true;

    [Header("---------- Rotate")]
    public float rotateSpeed = 1;

    public float basicCamRotateX = 30f;           // �⺻ ī�޶��� x��
    public float chasingCamRotateX = 20f;         // ���� �� ī�޶��� x��

    public float minRotateX = -10f;                // ȸ�� �� x���� Min ��
    public float maxRotateX = 20f;                 // ȸ�� �� x���� Max ��
    
    public float minRotateY = -40f;                // ȸ�� �� y���� Min ��
    public float maxRotateY = 70f;                 // ȸ�� �� y���� Max ��

    float mouseX;
    float mouseY;

    Vector3 FirstPoint;
    Vector3 SecondPoint;
    float xAngle;
    float yAngle;
    float xAngleTemp;
    float yAngleTemp;


    [Header("---------- Zoom")]
    public Camera cam;
    public float perspectiveZoomSpeed = 0.09f;  //����,�ܾƿ��Ҷ� �ӵ�   
    public float zoomMinValue = 10f;
    public float zoomMaxValue = 130f;

    #region �̱���
    private static CameraMovement cameraInstance = null;

    private void Awake()
    {
        if (cameraInstance == null)
        {
            cameraInstance = this;
            DontDestroyOnLoad(gameObject);      // �� ��ȯ �ÿ��� �ı����� ����
        }
        else
        {
            if (cameraInstance != this)
                Destroy(this.gameObject);
        }
    }

    public static CameraMovement Instance
    {
        get
        {
            if (cameraInstance == null)
            {
                return null;
            }
            return cameraInstance;
        }
    }

    #endregion

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

            this.transform.Translate(movePos);                      // ī�޶� �̵� ��Ŵ

            previousPos = touch.position - touch.deltaPosition;
        }
        else if (touch.phase == TouchPhase.Ended)
        {
        }
    }

    // ī�޶� ȸ��
    void CamRotate()            
    {
        if (Input.GetMouseButton(0))
        {
            mouseX += Input.GetAxis("Mouse X");
            mouseY += Input.GetAxis("Mouse Y") * -1;

            float xRot = Mathf.Clamp(transform.rotation.x + mouseY, minRotateX, maxRotateX);
            float yRot = Mathf.Clamp(transform.rotation.y + mouseX, minRotateY, maxRotateY);

            transform.rotation = Quaternion.Euler(new Vector3(xRot, yRot, 0) * rotateSpeed);
        }

        //if (Input.touchCount > 0)
        //{
        //    if (Input.GetTouch(0).phase == TouchPhase.Began)
        //    {
        //        FirstPoint = Input.GetTouch(0).position;
        //        xAngleTemp = xAngle;
        //        yAngleTemp = yAngle;
        //    }
        //    if (Input.GetTouch(0).phase == TouchPhase.Moved)
        //    {
        //        SecondPoint = Input.GetTouch(0).position;
        //        xAngle = xAngleTemp + (SecondPoint.x - FirstPoint.x) * 180 / Screen.width;
        //        yAngle = yAngleTemp - (SecondPoint.y - FirstPoint.y) * 90 * 3f / Screen.height; // Y�� ��ȭ�� �� ������ 3�� ������.

        //        // ȸ������ 40~85�� ����
        //        if (yAngle < 40f)
        //            yAngle = 40f;
        //        if (yAngle > 85f)
        //            yAngle = 85f;

        //        transform.rotation = Quaternion.Euler(yAngle, xAngle, 0.0f);
        //    }
        //}
    }

    public void OnDrag(PointerEventData eventData)
    {
        xAngle = eventData.delta.x * Time.deltaTime * 8f;
        yAngle = eventData.delta.y * Time.deltaTime * 8f;

        transform.Rotate(yAngle, -xAngle, 0, Space.World);
    }

    // ī�޶� �� ��/�� �ƿ�
    void Zoom()
    {
        if (chaseState == ChaseState.chaseStart) chaseState = ChaseState.chasing;

        Touch touchZero = Input.GetTouch(0); //ù��° �հ��� ��ġ�� ����
        Touch touchOne = Input.GetTouch(1); //�ι�° �հ��� ��ġ�� ����

        //��ġ�� ���� ���� ��ġ���� ���� ������
        //ó�� ��ġ�� ��ġ(touchZero.position)���� ���� �����ӿ����� ��ġ ��ġ�� �̹� �����ӿ��� ��ġ ��ġ�� ���̸� ��
        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition; //deltaPosition�� �̵����� ������ �� ���
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        // �� �����ӿ��� ��ġ ������ ���� �Ÿ� ����
        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude; //magnitude�� �� ������ �Ÿ� ��(����)
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        // �Ÿ� ���� ����(�Ÿ��� �������� ũ��(���̳ʽ��� ������)�հ����� ���� ����_���� ����)
        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        cam.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, zoomMinValue, zoomMaxValue);
    }

    // 3�� �� ChaseState ����
    IEnumerator ChangeChaseState(ChaseState cs)
    {
        yield return new WaitForSeconds(3f);

        chaseState = cs;
    }

    // Ÿ�� ���� ����
    public void StartChaseTarget(Transform targetTransform)                               
    {
        GameManager.Instance.ShowSantaPanel();

        chasingTarget = targetTransform;

        chaseState = ChaseState.chaseStart;

        StartCoroutine(ChangeChaseState(ChaseState.chasing));       // 3�� �� chasing ���·� ����
    }

    // Ÿ�� ���� ����
    public void EndChaseTarget()
    {
        chaseState = ChaseState.chaseEnd;

        chasingTarget = null;

        StartCoroutine(ChangeChaseState(ChaseState.noChase));       // 3�� �� noChase ���·� ����
    }

    // Ÿ�� ���� ���� �� ī�޶� ����
    void SetStartChaseCam()     
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, chasingFieldOfView, Time.deltaTime);          // Ÿ���� ���� ����

        Quaternion chasingCamRot = Quaternion.Euler(new Vector3(chasingCamRotateX, cam.transform.rotation.y, cam.transform.rotation.z));    // ī�޶��� x���� ����
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, chasingCamRot, Time.deltaTime);
    }

    // Ÿ�� ���� ���� �� ī�޶� �⺻������ ����
    void SetEndChaseCam()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, basicFieldOfView, Time.deltaTime);

        Quaternion chasingCamRot = Quaternion.Euler(new Vector3(basicCamRotateX, cam.transform.rotation.y, cam.transform.rotation.z));    // ī�޶��� x���� ����
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, chasingCamRot, Time.deltaTime);

        Quaternion chasingAxisRot = Quaternion.Euler(new Vector3(0, 0, 0));
        transform.rotation = Quaternion.Lerp(transform.rotation, chasingAxisRot, Time.deltaTime);

        Vector3 targetPos = new Vector3(0, 0, 0);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime);
    }

    void SetChaseCam()
    {
        if (chaseState == ChaseState.chaseStart && chasingTarget != null)
        {
            SetStartChaseCam();             // Ÿ�� ���� ���� �� ī�޶� ����
        }
        else if (chaseState == ChaseState.chaseEnd)
        {
            SetEndChaseCam();               // Ÿ�� ���� ���� �� ī�޶� ����
        }
    }

    // Update is called once per frame
    void Update()
    {
        CamRotate();

        SetChaseCam();      // Ÿ�� ���� ����/���� �� ī�޶� ����

        if (canMove)
        {
            if (Input.touchCount == 1)  // �� �հ������� ��ġ �� 
            {
                if (chasingTarget == null)          // ������ Ÿ���� ������
                    CamMove();                      // ī�޶� �̵�
                else                                // ������ Ÿ���� ������ 
                    CamRotate();                    // ������Ʈ�� �߽����� ī�޶� ȸ��
            }

            if (Input.touchCount == 2) // �� �հ������� ��ġ ��
            {
                Zoom();                 // �� ��/�ƿ�
            }
        }
    }

    private void FixedUpdate()
    {
        // ���� ���� �� ������Ʈ�� ����ٴ�
        if (chasingTarget != null)
        {
            Vector3 targetPos = new Vector3(chasingTarget.position.x, chasingTarget.position.y, chasingTarget.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime);
        }
    }
}

