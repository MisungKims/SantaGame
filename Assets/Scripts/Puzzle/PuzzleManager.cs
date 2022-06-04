/**
 * @brief 퍼즐을 관리
 * @author 김미성
 * @date 22-06-02
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class PuzzleManager : MonoBehaviour
{
    #region 변수
    // 퍼즐의 모든 정보를 담고 있는 리스트
    public List<Puzzle> puzzleList = new List<Puzzle>();    

    // 퍼즐 UI 버튼 리스트
    public List<PuzzleButton> puzzleButtons = new List<PuzzleButton>();    

    // 퍼즐의 이미지
    [SerializeField]
    private List<Sprite> puzzleImageList = new List<Sprite>();

    // 퍼즐 조각 Sprite 배열 구조체
    [System.Serializable]
    struct puzzlePieceSprites
    {
        public Sprite[] sprite;
    }

    // 퍼즐 조각 이미지 배열 리스트
    [SerializeField]
    private List<puzzlePieceSprites> puzzlePieceImageList = new List<puzzlePieceSprites>();


    //앱의 활성화 상태를 저장하는 변수
    bool isPaused = false;


    // 캐싱
    private PuzzleUI puzzleUI;
    private GetRewardWindow getRewardWindow;
    private GiftManager giftManager;

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

        giftManager = GiftManager.Instance;

        LoadData();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            isPaused = true;

            SaveData();         // 앱이 비활성화되었을 때 데이터 저장
        }

        else
        {
            if (isPaused)
            {
                isPaused = false;
            }
        }
    }

    void OnApplicationQuit()
    {
        SaveData();         // 앱 종료 시 데이터 저장
    }
    #endregion


    #region 함수

    // 테스트 용
    public void get()
    {
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
    /// <param name="isOpenWindow">보상 획득창을 열 것인지?</param>
    public void GetPiece(EGiftType ePuzzle, int pieceIndex, bool isOpenWindow)
    {
        PuzzlePiece puzzlePiece = puzzleList[(int)ePuzzle].puzzlePieceList[pieceIndex];

        if (!puzzlePiece.isGet)          // 새로 얻은 퍼즐조각이라면
        {
            puzzlePiece.isGet = true;

            puzzleButtons[(int)ePuzzle].Count++;

            IsSuccess(ePuzzle);     // 퍼즐을 다 완성했는지 확인

            // 퍼즐 UI가 열려있고 얻은 퍼즐을 보여주고 있다면 새로고침
            if (puzzleUI.gameObject.activeSelf && puzzleUI.puzzleType == ePuzzle)
            {
                puzzleUI.RefreshPieceImage(pieceIndex);
            }
        }
        
        if (isOpenWindow)
        {
            string puzzleName = giftManager.giftList[(int)ePuzzle].giftName;
            getRewardWindow.OpenWindow(puzzleName + " 퍼즐 조각", puzzlePiece.pieceImage);      // 보상 획득창 보여줌
        }
    }

    /// <summary>
    /// 랜덤으로 퍼즐 조각 획득
    /// </summary>
    public void GetRandomPuzzle()
    {
        // 확률에 따라 랜덤으로 퍼즐 그림 정하기
        EGiftType RandomPuzzleIndex = giftManager.RandomGift().giftType;        

        // 그 퍼즐의 어떤 조각을 가져올지
        int RandomPieceIndex = Random.Range(0, 12);                        

        GetPiece(RandomPuzzleIndex, RandomPieceIndex, true);
    }

    /// <summary>
    /// 다수의 랜덤 퍼즐 조각 획득
    /// </summary>
    /// <param name="count">얼만큼 퍼즐 조각을 획득할 것인지</param>
    public void GetManyRandomPiece(int count)
    {
        for (int i = 0; i < count; i++)
        {
            EGiftType RandomPuzzleIndex = GiftManager.Instance.RandomGift().giftType;        // 확률에 따라 랜덤으로 퍼즐 그림 정하기

            int RandomPieceIndex = Random.Range(0, 12);         // 그 퍼즐의 어떤 조각을 가져올지

            GetPiece(RandomPuzzleIndex, RandomPieceIndex, false);
        }
    }


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
   
    /// <summary>
    /// 데이터 저장
    /// </summary>
    void SaveData()
    {
        string jdata = JsonUtility.ToJson(new Serialization<Puzzle>(puzzleList));
        File.WriteAllText(Application.persistentDataPath + "/PuzzleData.json", jdata);
    }

    /// <summary>
    /// 데이터 로드
    /// </summary>
    /// <returns>불러오기 성공 여부</returns>
    public bool LoadData()
    {
        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/PuzzleData.json");
        if (fileInfo.Exists)
        {
            string jdata = File.ReadAllText(Application.persistentDataPath + "/PuzzleData.json");

            puzzleList = JsonUtility.FromJson<Serialization<Puzzle>>(jdata).target;

            // 값 초기화 (퍼즐의 이미지, 퍼즐 조각 개수)
            for (int i = 0; i < puzzleList.Count; i++)
            {
                puzzleList[i].puzzleImage = puzzleImageList[i];

                for (int j = 0; j < puzzleList[i].puzzlePieceList.Count; j++)
                {
                    puzzleList[i].puzzlePieceList[j].pieceImage = puzzlePieceImageList[i].sprite[j];
                    if (puzzleList[i].puzzlePieceList[j].isGet)
                    {
                        puzzleButtons[i].Count++;
                    }
                }
            }
            

            return true;
        }

        return false;
    }
    #endregion
}
