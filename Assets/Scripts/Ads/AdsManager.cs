/**
 * @brief ���� ���� ���� ���� ȹ��
 * @author ��̼�
 * @date 22-06-05
 */

using UnityEngine;
using GoogleMobileAds.Api;
using System;
using System.Collections.Generic;

public class AdsManager : MonoBehaviour
{
    #region ����
    private RewardedAd rewardedAd;
    private string adUnitId;

    enum EAdType { puzzle, dia };
    EAdType adType;     // ���� Ÿ��

    private ObjectManager objectManager;
    #endregion

    public void Start()
    {
        // ����� ���� SDK�� �ʱ�ȭ��.
        MobileAds.Initialize(initStatus => { });

        CreateAndLoadRewardedAd();
    }

    public void CreateAndLoadRewardedAd()
    {
#if UNITY_ANDROID
        //adUnitId ����
        adUnitId = "ca-app-pub-3940256099942544/5224354917";        // �׽�Ʈ ���̵�
#else
            string adUnitId = "unexpected_platform";
#endif

        this.rewardedAd = new RewardedAd(adUnitId);

        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded; // ���� �ε尡 �Ϸ�Ǹ� ȣ��
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad; // ���� �ε尡 �������� �� ȣ��
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening; // ���� ǥ�õ� �� ȣ��(��� ȭ���� ����)
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow; // ���� ǥ�ð� �������� �� ȣ��
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;// ���� ��û�� �� ������ �޾ƾ��� �� ȣ��
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed; // �ݱ� ��ư�� �����ų� �ڷΰ��� ��ư�� ���� ������ ���� ���� �� ȣ��

        //���� �ε� : RewardedAd ��ü�� loadAd�޼��忡 AdRequest �ν��Ͻ��� ����
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
