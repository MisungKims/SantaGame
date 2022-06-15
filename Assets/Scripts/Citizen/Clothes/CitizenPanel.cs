/**
 * @brief ≈‰≥¢ ¡÷πŒ√¢ UI
 * @author ±ËπÃº∫
 * @date 22-06-04
 */
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public enum EMoveType { none, opening, closing };          

public class CitizenPanel : MonoBehaviour
{
    #region ∫Øºˆ
    public RabbitCitizen rabbitCitizen;     // «ˆ¿Á ≈‰≥¢ ¡÷πŒ√¢¿« ≈‰≥¢

    [SerializeField]
    private GameObject clothesWindowButton;       // ≈‰≥¢¿« ø ¿Â√¢ πˆ∆∞

    [SerializeField]
    private GameObject clothesWindow;       // ≈‰≥¢¿« ø ¿Â√¢

    [SerializeField]
    private Text buttonText;


    private Vector3 upCloset = new Vector3(0, -377f, 0);

    private Vector3 downCloset = new Vector3(0, -546f, 0);

    private EMoveType moveType;         // ø ¿Â¿ª ø≠∞Ì ¥›¥¬ ªÛ≈¬

    // ƒ≥ΩÃ
    private ClothesManager clothesManager;
    private ObjectManager objectManager;
    #endregion

    #region ¿Ø¥œ∆º «‘ºˆ

    void Awake()
    {
        clothesManager = ClothesManager.Instance;
        objectManager = ObjectManager.Instance;

    }

    private void OnEnable()
    {
        Open();
    }
    #endregion

    #region ƒ⁄∑Á∆æ
    /// <summary>
    /// ø ¿Â øÚ¡˜¿”
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveCloset()
    {
        while (true)
        {
            // ø ¿Â¿ª øÆ
            if (moveType.Equals(EMoveType.opening))
            {
                OpenCloset();

                while (clothesWindowButton.transform.localPosition.y - upCloset.y < 0.05f && moveType.Equals(EMoveType.opening))
                {
                    clothesWindowButton.transform.localPosition = Vector3.Lerp(clothesWindowButton.transform.localPosition, upCloset, Time.deltaTime * 2f);

                    yield return null;
                }

                clothesWindowButton.transform.localPosition = upCloset;
                if (moveType.Equals(EMoveType.opening)) moveType = EMoveType.none;
            }
            // ø ¿Â¿ª ¥›¿Ω
            else if (moveType.Equals(EMoveType.closing))
            {
                while (clothesWindowButton.transform.localPosition.y - downCloset.y > 0.05f && moveType.Equals(EMoveType.closing))
                {
                    clothesWindowButton.transform.localPosition = Vector3.Lerp(clothesWindowButton.transform.localPosition, downCloset, Time.deltaTime * 2f);

                    yield return null;
                }

                if (moveType.Equals(EMoveType.closing)) moveType = EMoveType.none;
                CloseCloset();
            }

            yield return null;
        }
    }
    #endregion

    #region «‘ºˆ

    public void Open()
    {
        // ø ∞°∞‘∞° ¿·±›«ÿ¡¶ µ«æ˙¿∏∏È ø ¿Â √¢¿ª ∫∏ø©¡‹
        if (objectManager.objectList[7].buildingLevel > 0)
        {
            if (!clothesWindowButton.activeSelf)
            {
                clothesWindowButton.SetActive(true);
            }

            CloseCloset();
            StartCoroutine(MoveCloset());
        }
        else if (clothesWindowButton.activeSelf && objectManager.objectList[7].buildingLevel <= 0)
        {
            clothesWindowButton.SetActive(false);
        }
    }

    /// <summary>
    /// ø ¿Â ø≠∞≈≥™ ¥›±‚ (¿ŒΩ∫∆Â≈Õø°º≠ »£√‚)
    /// </summary>
    public void OpenOrCloseCloset()
    {
        switch (moveType)
        {
            case EMoveType.none:
                // ø ¿Â¿Ã ø≠∑¡¿÷¥Ÿ∏È ¥›∞Ì, ¥›«Ù¿÷¥Ÿ∏È ø≠±‚
                if (clothesWindow.activeSelf)
                {
                    moveType = EMoveType.closing;
                }
                else
                {
                    moveType = EMoveType.opening;
                }
                break;

            case EMoveType.opening:
                moveType = EMoveType.closing;
                break;

            case EMoveType.closing:
                moveType = EMoveType.opening;
                break;
        }
        
    }

    /// <summary>
    /// ø ¿Â ø≠±‚
    /// </summary>
    public void OpenCloset()
    {
        buttonText.text = "<";
        InitClothesSlot();
        clothesWindow.SetActive(true);
    }

    /// <summary>
    /// ø ¿Â ¥›±‚
    /// </summary>
    public void CloseCloset()
    {
        buttonText.text = ">";
        clothesWindow.SetActive(false);
        clothesWindowButton.transform.localPosition = downCloset;
    }

    /// <summary>
    /// ø ¿Â ΩΩ∑‘ UI √ ±‚ º≥¡§
    /// </summary>
    public void InitClothesSlot()
    {
        // ø  UI ΩΩ∑‘µÈ¿« rabbitCitizen¿ª «ˆ¿Á ≈‰≥¢∑Œ ∫Ø∞Ê
        for (int i = 0; i < clothesManager.clothesSlotList.Count; i++)
        {
            clothesManager.clothesSlotList[i].rabbitCitizen = rabbitCitizen;
            clothesManager.clothesSlotList[i].gameObject.SetActive(true);
        }
    }
    #endregion
}
