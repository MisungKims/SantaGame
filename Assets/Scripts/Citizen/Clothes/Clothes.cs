/**
 * @brief ���� ����
 * @author ��̼�
 * @date 22-06-02
 */

using UnityEngine;

[System.Serializable]
public class Clothes
{
    public EClothesFlag flag;               // � ������?
    public string clothesName;              // ���� �̸�
    public int price;                       // ���� ����
    public Sprite image;                    // ���� �̹���
    public GameObject clothesPrefabs;       // ���� ������Ʈ
    public ClothesInfo clothesInfo;         // �����ؾ��� ���� ����

    // �䳢�� �پ���� ��ġ
    public Vector3 pos;
    public Vector3 rot;
    public Vector3 scale;
}

/// <summary>
/// �����ؾ� �� ���� ����
/// </summary>
[System.Serializable]
public class ClothesInfo
{
    public int totalAmount;         // �� ����
    public int wearingCount;        // �ش� ���� ����� �԰� �ִ���

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