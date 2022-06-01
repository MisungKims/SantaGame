/**
 * @brief 토끼 주민창 UI
 * @author 김미성
 * @date 22-06-01
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenPanel : MonoBehaviour
{
    #region 변수
    public RabbitCitizen rabbitCitizen;   // 현재 토끼 주민창의 토끼
    #endregion

    #region 유니티 함수
    private void OnEnable()
    {
        // 옷 UI 슬롯들의 rabbitCitizen을 현재 토끼로 변경
        for (int i = 0; i < ClothesManager.Instance.clothesSlots.Count; i++)
        {
            ClothesManager.Instance.clothesSlots[i].rabbitCitizen = rabbitCitizen;
            ClothesManager.Instance.clothesSlots[i].gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < ClothesManager.Instance.clothesSlots.Count; i++)
        {
            ClothesManager.Instance.clothesSlots[i].gameObject.SetActive(false);
            ClothesManager.Instance.clothesSlots[i].rabbitCitizen = null;
        }
    }
    #endregion
}
