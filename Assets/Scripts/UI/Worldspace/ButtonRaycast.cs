/**
 * @brief UI를 레이캐스트를 사용해 터치했는지 확인
 * @author 김미성
 * @date 22-04-26
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRaycast : ClickScale
{
    #region 변수
    protected UIManager uIManager;
    protected SoundManager soundManager;
    #endregion

    #region 유니티 함수
    private void Awake()
    {
        uIManager = UIManager.Instance;
        soundManager = SoundManager.Instance;
    }

    protected override IEnumerator Start()
    {
        yield return StartCoroutine(base.Start()); // 베이스 호출
    }

    void Update()
    {
        DetectTouch();
    }
    #endregion

    #region 함수
    /// <summary>
    /// 버튼 터치시 동작
    /// </summary>
    protected virtual void Touched()
    {
        StartCoroutine(ScaleDown());
    }

    void DetectTouch()
    {
        if (Input.GetMouseButtonDown(0) && !UIManagerInstance().isOpenPanel)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))
            {
                if (hit.collider.CompareTag("WorldSpaceUI") && hit.collider.name == this.name)
                {
                    Touched();
                }
            }
        }
    }

    protected UIManager UIManagerInstance()
    {
        if (!uIManager)
        {
            uIManager = UIManager.Instance;
        }

        return uIManager;
    }
    #endregion
}
