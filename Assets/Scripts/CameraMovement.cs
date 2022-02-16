using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Move")]
    public float speed = 1;
    public Vector2 currentPos, previousPos;
    public Vector3 movePos;

    [Header("Zoom")]
    public Camera camera;
    public float perspectiveZoomSpeed = 0.5f;  //줌인,줌아웃할때 속도(perspective모드 용)      

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

            movePos = (Vector3)(previousPos - currentPos) * speed;  // 이전 값과 현재 값의 벡터 거리를 구하여

            //movePos.y = 8;
            //movePos.z = 0;
            //transform.position = Vector3.Lerp(transform.position, mousePos, 0.2f);

            this.transform.Translate(movePos);                      // 카메라를 이동 시킴

            previousPos = touch.position - touch.deltaPosition;
        }
        else if (touch.phase == TouchPhase.Ended)
        {
        }
    }

    void Zoom()
    {
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

        camera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
        camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, 0.1f, 179.9f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1)  // 한 손가락으로 터치 시 
        {       
            CamMove();              // 카메라 이동
        }

        if (Input.touchCount == 2) // 두 손가락으로 터치 시
        {
            Zoom();                 // 줌 인/아웃
        }
    }
}

