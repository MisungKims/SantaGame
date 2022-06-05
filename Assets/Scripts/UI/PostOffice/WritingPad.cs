/**
 * @brief ������
 * @author ��̼�
 * @date 22-06-02
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WritingPad : MonoBehaviour
{
    #region ����
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text contentText;

    private string postName;        // ������
    public string PostName
    {
        set
        {
            postName = value;
            nameText.text = postName;
        }
    }

    private string postContent;     // ������ ����
    public string PostConent
    {
        set
        {
            postContent = value;
            contentText.text = postContent;
        }
    }

    // �̹��� �̵�
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
    #endregion

    #region ����Ƽ �Լ�
    public void OnEnable()
    {
        // ������ ó�� �о��� �� ���ø���Ʈ�� ������ ���� ���� ������
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
    #endregion

    #region �ڷ�ƾ
    /// <summary>
    /// ���� �̹��� �̵� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveImage()
    {
        SoundManager.Instance.PlaySoundEffect(ESoundEffectType.newBuilding);

        postGiftImage.transform.localScale = zeroScale;

        yield return new WaitForSeconds(0.2f);

        // �̹����� ũ�⸦ Ű��
        while (Vector3.Distance(postGiftImage.transform.localScale, bigScale) > 0.05f)
        {
            postGiftImage.transform.localScale = Vector3.Lerp(postGiftImage.transform.localScale, bigScale, Time.deltaTime * 55f);

            yield return null;
        }

        postGiftImage.transform.localScale = bigScale;

        yield return new WaitForSeconds(0.03f);

        // �̹����� ũ�⸦ ����
        while (Vector3.Distance(postGiftImage.transform.localScale, oneScale) > 1.0f)
        {
            postGiftImage.transform.localScale = Vector3.Lerp(postGiftImage.transform.localScale, oneScale, Time.deltaTime * 5f);

            yield return null;
        }

        postGiftImage.transform.localScale = oneScale;

        yield return new WaitForSeconds(1f);

        // �̹����� ��ġ�� �ٱ����� �̵���Ŵ
        while (Vector3.Distance(postGiftImage.transform.localPosition, giftImageArrivedPos.transform.localPosition) > 0.1f)
        {
            postGiftImage.transform.localPosition = Vector3.Lerp(postGiftImage.transform.localPosition, giftImageArrivedPos.transform.localPosition, Time.deltaTime * 3f);

            yield return null;
        }

        postGiftImage.gameObject.SetActive(false);
    }
    #endregion
}
