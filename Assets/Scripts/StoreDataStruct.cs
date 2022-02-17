using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct StoreDataStruct
{
    public string buildingName;            // 건물 이름
    public int unlockLevel;                // 잠금 해제 가능 레벨
    public int second;                     // 몇 초 마다 증가할 것인지
    public float multiplyBuildingPrice;    // 업그레이드 후 건물 가격 증가 배율
    public int buildingPrice;              // 건물 가격 
    public float multiplyGold;             // 업그레이드 후 플레이어 돈 증가 배율
    public int incrementGold;              // 플레이어의 돈 증가량
    public string santaName;               // 산타 이름
    public float multiplySantaPrice;       // 업그레이드 후 건물 가격 증가 배율
    public int santaPrice;                 // 건물 가격 
    public string desc;                    // 건물의 설명

    public StoreDataStruct(string buildingName, int unlockLevel, int second, float multiplyBuildingPrice, int buildingPrice, float multiplyGold, int incrementGold, string santaName, float multiplySantaPrice, int santaPrice, string desc)
    {
        this.buildingName = buildingName;
        this.unlockLevel = unlockLevel;
        this.second = second;
        this.multiplyBuildingPrice = multiplyBuildingPrice;
        this.buildingPrice = buildingPrice;
        this.multiplyGold = multiplyGold;
        this.incrementGold = incrementGold;
        this.santaName = santaName;
        this.multiplySantaPrice = multiplySantaPrice;
        this.santaPrice = santaPrice;
        this.desc = desc;
    }
}