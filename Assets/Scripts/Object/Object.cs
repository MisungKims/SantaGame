/**
 * @brief 오브젝트(산타, 건물) 구조체
 * @author 김미성
 * @date 22-04-26
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object
{
    public string desc;                    // 건물의 설명
    public int unlockLevel;                // 잠금 해제 가능 레벨
    public int second;                     // 몇 초 마다 골드가 증가할 것인지

    public string buildingName;                 // 건물 이름
    public int buildingLevel;
    public string buildingPrice;              // 건물 가격 
    public float multiplyBuildingPrice;        // 업그레이드 후 건물 가격 증가 배율
    public string incrementGold;              // 돈 증가량
    public Sprite buildingSprite;       // 빌딩 이미지
    
    public string santaName;               // 산타 이름
    public int santaLevel;
    public string santaPrice;               // 산타 가격
    public float multiplySantaPrice;       // 업그레이드 후 산타 가격 증가 배율
    public int santaEfficiency;            // 알바 효율
    public Sprite santaSprite;       // 산타 이미지

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
