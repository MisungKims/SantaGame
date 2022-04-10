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
        // 토끼 Material 설정
        int rand = Random.Range(0, 12);
        anim.SetInteger("SantaIndex", rand);

        waitSecond = Random.Range(20.0f, 70.0f);
        waitForSecond = new WaitForSeconds(waitSecond);

        StartCoroutine(GetCarrotTimer());
    }

    // 당근 획득 타이머
    IEnumerator GetCarrotTimer()
    {
        while(true)
        {
            yield return waitForSecond;

            gameManager.MyCarrots += GoldManager.UnitToBigInteger(carrot);

            yield return IsGetCarrot();
        }
    }

    // 당근 받기
    // 10초 동안 안받으면 자동으로 받아짐
    IEnumerator IsGetCarrot()
    {
        for (int i = 0; i < 10; i++)
        {
            if (isTouch)  
            {
                break;
            }
            yield return new WaitForSeconds(1f);
        }
       
        //TODO: 당근 받기

        yield return null;
    }
}
