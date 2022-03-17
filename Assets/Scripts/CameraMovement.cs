using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    #region 전역 변수
    private static CameraMovement instance = null;
    public static CameraMovement Instance
    {
        get
        {
            return instance;
        }
    }

    public Vector3 distance;                // 항상 떨어져있어야 할 거리

    [Header("---------- Chase")]

    [SerializeField]
    public Transform chasingSanta = null;          // 추적할 타겟

    public Transform chasingBuilding = null;

    public Vector3 buildingDistance;

    [SerializeField]
    private float basicFieldOfView = 60f;            // 카메라의 기본  field of view
    [SerializeField]
    private float chasingSantaFieldOfView = 15f;          // 산타 추적 시 카메라의 field of view
    [SerializeField]
    private float chasingBuildingFieldOfView = 25f;          // 건물 추적 시 카메라의 field of view
    [SerializeField]
    private float basicRotateX = 30f;               // 카메라의 기본  x축
    [SerializeField]
    private float chasingRotateX = 19.7f;         // 추적 시 카메라의 x축

    enum ChaseState { chaseStart, chasing, chaseEnd, noChase };         // 카메라의 상태
    ChaseState chaseState = ChaseState.noChase;

    [Header("---------- Move")]
    public float moveSpeed = 0.06f;
    public Vector2 currentPos, previousPos;
    public Vector3 movePos;
    public bool canMove = true;

    [Header("---------- Rotate")]
    public float rotateSpeed = 1;

    public float minRotateX = -10f;                // 회전 시 x축의 Min 값
    public float maxRotateX = 20f;                 // 회전 시 x축의 Max 값

    public float minRotateY = -55f;                // 회전 시 y축의 Min 값
    public float maxRotateY = 55f;                 // 회전 시 y축의 Max 값

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
    public float perspectiveZoomSpeed = 5f;  //줌인,줌아웃할때 속도   
    public float zoomMinValue = 6f;
    public float zoomMaxValue = 70f;
    public float sensitive = 1f;
    #endregion

    #region 사용자 정의 함수


    #region 카메라 움직임
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

            cam.transform.Translate(movePos);                      // 카메라를 이동 시킴
            //cam.transform.position = Vector3.Lerp(cam.transform.position, movePos, Time.deltaTime);

            previousPos = touch.position - touch.deltaPosition;
        }
        else if (touch.phase == TouchPhase.Ended)
        {

        }
    }

    // 카메라 회전
    void CamRotate()
    {
        //// 마우스로 회전
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

        //모바일에서 터치로 회전
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

    // 카메라 줌 인/줌 아웃
    void Zoom()
    {
        if (chaseState == ChaseState.chaseStart) chaseState = ChaseState.chasing;
        else if (chaseState == ChaseState.chaseEnd) chaseState = ChaseState.noChase;

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

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, cam.fieldOfView + deltaMagnitudeDiff * perspectiveZoomSpeed, sensitive * Time.deltaTime);
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, zoomMinValue, zoomMaxValue);
    }

    void CameraMove()
    {
        if (canMove)
        {
            if (Input.touchCount == 1)  // 한 손가락으로 터치 시 
            {
                if (chaseState == ChaseState.noChase)          // 추적 중이 아니면
                    CamMove();                                 // 카메라 이동
                else if (chaseState == ChaseState.chasing)      // 추적 중이면
                    CamRotate();                               // 오브젝트를 중심으로 카메라 회전
            }

            else if (Input.touchCount == 2) // 두 손가락으로 터치 시
            {
                Zoom();                 // 줌 인/아웃
            }

            else  // PC에서 확인하기 위함
            {
                if (chaseState == ChaseState.chasing)          // 추적할 타겟이 있으면
                    CamRotate();                    // 오브젝트를 중심으로 카메라 회전
            }
        }
    }
    #endregion

    #region 타깃 추적
    // 3초 후 ChaseState 변경
    IEnumerator ChangeChaseState(ChaseState cs)
    {
        yield return new WaitForSeconds(5f);

        chaseState = cs;
    }

    // 타깃 추적 시작
    public void StartChaseTarget()
    {
        GameManager.Instance.ShowClickObjPanel();               // 산타 정보 창 띄우기

        chaseState = ChaseState.chaseStart;                     // 카메라의 상태 변경

        StartCoroutine(ChangeChaseState(ChaseState.chasing));   // 3초 후 chasing 상태로 변경
    }

    // 타깃 추적 종료
    public void EndChaseTarget()
    {
        GameManager.Instance.HideClickObjPanel();             // 클릭 오브젝트 창 없애기

        if (chasingSanta) chasingSanta = null;
        else if (chasingBuilding) chasingBuilding = null;

        chaseState = ChaseState.chaseEnd;                     // 카메라의 상태 변경 

        StartCoroutine(ChangeChaseState(ChaseState.noChase));       // 3초 후 noChase 상태로 변경
    }

    // 타깃 추적 시작 시 카메라 세팅 (산타가 타깃일 때)
    void SetStartChaseCam()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, chasingSantaFieldOfView, Time.deltaTime);          // 타겟을 향해 줌인

        Quaternion chasingCamRot = Quaternion.Euler(new Vector3(chasingRotateX, cam.transform.rotation.y, cam.transform.rotation.z));    // 카메라의 x축을 조정
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, chasingCamRot, Time.deltaTime);
    }

    // 건물이 타깃일 때는 건물을 향해 줌인
    void SetZoomInBuilding()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, chasingBuildingFieldOfView, Time.deltaTime);          // 타겟을 향해 줌인

        cam.transform.position = Vector3.Lerp(cam.transform.position, chasingBuilding.position + buildingDistance, Time.deltaTime);     // 카메라의 위치 조정

        Quaternion chasingCamRot = Quaternion.Euler(new Vector3(chasingRotateX, cam.transform.rotation.y, cam.transform.rotation.z));    // 카메라의 x축을 조정
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, chasingCamRot, Time.deltaTime);
    }

    // 타깃 추적 종료 시 카메라를 기본값으로 세팅
    void SetEndChaseCam()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, basicFieldOfView, Time.deltaTime);            // 줌 아웃

        // 카메라의 x축을 원래대로
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
                SetStartChaseCam();     // 산타 추적 시작 시 카메라 세팅
            }
            else if(chasingBuilding != null)
            {
                SetZoomInBuilding();    // 건물로 줌인
            }         
        }
        else if (chaseState == ChaseState.chaseEnd)
        {
            SetEndChaseCam();               // 타겟 추적 종료 시 카메라 세팅
        }
    }

    void FollowSanta()
    {
        // 추적 시작 후 산타를 따라다님
        if (chasingSanta != null)
        {
            Vector3 targetPos = chasingSanta.position + distance;
            cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, Time.deltaTime);
        }
    }
    #endregion

    #endregion


    #region 유니티 함수

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
        // 카메라의 상태 지정
        chaseState = ChaseState.noChase;            

        // 카메라의 기본 값 조정
        cam.fieldOfView = basicFieldOfView;

        // 카메라의 기본 각도 조정
        cam.transform.rotation = Quaternion.Euler(new Vector3(basicRotateX, 0, 0));

        // 카메라의 기본 위치 조정
        distance = this.transform.GetChild(1).localPosition;
        cam.transform.position = distance;
    }


    private void LateUpdate()
    {
        CameraMove();       // 카메라의 움직임

        SetChaseCam();      // 타켓 추적 시작/종료 시 카메라 세팅

        FollowSanta();     // 추적 시작 후 오브젝트를 따라다님
    }
    #endregion
}

