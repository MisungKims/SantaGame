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
    // 싱글톤
    private static InviteRabbit instance;
    public static InviteRabbit Instance
    {
        get { return instance; }
    }

    // UI 변수
    [SerializeField]
    private Text priceText;
    [SerializeField]
    private Text citizenCountText;
    public GameObject rabbit;
    public GameObject rabbitGroup;

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
        if (instance == null)
            instance = this;
        else
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
        }

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
    /// 토끼를 초대 (새로운 토끼 생성) (인스펙터에서 호출)
    /// </summary>
    public void Invite()
    {
        // 지불할 당근이 없다면 return
        if (!GoldManager.CompareBigintAndUnit(gameManager.MyCarrots, price))    
        {
            return;
        }

        // 토끼 초대창 닫기
        this.gameObject.SetActive(false);   

        // 토끼 주민 생성 후 카메라의 타깃으로 설정
        RabbitCitizen rabbitCitizen = GameObject.Instantiate(rabbit, rabbitGroup.transform).GetComponent<RabbitCitizen>();
        CitizenRabbitManager.Instance.rabbitCitizens.Add(rabbitCitizen);
        rabbitCitizen.name = CitizenRabbitManager.Instance.rabbitCitizens.Count.ToString();
        rabbitCitizen.SetCamTargetThis();

        // 초대 비용 지불
        gameManager.MyCarrots -= GoldManager.UnitToBigInteger(price);  

        // 토끼 주민 수 증가
        Count = ++gameManager.CitizenCount + 1;         

        // 효율 증가
        gameManager.goldEfficiency *= 1.5f;             

        // 토끼 초대 가격 증가
        Price = GoldManager.MultiplyUnit(price, magnification);     
        magnification += 0.5f;

        // 토끼의 Material을 랜덤으로 설정
        int rand = Random.Range(0, 12);
        rabbitCitizen.rabbitMat.material = CitizenRabbitManager.Instance.materials[rand];       

        // 토끼의 정보를 저장할 새 인스턴스 생성
        Citizen citizen = new Citizen(rabbitCitizen.name, rand, -1, rabbitCitizen.transform.position);
        CitizenRabbitManager.Instance.citizenList.Add(citizen);
    }

    #endregion
}
