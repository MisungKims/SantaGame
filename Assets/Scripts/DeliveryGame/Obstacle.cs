using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    float moveSpeed = 50f;
    public EDeliveryFlag flag;

    Vector3 startPos = new Vector3(0, 0, 0);

    protected virtual void OnEnable()
    {
        this.transform.localPosition = startPos;
        StartCoroutine(Dissapear());
    }

    void Update()
    {
        if (!DeliveryGameManager.Instance.isEnd)
        {
            this.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        else ObjectPoolingManager.Instance.Set(this.gameObject, flag);
    }

    IEnumerator Dissapear()
    {
        yield return new WaitForSeconds(5f);

        ObjectPoolingManager.Instance.Set(this.gameObject, flag);
    }
}
