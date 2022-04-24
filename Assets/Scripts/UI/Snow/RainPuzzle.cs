/**
 * @brief 눈과 함께 내리는 퍼즐
 * @author 김미성
 * @date 22-04-25
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainPuzzle : MonoBehaviour
{
    [SerializeField]
    private RectTransform rect;
    private float moveSpeed;
    private bool isCatch;

    // 캐싱
    private WaitForSeconds wait;

    // Start is called before the first frame update
    void OnEnable()
    {
        isCatch = false;
        moveSpeed = Random.Range(100.0f, 200.0f);

        float randX = Random.Range(-490.0f, 490.0f);
        rect.anchoredPosition = new Vector2(randX, 344);

        float randTime = Random.Range(1.0f, 5.0f);      // 퍼즐이 언제 내려올지
        wait = new WaitForSeconds(randTime);
        StartCoroutine(Falling());      // randTime 이후 퍼즐이 내려옴
    }

    /// <summary>
    /// 아래로 떨어짐
    /// </summary>
    IEnumerator Falling()
    {
        yield return wait;

        while(rect.anchoredPosition.y > -400.0f)
        {
            this.transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

            if (isCatch) break;

            yield return null;
        }
    }

    /// <summary>
    /// 내려오는 퍼즐을 클릭했을 때 (인스펙터에서 호출)
    /// </summary>
    public void ClickPuzzle()
    {
        isCatch = true;

        PuzzleManager.Instance.GetRandomPuzzle();       // 랜덤으로 퍼즐 획득
    }    
}
