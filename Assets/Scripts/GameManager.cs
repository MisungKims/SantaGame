using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    #region �̱���
    private static GameManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);      // �� ��ȯ �ÿ��� �ı����� ����
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

    #region ����
    [Header("---------- ���� ������Ʈ")]
    public Slider gaugeSlider;        // �������� ��Ÿ���� �����̴�
    public Text lvText;               // ������ ��Ÿ���� �ؽ�Ʈ
    public Text goldText;            // ���� ��Ÿ���� �ؽ�Ʈ
    public Text dateText;             // ��¥�� ��Ÿ���� �ؽ�Ʈ
    public Text timeText;             // �ð��� ��Ÿ���� �ؽ�Ʈ

    [Header("---------- �г�")]
    public GameObject mainPanel;     // ���� �г�
    public GameObject storePanel;     // ���� �г�
    public GameObject santaPanel;     // ���� �г�

    [Header("---------- ����")]
    public static float gauge;
    public static int level = 1;
    public static double myGold = 10000;
    public static float second = 0;

 
    #endregion

    #region �Լ�



    /// <summary>
    /// 1000 ���� ���� �޸��� �ٿ��ִ� �Լ�
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

    // Store Panel�� �����ֱ�
    public void ShowStorePanel()
    {
        storePanel.SetActive(true);                 

        CameraMovement.Instance.SetCanMove(false);      // ī�޶� ������ �� ����
    }

    // Store Panel�� �����
    public void HideStorePanel()
    {
        storePanel.SetActive(false);

        CameraMovement.Instance.SetCanMove(true);       // ī�޶� ������ �� �ְ�
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
