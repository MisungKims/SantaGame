/**
 * @brief ���� ���� ���ӿ��� ������ ������Ʈ
 * @author ��̼�
 * @date 22-06-04
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryGameObject : MonoBehaviour
{
    #region ����
    protected float moveSpeed = 500f;

    public EObjectFlag flag;

    Vector3 startPos = new Vector3(0, 0, 0);

    // ĳ��
    protected  DeliveryGameManager deliveryGameManager;
    protected ObjectPoolingManager objectPoolingManager;
    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        deliveryGameManager = DeliveryGameManager.Instance;
        objectPoolingManager = ObjectPoolingManager.Instance;
    }

    protected virtual void OnEnable()
    {
        this.transform.localPosition = startPos;
        StartCoroutine(Dissapear());
    }

    protected virtual void Update()
    {
        if (!DeliveryGameManagerInstance().isEnd)
        {
            this.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        else ObjectPoolingManagerInstance().Set(this.gameObject, flag);
    }
    #endregion

    #region �ڷ�ƾ
    /// <summary>
    /// 10�� �� ��Ȱ��ȭ
    /// </summary>
    /// <returns></returns>
    IEnumerator Dissapear()
    {
        yield return new WaitForSeconds(10f);

        ObjectPoolingManagerInstance().Set(this.gameObject, flag);
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// DeliveryGameManager �ν��Ͻ� ��ȯ
    /// </summary>
    /// <returns></returns>
    DeliveryGameManager DeliveryGameManagerInstance()
    {
        if (!deliveryGameManager)
        {
            deliveryGameManager = DeliveryGameManager.Instance;
        }

        return deliveryGameManager;
    }

    /// <summary>
    /// ObjectPoolingManager �ν��Ͻ� ��ȯ
    /// </summary>
    /// <returns></returns>
    ObjectPoolingManager ObjectPoolingManagerInstance()
    {
        if (!objectPoolingManager)
        {
            objectPoolingManager = ObjectPoolingManager.Instance;
        }

        return objectPoolingManager;
    }
    #endregion
}
