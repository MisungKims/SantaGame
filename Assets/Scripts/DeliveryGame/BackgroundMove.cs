/**
 * @brief 선물 전달 게임의 배경의 움직임
 * @author 김미성
 * @date 22-05-14
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : Background
{
    public bool isMove;     // 움직일 것인지?

    protected override void Update()
    {
        if (isMove)         // 게임이 시작될 때 움직이기 시작
        {
            base.Update();
        }
    }
}
