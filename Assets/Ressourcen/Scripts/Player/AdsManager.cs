using System;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

public class AdsManager : MonoBehaviour
{
    public event Action<Entry> CloseAd;

    private InterstitialAdLoader interstitialAdLoader;
    private Interstitial interstitial;
    private string adUnitId = "R-M-14715736-1";
    private float delayShow = 75;
    private float timer = 0;
    private Entry _entry;

#if !UNITY_EDITOR
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SetupLoader();
        RequestInterstitial();
    }

    private void Update()
    {
        if (timer >= 0)
            timer -= Time.deltaTime;
    }
#else
    private void Awake() => DontDestroyOnLoad(gameObject);
#endif

    public void ShowInterstitial(Entry _ent)
    {
#if !UNITY_EDITOR
        _entry = _ent;
        if (interstitial != null && timer <= 0)
            interstitial.Show();
        else
            CloseAd?.Invoke(_ent);

#else
            CloseAd?.Invoke(_ent);
#endif

    }

    private void SetupLoader()
    {
        interstitialAdLoader = new InterstitialAdLoader();
        interstitialAdLoader.OnAdLoaded += HandleInterstitialLoaded;
        interstitialAdLoader.OnAdFailedToLoad += HandleInterstitialFailedToLoad;
    }

    private void RequestInterstitial()
    {
        AdRequestConfiguration adRequestConfiguration = new AdRequestConfiguration.Builder(adUnitId).Build();
        interstitialAdLoader.LoadAd(adRequestConfiguration);
    }

    private void HandleInterstitialLoaded(object sender, InterstitialAdLoadedEventArgs args)
    {
        interstitial = args.Interstitial;

        interstitial.OnAdShown += HandleInterstitialShown;
        interstitial.OnAdFailedToShow += HandleInterstitialFailedToShow;
        interstitial.OnAdImpression += HandleImpression;
        interstitial.OnAdDismissed += HandleInterstitialDismissed;
    }

    private void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args) => Debug.Log("Fail load ad");

    private void HandleInterstitialDismissed(object sender, EventArgs args)
    {
        CloseAd?.Invoke(_entry);
        Time.timeScale = 1;
        timer = delayShow;
        DestroyInterstitial();
        RequestInterstitial();
    }

    private void HandleInterstitialFailedToShow(object sender, EventArgs args)
    {
        CloseAd?.Invoke(_entry);
        Time.timeScale = 1;
        timer = delayShow;
        DestroyInterstitial();
        RequestInterstitial();
    }

    private void HandleInterstitialShown(object sender, EventArgs args) => Time.timeScale = 0;

    private void HandleImpression(object sender, ImpressionData impressionData)
    {
        // при регестрации
    }

    private void DestroyInterstitial()
    {
        if (interstitial != null)
        {
            interstitial.Destroy();
            interstitial = null;
        }
    }
}
