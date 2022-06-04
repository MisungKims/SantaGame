/**
 * @details �̹��� ����� ������ �Ʒ��� ��ũ��
 * @author ��̼�
 * @date 22-04-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����� ������ ����
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
    #region ����
    [SerializeField]
    private float speed = 0.03f;

    [SerializeField]
    private EBackgroundDirection direction;

    private Material mat;           // ���͸���
    #endregion

    #region ����Ƽ �Լ�
    private void Start()
    {
        mat = GetComponent<Image>().material;
        mat.SetTextureScale("_MainTex", Vector2.one);
    }

    protected virtual void Update()
    {
        // Material�� Offset�� y���� �����Ͽ� ����� ������
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

