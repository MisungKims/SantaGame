/**
 * @details CSV 파일을 파싱하여 오브젝트(건물, 산타) 리스트 생성
 * @author 김미성
 * @date 22-04-26
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    #region 변수

    public int unlockCount = 1;     // 잠금해제된 건물의 수
    
    // 리스트
    public List<Building> buildingList = new List<Building>();
    
    public List<Santa> santaList = new List<Santa>();
    
    public List<Object> objectList = new List<Object>();    // 오브젝트의 모든 정보를 가진 리스트

    public Sprite[] buildingSprites;
    public Sprite[] santaSprites;


    // 싱글톤
    private static ObjectManager instance;
    public static ObjectManager Instance
    {
        get { return instance; }
    }
    #endregion


    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
        }

        ReadCSV();

        SetBuilidng();

        SetSanta();
    }

    /// <summary>
    /// csv 리더를 통해 StoreData 파일 가져오기
    /// </summary>
    void ReadCSV()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("StoreData");

        for (int i = 0; i < data.Count; i++)         // 가져온 내용으로 StoreObject 생성
        {
            string prerequisites;
            if (i == 0)
                prerequisites = null;
            else prerequisites = data[i-1]["이름"].ToString();



            Object newObject = new Object(
                data[i]["Desc"].ToString(),
                (int)data[i]["잠금 해제 레벨"],
                (int)data[i]["초"],
                data[i]["이름"].ToString(),
                0,
                data[i]["건물 가격"].ToString(),
                (float)data[i]["건물 가격 배수"],
                data[i]["골드 증가량"].ToString(),
                buildingSprites[i],
                data[i]["산타 이름"].ToString(),
                0,
                data[i]["산타 가격"].ToString(),
                (int)data[i]["산타 가격 배수"],
                (int)data[i]["알바 효율 증가"],
                santaSprites[i],
                prerequisites
                );

            objectList.Add(newObject);

            //buildingList[i].buildingObj = objectList[i];
        }
    }
    /// TODO : 하이라키에서 다 셋팅 후에 밑에꺼 위로 올리기

    /// <summary>
    /// 건물의 값 초기화
    /// </summary>
    void SetBuilidng()
    {
        for (int i = 0; i < buildingList.Count; i++)
        {
            buildingList[i].buildingObj = objectList[i];
            buildingList[i].index = i;
        }
    }

    /// <summary>
    /// 산타의 값 초기화
    /// </summary>
    void SetSanta()
    {
        for (int i = 0; i < santaList.Count; i++)
        {
            santaList[i].santaObj = objectList[i];
            santaList[i].index = i;
            
            //santaList[i].InitSanta(
            //    i,
            //    objectList[i].santaName,
            //    objectList[i].multiplySantaPrice,
            //    objectList[i].santaPrice,
            //    objectList[i].santaEfficiency,
            //    buildingList[i].GetComponent<Building>()
            //    );
        }
    }
}
