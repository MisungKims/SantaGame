/**
 * @brief 토끼 주민
 * @details 토끼 주민 생성 시 초기화 / 랜덤한 시간마다 당근 Get
 * @author 김미성
 * @date 22-04-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitCitizen : MonoBehaviour
{
    private Animator anim;
    private WaitForSeconds waitForSecond;
    private GameManager gameManager;

    [SerializeField]
    private string carrot = "100.0A";

    private float waitSecond;

    private bool isTouch = false;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        gameManager = GameManager.Instance;
    }

    private void OnEnable()
    {
        int rand = Random.Range(0, 12);
        anim.SetInteger("SantaIndex", rand);                 // 애니메이션을 이용해 토끼의 Material 설정

        waitSecond = Random.Range(20.0f, 70.0f);            // 당근을 몇 초마다 얻을건지 랜덤한 시간을 정함
        waitForSecond = new WaitForSeconds(waitSecond);

        StartCoroutine(GetCarrotTimer());                   // 당근 획득 타이머 실행
    }


    /// <summary>
    /// 랜덤한 시간이 지난 후 당근 획득 UI 생성
    /// </summary>
    IEnumerator GetCarrotTimer()
    {
        while(true)
        {
            yield return waitForSecond;

            /// TODO: 당근 받기 UI 생성

            yield return IsGetCarrot();         // UI 터치를 기다림
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
        isTouch = false;

        yield return null;
    }
}
