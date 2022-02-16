using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParseCSV : MonoBehaviour
{
    #region 변수
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

    #region 함수
    public int GetCount()   // 리스트의 길이 Get
    {
        count = nameList.Count;
        return count;
    }
    #endregion

    #region 유니티 메소드
    void Awake()    // 게임 매니저의 Start보다 먼저 실행
    {
        List<Dictionary<string, object>> data = CSVReader.Read(fileName);       // csv 리더를 통해 파일 가져오기

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
