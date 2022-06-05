/**
 * @brief 퍼즐 그림 변경 버튼
 * @author 김미성
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleButton : MonoBehaviour
{
    #region 변수
    [SerializeField]
    private EGiftType puzzleType;       // 퍼즐 그림의 종류

    [SerializeField]
    private Text countText;

    private int count = 0;
    public int Count
    {
        get { return count; }
        set
        {
            count = value;
            countText.text = count.ToString();
        }
    }
    #endregion

    #region 함수
    // 버튼을 누르면 퍼즐 UI가 해당 퍼즐로 변경
    public void SetPuzzleEnum()
    {
        PuzzleUI.Instance.puzzleType = puzzleType;
        PuzzleUI.Instance.SetPuzzle();
    }
    #endregion
}
