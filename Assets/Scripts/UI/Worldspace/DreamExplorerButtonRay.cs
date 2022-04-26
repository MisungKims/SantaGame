using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamExplorerButtonRay : ButtonRaycast
{
    protected override void Touched()
    {
        base.Touched();

        UIManager.Instance.puzzlePanel.gameObject.SetActive(true);
    }
}
