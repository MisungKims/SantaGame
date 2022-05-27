using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftDeliveryButton : MonoBehaviour
{
    [SerializeField]
    private GameObject deliveryGame;

    [SerializeField]
    private Canvas villiageCanvas;

    [SerializeField]
    private Canvas villiageWorldCanvas;

    public void ClickButton()
    {
        SoundManager.Instance.StopBGM();

        UIManager.Instance.SetisOpenPanel(true);

        deliveryGame.SetActive(true);
        UIManager.Instance.mainPanel.SetActive(false);
        villiageCanvas.enabled = false;
        villiageWorldCanvas.enabled = false;
    }
}
