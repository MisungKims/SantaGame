/**
 * @brief �� Ž��� ���� ��ư
 * @author ��̼�
 * @date 22-04-26
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamExplorerButtonRay : ButtonRaycast
{
    protected override void Touched()
    {
        UIManager.Instance.SetisOpenPanel(true);
        UIManager.Instance.puzzlePanel.gameObject.SetActive(true);
    }
}
