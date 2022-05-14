using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftDeliveryButton : MonoBehaviour
{
   public void ClickButton()
    {
        GameLoadManager.LoadScene("GiftDeliveryGame");
    }
}
