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
    #region MyRegion
    // 싱글톤
    private static RewardManager instance;
    public static RewardManager Instance
    {
        get { return instance; }
    }

    public Sprite[] rewardImages;                   // 보상 이미지
    #endregion

    #region 유니티 함수
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }
    #endregion

    #region 함수
    /// <summary>
    /// 보상의 종류에 따라 보상 획득
    /// </summary>
    public static void GetReward(ERewardType reward, string amount)
    {
        switch (reward)
        {
            case ERewardType.gold:
                GameManager.Instance.MyGold += GoldManager.UnitToBigInteger(amount);
                break;

            case ERewardType.dia:
                GameManager.Instance.MyDia += int.Parse(amount);
                break;

            case ERewardType.puzzle:
                int iAmount = int.Parse(amount);
                if (iAmount > 1)
                {
                    PuzzleManager.Instance.GetManyRandomPiece(iAmount);
                }
                else
                {
                    PuzzleManager.Instance.GetRandomPuzzle();
                }
                break;

            case ERewardType.clothesBox:
                ClothesManager.Instance.GetRandomClothes();
                break;

            case ERewardType.carrot:
                GameManager.Instance.MyCarrots += GoldManager.UnitToBigInteger(amount);
                break;

            default:
                break;
        }
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

            case "다이아":
                return ERewardType.dia;

            case "퍼즐":
                return ERewardType.puzzle;

            case "랜덤 상자":
                return ERewardType.clothesBox;

            case "당근":
                return ERewardType.carrot;

            default:
                return ERewardType.gold;
        }
    }

    /// <summary>
    /// 랜덤 보상을 반환
    /// </summary>
    /// <returns></returns>
    public static ERewardType RandomReward()
    {
        int randType = Random.Range(0, 5);

        return (ERewardType)randType;
    }
    #endregion
}
