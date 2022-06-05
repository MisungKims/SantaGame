/**
 * @brief 설정창
 * @author 김미성
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    #region 변수
    [SerializeField]
    private Slider bgmSlider;
    [SerializeField]
    private Slider soundEffectSlider;

    // 캐싱
    private SoundManager soundManager;
    #endregion

    #region 유니티 함수
    private void Awake()
    {
        soundManager = SoundManager.Instance;
    }

    private void Update()
    {
        // 음향을 설정
        soundManager.bgm.volume = bgmSlider.value;
        soundManager.soundEffect.volume = soundEffectSlider.value;
    }
    #endregion

}
