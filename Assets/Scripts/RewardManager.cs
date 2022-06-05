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
    #region MyRegion
    // �̱���
    private static RewardManager instance;
    public static RewardManager Instance
    {
        get { return instance; }
    }

    public Sprite[] rewardImages;                   // ���� �̹���
    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// ������ ������ ���� ���� ȹ��
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
    /// ������ �̸����� ERewardType�� ��ȯ
    /// </summary>
    /// <param name="rewardName"></param>
    public static ERewardType StringToRewardType(string rewardName)
    {
        switch (rewardName)
        {
            case "���":
                return ERewardType.gold;

            case "���̾�":
                return ERewardType.dia;

            case "����":
                return ERewardType.puzzle;

            case "���� ����":
                return ERewardType.clothesBox;

            case "���":
                return ERewardType.carrot;

            default:
                return ERewardType.gold;
        }
    }

    /// <summary>
    /// ���� ������ ��ȯ
    /// </summary>
    /// <returns></returns>
    public static ERewardType RandomReward()
    {
        int randType = Random.Range(0, 5);

        return (ERewardType)randType;
    }
    #endregion
}
