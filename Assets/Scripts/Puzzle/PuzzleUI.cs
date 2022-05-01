/**
 * @brief ������ UI
 * @author ��̼�
 * @date 22-04-21
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
    //[SerializeField]
    //private Image[] PieceImages;          // ���� ���� ��ġ

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
        currentObj.LineType.gameObject.SetActive(true);

        for (int i = 0; i < currentObj.images.Length; i++)
        {
            currentObj.images[i].sprite = puzzlePieces[i].pieceImage;
            currentObj.images[i].gameObject.SetActive(puzzlePieces[i].isGet);   // ���� ���� �����̸� ������ �ʰ�
        }
    }

    /// <summary>
    /// ���� ���� ȹ�� �� ȹ���� ������ �����ǿ� ���̰�
    /// </summary>
    public void RefreshPieceImage(int index)
    {
        PieceObject visibleObject = pieceImages[puzzleManager.puzzleList[(int)puzzleType].line - 1];
        visibleObject.images[index].gameObject.SetActive(true);

        // ������ �� �ϼ��ߴٸ� �ϼ� ��ư ������
        if (puzzleManager.puzzleList[(int)puzzleType].isSuccess)
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

        InitPiece();
    }

    /// <summary>
    /// ������ �ʱ�ȭ
    /// </summary>
    public void InitPiece()
    {
        // ���� ���� �ҷ�����
        List<PuzzlePiece> puzzlePieces = puzzleManager.puzzleList[(int)puzzleType].puzzlePieceList;

        // ������ �ʱ�ȭ
        for (int i = 0; i < puzzlePieces.Count; i++)
        {
            PuzzlePiece puzzlePiece = puzzlePieces[i];
            puzzlePiece.isGet = false;
            puzzlePieces[i] = puzzlePiece;     // �ش� ���� ������ isGet�� false�� �ٲ�

            PieceObject invisibleObject = pieceImages[puzzleManager.puzzleList[(int)puzzleType].line - 1];
            invisibleObject.images[i].gameObject.SetActive(false);
        }

        successButton.SetActive(false);
    }
    #endregion
}
