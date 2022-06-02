/**
 * @brief 편지지
 * @author 김미성
 * @date 22-06-02
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WritingPad : MonoBehaviour
{
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text contentText;

    private string postName;        // 수신인
    public string PostName
    {
        set
        {
            postName = value;
            nameText.text = postName;
        }
    }

    private string postContent;     // 편지의 내용
    public string PostConent
    {
        set
        {
            postContent = value;
            contentText.text = postContent;
        }
    }

    // 이미지 이동
    [SerializeField]
    private Image postGiftImage;
    [SerializeField]
    private GameObject postGiftInitPos;
    [SerializeField]
    private GameObject giftImageArrivedPos;

    public Sprite giftSprite;

    public bool canMove = false;

    private Vector3 zeroScale = new Vector3(0, 0, 1);
    private Vector3 bigScale = new Vector3(1.4f, 1.4f, 1.4f);
    private Vector3 oneScale = new Vector3(1, 1, 1);

    public float speed = 50f;

    public void OnEnable()
    {
        if (canMove)
        {
            postGiftImage.gameObject.SetActive(true);
            postGiftImage.transform.localPosition = postGiftInitPos.transform.localPosition;
            postGiftImage.sprite = giftSprite;

            StartCoroutine(MoveImage());
        }
        else
        {
            postGiftImage.gameObject.SetActive(false);
        }
    }

 

    
    IEnumerator MoveImage()
    {
        SoundManager.Instance.PlaySoundEffect(ESoundEffectType.newBuilding);

        postGiftImage.transform.localScale = zeroScale;

        yield return new WaitForSeconds(0.2f);

        while (Vector3.Distance(postGiftImage.transform.localScale, bigScale) > 0.05f)
        {
            postGiftImage.transform.localScale = Vector3.Lerp(postGiftImage.transform.localScale, bigScale, Time.deltaTime * 55f);

            yield return null;
        }

        postGiftImage.transform.localScale = bigScale;

        yield return new WaitForSeconds(0.03f);

        while (Vector3.Distance(postGiftImage.transform.localScale, oneScale) > 1.0f)
        {
            postGiftImage.transform.localScale = Vector3.Lerp(postGiftImage.transform.localScale, oneScale, Time.deltaTime * 5f);

            yield return null;
        }

        postGiftImage.transform.localScale = oneScale;

        yield return new WaitForSeconds(1f);

        while (Vector3.Distance(postGiftImage.transform.localPosition, giftImageArrivedPos.transform.localPosition) > 0.1f)
        {
            postGiftImage.transform.localPosition = Vector3.Lerp(postGiftImage.transform.localPosition, giftImageArrivedPos.transform.localPosition, Time.deltaTime * 3f);

            yield return null;
        }

        postGiftImage.gameObject.SetActive(false);

        //while (true)
        //{
        //    postGiftImage.transform.Translate(giftImageArrivedPos.transform.localPosition * Time.deltaTime * 2f);

        //    yield return null;
        //}
    }
}
