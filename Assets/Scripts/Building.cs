using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Building : MonoBehaviour
{
    #region ����
    public GameObject clickObjWindow;

    [SerializeField]
    private int level = 1;
    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    [SerializeField]
    private float multiplyBuildingPrice;    // ���׷��̵� �� �ǹ� ���� ���� ����
    //public float MultiplyBuildingPrice
    //{
    //    //get { return multiplyBuildingPrice; }
    //    set { multiplyBuildingPrice = value; }
    //}

    [SerializeField]
    private int buildingPrice;              // �ǹ� ���� 
    public int BuildingPrice
    {
        get { return buildingPrice; }
        //set { buildingPrice = value; }
    }

    [SerializeField]
    private float multiplyGold;             // ���׷��̵� �� �÷��̾� �� ���� ����
    //public float MultiplyGold
    //{
    //    //get { return multiplyGold; }
    //    set { multiplyGold = value; }
    //}

    [SerializeField]
    private int incrementGold;              // �÷��̾��� �� ������
    public int IncrementGold
    {
        get { return incrementGold; }
        //set { incrementGold = value; }
    }


    private int index;
    public int Index
    {
        get { return index; }
        //set { incrementGold = value; }
    }

    private string buildingName;
    public string BuilidingName
    {
        get { return buildingName; }
    }

    [SerializeField]
    private Vector3 distance;

    private ClickObjWindow window;

    StringBuilder sb = new StringBuilder();

    // ĳ��
    private StoreObjectSc objectList;
    private GameManager gameManager;
    private CameraMovement cameraMovement;

    #endregion

    #region �Լ�

    /// <summary>
    /// �ǹ� �ʱ� ����
    /// </summary>
    /// <param name="index">�ǹ��� �ε���</param>
    /// <param name="name">�ǹ��� �̸�</param>
    /// <param name="multiplyPrice">�ǹ��� ���� ���� ���</param>
    /// <param name="price">�ǹ��� ����</param>
    /// <param name="mGold">��� ������ ���</param>
    /// <param name="iGold">��� ������</param>
    public void InitBuilding(int index, string name, float multiplyPrice, int price, float mGold, int iGold)
    {
        gameObject.SetActive(true);         // �ǹ��� ���̵���

        this.index = index;
        buildingName = name;
        multiplyBuildingPrice = multiplyPrice;
        buildingPrice = price;
        multiplyGold = mGold;
        incrementGold = iGold;

        SetCamTargetThis();                 // ī�޶� ��Ÿ�� ����ٴϵ���

        ShowObjWindow();                    // ������Ʈ ���� â�� ���̵���
    }

    // �ǹ� ���׷��̵�
    public void Upgrade()
    {
        if (gameManager.MyGold < buildingPrice)
        {
            return;
        }
        gameManager.MyGold -= buildingPrice;

        buildingPrice = (int)(buildingPrice * multiplyBuildingPrice);    // ����� ������ŭ ����
        //objectList.buildingPrice = buildingPrice;

        incrementGold = (int)(incrementGold * multiplyGold);            // ���� �������� ������ŭ ����
        //objectList.incrementGold = incrementGold;

        level++;
    }

    // ī�޶� �ش� ��Ÿ�� ����ٴ�
    public void SetCamTargetThis()
    {
        cameraMovement.chasingBuilding = this.transform;
        cameraMovement.buildingDistance = this.transform.GetChild(0).localPosition;
        cameraMovement.StartChaseTarget();
    }

    // ������Ʈ ����â ������
    public void ShowObjWindow()
    {
        window = gameManager.clickObjWindow.transform.GetComponent<ClickObjWindow>();

        sb.Clear();
        sb.Append("+ ");
        sb.Append(incrementGold.ToString());

        window.Builidng = this;
        window.SetBuildingInfo();

        gameManager.ShowClickObjWindow();
    }

    // �ǹ� ��ġ �� ī�޶��� Target�� �ش� �ǹ��� set
    void TouchBuilding()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))
            {
                if (hit.collider.CompareTag("Building"))
                {
                    SetCamTargetThis();
                    ShowObjWindow();
                }
            }
        }
    }


    #endregion

    #region ����Ƽ �޼ҵ�

    private void Awake()
    {
        gameManager = GameManager.Instance;
        cameraMovement = CameraMovement.Instance;
    }
    private void Start()
    {
        objectList = StorePanel.Instance.ObjectList[index];
    }

    void Update()
    {
        TouchBuilding();
    }
    #endregion
}
