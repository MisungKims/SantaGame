/**
 * @brief 퍼즐의 UI
 * @author 김미성
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private Image[] PieceImages;          // 퍼즐 조각 위치

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
        switch (puzzleType)
        {
            case EGiftType.RCcar:
                PuzzleName = "RC카";

                break;
            default:
                break;
        }

        // 퍼즐 배경 이미지 불러오기
        PuzzleImage.sprite = puzzleManager.puzzleList[(int)puzzleType].puzzleImage;

        // 퍼즐 조각 불러오기
        List<PuzzlePiece> puzzlePieces = puzzleManager.puzzleList[(int)puzzleType].puzzlePieceList;

        for (int i = 0; i < PieceImages.Length; i++)
        {
            PieceImages[i].sprite = puzzlePieces[i].pieceImage.sprite;
            PieceImages[i].gameObject.SetActive(puzzlePieces[i].isGet);   // 얻지 않은 조각이면 보이지 않게

            //PieceImages[i].gameObject.transform.GetComponent<RectTransform>().anchoredPosition = puzzlePieces[i].pieceImage.transform.GetComponent<RectTransform>().anchoredPosition;
        }
    }

    /// <summary>
    /// 퍼즐 조각 획득 시 획득한 조각을 퍼즐판에 보이게
    /// </summary>
    public void RefreshPieceImage(int index)
    {
        PieceImages[index].gameObject.SetActive(true);

        // 퍼즐을 다 완성했다면 완성 버튼 보여줌
        if (PuzzleManager.Instance.puzzleList[(int)puzzleType].isSuccess)
        {
            successButton.SetActive(true);
        }
    }

    /// <summary>
    /// 완성 버튼 클릭 시 해당 퍼즐의 선물 받기 (인스펙터에서 호출)
    /// </summary>
    public void ClickSuccessButton()
    {
        GiftManager.Instance.ReceiveGift(GiftManager.Instance.giftList[(int)puzzleType]);
    }
    #endregion
}
