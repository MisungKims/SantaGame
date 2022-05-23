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

        UIManager.Instance.SetisOpenPanel(true);
        UIManager.Instance.giftShopPanel.SetActive(true);
    }
}
