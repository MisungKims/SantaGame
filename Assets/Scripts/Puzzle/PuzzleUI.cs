/**
 * @brief ������ UI
 * @author ��̼�
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleUI : MonoBehaviour
{
    #region ����
    public EGiftType puzzleType;

    // UI ����
    [SerializeField]
    private GameObject successButton;       // ���� �ϼ� ��ư
    [SerializeField]
    private Text puzzleNameText;
    [SerializeField]
    private Image PuzzleImage;          // ���� ���� �̹���
    [SerializeField]
    private Image[] PieceImages;          // ���� ���� ��ġ

    // ������Ƽ
    public string PuzzleName
    {
        set { puzzleNameText.text = value; }
    }

    // ĳ��
    PuzzleManager puzzleManager;

    // �̱���
    private static PuzzleUI instance;
    public static PuzzleUI Instance
    {
        get { return instance; }
    }
    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        puzzleManager = PuzzleManager.Instance;
    }

    private void OnEnable()
    {
        SetPuzzle();
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// UI�� ���� �־���
    /// </summary>
    public void SetPuzzle()
    {
        switch (puzzleType)
        {
            case EGiftType.RCcar:
                PuzzleName = "RCī";

                break;
            default:
                break;
        }

        // ���� ��� �̹��� �ҷ�����
        PuzzleImage.sprite = puzzleManager.puzzleList[(int)puzzleType].puzzleImage;

        // ���� ���� �ҷ�����
        List<PuzzlePiece> puzzlePieces = puzzleManager.puzzleList[(int)puzzleType].puzzlePieceList;

        for (int i = 0; i < PieceImages.Length; i++)
        {
            PieceImages[i].sprite = puzzlePieces[i].pieceImage.sprite;
            PieceImages[i].gameObject.SetActive(puzzlePieces[i].isGet);   // ���� ���� �����̸� ������ �ʰ�

            //PieceImages[i].gameObject.transform.GetComponent<RectTransform>().anchoredPosition = puzzlePieces[i].pieceImage.transform.GetComponent<RectTransform>().anchoredPosition;
        }
    }

    /// <summary>
    /// ���� ���� ȹ�� �� ȹ���� ������ �����ǿ� ���̰�
    /// </summary>
    public void RefreshPieceImage(int index)
    {
        PieceImages[index].gameObject.SetActive(true);

        // ������ �� �ϼ��ߴٸ� �ϼ� ��ư ������
        if (PuzzleManager.Instance.puzzleList[(int)puzzleType].isSuccess)
        {
            successButton.SetActive(true);
        }
    }

    /// <summary>
    /// �ϼ� ��ư Ŭ�� �� �ش� ������ ���� �ޱ� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void ClickSuccessButton()
    {
        GiftManager.Instance.ReceiveGift(GiftManager.Instance.giftList[(int)puzzleType]);
    }
    #endregion
}
