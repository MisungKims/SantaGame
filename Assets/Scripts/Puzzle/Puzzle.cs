/**
 * @brief 퍼즐 구조체
 * @author 김미성
 * @date 22-06-02
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Puzzle
{
    public Sprite puzzleImage;
    public int line;
    public List<PuzzlePiece> puzzlePieceList = new List<PuzzlePiece>();
    public bool isSuccess;

    public Puzzle(Sprite puzzleImage, int line, List<PuzzlePiece> puzzlePieceList, bool isSuccess)
    {
        this.puzzleImage = puzzleImage;
        this.line = line;
        this.puzzlePieceList = puzzlePieceList;
        this.isSuccess = isSuccess;
    }
}

[System.Serializable]
public class PuzzlePiece
{
    public Sprite pieceImage;
    public bool isGet;
}
