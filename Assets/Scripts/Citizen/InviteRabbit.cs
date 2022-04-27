/**
 * @details 토끼를 초대
 * @author 김미성
 * @date 22-04-23
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InviteRabbit : MonoBehaviour
{
    #region 변수
    // UI 변수
    [SerializeField]
    private Text priceText;
    [SerializeField]
    private Text citizenCountText;
    [SerializeField]
    private GameObject rabbit;
    [SerializeField]
    private GameObject rabbitGroup;

    // 프로퍼티
    private string price = "100";
    public string Price
    {
        set
        {
            price = value;
            priceText.text = price;
        }
    }

    private int count;
    public int Count
    {
        set
        {
            citizenCountText.text = value.ToString();
        }
    }


    // 토끼 초대 비용 증가 시 필요
    private float magnification = 1.7f;

    // 캐싱
    private GameManager gameManager;
    #endregion

    #region 유니티 함수
    private void Awake()
    {
        gameManager = GameManager.Instance;
        priceText.text = price;
    }

    private void OnEnable()
    {
        Count = (gameManager.CitizenCount + 1);
    }
    #endregion

    #region 함수
    /// <summary>
    /// 토끼를 초대 (인스펙터에서 호출)
    /// </summary>
    public void Invite()
    {
        if (!GoldManager.CompareBigintAndUnit(gameManager.MyCarrots, price))    // 지불할 당근이 없다면 return
        {
            return;
        }

        this.gameObject.SetActive(false);   // 토끼 초대창 닫기

        GameObject.Instantiate(rabbit, rabbitGroup.transform).GetComponent<RabbitCitizen>().SetCamTargetThis();    // 토끼 주민 생성 후 카메라의 타깃 설정

        gameManager.MyCarrots -= GoldManager.UnitToBigInteger(price);  // 초대 비용 지불

        Count = ++gameManager.CitizenCount + 1;

        gameManager.goldEfficiency *= 1.5f;     // 효율 증가

        Price = GoldManager.MultiplyUnit(price, magnification);
        magnification += 0.5f;
    }
    #endregion
}
