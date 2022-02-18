using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    [Header("---------- Chase")]
    public Transform chasingTarget = null;

    public float basicFieldOfView = 60f;            // 기본 카메라의 field of view
    public float chasingFieldOfView = 25f;          // 추적 시 카메라의 field of view

    enum ChaseState{ chaseStart, chasing, chaseEnd, noChase };
    ChaseState chaseState = ChaseState.noChase;

    [Header("---------- Move")]
    public float moveSpeed = 0.06f;
    public Vector2 currentPos, previousPos;
    public Vector3 movePos;
    public bool canMove = true;

    [Header("---------- Rotate")]
    public float rotateSpeed = 1;

    public float basicCamRotateX = 30f;           // 기본 카메라의 x축
    public float chasingCamRotateX = 20f;         // 추적 시 카메라의 x축

    public float minRotateX = -10f;                // 회전 시 x축의 Min 값
    public float maxRotateX = 20f;                 // 회전 시 x축의 Max 값
    
    public float minRotateY = -40f;                // 회전 시 y축의 Min 값
    public float maxRotateY = 70f;                 // 회전 시 y축의 Max 값

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
    public float perspectiveZoomSpeed = 0.09f;  //줌인,줌아웃할때 속도   
    public float zoomMinValue = 10f;
    public float zoomMaxValue = 130f;

    #region 싱글톤
    private static CameraMovement cameraInstance = null;

    private void Awake()
    {
        if (cameraInstance == null)
        {
            cameraInstance = this;
            DontDestroyOnLoad(gameObject);      // 씬 전환 시에도 파괴되지 않음
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

    // 카메라 움직임
    void CamMove()                  
    {
        Touch touch = Input.GetTouch(0);        // 손가락 터치
        if (touch.phase == TouchPhase.Began)    // 눌렸을 때
        {
            previousPos = touch.position - touch.deltaPosition;     // 터치에 대한 이전 값 위치 저장
        }
        else if (touch.phase == TouchPhase.Moved)   // 움직일 때
        {
            currentPos = touch.position - touch.deltaPosition;      // 터치에 대한 현재 값 위치 저장

            movePos = (Vector3)(previousPos - currentPos) * moveSpeed;  // 이전 값과 현재 값의 벡터 거리를 구하여

            this.transform.Translate(movePos);                      // 카메라를 이동 시킴

            previousPos = touch.position - touch.deltaPosition;
        }
        else if (touch.phase == TouchPhase.Ended)
        {
        }
    }

    // 카메라 회전
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
        //        yAngle = yAngleTemp - (SecondPoint.y - FirstPoint.y) * 90 * 3f / Screen.height; // Y값 변화가 좀 느려서 3배 곱해줌.

        //        // 회전값을 40~85로 제한
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

    // 카메라 줌 인/줌 아웃
    void Zoom()
    {
        if (chaseState == ChaseState.chaseStart) chaseState = ChaseState.chasing;

        Touch touchZero = Input.GetTouch(0); //첫번째 손가락 터치를 저장
        Touch touchOne = Input.GetTouch(1); //두번째 손가락 터치를 저장

        //터치에 대한 이전 위치값을 각각 저장함
        //처음 터치한 위치(touchZero.position)에서 이전 프레임에서의 터치 위치와 이번 프로임에서 터치 위치의 차이를 뺌
        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition; //deltaPosition는 이동방향 추적할 때 사용
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        // 각 프레임에서 터치 사이의 벡터 거리 구함
        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude; //magnitude는 두 점간의 거리 비교(벡터)
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        // 거리 차이 구함(거리가 이전보다 크면(마이너스가 나오면)손가락을 벌린 상태_줌인 상태)
        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        cam.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, zoomMinValue, zoomMaxValue);
    }

    // 3초 후 ChaseState 변경
    IEnumerator ChangeChaseState(ChaseState cs)
    {
        yield return new WaitForSeconds(3f);

        chaseState = cs;
    }

    // 타겟 추적 시작
    public void StartChaseTarget(Transform targetTransform)                               
    {
        GameManager.Instance.ShowSantaPanel();

        chasingTarget = targetTransform;

        chaseState = ChaseState.chaseStart;

        StartCoroutine(ChangeChaseState(ChaseState.chasing));       // 3초 후 chasing 상태로 변경
    }

    // 타겟 추적 종료
    public void EndChaseTarget()
    {
        chaseState = ChaseState.chaseEnd;

        chasingTarget = null;

        StartCoroutine(ChangeChaseState(ChaseState.noChase));       // 3초 후 noChase 상태로 변경
    }

    // 타겟 추적 시작 시 카메라 세팅
    void SetStartChaseCam()     
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, chasingFieldOfView, Time.deltaTime);          // 타겟을 향해 줌인

        Quaternion chasingCamRot = Quaternion.Euler(new Vector3(chasingCamRotateX, cam.transform.rotation.y, cam.transform.rotation.z));    // 카메라의 x축을 조정
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, chasingCamRot, Time.deltaTime);
    }

    // 타깃 추적 종료 시 카메라를 기본값으로 세팅
    void SetEndChaseCam()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, basicFieldOfView, Time.deltaTime);

        Quaternion chasingCamRot = Quaternion.Euler(new Vector3(basicCamRotateX, cam.transform.rotation.y, cam.transform.rotation.z));    // 카메라의 x축을 조정
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
            SetStartChaseCam();             // 타겟 추적 시작 시 카메라 세팅
        }
        else if (chaseState == ChaseState.chaseEnd)
        {
            SetEndChaseCam();               // 타겟 추적 종료 시 카메라 세팅
        }
    }

    // Update is called once per frame
    void Update()
    {
        CamRotate();

        SetChaseCam();      // 타켓 추적 시작/종료 시 카메라 세팅

        if (canMove)
        {
            if (Input.touchCount == 1)  // 한 손가락으로 터치 시 
            {
                if (chasingTarget == null)          // 추적할 타겟이 없으면
                    CamMove();                      // 카메라 이동
                else                                // 추적할 타겟이 있으면 
                    CamRotate();                    // 오브젝트를 중심으로 카메라 회전
            }

            if (Input.touchCount == 2) // 두 손가락으로 터치 시
            {
                Zoom();                 // 줌 인/아웃
            }
        }
    }

    private void FixedUpdate()
    {
        // 추적 시작 후 오브젝트를 따라다님
        if (chasingTarget != null)
        {
            Vector3 targetPos = new Vector3(chasingTarget.position.x, chasingTarget.position.y, chasingTarget.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime);
        }
    }
}

