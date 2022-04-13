using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorePanel : MonoBehaviour
{
    #region 변수
    public static StorePanel Instance;

    [Header("---------- 상점 오브젝트")]
    public GameObject storeObject;              // 복제가 될 상점의 건물 오브젝트
    public GameObject storeObjectParent;              // 건물 오브젝트의 부모

    [HideInInspector]
    public List<StoreObjectSc> ObjectList = new List<StoreObjectSc>();

    private string prerequisites = null;

    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    private float margin = 410;

    #endregion

    void Awake()    // 게임 매니저의 Start보다 먼저 실행
    {
        Instance = this;

        rectTransform = storeObject.transform.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector3(206, 300, 0);

        parentRectTransform = storeObjectParent.transform.GetComponent<RectTransform>();
        parentRectTransform.sizeDelta = new Vector2(513, 298);

        ReadCSV();
    }

    void ReadCSV()
    {
        // csv 리더를 통해 StoreData 파일 가져오기
        List<Dictionary<string, object>> data = CSVReader.Read("StoreData");
        // 가져온 내용으로 StoreObject 생성하기
        for (int i = 0; i < data.Count; i++)
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
        rectTransform.anchoredPosition += new Vector2(margin, 0);
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

        if (prerequisites == null)
        {
            copiedStoreObject.Prerequisites = prerequisites;
            prerequisites = buildingName;
        }
        else
        {
            copiedStoreObject.Prerequisites = prerequisites;
            prerequisites = buildingName;
        }

        ObjectList.Add(copiedStoreObject);
    }

}
