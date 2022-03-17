using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    #region ����

    [SerializeField]
    private int level = 1;
    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    private float multiplyBuildingPrice;    // ���׷��̵� �� �ǹ� ���� ���� ����
    public float MultiplyBuildingPrice
    {
        get { return multiplyBuildingPrice; }
        set { multiplyBuildingPrice = value; }
    }

    private int buildingPrice;              // �ǹ� ���� 
    public int BuildingPrice
    {
        get { return buildingPrice; }
        set { buildingPrice = value; }
    }

    private float multiplyGold;             // ���׷��̵� �� �÷��̾� �� ���� ����
    public float MultiplyGold
    {
        get { return multiplyGold; }
        set { multiplyGold = value; }
    }

    private int incrementGold;              // �÷��̾��� �� ������
    public int IncrementGold
    {
        get { return incrementGold; }
        set { incrementGold = value; }
    }

    private Vector3 distance;

    #endregion

    #region �Լ�

    /// <summary>
    /// �ǹ� �ʱ�ȭ
    /// </summary>
    /// <param name="buildingName">��Ÿ�� �̸�</param>
    public void InitBuilding(float multiplyPrice, int price, float mGold, int iGold)
    {
        SetCamTargetThis();                 // ī�޶� ��Ÿ�� ����ٴϵ���

        gameObject.SetActive(true);         // ��Ÿ�� ���̵���

        multiplyBuildingPrice = multiplyPrice;
        buildingPrice = price;
        multiplyGold = mGold;
        incrementGold = iGold;
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
                    Debug.Log("����");
                    SetCamTargetThis();
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
    }
    #endregion

}
