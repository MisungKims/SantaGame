/**
 * @brief 토끼 주민
 * @details 토끼 주민 (랜덤한 시간마다 당근 Get, AI)
 * @author 김미성
 * @date 22-04-26
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// 토끼 주민이 해야하는 행동
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
    #region 변수
    [SerializeField]
    private string carrot = "100.0A";   // 얻을 당근

    private float waitSecond;           // 당근을 몇 초마다 얻을건지

    

    private goal goal;      // 사용 중인 건물의 위치
    private int usingIndex; // 사용 중인 건물 위치의 인덱스

   
    private ECitizenBehavior citizenBehavior;

    private Animator anim;
    [SerializeField]
    private NavMeshAgent nav;


    private CitizenButtonRay getCarrotButton;      // 당근 획득 UI
    public bool isTouch = false;       // 당근 획득 UI를 클릭했는지

    // 머터리얼
    [SerializeField]
    private SkinnedMeshRenderer rabbitMat;
    [SerializeField]
    private Material[] materials;

    // 캐싱
    private WaitForSeconds waitForSecond;
    private GameManager gameManager;
    #endregion


    #region 유니티 함수
    void Awake()
    {
        gameManager = GameManager.Instance;
        anim = GetComponent<Animator>();

        GetButton();

        int rand = Random.Range(0, 12);
        rabbitMat.material = materials[rand];       // 토끼의 Material을 랜덤으로 설정

        waitSecond = Random.Range(20.0f, 70.0f);            // 당근을 몇 초마다 얻을건지 랜덤한 시간을 정함
        waitForSecond = new WaitForSeconds(waitSecond);
    }

    private void Start()
    {
        StartCoroutine(GetCarrotTimer());                   // 당근 획득 타이머 실행
        StartCoroutine(Action());       /// TODO : 초대 시 문제발생
    }
    #endregion

    #region 코루틴
    /// <summary>
    /// 랜덤한 시간이 지난 후 당근 획득 UI 생성
    /// </summary>
    IEnumerator GetCarrotTimer()
    {
        while (true)
        {
            yield return waitForSecond;

            getCarrotButton.gameObject.SetActive(true);     // 당근 획득 버튼을 보여줌

            yield return IsGetCarrot();         // 버튼 터치를 기다림
        }
    }

    /// <summary>
    /// 10초 동안 수동으로 획득하지 않으면 자동으로 획득
    /// </summary>
    IEnumerator IsGetCarrot()
    {
        for (int i = 0; i < 10; i++)
        {
            if (isTouch) break;                     // UI 터치 시 바로 당근 획득

            yield return new WaitForSeconds(1f);
        }

        gameManager.MyCarrots += GoldManager.UnitToBigInteger(carrot);
        getCarrotButton.gameObject.SetActive(false);
        isTouch = false;

        yield return null;
    }

    /// <summary>
    /// 위치를 정해서 이동한 후 애니메이션을 실행
    /// </summary>
    /// <returns></returns>
    IEnumerator Action()
    {
        while (true)
        {
            int randBehavior = Random.Range(0, 4);    // 랜덤 행동을 정함
            citizenBehavior = (ECitizenBehavior)randBehavior;

            Vector3 goalPoint;
            bool isFindGoal= false;

            if (randBehavior < 4)             // idle, dance 일 때는 랜덤 위치로
            {
                if (RandomPoint(this.transform.position, 50, out goalPoint))        // 랜덤 목적지를 찾았을 때
                {
                    isFindGoal = true;
                }
            }
            else        // goBuilding일 때는 랜덤 건물을 정하여 그 건물의 위치로
            {
                if (BuildingPoint(out goalPoint))               // 갈 수 있는 건물이 있을 때
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
    /// 각 행동에 맞는 애니메이션을 실행
    /// </summary>
    /// <returns></returns>
    IEnumerator Behavior()
    {
        int randTime = Random.Range(3, 10);         // 몇 초 동안 행동할 건지 랜덤으로 결정

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

    /// TODO : 건물 배치 후 각 건물 위치 리스트에 넣기
    #region 함수

    /// <summary>
    /// 당근 획득 버튼 클릭 시(인스펙터에서 호출)
    /// </summary>
    public void TouchButton()
    {
        Debug.Log("TOUCH");
        isTouch = true;
    }

    /// <summary>
    /// 오브젝트 풀에서 당근 획득 UI를 가져옴
    /// </summary>
    void GetButton()
    {
        getCarrotButton = ObjectPool.Instance.Get(EObjectFlag.getCarrotButton).GetComponent<CitizenButtonRay>();
        getCarrotButton.citizen = this;
        getCarrotButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// 랜덤 목적지 생성
    /// </summary>
    /// <param name="center">목적지</param>
    /// <param name="range">목적지 범위</param>
    /// <param name="result">랜덤 목적지</param>
    /// <returns>목적지를 찾았을 때 true 반환</returns>
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
    /// 건물을 목적지로
    /// </summary>
    /// <param name="result">반활할 목적지의 위치</param>
    /// <returns></returns>
    bool BuildingPoint(out Vector3 result)
    {
        // 잠금해제 된 건물 중 갈 곳을 설정
        int randBuilding = Random.Range(0, ObjectManager.Instance.unlockCount);
        goal = CitizenRabbitManager.Instance.goalPositions[randBuilding];

        // 설정한 건물에서 산타 주민이 사용하지 않는 위치를 찾아 그 곳으로 이동
        for (int i = 0; i < goal.goalObjects.Length; i++)
        {
            if (!goal.goalObjects[i].isUse)    // 가려고 하는 위치가 사용 중이 아닐 때
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
    /// 카메라가 해당 산타를 따라다님
    /// </summary>
    public void SetCamTargetThis()
    {
        UIManager.Instance.citizenPanel.SetActive(true);
        CameraMovement.Instance.ChaseSanta(this.transform);
    }

    /// <summary>
    /// 산타 터치 시 카메라의 타깃을 산타로 설정
    /// </summary>
    void TouchSanta()
    {
        if (Input.GetMouseButtonDown(0) && !UIManager.Instance.isOpenPanel)
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
    #endregion

    private void Update()
    {
        if (getCarrotButton.gameObject.activeSelf)
        {
            Vector3 newPos = transform.position;
            newPos.y += 5f;
            //Vector3 ButtonPos = Camera.main.WorldToScreenPoint(newPos);
            getCarrotButton.transform.position = newPos;
        }

        TouchSanta();
    }
}
