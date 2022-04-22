/**
 * @brief ������ ����
 * @author ��̼�
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    // �̱���
    private static RewardManager instance;
    public static RewardManager Instance
    {
        get { return instance; }
    }

    public Image[] rewardImages;                   // ���� �̹���

    // ĳ��
   
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

    }

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
                PuzzleManager.Instance.GetPiece(EPuzzle.rcCar, 1);      /// TODO: ���� ����
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
