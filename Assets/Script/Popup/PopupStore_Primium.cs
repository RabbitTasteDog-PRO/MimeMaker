using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System.Linq;
using System.Linq.Expressions;

public class PopupStore_Primium : MonoBehaviour
{

    ////  구매 완료 
    public GameObject[] objPrimium;

    public GameObject objPriminumSkillUpGrade;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        for (int i = 0; i < objPrimium.Length; i++)
        {
            SetInAppUI(objPrimium[i], i);
            RefrashUI(objPrimium[i], i);
        }

        PopupBuyEnding.ACTION_VIEW_ENDING = ACTION_VIEW_ENDING;
        /// 프리미엄 아이템 콜백
        JHIAPManager.Instance.ACTION_BUY_ITEM = ACTION_BUY_ITEM;
        JHIAPManager.Instance.ACTION_BUY_ENDING = ACTION_BUY_ENDING;

        JHIAPManager.Instance.ACTION_BUY_SKILL = ACTION_BUY_SKILL;
        PopupSkillUpGrade.ACTION_BUY_SKILL_UPGRADE = ACTION_BUY_SKILL;

    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        PopupBuyEnding.ACTION_VIEW_ENDING = null;
        JHIAPManager.Instance.ACTION_BUY_ITEM = null;
        JHIAPManager.Instance.ACTION_BUY_ENDING = null;

        JHIAPManager.Instance.ACTION_BUY_SKILL = null;
        PopupSkillUpGrade.ACTION_BUY_SKILL_UPGRADE = null;
    }

    void ACTION_BUY_SKILL(string type, STManagerSkill _skillData, bool flag)
    {
        if (flag == true)
        {
            Transform _root = PopupManager.Instance.RootPopup;
            object[] _data = new object[2];
            _data[0] = type;
            _data[1] = _skillData;
            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupSkillSBuyConfirm, _data);
            SetPrimiumUI();
        }
    }


    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        SetPrimiumUI();
    }


    public void SetPrimiumUI()
    {

        for (int i = 0; i < objPrimium.Length; i++)
        {
            SetInAppUI(objPrimium[i], i);
            if (i < 5)
            {
                RefrashUI(objPrimium[i], i);
            }
        }

        SetUIDiaSkillUpgrade();
    }


    void SetInAppUI(GameObject obj, int objIndex)
    {
        eShopPurchaseKey _key = RayUtils.Utils.ConvertEnumData<eShopPurchaseKey>(string.Format("eStorePrimium_{0}", objIndex));


        STShopData _data = SceneBase.Instance.dataManager.mGetSTShopData(_key);


        UISprite itemImg = obj.transform.Find("Light").transform.Find("Sprite").gameObject.GetComponent<UISprite>();
        UILabel labelTitle = obj.transform.Find("Light").transform.Find("labelFree").gameObject.GetComponent<UILabel>();
        UILabel labelDesc = obj.transform.Find("Light").transform.Find("labelFreeDesc").gameObject.GetComponent<UILabel>();

        UIButton btnBuy = obj.transform.Find("btnBuyOn").gameObject.GetComponent<UIButton>();

        UILabel labelPrice = btnBuy.transform.Find("labelPrice").gameObject.GetComponent<UILabel>();

        if (_data.fakePrice != -1)
        {
            UILabel labelFakePrice = btnBuy.transform.Find("labelPrice_Fake").gameObject.GetComponent<UILabel>();
            labelFakePrice.text = string.Format("₩ {0:N0}", _data.fakePrice);
            labelFakePrice.gameObject.SetActive(true);
        }

        labelPrice.text = string.Format("₩ {0:N0}", _data.price);
        itemImg.spriteName = _data.image;

        eGlobalTextKey _titleKey = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(_data.title);
        eGlobalTextKey _descKey = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(_data.desc);

        labelTitle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(_titleKey);
        labelDesc.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(_descKey);

        btnBuy.onClick.Clear();
        btnBuy.onClick.Add(new EventDelegate(() => OnClickInapp(_key)));
    }


    void RefrashUI(GameObject obj, int objIndex)
    {
        eShopPurchaseKey _key = RayUtils.Utils.ConvertEnumData<eShopPurchaseKey>(string.Format("eStorePrimium_{0}", objIndex));

        UIButton btnBuy = obj.transform.Find("btnBuyOn").gameObject.GetComponent<UIButton>();
        UISprite btnDisable = obj.transform.Find("btnBuyOff").gameObject.GetComponent<UISprite>();
        UILabel labelbtnDisable = btnDisable.transform.Find("labelPrice").gameObject.GetComponent<UILabel>();

        bool isCheck = UserInfoManager.Instance.GetSaveInAppPrimiumItem(_key.ToString());

        btnBuy.gameObject.SetActive(!isCheck);
        btnDisable.gameObject.SetActive(isCheck);

        labelbtnDisable.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eStoreBtnBuyComplite);
        btnBuy.onClick.Add(new EventDelegate(() => OnClickInapp(_key)));

    }
    int upgradPrice = 0;
    ///<summary>
    /// 다이아로 스킬 업그레이드만 따로 
    ///</summary>
    void SetUIDiaSkillUpgrade()
    {

        UIButton btnBuy = objPriminumSkillUpGrade.transform.Find("btnBuyOn").gameObject.GetComponent<UIButton>();
        UILabel labelTitle = objPriminumSkillUpGrade.transform.Find("Light").transform.Find("labelFree").gameObject.GetComponent<UILabel>();
        UILabel labelDesc = objPriminumSkillUpGrade.transform.Find("Light").transform.Find("labelFreeDesc").gameObject.GetComponent<UILabel>();
        UILabel labelPrice = btnBuy.transform.Find("labelPrice").gameObject.GetComponent<UILabel>();

        UISprite btnDisable = objPriminumSkillUpGrade.transform.Find("btnBuyOff").gameObject.GetComponent<UISprite>();
        UILabel labelbtnDisable = btnDisable.transform.Find("labelPrice").gameObject.GetComponent<UILabel>();

        // eStoreTitle_Upgrade_Skill
        labelTitle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eStoreTitle_Upgrade_Skill);
        labelDesc.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eStoreUpgradeSkill_Desc);

        bool isCheck = UserInfoManager.Instance.GetMySkillGrade().Equals(eSkillGrade.S.ToString());
        eSkillGrade _myGrade = RayUtils.Utils.ConvertEnumData<eSkillGrade>(UserInfoManager.Instance.GetMySkillGrade());

        btnBuy.gameObject.SetActive(!isCheck);
        btnDisable.gameObject.SetActive(isCheck);
        labelPrice.gameObject.SetActive(!isCheck);

        // C → B = 10 보석 
        // B → A = 50 보석
        // A → S = 100 보석
        // int price = 0;
        switch (_myGrade)
        {
            case eSkillGrade.C: { upgradPrice = 10; break; }
            case eSkillGrade.B: { upgradPrice = 50; break; }
            case eSkillGrade.A: { upgradPrice = 100; break; }
            case eSkillGrade.S: { upgradPrice = -1; break; }
        }
        // labelbtnDisable.text = _myGrade.ToString();
        labelbtnDisable.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eStoreBtnBuyComplite);
        labelPrice.text = string.Format("x {0:00}", upgradPrice);

        btnBuy.onClick.Add(new EventDelegate(() => OnClickInapp(eShopPurchaseKey.eStorePrimium_9)));

    }

    eShopPurchaseKey _purchageKey = eShopPurchaseKey.NONE;
    bool isActionBuy = false;
    void OnClickInapp(eShopPurchaseKey _key)
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);
        
        STShopData _data = SceneBase.Instance.dataManager.mGetSTShopData(_key);

        Debug.LogError("##################### OnClickInapp : " + _key);
        if (isActionBuy == true)
        {
            return;
        }
        isActionBuy = true;

        string inappKey = "";
#if UNITY_ANDROID
        inappKey = _data.AOS_INAPP_KEY;
#elif UNITY_IOS
        inappKey = _data.IOS_INAPP_KEY;
#endif
        switch (_key)
        {

            case eShopPurchaseKey.eStorePrimium_0:
            case eShopPurchaseKey.eStorePrimium_1:
            case eShopPurchaseKey.eStorePrimium_2:
            case eShopPurchaseKey.eStorePrimium_3:
            case eShopPurchaseKey.eStorePrimium_4:
                {
                    if (PopupManager.Instance != null)
                    {
                        PopupManager.Instance.objNoneTouchBlock.SetActive(true);
                    }

                    SceneBase.Instance.iapManager.BuyProduct(inappKey, ReFrashGoldAndDia);
                    break;
                }
            //// 스타엔딩 구매 
            case eShopPurchaseKey.eStorePrimium_5:
                {
#if UNITY_EDITOR
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
                    Transform _root = PopupManager.Instance.RootPopup;
                    object[] _endData = new object[3];
                    _endData[0] = (eEndingNumber)_random;
                    _endData[1] = false;
                    _endData[2] = true;
                    SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupBuyEnding, _endData);
#else
                    SceneBase.Instance.iapManager.BuyProduct(inappKey, ReFrashGoldAndDia);
#endif

                    break;
                }
            //// 노멀엔딩 구매 
            case eShopPurchaseKey.eStorePrimium_6:
                {
#if UNITY_EDITOR
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
                    Transform _root = PopupManager.Instance.RootPopup;

                    object[] _endData = new object[3];
                    _endData[0] = (eEndingNumber)_random;
                    _endData[1] = false;
                    _endData[2] = true;
                    SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupBuyEnding, _endData);
#else
                    SceneBase.Instance.iapManager.BuyProduct(inappKey, ReFrashGoldAndDia);
#endif
                    break;
                }
            //// S스킬 선택구매 
            case eShopPurchaseKey.eStorePrimium_7:
                {
                    Transform _root = PopupManager.Instance.RootPopup;
                    SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupSkillSList);
                    break;
                }

            //// S스킬 랜덤 구매 
            case eShopPurchaseKey.eStorePrimium_8:
                {
                    Transform _root = PopupManager.Instance.RootPopup;
                    SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupSkillSRandomBuy);
                    break;
                }
            //// 현재 스킬 가져와서 
            //// 다이아 소모값 계산
            case eShopPurchaseKey.eStorePrimium_9:
                {
                    Transform _root = PopupManager.Instance.RootPopup;
                    SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupSkillUpGrade, upgradPrice);
                    break;
                }

        }

        _purchageKey = _key;
        Invoke("InvokeIsActionBuy", 1.0f);
        // SceneBase.Instance.iapManager.BuyProduct(inappKey, ReFrashGoldAndDia);
    }


    void InvokeIsActionBuy()
    {
        isActionBuy = false;
    }

    void ReFrashGoldAndDia(eInAppActionCoce _inappCode, int B)
    {
        if (PopupManager.Instance != null)
        {
            PopupManager.Instance.objNoneTouchBlock.SetActive(false);
        }

        if (eInAppActionCoce.COMPLETE == _inappCode)
        {
            Transform _root = PopupManager.Instance.RootPopup;
            switch (_purchageKey)
            {
                case eShopPurchaseKey.eStoreDia_0:
                case eShopPurchaseKey.eStoreDia_1:
                case eShopPurchaseKey.eStoreDia_2:
                case eShopPurchaseKey.eStoreDia_3:
                    {
                        SceneBase.Instance.PLAY_SFX(eSFX.SFX_26_pruchase_success);

                        SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupBuyInAppItemConfirm, _purchageKey);
                        break;
                    }

                case eShopPurchaseKey.eStorePrimium_0:
                case eShopPurchaseKey.eStorePrimium_1:
                case eShopPurchaseKey.eStorePrimium_2:
                case eShopPurchaseKey.eStorePrimium_3:
                case eShopPurchaseKey.eStorePrimium_4:
                    {
                        SceneBase.Instance.PLAY_SFX(eSFX.SFX_26_pruchase_success);
                        
                        SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupBuyInAppItemConfirm, _purchageKey);
                        SetPrimiumUI();
                        break;
                    }
                    // case eShopPurchaseKey.eStorePrimium_7:
                    // case eShopPurchaseKey.eStorePrimium_8:
                    //     {
                    //         break;
                    //     }

            }
        }
        else
        {
            _purchageKey = eShopPurchaseKey.NONE;
        }

        SetPrimiumUI();
        SceneBase.RefrashGoldDia();
    }

    ///<summary>
    /// 인앱 앤딩 구매
    ///</summary>
    void ACTION_VIEW_ENDING(object[] _data)
    {
        if (_data != null)
        {
            object[] data = _data;
            Transform _root = PopupManager.Instance.RootPopup;
            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupEnding, data);
        }
    }
    ///<summary>
    /// 프리미엄 아이템 구매
    ///<summary>
    void ACTION_BUY_ITEM(eShopPurchaseKey _shopKey)
    {
        if (PopupManager.Instance != null)
        {
            PopupManager.Instance.objNoneTouchBlock.SetActive(false);
        }
        Transform _root = PopupManager.Instance.RootPopup;

        SceneBase.Instance.PLAY_SFX(eSFX.SFX_26_pruchase_success);

        switch (_shopKey)
        {
            //// 올인원팩
            case eShopPurchaseKey.eStorePrimium_0:
            //// 무한의 체력
            case eShopPurchaseKey.eStorePrimium_1:
            //// 바쁘다바빠
            case eShopPurchaseKey.eStorePrimium_2:
            //// 광고스킵
            case eShopPurchaseKey.eStorePrimium_4:
            //// 수익 2배
            case eShopPurchaseKey.eStorePrimium_3:
                {
                    SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupBuyInAppItemConfirm, _shopKey);
                    SetPrimiumUI();
                    break;
                }
        }

        SceneBase.RefrashGoldDia();

    }

    ///<summary>
    /// 앤딩 구매 후 구매 콜백
    ///</summary>
    void ACTION_BUY_ENDING(eShopPurchaseKey _shopKey, object[] data)
    {
        Transform _root = PopupManager.Instance.RootPopup;
        if (data != null)
        {
            object[] _endData = new object[3];
            _endData[0] = (eEndingNumber)data[0];
            _endData[1] = data[1];
            _endData[2] = data[2];

            switch (_shopKey)
            {
                ///// 스타엔딩 구매 
                case eShopPurchaseKey.eStorePrimium_5:
                case eShopPurchaseKey.eStorePrimium_6:
                    {
                        SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupBuyEnding, _endData);
                        break;
                    }
            }
        }

        SceneBase.RefrashGoldDia();
    }
}
