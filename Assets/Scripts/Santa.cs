using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    Animator anim;

    private float multiplySantaPrice;       // ���׷��̵� �� ��Ÿ ���� ���� ����
    public float MultiplySantaPrice
    {
        //get { return multiplySantaPrice; }
        set { multiplySantaPrice = value; }
    }

    private int santaPrice;                 // ��Ÿ ���� 
    public int SantaPrice
    {
        //get { return santaPrice; }
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
        //get { return amountObtained; }
        set { amountObtained = value; }
    }

    [SerializeField]
    public Building building;

    string santaName;

    int index;

    public static readonly WaitForSeconds m_waitForSecond1s = new WaitForSeconds(1f); // ĳ��
    #endregion

    #region �Լ�


    /// <summary>
    /// ��Ÿ �ʱ�ȭ
    /// </summary>
    /// <param name="santaName">��Ÿ�� �̸�</param>
    public void InitSanta(int index, string santaName, float multiplySantaPrice, int santaPrice, float multiplyAmountObtained, float amountObtained)
    {
        name += " " + santaName;            // ��Ÿ �̸� ����

        this.index = index;
        this.santaName = santaName;
        this.multiplySantaPrice = multiplySantaPrice;
        this.santaPrice = santaPrice;
        this.multiplyAmountObtained = multiplyAmountObtained;
        this.amountObtained = amountObtained;

        gameObject.SetActive(true);         // ��Ÿ�� ���̵���

        anim.SetInteger("SantaIndex", index);     // � ��Ÿ�� �ҷ��ð���

        SetCamTargetThis();                 // ī�޶� ��Ÿ�� ����ٴϵ���

        ShowObjWindow();

        StartCoroutine(Increment());
    }

    /// <summary>
    /// ��Ÿ ���׷��̵�
    /// </summary>
    public void Upgrade()
    {
        GameManager.Instance.MyGold -= santaPrice;

        santaPrice = (int)(santaPrice * multiplySantaPrice);    // ����� ������ŭ ����
        StorePanel.Instance.ObjectList[index].santaPrice = santaPrice;

        amountObtained = (int)(amountObtained * multiplyAmountObtained);            // ���� ȹ�淮�� ������ŭ ����
        StorePanel.Instance.ObjectList[index].amountObtained = amountObtained;

        level++;
    }

    /// <summary>
    /// ī�޶� �ش� ��Ÿ�� ����ٴ�
    /// </summary>
    public void SetCamTargetThis()
    {
        CameraMovement.Instance.StartChaseTarget();
        CameraMovement.Instance.chasingSanta = this.transform;
    }

    public void ShowObjWindow()
    {
        ClickObjWindow window = GameManager.Instance.clickObjWindow.transform.GetComponent<ClickObjWindow>();
        window.Set(santaName, level, santaPrice, "��� ȹ�淮 " + amountObtained.ToString() + "% ����");

        GameManager.Instance.ShowClickObjWindow();
    }

    /// <summary>
    /// ��Ÿ ��ġ �� ī�޶��� Target�� �ش� ��Ÿ�� set
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

    #region ����Ƽ �޼ҵ�
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        TouchSanta();
    }
    #endregion

}
