/**
 * @brief 선물 전달 게임 버튼
 * @author 김미성
 * @date 22-06-04
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftDeliveryButton : MonoBehaviour
{
    /// <summary>
    /// 선물 전달 버튼 클릭 (인스펙터에서 호출)
    /// </summary>
    public void ClickButton()
    {
        SoundManager.Instance.StopBGM();

        // 선물 전달 게임 오브젝트 활성화
        GameManager.Instance.StartDeliveryGame();
    }
}
