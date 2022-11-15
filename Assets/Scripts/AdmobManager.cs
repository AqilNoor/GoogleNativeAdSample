using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.UI;
using TMPro;

public class AdmobManager : MonoBehaviour
{
    private string adUnitId = "ca-app-pub-3940256099942544/630097811";
    private string nativeAdUnitId = "ca-app-pub-3940256099942544/2247696110";
    private BannerView bannerView;

    public GameObject MainTexture;
    public GameObject icon;
    public GameObject adchoice;
    public GameObject ctaText;

    private NativeAd nativeAd;
    private bool nativeAdLoaded;

    public void Start()
    {
        RequestConfiguration requestConfiguration =
            new RequestConfiguration.Builder()
            .SetSameAppKeyEnabled(true).build();
        MobileAds.SetRequestConfiguration(requestConfiguration);

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((s) =>
        {
            RequestBannerAd();
            RequestNativeAd();


        });
    }

    private void RequestBannerAd()
    {

        AdSize adSize = new AdSize(320, 50);
        AdRequest request = new AdRequest.Builder().Build();

        bannerView = new BannerView(adUnitId, adSize, AdPosition.Bottom);

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
    }

    private void RequestNativeAd()
    {
        Debug.Log($"Requesting Native Ad");
        AdLoader adLoader = new AdLoader.Builder(nativeAdUnitId)
        .ForNativeAd()
        .Build();

        adLoader.OnNativeAdLoaded += this.HandleNativeAdLoaded;
        adLoader.OnAdFailedToLoad += this.HandleAdFailedToLoad;
        adLoader.OnNativeAdClicked += this.HandleAdClick;
        adLoader.LoadAd(new AdRequest.Builder().Build());
    }

    private void HandleNativeAdLoaded(object sender, NativeAdEventArgs nativeAdEventArgs)
    {
        Debug.Log("Ad was loaded");
        nativeAd = nativeAdEventArgs.nativeAd;
        nativeAdLoaded = true;
    }

    private void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs nativeAdEventArgs)
    {
        Debug.LogError("Ad failed to load with error : " + nativeAdEventArgs.LoadAdError);
    }

    private void HandleAdClick(object sender, EventArgs nativeAdEventArgs)
    {
        Debug.Log("Ad click was detected!");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            RequestNativeAd();
        }

        if (nativeAdLoaded)
        {
            nativeAdLoaded = false;
            SetupNativeAd();
        }
    }

    private void SetupNativeAd()
    {
        Debug.Log(nativeAd.GetIconTexture()+"==>"+icon);
        AssignSpriteToObject(nativeAd.GetIconTexture(), icon);
        Debug.Log(nativeAd.GetImageTextures()[0] + "==>" + MainTexture);
        AssignSpriteToObject(nativeAd.GetImageTextures()[0], MainTexture);
        //Debug.Log(nativeAd.GetAdChoicesLogoTexture() + "==>" + adchoice);
        //AssignSpriteToObject(nativeAd.GetAdChoicesLogoTexture(), adchoice);
        Debug.Log(nativeAd.GetCallToActionText() + "==>" + ctaText + "===>"+ ctaText.GetComponent<TextMeshProUGUI>());
        ctaText.GetComponent<TextMeshProUGUI>().text = nativeAd.GetCallToActionText();

        nativeAd.RegisterIconImageGameObject(icon);
        nativeAd.RegisterImageGameObjects(new List<GameObject>() { MainTexture });
        //nativeAd.RegisterAdChoicesLogoGameObject(adchoice);
        nativeAd.RegisterCallToActionGameObject(ctaText);
    }

    private void AssignSpriteToObject(Texture2D spriteToAssign, GameObject obj)
    {
        var sprite = Sprite.Create(spriteToAssign,
        new Rect(0.0f, 0.0f, spriteToAssign.width, spriteToAssign.height),
        new Vector2(0.5f, 0.5f), 100.0f);
        Debug.Log(obj.name +"==>"+ obj.GetComponent<Image>());
        obj.GetComponent<Image>().sprite = sprite;
    }

}
