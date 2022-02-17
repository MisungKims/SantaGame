using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Move")]
    public float speed = 0.06f;
    public Vector2 currentPos, previousPos;
    public Vector3 movePos;
    public bool canMove = true;

    [Header("Zoom")]
    public Camera cam;
    public float perspectiveZoomSpeed = 0.09f;  //����,�ܾƿ��Ҷ� �ӵ�   
    public float zoomMinValue = 10f;
    public float zoomMaxValue = 130f;

    public void SetCanMove(bool move)
    {
        canMove = move;
    }

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

            movePos = (Vector3)(previousPos - currentPos) * speed;  // ���� ���� ���� ���� ���� �Ÿ��� ���Ͽ�

            this.transform.Translate(movePos);                      // ī�޶� �̵� ��Ŵ

            previousPos = touch.position - touch.deltaPosition;
        }
        else if (touch.phase == TouchPhase.Ended)
        {
        }
    }

    void Zoom()
    {
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

    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {
            if (Input.touchCount == 1)  // �� �հ������� ��ġ �� 
            {
                CamMove();              // ī�޶� �̵�
            }

            if (Input.touchCount == 2) // �� �հ������� ��ġ ��
            {
                Zoom();                 // �� ��/�ƿ�
            }
        }
    }
}

