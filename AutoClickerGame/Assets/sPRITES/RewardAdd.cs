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
    string adUnitId = null;
    int adCount = 0;
    void Awake()
    {
#if UNITY_ANDROID
        adUnitId = _androidAdUnitId;
#endif

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
            adCount++;
            if (adCount <= 5)
            {
                // Dar recompensa según el número de veces que se ha reproducido el anuncio
                switch (adCount)
                {
                    case 1:
                        AutoClicker.instance.ChangeState(StateMultiplier.Cinco);
                        _showAdButton.interactable = false;
                        // Dar recompensa 1
                        break;
                    case 2:
                        print("Recompensa 2");
                        // Dar recompensa 2
                        break;
                    case 3:
                        print("Recompensa 3");
                        // Dar recompensa 3
                        break;
                    case 4:
                        print("Recompensa 4");
                        // Dar recompensa 4
                        break;
                    case 5:
                        print("Recompensa 5");
                        // Dar recompensa 5
                        // Desactivar el botón después de la quinta reproducción
                        _showAdButton.interactable = false;
                        break;
                }
            }
        }
    }
    #endregion
}
