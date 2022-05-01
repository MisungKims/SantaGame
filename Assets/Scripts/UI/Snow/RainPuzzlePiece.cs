/**
 * @brief ���� �Բ� ������ ����
 * @author ��̼�
 * @date 22-05-01
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainPuzzlePiece : MonoBehaviour
{
    #region ����
    [SerializeField]
    private RectTransform rect;
    private float moveSpeed;

    public bool isStop;

    // ĳ��
    private WaitForSeconds wait;
    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        rect = this.GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        isStop = false;

        moveSpeed = Random.Range(100.0f, 200.0f);

        float randX = Random.Range(-490.0f, 490.0f);
        rect.anchoredPosition = new Vector2(randX, 580);

        float randTime = Random.Range(1.0f, 5.0f);      // ������ ���� ��������
        wait = new WaitForSeconds(randTime);

        StartCoroutine(Falling());      // randTime ���� ������ ������
    }
    #endregion

    #region �ڷ�ƾ
    /// <summary>
    /// �Ʒ��� ������
    /// </summary>
    IEnumerator Falling()
    {
        yield return wait;

        while (rect.anchoredPosition.y > -620.0f)
        {
            this.transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

            if (isStop) break;          // �������� ���� ������ Ŭ���ߴٸ� ����

            yield return null;
        }

        if (!isStop)
        {
            isStop = true;
            this.gameObject.SetActive(false);
        }
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// �������� ������ Ŭ������ �� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void ClickPuzzle()
    {
        isStop = true;
        this.gameObject.SetActive(false);

        PuzzleManager.Instance.GetRandomPuzzle();       // �������� ���� ȹ��
    }
    #endregion
}
