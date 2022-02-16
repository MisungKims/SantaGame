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
    public float perspectiveZoomSpeed = 0.5f;  //����,�ܾƿ��Ҷ� �ӵ�(perspective��� ��)      

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

            //movePos.y = 8;
            //movePos.z = 0;
            //transform.position = Vector3.Lerp(transform.position, mousePos, 0.2f);

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

        camera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
        camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, 0.1f, 179.9f);
    }

    // Update is called once per frame
    void Update()
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

