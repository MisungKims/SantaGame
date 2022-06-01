using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenPanel : MonoBehaviour
{
    public RabbitCitizen rabbitCitizen;

    private void OnEnable()
    {
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
}
