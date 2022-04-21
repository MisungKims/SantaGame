/**
 * @brief ������ ����
 * @author ��̼�
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���� �׸� ����
/// </summary>
public enum EPuzzle
{
    rcCar
}

public class PuzzleManager : MonoBehaviour
{
    #region ����
    //UI����
    public Image[] PuzzleImages;    // ���� ��� �̹��� �迭

    public List<Puzzle> puzzleList = new List<Puzzle>();    // ������ ��� ������ ��� �ִ� ����Ʈ

    private List<Image[]> puzzlePieceList = new List<Image[]>();         // �� ������ �������� ��� �ִ� ����Ʈ
    [SerializeField]
    private Image[] rcPiece;

    [SerializeField]
    private PuzzleButton[] buttons;


    // ĳ��
    private PuzzleUI puzzleUI;


    // �̱���
    private static PuzzleManager instance;
    public static PuzzleManager Instance
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

        puzzleUI = PuzzleUI.Instance;

        InitPuzzlePieceList();

        InitPuzzleList();
    }
    #endregion


    #region �Լ�

    /// <summary>
    /// ���� ���� ����Ʈ�� �� ������ ���� �̹����� �־���
    /// </summary>
    void InitPuzzlePieceList()
    {
        puzzlePieceList.Add(rcPiece);
    }

    /// <summary>
    /// Puzzle ����Ʈ�� ������ �� ������ �־���
    /// </summary>
    void InitPuzzleList()
    {
        for (int i = 0; i < PuzzleImages.Length; i++)
        {
            List<PuzzlePiece> pieceList = new List<PuzzlePiece>();
            for (int j = 0; j < puzzlePieceList[i].Length; j++)
            {
                PuzzlePiece puzzlePiece;
                puzzlePiece.index = j;
                puzzlePiece.pieceImage = puzzlePieceList[i][j];
                puzzlePiece.isGet = false;

                pieceList.Add(puzzlePiece);
            }

            Puzzle puzzle = new Puzzle((EPuzzle)i, PuzzleImages[i], pieceList, buttons[i], false);
            puzzleList.Add(puzzle);
        }
    }

    // �׽�Ʈ ��
    public void get()
    {
        GetPiece(EPuzzle.rcCar, 0);
        GetPiece(EPuzzle.rcCar, 1);
        GetPiece(EPuzzle.rcCar, 2);
    }

    /// <summary>
    /// ���� ���� ȹ��
    /// </summary>
    /// <param name="ePuzzle">ȹ���� ������ ����</param>
    /// <param name="pieceIndex">���� ���� �ε���</param>
    public void GetPiece(EPuzzle ePuzzle, int pieceIndex)
    {
        PuzzlePiece puzzlePiece = puzzleList[(int)ePuzzle].puzzlePieceList[pieceIndex];
        puzzlePiece.isGet = true;

        puzzleList[(int)ePuzzle].puzzlePieceList[pieceIndex] = puzzlePiece;

        puzzleList[(int)ePuzzle].button.Count++;

        IsSuccess(ePuzzle);     // ������ �� �ϼ��ߴ��� Ȯ��

        // ���� UI�� �����ְ� ���� ������ �����ְ� �ִٸ� ���ΰ�ħ
        if (PuzzleUI.Instance.gameObject.activeSelf && PuzzleUI.Instance.ePuzzle == ePuzzle)
        {
            PuzzleUI.Instance.RefreshPieceImage(pieceIndex);
        }
    }

    /// <summary>
    /// �������� ���� ���� ȹ��
    /// </summary>
    public void GetRandomPuzzle()
    {
        int RandomPuzzleIndex = Random.Range(0, (int)EPuzzle.rcCar + 1);        // rcCar�� �� ���������� �ٲٱ�
        int RandomPieceIndex = Random.Range(0, 13);

        GetPiece((EPuzzle)RandomPuzzleIndex, RandomPieceIndex);
    }

    /// <summary>
    /// ������ �� �ϼ��ߴ��� üũ
    /// </summary>
    /// <param name="ePuzzle">üũ�� ����</param>
    public void IsSuccess(EPuzzle ePuzzle)
    {
        for (int i = 0; i < puzzleList[(int)ePuzzle].puzzlePieceList.Count; i++)
        {
            if (!puzzleList[(int)ePuzzle].puzzlePieceList[i].isGet)       // ���� ���� ������ ������ üũ�� ����
            {
                return;
            }
        }

        puzzleList[(int)ePuzzle].isSuccess = true;
    }
    #endregion
}
