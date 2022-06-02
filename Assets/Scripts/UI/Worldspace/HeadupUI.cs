/**
 * @brief 오브젝트의 위에 있는 UI
 * @author 김미성
 * @date 22-05-14
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadupUI : MonoBehaviour
{
    [SerializeField]
    Transform hostObject;

    [SerializeField]
    float height = 8f;

    float Distance;

    Canvas canvas;

    Camera cam;

    [SerializeField]
    private bool isUseDistance = true;

    void Awake()
    {
        cam = CameraMovement.Instance.cam;
        canvas = this.transform.parent.GetComponent<Canvas>();

        // 위치 설정
        Vector3 newPos = hostObject.position;
        newPos.y += height;
        this.transform.position = newPos;

        if (isUseDistance)
        {
            StartCoroutine(CamDistance());
        }
    }


    /// <summary>
    /// 메인 카메라와의 거리를 계산하여 거리가 가까우면 보이지 않게 함
    /// </summary>
    /// <returns></returns>
    IEnumerator CamDistance()
    {
        while (true)
        {
            Distance = Vector3.Distance(cam.transform.position, this.transform.position);
            if (Distance < 17)
            {
                canvas.enabled = false;
            }
            else
            {
                canvas.enabled = true;
            }
            yield return null;
        }
    }
}
