/**
 * @details 눈 내리는 UI
 * @author 김미성
 * @date 22-05-01
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowPuzzle : MonoBehaviour
{
    #region 변수
    public GameObject snowPanel;            // 눈 패널

    public RainPuzzlePiece[] rainPuzzles;
    bool isAllStop;

    private WaitForSeconds waitMonth;
    #endregion

    #region 유니티 함수
    void Start()
    {
        waitMonth = new WaitForSeconds(GameManager.Instance.dayCount * 7);          // 일주일에 한번씩 눈이 내리게

        StartCoroutine(SnowTimer());
    }
    #endregion

    #region 코루틴
    /// <summary>
    /// 눈 내리는 패널 보이는 타이머
    /// </summary>
    private IEnumerator SnowTimer()
    {
        yield return waitMonth;

        // 옷가게가 열려있다면 닫힐 때까지 대기
        while (UIManager.Instance.clothesStorePanel.activeSelf)
        {
            yield return new WaitForSeconds(10f);
        }
        

        isAllStop = false;
        snowPanel.SetActive(true);

        // 모든 퍼즐이 멈출 때까지 대기 (클릭되거나 화면에서 안보일 때)
        while (!isAllStop)
        {
            for (int i = 0; i < rainPuzzles.Length; i++)
            {
                if (!rainPuzzles[i].isStop)
                {
                    i = 0;
                    yield return null;
                }
            }

            yield return null;
            isAllStop = true;
        }

        snowPanel.SetActive(false);
    }
    #endregion
}
