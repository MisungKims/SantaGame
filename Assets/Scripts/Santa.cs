using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

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

    private float second;
  
    private float multiplySantaPrice;       // 업그레이드 후 산타 가격 증가 배율
    public float MultiplySantaPrice
    {
        //get { return multiplySantaPrice; }
        set { multiplySantaPrice = value; }
    }

    private int santaPrice;                 // 산타 가격 
    public int SantaPrice
    {
        get { return santaPrice; }
        set { santaPrice = value; }
    }

    private float multiplyAmountObtained;   // 업그레이드 후 획득량 증가 배율
    public float MultiplyAmountObtained
    {
        //get { return multiplyAmountObtained; }
        set { multiplyAmountObtained = value; }
    }

    private float amountObtained;           // 획득량 증가
    public float AmountObtained
    {
        get { return amountObtained; }
        set { amountObtained = value; }
    }

    [SerializeField]
    public Building building;

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

    StringBuilder sb = new StringBuilder();

    // 캐싱
    private static WaitForSeconds m_waitForSecond1s;

    private StoreObjectSc objectList;
    private GameManager gameManager;
    private CameraMovement cameraMovement;

    private ClickObjWindow window;

    #endregion

    #region 함수


    // 산타 초기 설정
    public void InitSanta(int index, string santaName, float second, float multiplySantaPrice, int santaPrice, float multiplyAmountObtained, float amountObtained, Building building)
    {
        
        this.index = index;
        this.santaName = santaName;
        this.second = second;
        this.multiplySantaPrice = multiplySantaPrice;
        this.santaPrice = santaPrice;
        this.multiplyAmountObtained = multiplyAmountObtained;
        this.amountObtained = amountObtained;
        this.building = building;

        //this.transform.position = this.building.transform.GetChild(1).position;


        gameObject.SetActive(true);         // 산타가 보이도록

        SetCamTargetThis();                 // 카메라가 산타를 따라다니도록

        ShowObjWindow();                    // 클릭 오브젝트창 보여줌

        StartCoroutine(Increment());        // 골드획득 자동화 시작

        m_waitForSecond1s = new WaitForSeconds(second);
    }


    // 산타 업그레이드
    public void Upgrade()
    {
        if (gameManager.MyGold < santaPrice)
        {
            return;
        }

        gameManager.MyGold -= santaPrice;

        santaPrice = (int)(santaPrice * multiplySantaPrice);    // 비용을 배율만큼 증가
        objectList.santaPrice = santaPrice;

        amountObtained = (int)(amountObtained * multiplyAmountObtained);            // 코인 획득량을 배율만큼 증가
        objectList.amountObtained = amountObtained;

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
            yield return m_waitForSecond1s;

            gameManager.MyGold += building.IncrementGold;
        }

    }
    #endregion

    #region 유니티 메소드
    void Awake()
    {
        //anim = GetComponent<Animator>();

        objectList = StorePanel.Instance.ObjectList[index];
        gameManager = GameManager.Instance;
        cameraMovement = CameraMovement.Instance;
    }

   
    void Update()
    {
        TouchSanta();
    }
    #endregion

}
