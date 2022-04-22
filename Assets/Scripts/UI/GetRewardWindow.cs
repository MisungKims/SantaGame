/**
 * @brief ���� ȹ�� UI
 * @author ��̼�
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
    /// ���� ȹ�� â�� �� �� �ʱ� ����
    /// </summary>
    /// <param name="sprite">������ �̹���</param>
    public void OpenWindow(Sprite sprite)
    {
        this.gameObject.SetActive(true);
        RewardImg.sprite = sprite;
    }

    /// <summary>
    /// ���� ȹ�� â�� ���� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void CloseWindow()
    {
        isTouch = true;
        this.gameObject.SetActive(false);
    }
}
