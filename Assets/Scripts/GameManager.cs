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
    public static int myGold = 100;
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
        lvText.text = level.ToString();
    }

    public void IncreaseGauge(float amount)
    {
        gauge += amount;
        gaugeSlider.value = gauge;
    }

    public void IncreaseGold(int amount)
    {
        myGold += amount;
        goldText.text = GetCommaText(myGold);
    }

    public void DecreaseGold(int amount)
    {
        myGold -= amount;
        goldText.text = GetCommaText(myGold);
    }


    public void DoIncreaseGold(int second, int incrementGold)
    {
        StartCoroutine(IncreaseGold(second, incrementGold));
    }

    /// <summary>
    /// ������ �ð�(second)���� �� ����
    /// </summary>
    /// <returns></returns>
    IEnumerator IncreaseGold(int second, int incrementGold)
    {
        while (true)
        {
            yield return new WaitForSeconds(second);

            IncreaseGold(incrementGold);
        }
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

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        lvText.text = level.ToString();

        gaugeSlider.value = gauge;

        goldText.text = GetCommaText(myGold);

        storePanel.SetActive(false);
        santaPanel.SetActive(false);

       
    }

    private void Update()
    {
        

        second += Time.deltaTime;

        //timeText.text = (int)second / 60 + " : " + (int)second % 60;
        timeText.text = string.Format("{0:D2} : {1:D2}", (int)second / 60, (int)second % 60);
    }

    

}
