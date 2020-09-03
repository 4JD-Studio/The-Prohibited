using UnityEngine;
using UnityEngine.Advertisements;

public class AdController : MonoBehaviour, IUnityAdsListener
{
    public static AdController Inistance;

    private string GooglePlayStoreID = "3692423";
    private string AppleAppStoreID = "3692422";
    private string InterstitialAd = "video";
    public bool isTargetGoolePlayStore = true;
    public bool isTestAd = false;

    private void Awake()
    {
        if (Inistance == null)
            Inistance = this;
    }

    private void Start()
    {
        Advertisement.AddListener(this);

        InitializeAdvertisment();
    }

    private void InitializeAdvertisment()
    {
        if (isTargetGoolePlayStore)
            Advertisement.Initialize(GooglePlayStoreID, isTestAd);
        else
            Advertisement.Initialize(AppleAppStoreID, isTestAd);
    }

    public void PlayInterstitialAd()
    {
        if (!Advertisement.IsReady(InterstitialAd))
            return;

        Advertisement.Show(InterstitialAd);
    }

    public void OnUnityAdsReady(string placementId){}

    public void OnUnityAdsDidError(string message){}

    public void OnUnityAdsDidStart(string placementId)
    {
        AudioController.Inistance.MusicSource.mute = true;
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        AudioController.Inistance.MusicSource.mute = false;
    }
}
