/**
 * @brief 옷의 정보
 * @author 김미성
 * @date 22-06-02
 */

using UnityEngine;

[System.Serializable]
public class Clothes
{
    public EClothesFlag flag;               // 어떤 옷인지?
    public string clothesName;              // 옷의 이름
    public int price;                       // 옷의 가격
    public Sprite image;                    // 옷의 이미지
    public GameObject clothesPrefabs;       // 옷의 오브젝트
    public ClothesInfo clothesInfo;         // 저장해야할 옷의 정보

    // 토끼에 붙어야할 위치
    public Vector3 pos;
    public Vector3 rot;
    public Vector3 scale;
}

/// <summary>
/// 저장해야 할 옷의 정보
/// </summary>
[System.Serializable]
public class ClothesInfo
{
    public int totalAmount;         // 총 수량
    public int wearingCount;        // 해당 옷을 몇마리가 입고 있는지

    public ClothesInfo(int totalAmount, int wearingCount)
    {
        this.totalAmount = totalAmount;
        this.wearingCount = wearingCount;
    }
}

public enum EClothesFlag
{
    kindergardenHat,
    crown,
    santaHat,
    flower,
    glasses,
    pirateHat,
    propellerHat,
    ribbon,
    sherifHat
}