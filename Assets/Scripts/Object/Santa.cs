/**
 * @brief ��Ÿ �˹ٸ� ����
 * @author ��̼�
 * @date 22-04-19
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

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

    private float multiplySantaPrice;       // ���׷��̵� �� ��Ÿ ���� ���� ����
    public float MultiplySantaPrice
    {
        set { multiplySantaPrice = value; }
    }

    private string santaPrice;               // ��Ÿ ���� 
    public string SantaPrice
    {
        get { return santaPrice; }
        set { santaPrice = value; }
    }

    private int santaEfficiency;             // �˹� ȿ��
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
   
    private GameManager gameManager;
    private UIManager uiManager;
    private ClickObjWindow window;

    #endregion

    #region �Լ�

    /// <summary>
    /// ��Ÿ �ʱ� ����
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
        
        SetCamTargetThis();                 // ī�޶� ��Ÿ�� ����ٴϵ���

        ShowObjWindow();                    // Ŭ�� ������Ʈâ ������
    }


    /// <summary>
    /// ��Ÿ ���׷��̵�
    /// </summary>
    public void Upgrade()
    {
        if (!GoldManager.CompareBigintAndUnit(gameManager.MyCarrots, santaPrice))   // ���� ������� ��Ÿ�� ���׷��̵� �� �� ���ٸ�
            return;

        gameManager.MyCarrots -= GoldManager.UnitToBigInteger(santaPrice);          // ��� ����

        santaPrice = GoldManager.MultiplyUnit(santaPrice, multiplySantaPrice);      // ����� ������ŭ ����

        // ��Ÿ�� ȿ����ŭ �ǹ��� ��� �������� ����
        building.IncrementGold = GoldManager.MultiplyUnit(building.IncrementGold, 1 + (santaEfficiency * 0.001f));  

        level++;
    }

    /// <summary>
    /// ī�޶� �ش� ��Ÿ�� ����ٴ�
    /// </summary>
    public void SetCamTargetThis()
    {
        CameraMovement.Instance.ChaseSanta(this.transform);
    }

    /// <summary>
    /// Ŭ�� ������Ʈ â ������
    /// </summary>
    public void ShowObjWindow()
    {
        window.Santa = this;
        window.SetSantaInfo();

        uiManager.ShowClickObjWindow();
    }


    /// <summary>
    /// ��Ÿ ��ġ �� ī�޶��� Ÿ���� ��Ÿ�� ����, Ŭ�� ������Ʈâ�� ������
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

    #region ����Ƽ �޼ҵ�
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
