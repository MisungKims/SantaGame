/**
 * @brief �ʰ��� �� �䳢
 * @author ��̼�
 * @date 22-06-02
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitModel : MonoBehaviour
{
    #region MyRegion
    public Clothes clothes = null;      // �ֹ��� ��(�ڵ���)

    private GameObject clothesObj;      // �� ������Ʈ

    public Transform clothesParent;    // �� ������Ʈ�� �θ�(���̶�Ű)

    private bool isWearing = false;     // ���� �԰� �ִ���?

    // ȸ��
    private float moveSpeed = 50f;
    private Vector2 currentPos, previousPos;
    private Vector3 movePos;
    private bool canRotate = false;
    #endregion

    #region ����Ƽ �Լ�
    private void OnEnable()
    {
        PutOff();
    }
    
    private void Update()
    {
        Rotate();
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// ���콺 Ȥ�� ��ġ�� ���� ȸ����Ŵ
    /// </summary>
    public void Rotate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            previousPos = Input.mousePosition;      // ��ġ�� ���� ���� �� ��ġ ����

            // Ư�� ���� �ȿ����� ȸ���� �� �ֵ���
            if (previousPos.x >= 1139f && previousPos.x <= 1902f && previousPos.y >= 141f && previousPos.y <= 887f)
            {
                canRotate = true;
            }
            else canRotate = false;
        }
        else if (Input.GetMouseButton(0) && canRotate)
        {
            currentPos = Input.mousePosition;

            movePos = previousPos - currentPos;

            Vector3 pos = transform.eulerAngles + movePos;
            pos.x = 0;
            pos.z = 0;

            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, pos, Time.deltaTime * moveSpeed);

            previousPos = Input.mousePosition;
        }
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="clothes">���� ��</param>
    public bool PutOn(Clothes clothes)
    {
        if (clothes != null)
        {
            isWearing = true;

            this.clothes = clothes;

            clothesObj = ObjectPoolingManager.Instance.Get(clothes.flag, clothesParent);

            clothesObj.layer = 7;           // UI ī�޶� ���������� layer�� ����
            for (int i = 0; i < clothesObj.transform.childCount; i++)
            {
                clothesObj.transform.GetChild(i).gameObject.layer = 7;
            }

            // ������Ʈ�� Transform ����
            clothesObj.transform.localPosition = clothes.pos;
            clothesObj.transform.localEulerAngles = clothes.rot;
            clothesObj.transform.localScale = clothes.scale;

            return true;
        }

        return false;
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    public void PutOff()
    {
        if (isWearing)
        {
            isWearing = false;

            clothesObj.layer = 0;

            ObjectPoolingManager.Instance.Set(clothesObj, clothes.flag);    // ���� ������Ʈ Ǯ�� ��ȯ

            clothes = null;
        }
    }
    #endregion
}
