/**
 * @brief �� ������ UI ����
 * @author ��̼�
 * @date 22-06-04
 */

using UnityEngine;
using UnityEngine.UI;

public class ClotesStoreSlot : Closet
{
    #region ����
    // UI ����
    [SerializeField]
    private Button buyButton;
    [SerializeField]
    private Text priceText;
    
    // ������Ƽ
    private int price;                 // ���� ����
    public int Price
    {
        set
        {
            price = value;
            priceText.text = price.ToString();
        }
    }

    // ĳ��
    private GameManager gameManager;
    private ClothesManager clothesManager;
    #endregion

    #region ����Ƽ �Լ�
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

    #region �Լ�
    /// <summary>
    /// ���� �� �� �ִ¸�ŭ�� ���̾ư� ���� �� ��ư�� Ŭ������ ���ϵ���
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
    /// ���� ����
    /// </summary>
    /// <param name="clothes">������ ��</param>
    public virtual void SetSlot(Clothes clothes)
    {
        base.SetClothes(clothes);

        Price = clothes.price;
        SetInteractable();
    }

    /// <summary>
    /// ���� ������ (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void Buy()
    {
        if (gameManager.MyDia >= price)
        {
            gameManager.MyDia -= price;             // ���� ����

            clothesManager.GetClothes(clothes);     // �� ȹ��

            SetInteractable();
        }
    }
    #endregion
}
