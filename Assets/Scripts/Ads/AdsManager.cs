/**
 * @brief 광고 보고 퍼즐 조각 획득
 * @author 김미성
 * @date 22-06-05
 */

using UnityEngine;
using GoogleMobileAds.Api;
using System;
using System.Collections.Generic;

public class AdsManager : MonoBehaviour
{
    #region 변수
    private RewardedAd rewardedAd;
    private string adUnitId;

    enum EAdType { puzzle, dia };
    EAdType adType;     // 광고 타입

    private ObjectManager objectManager;
    #endregion

    public void Start()
    {
        // 모바일 광고 SDK를 초기화함.
        MobileAds.Initialize(initStatus => { });

        CreateAndLoadRewardedAd();
    }

    public void CreateAndLoadRewardedAd()
    {
#if UNITY_ANDROID
        //adUnitId 설정
        adUnitId = "ca-app-pub-3940256099942544/5224354917";        // 테스트 아이디
#else
            string adUnitId = "unexpected_platform";
#endif

        this.rewardedAd = new RewardedAd(adUnitId);

        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded; // 광고 로드가 완료되면 호출
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad; // 광고 로드가 실패했을 때 호출
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening; // 광고가 표시될 때 호출(기기 화면을 덮음)
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow; // 광고 표시가 실패했을 때 호출
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;// 광고를 시청한 후 보상을 받아야할 때 호출
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed; // 닫기 버튼을 누르거나 뒤로가기 버튼을 눌러 동영상 광고를 닫을 때 호출

        //광고 로드 : RewardedAd 객체의 loadAd메서드에 AdRequest 인스턴스를 넣음
        AdRequest request = new AdRequest.Builder().Build();
        this.rewardedAd.LoadAd(request);
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args) { }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args) { }

    public void HandleRewardedAdOpening(object sender, EventArgs args) 
    {
        ObjectManagerInstance().isWatchingAds = true;
    }

    public void HandleRewardedAdFailedToShow(object sender, EventArgs args) { }

    public void HandleRewardedAdClosed(object sender, EventArgs args) { this.CreateAndLoadRewardedAd(); }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        switch (adType)
        {
            case EAdType.puzzle:
                RewardManager.GetReward(ERewardType.puzzle, "1");
                break;
            case EAdType.dia:
                RewardManager.GetReward(ERewardType.dia, "10");
                break;
            default:
                break;
        }
    }

    public void ShowAds(string sAdType)
    {
        if (sAdType.Equals("puzzle"))
        {
            adType = EAdType.puzzle;
        }
        else if (sAdType.Equals("dia"))
        {
            adType = EAdType.dia;
        }

        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
    }


    ObjectManager ObjectManagerInstance()
    {
        if (!objectManager)
        {
            objectManager = ObjectManager.Instance;
        }

        return objectManager;
    }
}
