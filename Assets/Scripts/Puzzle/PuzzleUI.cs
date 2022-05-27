/**
 * @brief 퍼즐의 UI
 * @author 김미성
 * @date 22-05-04
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PieceObject            // 라인의 종류에 따른 퍼즐 오브젝트(UI)
{
    public GameObject LineType;
    public Image[] images;
}

public class PuzzleUI : MonoBehaviour
{
    #region 변수
    public EGiftType puzzleType;

    // UI 변수
    [SerializeField]
    private GameObject successButton;       // 퍼즐 완성 버튼
    [SerializeField]
    private Text puzzleNameText;
    [SerializeField]
    private Image PuzzleImage;          // 현재 퍼즐 이미지
    [SerializeField]
    private GameObject coverBackground;

    public List<PieceObject> pieceImages = new List<PieceObject>();

    private PieceObject currentObj;     // 현재 보여지고 있는 라인의 퍼즐 오브젝트

    // 프로퍼티
    public string PuzzleName
    {
        set { puzzleNameText.text = value; }
    }

    // 캐싱
    PuzzleManager puzzleManager;

    // 싱글톤
    private static PuzzleUI instance;
    public static PuzzleUI Instance
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

        puzzleType = EGiftType.bubbles;
        puzzleManager = PuzzleManager.Instance;
    }

    private void OnEnable()
    {
        SetPuzzle();
    }
    #endregion

    #region 함수
    /// <summary>
    /// UI에 값을 넣어줌
    /// </summary>
    public void SetPuzzle()
    {
        if (currentObj != null)
        {
            currentObj.LineType.gameObject.SetActive(false);        // 기존에 보여지고 있던 오브젝트를 끔
            currentObj = null;
        }

        PuzzleName = GiftManager.Instance.giftList[(int)puzzleType].giftName;

        // 퍼즐 배경 이미지 불러오기
        PuzzleImage.sprite = puzzleManager.puzzleList[(int)puzzleType].puzzleImage;

        // 퍼즐 조각 불러오기
        List<PuzzlePiece> puzzlePieces = puzzleManager.puzzleList[(int)puzzleType].puzzlePieceList;

        // UI에서 해당 퍼즐의 라인에 해당하는 오브젝트를 보여줌
        currentObj = pieceImages[puzzleManager.puzzleList[(int)puzzleType].line - 1];
        currentObj.LineType.SetActive(true);

        for (int i = 0; i < currentObj.images.Length; i++)
        {
            currentObj.images[i].sprite = puzzlePieces[i].pieceImage;
            currentObj.images[i].gameObject.SetActive(puzzlePieces[i].isGet);   // 얻지 않은 조각이면 보이지 않게
        }

        IsSuccess(currentObj);
    }

    /// <summary>
    /// 퍼즐 조각 UI 새로 고침
    /// </summary>
    public void RefreshPieceImage(int index)
    {
        Puzzle puzzles = puzzleManager.puzzleList[(int)puzzleType];

        currentObj.images[index].gameObject.SetActive(puzzles.puzzlePieceList[index].isGet);     // 얻었을 때만 보이도록
        IsSuccess(currentObj);
    }

    /// <summary>
    /// 퍼즐을 다 완성했다면 완성된 퍼즐과 완성 버튼 보여줌
    /// </summary>
    private void IsSuccess(PieceObject piece)
    {
        if (puzzleManager.puzzleList[(int)puzzleType].isSuccess)
        {
            piece.LineType.SetActive(false);
            coverBackground.SetActive(false);       // 완성된 퍼즐을 보여주기 위해 덮어두었던 배경을 숨김

            successButton.SetActive(true);
        }
    }

    /// <summary>
    /// 완성 버튼 클릭 시 해당 퍼즐의 선물 받기 (인스펙터에서 호출)
    /// </summary>
    public void ClickSuccessButton()
    {
        GiftManager.Instance.ReceiveGift(puzzleType);

        GameManager.Instance.IncreaseGauge(5f);

        InitPiece();
    }

    /// <summary>
    /// 퍼즐을 초기화
    /// </summary>
    public void InitPiece()
    {
        puzzleManager.puzzleList[(int)puzzleType].isSuccess = false;
        coverBackground.SetActive(true);

        // 퍼즐 조각 불러오기
        List<PuzzlePiece> puzzlePieces = puzzleManager.puzzleList[(int)puzzleType].puzzlePieceList;
        PieceObject invisibleObject = pieceImages[puzzleManager.puzzleList[(int)puzzleType].line - 1];

        // 퍼즐을 초기화
        for (int i = 0; i < puzzlePieces.Count; i++)
        {
            PuzzlePiece puzzlePiece = puzzlePieces[i];
            puzzlePiece.isGet = false;
            puzzlePieces[i] = puzzlePiece;     // 해당 퍼즐 조각의 isGet을 false로 바꿈

            invisibleObject.images[i].gameObject.SetActive(false);      // UI에서 퍼즐 조각을 안보이게
        }

        invisibleObject.LineType.SetActive(true);
        successButton.SetActive(false);
    }
    #endregion
}
