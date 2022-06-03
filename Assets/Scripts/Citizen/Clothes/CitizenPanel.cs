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

    [SerializeField]
    private GameObject clothesWindow;
    #endregion

    #region ����Ƽ �Լ�
    private void OnEnable()
    {
        //// �ʰ��԰� ������� �Ǿ����� �� â�� ������
        if (ObjectManager.Instance.objectList[7].buildingLevel > 0)
        {
            if (!clothesWindow.activeSelf)
            {
                clothesWindow.SetActive(true);
            }

            InitClothesSlot();
        }
        else if (clothesWindow.activeSelf && ObjectManager.Instance.objectList[7].buildingLevel <= 0)
        {
            clothesWindow.SetActive(false);
        }

        //if (clothesWindow.activeSelf)
        //{
        //    InitClothesSlot();
        //}  
    }

    //private void OnDisable()
    //{
    //    for (int i = 0; i < ClothesManager.Instance.clothesSlots.Count; i++)
    //    {
    //        ClothesManager.Instance.clothesSlots[i].gameObject.SetActive(false);
    //        ClothesManager.Instance.clothesSlots[i].rabbitCitizen = null;
    //    }
    //}
    #endregion

    #region �Լ�
    public void InitClothesSlot()
    {
        // �� UI ���Ե��� rabbitCitizen�� ���� �䳢�� ����
        for (int i = 0; i < ClothesManager.Instance.clothesSlots.Count; i++)
        {
            ClothesManager.Instance.clothesSlots[i].rabbitCitizen = rabbitCitizen;
            ClothesManager.Instance.clothesSlots[i].gameObject.SetActive(true);
        }
    }
    #endregion
}
