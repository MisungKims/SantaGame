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
    public Gift gift;
    public GiftCollider giftCollider;

    public Transform targetPos;

    //private Vector3 startPos = new Vector3(0, 0, 0);

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
        gift = null;

        if(DeliverySanta.giftPos != null) this.transform.position = DeliverySanta.giftPos.transform.position;

        StartCoroutine(Dissapear());
    }

    void Update()
    {
        this.transform.Translate(targetPos.localPosition * Time.deltaTime);        // �Ʒ��� ������
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���ҿ� �ε����� ���� ���� �Ϸ�
        if (other.gameObject.CompareTag("Chimney"))    
        {
            Chimney chimney = other.GetComponent<Chimney>();

            if (chimney.isAlreadyGet)       //�̹� ������ ���� ���ҿ��� �ƹ��� ȿ�� ����
            {
                return;
            }

            soundManager.PlaySoundEffect(ESoundEffectType.deliveryGetGift);     // ȿ���� ����

            gift = chimney.gift;

            inventory.RemoveItem(gift, false);       // �κ��丮���� ����
            if (gift.wishCount > 0)                     // ���ø���Ʈ�� �־��� �͵��� ����ī��Ʈ ����
            {
                gift.wishCount--;
                deliveryGameManager.WishCount++;
            }

            deliveryGameManager.SatisfiedCount++;

            //deliveryGameManager.GiftCount = inventory.count;
            objectPoolingManager.Set(this.gameObject, EDeliveryFlag.gift);

            chimney.giftImage.transform.parent.gameObject.SetActive(false);

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
        yield return oneSec;

        giftCollider.gameObject.SetActive(true);
        //yield return oneSec;

        if (!giftCollider.isRemove)                 // GiftCollider�� ��ó ������ ã�� ���Ͽ� �κ��丮���� �������� ������ �� �����ٸ�, �κ��丮�� ���� �������� ����
        {
            gift = inventory.RandomGet();
            inventory.RemoveItem(gift, false);
        }

        yield return oneSec;

        objectPoolingManager.Set(this.gameObject, EDeliveryFlag.gift);

        // ����߸� ������ ���ٸ� ���� ����
        if (deliveryGameManager.GiftCount == 0)
        {
            deliveryGameManager.End(true);
        }
    }
    #endregion
}
