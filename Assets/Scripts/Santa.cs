using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Santa : MonoBehaviour
{
    #region ����
    [SerializeField]
    private int level = 1;
    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    private float second;
  
    private float multiplySantaPrice;       // ���׷��̵� �� ��Ÿ ���� ���� ����
    public float MultiplySantaPrice
    {
        //get { return multiplySantaPrice; }
        set { multiplySantaPrice = value; }
    }

    private int santaPrice;                 // ��Ÿ ���� 
    public int SantaPrice
    {
        get { return santaPrice; }
        set { santaPrice = value; }
    }

    private float multiplyAmountObtained;   // ���׷��̵� �� ȹ�淮 ���� ����
    public float MultiplyAmountObtained
    {
        //get { return multiplyAmountObtained; }
        set { multiplyAmountObtained = value; }
    }

    private float amountObtained;           // ȹ�淮 ����
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

    // ĳ��
    private static WaitForSeconds m_waitForSecond1s;

    private StoreObjectSc objectList;
    private GameManager gameManager;
    private CameraMovement cameraMovement;

    private ClickObjWindow window;

    #endregion

    #region �Լ�


    // ��Ÿ �ʱ� ����
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


        gameObject.SetActive(true);         // ��Ÿ�� ���̵���

        SetCamTargetThis();                 // ī�޶� ��Ÿ�� ����ٴϵ���

        ShowObjWindow();                    // Ŭ�� ������Ʈâ ������

        StartCoroutine(Increment());        // ���ȹ�� �ڵ�ȭ ����

        m_waitForSecond1s = new WaitForSeconds(second);
    }


    // ��Ÿ ���׷��̵�
    public void Upgrade()
    {
        if (gameManager.MyGold < santaPrice)
        {
            return;
        }

        gameManager.MyGold -= santaPrice;

        santaPrice = (int)(santaPrice * multiplySantaPrice);    // ����� ������ŭ ����
        objectList.santaPrice = santaPrice;

        amountObtained = (int)(amountObtained * multiplyAmountObtained);            // ���� ȹ�淮�� ������ŭ ����
        objectList.amountObtained = amountObtained;

        level++;
    }

    // ī�޶� �ش� ��Ÿ�� ����ٴ�
    public void SetCamTargetThis()
    {
        cameraMovement.chasingSanta = this.transform;
        cameraMovement.StartChaseTarget();
    }

    // Ŭ�� ������Ʈ â ������
    public void ShowObjWindow()
    {
        window = gameManager.clickObjWindow.transform.GetComponent<ClickObjWindow>();

        window.Santa = this;
        window.SetSantaInfo();

        gameManager.ShowClickObjWindow();
    }

    // ��Ÿ ��ġ �� ī�޶��� Target�� �ش� ��Ÿ�� set
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

    // ��� ȹ�� �ڵ�ȭ
    IEnumerator Increment()
    {
        while (true)
        {
            yield return m_waitForSecond1s;

            gameManager.MyGold += building.IncrementGold;
        }

    }
    #endregion

    #region ����Ƽ �޼ҵ�
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
