/**
 * @brief 옷 가게/보관함의 슬롯
 * @author 김미성
 * @date 22-06-01
 */

using UnityEngine;
using UnityEngine.UI;

public class Closet : MonoBehaviour
{
    #region 변수
    public Clothes clothes;     // 어떤 옷인지

    // UI 변수
    [SerializeField]
    protected Text clothesNameText;
    [SerializeField]
    protected Image clothesImage;

    private string clothesName;         // 옷의 이름
    public string ClothesName
    {
        set
        {
            clothesName = value;
            clothesNameText.text = clothesName;
        }
    }

    [SerializeField]
    protected RabbitModel model;      // 옷의 모델
    #endregion

    #region 함수
    /// <summary>
    /// 슬롯 설정
    /// </summary>
    /// <param name="clothes">설정할 옷</param>
    public virtual void SetClothes(Clothes clothes)
    {
        this.clothes = clothes;
        ClothesName = clothes.clothesName;
        clothesImage.sprite = clothes.image;
    }

    /// <summary>
    /// 모델에게 옷을 입혀봄 (인스펙터에서 호출)
    /// </summary>
    public void Wear()
    {
        if (model.clothes != clothes)       // 옷을 입지 않거나 다른 옷을 입고 있을 때에는 옷을 입힘
        {
            model.PutOn(clothes);
        }
        else         // 해당 슬롯의 옷을 입고 있었다면 옷을 벗김
        {
            model.PutOff();
        }
    }
    #endregion
}
