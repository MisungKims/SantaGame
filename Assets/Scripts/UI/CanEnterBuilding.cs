using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanEnterBuilding : MonoBehaviour
{
    #region ����
    [SerializeField]
    private string title;
    [SerializeField]
    private string content;
    [SerializeField]
    private string enter;
    #endregion


    #region �Լ�
    /// <summary>
    /// �ش� �ǹ��� ��ġ�ߴ��� üũ
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

    #region ����Ƽ �Լ�
    void Update()
    {
        TouchBuilding();
    }
    #endregion
}
