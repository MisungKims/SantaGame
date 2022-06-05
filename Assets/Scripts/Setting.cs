/**
 * @brief ����â
 * @author ��̼�
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    #region ����
    [SerializeField]
    private Slider bgmSlider;
    [SerializeField]
    private Slider soundEffectSlider;

    // ĳ��
    private SoundManager soundManager;
    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        soundManager = SoundManager.Instance;
    }

    private void Update()
    {
        // ������ ����
        soundManager.bgm.volume = bgmSlider.value;
        soundManager.soundEffect.volume = soundEffectSlider.value;
    }
    #endregion

}
