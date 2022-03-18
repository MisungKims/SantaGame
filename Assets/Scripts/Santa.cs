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

    public static readonly WaitForSeconds m_waitForSecond1s = new WaitForSeconds(1f); // 캐싱

    StringBuilder sb = new StringBuilder();
    #endregion

    #region 함수


    /// <summary>
    /// 산타 초기화
    /// </summary>
    /// <param name="santaName">산타의 이름</param>
    public void InitSanta(int index, string santaName, float multiplySantaPrice, int santaPrice, float multiplyAmountObtained, float amountObtained, Building building)
    {
        name += " " + santaName;            // 산타 이름 설정

        this.index = index;
        this.santaName = santaName;
        this.multiplySantaPrice = multiplySantaPrice;
        this.santaPrice = santaPrice;
        this.multiplyAmountObtained = multiplyAmountObtained;
        this.amountObtained = amountObtained;
        this.building = building;

        gameObject.SetActive(true);         // 산타가 보이도록

        //anim.SetInteger("SantaIndex", index);     // 어떤 산타를 불러올건지

        SetCamTargetThis();                 // 카메라가 산타를 따라다니도록

        ShowObjWindow();

        StartCoroutine(Increment());
    }

    /// <summary>
    /// 산타 업그레이드
    /// </summary>
    public void Upgrade()
    {
        if (GameManager.Instance.MyGold < santaPrice)
        {
            return;
        }

        GameManager.Instance.MyGold -= santaPrice;

        santaPrice = (int)(santaPrice * multiplySantaPrice);    // 비용을 배율만큼 증가
        StorePanel.Instance.ObjectList[index].santaPrice = santaPrice;

        amountObtained = (int)(amountObtained * multiplyAmountObtained);            // 코인 획득량을 배율만큼 증가
        StorePanel.Instance.ObjectList[index].amountObtained = amountObtained;

        level++;
    }

    /// <summary>
    /// 카메라가 해당 산타를 따라다님
    /// </summary>
    public void SetCamTargetThis()
    {
        CameraMovement.Instance.StartChaseTarget();
        CameraMovement.Instance.chasingSanta = this.transform;
    }

    public void ShowObjWindow()
    {
        ClickObjWindow window = GameManager.Instance.clickObjWindow.transform.GetComponent<ClickObjWindow>();

        sb.Clear();
        sb.Append("골드 획득량 ");
        sb.Append(amountObtained.ToString());
        sb.Append("% 증가");

        window.santa = this;

        window.SetSantaInfo();

        GameManager.Instance.ShowClickObjWindow();
    }

    /// <summary>
    /// 산타 터치 시 카메라의 Target을 해당 산타로 set
    /// </summary>
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

    IEnumerator Increment()
    {
        while (true)
        {
            yield return m_waitForSecond1s;

            GameManager.Instance.MyGold += building.IncrementGold;
        }

    }
    #endregion

    #region 유니티 메소드
    //void Awake()
    //{
    //    //anim = GetComponent<Animator>();
    //}

    void Update()
    {
        TouchSanta();
    }
    #endregion

}
