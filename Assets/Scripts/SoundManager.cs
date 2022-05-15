/**
 * @brief ������ ����
 * @author ��̼�
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
    #region ����
    [Header("------------ �������")]
    public AudioSource bgm;
    [SerializeField]
    private AudioClip[] bgmList;
    private EBgmType nowBgm = EBgmType.none;            // ���� �������� bgm

    // ȿ����
    [Header("------------ ȿ����")]
    public AudioSource soundEffect;
    [SerializeField]
    private AudioClip[] soundEffectList;
    
    
    // �̱���
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

    #region ����Ƽ �Լ�
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

    #region �Լ�
    /// <summary>
    /// BGM�� ����
    /// </summary>
    /// <param name="bgmType">������ bgm</param>
    public void PlayBGM(EBgmType bgmType)
    {
        if (nowBgm.Equals(bgmType)) return;             // �̹� ��� ���̶�� return 

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
    /// ȿ���� ����
    /// </summary>
    /// <param name="soundEffectType">������ ȿ����</param>
    public void PlaySoundEffect(ESoundEffectType soundEffectType)
    {
        soundEffect.clip = soundEffectList[(int)soundEffectType];
        soundEffect.Play();
    }
    #endregion
}
