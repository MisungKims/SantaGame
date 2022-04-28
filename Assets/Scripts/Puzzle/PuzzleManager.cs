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
    public List<Puzzle> puzzleList = new List<Puzzle>();    // ������ ��� ������ ��� �ִ� ����Ʈ

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

    }
    #endregion


    #region �Լ�

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
      
        for (int i = 0; i < 12; i++)
        {
            GetPiece(EGiftType.RCcar, i);
            while (!getRewardWindow.isTouch)
            {
                yield return null;
            }

            
        }
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
