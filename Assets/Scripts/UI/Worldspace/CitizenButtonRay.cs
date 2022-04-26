using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenButtonRay : ButtonRaycast
{
    public RabbitCitizen citizen;

    protected override void Touched()
    {
        base.Touched();

        citizen.isTouch = true;
    }
}
