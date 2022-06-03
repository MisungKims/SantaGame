/**
 * @brief Åä³¢ ÁÖ¹ÎÃ¢ UI
 * @author ±è¹Ì¼º
 * @date 22-06-01
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenPanel : MonoBehaviour
{
    #region º¯¼ö
    public RabbitCitizen rabbitCitizen;   // ÇöÀç Åä³¢ ÁÖ¹ÎÃ¢ÀÇ Åä³¢

    [SerializeField]
    private GameObject clothesWindow;
    #endregion

    #region À¯´ÏÆ¼ ÇÔ¼ö
    private void OnEnable()
    {
        //// ¿Ê°¡°Ô°¡ Àá±ÝÇØÁ¦ µÇ¾úÀ¸¸é ¿Ê Ã¢À» º¸¿©ÁÜ
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

    #region ÇÔ¼ö
    public void InitClothesSlot()
    {
        // ¿Ê UI ½½·ÔµéÀÇ rabbitCitizenÀ» ÇöÀç Åä³¢·Î º¯°æ
        for (int i = 0; i < ClothesManager.Instance.clothesSlots.Count; i++)
        {
            ClothesManager.Instance.clothesSlots[i].rabbitCitizen = rabbitCitizen;
            ClothesManager.Instance.clothesSlots[i].gameObject.SetActive(true);
        }
    }
    #endregion
}
