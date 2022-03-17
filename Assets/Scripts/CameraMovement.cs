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
    private float chasingSantaFieldOfView = 15f;          // ��Ÿ ���� �� ī�޶��� field of view
    [SerializeField]
    private float chasingBuildingFieldOfView = 25f;          // �ǹ� ���� �� ī�޶��� field of view
    [SerializeField]
    private float basicRotateX = 30f;               // ī�޶��� �⺻  x��
    [SerializeField]
    private float chasingRotateX = 19.7f;         // ���� �� ī�޶��� x��

    enum ChaseState { chaseStart, chasing, chaseEnd, noChase };         // ī�޶��� ����
    ChaseState chaseState = ChaseState.noChase;

    [Header("---------- Move")]
    public float moveSpeed = 0.06f;
    public Vector2 currentPos, previousPos;
    public Vector3 movePos;
    public bool canMove = true;

    [Header("---------- Rotate")]
    public float rotateSpeed = 1;

    public float minRotateX = -10f;                // ȸ�� �� x���� Min ��
    public float maxRotateX = 20f;                 // ȸ�� �� x���� Max ��

    public float minRotateY = -55f;                // ȸ�� �� y���� Min ��
    public float maxRotateY = 55f;                 // ȸ�� �� y���� Max ��

    public float mouseX;
    public float mouseY;
    float touchX;
    float touchY;

    Vector3 FirstPoint;
    Vector3 SecondPoint;
    float xAngle;
    float yAngle;
    float xAngleTemp;
    float yAngleTemp;


    [Header("---------- Zoom")]
    public Camera cam;
    public float perspectiveZoomSpeed = 5f;  //����,�ܾƿ��Ҷ� �ӵ�   
    public float zoomMinValue = 6f;
    public float zoomMaxValue = 70f;
    public float sensitive = 1f;
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

    // ī�޶� �� ��/�� �ƿ�
    void Zoom()
    {
        if (chaseState == ChaseState.chaseStart) chaseState = ChaseState.chasing;
        else if (chaseState == ChaseState.chaseEnd) chaseState = ChaseState.noChase;

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

            else  // PC���� Ȯ���ϱ� ����
            {
                if (chaseState == ChaseState.chasing)          // ������ Ÿ���� ������
                    CamRotate();                    // ������Ʈ�� �߽����� ī�޶� ȸ��
            }
        }
    }
    #endregion

    #region Ÿ�� ����
    // 3�� �� ChaseState ����
    IEnumerator ChangeChaseState(ChaseState cs)
    {
        yield return new WaitForSeconds(5f);

        chaseState = cs;
    }

    // Ÿ�� ���� ����
    public void StartChaseTarget()
    {
        GameManager.Instance.ShowClickObjPanel();               // ��Ÿ ���� â ����

        chaseState = ChaseState.chaseStart;                     // ī�޶��� ���� ����

        StartCoroutine(ChangeChaseState(ChaseState.chasing));   // 3�� �� chasing ���·� ����
    }

    // Ÿ�� ���� ����
    public void EndChaseTarget()
    {
        GameManager.Instance.HideClickObjPanel();             // Ŭ�� ������Ʈ â ���ֱ�

        if (chasingSanta) chasingSanta = null;
        else if (chasingBuilding) chasingBuilding = null;

        chaseState = ChaseState.chaseEnd;                     // ī�޶��� ���� ���� 

        StartCoroutine(ChangeChaseState(ChaseState.noChase));       // 3�� �� noChase ���·� ����
    }

    // Ÿ�� ���� ���� �� ī�޶� ���� (��Ÿ�� Ÿ���� ��)
    void SetStartChaseCam()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, chasingSantaFieldOfView, Time.deltaTime);          // Ÿ���� ���� ����

        Quaternion chasingCamRot = Quaternion.Euler(new Vector3(chasingRotateX, cam.transform.rotation.y, cam.transform.rotation.z));    // ī�޶��� x���� ����
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, chasingCamRot, Time.deltaTime);
    }

    // �ǹ��� Ÿ���� ���� �ǹ��� ���� ����
    void SetZoomInBuilding()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, chasingBuildingFieldOfView, Time.deltaTime);          // Ÿ���� ���� ����

        cam.transform.position = Vector3.Lerp(cam.transform.position, chasingBuilding.position + buildingDistance, Time.deltaTime);     // ī�޶��� ��ġ ����

        Quaternion chasingCamRot = Quaternion.Euler(new Vector3(chasingRotateX, cam.transform.rotation.y, cam.transform.rotation.z));    // ī�޶��� x���� ����
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, chasingCamRot, Time.deltaTime);
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
            Vector3 targetPos = chasingSanta.position + distance;
            cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, Time.deltaTime);
        }
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

