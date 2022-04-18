/**
 * @brief 보상을 관리
 * @author 김미성
 * @date 22-04-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 보상의 종류
/// </summary>
public enum ERewardType
{
    gold,
    dia,
    puzzle,
    clothesBox,
    carrot
}

public class RewardManager : MonoBehaviour
{
    /// <summary>
    /// 보상의 종류에 따라 보상 획득
    /// </summary>
    public static void GetReward(ERewardType reward, string amount)
    {
        //Debug.Log(GameManager.Instance.MyGold);
        switch (reward)
        {
            case ERewardType.gold:
                GameManager.Instance.MyGold += GoldManager.UnitToBigInteger(amount);
                break;

            case ERewardType.dia:
                GameManager.Instance.MyDia += int.Parse(amount);
                break;

            case ERewardType.puzzle:
                break;

            case ERewardType.clothesBox:
                break;

            case ERewardType.carrot:
                GameManager.Instance.MyCarrots += GoldManager.UnitToBigInteger(amount);
                break;

            default:
                break;
        }
        //Debug.Log(GameManager.Instance.MyGold);
    }

    /// <summary>
    /// 보상의 이름으로 ERewardType을 반환
    /// </summary>
    /// <param name="rewardName"></param>
    public static ERewardType StringToRewardType(string rewardName)
    {
        switch (rewardName)
        {
            case "골드":
                return ERewardType.gold;
                
            case "보석":
                return ERewardType.dia;
              
            case "퍼즐 조각":
                return ERewardType.puzzle;
                
            case "랜덤 의상 상자":
                return ERewardType.clothesBox;
              
            case "당근":
                return ERewardType.carrot;

            default:
                return ERewardType.gold;
        }
    }
}
