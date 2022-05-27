/**
 * @brief ������ UI
 * @author ��̼�
 * @date 22-05-04
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PieceObject            // ������ ������ ���� ���� ������Ʈ(UI)
{
    public GameObject LineType;
    public Image[] images;
}

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
    private GameObject coverBackground;

    public List<PieceObject> pieceImages = new List<PieceObject>();

    private PieceObject currentObj;     // ���� �������� �ִ� ������ ���� ������Ʈ

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

        puzzleType = EGiftType.bubbles;
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
        if (currentObj != null)
        {
            currentObj.LineType.gameObject.SetActive(false);        // ������ �������� �ִ� ������Ʈ�� ��
            currentObj = null;
        }

        PuzzleName = GiftManager.Instance.giftList[(int)puzzleType].giftName;

        // ���� ��� �̹��� �ҷ�����
        PuzzleImage.sprite = puzzleManager.puzzleList[(int)puzzleType].puzzleImage;

        // ���� ���� �ҷ�����
        List<PuzzlePiece> puzzlePieces = puzzleManager.puzzleList[(int)puzzleType].puzzlePieceList;

        // UI���� �ش� ������ ���ο� �ش��ϴ� ������Ʈ�� ������
        currentObj = pieceImages[puzzleManager.puzzleList[(int)puzzleType].line - 1];
        currentObj.LineType.SetActive(true);

        for (int i = 0; i < currentObj.images.Length; i++)
        {
            currentObj.images[i].sprite = puzzlePieces[i].pieceImage;
            currentObj.images[i].gameObject.SetActive(puzzlePieces[i].isGet);   // ���� ���� �����̸� ������ �ʰ�
        }

        IsSuccess(currentObj);
    }

    /// <summary>
    /// ���� ���� UI ���� ��ħ
    /// </summary>
    public void RefreshPieceImage(int index)
    {
        Puzzle puzzles = puzzleManager.puzzleList[(int)puzzleType];

        currentObj.images[index].gameObject.SetActive(puzzles.puzzlePieceList[index].isGet);     // ����� ���� ���̵���
        IsSuccess(currentObj);
    }

    /// <summary>
    /// ������ �� �ϼ��ߴٸ� �ϼ��� ����� �ϼ� ��ư ������
    /// </summary>
    private void IsSuccess(PieceObject piece)
    {
        if (puzzleManager.puzzleList[(int)puzzleType].isSuccess)
        {
            piece.LineType.SetActive(false);
            coverBackground.SetActive(false);       // �ϼ��� ������ �����ֱ� ���� ����ξ��� ����� ����

            successButton.SetActive(true);
        }
    }

    /// <summary>
    /// �ϼ� ��ư Ŭ�� �� �ش� ������ ���� �ޱ� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void ClickSuccessButton()
    {
        GiftManager.Instance.ReceiveGift(puzzleType);

        GameManager.Instance.IncreaseGauge(5f);

        InitPiece();
    }

    /// <summary>
    /// ������ �ʱ�ȭ
    /// </summary>
    public void InitPiece()
    {
        puzzleManager.puzzleList[(int)puzzleType].isSuccess = false;
        coverBackground.SetActive(true);

        // ���� ���� �ҷ�����
        List<PuzzlePiece> puzzlePieces = puzzleManager.puzzleList[(int)puzzleType].puzzlePieceList;
        PieceObject invisibleObject = pieceImages[puzzleManager.puzzleList[(int)puzzleType].line - 1];

        // ������ �ʱ�ȭ
        for (int i = 0; i < puzzlePieces.Count; i++)
        {
            PuzzlePiece puzzlePiece = puzzlePieces[i];
            puzzlePiece.isGet = false;
            puzzlePieces[i] = puzzlePiece;     // �ش� ���� ������ isGet�� false�� �ٲ�

            invisibleObject.images[i].gameObject.SetActive(false);      // UI���� ���� ������ �Ⱥ��̰�
        }

        invisibleObject.LineType.SetActive(true);
        successButton.SetActive(false);
    }
    #endregion
}
