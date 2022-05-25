/**
 * @brief 건물
 * @author 김미성
 * @date 22-04-27
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class Building : MonoBehaviour
{
    #region 변수
    [HideInInspector]
    public int index;

    public Object buildingObj;

    public string BuilidingName
    {
        get { return buildingObj.buildingName; }
    }

    // 프로퍼티
    public int Level
    {
        get { return buildingObj.buildingLevel; }
        set { buildingObj.buildingLevel = value;  }
    }

    public float MultiplyBuildingPrice        // 업그레이드 후 건물 가격 증가 배율
    {
        get { return buildingObj.multiplyBuildingPrice; }
    }

    public string BuildingPrice     // 건물 가격 
    {
        get { return buildingObj.buildingPrice; }
        set { buildingObj.buildingPrice = value; }
    }

    public string IncrementGold              // 플레이어의 골드 증가량
    {
        get { return buildingObj.incrementGold; }
        set 
        { 
            buildingObj.incrementGold = value;
            getGoldAmountText.text = $"+ {IncrementGold}";
        }
    }

   
    public float Second
    {
        get { return buildingObj.second; }
    }

    //public Santa santa;             // 고용한 알바 (산타)

    // UI 변수
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
    private Text getGoldAmountText;       // 골드 획득 슬라이더의 텍스트

    [SerializeField]
    private GameObject getGoldBtn;      // 골드 수동 획득 버튼

    [SerializeField]
    private Transform cameraPos;      // 카메라의 위치


    // 그 외 변수

    public bool isAuto = false;     // 알바를 고용했는지? (알바를 고용했다면 골드 획득 자동화)

    private bool isClickGetBtn = false;   // 획득 UI를 클릭했으면 true, 아니면 false (골드 획득 수동일 때)

    private Vector3 distance;       // 카메라와의 거리

    private int questID = 2;

    // 캐싱
    private GameManager gameManager;
    private CameraMovement cameraMovement;
    private UIManager uiManager;
    private ClickObjWindow window;
    private static WaitForSeconds waitForSecond1 = new WaitForSeconds(1f);

    #endregion

    #region 함수

    /// <summary>
    /// 건물 생성 시
    /// </summary>
    public void NewBuilding()
    {
        gameObject.SetActive(true);         // 건물이 보이도록

        SetCamTargetThis();                 // 카메라가 건물을 바라보도록

        Level = 1;
        IncrementGold = IncrementGold;

        getGoldSlider.maxValue = (int)Second;
        StartCoroutine(Increment());        // 골드획득 시작

        ObjectManager.Instance.unlockCount++;

        getGoldSlider.gameObject.SetActive(true);
    }

    /// <summary>
    /// 건물 업그레이드
    /// </summary>
    public bool Upgrade()
    {
        if (!GoldManager.CompareBigintAndUnit(gameManager.MyGold, BuildingPrice))
        {
            return false;
        }

        DailyQuestManager.Instance.Success(questID);        // 퀘스트 완료

        gameManager.MyGold -= GoldManager.UnitToBigInteger(BuildingPrice);  // 업그레이드 비용 지불

        BuildingPrice = GoldManager.MultiplyUnit(BuildingPrice, MultiplyBuildingPrice); // 비용을 배율만큼 증가

        IncrementGold = GoldManager.MultiplyUnit(IncrementGold, 1.1f * gameManager.goldEfficiency);  // 골드 증가량을 배율만큼 증가

        Level++;

        return true;
    }


    /// <summary>
    /// 카메라가 해당 건물을 바라봄
    /// </summary>
    public void SetCamTargetThis()
    {
        if (uiManager.storePanel.activeSelf) uiManager.storePanel.SetActive(false);

        cameraMovement.ChaseBuilding(transform, cameraPos);
    }

   
    ///// <summary>
    ///// 오브젝트 정보창 보여줌
    ///// </summary>
    //public void ShowObjWindow()
    //{
    //    window.clickedObj = buildingObj;
    //    window.Builidng = this;

    //    uiManager.ShowClickObjWindow();
    //}

 
    ///// <summary>
    ///// 건물을 터치할 때
    ///// </summary>
    //void TouchBuilding()
    //{
    //    if (Input.GetMouseButtonDown(0) && !uiManager.isOpenPanel)
    //    {
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit = new RaycastHit();

    //        if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))
    //        {
    //            if (hit.collider.CompareTag("Building") && hit.collider.name == this.name)
    //            {
    //                SetCamTargetThis();
    //                ShowObjWindow();
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// 수동 획득 버튼 클릭
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

            if(!isAuto)            // 수동 상태이면 UI 클릭을 기다림
            {
                getGoldSlider.gameObject.SetActive(false);

                yield return StartCoroutine(WaitForManualAcquisition());

                getGoldSlider.gameObject.SetActive(true);
                getGoldBtn.SetActive(false);
            }

            gameManager.MyGold += GoldManager.UnitToBigInteger(IncrementGold);      // 골드 획득
        }
    }

    /// <summary>
    /// 정해진 시간만큼 카운트
    /// </summary>
    IEnumerator TimeCount()
    {
        Count = 0;

        for (int i = 0; i <= Second; i++)
        {
            yield return waitForSecond1;

            Count++;
        }
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

            if (isAuto)         // 수동 획득 대기 중에 알바를 고용할 때
            {
                break;
            }
        }

        yield return new WaitForSeconds(0.13f);

        isClickGetBtn = false;
    }
    #endregion

    #region 유니티 메소드

    private void Awake()
    {
        gameManager = GameManager.Instance;
        cameraMovement = CameraMovement.Instance;
        uiManager = UIManager.Instance;
        window = uiManager.clickObjWindow.transform.GetComponent<ClickObjWindow>();

    }
   
    //void Update()
    //{
    //    TouchBuilding();
    //}
    #endregion
}
