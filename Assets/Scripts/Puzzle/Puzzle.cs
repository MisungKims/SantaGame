/**
 * @brief ∆€¡Ò ±∏¡∂√º
 * @author ±ËπÃº∫
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle
{
    public Sprite puzzleImage;
    public List<PuzzlePiece> puzzlePieceList = new List<PuzzlePiece>();
    public PuzzleButton button;
    public bool isSuccess;

    public Puzzle(Sprite puzzleImage, List<PuzzlePiece> puzzlePieceList, PuzzleButton button, bool isSuccess)
    {
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