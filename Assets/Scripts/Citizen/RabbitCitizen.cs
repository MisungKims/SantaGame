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
        // �䳢 Material ����
        int rand = Random.Range(0, 12);
        anim.SetInteger("SantaIndex", rand);

        waitSecond = Random.Range(20.0f, 70.0f);
        waitForSecond = new WaitForSeconds(waitSecond);

        StartCoroutine(GetCarrotTimer());
    }

    // ��� ȹ�� Ÿ�̸�
    IEnumerator GetCarrotTimer()
    {
        while(true)
        {
            yield return waitForSecond;

            gameManager.MyCarrots += GoldManager.UnitToBigInteger(carrot);

            yield return IsGetCarrot();
        }
    }

    // ��� �ޱ�
    // 10�� ���� �ȹ����� �ڵ����� �޾���
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
       
        //TODO: ��� �ޱ�

        yield return null;
    }
}
