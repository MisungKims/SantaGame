/**
 * @brief ������ ����
 * @author ��̼�
 * @date 22-06-02
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class PuzzleManager : MonoBehaviour
{
    #region ����
    // ������ ��� ������ ��� �ִ� ����Ʈ
    public List<Puzzle> puzzleList = new List<Puzzle>();    

    // ���� UI ��ư ����Ʈ
    public List<PuzzleButton> puzzleButtons = new List<PuzzleButton>();    

    // ������ �̹���
    [SerializeField]
    private List<Sprite> puzzleImageList = new List<Sprite>();

    // ���� ���� Sprite �迭 ����ü
    [System.Serializable]
    struct puzzlePieceSprites
    {
        public Sprite[] sprite;
    }

    // ���� ���� �̹��� �迭 ����Ʈ
    [SerializeField]
    private List<puzzlePieceSprites> puzzlePieceImageList = new List<puzzlePieceSprites>();


    //���� Ȱ��ȭ ���¸� �����ϴ� ����
    bool isPaused = false;


    // ĳ��
    private PuzzleUI puzzleUI;
    private GetRewardWindow getRewardWindow;
    private GiftManager giftManager;

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

        giftManager = GiftManager.Instance;

        LoadData();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            isPaused = true;

            SaveData();         // ���� ��Ȱ��ȭ�Ǿ��� �� ������ ����
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
        SaveData();         // �� ���� �� ������ ����
    }
    #endregion


    #region �Լ�

    // �׽�Ʈ ��
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
    /// ���� ���� ȹ��
    /// </summary>
    /// <param name="ePuzzle">ȹ���� ������ ����</param>
    /// <param name="pieceIndex">���� ���� �ε���</param>
    /// <param name="isOpenWindow">���� ȹ��â�� �� ������?</param>
    public void GetPiece(EGiftType ePuzzle, int pieceIndex, bool isOpenWindow)
    {
        PuzzlePiece puzzlePiece = puzzleList[(int)ePuzzle].puzzlePieceList[pieceIndex];

        if (!puzzlePiece.isGet)          // ���� ���� ���������̶��
        {
            puzzlePiece.isGet = true;

            puzzleButtons[(int)ePuzzle].Count++;

            IsSuccess(ePuzzle);     // ������ �� �ϼ��ߴ��� Ȯ��

            // ���� UI�� �����ְ� ���� ������ �����ְ� �ִٸ� ���ΰ�ħ
            if (puzzleUI.gameObject.activeSelf && puzzleUI.puzzleType == ePuzzle)
            {
                puzzleUI.RefreshPieceImage(pieceIndex);
            }
        }
        
        if (isOpenWindow)
        {
            string puzzleName = giftManager.giftList[(int)ePuzzle].giftName;
            getRewardWindow.OpenWindow(puzzleName + " ���� ����", puzzlePiece.pieceImage);      // ���� ȹ��â ������
        }
    }

    /// <summary>
    /// �������� ���� ���� ȹ��
    /// </summary>
    public void GetRandomPuzzle()
    {
        // Ȯ���� ���� �������� ���� �׸� ���ϱ�
        EGiftType RandomPuzzleIndex = giftManager.RandomGift().giftType;        

        // �� ������ � ������ ��������
        int RandomPieceIndex = Random.Range(0, 12);                        

        GetPiece(RandomPuzzleIndex, RandomPieceIndex, true);
    }

    /// <summary>
    /// �ټ��� ���� ���� ���� ȹ��
    /// </summary>
    /// <param name="count">��ŭ ���� ������ ȹ���� ������</param>
    public void GetManyRandomPiece(int count)
    {
        for (int i = 0; i < count; i++)
        {
            EGiftType RandomPuzzleIndex = GiftManager.Instance.RandomGift().giftType;        // Ȯ���� ���� �������� ���� �׸� ���ϱ�

            int RandomPieceIndex = Random.Range(0, 12);         // �� ������ � ������ ��������

            GetPiece(RandomPuzzleIndex, RandomPieceIndex, false);
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
   
    /// <summary>
    /// ������ ����
    /// </summary>
    void SaveData()
    {
        string jdata = JsonUtility.ToJson(new Serialization<Puzzle>(puzzleList));
        File.WriteAllText(Application.persistentDataPath + "/PuzzleData.json", jdata);
    }

    /// <summary>
    /// ������ �ε�
    /// </summary>
    /// <returns>�ҷ����� ���� ����</returns>
    public bool LoadData()
    {
        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/PuzzleData.json");
        if (fileInfo.Exists)
        {
            string jdata = File.ReadAllText(Application.persistentDataPath + "/PuzzleData.json");

            puzzleList = JsonUtility.FromJson<Serialization<Puzzle>>(jdata).target;

            // �� �ʱ�ȭ (������ �̹���, ���� ���� ����)
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
