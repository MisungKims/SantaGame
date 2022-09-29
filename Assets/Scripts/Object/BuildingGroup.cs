using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGroup : MonoBehaviour
{
    public static BuildingGroup instance;

    public List<Building> BuildingList = new List<Building>();

    public void Awake()
    {
        instance = this;
    }
}
