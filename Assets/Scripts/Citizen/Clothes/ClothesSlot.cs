/**
 * @brief 사용자가 가진 옷의 UI 슬롯
 * @author 김미성
 * @date 22-06-01
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClothesSlot : MonoBehaviour
{
    #region 변수
    [SerializeField]
    private Image clothesImage;             // 슬롯의 옷 이미지

    [SerializeField]
    private GameObject checkImage;          // 옷의 착용 여부를 보여주는 이미지

    [SerializeField]
    private Button button;                  // 슬롯의 버튼

    private bool isWearing;                 // 옷의 착용 여부
    public bool IsWearing
    {
        set
        {
            isWearing = value;
            checkImage.SetActive(isWearing);
        }
    }

    public RabbitCitizen rabbitCitizen;     // 현재 토끼 주민창의 토끼

    public Clothes clothes;                 // 해당 슬롯의 옷
    #endregion

    #region 유니티 함수
    private void OnEnable()
    {
        CheckWearing();

        SetInteractable();
    }
    #endregion

    #region 함수
    /// <summary>
    /// 슬롯 UI 초기화
    /// </summary>
    /// <param name="myClothes">해당 슬롯의 옷</param>
    public void Init(Clothes myClothes)
    {
        clothes = myClothes;
        clothesImage.sprite = myClothes.image;
    }

    /// <summary>
    /// 이 슬롯의 옷을 입고있는지?
    /// </summary>
    /// <returns></returns>
    bool CheckWearing()
    {
        if (!rabbitCitizen) return false;

        if (rabbitCitizen.clothes == clothes)       // 해당 옷을 입고 있을 때
        {
            IsWearing = true;
            return true;
        }
        else    //옷을 입고있지 않을 때
        {
            IsWearing = false;
            return false;
        }
    }

    /// <summary>
    /// 버튼의 Interactable 설정
    /// </summary>
    void SetInteractable()
    {
        // 옷을 입을 수 없고 토끼가 슬롯의 옷을 입고 있지 않을 때 버튼을 누를 수 없게
        if (clothes.totalAmount <= clothes.wearingCount && rabbitCitizen.clothes != clothes)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }


    /// <summary>
    /// 토끼 주민에게 옷을 입히거나 벗김
    /// </summary>
    public void PutOnOrOffRabbit()
    {
        if (!rabbitCitizen) return;

        if (CheckWearing())     // 똑같은 옷을 입고있을 때는 벗김
        {
            PutOff();
        }
        else   // 똑같은 옷을 입고있지 않을 때는 입힘
        {
            if (clothes.totalAmount > clothes.wearingCount)
            {
                if (rabbitCitizen.clothes != null)      // 이미 다른 옷을 입고 있을 때에는
                {
                    for (int i = 0; i < ClothesManager.Instance.clothesSlots.Count; i++)
                    {
                        if (ClothesManager.Instance.clothesSlots[i].clothes == rabbitCitizen.clothes)
                        {
                            ClothesManager.Instance.clothesSlots[i].PutOff();           // 해당 옷을 벗김
                            break;
                        }
                    }
                }

                PutOn();
            }
        }

        SetInteractable();
    }

    /// <summary>
    /// 토끼에게 옷을 입힘
    /// </summary>
    void PutOn()
    {
        if (rabbitCitizen.PutOn(clothes))
        {
            clothes.wearingCount++;
            IsWearing = true;
        }
    }

    /// <summary>
    /// 토끼에게 옷을 벗김
    /// </summary>
    void PutOff()
    {
        rabbitCitizen.PutOff();
        clothes.wearingCount--;

        IsWearing = false;
    }
    #endregion
}
