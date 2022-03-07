using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    #region 싱글톤
    private static GameManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);      // 씬 전환 시에도 파괴되지 않음
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    #endregion

    #region 변수
    [Header("---------- 메인 오브젝트")]
    public Slider gaugeSlider;        // 게이지를 나타내는 슬라이더
    public Text lvText;               // 레벨을 나타내는 텍스트
    public Text goldText;            // 돈을 나타내는 텍스트
    public Text dateText;             // 날짜를 나타내는 텍스트
    public Text timeText;             // 시간을 나타내는 텍스트

    [Header("---------- 패널")]
    public GameObject mainPanel;     // 상점 패널
    public GameObject storePanel;     // 상점 패널
    public GameObject santaPanel;     // 상점 패널

    [Header("---------- 변수")]
    public static float gauge;
    public static int level = 1;
    public static double myGold = 10000;
    public static float second = 0;

 
    #endregion

    #region 함수



    /// <summary>
    /// 1000 단위 마다 콤마를 붙여주는 함수
    /// </summary>
    string GetCommaText(int i)
    {
        return string.Format("{0: #,###; -#,###;0}", i);
    }

    public void LevelUp()
    {
        level++;
        lvText.text = string.Format("{0:D2}", level.ToString());
    }

    public void IncreaseGauge(float amount)
    {
        gauge += amount;
        gaugeSlider.value = gauge;
    }

    public void IncreaseGold(int amount)
    {
        myGold += amount;
        ShowMyGold();
    }

    public void DecreaseGold(int amount)
    {
        myGold -= amount;
        ShowMyGold();
    }


    public void DoIncreaseGold(int second, int incrementGold)
    {
        //StartCoroutine(IncreaseGold(second, incrementGold));
    }

    public void ShowMyGold()
    {
        goldText.text = GoldManager.ExpressUnitOfGold(myGold);
    }


    public void ShowSantaPanel()
    {
        if(!santaPanel.activeSelf)
        {
            mainPanel.SetActive(false);
            santaPanel.SetActive(true);
        }
    }

    public void HideSantaPanel()
    {
        mainPanel.SetActive(true);
        santaPanel.SetActive(false);
    }

    // Store Panel을 보여주기
    public void ShowStorePanel()
    {
        storePanel.SetActive(true);                 

        CameraMovement.Instance.SetCanMove(false);      // 카메라 움직일 수 없게
    }

    // Store Panel을 숨기기
    public void HideStorePanel()
    {
        storePanel.SetActive(false);

        CameraMovement.Instance.SetCanMove(true);       // 카메라 움직일 수 있게
    }

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        lvText.text = string.Format("{0:D2}", level); ;
        
        gaugeSlider.value = gauge;

        ShowMyGold();

        storePanel.SetActive(false);
        santaPanel.SetActive(false);

        //StartCoroutine(CalcSecond());
    }

    IEnumerator CalcSecond()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);

            second++;

            //timeText.text = string.Format("{0:D2} : {1:D2}", (int)second / 60, (int)second % 60);
        }
    }
}
