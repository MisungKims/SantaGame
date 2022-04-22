using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanEnterBuilding : MonoBehaviour
{
    #region 변수
    [SerializeField]
    private string title;
    [SerializeField]
    private string content;
    [SerializeField]
    private string enter;
    #endregion


    #region 함수
    /// <summary>
    /// 해당 건물을 터치했는지 체크
    /// </summary>
    void TouchBuilding()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))
            {
                if (hit.collider.name == this.name)
                {
                    UIManager.Instance.questionWindow.gameObject.SetActive(true);
                    UIManager.Instance.questionWindow.Title = title;
                    UIManager.Instance.questionWindow.Content = content;
                    UIManager.Instance.questionWindow.Button = enter;
                }
            }
        }
    }

    

    #endregion

    #region 유니티 함수
    void Update()
    {
        TouchBuilding();
    }
    #endregion
}
