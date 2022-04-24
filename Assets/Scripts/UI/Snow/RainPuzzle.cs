/**
 * @brief ���� �Բ� ������ ����
 * @author ��̼�
 * @date 22-04-25
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainPuzzle : MonoBehaviour
{
    [SerializeField]
    private RectTransform rect;
    private float moveSpeed;
    private bool isCatch;

    // ĳ��
    private WaitForSeconds wait;

    // Start is called before the first frame update
    void OnEnable()
    {
        isCatch = false;
        moveSpeed = Random.Range(100.0f, 200.0f);

        float randX = Random.Range(-490.0f, 490.0f);
        rect.anchoredPosition = new Vector2(randX, 344);

        float randTime = Random.Range(1.0f, 5.0f);      // ������ ���� ��������
        wait = new WaitForSeconds(randTime);
        StartCoroutine(Falling());      // randTime ���� ������ ������
    }

    /// <summary>
    /// �Ʒ��� ������
    /// </summary>
    IEnumerator Falling()
    {
        yield return wait;

        while(rect.anchoredPosition.y > -400.0f)
        {
            this.transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

            if (isCatch) break;

            yield return null;
        }
    }

    /// <summary>
    /// �������� ������ Ŭ������ �� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void ClickPuzzle()
    {
        isCatch = true;

        PuzzleManager.Instance.GetRandomPuzzle();       // �������� ���� ȹ��
    }    
}
