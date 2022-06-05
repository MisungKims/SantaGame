/**
 * @brief �䳢 �ֹ�
 * @details �䳢 �ֹ� (������ �ð����� ��� Get, AI)
 * @author ��̼�
 * @date 22-06-01
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// �䳢 �ֹ��� �ؾ��ϴ� �ൿ
/// </summary>
enum ECitizenBehavior
{
    idleA,
    idleB,
    idleC,
    goBuilding
}

public class RabbitCitizen : MonoBehaviour
{
    #region ����
    private Animator anim;
    public SkinnedMeshRenderer rabbitMat;       // ���͸���
    [SerializeField]
    private NavMeshAgent nav;


    // ��� ȹ��
    [SerializeField]
    private string carrot = "100.0A";   // ���� ���

    private float waitSecond;           // ����� �� �ʸ��� ��������
    private CitizenButtonRay getCarrotButton;      // ��� ȹ�� UI
    public bool isTouch = false;       // ��� ȹ�� UI�� Ŭ���ߴ���
    bool isAutoGet = false;         // ����� �ڵ����� ������


    // �̵� �� �ʿ��� ����
    private goal goal;          // ��� ���� �ǹ��� ��ġ
    private int usingIndex;     // ��� ���� �ǹ� ��ġ�� �ε���

    private ECitizenBehavior citizenBehavior;

    bool isCanGo = true;        // �� �ؿ� ���� �־ �� �� �ִ���?
    Vector3 goalPoint;          // ������
    bool isFindGoal = false;    // �������� ã�Ҵ���?

    int preGoal = -1;           // ���� �������� �ε���

    Vector3 centerPos;          // ���� ������ ������ center
    int range;                  // ���� ������ ����

    bool isGoing = false;       // �����̰� �ִ���?
    int timerCnt = 0;

    [SerializeField]
    private LayerMask layerMask;

    
    // ��
    public bool isWearing = false;
    public Clothes clothes = null;      // �ֹ��� ��(�ڵ���)
    private GameObject clothesObj;
    public Transform clothesParent;    // �� ������Ʈ�� �θ�


    // ĳ��
    private WaitForSeconds randomWaitForSecond;
    private GameManager gameManager;
    private ObjectPoolingManager objectPoolingManager;
    private UIManager uIManager;
    private CameraMovement cameraMovement;
    #endregion


    #region ����Ƽ �Լ�
    void Awake()
    {
        anim = GetComponent<Animator>();

        gameManager = GameManager.Instance;
        objectPoolingManager = ObjectPoolingManager.Instance;
        uIManager = UIManager.Instance;
        cameraMovement = CameraMovement.Instance;

        waitSecond = Random.Range(20.0f, 70.0f);            // ����� �� �ʸ��� �������� ������ �ð��� ����
        randomWaitForSecond = new WaitForSeconds(waitSecond);
    }

    private void Start()
    {
        StartCoroutine(GetCarrotTimer());       // ��� ȹ�� Ÿ�̸� ����

        // �䳢 �̵� ����
        StartCoroutine(Action());              
        StartCoroutine(Timer());

        StartCoroutine(UpdateCoru());
    }
    #endregion

    #region �ڷ�ƾ
    private IEnumerator UpdateCoru()
    {
        while (true)
        {
            SetCarrotButtonPos();

            TouchRabbit();

            yield return null;
        }
    }

    #region ��� ȹ��
    /// <summary>
    /// ������ �ð��� ���� �� ��� ȹ�� UI ����
    /// </summary>
    IEnumerator GetCarrotTimer()
    {
        while (true)
        {
            yield return randomWaitForSecond;

            GetButton();

            yield return StartCoroutine(IsGetCarrot());         // ��� ȹ���� ��ٸ�
        }
    }

    /// <summary>
    /// ��ư ��ġ Ȥ�� 10�� ī��Ʈ �ڿ� ��� ȹ��
    /// </summary>
    IEnumerator IsGetCarrot()
    {
        StartCoroutine(TimerCount());      // 10�� ī��Ʈ�� ����

        while (true)
        {
            if (isTouch)                     // UI ��ġ �� �ٷ� ��� ȹ��
            {
                isTouch = false;
                break;
            }
            if (isAutoGet)                  // 10�� ī��Ʈ ���� ��ġ���� �ʾ��� ��
            {
                isAutoGet = false;
                break;
            }

            yield return null;
        }

        gameManager.MyCarrots += GoldManager.UnitToBigInteger(carrot);          // ��� ȹ��

        yield return new WaitForSeconds(0.13f);
        ObjectPoolingManager.Instance.Set(getCarrotButton.gameObject, EObjectFlag.getCarrotButton);         // UI�� ������Ʈ Ǯ�� ��ȯ
        getCarrotButton = null;
    }

    /// <summary>
    /// 10�� ���� �������� ȹ������ ������ �ڵ����� ȹ��
    /// </summary>
    IEnumerator TimerCount()
    {
        for (int i = 0; i < 10; i++)
        {
            if (isTouch) break;                     // UI ��ġ �� ī��Ʈ ����

            yield return new WaitForSeconds(1f);
        }

        if (!isTouch)
        {
            isAutoGet = true;
        }

        yield return null;
    }
    #endregion

    #region ������
    /// <summary>
    /// ��ġ�� ���ؼ� �̵��� �� �ִϸ��̼��� ����
    /// </summary>
    /// <returns></returns>
    IEnumerator Action()
    {
        while (true)
        {
            // ���� �ൿ�� ����
            int randBehavior = Random.Range(0, 4);
            //int randBehavior = 3;    
            citizenBehavior = (ECitizenBehavior)randBehavior;

            isFindGoal = false;

            if (isCanGo)
            {
                centerPos = this.transform.position;
                range = 50;

                // �ؾ��� �ൿ�� idle �� ���� ���� ��ġ��
                if (randBehavior < 3)
                {
                    if (RandomPoint(centerPos, range, out goalPoint))        // ���� �������� ã���� �� �̵�
                    {
                        isFindGoal = true;
                    }
                }
                else        // goBuilding�� ���� ���� �ǹ��� ���Ͽ� �� �ǹ��� ��ġ��
                {
                    if (BuildingPoint(out goalPoint))               // �� �� �ִ� �ǹ��� ���� �� �̵�
                    {
                        isFindGoal = true;
                        goal.goalObjects[usingIndex].isUse = true;
                    }
                }
            }
            else
            {
                goalPoint = CitizenRabbitManager.Instance.zeroPos.position;
                isFindGoal = true;
            }

            if (isFindGoal)
            {
                isCanGo = true;
                isGoing = true;
                anim.SetBool("isWalking", true);        // �ȱ� �ִϸ��̼� ����

                nav.SetDestination(goalPoint);          // AI�� ������ ����

                yield return new WaitForSeconds(1f);

                float distance = Vector3.Distance(this.transform.position, goalPoint);
                while (distance > 1f && isCanGo && timerCnt < 25)
                {
                    distance = Vector3.Distance(this.transform.position, goalPoint);

                    //���� ���� ���� ��� �� �� ������ ����
                    if (!CanGo())
                    {
                        isCanGo = false;
                    }

                    yield return null;
                }

                isGoing = false;
                nav.ResetPath();
                anim.SetBool("isWalking", false);

                yield return StartCoroutine(Behavior());
            }

            yield return new WaitForSeconds(2f);
        }
    }

    /// <summary>
    /// �䳢�� ������ ���� �����ϴ� Ÿ�̸�
    /// </summary>
    /// <returns></returns>
    IEnumerator Timer()
    {
        while (true)
        {
            timerCnt = 0;

            while (isGoing)
            {
                yield return new WaitForSeconds(1f);
                timerCnt++;
            }

            yield return null;
        }
    }

    /// <summary>
    /// �� �ൿ�� �´� �ִϸ��̼��� ����
    /// </summary>
    /// <returns></returns>
    IEnumerator Behavior()
    {
        if (citizenBehavior == ECitizenBehavior.goBuilding)
        {
            // �ǹ��� ���ٰ� ������ �� Idle ����
            if (!isCanGo)
            {
                citizenBehavior = ECitizenBehavior.idleA;
                EndUseBuilding();
            }
            // ��Ȯ�ϰ� �ǹ��� �������� �� 
            else
            {
                if (goal != null)
                {
                    this.transform.LookAt(goal.goalObjects[usingIndex].lookAtPos);          // �ٶ�����ϴ� ���� �ٶ󺸵��� ��
                }
            }
        }

        int randTime = Random.Range(3, 10);         // �� �� ���� �ൿ�� ���� �������� ����

        anim.SetInteger("Action", (int)citizenBehavior);

        yield return new WaitForSeconds(randTime);

        anim.SetInteger("Action", -1);

        if (citizenBehavior == ECitizenBehavior.goBuilding)
        {
            EndUseBuilding();
        }

        Vector3 temp = this.transform.eulerAngles;
        temp.x = 0;
        temp.z = 0;
        this.transform.eulerAngles = temp;
    }
    #endregion
    #endregion

    #region �Լ�
    /// <summary>
    /// ������Ʈ Ǯ���� ��� ȹ�� UI�� ������
    /// </summary>
    void GetButton()
    {
        getCarrotButton = objectPoolingManager.Get(EObjectFlag.getCarrotButton).GetComponent<CitizenButtonRay>();
        getCarrotButton.citizen = this;
    }

    /// <summary>
    /// ī�޶� �ش� ��Ÿ�� ����ٴ�
    /// </summary>
    public void SetCamTargetThis()
    {
        uIManager.ShowCitizenPanel(this);
        cameraMovement.ChaseSanta(this.transform);
    }

    /// <summary>
    /// �䳢 ��ġ �� ī�޶��� Ÿ���� �䳢�� ����
    /// </summary>
    void TouchRabbit()
    {
        if (Input.GetMouseButtonDown(0) && !uIManager.isOpenPanel)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))
            {
                if (hit.collider.CompareTag("Santa") && hit.collider.name == this.name)
                {
                    SetCamTargetThis();
                }
            }
        }
    }

    /// <summary>
    /// ��� ȹ�� ��ư�� ��ġ�� ����
    /// </summary>
    void SetCarrotButtonPos()
    {
        if (getCarrotButton && getCarrotButton.gameObject.activeSelf)
        {
            Vector3 newPos = transform.position;
            newPos.y += 5f;

            getCarrotButton.transform.position = newPos;
        }
    }

    #region ������
    /// <summary>
    /// �ǹ��� �ش� ��ġ�� ��� ��
    /// </summary>
    void EndUseBuilding()
    {
        if (goal != null)
        {
            goal.goalObjects[usingIndex].isUse = false;
            goal = null;
        }
    }

    /// <summary>
    /// ���� ������ ����
    /// </summary>
    /// <param name="center">������</param>
    /// <param name="range">������ ����</param>
    /// <param name="result">��ȯ�� ���� ������</param>
    /// <returns>�������� ã���� �� true ��ȯ</returns>
    public bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;

        return false;
    }

    /// <summary>
    /// �ǹ��� ��������
    /// </summary>
    /// <param name="result">��Ȱ�� �������� ��ġ</param>
    /// <returns></returns>
    bool BuildingPoint(out Vector3 result)
    {
        // �ǹ� �� �� ���� ����
        int randBuilding = Random.Range(0, CitizenRabbitManager.Instance.goalPositions.Count);

        // ������ ���� �ǹ����� ���� �ʵ���
        if (randBuilding == preGoal)
        {
            preGoal = -1;
            result = Vector3.zero;
            return false;
        }

        preGoal = randBuilding;
        goal = CitizenRabbitManager.Instance.goalPositions[randBuilding];

        // ������ �ǹ����� ��Ÿ �ֹ��� ������� �ʴ� ��ġ�� ã�� �� ������ �̵�
        for (int i = 0; i < goal.goalObjects.Length; i++)
        {
            if (!goal.goalObjects[i].isUse)    // ������ �ϴ� ��ġ�� ��� ���� �ƴ� ��
            {
                usingIndex = i;
                result = goal.goalObjects[i].pos.position;
                return true;
            }
        }

        result = Vector3.zero;
        return false;
    }

    /// <summary>
    /// ����ĳ��Ʈ�� �䳢�� �� �ؿ� ���� �ִ��� ������ Ȯ��
    /// </summary>
    /// <returns>�� �� ������ true</returns>
    public bool CanGo()
    {
        Ray rayRoad = new Ray(transform.position + transform.TransformVector(0, 0.1f, 0.2f),
                        transform.TransformDirection(0, -1, 0));
        Debug.DrawLine(rayRoad.origin, rayRoad.origin + rayRoad.direction, Color.red);

        return Physics.Raycast(rayRoad, out _, 0.8f, layerMask);
    }
    #endregion

    #region �ڵ�
    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="clothes">���� ��</param>
    public bool PutOn(Clothes clothes)
    {
        if (clothes != null)
        {
            this.clothes = clothes;

            clothesObj = objectPoolingManager.Get(clothes.flag, clothesParent);
            clothesObj.transform.localPosition = clothes.pos;
            clothesObj.transform.localEulerAngles = clothes.rot;
            clothesObj.transform.localScale = clothes.scale;

            isWearing = true;

            return true;
        }

        return false;
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    public void PutOff()
    {
        isWearing = false;
        objectPoolingManager.Set(clothesObj, clothes.flag);
        clothes = null;
    }
    #endregion
    #endregion
}
