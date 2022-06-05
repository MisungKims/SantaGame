using UnityEngine.Events;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class AdsManager : MonoBehaviour
{
    string adUnitId;

//    public void Start()
//    {
//        Quiz_Manager = GameObject.FindObjectOfType<Quiz_Manager>();
//        PlayScene_Manager = GameObject.FindObjectOfType<PlayScene_Manager>();

//        // 모바일 광고 SDK를 초기화함.
//        MobileAds.Initialize(initStatus => { });

//        //광고 로드 : RewardedAd 객체의 loadAd메서드에 AdRequest 인스턴스를 넣음
//        AdRequest request = new AdRequest.Builder().Build();
//        this.rewardedAd = new RewardedAd(adUnitId);
//        this.rewardedAd.LoadAd(request);

//        //adUnitId 설정
//#if UNITY_ANDROID
//        adUnitId = "ca-app-pub-3115045377477281/4539879882";
//#endif

//        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded; // 광고 로드가 완료되면 호출
//        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad; // 광고 로드가 실패했을 때 호출
//        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening; // 광고가 표시될 때 호출(기기 화면을 덮음)
//        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow; // 광고 표시가 실패했을 때 호출
//        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;// 광고를 시청한 후 보상을 받아야할 때 호출
//        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed; // 닫기 버튼을 누르거나 뒤로가기 버튼을 눌러 동영상 광고를 닫을 때 호출
//    }

//    public void HandleRewardedAdLoaded(object sender, EventArgs args) { }

//    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
//    {
//        Quiz_Manager.StopADs();
//    }

//    public void HandleRewardedAdOpening(object sender, EventArgs args) { }

//    public void HandleRewardedAdFailedToShow(object sender, EventArgs args) { }

//    public void HandleRewardedAdClosed(object sender, EventArgs args) { }

//    public void HandleUserEarnedReward(object sender, Reward args)
//    {
//        if (Quiz_Manager == null) Quiz_Manager = GameObject.FindObjectOfType<Quiz_Manager>();
//        Quiz_Manager.PostADs();
//    }

//    public void ShowAds()
//    {
//        if (this.rewardedAd.IsLoaded())
//        {
//            this.rewardedAd.Show();
//        }
//    }
}
