/**
 * @brief 옷가게 입장 버튼
 * @author 김미성
 * @date 22-06-02
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothesStoreButtonRay : ButtonRaycast
{
    protected override void Touched()
    {
        base.Touched();
        StartCoroutine(OpenPanel());
    }

    IEnumerator OpenPanel()
    {
        yield return new WaitForSeconds(0.13f);

        UIManagerInstance().ShowClothesStore();
        SoundManagerInstance().PlaySoundEffect(ESoundEffectType.uiButton);      // 효과음
    }
}
