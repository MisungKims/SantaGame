/**
 * @brief �ǹ�
 * @author ��̼�
 * @date 22-04-20
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class Building : MonoBehaviour
{
    #region ����

    private Transform thisTransform;

    public int index;
    public Object buildingObj;
    public string BuilidingName
    {
        get { return buildingObj.buildingName; }
    }

    // ������Ƽ
    public int Level
    {
        get { return buildingObj.buildingLevel; }
        set { buildingObj.buildingLevel = value;  }
    }

    public float MultiplyBuildingPrice        // ���׷��̵� �� �ǹ� ���� ���� ����
    {
        get { return buildingObj.multiplyBuildingPrice; }
    }

    public string BuildingPrice     // �ǹ� ���� 
    {
        get { return buildingObj.buildingPrice; }
        set { buildingObj.buildingPrice = value; }
    }

    public string IncrementGold              // �÷��̾��� ��� ������
    {
        get { return buildingObj.incrementGold; }
        set { buildingObj.incrementGold = value; }
    }

   
    public float Second
    {
        get { return buildingObj.second; }
        set 
        {
            buildingObj.second = (int)value;
            getGoldSlider.maxValue = (int)value;
        }
    }

    //public Santa santa;             // ����� �˹� (��Ÿ)

    // UI ����
    [SerializeField]
    private Slider getGoldSlider;       // ��� ȹ�� �����̴�
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

    [SerializeField]
    private GameObject getGoldBtn;      // ��� ���� ȹ�� ��ư

   
    // �� �� ����

    public bool isAuto = false;     // �˹ٸ� ����ߴ���? (�˹ٸ� ����ߴٸ� ��� ȹ�� �ڵ�ȭ)

    private bool isClickGetBtn = false;   // ȹ�� UI�� Ŭ�������� true, �ƴϸ� false (��� ȹ�� ������ ��)

    private Vector3 distance;       // ī�޶���� �Ÿ�


    // ĳ��
    private GameManager gameManager;
    private CameraMovement cameraMovement;
    private UIManager uiManager;
    private ClickObjWindow window;
    private static WaitForSeconds waitForSecond1 = new WaitForSeconds(1f);

    #endregion

    #region �Լ�

    /// <summary>
    /// �ǹ� ���� ��
    /// </summary>
    public void NewBuilding()
    {
        gameObject.SetActive(true);         // �ǹ��� ���̵���

        SetCamTargetThis();                 // ī�޶� �ǹ��� �ٶ󺸵���

        ShowObjWindow();                    // ������Ʈ ���� â�� ���̵���

        StartCoroutine(Increment());        // ���ȹ�� ����
    }

    /// <summary>
    /// �ǹ� ���׷��̵�
    /// </summary>
    public bool Upgrade()
    {
        if (!GoldManager.CompareBigintAndUnit(gameManager.MyGold, BuildingPrice))
        {
            return false;
        }

        gameManager.MyGold -= GoldManager.UnitToBigInteger(BuildingPrice);  // ���׷��̵� ��� ����

        BuildingPrice = GoldManager.MultiplyUnit(BuildingPrice, MultiplyBuildingPrice); // ����� ������ŭ ����

        IncrementGold = GoldManager.MultiplyUnit(IncrementGold, 1.1f * gameManager.goldEfficiency);  // ��� �������� ������ŭ ����

        Level++;

        return true;
    }


    /// <summary>
    /// ī�޶� �ش� �ǹ��� ����ٴ�
    /// </summary>
    public void SetCamTargetThis()
    {
        cameraMovement.ChaseBuilding(thisTransform, thisTransform.GetChild(0).transform);
    }

   
    /// <summary>
    /// ������Ʈ ����â ������
    /// </summary>
    public void ShowObjWindow()
    {
        window.clickedObj = buildingObj;
        window.Builidng = this;

        uiManager.ShowClickObjWindow();
    }

 
    /// <summary>
    /// �ǹ��� ��ġ�� ��
    /// </summary>
    void TouchBuilding()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))
            {
                if (hit.collider.CompareTag("Building") && hit.collider.name == this.name)
                {
                    SetCamTargetThis();
                    ShowObjWindow();
                }
            }
        }
    }

    /// <summary>
    /// ���� ȹ�� ��ư Ŭ�� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void ClickGetBtn()
    {
        isClickGetBtn = true;
    }

    #endregion

    #region �ڷ�ƾ

    /// <summary>
    /// ������ �ð���ŭ ī��Ʈ
    /// </summary>
    IEnumerator TimeCount()
    {
        for (int i = 0; i <= Second; i++)
        {
            yield return waitForSecond1;

            Count++;
        }
        Count = 0;
    }

    /// <summary>
    /// ��� ȹ�� Ÿ�̸�
    /// </summary>
    IEnumerator Increment()
    {
        getGoldBtn.SetActive(false);
        while (true)
        {
            yield return StartCoroutine(TimeCount());

            if (isAuto)     // �ڵ�ȭ �����̸� �ٷ� ��� ȹ��
            {
                gameManager.MyGold += GoldManager.UnitToBigInteger(IncrementGold);
            }
            else            // ���� �����̸� UI Ŭ���� ��ٸ�
            {
                getGoldSlider.gameObject.SetActive(false);

                yield return StartCoroutine(WaitForManualAcquisition());

                getGoldSlider.gameObject.SetActive(true);
                getGoldBtn.SetActive(false);
            }
        }
    }


    /// <summary>
    /// ����� ���� ȹ���� ��� (UI ��ġ�� ��ٸ�)
    /// </summary>
    IEnumerator WaitForManualAcquisition()
    {
        getGoldBtn.SetActive(true);

        while (!isClickGetBtn)
        {
            yield return null;

            if (isAuto)         // ���� ȹ�� ��� �߿� �˹ٸ� ����� ��
            {
                gameManager.MyGold += GoldManager.UnitToBigInteger(IncrementGold);
                break;
            }
        }
    }
    #endregion

    #region ����Ƽ �޼ҵ�

    private void Awake()
    {
        gameManager = GameManager.Instance;
        cameraMovement = CameraMovement.Instance;
        uiManager = UIManager.Instance;
        window = uiManager.clickObjWindow.transform.GetComponent<ClickObjWindow>();

        thisTransform = this.transform;

    }
   
    void Update()
    {
        TouchBuilding();
    }
    #endregion
}
