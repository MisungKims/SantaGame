/**
 * @details UI ������Ʈ�� ������ ũ�Ⱑ �۾����ٰ� Ŀ��
 * @author ��̼�
 * @date 22-04-27
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickScale : MonoBehaviour
{
    #region ����

    protected Vector3 startScale; // �۾��� ���� �ٽ� ������ ������

    public float speed = 0.9f;  // �۾����� �ӵ�

    public float size = 0.8f;   // �۾����� ����(ũ��)

    #endregion

    #region �ڷ�ƾ
    protected IEnumerator ScaleDown()
    {
        while (transform.localScale.x > startScale.x * size)
        {
            transform.localScale *= speed;     // ������ 0.9�� �����Ϸ� ����

            yield return null;
        }

        yield return new WaitForSeconds(0.05f);

        StartCoroutine(ScaleUp());
    }


    protected IEnumerator ScaleUp()
    {
        while (transform.localScale.x < startScale.x)
        {
            transform.localScale *= 2 - speed;     // �پ��� �ӵ��� �����ϰ� ����

            yield return null;
        }

        transform.localScale = startScale;

        yield return null;
    }

    #endregion


    #region ����Ƽ �޼ҵ�
    protected virtual IEnumerator Start()
    {
        yield return null;          // ��ũ�Ѻ信 ����Ʈ�� �ڵ����� ��ġ�� ����ִ� ���� ��ٸ�

        startScale = transform.localScale;
    }

    protected void OnEnable()
    {
        if (startScale != Vector3.zero)
        {
            transform.localScale = startScale;
        }
    }

    #endregion

}
