/**
 * @brief ��ü�� ���� ��ư
 * @author ��̼�
 * @date 22-05-24
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostOfficeButtonRay : ButtonRaycast
{
    protected override void Touched()
    {
        base.Touched();
        StartCoroutine(OpenPanel());
    }

    IEnumerator OpenPanel()
    {
        yield return new WaitForSeconds(0.13f);

        soundManager.PlaySoundEffect(ESoundEffectType.uiButton);      // ȿ����

        UIManagerInstance().SetisOpenPanel(true);
        UIManagerInstance().postOfficePanel.SetActive(true);
    }
}
