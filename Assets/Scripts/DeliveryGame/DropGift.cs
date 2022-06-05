/**
 * @brief ���� ���� ���ӿ��� ��Ÿ�� ����߸� ����
 * @author ��̼�
 * @date 22-06-04
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


    // ĳ��
    private DeliveryGameManager deliveryGameManager;

    private Inventory inventory;

    private ObjectPoolingManager objectPoolingManager;

    private SoundManager soundManager;

    private WaitForSeconds oneSec = new WaitForSeconds(1f);

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
        this.transform.Translate(targetPos.localPosition * Time.deltaTime * 5f);        // �Ʒ��� ������
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���ҿ� �ε����� ���� ���� �Ϸ�
        if (other.gameObject.CompareTag("Chimney"))    
        {
            Delivery(other.GetComponent<Chimney>());
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

        // GiftCollider�� ��ó ������ ã�� ���Ͽ� �κ��丮���� �������� ������ �� �����ٸ�, �κ��丮�� ���� �������� ����
        if (!giftCollider.isRemove)                 
        {
            gift = inventory.RandomGet();
            inventory.RemoveItem(gift, false);
        }

        yield return oneSec;

        // ������Ʈ Ǯ�� ��ȯ
        objectPoolingManager.Set(this.gameObject, EObjectFlag.gift);

        // ����߸� ������ ���ٸ� ���� ����
        if (deliveryGameManager.GiftCount <= 0)
        {
            deliveryGameManager.End(true);
        }
    }
    #endregion

    /// <summary>
    /// ���� ����
    /// </summary>
    #region �Լ�
    void Delivery(Chimney chimney)
    {
        if (chimney.isAlreadyGet)       //�̹� ������ ���� ���ҿ��� �ƹ��� ȿ�� ����
        {
            return;
        }

        soundManager.PlaySoundEffect(ESoundEffectType.deliveryGetGift);     // ȿ���� ����

        gift = chimney.gift;

        inventory.RemoveItem(gift, false);          // �κ��丮���� �ش� ���� ����
        if (gift.giftInfo.wishCount > 0)            // ���ø���Ʈ�� �־��� �͵��� ����ī��Ʈ ����
        {
            gift.giftInfo.wishCount--;
            deliveryGameManager.wishCount++;
        }

        // ��޿� ���� ���� ���
        int score = 1;
        switch (gift.giftGrade)
        {
            case EGiftGrade.SS:
                score = 5;
                break;
            case EGiftGrade.S:
                score = 4;
                break;
            case EGiftGrade.A:
                score = 3;
                break;
            case EGiftGrade.B:
                score = 2;
                break;
            case EGiftGrade.C:
                score = 1;
                break;
        }

        deliveryGameManager.Score += score;         // ���� ȹ��

        objectPoolingManager.Set(this.gameObject, EObjectFlag.gift);        // ������Ʈ Ǯ�� ��ȯ

        chimney.giftImage.transform.parent.gameObject.SetActive(false);     // ��� ǳ�� �̹��� ��Ȱ��ȭ

        // ���̻� �� ������ ������ ���� ����
        if (deliveryGameManager.GiftCount <= 0)
        {
            deliveryGameManager.End(false);
        }
    }
    #endregion
}
