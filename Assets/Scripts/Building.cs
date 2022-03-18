using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    #region 변수
    public GameObject clickObjWindow;

    [SerializeField]
    private int level = 1;
    public int Level
    {
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
    //public int BuildingPrice
    //{
    //    //get { return buildingPrice; }
    //    //set { buildingPrice = value; }
    //}

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

    private Vector3 distance;

    //[SerializeField]
    //private Santa santa;
    //public Santa Santa
    //{
    //    get { return santa; }
    //    set { santa = value; }
    //}

    private string buildingName;

    private int index;

    
    #endregion

    #region 함수

    /// <summary>
    /// 건물 초기화
    /// </summary>
    /// <param name="buildingName">산타의 이름</param>
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

    public void Upgrade()
    {
        GameManager.Instance.MyGold -= buildingPrice;

        buildingPrice = (int)(buildingPrice * multiplyBuildingPrice);    // 비용을 배율만큼 증가
        StorePanel.Instance.ObjectList[index].buildingPrice = buildingPrice;

        incrementGold = (int)(incrementGold * multiplyGold);            // 코인 증가량을 배율만큼 증가
        StorePanel.Instance.ObjectList[index].incrementGold = incrementGold;

        level++;
    }

    /// <summary>
    /// 카메라가 해당 산타를 따라다님
    /// </summary>
    public void SetCamTargetThis()
    {
        CameraMovement.Instance.StartChaseTarget();
        CameraMovement.Instance.chasingBuilding = this.transform;
        CameraMovement.Instance.buildingDistance = distance;
    }

    /// <summary>
    /// 오브젝트 정보창 보여줌
    /// </summary>
    public void ShowObjWindow()
    {
        ClickObjWindow window = GameManager.Instance.clickObjWindow.transform.GetComponent<ClickObjWindow>();
        window.Set(buildingName, level, buildingPrice, "+ " + incrementGold.ToString());
       
        GameManager.Instance.ShowClickObjWindow();
    }

    /// <summary>
    /// 건물 터치 시 카메라의 Target을 해당 건물로 set
    /// </summary>
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
    }

    void Update()
    {
        TouchBuilding();

        //if (this.gameObject.activeSelf)
        //{
        //    GameManager.Instance.MyGold += IncrementGold * Time.deltaTime;
        //}
    }
    #endregion

}
