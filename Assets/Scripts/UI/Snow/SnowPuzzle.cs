/**
 * @details �� ������ UI
 * @author ��̼�
 * @date 22-05-01
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowPuzzle : MonoBehaviour
{
    #region ����
    public GameObject snowPanel;            // �� �г�

    public RainPuzzlePiece[] rainPuzzles;
    bool isAllStop;

    private WaitForSeconds waitMonth;
    #endregion

    #region ����Ƽ �Լ�
    void Start()
    {
        waitMonth = new WaitForSeconds(GameManager.Instance.dayCount * 7);          // �����Ͽ� �ѹ��� ���� ������

        StartCoroutine(SnowTimer());
    }
    #endregion

    #region �ڷ�ƾ
    /// <summary>
    /// �� ������ �г� ���̴� Ÿ�̸�
    /// </summary>
    private IEnumerator SnowTimer()
    {
        yield return waitMonth;

        // �ʰ��԰� �����ִٸ� ���� ������ ���
        while (UIManager.Instance.clothesStorePanel.activeSelf)
        {
            yield return new WaitForSeconds(10f);
        }
        

        isAllStop = false;
        snowPanel.SetActive(true);

        // ��� ������ ���� ������ ��� (Ŭ���ǰų� ȭ�鿡�� �Ⱥ��� ��)
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
