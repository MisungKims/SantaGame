/**
 * @brief 선물 전달 게임에서 산타가 떨어뜨릴 선물
 * @author 김미성
 * @date 22-05-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropGift : MonoBehaviour
{
    #region 변수
    public Gift gift;
    public GiftCollider giftCollider;

    public Transform targetPos;

    //private Vector3 startPos = new Vector3(0, 0, 0);

    // 캐싱
    private DeliveryGameManager deliveryGameManager;

    private Inventory inventory;

    private ObjectPoolingManager objectPoolingManager;

    private SoundManager soundManager;

    private WaitForSeconds oneSec = new WaitForSeconds(1f);

    private WaitForSeconds twoSec = new WaitForSeconds(2.5f);
    #endregion

    #region 유니티 함수
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
        this.transform.Translate(targetPos.localPosition * Time.deltaTime);        // 아래로 떨어짐
    }

    private void OnTriggerEnter(Collider other)
    {
        // 굴뚝에 부딪히면 선물 전달 완료
        if (other.gameObject.CompareTag("Chimney"))    
        {
            Chimney chimney = other.GetComponent<Chimney>();

            if (chimney.isAlreadyGet)       //이미 선물을 받은 굴뚝에는 아무런 효과 없음
            {
                return;
            }

            soundManager.PlaySoundEffect(ESoundEffectType.deliveryGetGift);     // 효과음 실행

            gift = chimney.gift;

            inventory.RemoveItem(gift, false);       // 인벤토리에서 제거
            if (gift.wishCount > 0)                     // 위시리스트에 있었던 것들은 위시카운트 감소
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

    #region 코루틴
    /// <summary>
    /// 굴뚝으로 들어가지 못했을 때 몇초 뒤 사라짐
    /// </summary>
    IEnumerator Dissapear()
    {
        yield return oneSec;

        giftCollider.gameObject.SetActive(true);
        //yield return oneSec;

        if (!giftCollider.isRemove)                 // GiftCollider가 근처 굴뚝을 찾지 못하여 인벤토리에서 아이템을 제거할 수 없었다면, 인벤토리의 랜덤 아이템을 제거
        {
            gift = inventory.RandomGet();
            inventory.RemoveItem(gift, false);
        }

        yield return oneSec;

        objectPoolingManager.Set(this.gameObject, EDeliveryFlag.gift);

        // 떨어뜨릴 선물이 없다면 게임 종료
        if (deliveryGameManager.GiftCount == 0)
        {
            deliveryGameManager.End(true);
        }
    }
    #endregion
}
