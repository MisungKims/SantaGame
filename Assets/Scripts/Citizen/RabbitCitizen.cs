/**
 * @brief �䳢 �ֹ�
 * @details �䳢 �ֹ� (������ �ð����� ��� Get, AI)
 * @author ��̼�
 * @date 22-04-27
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
    dance,
    goBuilding
}

public class RabbitCitizen : MonoBehaviour
{
    #region ����
    [SerializeField]
    private string carrot = "100.0A";   // ���� ���

    private float waitSecond;           // ����� �� �ʸ��� ��������

    private goal goal;      // ��� ���� �ǹ��� ��ġ
    private int usingIndex; // ��� ���� �ǹ� ��ġ�� �ε���

    private ECitizenBehavior citizenBehavior;

    private Animator anim;
    [SerializeField]
    private NavMeshAgent nav;


    private CitizenButtonRay getCarrotButton;      // ��� ȹ�� UI
    public bool isTouch = false;       // ��� ȹ�� UI�� Ŭ���ߴ���
    bool isAutoGet = false;         // ����� �ڵ����� ������

    // ���͸���
    [SerializeField]
    private SkinnedMeshRenderer rabbitMat;
    [SerializeField]
    private Material[] materials;

    // ĳ��
    private WaitForSeconds waitForSecond;
    private GameManager gameManager;
    #endregion


    #region ����Ƽ �Լ�
    void Awake()
    {
        gameManager = GameManager.Instance;
        anim = GetComponent<Animator>();

        int rand = Random.Range(0, 12);
        rabbitMat.material = materials[rand];       // �䳢�� Material�� �������� ����

        waitSecond = Random.Range(20.0f, 70.0f);            // ����� �� �ʸ��� �������� ������ �ð��� ����
        waitForSecond = new WaitForSeconds(waitSecond);
    }

    private void Start()
    {
        StartCoroutine(GetCarrotTimer());                   // ��� ȹ�� Ÿ�̸� ����
        StartCoroutine(Action());
    }
    #endregion

    #region �ڷ�ƾ
    /// <summary>
    /// ������ �ð��� ���� �� ��� ȹ�� UI ����
    /// </summary>
    IEnumerator GetCarrotTimer()
    {
        while (true)
        {
            yield return waitForSecond;

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

        gameManager.MyCarrots += GoldManager.UnitToBigInteger(carrot);

        yield return new WaitForSeconds(0.13f);
        ObjectPoolingManager.Instance.Set(getCarrotButton.gameObject, EObjectFlag.getCarrotButton);
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

    /// <summary>
    /// ��ġ�� ���ؼ� �̵��� �� �ִϸ��̼��� ����
    /// </summary>
    /// <returns></returns>
    IEnumerator Action()
    {
        while (true)
        {
            int randBehavior = Random.Range(0, 4);    // ���� �ൿ�� ����
            citizenBehavior = (ECitizenBehavior)randBehavior;

            Vector3 goalPoint;
            bool isFindGoal= false;
            
            if (randBehavior < 4)             // idle, dance �� ���� ���� ��ġ��
            {
                if (RandomPoint(this.transform.position, 50, out goalPoint))        // ���� �������� ã���� ��
                {
                    isFindGoal = true;
                }
            }
            else        // goBuilding�� ���� ���� �ǹ��� ���Ͽ� �� �ǹ��� ��ġ��
            {
                if (BuildingPoint(out goalPoint))               // �� �� �ִ� �ǹ��� ���� ��
                {
                    isFindGoal = true;
                    goal.goalObjects[usingIndex].isUse = true;
                }
            }

            if (isFindGoal)
            {
                anim.SetBool("isWalking", true);

                nav.SetDestination(goalPoint);
                
                float distance = Vector3.Distance(this.transform.position, goalPoint);

                while (distance > 0.7f)
                {
                    distance = Vector3.Distance(this.transform.position, goalPoint);

                    yield return null;
                }

                nav.ResetPath();
                anim.SetBool("isWalking", false);

                yield return StartCoroutine(Behavior());
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
        int randTime = Random.Range(3, 10);         // �� �� ���� �ൿ�� ���� �������� ����

        anim.SetInteger("Action", (int)citizenBehavior);

        yield return new WaitForSeconds(randTime);

        anim.SetInteger("Action", -1);

        if (citizenBehavior == ECitizenBehavior.goBuilding)
        {
            goal.goalObjects[usingIndex].isUse = false;
            goal = null;
        }

        Vector3 temp = this.transform.eulerAngles;
        temp.x = 0;
        temp.z = 0;
        this.transform.eulerAngles = temp;
    }
    #endregion

    /// TODO : �ǹ� ��ġ �� �� �ǹ� ��ġ ����Ʈ�� �ֱ�
    #region �Լ�


    /// <summary>
    /// ������Ʈ Ǯ���� ��� ȹ�� UI�� ������
    /// </summary>
    void GetButton()
    {
        getCarrotButton = ObjectPoolingManager.Instance.Get(EObjectFlag.getCarrotButton).GetComponent<CitizenButtonRay>();
        getCarrotButton.citizen = this;
        //getCarrotButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// ���� ������ ����
    /// </summary>
    /// <param name="center">������</param>
    /// <param name="range">������ ����</param>
    /// <param name="result">���� ������</param>
    /// <returns>�������� ã���� �� true ��ȯ</returns>
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
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
        // ������� �� �ǹ� �� �� ���� ����
        int randBuilding = Random.Range(0, ObjectManager.Instance.unlockCount);
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
    /// ī�޶� �ش� ��Ÿ�� ����ٴ�
    /// </summary>
    public void SetCamTargetThis()
    {
        UIManager.Instance.ShowCitizenPanel();
        CameraMovement.Instance.ChaseSanta(this.transform);
    }

    /// <summary>
    /// ��Ÿ ��ġ �� ī�޶��� Ÿ���� ��Ÿ�� ����
    /// </summary>
    void TouchSanta()
    {
        if (Input.GetMouseButtonDown(0) && !UIManager.Instance.isOpenPanel)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))
            {
                if (hit.collider.name == this.name)
                {
                    SetCamTargetThis();
                }
            }
        }
    }
    #endregion

    private void Update()
    {
        if (getCarrotButton && getCarrotButton.gameObject.activeSelf)
        {
            Vector3 newPos = transform.position;
            newPos.y += 5f;
            //Vector3 ButtonPos = Camera.main.WorldToScreenPoint(newPos);
            getCarrotButton.transform.position = newPos;
        }

        TouchSanta();
    }
}
