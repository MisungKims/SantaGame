/**
 * @brief ∫∏ªÛ »πµÊ UI
 * @author ±ËπÃº∫
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetRewardWindow : MonoBehaviour
{
    public Image RewardImg;
    public bool isTouch;

    private void OnEnable()
    {
        isTouch = false;
    }

    /// <summary>
    /// ∫∏ªÛ »πµÊ √¢¿ª ø≠ ∂ß √ ±‚ º≥¡§
    /// </summary>
    /// <param name="sprite">∫∏ø©¡Ÿ ¿ÃπÃ¡ˆ</param>
    public void OpenWindow(Sprite sprite)
    {
        this.gameObject.SetActive(true);
        RewardImg.sprite = sprite;
    }

    /// <summary>
    /// ∫∏ªÛ »πµÊ √¢¿ª ¥›¿Ω (¿ŒΩ∫∆Â≈Õø°º≠ »£√‚)
    /// </summary>
    public void CloseWindow()
    {
        isTouch = true;
        this.gameObject.SetActive(false);
    }
}
