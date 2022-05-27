/**
 * @brief 퍼즐을 관리
 * @author 김미성
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PuzzleManager : MonoBehaviour
{
    #region 변수
    public List<Puzzle> puzzleList = new List<Puzzle>();    // 퍼즐의 모든 정보를 담고 있는 리스트

    // 캐싱
    private PuzzleUI puzzleUI;
    private GetRewardWindow getRewardWindow;


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

        puzzleUI = UIManager.Instance.puzzlePanel;

        getRewardWindow = UIManager.Instance.getRewardWindow;

    }
    #endregion


    #region 함수

    // 테스트 용
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
            GetPiece(EGiftType.phone, i, true);
            while (!getRewardWindow.isTouch)
            {
                yield return null;
            }
        }
    }


  
    /// <summary>
    /// 퍼즐 조각 획득
    /// </summary>
    /// <param name="ePuzzle">획득한 퍼즐의 종류</param>
    /// <param name="pieceIndex">퍼즐 조각 인덱스</param>
    public void GetPiece(EGiftType ePuzzle, int pieceIndex, bool isOpenWindow)
    {
        PuzzlePiece puzzlePiece = puzzleList[(int)ePuzzle].puzzlePieceList[pieceIndex];
        puzzlePiece.isGet = true;
        puzzleList[(int)ePuzzle].puzzlePieceList[pieceIndex] = puzzlePiece;     // 해당 퍼즐 조각의 isGet을 true로 바꿈

        puzzleList[(int)ePuzzle].button.Count++;

        IsSuccess(ePuzzle);     // 퍼즐을 다 완성했는지 확인

        // 퍼즐 UI가 열려있고 얻은 퍼즐을 보여주고 있다면 새로고침
        if (puzzleUI.gameObject.activeSelf && puzzleUI.puzzleType == ePuzzle)
        {
            puzzleUI.RefreshPieceImage(pieceIndex);
        }

        if (isOpenWindow)
        {
            getRewardWindow.OpenWindow("퍼즐 조각", puzzlePiece.pieceImage);      // 보상 획득창 보여줌
        }
    }

    /// <summary>
    /// 랜덤으로 퍼즐 조각 획득
    /// </summary>
    public void GetRandomPuzzle()
    {
        EGiftType RandomPuzzleIndex = GiftManager.Instance.RandomGift().giftType;        // 확률에 따라 랜덤으로 퍼즐 그림 정하기
        
        int RandomPieceIndex = Random.Range(0, 12);         // 그 퍼즐의 어떤 조각을 가져올지

        GetPiece(RandomPuzzleIndex, RandomPieceIndex, true);
    }

    public void GetManyRandomPiece(int count)
    {
        EGiftType RandomPuzzleIndex = GiftManager.Instance.RandomGift().giftType;        // 확률에 따라 랜덤으로 퍼즐 그림 정하기

        int RandomPieceIndex = Random.Range(0, 12);         // 그 퍼즐의 어떤 조각을 가져올지

        GetPiece(RandomPuzzleIndex, RandomPieceIndex, false);
    }


    ///// <summary>
    ///// 다수의 랜덤 퍼즐 조각 획득
    ///// </summary>
    ///// <param name="count">획득할 조각의 개수</param>
    ///// <returns></returns>
    //IEnumerator GetManyRandomPiece(int count)
    //{
    //    for (int i = 0; i < count; i++)
    //    {
            
    //        //GetRandomPuzzle();

    //        //while (!getRewardWindow.isTouch)        // 보상 획득 창이 닫힐 때까지 대기
    //        //{
    //        //    yield return null;
    //        //}
    //    }
    //}

    /// <summary>
    /// 퍼즐을 다 완성했는지 체크
    /// </summary>
    /// <param name="ePuzzle">체크할 퍼즐</param>
    public void IsSuccess(EGiftType ePuzzle)
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
