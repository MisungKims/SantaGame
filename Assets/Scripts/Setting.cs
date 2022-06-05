using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    [SerializeField]
    private Slider bgmSlider;
    [SerializeField]
    private Slider soundEffectSlider;

    SoundManager soundManager;

    private void Awake()
    {
        soundManager = SoundManager.Instance;
    }

    private void Update()
    {
        soundManager.bgm.volume = bgmSlider.value;
        soundManager.soundEffect.volume = soundEffectSlider.value;
    }
}
