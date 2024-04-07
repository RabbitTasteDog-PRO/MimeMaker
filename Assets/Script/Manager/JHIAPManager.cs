

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;
using Enums;


public class JHIAPManager : Ray_Singleton<JHIAPManager>, IStoreListener
{
    public Action<eShopPurchaseKey> ACTION_BUY_MONEY;
    public Action<eShopPurchaseKey> ACTION_BUY_ITEM;
    public Action<eShopPurchaseKey, object[]> ACTION_BUY_ENDING;
    public Action<string, STManagerSkill, bool> ACTION_BUY_SKILL;
    public Action<eShopPurchaseKey, bool> ACTION_HASCONSUME_PRODUCT;  //// 결제 실패시 컨슘받아옴


    private IStoreController storeController;
    private IExtensionProvider storeExtension;
    private List<string> list_productID;
    private bool isInitializeEnd = false;   // 초기화 완료 여부( Consume 안된 아이템의 Consume이 발생하기 때문에 체크 )
    private bool isPurchasing = false;      // 구매중인지

    public bool isInitalized { get { return storeController != null && storeExtension != null; } }
    public Action initalizeCallBack;        // 초기화 완료 콜백
    public Action<eInAppActionCoce, int> purchaseEndCallBack;      // <상품결과, 에러결과>구매 완료 콜백





    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
    }

    /// <summary>
    /// IAP 매니저 최초 초기화( 상품 등록 및 빌더 초기화 )
    /// </summary>
    /// <param name="_listPID">상품 id string</param>
    /// <param name="_callBack">초기화 완료 콜백</param>
    public void InitalizeIAP(List<string> _listPID, Action _callBack)
    {
        isPurchasing = false;
        initalizeCallBack = null;
        purchaseEndCallBack = null;

        initalizeCallBack = _callBack;

        if (isInitalized == false)
        {


            // // #if UNITY_ANDROID || UNITY_EDITOR
            list_productID = new List<string>();
            // //             builder.AddProduct(Strings.INAPP_1000, ProductType.Consumable, new IDs
            // //                     {
            // //                         { Strings.INAPP_1000, GooglePlay.Name }
            // //                     });
            // //             builder.AddProduct(Strings.INAPP_3000, ProductType.Consumable, new IDs
            // //                     {
            // //                         { Strings.INAPP_3000, GooglePlay.Name }
            // //                     });
            // // #elif UNITY_IPHONE
            // //         builder.AddProduct(Strings.IOS_INAPP_1000, ProductType.Consumable);
            // //         builder.AddProduct(Strings.IOS_INAPP_3000, ProductType.Consumable);
            // // #endif

            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            for (int i = 0; i < _listPID.Count; i++)
            {
                list_productID.Add(_listPID[i]);
                builder.AddProduct(_listPID[i], ProductType.Consumable, new IDs
                    {
                        #if UNITY_ANDROID
                        { _listPID[i], GooglePlay.Name }
                        #elif UNITY_IPHONE
                        { _listPID[i], AppleAppStore.Name }
                        #endif
                    });

                Debug.LogError("InitalizeIAP : _listPID[ " + i + "] : " + _listPID[i]);
            }

            UnityPurchasing.Initialize(this, builder);
        }

        Debug.LogError("################### Check IAP : " + isInitalized);
    }

    /// <summary>
    /// Consume이 필요한 Product가 있는지 체크. Consume이 다 될때까지 기다리기 위함
    /// </summary>
    public bool HasConsumeProduct()
    {
        if (isInitalized == true)
        {
            for (int i = 0; i < list_productID.Count; i++)
            {
                Product pd = storeController.products.WithID(list_productID[i]);
                Debug.LogError("################ HasConsumeProduct : " + pd.hasReceipt + "//  " + i + " : " + list_productID[i]);
                if (pd.hasReceipt == true)
                {
                    if (ACTION_HASCONSUME_PRODUCT != null && ACTION_HASCONSUME_PRODUCT.GetInvocationList().Length > 0)
                    {
                        eShopPurchaseKey _key = CoverShopKey(list_productID[i]);
                        ACTION_HASCONSUME_PRODUCT(_key, pd.hasReceipt);
                    }

                    return true;
                }
            }
        }

        return false;
    }

    eInAppActionCoce eInappAction;
    bool isError = false;
    /// <summary>
    /// 상품 구매
    /// </summary>
    public void BuyProduct(string _pID, Action<eInAppActionCoce, int> _buyCallBack)
    {

        purchaseEndCallBack = _buyCallBack;

        eInappAction = eInAppActionCoce.NONE;
        // 초기화 상태 여부
        if (isInitalized == false)
        {
            eInappAction = eInAppActionCoce.ERR_INITALIZED;
            Debug.LogError("Cannot Buy Product [ Initalize : " + isInitalized + " ]");
            isError = true;
        }

        // 구매처리중인지 여부
        if (isPurchasing == true)
        {
            eInappAction = eInAppActionCoce.ERR_PURCHASING;
            Debug.LogError("Cannot Buy Product [ Purchasing ]");
            isError = true;
        }

        // 상품 보유 여부
        if (storeController.products.WithID(_pID) == null)
        {
            eInappAction = eInAppActionCoce.ERR_PRODUCT;
            Debug.LogError("Cannot Buy Product [ Product is Null : " + _pID + " ]");
            isError = true;
        }

        if (isError == true)
        {
            // 에러일 경우 CallBack 호출
            // TODO : 에러코드 필요
            purchaseEndCallBack(eInappAction, ErrCode);

        }
        else
        {
            eInappAction = eInAppActionCoce.COMPLETE;
            isPurchasing = true;
            storeController.InitiatePurchase(_pID);
        }
    }

    /// <summary>
    /// 초기화 완료 콜백
    /// </summary>
    private void InitializeCallBack()
    {
        if (initalizeCallBack != null)
        {
            initalizeCallBack();
        }
    }

    /// <summary>
    /// 구매 완료 CallBack
    /// </summary>
    private void PurchaseCallBack()
    {
        if (purchaseEndCallBack != null)
        {
            purchaseEndCallBack(eInappAction, (int)ErrCode);
            ErrCode = -1;
        }
    }


    /// <summary>
    /// 서버로 부터 지급완료 받음
    /// </summary>
    private void BuyItemCallBack()
    {
        // 추가로 Consume해야 하는 아이템이 없다면 콜백
        if (HasConsumeProduct() == false)
        {
            // 초기화 중에 Consume으로 인해서 들어왔다면 초기화 콜백 호출
            // 일반 구매였을 경우 구매 콜백 호출
            if (isInitializeEnd == false)
            {
                isInitializeEnd = true;
                InitializeCallBack();

                Debug.LogError("############################ BuyItemCallBack InitializeCallBack");
            }
            else
            {
                PurchaseCallBack();
                Debug.LogError("############################ BuyItemCallBack PurchaseCallBack");
            }
        }
    }

    #region IStoreListener 인터페이스 함수
    /// <summary>
    /// UnityPurchasing.Initialize 성공 Call Back
    /// </summary>
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        storeExtension = extensions;

        // 해당 콜백 후 Consume 안된 아이템의 Consume처리( ProcessPurchase 자동호출 )가 되어서 체크 필요
        // Consume할 아이템이 없다면 
        if (HasConsumeProduct() == false)
        {
            Debug.LogError("############################ OnInitialized");
            // 초기화 완료 CallBack
            InitializeCallBack();
        }
    }

    /// <summary>
    /// UnityPurchasing.Initialize 실패 Call Back
    /// </summary>
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // TODO : 초기화 에러시 처리 필요
        // 1. title로 돌아가기
        // 2. 그냥 게임에 접속( IAP 못함 )
        Debug.LogError("OnInitializeFailed : " + error);
    }

    /// <summary>
    /// Purchase 성공 Call Back
    /// </summary>
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        string purchasedItem = e.purchasedProduct.definition.id;
        Debug.LogError("###################### PurchaseProcessingResult purchasedItem : " + purchasedItem);
        eShopPurchaseKey _changeKey = CoverShopKey(purchasedItem);
        Debug.LogError("###################### PurchaseProcessingResult _changeKey : " + _changeKey);
        switch (_changeKey)
        {
            //// 다이아 10
            case eShopPurchaseKey.eStoreDia_0:
            //// 다이아 30
            case eShopPurchaseKey.eStoreDia_1:
            //// 다이아 50
            case eShopPurchaseKey.eStoreDia_2:
            //// 다이아 100
            case eShopPurchaseKey.eStoreDia_3:
                {
                    // eShopPurchaseKey _key = RayUtils.Utils.ConvertEnumData<eShopPurchaseKey>(purchasedItem);
                    STShopData _data = SceneBase.Instance.dataManager.mGetSTShopData(_changeKey);
                    int getAmount = _data.amount;
                    int crrAmount = int.Parse(UserInfoManager.Instance.GetDia());
                    UserInfoManager.Instance.SetDia((getAmount + crrAmount).ToString());
                    if (ACTION_BUY_MONEY != null && ACTION_BUY_MONEY.GetInvocationList().Length > 0)
                    {
                        ACTION_BUY_MONEY(_changeKey);
                    }
                    SceneBase.RefrashGoldDia();
                    break;
                }
            //// 올인원팩
            case eShopPurchaseKey.eStorePrimium_0:
                {
                    UserInfoManager.Instance.SetSaveInAppPrimiumItem(eShopPurchaseKey.eStorePrimium_0.ToString(), true);
                    UserInfoManager.Instance.SetSaveInAppPrimiumItem(eShopPurchaseKey.eStorePrimium_1.ToString(), true);
                    UserInfoManager.Instance.SetSaveInAppPrimiumItem(eShopPurchaseKey.eStorePrimium_2.ToString(), true);
                    UserInfoManager.Instance.SetSaveInAppPrimiumItem(eShopPurchaseKey.eStorePrimium_3.ToString(), true);
                    UserInfoManager.Instance.SetSaveInAppPrimiumItem(eShopPurchaseKey.eStorePrimium_4.ToString(), true);
                    if (ACTION_BUY_ITEM != null && ACTION_BUY_ITEM.GetInvocationList().Length > 0)
                    {
                        ACTION_BUY_ITEM(eShopPurchaseKey.eStorePrimium_0);
                    }
                    break;
                }
            //// 무한의 체력
            case eShopPurchaseKey.eStorePrimium_1:
            //// 바쁘다바빠
            case eShopPurchaseKey.eStorePrimium_2:
            //// 광고스킵
            case eShopPurchaseKey.eStorePrimium_3:
            //// 수익 2배
            case eShopPurchaseKey.eStorePrimium_4:
                {
                    UserInfoManager.Instance.SetSaveInAppPrimiumItem(_changeKey.ToString(), true);

                    if (ACTION_BUY_ITEM != null && ACTION_BUY_ITEM.GetInvocationList().Length > 0)
                    {
                        ACTION_BUY_ITEM(_changeKey);
                    }
                    break;
                }
            ///// 스타엔딩 구매 
            case eShopPurchaseKey.eStorePrimium_5:
                {
                    List<eEndingNumber> _list = new List<eEndingNumber>();
                    for (int i = 0; i < (int)eEndingNumber.COUNT; i++)
                    {
                        STEndingData _dataEnd = SceneBase.Instance.dataManager.GetSTEndingData((eEndingNumber)i);

                        if (_dataEnd.endingType.Equals("STAR"))
                        {
                            if (UserInfoManager.Instance.GetSaveEnding(((eEndingNumber)i).ToString()) == false)
                            {
                                _list.Add((eEndingNumber)i);
                            }
                        }
                    }
                    int _random = UnityEngine.Random.Range(0, (_list.Count - 1));

                    UserInfoManager.Instance.SetSaveEnding(((eEndingNumber)_random).ToString(), true);

                    Transform _root = PopupManager.Instance.RootPopup;
                    object[] _endData = new object[3];
                    _endData[0] = (eEndingNumber)_random;
                    _endData[1] = false;
                    _endData[2] = true;

                    if (ACTION_BUY_ENDING != null && ACTION_BUY_ENDING.GetInvocationList().Length > 0)
                    {
                        ACTION_BUY_ENDING(_changeKey, _endData);
                    }
                    break;
                }
            //// 노멀엔딩 구매
            case eShopPurchaseKey.eStorePrimium_6:
                {
                    List<eEndingNumber> _list = new List<eEndingNumber>();
                    for (int i = 0; i < (int)eEndingNumber.COUNT; i++)
                    {
                        STEndingData _dataEnd = SceneBase.Instance.dataManager.GetSTEndingData((eEndingNumber)i);

                        if (_dataEnd.endingType.Equals("NORMAL"))
                        {
                            if (UserInfoManager.Instance.GetSaveEnding(((eEndingNumber)i).ToString()) == false)
                            {
                                _list.Add((eEndingNumber)i);
                            }
                        }
                    }
                    int _random = UnityEngine.Random.Range(0, (_list.Count - 1));
                    UserInfoManager.Instance.SetSaveEnding(((eEndingNumber)_random).ToString(), true);
                    Transform _root = PopupManager.Instance.RootPopup;

                    object[] _endData = new object[3];
                    _endData[0] = (eEndingNumber)_random;
                    _endData[1] = false;
                    _endData[2] = true;

                    if (ACTION_BUY_ENDING != null && ACTION_BUY_ENDING.GetInvocationList().Length > 0)
                    {
                        ACTION_BUY_ENDING(_changeKey, _endData);
                    }
                    break;
                }
            case eShopPurchaseKey.eStorePrimium_7:
                {
                    if (ACTION_BUY_SKILL != null && ACTION_BUY_SKILL.GetInvocationList().Length > 0)
                    {
                        if (SceneBase.INAPP_SELECT_SKILL_DATA != null)
                        {
                            STManagerSkill _skillData = (STManagerSkill)SceneBase.INAPP_SELECT_SKILL_DATA;

                            UserInfoManager.Instance.SetSaveInappManagerSKill(_skillData.managerSkill.ToString(), true);
                            UserInfoManager.Instance.SetMySkill(_skillData.managerSkill.ToString());
                            UserInfoManager.Instance.SetMySkillGrade(eSkillGrade.S.ToString());

                            ACTION_BUY_SKILL("SELECT", _skillData, true);
                        }

                    }
                    break;
                }
            case eShopPurchaseKey.eStorePrimium_8:
                {
                    if (ACTION_BUY_SKILL != null && ACTION_BUY_SKILL.GetInvocationList().Length > 0)
                    {
                        List<eManager_Skill> _skill = new List<eManager_Skill>();
                        for (int i = 0; i < (int)eManager_Skill.COUNT; i++)
                        {
                            if ((eManager_Skill)i == eManager_Skill.SKILL_F)
                            {
                                continue;
                            }

                            if (UserInfoManager.Instance.GetSaveInappManagerSKill(((eManager_Skill)i).ToString()) == false)
                            {
                                _skill.Add((eManager_Skill)i);
                            }
                        }

                        eManager_Skill _random = (eManager_Skill)UnityEngine.Random.Range(0, _skill.Count - 1);
                        Dictionary<eManager_Skill, Dictionary<eSkillGrade, STManagerSkill>> _dic = SceneBase.Instance.dataManager.GetDicManagerSkillData();
                        Dictionary<eSkillGrade, STManagerSkill> _tempDic = _dic[_random];
                        STManagerSkill skillData = _tempDic[eSkillGrade.S];

                        UserInfoManager.Instance.SetMySkill(_random.ToString());
                        UserInfoManager.Instance.SetMySkillGrade(eSkillGrade.S.ToString());
                        //// 인앱으로산 스킬 저장
                        UserInfoManager.Instance.SetSaveInappManagerSKill(((eManager_Skill)_random).ToString(), true);

                        eManager_Skill _randomSkill = _random;
                        ACTION_BUY_SKILL("RANDOM", skillData, true);
                    }
                    break;
                }
        }

        isPurchasing = false;
        // PurchaseProcessingResult.Complete 리턴시 자동으로 Consume 처리
        // PurchaseProcessingResult.Pending 리턴시 Consume 대기( controller.InitiatePurchase를 호출해야만 Consume 처리 )
        return PurchaseProcessingResult.Complete;
    }


    int ErrCode = -1;
    /// <summary>
    /// Purchase 실패 Call BacK
    /// </summary>
    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        // TODO : 에러 콜백
        isPurchasing = false;
        // PurchaseCallBack();
        ErrCode = (int)p;
        Debug.LogError("OnPurchaseFailed : " + p);

    }

    #endregion IStoreListener 인터페이스 함수

    private bool m_IsGooglePlayStoreSelected;
    private IAppleExtensions m_AppleExtensions;
    private IGooglePlayStoreExtensions m_GooglePlayStoreExtensions;
    //// 구매 확인용 매소드
    public void RestoreButtonClick()
    {

        if (m_IsGooglePlayStoreSelected)
        {
            m_GooglePlayStoreExtensions.RestoreTransactions(OnTransactionsRestored);
        }
        else
        {
            m_AppleExtensions.RestoreTransactions(OnTransactionsRestored);
        }
    }
    // <summary>
    /// This will be called after a call to IAppleExtensions.RestoreTransactions().
    /// </summary>
    private void OnTransactionsRestored(bool success)
    {
        Debug.Log("Transactions restored." + success);
    }


    eShopPurchaseKey CoverShopKey(string key)
    {
        eShopPurchaseKey _key = eShopPurchaseKey.NONE;

        switch (key)
        {
            case "com.funnyeve.celeb_gem_10":
            case "com.funnyeve.celeb_iOS_Gem_10":
                { _key = eShopPurchaseKey.eStoreDia_0; break; }

            case "com.funnyeve.celeb_gem_30":
            case "com.funnyeve.celeb_iOS_Gem_30":
                { _key = eShopPurchaseKey.eStoreDia_1; break; }

            case "com.funnyeve.celeb_gem_50":
            case "com.funnyeve.celeb_iOS_Gem_50":
                { _key = eShopPurchaseKey.eStoreDia_2; break; }

            case "com.funnyeve.celeb_gem_100":
            case "com.funnyeve.celeb_iOS_Gem_100":
                { _key = eShopPurchaseKey.eStoreDia_3; break; }

            case "com.funnyeve.celeb_pack_all_in_one":
            case "com.funnyeve.celeb_iOS_pack_all_in_one":
                { _key = eShopPurchaseKey.eStorePrimium_0; break; }

            case "com.funnyeve.celeb_iOS_infinity_strength":
            case "com.funnyeve.celeb_infinity_strength":
                { _key = eShopPurchaseKey.eStorePrimium_1; break; }

            case "com.funnyeve.celeb_5x_speed":
            case "com.funnyeve.celeb_iOS_5x_speed":
                { _key = eShopPurchaseKey.eStorePrimium_2; break; }

            case "com.funnyeve.celeb_skip_ad":
            case "com.funnyeve.celeb_iOS_skip_ad":
                { _key = eShopPurchaseKey.eStorePrimium_3; break; }

            case "com.funnyeve.celeb_double_coin":
            case "com.funnyeve.celeb_iOS_double_coin":
                { _key = eShopPurchaseKey.eStorePrimium_4; break; }


            case "com.funnyeve.celeb_random_ending_star":
            case "com.funnyeve.celeb_iOS_random_ending_star":
                { _key = eShopPurchaseKey.eStorePrimium_5; break; }

            case "com.funnyeve.celeb_random_ending_normal":
            case "com.funnyeve.celeb_iOS_random_ending_Normal":
                { _key = eShopPurchaseKey.eStorePrimium_6; break; }

            case "com.funnyeve.celeb_manager_skill_pre":
            case "com.funnyeve.celeb_iOS_Manager_skill_pre":
                { _key = eShopPurchaseKey.eStorePrimium_7; break; }

            case "com.funnyeve.celeb_manager_skill_basic":
            case "com.funnyeve.celeb_iOS_Manager_skill_basic":
                { _key = eShopPurchaseKey.eStorePrimium_8; break; }
        }

        return _key;
    }


}
