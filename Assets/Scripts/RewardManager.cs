/**
 * @brief 보상을 관리
 * @author 김미성
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    // 싱글톤
    private static RewardManager instance;
    public static RewardManager Instance
    {
        get { return instance; }
    }

    public Image[] rewardImages;                   // 보상 이미지

    // 캐싱
   
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

    }

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
                PuzzleManager.Instance.GetPiece(EPuzzle.rcCar, 1);      /// TODO: 추후 변경
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
