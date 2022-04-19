/**
 * @brief 상점 구조체
 * @author 김미성
 * @date 22-04-19
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store
{
    public int index;
    public string desc;                    // 건물의 설명
    public int unlockLevel;                // 잠금 해제 가능 레벨
    public int second;                     // 몇 초 마다 골드가 증가할 것인지

    public string buildingName;                 // 건물 이름
    public int buildingLevel;
    public string buildingPrice;              // 건물 가격 
    public string incrementGold;              // 돈 증가량
    
    public string santaName;               // 산타 이름
    public string santaPrice;               // 산타 가격 
    public int santaLevel;
    public int santaEfficiency;            // 알바 효율

    public string prerequisites;
}
