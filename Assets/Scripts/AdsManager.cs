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

//        // ����� ���� SDK�� �ʱ�ȭ��.
//        MobileAds.Initialize(initStatus => { });

//        //���� �ε� : RewardedAd ��ü�� loadAd�޼��忡 AdRequest �ν��Ͻ��� ����
//        AdRequest request = new AdRequest.Builder().Build();
//        this.rewardedAd = new RewardedAd(adUnitId);
//        this.rewardedAd.LoadAd(request);

//        //adUnitId ����
//#if UNITY_ANDROID
//        adUnitId = "ca-app-pub-3115045377477281/4539879882";
//#endif

//        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded; // ���� �ε尡 �Ϸ�Ǹ� ȣ��
//        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad; // ���� �ε尡 �������� �� ȣ��
//        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening; // ���� ǥ�õ� �� ȣ��(��� ȭ���� ����)
//        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow; // ���� ǥ�ð� �������� �� ȣ��
//        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;// ���� ��û�� �� ������ �޾ƾ��� �� ȣ��
//        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed; // �ݱ� ��ư�� �����ų� �ڷΰ��� ��ư�� ���� ������ ���� ���� �� ȣ��
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
