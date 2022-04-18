/**
 * @details 이미지 배경을 위에서 아래로 스크롤
 * @author 김미성
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

    private void Start()
    {
        mat = GetComponent<Image>().material;
        mat.SetTextureScale("_MainTex", Vector2.one);
    }

    void Update()
    {
        // Material의 Offset의 y값을 조정하여 위에서 아래로 움직이는 것 처럼 보이게 함

        mat.SetTextureOffset("_MainTex", new Vector2(0, Time.time * speed));
    }
}

