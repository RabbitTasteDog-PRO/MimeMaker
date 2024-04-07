using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System.Linq;
using System.Linq.Expressions;




public class PopupStore_Money : MonoBehaviour
{

    public UILabel labelVideoCount;
    public UIButton btnVideo;
    public UILabel labelBtnVideo;

    public GameObject btnVideoOff;
    public UILabel labelVideoTimeCheck;



    public GameObject[] objInapp;
    public GameObject[] objGold;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        JHIAPManager.Instance.ACTION_BUY_MONEY = ACTION_BUY_MONEY;

        btnVideo.onClick.Add(new EventDelegate(OnClickVideo));
        SetMoneyUI();
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        JHIAPManager.Instance.ACTION_BUY_MONEY = null;
    }

    public void SetMoneyUI()
    {
        int diaIndex = 0;
        for (int i = (int)eShopPurchaseKey.eStoreDia_0; i < (int)eShopPurchaseKey.eStoreDia_3 + 1; i++)
        {
            SetInAppUI(objInapp[diaIndex], i);
            diaIndex++;
        }

        int goldIndex = 0;
        for (int i = (int)eShopPurchaseKey.eStoreGold_0; i < (int)eShopPurchaseKey.eStoreGold_3 + 1; i++)
        {
            SetGoldUI(objGold[goldIndex], i);
            goldIndex++;
        }

        if (string.IsNullOrEmpty(UserInfoManager.Instance.GetFreeVideoTimeClac()) == true)
        {
            btnVideo.gameObject.SetActive(true);
            btnVideoOff.SetActive(false);
            labelVideoTimeCheck.gameObject.SetActive(false);
        }
        else
        {
            btnVideo.gameObject.SetActive(false);
            btnVideoOff.SetActive(true);
            labelVideoTimeCheck.gameObject.SetActive(true);
        }

        labelBtnVideo.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eStoreAdsDescBtnText);
        string videCount = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eStoreAdsDesc);
        labelVideoCount.text = string.Format(videCount, UserInfoManager.Instance.GetFreeVideoCount());
    }



    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        JHAdsManager.ACTION_ADS_COMPLETE = SucessAdMob;

        string videCount = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eStoreAdsDesc);
        labelVideoCount.text = string.Format(videCount, UserInfoManager.Instance.GetFreeVideoCount());

        StartCoroutine(IEFreeVideoTimeCount());

    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        JHAdsManager.ACTION_ADS_COMPLETE = null;

        StopCoroutine(IEFreeVideoTimeCount());

        if (string.IsNullOrEmpty(UserInfoManager.Instance.GetFreeVideoTimeClac()) == true)
        {
            StopCoroutine(IEFreeVideoTimeCount());
            btnVideo.gameObject.SetActive(true);
            btnVideoOff.gameObject.SetActive(false);
            labelVideoTimeCheck.gameObject.SetActive(false);
        }
    }

    void SetInAppUI(GameObject obj, int objIndex)
    {
        STShopData _data = SceneBase.Instance.dataManager.mGetSTShopData((eShopPurchaseKey)objIndex);

        UIButton btnBuy = obj.transform.Find("btnBuy").gameObject.GetComponent<UIButton>();

        UILabel labelAmount = obj.transform.Find("Light").transform.Find("labelAmount").gameObject.GetComponent<UILabel>();
        UILabel labelPrice = btnBuy.transform.Find("labelPrice").gameObject.GetComponent<UILabel>(); ;
        if (_data.fakePrice != -1)
        {
            UILabel labelFakePrice = btnBuy.transform.Find("labelPrice_Fake").gameObject.GetComponent<UILabel>();
            labelFakePrice.gameObject.SetActive(true);
        }

        UISprite itemImg = obj.transform.Find("Light").transform.Find("Sprite").gameObject.GetComponent<UISprite>();


        labelAmount.text = string.Format("X {0:N0}", _data.amount);
        labelPrice.text = string.Format("₩ {0:N0}", _data.price);


        itemImg.spriteName = _data.image;


        btnBuy.onClick.Add(new EventDelegate(() => OnClickInapp((eShopPurchaseKey)objIndex)));
    }


    void SetGoldUI(GameObject obj, int objIndex)
    {
        STShopData _data = SceneBase.Instance.dataManager.mGetSTShopData((eShopPurchaseKey)objIndex);

        UIButton btnBuy = obj.transform.Find("btnBuy").gameObject.GetComponent<UIButton>();

        UILabel labelAmount = obj.transform.Find("Light").transform.Find("labelAmount").gameObject.GetComponent<UILabel>();
        UILabel labelPrice = btnBuy.transform.Find("labelPrice").gameObject.GetComponent<UILabel>(); ;
        if (_data.fakePrice != -1)
        {
            UILabel labelFakePrice = btnBuy.transform.Find("labelPrice_Fake").gameObject.GetComponent<UILabel>();
            labelFakePrice.gameObject.SetActive(true);
        }

        UISprite itemImg = obj.transform.Find("Light").transform.Find("Sprite").gameObject.GetComponent<UISprite>();


        labelAmount.text = string.Format("X {0:N0}", _data.amount);
        labelPrice.text = string.Format("x {0:N0}", _data.price);


        itemImg.spriteName = _data.image;


        btnBuy.onClick.Add(new EventDelegate(() => OnClickGold((eShopPurchaseKey)objIndex)));

    }

    ///<summary>
    /// 인앱 다이아 구매
    ///</summary>
    void OnClickInapp(eShopPurchaseKey _key)
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        Debug.LogError("##################### OnClickInapp : " + _key);
        if (isActionBuy == true)
        {
            return;
        }
        isActionBuy = true;
        Invoke("InvokeIsActionBuy", 0.5f);
        STShopData _data = SceneBase.Instance.dataManager.mGetSTShopData(_key);
        string inappKey = "";
#if UNITY_ANDROID
        inappKey = _data.AOS_INAPP_KEY;
#elif UNITY_IOS
        inappKey = _data.IOS_INAPP_KEY;
#endif

        if (PopupManager.Instance != null)
        {
            PopupManager.Instance.objNoneTouchBlock.SetActive(true);
        }
        SceneBase.Instance.iapManager.BuyProduct(inappKey, ReFrashGoldAndDia);


    }



    bool isActionBuy = false;
    ///<summary>
    /// 골드구매 
    ///</summary>
    void OnClickGold(eShopPurchaseKey _key)
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        // Debug.LogError("##################### eShopPurchaseKey : " + _key);
        if (isActionBuy == true)
        {
            return;
        }
        isActionBuy = true;
        Invoke("InvokeIsActionBuy", 0.5f);

        STShopData _data = SceneBase.Instance.dataManager.mGetSTShopData(_key);

        int dia = int.Parse(UserInfoManager.Instance.GetDia());

        if (dia < _data.price)
        {
            SceneBase.Instance.AddTestTextPopup("다이아가 부족합니다");
        }
        else
        {
            UserInfoManager.Instance.SetDia((dia - _data.price).ToString());
            int plus = int.Parse(UserInfoManager.Instance.GetGold()) + _data.amount;
            UserInfoManager.Instance.SetGold(plus.ToString());

            Transform _root = PopupManager.Instance.RootPopup; ;
            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupBuyInAppItemConfirm, _key);
            // SceneBase.Instance.AddTestTextPopup(string.Format("{0:0N}의 골드를 구매했습니다.", _data.amount));

        }

        SceneBase.RefrashGoldDia();
    }

    ///<summary>
    /// 비디오광고 
    ///</summary>
    void OnClickVideo()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);
        
        if (UserInfoManager.Instance.GetFreeVideoCount() >= 10)
        {
            string _msg = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.TextFreeVideoNotice);
            SceneBase.Instance.AddTestTextPopup(_msg);

            return;
        }
        else
        {
            if (isActionVideoReward == true)
            {
                return;
            }
            isActionVideoReward = true;
            SceneBase.Instance.adsManager.AsyncShowAdMobVideo(eVideoAds.eVideoFree);
        }
    }

    void RefrashVideo()
    {
        int crrCount = UserInfoManager.Instance.GetFreeVideoCount();
        UserInfoManager.Instance.SetFreeVideoCount(crrCount + 1);

        string videCount = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eStoreAdsDesc);
        string strVideoCnt = string.Format(videCount, UserInfoManager.Instance.GetFreeVideoCount());
        labelVideoCount.text = string.Format(videCount, UserInfoManager.Instance.GetFreeVideoCount());

        int randomDia = Random.Range(1, 3);
        int crrDia = int.Parse(UserInfoManager.Instance.GetDia());
        UserInfoManager.Instance.SetDia((crrDia + randomDia).ToString());


        string _crrTime = System.DateTime.Now.ToString("yyyyMMddHHmmss");
        UserInfoManager.Instance.SetFreeVideoTimeCalc(_crrTime);

        SceneBase.RefrashGoldDia();

        StartCoroutine(IEFreeVideoTimeCount());
    }
    IEnumerator IEFreeVideoTimeCount()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_28_item);

        yield return null;
#if !UNITY_EDITOR
        string _strSaveTime = UserInfoManager.Instance.GetFreeVideoTimeClac();

        if (string.IsNullOrEmpty(_strSaveTime) == false)
        {
            btnVideo.gameObject.SetActive(false);
            btnVideoOff.SetActive(true);
            labelVideoTimeCheck.gameObject.SetActive(true);

            if (string.IsNullOrEmpty(UserInfoManager.Instance.GetFreeVideoTimeClac()) == true)
            {
                UserInfoManager.Instance.SetFreeVideoTimeCalc(System.DateTime.Now.ToString("yyyyMMddHHmmss"));
            }
            System.DateTime _saveTime = System.DateTime.ParseExact(_strSaveTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
            Debug.LogError("_saveTime : " + _saveTime.ToString());
            while (true)
            {
                string _strCrrTime = System.DateTime.Now.ToString("yyyyMMddHHmmss");
                System.DateTime _crrTime = System.DateTime.ParseExact(_strCrrTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                System.TimeSpan totlaTime = _crrTime - _saveTime;

                double check = 600 - totlaTime.TotalSeconds;
                if (check <= 0)
                {
                    //// UI 변경
                    UserInfoManager.Instance.SetFreeVideoTimeCalc("");
                    btnVideo.gameObject.SetActive(true);
                    btnVideoOff.SetActive(false);
                    labelVideoTimeCheck.gameObject.SetActive(false);

                    StopCoroutine(IEFreeVideoTimeCount());
                    break;
                }

                System.TimeSpan reverTime = System.TimeSpan.FromSeconds(check);
                labelVideoTimeCheck.text = string.Format("{0:00} : {1:00}", reverTime.Minutes, reverTime.Seconds);
                yield return new WaitForSeconds(1.0f);

            }

        }
#endif

    }


    void InvokeIsActionBuy()
    {
        isActionBuy = false;
    }


    //// 광고 시청시 매니저 스킬 변경
    bool isActionVideoReward = false;
    ///<summary>
    /// 광고 성공 시 보냄
    ///</summary>
    void SucessAdMob(bool flag)
    {
        if (flag == true)
        {
            isActionVideoReward = false;
            RefrashVideo();
        }
    }


    void ReFrashGoldAndDia(Enums.eInAppActionCoce A, int B)
    {
        //// 실패 성공 나눠야함
        if (PopupManager.Instance != null)
        {
            PopupManager.Instance.objNoneTouchBlock.SetActive(false);
        }
        SceneBase.RefrashGoldDia();
    }

    void ACTION_BUY_MONEY(eShopPurchaseKey _key)
    {
        STShopData _data = SceneBase.Instance.dataManager.mGetSTShopData(_key);

        if (PopupManager.Instance != null)
        {
            PopupManager.Instance.objNoneTouchBlock.SetActive(false);
        }


        Transform _root = PopupManager.Instance.RootPopup;
        switch (_key)
        {
            case eShopPurchaseKey.eStoreDia_0:
            case eShopPurchaseKey.eStoreDia_1:
            case eShopPurchaseKey.eStoreDia_2:
            case eShopPurchaseKey.eStoreDia_3:
                {
                    SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupBuyInAppItemConfirm, _key);
                    break;
                }
            default : 
            {
                SceneBase.Instance.PLAY_SFX(eSFX.SFX_28_item);
                break;
            }

        }
        SceneBase.RefrashGoldDia();

    }

}
