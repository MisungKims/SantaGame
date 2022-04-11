using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

public class Santa : MonoBehaviour
{
    #region ����
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
    

    private float multiplySantaPrice;       // ���׷��̵� �� ��Ÿ ���� ���� ����
    public float MultiplySantaPrice
    {
        //get { return multiplySantaPrice; }
        set { multiplySantaPrice = value; }
    }

    private string santaPrice;                 // ��Ÿ ���� 
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

    // ĳ��
    private static WaitForSeconds m_waitForSecond;

    private GameManager gameManager;
    private CameraMovement cameraMovement;

    private ClickObjWindow window;

    #endregion

    #region �Լ�


    // ��Ÿ �ʱ� ����
    public void InitSanta(int index, string santaName, float second, float multiplySantaPrice, string santaPrice, int santaEfficiency, Building building)
    {
        this.index = index;
        this.santaName = santaName;
        Second = second;
        this.multiplySantaPrice = multiplySantaPrice;
        this.santaPrice = santaPrice;
        this.santaEfficiency = santaEfficiency;
        this.building = building;

        gameObject.SetActive(true);         // ��Ÿ�� ���̵���
        getGoldSlider.transform.parent.gameObject.SetActive(true);   // ��� �ڵ� �����̴��� ���̵���

        SetCamTargetThis();                 // ī�޶� ��Ÿ�� ����ٴϵ���

        ShowObjWindow();                    // Ŭ�� ������Ʈâ ������

        StartCoroutine(Increment());        // ���ȹ�� �ڵ�ȭ ����

        m_waitForSecond = new WaitForSeconds(second);
    }


    // ��Ÿ ���׷��̵�
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
            yield return StartCoroutine(TimeCount());
           
            gameManager.MyGold += GoldManager.UnitToBigInteger(building.IncrementGold);
        }
    }

    // ������ �ð���ŭ ī��Ʈ
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

    #region ����Ƽ �޼ҵ�
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
