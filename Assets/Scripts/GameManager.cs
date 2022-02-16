using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region ����
    [Header("Parse CSV")]
    public ParseCSV storeParseCSV;

    [Header("���� ������Ʈ")]
    public Slider gaugeSlider;        // �������� ��Ÿ���� �����̴�
    public Text lvText;               // ������ ��Ÿ���� �ؽ�Ʈ
    public Text moneyText;            // ���� ��Ÿ���� �ؽ�Ʈ
    public Text dateText;             // ��¥�� ��Ÿ���� �ؽ�Ʈ
    public Text timeText;             // �ð��� ��Ÿ���� �ؽ�Ʈ

    [Header("���� ������Ʈ")]
    public GameObject storePanel;     // ���� �г�
    public Text moneyTextStore;            // ���� ��Ÿ���� �ؽ�Ʈ
    public GameObject storeBuildingObject;      // ������ �� ������ �ǹ� ������Ʈ

    private List<StoreObjectSc> BuildingList = new List<StoreObjectSc>();
 

    [Header("����")]
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
        moneyTextStore.text = GetCommaText(myMoney);
    }

    public void DecreaseMoney(int amount)
    {
        myMoney -= amount;
        moneyText.text = GetCommaText(myMoney);
        moneyTextStore.text = GetCommaText(myMoney);
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


    /// <summary>
    /// �ǹ� ����Ʈ ����
    /// </summary>
    /// <param name="gameObject">������ Object</param>
    /// <param name="buildingName">�ǹ��� �̸�</param>
    /// <param name="multiply">�ǹ��� ��������</param>
    /// <param name="cost">�ǹ��� ����</param>
    /// <param name="incrementAmount">�÷��̾� Money�� ������</param>
    /// <param name="unlockLevel">��� ���� ���� ����</param>
    void GetStoreInstant(GameObject gameObject, string buildingName, float multiplyCost, int cost, float multiplyMoney, int incrementMoney, int unlockLevel, int second, string desc)
    {
        GameObject instant = GameObject.Instantiate(gameObject, gameObject.transform.position, Quaternion.identity, gameObject.transform.parent);

        // csv������ ������ storeObject�� �־���
        StoreObjectSc storeObject = instant.transform.GetComponent<StoreObjectSc>();      
        storeObject.buildingName = buildingName;
        storeObject.multiplyCost = multiplyCost;
        storeObject.cost = cost;
        storeObject.multiplyMoney = multiplyMoney;
        storeObject.incrementMoney = incrementMoney;
        storeObject.unlockLevel = unlockLevel;
        storeObject.second = second;
        storeObject.desc = desc;

        storeObject.gameObject.SetActive(true);
        storeObject.gameObject.name = buildingName;

        BuildingList.Add(storeObject);
    }


    #endregion


    // Start is called before the first frame update
    void Start()
    {
        lvText.text = level.ToString();
        storePanel.SetActive(false);
        
        // ������ �ǹ� ����Ʈ ����
        for(int i = 0; i < storeParseCSV.GetCount(); i++)
        {
            GetStoreInstant(storeBuildingObject, 
                storeParseCSV.nameList[i], 
                storeParseCSV.multiplyCostList[i], 
                storeParseCSV.costList[i],
                storeParseCSV.multiplyMoneyList[i],
                storeParseCSV.incrementMoneyList[i],
                storeParseCSV.unlockLevelList[i],
                storeParseCSV.secondList[i],
                storeParseCSV.descList[i]);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        gaugeSlider.value = gauge;
        moneyText.text = GetCommaText(myMoney);
        moneyTextStore.text = GetCommaText(myMoney);
    }
}
