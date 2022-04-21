/**
 * @brief ���� ����ü
 * @author ��̼�
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle
{
    public EPuzzle ePuzzle;     // �ʿ����� ���� �� ����
    public Image puzzleImage;
    public List<PuzzlePiece> puzzlePieceList = new List<PuzzlePiece>();
    public PuzzleButton button;
    public bool isSuccess;

    public Puzzle(EPuzzle ePuzzle, Image puzzleImage, List<PuzzlePiece> puzzlePieceList, PuzzleButton button, bool isSuccess)
    {
        this.ePuzzle = ePuzzle;
        this.puzzleImage = puzzleImage;
        this.puzzlePieceList = puzzlePieceList;
        this.button = button;
        this.isSuccess = isSuccess;
    }
}

public struct PuzzlePiece
{
    public int index;
    public Image pieceImage;
    public bool isGet;
}