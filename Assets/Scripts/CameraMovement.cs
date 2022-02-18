using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("---------- Chase")]
    public Transform chasingTarget = null;

    public float basicFieldOfView = 60f;            // �⺻ ī�޶��� field of view
    public float chasingFieldOfView = 25f;          // ���� �� ī�޶��� field of view

    enum ChaseState{ chaseStart, chasing, chaseEnd, noChase };
    ChaseState chaseState;

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

    void CamMove()                  // ī�޶� ������
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

    void CamRotate()            // ī�޶� ȸ��
    {
        if (Input.GetMouseButton(0))
        {
            mouseX += Input.GetAxis("Mouse X");
            mouseY += Input.GetAxis("Mouse Y") * -1;

            float xRot = Mathf.Clamp(transform.rotation.x + mouseY, minRotateX, maxRotateX);
            float yRot = Mathf.Clamp(transform.rotation.y + mouseX, minRotateY, maxRotateY);

            transform.rotation = Quaternion.Euler(new Vector3(xRot, yRot, 0) * rotateSpeed);
        }
    }

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

    public void StartChaseTarget(Transform targetTransform)                                // Ÿ�� ���� ����
    {
        chasingTarget = targetTransform;

        chaseState = ChaseState.chaseStart;

        StartCoroutine(changeChaseState(ChaseState.chasing));       // 3�� �� chasing ���·� ����
    }

    void SetChasingCamera()     // Ÿ�� ���� ���� �� ī�޶� ����
    {
        if (chaseState == ChaseState.chaseStart && chasingTarget != null)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, chasingFieldOfView, Time.deltaTime);          // Ÿ���� ���� ����

            Quaternion chasingCamRot = Quaternion.Euler(new Vector3(chasingCamRotateX, cam.transform.rotation.y, cam.transform.rotation.z));    // ī�޶��� x���� ����
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, chasingCamRot, Time.deltaTime);
        }
    }

    IEnumerator changeChaseState(ChaseState cs)
    {
        yield return new WaitForSeconds(3f);

        chaseState = cs;
    }

    void EndChaseTarget()                   // Ÿ�� ���� ����
    {
        // ��� �⺻ ������ �ǵ���
        chaseState = ChaseState.chaseEnd;
        cam.fieldOfView = basicFieldOfView;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        cam.transform.rotation = Quaternion.Euler(new Vector3(basicCamRotateX, cam.transform.rotation.y, cam.transform.rotation.z));

        chasingTarget = null;
    }



    // Update is called once per frame
    void Update()
    {
        CamRotate();

        SetChasingCamera();             // Ÿ�� ���� ���� �� ī�޶� ����

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
        // ������ Ÿ���� ������ ������Ʈ�� ����ٴ�
        if (chasingTarget != null)
        {
            Vector3 targetPos = new Vector3(chasingTarget.position.x, chasingTarget.position.y, chasingTarget.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime);
        }
    }
}

