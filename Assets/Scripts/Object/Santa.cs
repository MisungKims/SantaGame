/**
 * @brief 산타 알바를 생성
 * @author 김미성
 * @date 22-04-19
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

public class Santa : MonoBehaviour
{
    #region 변수
    [SerializeField]
    private int level = 1;
    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    private float multiplySantaPrice;       // 업그레이드 후 산타 가격 증가 배율
    public float MultiplySantaPrice
    {
        set { multiplySantaPrice = value; }
    }

    private string santaPrice;               // 산타 가격 
    public string SantaPrice
    {
        get { return santaPrice; }
        set { santaPrice = value; }
    }

    private int santaEfficiency;             // 알바 효율
    public int SantaEfficiency
    {
        get { return santaEfficiency; }
        set { santaEfficiency = value; }
    }

   
    string santaName;
    public string SantaName
    {
        get { return santaName; }
    }

    private int index;
    public int Index
    {
        get { return index; }
    }

    private Building building;

    StringBuilder sb = new StringBuilder();

    // 캐싱
   
    private GameManager gameManager;
    private UIManager uiManager;
    private ClickObjWindow window;

    #endregion

    #region 함수

    /// <summary>
    /// 산타 초기 설정
    /// </summary>

    public void InitSanta(int index, string santaName, float multiplySantaPrice, string santaPrice, int santaEfficiency, Building building)
    {
        this.index = index;
        this.santaName = santaName;
        this.multiplySantaPrice = multiplySantaPrice;
        this.santaPrice = santaPrice;
        this.santaEfficiency = santaEfficiency;
        this.building = building;

        this.building.isAuto = true;

        gameObject.SetActive(true);
        
        SetCamTargetThis();                 // 카메라가 산타를 따라다니도록

        ShowObjWindow();                    // 클릭 오브젝트창 보여줌
    }


    /// <summary>
    /// 산타 업그레이드
    /// </summary>
    public void Upgrade()
    {
        if (!GoldManager.CompareBigintAndUnit(gameManager.MyCarrots, santaPrice))   // 가진 당근으로 산타를 업그레이드 할 수 없다면
            return;

        gameManager.MyCarrots -= GoldManager.UnitToBigInteger(santaPrice);          // 비용 지불

        santaPrice = GoldManager.MultiplyUnit(santaPrice, multiplySantaPrice);      // 비용을 배율만큼 증가

        // 산타의 효율만큼 건물의 골드 증가량을 증가
        building.IncrementGold = GoldManager.MultiplyUnit(building.IncrementGold, 1 + (santaEfficiency * 0.001f));  

        level++;
    }

    /// <summary>
    /// 카메라가 해당 산타를 따라다님
    /// </summary>
    public void SetCamTargetThis()
    {
        CameraMovement.Instance.ChaseSanta(this.transform);
    }

    /// <summary>
    /// 클릭 오브젝트 창 보여줌
    /// </summary>
    public void ShowObjWindow()
    {
        window.Santa = this;
        window.SetSantaInfo();

        uiManager.ShowClickObjWindow();
    }


    /// <summary>
    /// 산타 터치 시 카메라의 타깃을 산타로 설정, 클릭 오브젝트창을 보여줌
    /// </summary>
    void TouchSanta()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))
            {
                if (hit.collider.CompareTag("Santa") && hit.collider.name == this.name)
                {
                    SetCamTargetThis();
                    ShowObjWindow();
                }
            }
        }
    }

    
    #endregion

    #region 유니티 메소드
    void Awake()
    {
        //anim = GetComponent<Animator>();
        
        gameManager = GameManager.Instance;
        uiManager = UIManager.Instance;
        window = uiManager.clickObjWindow.transform.GetComponent<ClickObjWindow>();
    }

   
    void Update()
    {
        TouchSanta();
    }
    #endregion

}
