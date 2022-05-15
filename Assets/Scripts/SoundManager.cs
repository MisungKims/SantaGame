/**
 * @brief 게임의 사운드
 * @author 김미성
 * @date 22-05-15
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBgmType
{
    main,
    loading,
    giftStore,
    game,
    none
}

public enum ESoundEffectType
{
    uiButton,
    getGoldButton,
    stamp,
    screenShot,
    newBuilding,
    giftShopLever,
    giftShopBall,
    giftShopFallingBall,
    getGift
}

public class SoundManager : MonoBehaviour
{
    #region 변수
    [Header("------------ 배경음악")]
    public AudioSource bgm;
    [SerializeField]
    private AudioClip[] bgmList;
    private EBgmType nowBgm = EBgmType.none;            // 현재 실행중인 bgm

    // 효과음
    [Header("------------ 효과음")]
    public AudioSource soundEffect;
    [SerializeField]
    private AudioClip[] soundEffectList;
    
    
    // 싱글톤
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion

    #region 유니티 함수
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }

        bgm.loop = true;
        soundEffect.loop = false;
    }
    #endregion

    #region 함수
    /// <summary>
    /// BGM을 실행
    /// </summary>
    /// <param name="bgmType">실행할 bgm</param>
    public void PlayBGM(EBgmType bgmType)
    {
        if (nowBgm.Equals(bgmType)) return;             // 이미 재생 중이라면 return 

        bgm.clip = bgmList[(int)bgmType];
        bgm.Play();
        nowBgm = bgmType;
    }

    public void StopBGM()
    {
        bgm.Stop();
        nowBgm = EBgmType.none;
    }

    /// <summary>
    /// 효과음 실행
    /// </summary>
    /// <param name="soundEffectType">실행할 효과음</param>
    public void PlaySoundEffect(ESoundEffectType soundEffectType)
    {
        soundEffect.clip = soundEffectList[(int)soundEffectType];
        soundEffect.Play();
    }
    #endregion
}
