/**
 * @brief 선물 전달 게임에서 생성될 오브젝트
 * @author 김미성
 * @date 22-06-04
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryGameObject : MonoBehaviour
{
    #region 변수
    protected float moveSpeed = 500f;

    public EObjectFlag flag;

    Vector3 startPos = new Vector3(0, 0, 0);

    // 캐싱
    protected  DeliveryGameManager deliveryGameManager;
    protected ObjectPoolingManager objectPoolingManager;
    #endregion

    #region 유니티 함수
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

    #region 코루틴
    /// <summary>
    /// 10초 뒤 비활성화
    /// </summary>
    /// <returns></returns>
    IEnumerator Dissapear()
    {
        yield return new WaitForSeconds(10f);

        ObjectPoolingManagerInstance().Set(this.gameObject, flag);
    }
    #endregion

    #region 함수
    /// <summary>
    /// DeliveryGameManager 인스턴스 반환
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
    /// ObjectPoolingManager 인스턴스 반환
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
