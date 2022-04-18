/**
 * @brief ������ ����
 * @author ��̼�
 * @date 22-04-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ����
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
    /// ������ ������ ���� ���� ȹ��
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
    /// ������ �̸����� ERewardType�� ��ȯ
    /// </summary>
    /// <param name="rewardName"></param>
    public static ERewardType StringToRewardType(string rewardName)
    {
        switch (rewardName)
        {
            case "���":
                return ERewardType.gold;
                
            case "����":
                return ERewardType.dia;
              
            case "���� ����":
                return ERewardType.puzzle;
                
            case "���� �ǻ� ����":
                return ERewardType.clothesBox;
              
            case "���":
                return ERewardType.carrot;

            default:
                return ERewardType.gold;
        }
    }
}
