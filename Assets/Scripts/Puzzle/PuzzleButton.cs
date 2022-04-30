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

    #region �Լ�
    // ��ư�� ������ ���� UI�� �ش� ����� ����
    public void SetPuzzleEnum()
    {
        PuzzleUI.Instance.puzzleType = puzzleType;
        PuzzleUI.Instance.SetPuzzle();
    }
    #endregion
}
