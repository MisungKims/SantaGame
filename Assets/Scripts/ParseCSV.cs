using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParseCSV : MonoBehaviour
{
    #region 변수
    public string fileName;

    // 상점 오브젝트에 필요한 변수 리스트
    //public List<string> buildingNameList = new List<string>();                  // 이름 리스트
    //public List<int> unlockLevelList = new List<int>();                 // 잠금 해제 레벨 리스트
    //public List<int> secondList = new List<int>();                      // 초(속도) 리스트
    //public List<float> multiplyBuildingPriceList = new List<float>();   // 건물 가격 배수 리스트
    //public List<int> buildingPriceList = new List<int>();               // 건물 가격 리스트
    //public List<float> multiplyGoldList = new List<float>();            // 골드 증가 배수 리스트
    //public List<int> incrementGoldList = new List<int>();               // 골드 증가량 리스트
    //public List<string> santaNameList = new List<string>();             // 산타 이름 리스트
    //public List<float> multiplySantaPriceList = new List<float>();      // 산타 가격 배수 리스트
    //public List<int> santaPriceList = new List<int>();                  // 산타 가격 리스트
    //public List<string> descList = new List<string>();                  // 설명 리스트

    #endregion

    #region 함수
    //public int GetCount()   // 리스트의 길이 Get
    //{
    //    return storeDataList.Count;
    //}
    //#endregion

    //#region 유니티 메소드
    //void Awake()    // 게임 매니저의 Start보다 먼저 실행
    //{
    //    List<Dictionary<string, object>> data = CSVReader.Read(fileName);       // csv 리더를 통해 파일 가져오기

    //    // storeDataList에 읽어온 내용 넣기
    //    for (int i = 0; i < data.Count; i++)
    //    {
    //        StoreDataStruct storeData;
    //        storeData.buildingName = data[i]["이름"].ToString();            // 건물 이름
    //        storeData.unlockLevel = (int)data[i]["잠금 해제 레벨"];                // 잠금 해제 가능 레벨
    //        storeData.second = (int)data[i]["초"];                     // 몇 초 마다 증가할 것인지
    //        storeData.multiplyBuildingPrice = (float)data[i]["건물 가격 배수"];    // 업그레이드 후 건물 가격 증가 배율
    //        storeData.buildingPrice = (int)data[i]["건물 가격"];              // 건물 가격 
    //        storeData.multiplyGold = (float)data[i]["골드 증가 배수"];             // 업그레이드 후 플레이어 돈 증가 배율
    //        storeData.incrementGold = (int)data[i]["골드 증가량"];              // 플레이어의 돈 증가량
    //        storeData.santaName = data[i]["산타 이름"].ToString();               // 산타 이름
    //        storeData.multiplySantaPrice = (float)data[i]["산타 가격 배수"];       // 업그레이드 후 건물 가격 증가 배율
    //        storeData.santaPrice = (int)data[i]["산타 가격"];                 // 건물 가격 
    //        storeData.desc = data[i]["Desc"].ToString();                    // 건물의 설명

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


    //        //buildingNameList.Add(data[i]["이름"].ToString());
    //        //unlockLevelList.Add((int)data[i]["잠금 해제 레벨"]);
    //        //secondList.Add((int)data[i]["초"]);
    //        //multiplyBuildingPriceList.Add((float)data[i]["건물 가격 배수"]);
    //        //buildingPriceList.Add((int)data[i]["건물 가격"]);
    //        //multiplyGoldList.Add((float)data[i]["골드 증가 배수"]);
    //        //incrementGoldList.Add((int)data[i]["골드 증가량"]);
    //        //santaNameList.Add(data[i]["산타 이름"].ToString());
    //        //multiplySantaPriceList.Add((float)data[i]["산타 가격 배수"]);
    //        //santaPriceList.Add((int)data[i]["산타 가격"]);
    //        //descList.Add(data[i]["Desc"].ToString());
    //    }
    //}
    #endregion
}
