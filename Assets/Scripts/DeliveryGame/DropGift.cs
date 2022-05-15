using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropGift : MonoBehaviour
{
    float moveSpeed = 50f;
    public Gift gift;

    private void OnEnable()
    {
        Debug.Log(gift.giftName);
        StartCoroutine(Dissapear());
    }

    void Update()
    {
        this.transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    }

    IEnumerator Dissapear()
    {
        yield return new WaitForSeconds(3f);

        ObjectPoolingManager.Instance.Set(this.gameObject, EDeliveryFlag.gift);
    }
}
