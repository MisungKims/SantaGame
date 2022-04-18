/**
 * @brief �䳢 �ֹ�
 * @details �䳢 �ֹ� ���� �� �ʱ�ȭ / ������ �ð����� ��� Get
 * @author ��̼�
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
        anim.SetInteger("SantaIndex", rand);                 // �ִϸ��̼��� �̿��� �䳢�� Material ����

        waitSecond = Random.Range(20.0f, 70.0f);            // ����� �� �ʸ��� �������� ������ �ð��� ����
        waitForSecond = new WaitForSeconds(waitSecond);

        StartCoroutine(GetCarrotTimer());                   // ��� ȹ�� Ÿ�̸� ����
    }


    /// <summary>
    /// ������ �ð��� ���� �� ��� ȹ�� UI ����
    /// </summary>
    IEnumerator GetCarrotTimer()
    {
        while(true)
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
}
