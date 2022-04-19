/**
 * @brief 건물을 생성
 * @author 김미성
 * @date 22-04-19
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class Building : MonoBehaviour
{
    #region 변수

    private Transform thisTransform;

    [SerializeField]
    private int level = 1;
    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    private float multiplyBuildingPrice;        // 업그레이드 후 건물 가격 증가 배율

    private string buildingPrice;              // 건물 가격 
    public string BuildingPrice
    {
        get { return buildingPrice; }
    }

    private string incrementGold;              // 플레이어의 골드 증가량
    public string IncrementGold
    {
        get { return incrementGold; }
        set { incrementGold = value; }
    }

    private int index;
    public int Index
    {
        get { return index; }
    }

    private string buildingName;
    public string BuilidingName
    {
        get { return buildingName; }
    }

    [SerializeField]
    private Slider getGoldSlider;       // 골드 획득 슬라이더
    private int count = 0;
    public int Count
    {
        get { return count; }
        set
        {
            count = value;
            getGoldSlider.value = count;
        }
    }

    [SerializeField]
    private GameObject getGoldBtn;      // 골드 수동 획득 버튼

    private float second;
    public float Second
    {
        set
        {
            second = value;
            getGoldSlider.maxValue = second;
        }
    }

    public Santa santa;         // 고용한 알바 (산타)

    public bool isAuto = false;    // 알바를 고용한 건물은 골드 획득 자동화

    private bool isClickGetBtn = false;   // 골드 획득이 수동일 때, 획득 UI를 클릭했으면 true, 아니면 false

    private Vector3 distance;       // 카메라와의 거리

    StringBuilder sb = new StringBuilder();

    // 캐싱
    private GameManager gameManager;
    private CameraMovement cameraMovement;
    private UIManager uiManager;
    private ClickObjWindow window;
    private static WaitForSeconds waitForSecond1 = new WaitForSeconds(1f);

    #endregion

    #region 함수

    /// <summary>
    /// 건물 초기 설정
    /// </summary>
    /// <param name="index">건물의 인덱스</param>
    /// <param name="name">건물의 이름</param>
    /// <param name="multiplyPrice">건물의 가격 증가 배수</param>
    /// <param name="price">건물의 가격</param>
    /// <param name="iGold">골드 증가량</param>
    public void InitBuilding(int index, string name, float multiplyPrice, string price, string iGold, float second)
    {
        gameObject.SetActive(true);         // 건물이 보이도록

        this.index = index;
        buildingName = name;
        multiplyBuildingPrice = multiplyPrice;
        buildingPrice = price;
        incrementGold = iGold;
        Second = second;

        SetCamTargetThis();                 // 카메라가 건물을 바라보도록

        ShowObjWindow();                    // 오브젝트 정보 창이 보이도록

        StartCoroutine(Increment());        // 골드획득 시작
    }

    /// <summary>
    /// 건물 업그레이드
    /// </summary>
    public void Upgrade()
    {
        if (!GoldManager.CompareBigintAndUnit(gameManager.MyGold, buildingPrice))
        {
            return;
        }

        gameManager.MyGold -= GoldManager.UnitToBigInteger(buildingPrice);  // 업그레이드 비용 지불

        buildingPrice = GoldManager.MultiplyUnit(buildingPrice, multiplyBuildingPrice); // 비용을 배율만큼 증가

        incrementGold = GoldManager.MultiplyUnit(incrementGold, 1.1f * gameManager.goldEfficiency);  // 골드 증가량을 배율만큼 증가

        level++;
    }


    /// <summary>
    /// 카메라가 해당 건물을 따라다님
    /// </summary>
    public void SetCamTargetThis()
    {
        cameraMovement.ChaseBuilding(thisTransform, thisTransform.GetChild(0).transform);
    }

   
    /// <summary>
    /// 오브젝트 정보창 보여줌
    /// </summary>
    public void ShowObjWindow()
    {
        sb.Clear();
        sb.Append("+ ");
        sb.Append(incrementGold.ToString());

        window.Builidng = this;
        window.SetBuildingInfo();

        uiManager.ShowClickObjWindow();
    }

 
    /// <summary>
    /// 건물을 터치할 때
    /// </summary>
    void TouchBuilding()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))
            {
                if (hit.collider.CompareTag("Building") && hit.collider.name == this.name)
                {
                    SetCamTargetThis();
                    ShowObjWindow();
                }
            }
        }
    }

    /// <summary>
    /// 수동 획득 버튼 클릭 (인스펙터에서 호출)
    /// </summary>
    public void ClickGetBtn()
    {
        isClickGetBtn = true;
    }

    #endregion

    #region 코루틴
    /// <summary>
    /// 골드 획득 타이머
    /// </summary>
    IEnumerator Increment()
    {
        getGoldBtn.SetActive(false);
        while (true)
        {
            yield return StartCoroutine(TimeCount());

            if (isAuto)     // 자동화 상태이면 바로 골드 획득
            {
                gameManager.MyGold += GoldManager.UnitToBigInteger(incrementGold);
            }
            else            // 수동 상태이면 UI 클릭을 기다림
            {
                getGoldSlider.gameObject.SetActive(false);

                yield return StartCoroutine(WaitForManualAcquisition());

                getGoldSlider.gameObject.SetActive(true);
                getGoldBtn.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 정해진 시간만큼 카운트
    /// </summary>
    IEnumerator TimeCount()
    {
        for (int i = 0; i <= second; i++)
        {
            yield return waitForSecond1;

            Count++;
        }
        Count = 0;
    }

    /// <summary>
    /// 골드의 수동 획득을 대기 (UI 터치를 기다림)
    /// </summary>
    IEnumerator WaitForManualAcquisition()
    {
        getGoldBtn.SetActive(true);

        while (!isClickGetBtn)
        {
            yield return null;

            if (isAuto)         // 수동 획득 대기 중 알바를 고용할 때
            {
                gameManager.MyGold += GoldManager.UnitToBigInteger(incrementGold);
                break;
            }
        }
    }
    #endregion

    #region 유니티 메소드

    private void Awake()
    {
        gameManager = GameManager.Instance;
        cameraMovement = CameraMovement.Instance;
        uiManager = UIManager.Instance;
        window = uiManager.clickObjWindow.transform.GetComponent<ClickObjWindow>();

        thisTransform = this.transform;
    }
   
    void Update()
    {
        TouchBuilding();
    }
    #endregion
}
