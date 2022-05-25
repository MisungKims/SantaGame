/**
 * @brief �ǹ�
 * @author ��̼�
 * @date 22-04-27
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class Building : MonoBehaviour
{
    #region ����
    [HideInInspector]
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
        set 
        { 
            buildingObj.incrementGold = value;
            getGoldAmountText.text = $"+ {IncrementGold}";
        }
    }

   
    public float Second
    {
        get { return buildingObj.second; }
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
    private Text getGoldAmountText;       // ��� ȹ�� �����̴��� �ؽ�Ʈ

    [SerializeField]
    private GameObject getGoldBtn;      // ��� ���� ȹ�� ��ư

    [SerializeField]
    private Transform cameraPos;      // ī�޶��� ��ġ


    // �� �� ����

    public bool isAuto = false;     // �˹ٸ� ����ߴ���? (�˹ٸ� ����ߴٸ� ��� ȹ�� �ڵ�ȭ)

    private bool isClickGetBtn = false;   // ȹ�� UI�� Ŭ�������� true, �ƴϸ� false (��� ȹ�� ������ ��)

    private Vector3 distance;       // ī�޶���� �Ÿ�

    private int questID = 2;

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

        Level = 1;
        IncrementGold = IncrementGold;

        getGoldSlider.maxValue = (int)Second;
        StartCoroutine(Increment());        // ���ȹ�� ����

        ObjectManager.Instance.unlockCount++;

        getGoldSlider.gameObject.SetActive(true);
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

        DailyQuestManager.Instance.Success(questID);        // ����Ʈ �Ϸ�

        gameManager.MyGold -= GoldManager.UnitToBigInteger(BuildingPrice);  // ���׷��̵� ��� ����

        BuildingPrice = GoldManager.MultiplyUnit(BuildingPrice, MultiplyBuildingPrice); // ����� ������ŭ ����

        IncrementGold = GoldManager.MultiplyUnit(IncrementGold, 1.1f * gameManager.goldEfficiency);  // ��� �������� ������ŭ ����

        Level++;

        return true;
    }


    /// <summary>
    /// ī�޶� �ش� �ǹ��� �ٶ�
    /// </summary>
    public void SetCamTargetThis()
    {
        if (uiManager.storePanel.activeSelf) uiManager.storePanel.SetActive(false);

        cameraMovement.ChaseBuilding(transform, cameraPos);
    }

   
    ///// <summary>
    ///// ������Ʈ ����â ������
    ///// </summary>
    //public void ShowObjWindow()
    //{
    //    window.clickedObj = buildingObj;
    //    window.Builidng = this;

    //    uiManager.ShowClickObjWindow();
    //}

 
    ///// <summary>
    ///// �ǹ��� ��ġ�� ��
    ///// </summary>
    //void TouchBuilding()
    //{
    //    if (Input.GetMouseButtonDown(0) && !uiManager.isOpenPanel)
    //    {
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit = new RaycastHit();

    //        if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))
    //        {
    //            if (hit.collider.CompareTag("Building") && hit.collider.name == this.name)
    //            {
    //                SetCamTargetThis();
    //                ShowObjWindow();
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// ���� ȹ�� ��ư Ŭ��
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

            if(!isAuto)            // ���� �����̸� UI Ŭ���� ��ٸ�
            {
                getGoldSlider.gameObject.SetActive(false);

                yield return StartCoroutine(WaitForManualAcquisition());

                getGoldSlider.gameObject.SetActive(true);
                getGoldBtn.SetActive(false);
            }

            gameManager.MyGold += GoldManager.UnitToBigInteger(IncrementGold);      // ��� ȹ��
        }
    }

    /// <summary>
    /// ������ �ð���ŭ ī��Ʈ
    /// </summary>
    IEnumerator TimeCount()
    {
        Count = 0;

        for (int i = 0; i <= Second; i++)
        {
            yield return waitForSecond1;

            Count++;
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
                break;
            }
        }

        yield return new WaitForSeconds(0.13f);

        isClickGetBtn = false;
    }
    #endregion

    #region ����Ƽ �޼ҵ�

    private void Awake()
    {
        gameManager = GameManager.Instance;
        cameraMovement = CameraMovement.Instance;
        uiManager = UIManager.Instance;
        window = uiManager.clickObjWindow.transform.GetComponent<ClickObjWindow>();

    }
   
    //void Update()
    //{
    //    TouchBuilding();
    //}
    #endregion
}
