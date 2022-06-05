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


    public string PuzzleName
    {
        set { puzzleNameText.text = value; }
    }

   
    // ĳ��
    GameManager gameManager;
    PuzzleManager puzzleManager;
    GiftManager giftManager;

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

        gameManager = GameManager.Instance;
        puzzleManager = PuzzleManager.Instance;
        giftManager = GiftManager.Instance;
    }

    private void OnEnable()
    {
        SetPuzzle(puzzleType);
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// puzzleType�� ���� ������ ������
    /// </summary>
    public void SetPuzzle(EGiftType puzzleType)
    {
        this.puzzleType = puzzleType;

        if (currentObj != null)
        {
            currentObj.LineType.gameObject.SetActive(false);        // ������ �������� �ִ� line ������Ʈ�� ��
            currentObj = null;
        }

        PuzzleName = giftManager.giftList[(int)puzzleType].giftName;

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
        else
        {
            coverBackground.SetActive(true);

            successButton.SetActive(false);
        }
    }

    /// <summary>
    /// �ϼ� ��ư Ŭ�� �� �ش� ������ ���� �ޱ� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void ClickSuccessButton()
    {
        giftManager.ReceiveGift(puzzleType);

        gameManager.IncreaseGauge(5f);

        puzzleManager.puzzleButtons[(int)puzzleType].Count = 0;

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
