using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Building : MonoBehaviour
{
    #region 변수
    public GameObject clickObjWindow;

    private Transform thisTransform;

    [SerializeField]
    private int level = 1;
    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    private float multiplyBuildingPrice;    // 업그레이드 후 건물 가격 증가 배율

    private string buildingPrice;              // 건물 가격 
    public string BuildingPrice
    {
        get { return buildingPrice; }
    }

    [SerializeField]
    private string incrementGold;              // 플레이어의 돈 증가량
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

    public Santa santa;

    private Vector3 distance;

    private ClickObjWindow window;

    StringBuilder sb = new StringBuilder();

    // 캐싱
    private GameManager gameManager;
    private CameraMovement cameraMovement;

    #endregion

    #region 함수

    /// <summary>
    /// 건물 초기 설정
    /// </summary>
    /// <param name="index">건물의 인덱스</param>
    /// <param name="name">건물의 이름</param>
    /// <param name="multiplyPrice">건물의 가격 증가 배수</param>
    /// <param name="price">건물의 가격</param>
    /// <param name="mGold">골드 증가량 배수</param>
    /// <param name="iGold">골드 증가량</param>
    public void InitBuilding(int index, string name, float multiplyPrice, string price, string iGold)
    {
        gameObject.SetActive(true);         // 건물이 보이도록

        this.index = index;
        buildingName = name;
        multiplyBuildingPrice = multiplyPrice;
        buildingPrice = price;
        incrementGold = iGold;

        SetCamTargetThis();                 // 카메라가 산타를 따라다니도록

        ShowObjWindow();                    // 오브젝트 정보 창이 보이도록
    }

    // 건물 업그레이드
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

    // 카메라가 해당 건물을 따라다님
    public void SetCamTargetThis()
    {
        cameraMovement.ChaseBuilding(thisTransform, thisTransform.GetChild(0).transform);
    }

    // 오브젝트 정보창 보여줌
    public void ShowObjWindow()
    {
        window = gameManager.clickObjWindow.transform.GetComponent<ClickObjWindow>();

        sb.Clear();
        sb.Append("+ ");
        sb.Append(incrementGold.ToString());

        window.Builidng = this;
        window.SetBuildingInfo();

        gameManager.ShowClickObjWindow();
    }

    // 건물 터치 시 카메라의 Target을 해당 건물로 set
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


    #endregion

    #region 유니티 메소드

    private void Awake()
    {
        gameManager = GameManager.Instance;
        cameraMovement = CameraMovement.Instance;

        thisTransform = this.transform;
    }
   
    void Update()
    {
        TouchBuilding();
    }
    #endregion
}
