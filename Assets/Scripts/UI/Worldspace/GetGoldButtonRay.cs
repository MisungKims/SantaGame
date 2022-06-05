/**
 * @brief �ǹ� ��� ȹ�� ��ư
 * @author ��̼�
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

        soundManager.PlaySoundEffect(ESoundEffectType.getGoldButton);      // ȿ����

        building.ClickGetBtn();
    }
}
