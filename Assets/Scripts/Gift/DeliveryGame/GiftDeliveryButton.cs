/**
 * @brief ���� ���� ���� ��ư
 * @author ��̼�
 * @date 22-06-04
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftDeliveryButton : MonoBehaviour
{
    /// <summary>
    /// ���� ���� ��ư Ŭ�� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void ClickButton()
    {
        SoundManager.Instance.StopBGM();

        // ���� ���� ���� ������Ʈ Ȱ��ȭ
        GameManager.Instance.StartDeliveryGame();
    }
}
