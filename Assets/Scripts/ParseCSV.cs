using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParseCSV : MonoBehaviour
{
    #region ����
    public string fileName;

    // ���� ������Ʈ�� �ʿ��� ���� ����Ʈ
    //public List<string> buildingNameList = new List<string>();                  // �̸� ����Ʈ
    //public List<int> unlockLevelList = new List<int>();                 // ��� ���� ���� ����Ʈ
    //public List<int> secondList = new List<int>();                      // ��(�ӵ�) ����Ʈ
    //public List<float> multiplyBuildingPriceList = new List<float>();   // �ǹ� ���� ��� ����Ʈ
    //public List<int> buildingPriceList = new List<int>();               // �ǹ� ���� ����Ʈ
    //public List<float> multiplyGoldList = new List<float>();            // ��� ���� ��� ����Ʈ
    //public List<int> incrementGoldList = new List<int>();               // ��� ������ ����Ʈ
    //public List<string> santaNameList = new List<string>();             // ��Ÿ �̸� ����Ʈ
    //public List<float> multiplySantaPriceList = new List<float>();      // ��Ÿ ���� ��� ����Ʈ
    //public List<int> santaPriceList = new List<int>();                  // ��Ÿ ���� ����Ʈ
    //public List<string> descList = new List<string>();                  // ���� ����Ʈ

    #endregion

    #region �Լ�
    //public int GetCount()   // ����Ʈ�� ���� Get
    //{
    //    return storeDataList.Count;
    //}
    //#endregion

    //#region ����Ƽ �޼ҵ�
    //void Awake()    // ���� �Ŵ����� Start���� ���� ����
    //{
    //    List<Dictionary<string, object>> data = CSVReader.Read(fileName);       // csv ������ ���� ���� ��������

    //    // storeDataList�� �о�� ���� �ֱ�
    //    for (int i = 0; i < data.Count; i++)
    //    {
    //        StoreDataStruct storeData;
    //        storeData.buildingName = data[i]["�̸�"].ToString();            // �ǹ� �̸�
    //        storeData.unlockLevel = (int)data[i]["��� ���� ����"];                // ��� ���� ���� ����
    //        storeData.second = (int)data[i]["��"];                     // �� �� ���� ������ ������
    //        storeData.multiplyBuildingPrice = (float)data[i]["�ǹ� ���� ���"];    // ���׷��̵� �� �ǹ� ���� ���� ����
    //        storeData.buildingPrice = (int)data[i]["�ǹ� ����"];              // �ǹ� ���� 
    //        storeData.multiplyGold = (float)data[i]["��� ���� ���"];             // ���׷��̵� �� �÷��̾� �� ���� ����
    //        storeData.incrementGold = (int)data[i]["��� ������"];              // �÷��̾��� �� ������
    //        storeData.santaName = data[i]["��Ÿ �̸�"].ToString();               // ��Ÿ �̸�
    //        storeData.multiplySantaPrice = (float)data[i]["��Ÿ ���� ���"];       // ���׷��̵� �� �ǹ� ���� ���� ����
    //        storeData.santaPrice = (int)data[i]["��Ÿ ����"];                 // �ǹ� ���� 
    //        storeData.desc = data[i]["Desc"].ToString();                    // �ǹ��� ����

    //        //StoreDataStruct storeData = new StoreDataStruct(buildingName,
    //        //    unlockLevel,
    //        //    second,
    //        //    multiplyBuildingPrice,
    //        //    buildingPrice,
    //        //    multiplyGold,
    //        //    incrementGold,
    //        //    santaName,
    //        //    multiplySantaPrice,
    //        //    santaPrice,
    //        //    desc);

          
    //        storeDataList.Add(storeData);


    //        //buildingNameList.Add(data[i]["�̸�"].ToString());
    //        //unlockLevelList.Add((int)data[i]["��� ���� ����"]);
    //        //secondList.Add((int)data[i]["��"]);
    //        //multiplyBuildingPriceList.Add((float)data[i]["�ǹ� ���� ���"]);
    //        //buildingPriceList.Add((int)data[i]["�ǹ� ����"]);
    //        //multiplyGoldList.Add((float)data[i]["��� ���� ���"]);
    //        //incrementGoldList.Add((int)data[i]["��� ������"]);
    //        //santaNameList.Add(data[i]["��Ÿ �̸�"].ToString());
    //        //multiplySantaPriceList.Add((float)data[i]["��Ÿ ���� ���"]);
    //        //santaPriceList.Add((int)data[i]["��Ÿ ����"]);
    //        //descList.Add(data[i]["Desc"].ToString());
    //    }
    //}
    #endregion
}
