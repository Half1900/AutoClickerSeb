using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using TMPro;

public class RewardAdd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] Button _showAdButton;
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    public AutoClicker _autoClicker;
    public TextMeshProUGUI texto;
    string adUnitId = null; // This will remain null for unsupported platforms

    void Awake()
    {
        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        adUnitId = _androidAdUnitId;
#endif

        // Disable the button until the ad is ready to show:
        _showAdButton.interactable = true;
    }
    public void LoadRewardedAd()
    {
        Advertisement.Load(adUnitId, this);
    }

    public void ShowRewardedAd()
    {
        Advertisement.Show(adUnitId, this);
        LoadRewardedAd();
    }




    #region LoadCallbacks
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Interstitial Ad Loaded");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) { }
    #endregion

    #region ShowCallbacks
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) { }

    public void OnUnityAdsShowStart(string placementId) { }

    public void OnUnityAdsShowClick(string placementId) { }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == adUnitId && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Ads Fully Watched .....");
            _autoClicker.multiplicador = 5;
            _autoClicker.boostDuration = 120f;
            texto.text = "X5";
            StartCoroutine(_autoClicker.ActivateBoost());
            _autoClicker.UpdateCountdownAndBoostText();
        }
    }
    #endregion
}
