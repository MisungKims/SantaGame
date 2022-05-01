/**
 * @brief 눈과 함께 내리는 퍼즐
 * @author 김미성
 * @date 22-05-01
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainPuzzlePiece : MonoBehaviour
{
    #region 변수
    [SerializeField]
    private RectTransform rect;
    private float moveSpeed;

    public bool isStop;

    // 캐싱
    private WaitForSeconds wait;
    #endregion

    #region 유니티 함수
    private void Awake()
    {
        rect = this.GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        isStop = false;

        moveSpeed = Random.Range(100.0f, 200.0f);

        float randX = Random.Range(-490.0f, 490.0f);
        rect.anchoredPosition = new Vector2(randX, 580);

        float randTime = Random.Range(1.0f, 5.0f);      // 퍼즐이 언제 내려올지
        wait = new WaitForSeconds(randTime);

        StartCoroutine(Falling());      // randTime 이후 퍼즐이 내려옴
    }
    #endregion

    #region 코루틴
    /// <summary>
    /// 아래로 떨어짐
    /// </summary>
    IEnumerator Falling()
    {
        yield return wait;

        while (rect.anchoredPosition.y > -620.0f)
        {
            this.transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

            if (isStop) break;          // 떨어지는 도중 퍼즐을 클릭했다면 멈춤

            yield return null;
        }

        if (!isStop)
        {
            isStop = true;
            this.gameObject.SetActive(false);
        }
    }
    #endregion

    #region 함수
    /// <summary>
    /// 내려오는 퍼즐을 클릭했을 때 (인스펙터에서 호출)
    /// </summary>
    public void ClickPuzzle()
    {
        isStop = true;
        this.gameObject.SetActive(false);

        PuzzleManager.Instance.GetRandomPuzzle();       // 랜덤으로 퍼즐 획득
    }
    #endregion
}
