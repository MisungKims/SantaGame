using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitModel : MonoBehaviour
{
    public Clothes clothes = null;      // �ֹ��� ��(�ڵ���)

    private GameObject clothesObj;      // �� ������Ʈ

    public Transform clothesParent;    // �� ������Ʈ�� �θ�

    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="clothes">���� ��</param>
    public bool PutOn(Clothes clothes)
    {
        if (clothes != null)
        {
            this.clothes = clothes;

            clothesObj = ObjectPoolingManager.Instance.Get(clothes.flag, clothesParent);

            clothesObj.layer = 7;

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
        clothesObj.layer = 0;
        ObjectPoolingManager.Instance.Set(clothesObj, clothes.flag);
        clothes = null;
    }
}
