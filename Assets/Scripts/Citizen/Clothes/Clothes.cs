using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Clothes
{
    public EClothesFlag flag;
    public string clothesName;
    public int price;
    public Sprite image;
    public GameObject clothesPrefabs;
    public int totalAmount;
    public int wearingCount;


    //public ClothesInfo clothesInfo;

    public Vector3 pos;
    public Vector3 rot;
    public Vector3 scale;
}
[System.Serializable]
public class ClothesInfo
{
    public int totalAmount;
    public int wearingCount;

    public ClothesInfo(int totalAmount, int wearingCount)
    {
        this.totalAmount = totalAmount;
        this.wearingCount = wearingCount;
    }
}
public enum EClothesFlag
{
    strawHat,
    kindergardenHat
}