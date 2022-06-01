/**
 * @brief �䳢 �ֹ�â UI
 * @author ��̼�
 * @date 22-06-01
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenPanel : MonoBehaviour
{
    #region ����
    public RabbitCitizen rabbitCitizen;   // ���� �䳢 �ֹ�â�� �䳢
    #endregion

    #region ����Ƽ �Լ�
    private void OnEnable()
    {
        // �� UI ���Ե��� rabbitCitizen�� ���� �䳢�� ����
        for (int i = 0; i < ClothesManager.Instance.clothesSlots.Count; i++)
        {
            ClothesManager.Instance.clothesSlots[i].rabbitCitizen = rabbitCitizen;
            ClothesManager.Instance.clothesSlots[i].gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < ClothesManager.Instance.clothesSlots.Count; i++)
        {
            ClothesManager.Instance.clothesSlots[i].gameObject.SetActive(false);
            ClothesManager.Instance.clothesSlots[i].rabbitCitizen = null;
        }
    }
    #endregion
}
