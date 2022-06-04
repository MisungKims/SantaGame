/**
 * @brief ������Ʈ�� ���� �ִ� UI
 * @author ��̼�
 * @date 22-05-14
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadupUI : MonoBehaviour
{
    #region ����
    public Transform hostObject;    // �پ����� host

    [SerializeField]
    float height = 8f;          // hostObject�� ���������� �Ÿ�

    float Distance;

    Canvas canvas;

    Camera cam;

    [SerializeField]
    private bool isUseDistance = true;      // ī�޶���� �Ÿ��� ����� ������?
    #endregion

    #region ����Ƽ �Լ�
    void Awake()
    {
        cam = CameraMovement.Instance.cam;
        canvas = this.transform.parent.GetComponent<Canvas>();

        // ��ġ ����
        Vector3 newPos = hostObject.position;
        newPos.y += height;
        this.transform.position = newPos;

        if (isUseDistance)
        {
            StartCoroutine(CamDistance());
        }
    }
    #endregion

    #region �ڷ�ƾ
    /// <summary>
    /// ���� ī�޶���� �Ÿ��� ����Ͽ� �Ÿ��� ������ ������ �ʰ� ��
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
                if (cam.fieldOfView < 40f)
                {
                    canvas.enabled = false;
                }
                else
                {
                    canvas.enabled = true;
                }
            }
            yield return null;
        }
    }
    #endregion
}
