/**
 * @details 패널의 스케일을 조정
 * @author 김미성
 * @date 22-05-14
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePanel : MonoBehaviour
{
    #region 변수
    private Vector3 zeroScale = new Vector3(0, 0, 1);
    private Vector3 oneScale = new Vector3(1, 1, 1);

    [SerializeField]
    private Vector3 anotherScale;

    public bool anotherVector = false;

    [SerializeField]
    private float speed = 15f;
    #endregion

    #region 유니티 함수
    private void Awake()
    {
        if (anotherVector)
        {
            oneScale = anotherScale;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(ScaleUp());
    }
    #endregion

    #region 코루틴
    /// <summary>
    /// 스케일이 0에서 1로 커짐
    /// </summary>
    /// <returns></returns>
    IEnumerator ScaleUp()
    {
        this.transform.localScale = zeroScale;

        while (Vector3.Distance(this.transform.localScale, oneScale) > 0.01f)
        {
            this.transform.localScale = Vector3.Lerp(this.transform.localScale, oneScale, Time.deltaTime * speed);

            yield return null;
        }

        this.transform.localScale = oneScale;
    }


    /// <summary>
    /// 스케일이 1에서 0으로 작아짐
    /// </summary>
    /// <returns></returns>
    IEnumerator ScaleDown()
    {
        while (Vector3.Distance(this.transform.localScale, zeroScale) > 1.0f)
        {
            this.transform.localScale = Vector3.Lerp(this.transform.localScale, zeroScale, Time.deltaTime * speed);

            yield return null;
        }

        this.transform.localScale = zeroScale;
        this.gameObject.SetActive(false);
    }
    #endregion

    #region 함수
    /// <summary>
    /// 패널을 닫을 때 스케일을 줄임 (인스펙터에서 호출)
    /// </summary>
    public void ClosePanel()
    {
        StartCoroutine(ScaleDown());
    }
    #endregion
}
