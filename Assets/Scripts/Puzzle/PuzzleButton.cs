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

    private int count = 0;              // 가지고있는 퍼즐 조각의 개수
    public int Count
    {
        get { return count; }
        set
        {
            count = value;

            countText.text = count.ToString();
        }
    }

    // 캐싱
    private PuzzleUI puzzleUI;
    private SoundManager soundManager;
    #endregion

    #region 유니티 함수
    private void Awake()
    {
        puzzleUI = PuzzleUI.Instance;
        soundManager = SoundManager.Instance;
    }
    #endregion

    #region 함수
    /// <summary>
    /// PuzzleUI 인스턴스 반환
    /// </summary>
    /// <returns></returns>
    PuzzleUI PuzzleUIInsance()
    {
        if (!puzzleUI)
        {
            puzzleUI = PuzzleUI.Instance;
        }

        return puzzleUI;
    }

    /// <summary>
    /// 버튼을 누르면 퍼즐 UI가 해당 퍼즐로 변경 (인스펙터에서 호출)
    /// </summary>
    public void SetPuzzleEnum()
    {
        PuzzleUIInsance().SetPuzzle(puzzleType);
        soundManager.PlaySoundEffect(ESoundEffectType.uiButton);
    }
    #endregion
}
