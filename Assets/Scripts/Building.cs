using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    #region 변수

    [SerializeField]
    private int level = 1;
    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    private float multiplyBuildingPrice;    // 업그레이드 후 건물 가격 증가 배율
    public float MultiplyBuildingPrice
    {
        get { return multiplyBuildingPrice; }
        set { multiplyBuildingPrice = value; }
    }

    private int buildingPrice;              // 건물 가격 
    public int BuildingPrice
    {
        get { return buildingPrice; }
        set { buildingPrice = value; }
    }

    private float multiplyGold;             // 업그레이드 후 플레이어 돈 증가 배율
    public float MultiplyGold
    {
        get { return multiplyGold; }
        set { multiplyGold = value; }
    }

    private int incrementGold;              // 플레이어의 돈 증가량
    public int IncrementGold
    {
        get { return incrementGold; }
        set { incrementGold = value; }
    }

    private Vector3 distance;

    #endregion

    #region 함수

    /// <summary>
    /// 건물 초기화
    /// </summary>
    /// <param name="buildingName">산타의 이름</param>
    public void InitBuilding(float multiplyPrice, int price, float mGold, int iGold)
    {
        SetCamTargetThis();                 // 카메라가 산타를 따라다니도록

        gameObject.SetActive(true);         // 산타가 보이도록

        multiplyBuildingPrice = multiplyPrice;
        buildingPrice = price;
        multiplyGold = mGold;
        incrementGold = iGold;
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
                    Debug.Log("빌딩");
                    SetCamTargetThis();
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
    }
    #endregion

}
