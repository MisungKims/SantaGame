/**
 * @details 상점의 UI를 생성
 * @author 김미성
 * @date 22-04-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorePanel : MonoBehaviour
{
    #region 변수
    public static StorePanel instance;

    [Header("---------- 상점 오브젝트")]
    public GameObject storeObject;           // 복제가 될 상점의 건물 오브젝트
    public GameObject storeObjectParent;     // 건물 오브젝트의 부모

    [HideInInspector]
    public List<StoreObjectSc> ObjectList = new List<StoreObjectSc>();

    private string prerequisites = null;    // 먼저 구입해야 할 건물

    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    private float nextXPos = 410;

    #endregion

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }

        SetTransform();

        ReadCSV();
    }

    /// <summary>
    /// 초기 위치와 사이즈 설정
    /// </summary>
    void SetTransform()
    {
        rectTransform = storeObject.transform.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector3(206, 300, 0);

        parentRectTransform = storeObjectParent.transform.GetComponent<RectTransform>();
        parentRectTransform.sizeDelta = new Vector2(513, 298);
    }

    /// <summary>
    /// csv 리더를 통해 StoreData 파일 가져오기
    /// </summary>
    void ReadCSV()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("StoreData");

        for (int i = 0; i < data.Count; i++)         // 가져온 내용으로 StoreObject 생성
        {
            StoreInstant(
                 i,
                 data[i]["이름"].ToString(),
                 (int)data[i]["잠금 해제 레벨"],
                 (int)data[i]["초"],
                 (float)data[i]["건물 가격 배수"],
                 data[i]["건물 가격"].ToString(),
                 data[i]["골드 증가량"].ToString(),
                 data[i]["산타 이름"].ToString(),
                 (int)data[i]["산타 가격 배수"],
                 data[i]["산타 가격"].ToString(),
                 (int)data[i]["알바 효율 증가"],
                 data[i]["Desc"].ToString()
                 );
        }
    }

    /// <summary>
    /// 상점 오브젝트 생성
    /// </summary>
    void StoreInstant(int index, string buildingName, int unlockLevel, int second, float multiplyBuildingPrice, string buildingPrice, string incrementGold, string santaName, float multiplySantaPrice, string santaPrice, int efficiency, string desc)
    {
        // csv파일의 내용을 copiedStoreObject에 넣어줌

        StoreObjectSc copiedStoreObject = GameObject.Instantiate(storeObject, storeObjectParent.transform).transform.GetComponent<StoreObjectSc>();
        copiedStoreObject.transform.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition += new Vector2(nextXPos, 0);
        parentRectTransform.sizeDelta += new Vector2(375, 0);

        copiedStoreObject.index = index;
        copiedStoreObject.BuildingName = buildingName;                      // 건물 이름
        copiedStoreObject.UnlockLevel = unlockLevel;                        // 잠금 해제 가능 레벨
        copiedStoreObject.Second = second;                                    // 몇 초 마다 증가할 것인지
        copiedStoreObject.multiplyBuildingPrice = multiplyBuildingPrice;       // 업그레이드 후 건물 가격 증가 배율
        copiedStoreObject.BuildingPrice = buildingPrice;                      // 건물 가격 
        copiedStoreObject.IncrementGold = incrementGold;                    // 플레이어의 돈 증가량
        copiedStoreObject.SantaName = santaName;                    // 산타 이름
        copiedStoreObject.multiplySantaPrice = multiplySantaPrice;          // 업그레이드 후 건물 가격 증가 배율
        copiedStoreObject.SantaPrice = santaPrice;                         // 건물 가격 
        copiedStoreObject.SantaEfficiency = efficiency;
        copiedStoreObject.Desc = desc;                              // 건물의 설명

        copiedStoreObject.gameObject.SetActive(true);
        copiedStoreObject.gameObject.name = buildingName;

        copiedStoreObject.Prerequisites = prerequisites;
        prerequisites = buildingName;

        //if (prerequisites == null)
        //{
          
        //}
        //else
        //{
        //    copiedStoreObject.Prerequisites = prerequisites;
        //    prerequisites = buildingName;
        //}

        ObjectList.Add(copiedStoreObject);
    }

}
