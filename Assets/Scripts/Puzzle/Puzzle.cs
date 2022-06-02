/**
 * @brief ∆€¡Ò ±∏¡∂√º
 * @author ±ËπÃº∫
 * @date 22-04-21
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
   // public int index;
    public Sprite pieceImage;
    public bool isGet;
}