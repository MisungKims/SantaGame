using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadupUI : MonoBehaviour
{
    [SerializeField]
    Transform hostObject;

    [SerializeField]
    float height = 8f;

    private void Update()
    {
        Vector3 newPos = hostObject.position;
        newPos.y += height;
       
        this.transform.position = newPos;
    }
}
