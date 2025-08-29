using GoogleMobileAds.Api;
using UnityEngine;
using System;

public class AdManager : MonoBehaviour
{

    private BannerView bannerView;
    private InterstitialAd interstitial;

    private RewardBasedVideoAd rewardBasedVideo;
    bool rewarded = false;

    [Obsolete]
    public void Start()
    {

        MobileAds.Initialize(InitializationStatus => { });


        this.rewardBasedVideo = RewardBasedVideoAd.Instance;


        this.rewardBasedVideo.OnAdRewarded += this.HandleRewardBasedVideoRewarded;
        this.rewardBasedVideo.OnAdClosed += this.HandleRewardBasedVideoClosed;

        this.RequestRewardBasedVideo();
    }

    private void Update()
    {
        if (rewarded)
        {
            rewarded = false;
            ShopManager SM = FindObjectOfType<ShopManager>();
            if (SM != null)
                SM.RewardWithAD();
        }
    }


    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }

    public void RequestBanner()
    {

        string adUnitId = "ca-app-pub-3940256099942544/6300978111";


        if (this.bannerView != null)
        {
            this.bannerView.Destroy();
        }


        this.bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);


        this.bannerView.LoadAd(this.CreateAdRequest());
    }

    public void RequestInterstitial()
    {

        string adUnitId = "ca-app-pub-3940256099942544/1033173712";



        if (this.interstitial != null)
        {
            this.interstitial.Destroy();
        }


        this.interstitial = new InterstitialAd(adUnitId);


        this.interstitial.LoadAd(this.CreateAdRequest());
    }

    public void RequestRewardBasedVideo()
    {
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";

        this.rewardBasedVideo.LoadAd(this.CreateAdRequest(), adUnitId);
    }

    public void ShowInterstitial()
    {
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
        }
        else
        {
            Debug.Log("Interstitial is not ready yet");
        }
    }

    public void ShowRewardBasedVideo()
    {

        if (this.rewardBasedVideo.IsLoaded())
        {
            this.rewardBasedVideo.Show();
        }
        else
        {
            MainMenu menu = FindObjectOfType<MainMenu>();
            if (menu != null)
                menu.messageAnim.SetTrigger("show");

            Debug.Log("Video is not ready yet");
        }
    }


    #region RewardBasedVideo callback handlers

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        this.RequestRewardBasedVideo();
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        rewarded = true;
    }

    #endregion
}
