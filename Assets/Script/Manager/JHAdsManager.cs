using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.Advertisements;

public class JHAdsManager : MonoBehaviour
{

    // public delegate void DelegateSucessAdMob(bool flag);
    // public static event DelegateSucessAdMob DelSuccessAdMob;

    public static Action<bool> ACTION_ADS_COMPLETE;

    private RewardedAd rewardedAd;

    protected static JHAdsManager _instance = null;
    public static JHAdsManager Instance
    {
        ///중복 호출 방지
        // [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            if (_instance == null)
            {
                ///싱글톤 객체를 찾아서 넣는다.
                _instance = (JHAdsManager)FindObjectOfType(typeof(JHAdsManager));

                ///없다면 생성한다.
                if (_instance == null)
                {
                    string goName = typeof(JHAdsManager).ToString();
                    GameObject go = GameObject.Find(goName);
                    if (go == null)
                    {
                        go = new GameObject();
                        go.name = goName;
                    }
                    _instance = go.AddComponent<JHAdsManager>();
                }
            }
            return _instance;
        }
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Show();
        }
    }


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        string appId = "";
        string gameID = "";
        /// 테스트용
// #if UNITY_ANDROID
//         appId = "ca-app-pub-3940256099942544~3347511713";
// #elif UNITY_IPHONE
//                 appId = "ca-app-pub-3940256099942544~1458002511";
// #else
//                 appId = "unexpected_platform";
// #endif


                // 오픈용
        #if UNITY_ANDROID
                appId = "ca-app-pub-2900651421644134~1568174217";
        #elif UNITY_IPHONE
                        appId = "ca-app-pub-2900651421644134~4267038482";
        #else
                        appId = "unexpected_platform";
        #endif

        MobileAds.SetiOSAppPauseOnBackground(true);
        // admob init
        MobileAds.Initialize(appId);
        /// 유니티 init
    }



    /******************************************************************************************************/
    Enums.eVideoAds eVideoKey = Enums.eVideoAds.NONE;
    /// 동영상광고 테스트 
    public void CreateAndLoadRewardedAd(Enums.eVideoAds eKey)
    {
        eVideoKey = eKey;
        // videoKey = SceneBase.Instance.dataManager.mGetDicVideoData();
        STVideoAdsData _data = SceneBase.Instance.dataManager.mGetDicVideoData(eKey);
        string adUnitId = "unexpected_platform";
        /******************************************************************************************************/
        //// 테스트용 
// #if UNITY_EDITOR
//         adUnitId = "unused";
// #elif UNITY_ANDROID
//                 adUnitId = "ca-app-pub-3940256099942544/5224354917";
// #elif UNITY_IPHONE
//                 adUnitId = "ca-app-pub-3940256099942544/1712485313";
// #endif
        /******************************************************************************************************/
        // 오픈용
        #if UNITY_EDITOR
                adUnitId = "unused";
        #elif UNITY_ANDROID
                        adUnitId = _data.aos_id;
        #elif UNITY_IPHONE
                        adUnitId = _data.ios_id;
        #endif



        /******************************************************************************************************/
        // Create new rewarded ad instance.
        this.rewardedAd = new RewardedAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = CreateAdRequest();
        // Load the rewarded ad with the request.
        rewardedAd.LoadAd(request);
    }

    // Returns an ad request with custom ad targeting.
    private AdRequest CreateAdRequest()
    {
        /// 테스트용 디바이스 아이디 확인
        /// 안드로이드 로그창 
        // RequestConfiguration.Builder.setTestDeviceIds(Arrays.asList("33BE2250B43518CCDA7DE426D04EE231"))
        /// IOS 로그창
        // GADMobileAds.sharedInstance.requestConfiguration.testDeviceIdentifiers =@[ @"2077ef9a63d2b398840261c8221a0c9b" ];

        return new AdRequest.Builder()
            .AddTestDevice("85B25C47B718C0D52E9D7E2E62FEB203") /// 내꺼 A9
            .AddTestDevice("C4E68AA98EBC3EA2845B5162A208CEE6") // 내꺼 블루스텍
            .AddTestDevice("9c5651b029fd855014c477146c9c26fc") // 내꺼 아이폰7플러스
            .AddTestDevice("0D7FC5669658397423E9585D8718305A") // 내꺼 갤럭시 S8

            .AddTestDevice("DEF41010DC7C536207BAF00D9624215C") /// 엘리스 샤오미
            .AddTestDevice("5F080DD0831CCD602F567307E2CE6236") // 에밀리 V50
            .AddTestDevice("b51142411932b21b55c4d101ef5c8c42") // 엘리스 아이폰 

            .AddKeyword("game")
            .SetGender(Gender.Male)
            .SetBirthday(new DateTime(1985, 1, 1))
            .TagForChildDirectedTreatment(false)
            .AddExtra("color_bg", "9B30FF")
            .Build();
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");

        // ShowRewardedAd();
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        // SceneBase.Instance.ShowToast("현재 재생가능한 동영상광고가 없습니다\n잠시 후 다시 시도해주세요");
        isVideoLoad = false;
        isRewardResult = false;
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: " + args.Message);

        Transform _root = PopupManager.Instance.RootPopup;
        SceneBase.Instance.AddPopup(_root, Enums.ePopupLayer.PopupAdsErrorNetwork);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        isVideoLoad = false;
        isRewardResult = false;
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        // SceneBase.Instance.ShowToast("현재 재생가능한 동영상광고가 없습니다\n잠시 후 다시 시도해주세요");
        isVideoLoad = false;
        isRewardResult = false;

        Transform _root = PopupManager.Instance.RootPopup;
        SceneBase.Instance.AddPopup(_root, Enums.ePopupLayer.PopupAdsErrorNetwork);

        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: " + args.Message);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdClosed event received");
    }
    bool isRewardResult = false;
    ///<summary>
    /// 동영상 광고 보상
    ///</summary>
    public void HandleUserEarnedReward(object sender, Reward args)
    {
        Debug.LogError("################## 보상보상보상보상보상보상보상보상보상보상보상보상보상보상보상보상보상보상보상보상 ");
        isRewardResult = true;

        // if (DelSuccessAdMob != null)
        // {
        //     DelSuccessAdMob(true);
        // }
    }

    // 25  
    protected bool isActionAds = false;
    public bool GetIsActionAds
    {
        get { return isActionAds; }
    }

    protected bool isVideoLoad = false;
    ///<summary>
    /// 광고 연속 클릭 방지해야함 
    /// 최소 3분정도
    /// 연속으로 돌리면 리젝사유라함
    ///</summary>
    public IEnumerator IEShowAdMobVideo(Enums.eVideoAds eKey)
    {
        PopupManager.Instance.objNoneTouchBlock.SetActive(true);
        StartCoroutine(PopupManager.Instance.IEDisableNoneTouchBlock(true));


        isVideoLoad = true;
        isActionAds = true;
        CreateAndLoadRewardedAd(eKey);

        float fElapsedTime = 0f;
        float fDuration = 1.0f;
        float fUnitTime = 1.0f / fDuration;

        while (isVideoLoad == true)
        {

            if (isVideoLoad == false)
            {
                isVideoLoad = false;
                break;
            }

            if (rewardedAd.IsLoaded())
            {
                rewardedAd.Show();
            }
            Debug.LogError("########## IEShowAdMobVideo fElapsedTime : " + fElapsedTime);
            yield return YieldHelper.waitForSeconds(1000);
            fElapsedTime = fElapsedTime + (Time.deltaTime * fUnitTime);
            // plus++;
            // if (plus > 5)
            if (fElapsedTime > 0.2f)
            {
                isActionAds = false;
                isVideoLoad = false;
                string _msg = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.TextEmptyVideo);
                SceneBase.Instance.AddTestTextPopup(_msg);
                break;
            }

        }

    }

    ///<summary>
    /// 광고 연속 클릭 방지해야함 
    /// 최소 3분정도
    /// 연속으로 돌리면 리젝사유라함
    ///</summary>
    public async void AsyncShowAdMobVideo(Enums.eVideoAds eKey)
    {
        PopupManager.Instance.objNoneTouchBlock.SetActive(true);
        StartCoroutine(PopupManager.Instance.IEDisableNoneTouchBlock(true));


        isVideoLoad = true;
        isActionAds = true;
        CreateAndLoadRewardedAd(eKey);

        float fElapsedTime = 0f;
        float fDuration = 1.0f;
        float fUnitTime = 1.0f / fDuration;

        while (isVideoLoad == true)
        {

            if (isVideoLoad == false)
            {
                isVideoLoad = false;
                break;
            }

            if (rewardedAd.IsLoaded())
            {
                rewardedAd.Show();
            }
            Debug.LogError("########## IEShowAdMobVideo fElapsedTime : " + fElapsedTime);
            await System.Threading.Tasks.Task.Delay(1000);
            // yield return YieldHelper.waitForSeconds(1000);
            fElapsedTime = fElapsedTime + (Time.deltaTime * fUnitTime);
            // plus++;
            // if (plus > 5)
            if (fElapsedTime > 0.2f)
            {
                isActionAds = false;
                isVideoLoad = false;
                string _msg = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.TextEmptyVideo);
                SceneBase.Instance.AddTestTextPopup(_msg);
                //SceneBase.Instance.AddTestTextPopup("현재 플레이가능한 짧은 광고가 없습니다\n잠시 후 다시 시도해 주세요");


                break;
            }

        }
    }

    /// 이런 씨발 옘병진짜.....
    void OnApplicationFocus(bool focus)
    {
        // Debug.LogError("111111   OnApplicationFocus focus : " + focus);
        if (focus)
        {
            // Debug.LogError("2222222   OnApplicationFocus focus : " + focus);
            if (isRewardResult == true)
            {
                // Debug.LogError("33333333   OnApplicationFocus focus : " + focus);
                if (ACTION_ADS_COMPLETE != null && ACTION_ADS_COMPLETE.GetInvocationList().Length > 0)
                {
                    ACTION_ADS_COMPLETE(true);
                    isActionAds = false;
                    isRewardResult = false;
                    isVideoLoad = false;
                    // Debug.LogError("4444444444   OnApplicationFocus focus : " + focus);
                    // Debug.LogError("##################### eVideoKey : " + eVideoKey);
                    eVideoKey = Enums.eVideoAds.NONE;
                }
            }
        }
    }


}

