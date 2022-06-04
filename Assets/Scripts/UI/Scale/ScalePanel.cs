/**
 * @details �г��� �������� ����
 * @author ��̼�
 * @date 22-05-14
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePanel : MonoBehaviour
{
    #region ����
    private Vector3 zeroScale = new Vector3(0, 0, 1);
    private Vector3 oneScale = new Vector3(1, 1, 1);

    [SerializeField]
    private Vector3 anotherScale;

    public bool anotherVector = false;

    [SerializeField]
    private float speed = 15f;
    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        if (anotherVector)
        {
            oneScale = anotherScale;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(ScaleUp());
    }
    #endregion

    #region �ڷ�ƾ
    /// <summary>
    /// �������� 0���� 1�� Ŀ��
    /// </summary>
    /// <returns></returns>
    IEnumerator ScaleUp()
    {
        this.transform.localScale = zeroScale;

        while (Vector3.Distance(this.transform.localScale, oneScale) > 0.01f)
        {
            this.transform.localScale = Vector3.Lerp(this.transform.localScale, oneScale, Time.deltaTime * speed);

            yield return null;
        }

        this.transform.localScale = oneScale;
    }


    /// <summary>
    /// �������� 1���� 0���� �۾���
    /// </summary>
    /// <returns></returns>
    IEnumerator ScaleDown()
    {
        while (Vector3.Distance(this.transform.localScale, zeroScale) > 1.0f)
        {
            this.transform.localScale = Vector3.Lerp(this.transform.localScale, zeroScale, Time.deltaTime * speed);

            yield return null;
        }

        this.transform.localScale = zeroScale;
        this.gameObject.SetActive(false);
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// �г��� ���� �� �������� ���� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void ClosePanel()
    {
        StartCoroutine(ScaleDown());
    }
    #endregion
}
