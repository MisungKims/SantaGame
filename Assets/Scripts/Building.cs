using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Building : MonoBehaviour
{
    #region 변수
    public GameObject clickObjWindow;

    [SerializeField]
    private int level = 1;
    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    [SerializeField]
    private float multiplyBuildingPrice;    // 업그레이드 후 건물 가격 증가 배율
    //public float MultiplyBuildingPrice
    //{
    //    //get { return multiplyBuildingPrice; }
    //    set { multiplyBuildingPrice = value; }
    //}

    [SerializeField]
    private int buildingPrice;              // 건물 가격 
    public int BuildingPrice
    {
        get { return buildingPrice; }
        //set { buildingPrice = value; }
    }

    [SerializeField]
    private float multiplyGold;             // 업그레이드 후 플레이어 돈 증가 배율
    //public float MultiplyGold
    //{
    //    //get { return multiplyGold; }
    //    set { multiplyGold = value; }
    //}

    [SerializeField]
    private int incrementGold;              // 플레이어의 돈 증가량
    public int IncrementGold
    {
        get { return incrementGold; }
        //set { incrementGold = value; }
    }


    private int index;
    public int Index
    {
        get { return index; }
        //set { incrementGold = value; }
    }

    private string buildingName;
    public string BuilidingName
    {
        get { return buildingName; }
    }

    private Vector3 distance;

    private ClickObjWindow window;

    StringBuilder sb = new StringBuilder();

    // 캐싱
    private StoreObjectSc objectList;
    private GameManager gameManagerInstance;
    private CameraMovement cameraInstance;

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
    public void InitBuilding(int index, string name, float multiplyPrice, int price, float mGold, int iGold)
    {
        gameObject.SetActive(true);         // 건물이 보이도록

        this.index = index;
        buildingName = name;
        multiplyBuildingPrice = multiplyPrice;
        buildingPrice = price;
        multiplyGold = mGold;
        incrementGold = iGold;

        SetCamTargetThis();                 // 카메라가 산타를 따라다니도록

        ShowObjWindow();                    // 오브젝트 정보 창이 보이도록
    }

    // 건물 업그레이드
    public void Upgrade()
    {
        if (gameManagerInstance.MyGold < buildingPrice)
        {
            return;
        }
        gameManagerInstance.MyGold -= buildingPrice;

        buildingPrice = (int)(buildingPrice * multiplyBuildingPrice);    // 비용을 배율만큼 증가
        objectList.buildingPrice = buildingPrice;

        incrementGold = (int)(incrementGold * multiplyGold);            // 코인 증가량을 배율만큼 증가
        objectList.incrementGold = incrementGold;

        level++;
    }

    // 카메라가 해당 산타를 따라다님
    public void SetCamTargetThis()
    {
        CameraMovement.Instance.StartChaseTarget();
        CameraMovement.Instance.chasingBuilding = this.transform;
        CameraMovement.Instance.buildingDistance = distance;
    }

    // 오브젝트 정보창 보여줌
    public void ShowObjWindow()
    {
        window = GameManager.Instance.clickObjWindow.transform.GetComponent<ClickObjWindow>();

        sb.Clear();
        sb.Append("+ ");
        sb.Append(incrementGold.ToString());

        window.Builidng = this;
        window.SetBuildingInfo();

        GameManager.Instance.ShowClickObjWindow();
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
                if (hit.collider.CompareTag("Building"))
                {
                    SetCamTargetThis();
                    ShowObjWindow();
                }
            }
        }
    }

   
    #endregion

    #region 유니티 메소드
    private void Start()
    {
        distance = this.transform.GetChild(0).localPosition;

        objectList = StorePanel.Instance.ObjectList[index];
        gameManagerInstance = GameManager.Instance;
        cameraInstance = CameraMovement.Instance;
    }

    void Update()
    {
        TouchBuilding();
    }
    #endregion
}
