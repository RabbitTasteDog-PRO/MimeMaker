using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBase : JHScene
{

    [Header("오디오")]
    public AudioSource mAudioSourceBGM;
    public AudioClip mAudioClipBGM;

    public AudioSource mAudioSourceSFX;
    public AudioClip mAudioClipSFX;

    [HideInInspector]
    public static UILabel labelGold;
    [HideInInspector]
    public static UILabel labelDia;

    public bool ACTION_PROCESS = false;
    // [HideInInspector]
    public FadeInOut mFadeInOut;
    // [HideInInspector]
    // public IOSSafeArea safeArea;
    // [HideInInspector]
    public JHDataReader dataReader;
    public JHDataManager dataManager;
    // [HideInInspector]
    public JHAdsManager adsManager;
    public JHIAPManager iapManager;

    public PopupBuyInAppItemConfirm_Base inappErrorPopup;
    public GameObject objRootInappError;


    // public NavigationController mNavigationController;
    // [HideInInspector]


    // public UICamera mainCamera;
    [Header("Main UI Camera")]
    public Camera mainCamera;


    // public bool bDebugTest = false;
    protected bool isActionToast = false;
    public bool GetIsActionToast
    {
        get
        {
            return isActionToast;
        }
    }

    /***************************************************************************************/
    public EventDelegate.Callback mFadeInOutCallback;
    public EventDelegate.Callback mFadeInOutCallback_Two;
    const float FADE_DURATION = 0.25f;

    public delegate void Notification();
    public static event Notification RefreshNotification;
    /***************************************************************************************/

    public static object INAPP_SELECT_SKILL_DATA = null;

    ///싱글톤 객체
    private static SceneBase _instance = null;

    public static SceneBase Instance
    {
        ///중복 호출 방지
        // [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            if (_instance == null)
            {
                ///싱글톤 객체를 찾아서 넣는다.
                _instance = (SceneBase)FindObjectOfType(typeof(SceneBase));

                ///없다면 생성한다.
                if (_instance == null)
                {
                    string goName = typeof(SceneBase).ToString();
                    GameObject go = GameObject.Find(goName);
                    if (go == null)
                    {
                        go = new GameObject();
                        go.name = goName;
                    }
                    _instance = go.AddComponent<SceneBase>();
                }
            }
            return _instance;
        }
    }

    public string SYSTEM_LANGUGE = "";


    // public AdMobManager adManger;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

        DontDestroyOnLoad(this.gameObject);

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        // safeArea = new IOSSafeArea();
        // dataReader = new JHDataReader();

        switch (Application.systemLanguage)
        {
            case SystemLanguage.Korean: { SYSTEM_LANGUGE = "kr"; break; }
            case SystemLanguage.English: { SYSTEM_LANGUGE = "en"; break; }
            case SystemLanguage.Thai: { SYSTEM_LANGUGE = "tw"; break; }
        }

        // SetAIPData();
        // JHIAPManager.Instance.InitalizeIAP(_inappList, InappCheck);

        JHIAPManager.Instance.ACTION_HASCONSUME_PRODUCT = ACTION_HASCONSUME_PRODUCT;

        // Stack aaa = new Stack();
        // aaa.Peek();

        // Queue _queue = new Queue();
        // _queue.Enqueue(1);

        
        // SortedList sortedList = new SortedList();
        // int bbb = sortedList.Capacity;
        // Camera.main.
        // float height = mainCamera.orthographicSize;
        // float heightX2 = 2 * mainCamera.orthographicSize;
        // float width = heightX2 * mainCamera.aspect;

    }
    ///<summary>
    /// 인앱 초기화
    /// </summary>
    void SetAIPData()
    {
        Dictionary<Enums.eShopPurchaseKey, STShopData> _dic = dataManager.mGetDicShopData();

        List<string> _listInappAOSKey = new List<string>();

        for (int i = 0; i < (int)Enums.eShopPurchaseKey.COUNT; i++)
        {
            STShopData _data = _dic[(Enums.eShopPurchaseKey)i];
            string _strInappAOS = _data.AOS_INAPP_KEY;

            if (string.IsNullOrEmpty(_strInappAOS) == false && _strInappAOS.Equals("NONE") == false)
            {
                _listInappAOSKey.Add(_strInappAOS);
            }

        }

        List<string> _listInappIOSKey = new List<string>();
        for (int i = 0; i < (int)Enums.eShopPurchaseKey.COUNT; i++)
        {
            STShopData _data = _dic[(Enums.eShopPurchaseKey)i];
            string _strInappIOS = _data.IOS_INAPP_KEY;

            if (string.IsNullOrEmpty(_strInappIOS) == false && _strInappIOS.Equals("NONE") == false)
            {
                _listInappIOSKey.Add(_strInappIOS);
            }

        }

#if UNITY_ANDROID
        iapManager.InitalizeIAP(_listInappAOSKey, AIPCallbackInAppCheck);
#elif UNITY_IOS
        aipManager.InitalizeIAP(_listInappIOSKey, AIPCallbackInAppCheck);
#endif
    }
    bool isInApp = false;
    void AIPCallbackInAppCheck()
    {
        isInApp = true;
        // Debug.LogError("################### AIPCallbackInAppCheck : " + isInApp);
    }

    void GetADID()
    {
        Application.RequestAdvertisingIdentifierAsync(AdvertiveCallback);
    }

    void AdvertiveCallback(string id, bool check, string error)
    {
        Debug.LogError("Get IDFA => id : " + id + " // check : " + check + "// error : " + error);

        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                {
                    Debug.LogError("Get ADID => id : " + id + " // check : " + check + "// error : " + error);
                    break;
                }

            case RuntimePlatform.IPhonePlayer:
                {
                    Debug.LogError("Get IDFA => id : " + id + " // check : " + check + "// error : " + error);
                    break;
                }

            default:
                {
                    Debug.LogError("Get ADID => id : " + id + " // check : " + check + "// error : " + error);
                    break;
                }
        }
    }

    void ACTION_HASCONSUME_PRODUCT(Enums.eShopPurchaseKey inappKey, bool check)
    {
        if (check == true)
        {
            PopupBuyInAppItemConfirm_Base _poupup = Instantiate(inappErrorPopup, objRootInappError.transform) as PopupBuyInAppItemConfirm_Base;
            _poupup.transform.localPosition = Vector2.zero;
            _poupup.transform.localScale = Vector3.one;
            _poupup.SetPopupBuyInAppItemConfirm_Base(inappKey);

        }
    }

    // Use this for initialization 
    IEnumerator Start()
    {
        // GetADID();

        JHDataManager.Instance.initRelease();
        JHDataManager.Instance.initData();
        JHDataManager.Instance.LoadTextData();
        yield return new WaitForSeconds(0.5f);
        int count = 0;
        JHSceneManager.Instance.onFadeIn = fadeIn;
        JHSceneManager.Instance.onFadeOut = fadeOut;
        SetAIPData();
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            if (isInApp == true)
            {
                break;
            }

            if (count > 10)
            {
                break;
            }
            count++;
        }
        yield return YieldHelper.waitForSeconds(1000);

        Debug.LogError("################### AIPCallbackInAppCheck : " + isInApp);
        JHSceneManager.Instance.Action(JHSceneManager.ACTION.ACTION_REPLACE, Strings.SCENE_TITLE);
        Debug.LogError("UserInfoManager.Instance.GetWeeklyCount() : " + UserInfoManager.Instance.GetWeeklyCount());
        // Debug.LogError("############# String gold : " +100000.ToString("N0") );

    }

    public static void RefrashGoldDia()
    {
        if (labelGold != null)
        {
            int _gold = int.Parse(UserInfoManager.Instance.GetGold());
            labelGold.text = string.Format("{0}", _gold.ToString("N0"));
            // Debug.LogError("############# labelGold.text : " + labelGold.text);
        }

        if (labelDia != null)
        {
            int _dia = int.Parse(UserInfoManager.Instance.GetDia());
            labelDia.text = string.Format("{0}", _dia.ToString("N0"));
            // Debug.LogError("############# labelDia.text : " + labelDia.text);
        }
    }

    public IEnumerator fadeIn()
    {
        //FadeIn 효과
        mFadeInOut.gameObject.SetActive(true);
        TweenAlpha.Begin(mFadeInOut.gameObject, FADE_DURATION, 1f);
        yield return new WaitForSeconds(FADE_DURATION);
    }

    public IEnumerator fadeOut()
    {
        //FadeOut 효과
        TweenAlpha.Begin(mFadeInOut.gameObject, FADE_DURATION, 0f);
        yield return new WaitForSeconds(FADE_DURATION);

        mFadeInOut.gameObject.SetActive(false);
    }

    //페이드 in/out시키는 코르틴..
    public IEnumerator FadeInOutCoroutine()
    {

        float duration = 1f;
        mFadeInOut.gameObject.SetActive(true);

        //FadeIn 
        TweenAlpha.Begin(mFadeInOut.gameObject, duration, 1f);
        yield return new WaitForSeconds(duration);
        mFadeInOutCallback();

        //FadeOut 
        TweenAlpha.Begin(mFadeInOut.gameObject, duration, 0f);
        yield return new WaitForSeconds(duration);

        mFadeInOut.gameObject.SetActive(false);

    }
    /// <summary>
    /// 오버라이드 해서 사용 
    /// fadeout :  까만거 나타나는 속도 , outTime :  데이터 처리 
    /// fadein : 까만거 사라지는 속도 , startTime :  mFadeInOut.SetActive (false); 속도
    /// mFadeInOutCallback : 첫번째 콜백 
    /// mFadeInOutCallback_Two : 두번쨰 콜백 
    /// </summary>
    public IEnumerator FadeInOutCoroutine_Two(float fadeout, float outTime, float fadein, float startTime)
    {
        mFadeInOut.gameObject.SetActive(true);
        //FadeIn 
        TweenAlpha.Begin(mFadeInOut.gameObject, fadeout, 1f);
        yield return new WaitForSeconds(outTime);
        TweenAlpha.Begin(mFadeInOut.gameObject, fadein, 0f);
        mFadeInOutCallback();
        //FadeOut 
        yield return new WaitForSeconds(startTime);
        mFadeInOut.gameObject.SetActive(false);
        mFadeInOutCallback_Two();
    }

    public bool GetActionPrecess()
    {
        return ACTION_PROCESS;
    }

    public void PLAY_BGM(Enums.eBGM bgm)
    {
        if (UserInfoManager.Instance.GetSaveBGM() == true)
        {
            // AudioController.PlayMusic(bgm.ToString());
            string _path = string.Format("Sound/BGM/{0}", bgm.ToString());
            mAudioSourceBGM.clip = Resources.Load<AudioClip>(_path);
            mAudioSourceBGM.Stop();
            mAudioSourceBGM.volume = 1;
            mAudioSourceBGM.loop = true;
            mAudioSourceBGM.Play();

        }
        else
        {
            mAudioSourceBGM.Stop();
            mAudioSourceBGM.volume = 0;
        }

    }

    public void PLAY_SFX(Enums.eSFX se)
    {
        if (UserInfoManager.Instance.GetSaveSFX() == true)
        {
            AudioController.PlayMusic(se.ToString());
            AudioController.MuteSound(false);
        }
        else
        {
            AudioController.MuteSound(true);
        }
    }

    public void AddPopup(Transform _root, Enums.ePopupLayer layer)
    {
        JHPopup data = Instantiate(Resources.Load<JHPopup>(string.Format("Poupup/{0}", layer)), _root) as JHPopup;
        data.gameObject.transform.localScale = Vector3.one;
        data.gameObject.transform.localPosition = Vector2.zero;
        data.SetData();
        AnimationUtil.PopupAlphaIn(data.gameObject, null, 0.3f);

    }

    public void AddPopupInApp(Transform _root, Enums.ePopupLayer layer, object _data)
    {
        JHPopup data = Instantiate(Resources.Load<JHPopup>(string.Format("Poupup/{0}", layer)), _root) as JHPopup;
        data.gameObject.transform.localScale = Vector3.one;
        data.gameObject.transform.localPosition = Vector2.zero;
        if (_data != null)
        {
            data.SetData(_data);
        }
        AnimationUtil.PopupAlphaIn(data.gameObject, null, 0.3f);
    }

    public void AddPopup(Transform _root, Enums.ePopupLayer layer, object _data)
    {
        JHPopup data = Instantiate(Resources.Load<JHPopup>(string.Format("Poupup/{0}", layer)), _root) as JHPopup;
        data.gameObject.transform.localScale = Vector3.one;
        data.gameObject.transform.localPosition = Vector2.zero;
        if (_data != null)
        {
            data.SetData(_data);
        }
        AnimationUtil.PopupAlphaIn(data.gameObject, null, 0.3f);

    }


    public void AddPopup(Transform _root, Enums.ePopupLayer layer, object[] _data)
    {
        JHPopup data = Instantiate(Resources.Load<JHPopup>(string.Format("Poupup/{0}", layer)), _root) as JHPopup;
        data.gameObject.transform.localScale = Vector3.one;
        data.gameObject.transform.localPosition = Vector2.zero;
        if (_data != null)
        {
            data.SetData(_data);
        }
        AnimationUtil.PopupAlphaIn(data.gameObject, null, 0.3f);

    }


    public void AddPopup(Transform _root, Enums.ePopupLayer layer, List<object> _data)
    {
        JHPopup data = Instantiate(Resources.Load<JHPopup>(string.Format("Poupup/{0}", layer)), _root) as JHPopup;
        data.gameObject.transform.localScale = Vector3.one;
        data.gameObject.transform.localPosition = Vector2.zero;
        if (_data != null)
        {
            data.SetData(_data);
        }
        AnimationUtil.PopupAlphaIn(data.gameObject, null, 0.3f);

    }

    public void AddTestTextPopup(string text)
    {
        Transform _root = PopupManager.Instance.RootPopup;

        JHPopup data = Instantiate(Resources.Load<JHPopup>(string.Format("Poupup/{0}", "PopupTestSkillApply")), _root) as JHPopup;
        data.gameObject.transform.localScale = Vector3.one;
        data.gameObject.transform.localPosition = Vector2.zero;
        if (string.IsNullOrEmpty(text) == false)
        {
            data.SetData(text);
        }
        AnimationUtil.PopupAlphaIn(data.gameObject, null, 0.3f);
    }

    public void AddEmptyPurchasePopup(string text)
    {
        Transform _root = PopupManager.Instance.RootPopup;

        JHPopup data = Instantiate(Resources.Load<JHPopup>(string.Format("Poupup/{0}", "PopupTestSkillApply")), _root) as JHPopup;
        data.gameObject.transform.localScale = Vector3.one;
        data.gameObject.transform.localPosition = Vector2.zero;
        if (string.IsNullOrEmpty(text) == false)
        {
            data.SetData(text);
        }
        AnimationUtil.PopupAlphaIn(data.gameObject, null, 0.3f);
    }


    public void ClosePopup(Transform root)
    {
        if (root.childCount > 0)
        {
            Destroy(root.GetChild(Mathf.Max(0, root.childCount - 1)).gameObject);
        }
    }

}
