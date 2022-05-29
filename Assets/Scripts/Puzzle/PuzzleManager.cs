/**
 * @brief ������ ����
 * @author ��̼�
 * @date 22-04-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


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

        LoadData();
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
    public void GetPiece(EGiftType ePuzzle, int pieceIndex, bool isOpenWindow)
    {
        PuzzlePiece puzzlePiece = puzzleList[(int)ePuzzle].puzzlePieceList[pieceIndex];
        puzzlePiece.isGet = true;
        puzzleList[(int)ePuzzle].puzzlePieceList[pieceIndex] = puzzlePiece;     // �ش� ���� ������ isGet�� true�� �ٲ�

        puzzleList[(int)ePuzzle].button.Count++;

        IsSuccess(ePuzzle);     // ������ �� �ϼ��ߴ��� Ȯ��

        // ���� UI�� �����ְ� ���� ������ �����ְ� �ִٸ� ���ΰ�ħ
        if (puzzleUI.gameObject.activeSelf && puzzleUI.puzzleType == ePuzzle)
        {
            puzzleUI.RefreshPieceImage(pieceIndex);
        }

        if (isOpenWindow)
        {
            getRewardWindow.OpenWindow("���� ����", puzzlePiece.pieceImage);      // ���� ȹ��â ������
        }
    }

    /// <summary>
    /// �������� ���� ���� ȹ��
    /// </summary>
    public void GetRandomPuzzle()
    {
        EGiftType RandomPuzzleIndex = GiftManager.Instance.RandomGift().giftType;        // Ȯ���� ���� �������� ���� �׸� ���ϱ�
        
        int RandomPieceIndex = Random.Range(0, 12);         // �� ������ � ������ ��������

        GetPiece(RandomPuzzleIndex, RandomPieceIndex, true);
    }

    public void GetManyRandomPiece(int count)
    {
        EGiftType RandomPuzzleIndex = GiftManager.Instance.RandomGift().giftType;        // Ȯ���� ���� �������� ���� �׸� ���ϱ�

        int RandomPieceIndex = Random.Range(0, 12);         // �� ������ � ������ ��������

        GetPiece(RandomPuzzleIndex, RandomPieceIndex, false);
    }


    ///// <summary>
    ///// �ټ��� ���� ���� ���� ȹ��
    ///// </summary>
    ///// <param name="count">ȹ���� ������ ����</param>
    ///// <returns></returns>
    //IEnumerator GetManyRandomPiece(int count)
    //{
    //    for (int i = 0; i < count; i++)
    //    {
            
    //        //GetRandomPuzzle();

    //        //while (!getRewardWindow.isTouch)        // ���� ȹ�� â�� ���� ������ ���
    //        //{
    //        //    yield return null;
    //        //}
    //    }
    //}

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

    //���� Ȱ��ȭ ���¸� �����ϴ� ����
    bool isPaused = false;

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
                /* ���� Ȱ��ȭ �Ǿ��� �� ó�� */
            }
        }
    }

    void OnApplicationQuit()
    {
        SaveData();         // �� ���� �� ������ ����
    }

    /// <summary>
    /// ������ ����
    /// </summary>
    void SaveData()
    {
        string jdata = JsonUtility.ToJson(new Serialization<Puzzle>(puzzleList));
        File.WriteAllText(Application.dataPath + "/Resources/PuzzleData.json", jdata);
    }

    /// <summary>
    /// ������ �ε�
    /// </summary>
    /// <returns>�ҷ����� ���� ����</returns>
    public bool LoadData()
    {
        FileInfo fileInfo = new FileInfo(Application.dataPath + "/Resources/PuzzleData.json");
        if (fileInfo.Exists)
        {
            string jdata = File.ReadAllText(Application.dataPath + "/Resources/PuzzleData.json");

            puzzleList = JsonUtility.FromJson<Serialization<Puzzle>>(jdata).target;
            for (int i = 0; i < puzzleList.Count; i++)
            {
                for (int j = 0; j < puzzleList[i].puzzlePieceList.Count; j++)
                {
                    if (puzzleList[i].puzzlePieceList[j].isGet)
                    {
                        puzzleList[i].button.Count++;
                    }
                }
                
            }
            

            return true;
        }

        return false;
    }
}
