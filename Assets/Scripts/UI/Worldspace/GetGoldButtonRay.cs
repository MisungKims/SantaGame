using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGoldButtonRay : ButtonRaycast
{
    [SerializeField]
    private Building building;

    protected override void Touched()
    {
        building.ClickGetBtn();
    }
}
