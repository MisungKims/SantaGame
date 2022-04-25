/**
 * @brief ������ ����
 * @author ��̼�
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PuzzleManager : MonoBehaviour
{
    #region ����
    //UI����
    public Sprite[] PuzzleImages;    // ���� ��� �̹��� �迭

    public List<Puzzle> puzzleList = new List<Puzzle>();    // ������ ��� ������ ��� �ִ� ����Ʈ

    private List<Image[]> puzzlePieceList = new List<Image[]>();         // �� ������ �������� ��� �ִ� ����Ʈ
    [SerializeField]
    private Image[] rcPiece;

    [SerializeField]
    private PuzzleButton[] buttons;


    // ĳ��
    private PuzzleUI puzzleUI;
    private GetRewardWindow getRewardWindow;


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

        puzzleUI = UIManager.Instance.puzzlePanel;

        getRewardWindow = UIManager.Instance.getRewardWindow;

        InitPuzzlePieceList();

        InitPuzzleList();
    }
    #endregion


    #region �Լ�

    [SerializeField]
    GameObject PuzzlePanel;
    public void OpenPuzzlePanel()
    {
        PuzzlePanel.SetActive(true);
    }

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

            Puzzle puzzle = new Puzzle(PuzzleImages[i], pieceList, buttons[i], false);
            puzzleList.Add(puzzle);
        }
    }

    // �׽�Ʈ ��
    public void get()
    {
        //GetPiece(EPuzzle.rcCar, 0);
        //GetPiece(EPuzzle.rcCar, 1);
        //GetPiece(EPuzzle.rcCar, 2);

        StartCoroutine(getCoru());
    }

    IEnumerator getCoru()
    {
        GetPiece(EGiftType.RCcar, 0);

        while (!getRewardWindow.isTouch)
        {
            yield return null;
        }

        GetPiece(EGiftType.RCcar, 1);

        while (!getRewardWindow.isTouch)
        {
            yield return null;
        }

        GetPiece(EGiftType.RCcar, 2);
    }


  
    /// <summary>
    /// ���� ���� ȹ��
    /// </summary>
    /// <param name="ePuzzle">ȹ���� ������ ����</param>
    /// <param name="pieceIndex">���� ���� �ε���</param>
    public void GetPiece(EGiftType ePuzzle, int pieceIndex)
    {
        PuzzlePiece puzzlePiece = puzzleList[(int)ePuzzle].puzzlePieceList[pieceIndex];
        puzzlePiece.isGet = true;

        puzzleList[(int)ePuzzle].puzzlePieceList[pieceIndex] = puzzlePiece;

        puzzleList[(int)ePuzzle].button.Count++;

        IsSuccess(ePuzzle);     // ������ �� �ϼ��ߴ��� Ȯ��

        // ���� UI�� �����ְ� ���� ������ �����ְ� �ִٸ� ���ΰ�ħ
        if (puzzleUI.gameObject.activeSelf && puzzleUI.puzzleType == ePuzzle)
        {
            puzzleUI.RefreshPieceImage(pieceIndex);
        }

        getRewardWindow.OpenWindow("���� ����", puzzlePiece.pieceImage.sprite);      // ���� ȹ��â ������
    }

    /// <summary>
    /// �������� ���� ���� ȹ��
    /// </summary>
    public void GetRandomPuzzle()
    {
        EGiftType RandomPuzzleIndex = GiftManager.Instance.RandomGift().giftType;        // Ȯ���� ���� �������� ���� �׸� ���ϱ�
        
        int RandomPieceIndex = Random.Range(0, 13);         // �� ������ � ������ ��������

        //GetPiece(RandomPuzzleIndex, RandomPieceIndex);
        Debug.Log(RandomPuzzleIndex + " " + RandomPieceIndex);
    }

    /// <summary>
    /// �ټ��� ���� ���� ���� ȹ��
    /// </summary>
    /// <param name="count">ȹ���� ������ ����</param>
    /// <returns></returns>
    IEnumerator GetManyRandomPiece(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GetRandomPuzzle();

            while (!getRewardWindow.isTouch)        // ���� ȹ�� â�� ���� ������ ���
            {
                yield return null;
            }
        }
    }

    /// <summary>
    /// ������ �� �ϼ��ߴ��� üũ
    /// </summary>
    /// <param name="ePuzzle">üũ�� ����</param>
    public void IsSuccess(EGiftType ePuzzle)
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
