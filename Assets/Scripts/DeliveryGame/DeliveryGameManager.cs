/**
 * @brief 선물 전달 게임
 * @author 김미성
 * @date 22-05-14
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    private Text giftCountText;

    // 싱글톤
    private static DeliveryGameManager instance;
    public static DeliveryGameManager Instance
    {
        get { return instance; }
    }

    private int giftCount =10;
    public int GiftCount
    {
        get { return giftCount; }
        set
        {
            giftCount = value;
            giftCountText.text = giftCount.ToString();
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    void Start()
    {
        startWindow.gameObject.SetActive(true);
        santa.gameObject.SetActive(false);

        GiftCount = giftCount;
    }

    public void GameStart()
    {
        startWindow.gameObject.SetActive(false);
        santa.gameObject.SetActive(true);

        // 배경의 isMove를 true로 변경하여 배경을 움직이게 함
        background.isMove = true;
        cloud.isMove = true;
    }

    public void End()
    {
        santa.gameObject.SetActive(false);
        resultWindow.gameObject.SetActive(true);

        // 배경의 isMove를 true로 변경하여 배경을 움직이게 함
        background.isMove = false;
        cloud.isMove = false;
    }
}
