using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [Header("---------- Parse CSV")]
    public ParseCSV storeParseCSV;

    [Header("---------- ���� ������Ʈ")]
    public Slider gaugeSlider;        // �������� ��Ÿ���� �����̴�
    public Text lvText;               // ������ ��Ÿ���� �ؽ�Ʈ
    public Text moneyText;            // ���� ��Ÿ���� �ؽ�Ʈ
    public Text dateText;             // ��¥�� ��Ÿ���� �ؽ�Ʈ
    public Text timeText;             // �ð��� ��Ÿ���� �ؽ�Ʈ

    [Header("---------- ���� ������Ʈ")]
    public GameObject storePanel;     // ���� �г�
    
    [Header("---------- ����")]
    public float gauge;
    public int level = 1;
    public int myMoney = 0;

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
    /// ������ �ð�(second)���� �� ����
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
