/**
 * @brief 산타 알바
 * @author 김미성
 * @date 22-04-20
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

public class Santa : MonoBehaviour
{
    #region 변수

    [HideInInspector]
    public int index;

    public Object santaObj;

    public int Level
    {
        get { return santaObj.santaLevel; }
        set { santaObj.santaLevel = value; }
    }

    public float MultiplySantaPrice       // 업그레이드 후 산타 가격 증가 배율
    {
        get { return santaObj.multiplySantaPrice; }
    }

    public string SantaPrice               // 산타 가격 
    {
        get { return santaObj.santaPrice; }
        set { santaObj.santaPrice = value; }
    }

    public int SantaEfficiency             // 알바 효율
    {
        get { return santaObj.santaEfficiency; }
        set { santaObj.santaEfficiency = value; }
    }

    public string SantaName
    {
        get { return santaObj.santaName; }
    }


    public Building Building
    {
        get{ return ObjectManager.Instance.buildingList[index].GetComponent<Building>(); }
    }

    public Sprite SantaSprite
    {
        get { return santaObj.santaSprite; }
    }

    private int questID = 3;

    bool isInit = false;

    // 캐싱
    private GameManager gameManager;
    private UIManager uiManager;
    private ClickObjWindow window;
    private QuestManager questManager;
    private CameraMovement cameraMovement;
    #endregion

    #region 유니티 함수
    void Awake()
    {
        gameManager = GameManager.Instance;
        uiManager = UIManager.Instance;
        window = UIManager.Instance.clickObjWindow.transform.GetComponent<ClickObjWindow>();
        questManager = QuestManager.Instance;
        cameraMovement = CameraMovement.Instance;
    }

    void Start()
    {
        StartCoroutine(Touch());
    }
    #endregion

    #region 코루틴
    /// <summary>
    /// 산타 터치를 감지
    /// </summary>
    /// <returns></returns>
    IEnumerator Touch()
    {
        while (true)
        {
            TouchSanta();

            yield return null;
        }
    }
    #endregion

    #region 함수
    /// <summary>
    /// 초기 설정
    /// </summary>
    public void Init()
    {
        gameObject.SetActive(true);

        Building.isAuto = true;     // 골드 자동화 시작
    }

    /// <summary>
    /// 새로운 산타 생성
    /// </summary>
    public void NewSanta()
    {
        Init();

        isInit = true;

        Level = 0;
        Upgrade();

        isInit = false;

        SetCamTargetThis();                 // 카메라가 산타를 따라다니도록

        ShowObjWindow();                    // 클릭 오브젝트창 보여줌
    }


    /// <summary>
    /// 산타 업그레이드
    /// </summary>
    public bool Upgrade()
    {
        if (!GoldManager.CompareBigintAndUnit(gameManager.MyCarrots, SantaPrice))   // 가진 당근으로 산타를 업그레이드 할 수 없다면
            return false;
        
        if (!isInit)
        {
            QuestManagerInstance().Success(questID);        // 퀘스트 성공
        }

        gameManager.MyCarrots -= GoldManager.UnitToBigInteger(SantaPrice);          // 비용 지불

        gameManager.IncreaseGauge(3);       // 게이지 증가

        SantaPrice = GoldManager.MultiplyUnit(SantaPrice, MultiplySantaPrice);      // 비용을 배율만큼 증가

        // 산타의 효율만큼 건물의 골드 증가량을 증가
        Building.IncrementGold = GoldManager.MultiplyUnit(Building.IncrementGold, 1 + (SantaEfficiency * 0.001f));  

        Level++;

        return true;
    }

    /// <summary>
    /// 카메라가 해당 산타를 따라다님
    /// </summary>
    public void SetCamTargetThis()
    {
        CameraMovementInstance().ChaseSanta(this.transform);
    }

    /// <summary>
    /// 클릭 오브젝트 창 보여줌
    /// </summary>
    public void ShowObjWindow()
    {
        window.Santa = this;

        uiManager.ShowClickObjWindow();
    }

    /// <summary>
    /// 산타 터치 시 카메라의 타깃을 산타로 설정, 클릭 오브젝트창을 보여줌
    /// </summary>
    void TouchSanta()
    {
        if (Input.GetMouseButtonDown(0) && !uiManager.isOpenPanel)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))
            {
                if (hit.collider.CompareTag("Santa") && hit.collider.name.Equals(this.name))
                {
                    SetCamTargetThis();
                    ShowObjWindow();
                }
            }
        }
    }

    /// <summary>
    /// 퀘스트 매니저 인스턴스 반환
    /// </summary>
    /// <returns></returns>
    QuestManager QuestManagerInstance()
    {
        if (!questManager)
        {
            questManager = QuestManager.Instance;
        }

        return questManager;
    }

    /// <summary>
    /// 카메라 무브먼트 인스턴스 반환
    /// </summary>
    /// <returns></returns>
    CameraMovement CameraMovementInstance()
    {
        if (!cameraMovement)
        {
            cameraMovement = CameraMovement.Instance;
        }

        return cameraMovement;
    }
    #endregion
}
