using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    private BannerView bannerView;

    private void Start()
    {
        // Initialize Google Mobile Ads SDK
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("Google Mobile Ads SDK initialized.");
            RequestBannerAd();
        });
    }

    private void RequestBannerAd()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/6300978111"; // Test Ad Unit ID
#else
        string adUnitId = "unexpected_platform";
#endif

        // Create a Leaderboard banner ad (728x90) at the bottom of the screen
        bannerView = new BannerView(adUnitId, AdSize.Leaderboard, AdPosition.Bottom);

        // Create an ad request
        AdRequest request = new AdRequest();

        // Load the banner ad
        bannerView.LoadAd(request);
    }

    private void OnDestroy()
    {
        // Properly destroy the banner ad to avoid memory leaks
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }
}
