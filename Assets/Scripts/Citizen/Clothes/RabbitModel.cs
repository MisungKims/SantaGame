/**
 * @brief 옷가게 모델 토끼
 * @author 김미성
 * @date 22-06-02
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitModel : MonoBehaviour
{
    #region MyRegion
    public Clothes clothes = null;      // 주민의 옷(코디템)

    private GameObject clothesObj;      // 옷 오브젝트

    public Transform clothesParent;    // 옷 오브젝트의 부모(하이라키)

    private bool isWearing = false;     // 옷을 입고 있는지?

    // 회전
    private float moveSpeed = 50f;
    private Vector2 currentPos, previousPos;
    private Vector3 movePos;
    private bool canRotate = false;
    #endregion

    #region 유니티 함수
    private void OnEnable()
    {
        PutOff();
    }
    
    private void Update()
    {
        Rotate();
    }
    #endregion

    #region 함수
    /// <summary>
    /// 마우스 혹은 터치로 모델을 회전시킴
    /// </summary>
    public void Rotate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            previousPos = Input.mousePosition;      // 터치에 대한 이전 값 위치 저장

            // 특정 범위 안에서만 회전할 수 있도록
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
    /// 옷을 입음
    /// </summary>
    /// <param name="clothes">입힐 옷</param>
    public bool PutOn(Clothes clothes)
    {
        if (clothes != null)
        {
            isWearing = true;

            this.clothes = clothes;

            clothesObj = ObjectPoolingManager.Instance.Get(clothes.flag, clothesParent);

            clothesObj.layer = 7;           // UI 카메라에 보여지도록 layer를 변경
            for (int i = 0; i < clothesObj.transform.childCount; i++)
            {
                clothesObj.transform.GetChild(i).gameObject.layer = 7;
            }

            // 오브젝트의 Transform 설정
            clothesObj.transform.localPosition = clothes.pos;
            clothesObj.transform.localEulerAngles = clothes.rot;
            clothesObj.transform.localScale = clothes.scale;

            return true;
        }

        return false;
    }

    /// <summary>
    /// 옷을 벗음
    /// </summary>
    public void PutOff()
    {
        if (isWearing)
        {
            isWearing = false;

            clothesObj.layer = 0;

            ObjectPoolingManager.Instance.Set(clothesObj, clothes.flag);    // 옷을 오브젝트 풀에 반환

            clothes = null;
        }
    }
    #endregion
}
