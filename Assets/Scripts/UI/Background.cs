/**
 * @details 이미지 배경을 위에서 아래로 스크롤
 * @author 김미성
 * @date 22-04-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 배경이 움직일 방향
/// </summary>
enum EBackgroundDirection
{
    leftToRight,
    rightToLeft,
    upToDown,
    cross
}

public class Background : MonoBehaviour
{
    #region 변수
    [SerializeField]
    private float speed = 0.03f;

    [SerializeField]
    private EBackgroundDirection direction;

    private Material mat;           // 머터리얼
    #endregion

    #region 유니티 함수
    private void Start()
    {
        mat = GetComponent<Image>().material;
        mat.SetTextureScale("_MainTex", Vector2.one);
    }

    protected virtual void Update()
    {
        // Material의 Offset의 y값을 조정하여 배경을 움직임
        switch (direction)
        {
            case EBackgroundDirection.leftToRight:
                mat.SetTextureOffset("_MainTex", new Vector2(Time.time * speed, 0));
                break;

            case EBackgroundDirection.rightToLeft:
                mat.SetTextureOffset("_MainTex", new Vector2(Time.time * -speed, 0));
                break;

            case EBackgroundDirection.upToDown:
                mat.SetTextureOffset("_MainTex", new Vector2(0, Time.time * speed));
                break;

            case EBackgroundDirection.cross:
                mat.SetTextureOffset("_MainTex", new Vector2(Time.time * speed, Time.time * -speed));
                break;
            default:
                break;
        }
    }
    #endregion
}

