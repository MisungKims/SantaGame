using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Clothes : MonoBehaviour
{
    public EClothesFlag flag;
    public string clothesName;
    public Sprite image;
    public ClothesInfo clothesInfo;

    public Vector3 pos;
    public Vector3 rot;

    //public Clothes(EClothesFlag flag, string clothesName, Sprite image, ClothesInfo clothesInfo)
    //{
    //    this.flag = flag;
    //    this.clothesName = clothesName;
    //    this.image = image;
    //    this.clothesInfo = clothesInfo;
    //}


    //public int totalAmount;
    //public int wearingCount;

    //public Clothes(EClothesFlag flag, string clothesName, Sprite image, int totalAmount, int wearingCount)
    //{
    //    this.flag = flag;
    //    this.clothesName = clothesName;
    //    this.image = image;
    //    this.totalAmount = totalAmount;
    //    this.wearingCount = wearingCount;
    //}


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
    hat
}