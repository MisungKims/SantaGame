using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftDeliveryButton : MonoBehaviour
{
    public void ClickButton()
    {
        SoundManager.Instance.StopBGM();

        GameManager.Instance.StartDeliveryGame();
    }
}
