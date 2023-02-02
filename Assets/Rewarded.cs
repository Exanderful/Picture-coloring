using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class Rewarded : MonoBehaviour
{
    private RewardedAd rewardedAd;

#if UNITY_ANDROID
    private const string rewardedUnityId = "ca-app-pub-4712482859554270/5487775253";
#else
    private const string rewardedUnityId = "";
#endif

    private void OnEnable()
    {
        rewardedAd = new RewardedAd(rewardedUnityId);
        AdRequest adRequest = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(adRequest);
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
    }
}
