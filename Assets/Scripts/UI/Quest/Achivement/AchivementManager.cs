/**
 * @brief 업적을 관리
 * @author 김미성
 * @date 22-04-19
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchivementManager : QuestManager
{
    #region 변수
    // 싱글톤
    private static AchivementManager instance;
    public static AchivementManager Instance
    {
        get { return instance; }
    }

    #endregion
    #region 유니티 함수
    protected override void Awake()
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

        csvFileName = "AchivementData";
        questType = EQuestType.achivement;

        // UI 배치 시 필요
        startPos = new Vector3(0, 120, 0);
        startParentSize = new Vector2(0, 52);
        nextYPos = -85;
        increaseParentYSize = 10;

        base.Awake();
    }
    #endregion
}
