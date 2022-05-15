using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftDeliveryButton : MonoBehaviour
{
   public void ClickButton()
    {
        SoundManager.Instance.StopBGM();
        GameLoadManager.LoadScene("GiftDeliveryGame");
    }
}
