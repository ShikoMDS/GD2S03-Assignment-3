using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Required for scene management

public class AdManager : MonoBehaviour
{
    [SerializeField] private Button continueButton; // Reference to the Continue button

    private BannerView bannerView;
    private RewardedAd rewardedAd;
    private bool rewardAdUsed = false;

    private void Start()
    {
        // Initialize the button
        if (continueButton != null)
        {
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(ShowRewardedAd);
            continueButton.gameObject.SetActive(false); // Initially hidden
        }

        // Initialize Google Mobile Ads SDK
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("Google Mobile Ads SDK initialized.");
            RequestBannerAd();
        });
    }

    private void RequestBannerAd()
    {
        // Check if the current scene is "Menu"
        if (SceneManager.GetActiveScene().name != "Menu")
        {
            Debug.Log("Banner ads are only displayed in the Menu scene.");
            return; // Exit if not in the "Menu" scene
        }

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

        Debug.Log("Banner ad requested for the Menu scene.");
    }

    private void OnDestroy()
    {
        // Properly destroy the banner ad to avoid memory leaks
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }

    private void RequestRewardedAd()
    {
        string adUnitId = "ca-app-pub-3940256099942544/5224354917"; // Replace with your ad unit ID

        // Create a new AdRequest
        AdRequest request = new AdRequest();

        // Use the Load method to request the ad
        RewardedAd.Load(adUnitId, request, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null && error.GetMessage() != null)
            {
                Debug.LogError($"RewardedAd failed to load with error: {error.GetMessage()}");
                return;
            }

            Debug.Log("RewardedAd successfully loaded.");
            rewardedAd = ad;

            // Attach event handlers
            rewardedAd.OnAdFullScreenContentClosed += HandleAdClosed;
            rewardedAd.OnAdFullScreenContentFailed += HandleAdFailedToShow;
            rewardedAd.OnAdImpressionRecorded += HandleAdImpressionRecorded;
        });
    }

    // Show the rewarded ad
    public void ShowRewardedAd()
    {
        if (rewardAdUsed)
        {
            Debug.LogWarning("Reward ad already used in this session.");
            return;
        }

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log($"User earned reward: {reward.Amount} {reward.Type}");
                HandleUserEarnedReward();
            });
        }
        else
        {
            Debug.LogWarning("Rewarded ad not ready to be shown.");
        }
    }

    public bool RewardAdUsed()
    {
        return rewardAdUsed;
    }

    private void HandleUserEarnedReward()
    {
        Debug.Log("User earned a reward! Granting an extra life...");
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.GiveExtraLife();
        }
        rewardAdUsed = true; // Mark the reward as used
        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(false); // Disable the button after use
        }
    }

    private void HandleAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("Rewarded ad loaded.");
    }

    // Define the handlers
    private void HandleAdClosed()
    {
        Debug.Log("RewardedAd closed.");
        RequestRewardedAd(); // Optionally load a new ad
    }

    private void HandleAdFailedToShow(AdError adError)
    {
        Debug.LogError($"RewardedAd failed to show with error: {adError.GetMessage()}");
    }

    private void HandleAdImpressionRecorded()
    {
        Debug.Log("Ad impression recorded.");
    }
}
