/**
 * @brief ���� ���� ���� ���� ȹ��
 * @author ��̼�
 * @date 22-06-05
 */

using UnityEngine;
using GoogleMobileAds.Api;
using System;
using System.Collections.Generic;

public class PuzzleAdsManager : MonoBehaviour
{
    #region ����
    RewardedAd rewardedAd;
    string adUnitId;

    PuzzleManager puzzleManager;
    #endregion

    public void Start()
    {
        // ����� ���� SDK�� �ʱ�ȭ��.
        MobileAds.Initialize(initStatus => { });

        //���� �ε� : RewardedAd ��ü�� loadAd�޼��忡 AdRequest �ν��Ͻ��� ����
        AdRequest request = new AdRequest.Builder().Build();
        this.rewardedAd = new RewardedAd(adUnitId);
        this.rewardedAd.LoadAd(request);

        //adUnitId ����
#if UNITY_ANDROID
        adUnitId = "ca-app-pub-3940256099942544/5224354917";
#endif

        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded; // ���� �ε尡 �Ϸ�Ǹ� ȣ��
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad; // ���� �ε尡 �������� �� ȣ��
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening; // ���� ǥ�õ� �� ȣ��(��� ȭ���� ����)
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow; // ���� ǥ�ð� �������� �� ȣ��
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;// ���� ��û�� �� ������ �޾ƾ��� �� ȣ��
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed; // �ݱ� ��ư�� �����ų� �ڷΰ��� ��ư�� ���� ������ ���� ���� �� ȣ��
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args) { }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args) { }

    public void HandleRewardedAdOpening(object sender, EventArgs args) { }

    public void HandleRewardedAdFailedToShow(object sender, EventArgs args) { }

    public void HandleRewardedAdClosed(object sender, EventArgs args) { }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        // ���� ���� ȹ��
        PuzzleManagerInstance().GetRandomPuzzle();
    }

    public void ShowAds()
    {
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
    }

    PuzzleManager PuzzleManagerInstance()
    {
        if (!puzzleManager)
        {
            puzzleManager = PuzzleManager.Instance;
        }

        return puzzleManager;
    }
}
