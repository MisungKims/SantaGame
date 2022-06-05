/**
 * @brief ������Ʈ(��Ÿ, �ǹ�) ����ü
 * @author ��̼�
 * @date 22-04-26
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object
{
    public string desc;                    // �ǹ��� ����
    public int unlockLevel;                // ��� ���� ���� ����
    public int second;                     // �� �� ���� ��尡 ������ ������

    public string buildingName;                 // �ǹ� �̸�
    public int buildingLevel;
    public string buildingPrice;              // �ǹ� ���� 
    public float multiplyBuildingPrice;        // ���׷��̵� �� �ǹ� ���� ���� ����
    public string incrementGold;              // �� ������
    public Sprite buildingSprite;       // ���� �̹���
    
    public string santaName;               // ��Ÿ �̸�
    public int santaLevel;
    public string santaPrice;               // ��Ÿ ����
    public float multiplySantaPrice;       // ���׷��̵� �� ��Ÿ ���� ���� ����
    public int santaEfficiency;            // �˹� ȿ��
    public Sprite santaSprite;       // ��Ÿ �̹���

    public string prerequisites;

    public Object(string desc, int unlockLevel, int second, string buildingName, int buildingLevel, string buildingPrice, float multiplyBuildingPrice, string incrementGold, Sprite buildingSprite, string santaName, int santaLevel, string santaPrice, float multiplySantaPrice, int santaEfficiency, Sprite santaSprite, string prerequisites)
    {
        this.desc = desc;
        this.unlockLevel = unlockLevel;
        this.second = second;
        this.buildingName = buildingName;
        this.buildingLevel = buildingLevel;
        this.buildingPrice = buildingPrice;
        this.multiplyBuildingPrice = multiplyBuildingPrice;
        this.incrementGold = incrementGold;
        this.buildingSprite = buildingSprite;
        this.santaName = santaName;
        this.santaLevel = santaLevel;
        this.santaPrice = santaPrice;
        this.multiplySantaPrice = multiplySantaPrice;
        this.santaEfficiency = santaEfficiency;
        this.santaSprite = santaSprite;
        this.prerequisites = prerequisites;
    }
}
