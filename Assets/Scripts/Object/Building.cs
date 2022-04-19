/**
 * @brief �ǹ��� ����
 * @author ��̼�
 * @date 22-04-19
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

    [SerializeField]
    private int level = 1;
    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    private float multiplyBuildingPrice;        // ���׷��̵� �� �ǹ� ���� ���� ����

    private string buildingPrice;              // �ǹ� ���� 
    public string BuildingPrice
    {
        get { return buildingPrice; }
    }

    private string incrementGold;              // �÷��̾��� ��� ������
    public string IncrementGold
    {
        get { return incrementGold; }
        set { incrementGold = value; }
    }

    private int index;
    public int Index
    {
        get { return index; }
    }

    private string buildingName;
    public string BuilidingName
    {
        get { return buildingName; }
    }

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

    private float second;
    public float Second
    {
        set
        {
            second = value;
            getGoldSlider.maxValue = second;
        }
    }

    public Santa santa;         // ����� �˹� (��Ÿ)

    public bool isAuto = false;    // �˹ٸ� ����� �ǹ��� ��� ȹ�� �ڵ�ȭ

    private bool isClickGetBtn = false;   // ��� ȹ���� ������ ��, ȹ�� UI�� Ŭ�������� true, �ƴϸ� false

    private Vector3 distance;       // ī�޶���� �Ÿ�

    StringBuilder sb = new StringBuilder();

    // ĳ��
    private GameManager gameManager;
    private CameraMovement cameraMovement;
    private UIManager uiManager;
    private ClickObjWindow window;
    private static WaitForSeconds waitForSecond1 = new WaitForSeconds(1f);

    #endregion

    #region �Լ�

    /// <summary>
    /// �ǹ� �ʱ� ����
    /// </summary>
    /// <param name="index">�ǹ��� �ε���</param>
    /// <param name="name">�ǹ��� �̸�</param>
    /// <param name="multiplyPrice">�ǹ��� ���� ���� ���</param>
    /// <param name="price">�ǹ��� ����</param>
    /// <param name="iGold">��� ������</param>
    public void InitBuilding(int index, string name, float multiplyPrice, string price, string iGold, float second)
    {
        gameObject.SetActive(true);         // �ǹ��� ���̵���

        this.index = index;
        buildingName = name;
        multiplyBuildingPrice = multiplyPrice;
        buildingPrice = price;
        incrementGold = iGold;
        Second = second;

        SetCamTargetThis();                 // ī�޶� �ǹ��� �ٶ󺸵���

        ShowObjWindow();                    // ������Ʈ ���� â�� ���̵���

        StartCoroutine(Increment());        // ���ȹ�� ����
    }

    /// <summary>
    /// �ǹ� ���׷��̵�
    /// </summary>
    public void Upgrade()
    {
        if (!GoldManager.CompareBigintAndUnit(gameManager.MyGold, buildingPrice))
        {
            return;
        }

        gameManager.MyGold -= GoldManager.UnitToBigInteger(buildingPrice);  // ���׷��̵� ��� ����

        buildingPrice = GoldManager.MultiplyUnit(buildingPrice, multiplyBuildingPrice); // ����� ������ŭ ����

        incrementGold = GoldManager.MultiplyUnit(incrementGold, 1.1f * gameManager.goldEfficiency);  // ��� �������� ������ŭ ����

        level++;
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
        sb.Clear();
        sb.Append("+ ");
        sb.Append(incrementGold.ToString());

        window.Builidng = this;
        window.SetBuildingInfo();

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
                gameManager.MyGold += GoldManager.UnitToBigInteger(incrementGold);
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
    /// ������ �ð���ŭ ī��Ʈ
    /// </summary>
    IEnumerator TimeCount()
    {
        for (int i = 0; i <= second; i++)
        {
            yield return waitForSecond1;

            Count++;
        }
        Count = 0;
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

            if (isAuto)         // ���� ȹ�� ��� �� �˹ٸ� ����� ��
            {
                gameManager.MyGold += GoldManager.UnitToBigInteger(incrementGold);
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
