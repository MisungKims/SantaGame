/**
 * @brief 선물 전달 게임
 * @author 김미성
 * @date 22-05-14
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryGameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject startWindow;

    [SerializeField]
    private GameObject resultWindow;

    [SerializeField]
    private BackgroundMove background;

    [SerializeField]
    private BackgroundMove cloud;

    [SerializeField]
    private DeliverySanta santa;

    void Start()
    {
        startWindow.gameObject.SetActive(true);
        santa.gameObject.SetActive(false);
    }

    public void GameStart()
    {
        startWindow.gameObject.SetActive(false);
        santa.gameObject.SetActive(true);

        // 배경의 isMove를 true로 변경하여 배경을 움직이게 함
        background.isMove = true;
        cloud.isMove = true;
    }

}
