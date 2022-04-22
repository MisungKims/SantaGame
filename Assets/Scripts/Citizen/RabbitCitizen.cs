/**
 * @brief �䳢 �ֹ�
 * @details �䳢 �ֹ� (������ �ð����� ��� Get, AI)
 * @author ��̼�
 * @date 22-04-22
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

    private bool isTouch = false;       // ��� ȹ�� UI�� Ŭ���ߴ���

    private goal goal;      // ��� ���� �ǹ��� ��ġ
    private int usingIndex; // ��� ���� �ǹ� ��ġ�� �ε���

   
    private ECitizenBehavior citizenBehavior;

    private Animator anim;
    [SerializeField]
    private NavMeshAgent nav;

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

    }

    private void Start()
    {
        int rand = Random.Range(0, 12);
        rabbitMat.material = materials[rand];       // �䳢�� Material�� �������� ����

        waitSecond = Random.Range(20.0f, 70.0f);            // ����� �� �ʸ��� �������� ������ �ð��� ����
        waitForSecond = new WaitForSeconds(waitSecond);

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

            /// TODO: ��� �ޱ� UI ����

            yield return IsGetCarrot();         // UI ��ġ�� ��ٸ�
        }
    }

    /// <summary>
    /// 10�� ���� �������� ȹ������ ������ �ڵ����� ȹ��
    /// </summary>
    IEnumerator IsGetCarrot()
    {
        for (int i = 0; i < 10; i++)
        {
            if (isTouch) break;                     // UI ��ġ �� �ٷ� ��� ȹ��

            yield return new WaitForSeconds(1f);
        }

        gameManager.MyCarrots += GoldManager.UnitToBigInteger(carrot);
        isTouch = false;

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
            int randBehavior = 4;
            //int randBehavior = Random.Range(0, 4);    // ���� �ൿ�� ����
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
    #endregion
}

// �� ����
//float distance = Vector3.Distance(this.transform.position, targetPos);

//while (distance > 0.05f)
//{
//    distance = Vector3.Distance(this.transform.position, targetPos);
//    Debug.Log(distance);
//    this.transform.position = Vector3.Lerp(this.transform.position, targetPos, Time.deltaTime * moveSpeed);
//    yield return null;
//}


// ���̵�
//while (distance > 2f)
//{
//    distance = Vector3.Distance(this.transform.position, targetPos);

//    this.transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);

//    yield return null;
//}

//Vector3 targetPos = this.transform.position;
//targetPos.x += 10;

//this.transform.LookAt(targetPos);

//anim.SetBool("isWalking", true);

//nav.SetDestination(targetPos);

//float distance = Vector3.Distance(this.transform.position, targetPos);

//while (distance > 0.1f)
//{
//    distance = Vector3.Distance(this.transform.position, targetPos);
//    Debug.Log(distance);
//    yield return null;
//}

//anim.SetBool("isWalking", false);
