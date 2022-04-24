/**
 * @details �̹��� ����� ������ �Ʒ��� ��ũ��
 * @author ��̼�
 * @date 22-04-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.03f;

    Material mat;

    [SerializeField]
    bool isCross;       // �밢������ ������ ������?

    private void Start()
    {
        mat = GetComponent<Image>().material;
        mat.SetTextureScale("_MainTex", Vector2.one);
    }

    void Update()
    {
        if (isCross)
        {
            // Material�� Offset�� x, y���� �����Ͽ� �밢������ �����̴� �� ó�� ���̰� ��
            mat.SetTextureOffset("_MainTex", new Vector2(Time.time * speed, Time.time * -speed));
        }
        else
        {
            // Material�� Offset�� y���� �����Ͽ� ������ �Ʒ��� �����̴� �� ó�� ���̰� ��
            mat.SetTextureOffset("_MainTex", new Vector2(0, Time.time * speed));
        }

    }
}

