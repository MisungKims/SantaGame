/**
 * @brief UI�� ����ĳ��Ʈ�� ����� ��ġ�ߴ��� Ȯ��
 * @author ��̼�
 * @date 22-04-26
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRaycast : ClickScale
{
    #region ����
    protected UIManager uIManager;
    protected SoundManager soundManager;
    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        uIManager = UIManager.Instance;
        soundManager = SoundManager.Instance;
    }

    protected override IEnumerator Start()
    {
        yield return StartCoroutine(base.Start()); // ���̽� ȣ��
    }

    void Update()
    {
        DetectTouch();
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// ��ư ��ġ�� ����
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
