/**
 * @brief ���� ���� ������ ����� ������
 * @author ��̼�
 * @date 22-05-14
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : Background
{
    public bool isMove;     // ������ ������?

    protected override void Update()
    {
        if (isMove)         // ������ ���۵� �� �����̱� ����
        {
            base.Update();
        }
    }
}
