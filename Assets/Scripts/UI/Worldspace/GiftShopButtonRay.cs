/**
 * @brief 선물 가게 입장 버튼
 * @author 김미성
 * @date 22-05-24
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftShopButtonRay : ButtonRaycast
{
    protected override void Touched()
    {
        base.Touched();
        StartCoroutine(OpenPanel());
    }

    IEnumerator OpenPanel()
    {
        yield return new WaitForSeconds(0.13f);

        soundManager.PlaySoundEffect(ESoundEffectType.uiButton);      // 효과음

        UIManagerInstance().ShowGiftShopPanel();
    }
}
