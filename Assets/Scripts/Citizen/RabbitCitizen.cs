/**
 * @brief 토끼 주민
 * @details 토끼 주민 (랜덤한 시간마다 당근 Get, AI)
 * @author 김미성
 * @date 22-06-01
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
    goBuilding
}

public class RabbitCitizen : MonoBehaviour
{
    #region 변수
    private Animator anim;
    public SkinnedMeshRenderer rabbitMat;       // 머터리얼
    [SerializeField]
    private NavMeshAgent nav;


    // 당근 획득
    [SerializeField]
    private string carrot = "100.0A";   // 얻을 당근

    private float waitSecond;           // 당근을 몇 초마다 얻을건지
    private CitizenButtonRay getCarrotButton;      // 당근 획득 UI
    public bool isTouch = false;       // 당근 획득 UI를 클릭했는지
    bool isAutoGet = false;         // 당근을 자동으로 받을지


    // 이동 시 필요한 변수
    private goal goal;          // 사용 중인 건물의 위치
    private int usingIndex;     // 사용 중인 건물 위치의 인덱스

    private ECitizenBehavior citizenBehavior;

    bool isCanGo = true;        // 발 밑에 땅이 있어서 갈 수 있는지?
    Vector3 goalPoint;          // 목적지
    bool isFindGoal = false;    // 목적지를 찾았는지?

    int preGoal = -1;           // 이전 목적지의 인덱스

    Vector3 centerPos;          // 랜덤 목적지 범위의 center
    int range;                  // 랜덤 목적지 범위

    bool isGoing = false;       // 움직이고 있는지?
    int timerCnt = 0;

    [SerializeField]
    private LayerMask layerMask;

    
    // 옷
    public bool isWearing = false;
    public Clothes clothes = null;      // 주민의 옷(코디템)
    private GameObject clothesObj;
    public Transform clothesParent;    // 옷 오브젝트의 부모


    // 캐싱
    private WaitForSeconds randomWaitForSecond;
    private GameManager gameManager;
    private ObjectPoolingManager objectPoolingManager;
    private UIManager uIManager;
    private CameraMovement cameraMovement;
    #endregion


    #region 유니티 함수
    void Awake()
    {
        anim = GetComponent<Animator>();

        gameManager = GameManager.Instance;
        objectPoolingManager = ObjectPoolingManager.Instance;
        uIManager = UIManager.Instance;
        cameraMovement = CameraMovement.Instance;

        waitSecond = Random.Range(20.0f, 70.0f);            // 당근을 몇 초마다 얻을건지 랜덤한 시간을 정함
        randomWaitForSecond = new WaitForSeconds(waitSecond);
    }

    private void Start()
    {
        StartCoroutine(GetCarrotTimer());       // 당근 획득 타이머 실행

        // 토끼 이동 시작
        StartCoroutine(Action());              
        StartCoroutine(Timer());

        StartCoroutine(UpdateCoru());
    }
    #endregion

    #region 코루틴
    private IEnumerator UpdateCoru()
    {
        while (true)
        {
            SetCarrotButtonPos();

            TouchRabbit();

            yield return null;
        }
    }

    #region 당근 획득
    /// <summary>
    /// 랜덤한 시간이 지난 후 당근 획득 UI 생성
    /// </summary>
    IEnumerator GetCarrotTimer()
    {
        while (true)
        {
            yield return randomWaitForSecond;

            GetButton();

            yield return StartCoroutine(IsGetCarrot());         // 당근 획득을 기다림
        }
    }

    /// <summary>
    /// 버튼 터치 혹은 10초 카운트 뒤에 당근 획득
    /// </summary>
    IEnumerator IsGetCarrot()
    {
        StartCoroutine(TimerCount());      // 10초 카운트도 시작

        while (true)
        {
            if (isTouch)                     // UI 터치 시 바로 당근 획득
            {
                isTouch = false;
                break;
            }
            if (isAutoGet)                  // 10초 카운트 동안 터치하지 않았을 때
            {
                isAutoGet = false;
                break;
            }

            yield return null;
        }

        gameManager.MyCarrots += GoldManager.UnitToBigInteger(carrot);          // 당근 획득

        yield return new WaitForSeconds(0.13f);
        ObjectPoolingManager.Instance.Set(getCarrotButton.gameObject, EObjectFlag.getCarrotButton);         // UI를 오브젝트 풀에 반환
        getCarrotButton = null;
    }

    /// <summary>
    /// 10초 동안 수동으로 획득하지 않으면 자동으로 획득
    /// </summary>
    IEnumerator TimerCount()
    {
        for (int i = 0; i < 10; i++)
        {
            if (isTouch) break;                     // UI 터치 시 카운트 종료

            yield return new WaitForSeconds(1f);
        }

        if (!isTouch)
        {
            isAutoGet = true;
        }

        yield return null;
    }
    #endregion

    #region 움직임
    /// <summary>
    /// 위치를 정해서 이동한 후 애니메이션을 실행
    /// </summary>
    /// <returns></returns>
    IEnumerator Action()
    {
        while (true)
        {
            // 랜덤 행동을 정함
            int randBehavior = Random.Range(0, 4);
            //int randBehavior = 3;    
            citizenBehavior = (ECitizenBehavior)randBehavior;

            isFindGoal = false;

            if (isCanGo)
            {
                centerPos = this.transform.position;
                range = 50;

                // 해야할 행동이 idle 일 때는 랜덤 위치로
                if (randBehavior < 3)
                {
                    if (RandomPoint(centerPos, range, out goalPoint))        // 랜덤 목적지를 찾았을 때 이동
                    {
                        isFindGoal = true;
                    }
                }
                else        // goBuilding일 때는 랜덤 건물을 정하여 그 건물의 위치로
                {
                    if (BuildingPoint(out goalPoint))               // 갈 수 있는 건물이 있을 때 이동
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
                anim.SetBool("isWalking", true);        // 걷기 애니메이션 실행

                nav.SetDestination(goalPoint);          // AI의 목적지 설정

                yield return new WaitForSeconds(1f);

                float distance = Vector3.Distance(this.transform.position, goalPoint);
                while (distance > 1f && isCanGo && timerCnt < 25)
                {
                    distance = Vector3.Distance(this.transform.position, goalPoint);

                    //가는 도중 땅이 없어서 갈 수 없으면 멈춤
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
    /// 토끼가 움직일 동안 실행하는 타이머
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
    /// 각 행동에 맞는 애니메이션을 실행
    /// </summary>
    /// <returns></returns>
    IEnumerator Behavior()
    {
        if (citizenBehavior == ECitizenBehavior.goBuilding)
        {
            // 건물로 가다가 멈췄을 땐 Idle 실행
            if (!isCanGo)
            {
                citizenBehavior = ECitizenBehavior.idleA;
                EndUseBuilding();
            }
            // 정확하게 건물에 도착했을 때 
            else
            {
                if (goal != null)
                {
                    this.transform.LookAt(goal.goalObjects[usingIndex].lookAtPos);          // 바라봐야하는 곳을 바라보도록 함
                }
            }
        }

        int randTime = Random.Range(3, 10);         // 몇 초 동안 행동할 건지 랜덤으로 결정

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

    #region 함수
    /// <summary>
    /// 오브젝트 풀에서 당근 획득 UI를 가져옴
    /// </summary>
    void GetButton()
    {
        getCarrotButton = objectPoolingManager.Get(EObjectFlag.getCarrotButton).GetComponent<CitizenButtonRay>();
        getCarrotButton.citizen = this;
    }

    /// <summary>
    /// 카메라가 해당 산타를 따라다님
    /// </summary>
    public void SetCamTargetThis()
    {
        uIManager.ShowCitizenPanel(this);
        cameraMovement.ChaseSanta(this.transform);
    }

    /// <summary>
    /// 토끼 터치 시 카메라의 타깃을 토끼로 설정
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
    /// 당근 획득 버튼의 위치를 설정
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

    #region 움직임
    /// <summary>
    /// 건물의 해당 위치를 벗어날 때
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
    /// 랜덤 목적지 생성
    /// </summary>
    /// <param name="center">목적지</param>
    /// <param name="range">목적지 범위</param>
    /// <param name="result">반환할 랜덤 목적지</param>
    /// <returns>목적지를 찾았을 때 true 반환</returns>
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
    /// 건물을 목적지로
    /// </summary>
    /// <param name="result">반활할 목적지의 위치</param>
    /// <returns></returns>
    bool BuildingPoint(out Vector3 result)
    {
        // 건물 중 갈 곳을 설정
        int randBuilding = Random.Range(0, CitizenRabbitManager.Instance.goalPositions.Count);

        // 직전에 갔던 건물에는 가지 않도록
        if (randBuilding == preGoal)
        {
            preGoal = -1;
            result = Vector3.zero;
            return false;
        }

        preGoal = randBuilding;
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
    /// 레이캐스트로 토끼의 발 밑에 땅이 있는지 없는지 확인
    /// </summary>
    /// <returns>갈 수 있으면 true</returns>
    public bool CanGo()
    {
        Ray rayRoad = new Ray(transform.position + transform.TransformVector(0, 0.1f, 0.2f),
                        transform.TransformDirection(0, -1, 0));
        Debug.DrawLine(rayRoad.origin, rayRoad.origin + rayRoad.direction, Color.red);

        return Physics.Raycast(rayRoad, out _, 0.8f, layerMask);
    }
    #endregion

    #region 코디
    /// <summary>
    /// 옷을 입음
    /// </summary>
    /// <param name="clothes">입힐 옷</param>
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
    /// 옷을 벗음
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
