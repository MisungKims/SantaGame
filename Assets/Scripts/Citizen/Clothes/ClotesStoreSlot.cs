/**
 * @brief 옷 가게의 UI 슬롯
 * @author 김미성
 * @date 22-06-04
 */

using UnityEngine;
using UnityEngine.UI;

public class ClotesStoreSlot : Closet
{
    #region 변수
    // UI 변수
    [SerializeField]
    private Button buyButton;
    [SerializeField]
    private Text priceText;
    
    // 프로퍼티
    private int price;                 // 옷의 가격
    public int Price
    {
        set
        {
            price = value;
            priceText.text = price.ToString();
        }
    }

    // 캐싱
    private GameManager gameManager;
    private ClothesManager clothesManager;
    #endregion

    #region 유니티 함수
    void Awake()
    {
        gameManager = GameManager.Instance;
        clothesManager = ClothesManager.Instance;
    }

    private void OnEnable()
    {
        SetInteractable();
    }
    #endregion

    #region 함수
    /// <summary>
    /// 옷을 살 수 있는만큼의 다이아가 없을 땐 버튼을 클릭하지 못하도록
    /// </summary>
    public void SetInteractable()
    {
        if (gameManager == null) gameManager = GameManager.Instance;

        if (gameManager.MyDia < price)
        {
            buyButton.interactable = false;
        }
        else
        {
            buyButton.interactable = true;
        }
    }

    /// <summary>
    /// 슬롯 설정
    /// </summary>
    /// <param name="clothes">설정할 옷</param>
    public virtual void SetSlot(Clothes clothes)
    {
        base.SetClothes(clothes);

        Price = clothes.price;
        SetInteractable();
    }

    /// <summary>
    /// 옷을 구입함 (인스펙터에서 호출)
    /// </summary>
    public void Buy()
    {
        if (gameManager.MyDia >= price)
        {
            gameManager.MyDia -= price;             // 가격 지불

            clothesManager.GetClothes(clothes);     // 옷 획득

            SetInteractable();
        }
    }
    #endregion
}
