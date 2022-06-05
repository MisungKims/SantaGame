/**
 * @brief °Ç¹° °ñµå È¹µæ ¹öÆ°
 * @author ±è¹Ì¼º
 * @date 22-04-27
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGoldButtonRay : ButtonRaycast
{
    [SerializeField]
    private Building building;

    protected override void Touched()
    {
        base.Touched();

        soundManager.PlaySoundEffect(ESoundEffectType.getGoldButton);      // È¿°úÀ½

        building.ClickGetBtn();
    }
}
