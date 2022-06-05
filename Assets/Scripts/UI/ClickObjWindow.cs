/**
 * @details 산타를 클릭했을 때 보이는 UI
 * @author 김미성
 * @date 22-04-27
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class ClickObjWindow : MonoBehaviour
{
    #region 변수
    // UI 변수
    [SerializeField]
    private Text NameText;
    [SerializeField]
    private Text LevelText;
    [SerializeField]
    private Text DescText;
    [SerializeField]
    private Text PriceText;
    [SerializeField]
    private Button UpgradeButton;
    [SerializeField]
    private Image objImg;


    // 스트링 빌더
    StringBuilder levelSb = new StringBuilder();
    StringBuilder goldSb = new StringBuilder();


    // 프로퍼티 
    public string ObjName       // 산타 이름
    {
        set
        {
            NameText.text = value;
        }
    }

    public int ObjLevel         // 산타의 레벨
    {
        set
        {
            levelSb.Clear();
            levelSb.Append("Lv.");
            levelSb.Append(value.ToString());
            LevelText.text = levelSb.ToString();
        }
    }

    public string ObjAmount     // 산타의 효율 증가량
    {
        set
        {
            DescText.text = value;
        }
    }

    public string ObjPrice      // 산타의 가격
    {
        get { return PriceText.text; }
        set
        {
            PriceText.text = value;
        }
    }

    private Santa santa;
    public Santa Santa
    {
        set { santa = value; }
    }

    // 캐싱
    private GameManager gameManager;
    private SoundManager soundManager;
    #endregion

    #region 유니티 함수
    private void Awake()
    {
        soundManager = SoundManager.Instance;
        gameManager = GameManager.Instance;
    }

    private void OnEnable()
    {
        SetSantaInfo();
    }
    #endregion

    #region 함수
    /// <summary>
    /// 산타의 정보를 가져와 UI에 Set
    /// </summary>
    public void SetSantaInfo()
    {
        ObjName = santa.SantaName;
        objImg.sprite = santa.SantaSprite;

        ObjLevel = santa.Level;
        ObjPrice = santa.SantaPrice;

        goldSb.Clear();
        goldSb.Append("알바 효율 ");
        goldSb.Append(santa.SantaEfficiency.ToString());
        goldSb.Append("% 증가");
        ObjAmount = goldSb.ToString();
    }

    /// <summary>
    /// 업그레이드 버튼 클릭 시 (인스펙터에서 호출)
    /// </summary>
    public void UpgradeButtonClick()
    {
        Refresh();
    }

    /// <summary>
    /// UI 새로고침
    /// </summary>
    void Refresh()
    {
        if (santa.Upgrade())      // 산타 업그레이드
        {
            soundManager.PlaySoundEffect(ESoundEffectType.uiButton);       // 효과음 실행

            ObjLevel = santa.Level;
            ObjPrice = santa.SantaPrice;

            goldSb.Clear();
            goldSb.Append("알바 효율 ");
            goldSb.Append(santa.SantaEfficiency.ToString());
            goldSb.Append("% 증가");
            ObjAmount = goldSb.ToString();
        }

        SetButtonInteractable();
    }

  
    /// <summary>
    /// 업그레이드 버튼의 Interactable 설정
    /// </summary>
    void SetButtonInteractable()
    {
        if (GoldManager.CompareBigintAndUnit(gameManager.MyGold, ObjPrice))        //가진 돈이 오브젝트의 가격보다 클 때
            UpgradeButton.interactable = true;
        else UpgradeButton.interactable = false;
    }
    #endregion
}
