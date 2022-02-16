using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParseCSV : MonoBehaviour
{
    #region ����
    public string fileName;

    private int count;
    public List<string> nameList = new List<string>();
    public List<float> multiplyCostList = new List<float>();
    public List<int> costList = new List<int>();
    public List<float> multiplyMoneyList = new List<float>();
    public List<int> incrementMoneyList = new List<int>();
    public List<int> unlockLevelList = new List<int>(); 
    public List<int> secondList = new List<int>();
    public List<string> descList = new List<string>();

    #endregion

    #region �Լ�
    public int GetCount()   // ����Ʈ�� ���� Get
    {
        count = nameList.Count;
        return count;
    }
    #endregion

    #region ����Ƽ �޼ҵ�
    void Awake()    // ���� �Ŵ����� Start���� ���� ����
    {
        List<Dictionary<string, object>> data = CSVReader.Read(fileName);       // csv ������ ���� ���� ��������

        for (int i = 0; i < data.Count; i++)
        {
            nameList.Add(data[i]["Name"].ToString());
            multiplyCostList.Add((float)data[i]["Multiply Cost"]);
            costList.Add((int)data[i]["Cost"]);
            multiplyMoneyList.Add((float)data[i]["Multiply Money"]);
            incrementMoneyList.Add((int)data[i]["Increment Money"]);
            unlockLevelList.Add((int)data[i]["Unlock Level"]);
            secondList.Add((int)data[i]["Second"]);
            descList.Add(data[i]["Desc"].ToString());
        }
    }
    #endregion
}
