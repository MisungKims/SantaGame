/**
 * @details ��Ÿ�� Ŭ������ �� ���̴� UI
 * @author ��̼�
 * @date 22-04-27
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class ClickObjWindow : MonoBehaviour
{
    #region ����
    // UI ����
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


    // ��Ʈ�� ����
    StringBuilder levelSb = new StringBuilder();
    StringBuilder goldSb = new StringBuilder();


    // ������Ƽ 
    public string ObjName       // ��Ÿ �̸�
    {
        set
        {
            NameText.text = value;
        }
    }

    public int ObjLevel         // ��Ÿ�� ����
    {
        set
        {
            levelSb.Clear();
            levelSb.Append("Lv.");
            levelSb.Append(value.ToString());
            LevelText.text = levelSb.ToString();
        }
    }

    public string ObjAmount     // ��Ÿ�� ȿ�� ������
    {
        set
        {
            DescText.text = value;
        }
    }

    public string ObjPrice      // ��Ÿ�� ����
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

    // ĳ��
    private GameManager gameManager;
    private SoundManager soundManager;
    #endregion

    #region ����Ƽ �Լ�
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

    #region �Լ�
    /// <summary>
    /// ��Ÿ�� ������ ������ UI�� Set
    /// </summary>
    public void SetSantaInfo()
    {
        ObjName = santa.SantaName;
        objImg.sprite = santa.SantaSprite;

        ObjLevel = santa.Level;
        ObjPrice = santa.SantaPrice;

        goldSb.Clear();
        goldSb.Append("�˹� ȿ�� ");
        goldSb.Append(santa.SantaEfficiency.ToString());
        goldSb.Append("% ����");
        ObjAmount = goldSb.ToString();
    }

    /// <summary>
    /// ���׷��̵� ��ư Ŭ�� �� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void UpgradeButtonClick()
    {
        Refresh();
    }

    /// <summary>
    /// UI ���ΰ�ħ
    /// </summary>
    void Refresh()
    {
        if (santa.Upgrade())      // ��Ÿ ���׷��̵�
        {
            soundManager.PlaySoundEffect(ESoundEffectType.uiButton);       // ȿ���� ����

            ObjLevel = santa.Level;
            ObjPrice = santa.SantaPrice;

            goldSb.Clear();
            goldSb.Append("�˹� ȿ�� ");
            goldSb.Append(santa.SantaEfficiency.ToString());
            goldSb.Append("% ����");
            ObjAmount = goldSb.ToString();
        }

        SetButtonInteractable();
    }

  
    /// <summary>
    /// ���׷��̵� ��ư�� Interactable ����
    /// </summary>
    void SetButtonInteractable()
    {
        if (GoldManager.CompareBigintAndUnit(gameManager.MyGold, ObjPrice))        //���� ���� ������Ʈ�� ���ݺ��� Ŭ ��
            UpgradeButton.interactable = true;
        else UpgradeButton.interactable = false;
    }
    #endregion
}
