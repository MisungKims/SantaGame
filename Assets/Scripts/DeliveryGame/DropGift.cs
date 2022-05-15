using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropGift : MonoBehaviour
{
    float moveSpeed = 50f;

    private void Start()
    {
        StartCoroutine(Dissapear());
    }

    void Update()
    {
        this.transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    }

    IEnumerator Dissapear()
    {
        yield return new WaitForSeconds(5f);

        ObjectPoolingManager.Instance.Set(this.gameObject, EDeliveryFlag.gift1);
    }
}
