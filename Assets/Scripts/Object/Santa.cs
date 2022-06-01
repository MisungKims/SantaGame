/**
 * @brief ��Ÿ �˹�
 * @author ��̼�
 * @date 22-04-20
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

public class Santa : MonoBehaviour
{
    #region ����

    [HideInInspector]
    public int index;

    public Object santaObj;

    public int Level
    {
        get { return santaObj.santaLevel; }
        set { santaObj.santaLevel = value; }
    }

    public float MultiplySantaPrice       // ���׷��̵� �� ��Ÿ ���� ���� ����
    {
        get { return santaObj.multiplySantaPrice; }
    }

    public string SantaPrice               // ��Ÿ ���� 
    {
        get { return santaObj.santaPrice; }
        set { santaObj.santaPrice = value; }
    }

    public int SantaEfficiency             // �˹� ȿ��
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

    // ĳ��
    private GameManager gameManager;
    private UIManager uiManager;
    private ClickObjWindow window;
    #endregion

    #region �Լ�

    public void Init()
    {
        gameObject.SetActive(true);

        Building.isAuto = true;     // ��� �ڵ�ȭ ����
    }

    public void NewSanta()
    {
        Init();

        isInit = true;

        Level = 0;
        Upgrade();

        isInit = false;

        SetCamTargetThis();                 // ī�޶� ��Ÿ�� ����ٴϵ���

        ShowObjWindow();                    // Ŭ�� ������Ʈâ ������
    }


    /// <summary>
    /// ��Ÿ ���׷��̵�
    /// </summary>
    public bool Upgrade()
    {
        if (!GoldManager.CompareBigintAndUnit(gameManager.MyCarrots, SantaPrice))   // ���� ������� ��Ÿ�� ���׷��̵� �� �� ���ٸ�
            return false;
        
        if (!isInit)
        {
            QuestManager.Instance.Success(questID);        // ����Ʈ ����
        }

        gameManager.MyCarrots -= GoldManager.UnitToBigInteger(SantaPrice);          // ��� ����

        SantaPrice = GoldManager.MultiplyUnit(SantaPrice, MultiplySantaPrice);      // ����� ������ŭ ����

        // ��Ÿ�� ȿ����ŭ �ǹ��� ��� �������� ����
        Building.IncrementGold = GoldManager.MultiplyUnit(Building.IncrementGold, 1 + (SantaEfficiency * 0.001f));  

        Level++;

        return true;
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

        uiManager.ShowClickObjWindow();
    }


    /// <summary>
    /// ��Ÿ ��ġ �� ī�޶��� Ÿ���� ��Ÿ�� ����, Ŭ�� ������Ʈâ�� ������
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

    
    #endregion

    #region ����Ƽ �޼ҵ�
    void Awake()
    {
        //anim = GetComponent<Animator>();
        
        gameManager = GameManager.Instance;
        uiManager = UIManager.Instance;
        window = UIManager.Instance.clickObjWindow.transform.GetComponent<ClickObjWindow>();
    }

   
    void Update()
    {
        TouchSanta();
    }
    #endregion

}
