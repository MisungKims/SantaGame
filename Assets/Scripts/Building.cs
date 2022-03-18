using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    #region ����
    public GameObject clickObjWindow;

    [SerializeField]
    private int level = 1;
    public int Level
    {
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
    //public int BuildingPrice
    //{
    //    //get { return buildingPrice; }
    //    //set { buildingPrice = value; }
    //}

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

    private Vector3 distance;

    //[SerializeField]
    //private Santa santa;
    //public Santa Santa
    //{
    //    get { return santa; }
    //    set { santa = value; }
    //}

    private string buildingName;

    private int index;

    
    #endregion

    #region �Լ�

    /// <summary>
    /// �ǹ� �ʱ�ȭ
    /// </summary>
    /// <param name="buildingName">��Ÿ�� �̸�</param>
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

    public void Upgrade()
    {
        GameManager.Instance.MyGold -= buildingPrice;

        buildingPrice = (int)(buildingPrice * multiplyBuildingPrice);    // ����� ������ŭ ����
        StorePanel.Instance.ObjectList[index].buildingPrice = buildingPrice;

        incrementGold = (int)(incrementGold * multiplyGold);            // ���� �������� ������ŭ ����
        StorePanel.Instance.ObjectList[index].incrementGold = incrementGold;

        level++;
    }

    /// <summary>
    /// ī�޶� �ش� ��Ÿ�� ����ٴ�
    /// </summary>
    public void SetCamTargetThis()
    {
        CameraMovement.Instance.StartChaseTarget();
        CameraMovement.Instance.chasingBuilding = this.transform;
        CameraMovement.Instance.buildingDistance = distance;
    }

    /// <summary>
    /// ������Ʈ ����â ������
    /// </summary>
    public void ShowObjWindow()
    {
        ClickObjWindow window = GameManager.Instance.clickObjWindow.transform.GetComponent<ClickObjWindow>();
        window.Set(buildingName, level, buildingPrice, "+ " + incrementGold.ToString());
       
        GameManager.Instance.ShowClickObjWindow();
    }

    /// <summary>
    /// �ǹ� ��ġ �� ī�޶��� Target�� �ش� �ǹ��� set
    /// </summary>
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

    private void Start()
    {
        distance = this.transform.GetChild(0).localPosition;
    }

    void Update()
    {
        TouchBuilding();

        //if (this.gameObject.activeSelf)
        //{
        //    GameManager.Instance.MyGold += IncrementGold * Time.deltaTime;
        //}
    }
    #endregion

}
