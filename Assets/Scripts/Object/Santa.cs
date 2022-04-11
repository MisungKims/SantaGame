using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

public class Santa : MonoBehaviour
{
    #region 변수
    private int level = 1;
    public int Level
    {
        get { return level; }
        set { level = value; }
    }


    [SerializeField]
    private Slider getGoldSlider;
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

    private float second;
    public float Second
    {
        set
        {
            second = value;
            getGoldSlider.maxValue = second;
        }
    }
    

    private float multiplySantaPrice;       // 업그레이드 후 산타 가격 증가 배율
    public float MultiplySantaPrice
    {
        //get { return multiplySantaPrice; }
        set { multiplySantaPrice = value; }
    }

    private string santaPrice;                 // 산타 가격 
    public string SantaPrice
    {
        get { return santaPrice; }
        set { santaPrice = value; }
    }

    private int santaEfficiency;
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
    private static WaitForSeconds m_waitForSecond;

    private GameManager gameManager;
    private CameraMovement cameraMovement;

    private ClickObjWindow window;

    #endregion

    #region 함수


    // 산타 초기 설정
    public void InitSanta(int index, string santaName, float second, float multiplySantaPrice, string santaPrice, int santaEfficiency, Building building)
    {
        this.index = index;
        this.santaName = santaName;
        Second = second;
        this.multiplySantaPrice = multiplySantaPrice;
        this.santaPrice = santaPrice;
        this.santaEfficiency = santaEfficiency;
        this.building = building;

        gameObject.SetActive(true);         // 산타가 보이도록
        getGoldSlider.transform.parent.gameObject.SetActive(true);   // 골드 자동 슬라이더가 보이도록

        SetCamTargetThis();                 // 카메라가 산타를 따라다니도록

        ShowObjWindow();                    // 클릭 오브젝트창 보여줌

        StartCoroutine(Increment());        // 골드획득 자동화 시작

        m_waitForSecond = new WaitForSeconds(second);
    }


    // 산타 업그레이드
    public void Upgrade()
    {
        if (!GoldManager.CompareBigintAndUnit(gameManager.MyCarrots, santaPrice))
        {
            return;
        }

        gameManager.MyCarrots -= GoldManager.UnitToBigInteger(santaPrice);

        santaPrice = GoldManager.MultiplyUnit(santaPrice, multiplySantaPrice);

        building.IncrementGold = GoldManager.MultiplyUnit(building.IncrementGold, 1 + (santaEfficiency * 0.001f));

        level++;
    }

    // 카메라가 해당 산타를 따라다님
    public void SetCamTargetThis()
    {
        cameraMovement.chasingSanta = this.transform;
        cameraMovement.StartChaseTarget();
    }

    // 클릭 오브젝트 창 보여줌
    public void ShowObjWindow()
    {
        window = gameManager.clickObjWindow.transform.GetComponent<ClickObjWindow>();

        window.Santa = this;
        window.SetSantaInfo();

        gameManager.ShowClickObjWindow();
    }

    // 산타 터치 시 카메라의 Target을 해당 산타로 set
    void TouchSanta()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))
            {
                if (hit.collider.CompareTag("Santa"))
                {
                    SetCamTargetThis();
                    ShowObjWindow();
                }
            }
        }
    }

    // 골드 획득 자동화
    IEnumerator Increment()
    {
        while (true)
        {
            yield return StartCoroutine(TimeCount());
           
            gameManager.MyGold += GoldManager.UnitToBigInteger(building.IncrementGold);
        }
    }

    // 정해진 시간만큼 카운트
    IEnumerator TimeCount()
    {
        for (int i = 0; i <= second; i++)
        {
            yield return new WaitForSeconds(1f);

            Count++;
        }
        Count = 0;
    }
    #endregion

    #region 유니티 메소드
    void Awake()
    {
        //anim = GetComponent<Animator>();

        gameManager = GameManager.Instance;
        cameraMovement = CameraMovement.Instance;
    }

   
    void Update()
    {
        TouchSanta();
    }
    #endregion

}
