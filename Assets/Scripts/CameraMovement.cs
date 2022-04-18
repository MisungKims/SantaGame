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

    [SerializeField]
    private Transform distanceTransform;
    private Vector3 distance;                // 항상 떨어져있어야 할 거리

    [Header("---------- Chase")]
    private Transform chasingTarget = null;          // 추적할 타겟

    [SerializeField]
    private float basicFieldOfView = 60f;            // 카메라의 기본  field of view
    [SerializeField]
    private float chasingSantaFieldOfView = 10f;          // 산타 추적 시 카메라의 field of view
    [SerializeField]
    private float basicRotateX = 30f;               // 카메라의 기본  x축
    [SerializeField]
    private float chasingRotateX = 23f;         // 추적 시 카메라의 x축

    Vector3 chasingCamAngles;
    Vector3 basicCamAngles;

    enum EChaseState { chasing, noChase };         // 카메라의 상태
    EChaseState chaseState = EChaseState.noChase;

    [Header("---------- Move")]
    private float moveSpeed = 0.06f;
    private Vector2 currentPos, previousPos;
    private Vector3 movePos;
    private bool canMove = true;

    [Header("---------- Rotate")]
    private float rotateSpeed = 1;

    private float minRotateX = -10f;                // 회전 시 x축의 Min 값
    private float maxRotateX = 20f;                 // 회전 시 x축의 Max 값

    private float minRotateY = -55f;                // 회전 시 y축의 Min 값
    private float maxRotateY = 55f;                 // 회전 시 y축의 Max 값

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
    private float perspectiveZoomSpeed = 5f;  //줌인,줌아웃할때 속도   
    private float zoomMinValue = 6f;
    private float zoomMaxValue = 70f;
    private float sensitive = 1f;


    private GameManager gameManager;
    private UIManager uiManager;


    Vector3 targetPos;

    #endregion

    #region 사용자 정의 함수


    #region 카메라 움직임
   

    // 카메라 움직임
    void CamMove()
    {
        chaseState = EChaseState.noChase;

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
        chaseState = EChaseState.noChase;

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

    Vector2 touchZeroPrevPos;
    Vector2 touchOnePrevPos;

    float prevTouchDeltaMag;
    float touchDeltaMag;

    float deltaMagnitudeDiff;

    // 카메라 줌 인/줌 아웃
    void Zoom()
    {
        chaseState = EChaseState.noChase;

        Touch touchZero = Input.GetTouch(0); //첫번째 손가락 터치를 저장
        Touch touchOne = Input.GetTouch(1); //두번째 손가락 터치를 저장

        //터치에 대한 이전 위치값을 각각 저장함
        //처음 터치한 위치(touchZero.position)에서 이전 프레임에서의 터치 위치와 이번 프로임에서 터치 위치의 차이를 뺌
        touchZeroPrevPos = touchZero.position - touchZero.deltaPosition; //deltaPosition는 이동방향 추적할 때 사용
        touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        // 각 프레임에서 터치 사이의 벡터 거리 구함
        prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude; //magnitude는 두 점간의 거리 비교(벡터)
        touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        // 거리 차이 구함(거리가 이전보다 크면(마이너스가 나오면)손가락을 벌린 상태_줌인 상태)
        deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, cam.fieldOfView + deltaMagnitudeDiff * perspectiveZoomSpeed, sensitive * Time.deltaTime);
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, zoomMinValue, zoomMaxValue);
    }

    void CameraMove()
    {
        if (canMove)
        {
            if (Input.touchCount == 1)  // 한 손가락으로 터치 시 
            {
                if (chaseState == EChaseState.noChase)          // 추적 중이 아니면
                    CamMove();                                 // 카메라 이동
                else if (chaseState == EChaseState.chasing)      // 추적 중이면
                    CamRotate();                               // 오브젝트를 중심으로 카메라 회전
            }

            else if (Input.touchCount == 2) // 두 손가락으로 터치 시
            {
                Zoom();                 // 줌 인/아웃
            }

            //else  // PC에서 확인하기 위함
            //{
            //    if (chaseState == ChaseState.chasing)          // 추적할 타겟이 있으면
            //        CamRotate();                    // 오브젝트를 중심으로 카메라 회전
            //}
        }
    }
    #endregion

    #region 타깃 추적
    
    // 카메라가 산타를 따라다님
    public void ChaseSanta(Transform obj)
    {
        chaseState = EChaseState.chasing;    // 카메라의 상태 변경

        chasingTarget = obj;

        // 카메라 조정
        StartCoroutine(ChangeFieldOfView(chasingSantaFieldOfView, chaseState));
        StartCoroutine(ChangeAngles(chasingCamAngles, chaseState));
        StartCoroutine(SetCamPos(distance, false, chaseState));
    }

    // 카메라가 빌딩의 카메라 위치로 이동
    public void ChaseBuilding(Transform obj, Transform distance)
    {
        chaseState = EChaseState.chasing;    // 카메라의 상태 변경

        // 카메라 조정
        StartCoroutine(SetCamPos(obj.position + distance.localPosition, true, chaseState));
        StartCoroutine(ChangeAngles(distance.eulerAngles, chaseState));
    }

    // 타깃 추적 종료
    public void EndChaseTarget()
    {
        chaseState = EChaseState.noChase;                    // 카메라의 상태 변경 

        uiManager.HideClickObjWindow();             // 클릭 오브젝트 창 없애기

        if (chasingTarget)
        {
            chasingTarget = null;
            StartCoroutine(ChangeFieldOfView(basicFieldOfView, chaseState));
        }
       
        // 카메라를 기본값으로 세팅
        StartCoroutine(ChangeAngles(basicCamAngles, chaseState));
        StartCoroutine(SetCamPos(distance, true, chaseState));
    }


    /// <summary>
    /// 카메라의 FieldOfView를 조절하여 타겟을 향해 줌 인/줌 아웃
    /// </summary>
    /// <param name="fieldOfView">조절할 Field Of View</param>
    /// <returns></returns>
    IEnumerator ChangeFieldOfView(float fieldOfView, EChaseState state)
    {
        while (Mathf.Abs(cam.fieldOfView - fieldOfView) > 0.05f && chaseState == state)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fieldOfView, Time.deltaTime * 1.6f);          // 타겟을 향해 줌인

            yield return null;
        }
    }

    /// <summary>
    /// 카메라의 앵글을 조절
    /// </summary>
    /// <param name="angle">조절할 각도</param>
    /// <returns></returns>
    IEnumerator ChangeAngles(Vector3 angle, EChaseState state)
    {
        while (Vector3.Distance(cam.transform.eulerAngles, angle) > 0.05f && chaseState == state)
        {
            cam.transform.eulerAngles = Vector3.Lerp(cam.transform.eulerAngles, angle, Time.deltaTime);

            yield return null;
        }
    }

    /// <summary>
    /// 카메라의 위치를 조정
    /// </summary>
    /// <returns></returns>
    IEnumerator SetCamPos(Vector3 targetPos, bool isFix, EChaseState state)
    {
        // 카메라가 타깃을 따라 움직이지 않을 때
        if(isFix)
        {
            while (Vector3.Distance(cam.transform.position, targetPos) > 0.05f && chaseState == state)
            {
                cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, Time.deltaTime * 1.6f);

                yield return null;
            }
        }
        // 카메라가 타깃을 따라 움직일 때
        else
        {
            while (chaseState == EChaseState.chasing)
            {
                cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos + chasingTarget.position, Time.deltaTime);

                yield return null;
            }
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
        //gameManager = GameManager.Instance;
        uiManager = UIManager.Instance;

        // 카메라의 상태 지정
        chaseState = EChaseState.noChase;            

        // 카메라의 기본 값 조정
        cam.fieldOfView = basicFieldOfView;

        // 카메라의 기본 각도 조정
        basicCamAngles = new Vector3(basicRotateX, 0, 0);
        cam.transform.eulerAngles = basicCamAngles;

        // 추적 시 카메라의 각도 설정
        chasingCamAngles = new Vector3(chasingRotateX, 0, 0);

        // 카메라의 기본 위치 조정
        distance = distanceTransform.localPosition;
        //distance = this.transform.GetChild(1).localPosition;
        cam.transform.position = distance;
    }


    private void LateUpdate()
    {
        CameraMove();       // 카메라의 움직임
    }
    #endregion
}

