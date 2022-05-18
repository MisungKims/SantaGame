/**
 * @brief ���� ���� ���ӿ��� ��Ÿ�� ����߸� ����
 * @author ��̼�
 * @date 22-05-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropGift : MonoBehaviour
{
    #region ����
    float moveSpeed = 50f;

    public Gift gift;
    public GiftCollider giftCollider;

    // ĳ��
    private DeliveryGameManager deliveryGameManager;

    private Inventory inventory;

    private ObjectPoolingManager objectPoolingManager;

    private SoundManager soundManager;

    private WaitForSeconds oneSec = new WaitForSeconds(1f);

    private WaitForSeconds twoSec = new WaitForSeconds(2.5f);
    #endregion

    #region ����Ƽ �Լ�
    private IEnumerator Start()
    {
        yield return null;

        deliveryGameManager = DeliveryGameManager.Instance;
        inventory = Inventory.Instance;
        objectPoolingManager = ObjectPoolingManager.Instance;
        soundManager = SoundManager.Instance;
    }

    private void OnEnable()
    {
        giftCollider.gameObject.SetActive(false);

        StartCoroutine(Dissapear());
    }

    void Update()
    {
        this.transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);        // �Ʒ��� ������
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���ҿ� �ε����� ���� ���� �Ϸ�
        if (other.gameObject.CompareTag("Chimney"))    
        {
            soundManager.PlaySoundEffect(ESoundEffectType.deliveryGetGift);     // ȿ���� ����

            Chimney chimney = other.GetComponent<Chimney>();
            gift = chimney.gift;

            inventory.RemoveItem2(gift);       // �κ��丮���� ����
            if (gift.wishCount > 0)                     // ���ø���Ʈ�� �־��� �͵��� ����ī��Ʈ ����
            {
                gift.wishCount--;
                /// TODO : ���ø���Ʈ�� �ִ��͵��� �߰������� ����
            }

            deliveryGameManager.SatisfiedCount++;

            deliveryGameManager.GiftCount = inventory.count;
            objectPoolingManager.Set(this.gameObject, EDeliveryFlag.gift);

            
            if (deliveryGameManager.GiftCount == 0)
            {
                deliveryGameManager.End(false);
            }
        }
    }
    #endregion

    #region �ڷ�ƾ
    /// <summary>
    /// �������� ���� ������ �� ���� �� �����
    /// </summary>
    IEnumerator Dissapear()
    {
        yield return twoSec;

        giftCollider.gameObject.SetActive(true);
        yield return oneSec;

        if (!giftCollider.isRemove)                 // �κ��丮���� �������� ������ �� �����ٸ�,
        {
            gift = inventory.RandomGet();
            
            if (gift != null)
            {
                Debug.Log(gift.giftName);
                inventory.RemoveItem2(gift);     // �κ��丮�� ���� �������� ����
            }
            else
            {
                Debug.Log("null");
            }
        }

        //yield return oneSec;

        objectPoolingManager.Set(this.gameObject, EDeliveryFlag.gift);

        // ����߸� ������ ���ٸ� ���� ����
        if (deliveryGameManager.GiftCount == 0)
        {
            deliveryGameManager.End(true);
        }
    }
    #endregion
}
