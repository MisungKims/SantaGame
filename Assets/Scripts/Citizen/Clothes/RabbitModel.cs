using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitModel : MonoBehaviour
{
    public Clothes clothes = null;      // ¡÷πŒ¿« ø (ƒ⁄µ≈€)

    private GameObject clothesObj;      // ø  ø¿∫Í¡ß∆Æ

    public Transform clothesParent;    // ø  ø¿∫Í¡ß∆Æ¿« ∫Œ∏

    /// <summary>
    /// ø ¿ª ¿‘¿Ω
    /// </summary>
    /// <param name="clothes">¿‘»˙ ø </param>
    public bool PutOn(Clothes clothes)
    {
        if (clothes != null)
        {
            this.clothes = clothes;

            clothesObj = ObjectPoolingManager.Instance.Get(clothes.flag, clothesParent);

            clothesObj.layer = 7;

            clothesObj.transform.localPosition = clothes.pos;
            clothesObj.transform.localEulerAngles = clothes.rot;
            clothesObj.transform.localScale = clothes.scale;

            return true;
        }

        return false;
    }

    /// <summary>
    /// ø ¿ª π˛¿Ω
    /// </summary>
    public void PutOff()
    {
        clothesObj.layer = 0;
        ObjectPoolingManager.Instance.Set(clothesObj, clothes.flag);
        clothes = null;
    }
}
