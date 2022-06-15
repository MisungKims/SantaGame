/**
 * @brief ���� �׸� ���� ��ư
 * @author ��̼�
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleButton : MonoBehaviour
{
    #region ����
    [SerializeField]
    private EGiftType puzzleType;       // ���� �׸��� ����

    [SerializeField]
    private Text countText;

    private int count = 0;              // �������ִ� ���� ������ ����
    public int Count
    {
        get { return count; }
        set
        {
            count = value;

            countText.text = count.ToString();
        }
    }

    // ĳ��
    private PuzzleUI puzzleUI;
    private SoundManager soundManager;
    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        puzzleUI = PuzzleUI.Instance;
        soundManager = SoundManager.Instance;
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// PuzzleUI �ν��Ͻ� ��ȯ
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
    /// ��ư�� ������ ���� UI�� �ش� ����� ���� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void SetPuzzleEnum()
    {
        PuzzleUIInsance().SetPuzzle(puzzleType);
        soundManager.PlaySoundEffect(ESoundEffectType.uiButton);
    }
    #endregion
}
