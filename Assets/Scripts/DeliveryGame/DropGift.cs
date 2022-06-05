/**
 * @brief 선물 전달 게임에서 산타가 떨어뜨릴 선물
 * @author 김미성
 * @date 22-06-04
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


    // 캐싱
    private DeliveryGameManager deliveryGameManager;

    private Inventory inventory;

    private ObjectPoolingManager objectPoolingManager;

    private SoundManager soundManager;

    private WaitForSeconds oneSec = new WaitForSeconds(1f);

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
        this.transform.Translate(targetPos.localPosition * Time.deltaTime * 5f);        // 아래로 떨어짐
    }

    private void OnTriggerEnter(Collider other)
    {
        // 굴뚝에 부딪히면 선물 전달 완료
        if (other.gameObject.CompareTag("Chimney"))    
        {
            Delivery(other.GetComponent<Chimney>());
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

        // GiftCollider가 근처 굴뚝을 찾지 못하여 인벤토리에서 아이템을 제거할 수 없었다면, 인벤토리의 랜덤 아이템을 제거
        if (!giftCollider.isRemove)                 
        {
            gift = inventory.RandomGet();
            inventory.RemoveItem(gift, false);
        }

        yield return oneSec;

        // 오브젝트 풀에 반환
        objectPoolingManager.Set(this.gameObject, EObjectFlag.gift);

        // 떨어뜨릴 선물이 없다면 게임 종료
        if (deliveryGameManager.GiftCount <= 0)
        {
            deliveryGameManager.End(true);
        }
    }
    #endregion

    /// <summary>
    /// 선물 전달
    /// </summary>
    #region 함수
    void Delivery(Chimney chimney)
    {
        if (chimney.isAlreadyGet)       //이미 선물을 받은 굴뚝에는 아무런 효과 없음
        {
            return;
        }

        soundManager.PlaySoundEffect(ESoundEffectType.deliveryGetGift);     // 효과음 실행

        gift = chimney.gift;

        inventory.RemoveItem(gift, false);          // 인벤토리에서 해당 선물 제거
        if (gift.giftInfo.wishCount > 0)            // 위시리스트에 있었던 것들은 위시카운트 감소
        {
            gift.giftInfo.wishCount--;
            deliveryGameManager.wishCount++;
        }

        // 등급에 따른 점수 계산
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

        deliveryGameManager.Score += score;         // 점수 획득

        objectPoolingManager.Set(this.gameObject, EObjectFlag.gift);        // 오브젝트 풀에 반환

        chimney.giftImage.transform.parent.gameObject.SetActive(false);     // 상상 풍선 이미지 비활성화

        // 더이상 줄 선물이 없으면 게임 종료
        if (deliveryGameManager.GiftCount <= 0)
        {
            deliveryGameManager.End(false);
        }
    }
    #endregion
}
