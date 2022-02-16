using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [Header("---------- Parse CSV")]
    public ParseCSV storeParseCSV;

    [Header("---------- 메인 오브젝트")]
    public Slider gaugeSlider;        // 게이지를 나타내는 슬라이더
    public Text lvText;               // 레벨을 나타내는 텍스트
    public Text moneyText;            // 돈을 나타내는 텍스트
    public Text dateText;             // 날짜를 나타내는 텍스트
    public Text timeText;             // 시간을 나타내는 텍스트

    [Header("---------- 상점 오브젝트")]
    public GameObject storePanel;     // 상점 패널
    
    [Header("---------- 변수")]
    public float gauge;
    public int level = 1;
    public int myMoney = 0;

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
        lvText.text = level.ToString();
    }

    public void IncreaseGauge(float amount)
    {
        gauge += amount;
        gaugeSlider.value = gauge;
    }

    public void IncreaseMoney(int amount)
    {
        myMoney += amount;
        moneyText.text = GetCommaText(myMoney);
    }

    public void DecreaseMoney(int amount)
    {
        myMoney -= amount;
        moneyText.text = GetCommaText(myMoney);
    }


    public void DoIncreaseMoney(int second, int incrementMoney)
    {
        StartCoroutine(IncreaseMoney(second, incrementMoney));
    }

    /// <summary>
    /// 정해진 시간(second)마다 돈 증가
    /// </summary>
    /// <returns></returns>
    IEnumerator IncreaseMoney(int second, int incrementMoney)
    {
        while (true)
        {
            yield return new WaitForSeconds(second);

            IncreaseMoney(incrementMoney);
        }
    }

    #endregion
   

    // Start is called before the first frame update
    void Start()
    {
        lvText.text = level.ToString();

        gaugeSlider.value = gauge;

        moneyText.text = GetCommaText(myMoney);

        storePanel.SetActive(false);
    }

}
