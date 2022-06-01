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

        UIManager.Instance.ShowClothesStore();
    }
}
