/**
 * @brief ²Þ Å½Çè°ü ÀÔÀå ¹öÆ°
 * @author ±è¹Ì¼º
 * @date 22-04-27
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamExplorerButtonRay : ButtonRaycast
{
    protected override void Touched()
    {
        base.Touched();
        StartCoroutine(OpenPanel());
    }

    IEnumerator OpenPanel()
    {
        yield return new WaitForSeconds(0.13f);

        UIManagerInstance().SetisOpenPanel(true);
        UIManagerInstance().puzzlePanel.gameObject.SetActive(true);
    }
}
