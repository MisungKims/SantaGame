/**
 * @brief 퍼즐을 관리
 * @author 김미성
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 퍼즐 그림 종류
/// </summary>
public enum EPuzzle
{
    rcCar
}

public class PuzzleManager : MonoBehaviour
{
    #region 변수
    //UI변수
    public Image[] PuzzleImages;    // 퍼즐 배경 이미지 배열

    public List<Puzzle> puzzleList = new List<Puzzle>();    // 퍼즐의 모든 정보를 담고 있는 리스트

    private List<Image[]> puzzlePieceList = new List<Image[]>();         // 각 퍼즐의 조각들을 담고 있는 리스트
    [SerializeField]
    private Image[] rcPiece;

    [SerializeField]
    private PuzzleButton[] buttons;


    // 캐싱
    private PuzzleUI puzzleUI;


    // 싱글톤
    private static PuzzleManager instance;
    public static PuzzleManager Instance
    {
        get { return instance; }
    }
    #endregion


    #region 유니티 함수
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


    #region 함수

    /// <summary>
    /// 퍼즐 조각 리스트에 각 퍼즐의 조각 이미지를 넣어줌
    /// </summary>
    void InitPuzzlePieceList()
    {
        puzzlePieceList.Add(rcPiece);
    }

    /// <summary>
    /// Puzzle 리스트에 퍼즐의 각 값들을 넣어줌
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

    // 테스트 용
    public void get()
    {
        GetPiece(EPuzzle.rcCar, 0);
        GetPiece(EPuzzle.rcCar, 1);
        GetPiece(EPuzzle.rcCar, 2);
    }

    /// <summary>
    /// 퍼즐 조각 획득
    /// </summary>
    /// <param name="ePuzzle">획득한 퍼즐의 종류</param>
    /// <param name="pieceIndex">퍼즐 조각 인덱스</param>
    public void GetPiece(EPuzzle ePuzzle, int pieceIndex)
    {
        PuzzlePiece puzzlePiece = puzzleList[(int)ePuzzle].puzzlePieceList[pieceIndex];
        puzzlePiece.isGet = true;

        puzzleList[(int)ePuzzle].puzzlePieceList[pieceIndex] = puzzlePiece;

        puzzleList[(int)ePuzzle].button.Count++;

        IsSuccess(ePuzzle);     // 퍼즐을 다 완성했는지 확인

        // 퍼즐 UI가 열려있고 얻은 퍼즐을 보여주고 있다면 새로고침
        if (PuzzleUI.Instance.gameObject.activeSelf && PuzzleUI.Instance.ePuzzle == ePuzzle)
        {
            PuzzleUI.Instance.RefreshPieceImage(pieceIndex);
        }
    }

    /// <summary>
    /// 랜덤으로 퍼즐 조각 획득
    /// </summary>
    public void GetRandomPuzzle()
    {
        int RandomPuzzleIndex = Random.Range(0, (int)EPuzzle.rcCar + 1);        // rcCar를 맨 마지막껄로 바꾸기
        int RandomPieceIndex = Random.Range(0, 13);

        GetPiece((EPuzzle)RandomPuzzleIndex, RandomPieceIndex);
    }

    /// <summary>
    /// 퍼즐을 다 완성했는지 체크
    /// </summary>
    /// <param name="ePuzzle">체크할 퍼즐</param>
    public void IsSuccess(EPuzzle ePuzzle)
    {
        for (int i = 0; i < puzzleList[(int)ePuzzle].puzzlePieceList.Count; i++)
        {
            if (!puzzleList[(int)ePuzzle].puzzlePieceList[i].isGet)       // 얻지 못한 조각이 있으면 체크를 멈춤
            {
                return;
            }
        }

        puzzleList[(int)ePuzzle].isSuccess = true;
    }
    #endregion
}
