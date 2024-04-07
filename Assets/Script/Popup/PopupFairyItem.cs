using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Enums;
using StructuredDataType;

enum eFairyItem
{
    NONE = -1,
    EVENT,
    GOLD,
    TIME,
    COUNT
}


public class PopupFairyItem : JHPopup
{

    bool isBuyItem;
    public static Action<List<WeeklyScheduleData>, bool> ACTION_FAIRY_ITEM_STAR;

    public UILabel labelFairyItemTitle;
    public UILabel labelFairyItemTitleSub;


    [Header("돌발이벤트")]
    public GameObject objEvent;
    public UILabel labelEventDesc;
    public UILabel labelEventAmount;
    public UIButton btnEventAds;
    public UILabel labelBtnEventAds;
    public UIButton btnEventDia;
    public UILabel labelBtnEvntDia;
    public GameObject objEventSelect;

    [Header("골드 보너스")]
    public GameObject objGold;
    public UILabel labelGoldDesc;
    public UILabel labelGoldAmount;
    public UIButton btnGoldAds;
    public UILabel labelBtnGoldAds;
    public UIButton btnGoldDia;
    public UILabel labelBtnGoldDia;
    public GameObject objGoldSelect;

    [Header("시간감소")]
    public GameObject objTime;
    public UILabel labelTimeDesc;
    public UILabel labelTimeAmount;
    public UIButton btnTimeAds;
    public UILabel labelBtnTimeads;
    public UIButton btnTimeDia;
    public UILabel labelBtnTimeDia;
    public GameObject objTimeSelect;


    public UIButton btnStart;
    public UILabel labelBtnStart;


    eFairyGiftItem _efairyType = eFairyGiftItem.NONE;


    List<WeeklyScheduleData> list = null;
    bool quickStart = false;


    protected override void OnAwake()
    {
        base.OnAwake();

        JHAdsManager.ACTION_ADS_COMPLETE = SucessAdMob;
        PopupBuyFiaryItem.ACTION_BUY_FAIRY_ITEM = RefrashItem;

        labelEventAmount.text = "";
        labelGoldAmount.text = "";
        labelTimeAmount.text = "";

        labelBtnEvntDia.text = string.Format("x {0}", UserInfoManager.FAIRY_GIFT_DIA);
        labelBtnGoldDia.text = string.Format("x {0}", UserInfoManager.FAIRY_GIFT_DIA);
        labelBtnTimeDia.text = string.Format("x {0}", UserInfoManager.FAIRY_GIFT_DIA);


        labelFairyItemTitle.text = ConverGlobalText(Enums.eGlobalTextKey.eFairyItemTitle);
        labelFairyItemTitleSub.text = ConverGlobalText(Enums.eGlobalTextKey.eFairyItemTitleSub);

        labelEventDesc.text = ConverGlobalText(Enums.eGlobalTextKey.eFairyItemEventDesc);
        labelGoldDesc.text = ConverGlobalText(Enums.eGlobalTextKey.eFairyItemGoldDesc);
        labelTimeDesc.text = ConverGlobalText(Enums.eGlobalTextKey.eFairyItemTimeDesc);


        labelBtnStart.text = ConverGlobalText(Enums.eGlobalTextKey.eFairyItemStart);


        labelBtnEventAds.text = ConverGlobalText(Enums.eGlobalTextKey.eFairyItemAds);
        labelBtnGoldAds.text = ConverGlobalText(Enums.eGlobalTextKey.eFairyItemAds);
        labelBtnTimeads.text = ConverGlobalText(Enums.eGlobalTextKey.eFairyItemAds);


        btnEventDia.onClick.Add(new EventDelegate(OnClickEvent_Dia));
        btnGoldDia.onClick.Add(new EventDelegate(OnClickGold_Dia));
        btnTimeDia.onClick.Add(new EventDelegate(OnClickTime_Dia));

        btnEventAds.onClick.Add(new EventDelegate(OnClickEvent_Video));
        btnGoldAds.onClick.Add(new EventDelegate(OnClickGold_Video));
        btnTimeAds.onClick.Add(new EventDelegate(OnClickTime_Video));

        UIButton[] _btn = new UIButton[]
        {
            btnEventDia, btnGoldDia, btnTimeDia,
            btnEventAds, btnGoldAds, btnTimeAds
        };
        bool isCheck = false;
        for (int i = 0; i < (int)eWeekly.COUNT; i++)
        {
            isCheck = UserInfoManager.Instance.GetProgressedWeeklyData(((eWeekly)i).ToString());
            if (isCheck == true)
            {
                break;
            }
        }
        //// 일정 진행중이라면 아이템 못사게
        if (isCheck == true)
        {
            for (int i = 0; i < _btn.Length; i++)
            {
                _btn[i].onClick.Clear();
                _btn[i].onClick.Add(new EventDelegate(OnClickCheckSchedule));
            }
        }


        btnStart.onClick.Add(new EventDelegate(OnClickStart));

        for (int i = 0; i < (int)eFairyGiftItem.COUNT; i++)
        {
            SetFairyGiftUI((eFairyGiftItem)i);
        }

    }

    bool isScheduleStart = false;
    protected override void OnDestroied()
    {
        base.OnDestroied();

        JHAdsManager.ACTION_ADS_COMPLETE = null;

        //// 광고로 구입
        if (ACTION_FAIRY_ITEM_STAR != null && ACTION_FAIRY_ITEM_STAR.GetInvocationList().Length > 0)
        {
            if (list != null)
            {
                ACTION_FAIRY_ITEM_STAR(list, isScheduleStart);

                SceneBase.RefrashGoldDia();
            }
        }

    }

    protected override void OnClosed()
    {
        base.OnClosed();
    }

    public override void SetData(object[] _data)
    {
        base.SetData();


        if (_data != null)
        {
            list = (List<WeeklyScheduleData>)_data[0];
            quickStart = (bool)_data[1];
        }


        for (int i = 0; i < (int)eFairyGiftItem.COUNT; i++)
        {
            SetFairyGiftUI((eFairyGiftItem)i);
        }
    }

    void RefrashItem(bool flag)
    {
        for (int i = 0; i < (int)eFairyGiftItem.COUNT; i++)
        {
            SetFairyGiftUI((eFairyGiftItem)i);
        }
    }


    void SetFairyGiftUI(eFairyGiftItem giftType)
    {
        switch (giftType)
        {
            case eFairyGiftItem.EVENT:
                {
                    if (UserInfoManager.Instance.GetSaveFairyGiftItem(eFairyGiftItem.EVENT.ToString()) == true ||
                    UserInfoManager.Instance.GetSaveInAppPrimiumItem(eGlobalTextKey.eStoreItemBuff_2.ToString()) == true)
                    {
                        objEventSelect.SetActive(true);
                    }
                    else
                    {
                        objEventSelect.SetActive(false);
                    }
                    break;
                }

            case eFairyGiftItem.GOLD:
                {
                    if (UserInfoManager.Instance.GetSaveFairyGiftItem(eFairyGiftItem.GOLD.ToString()) == true ||
                    UserInfoManager.Instance.GetSaveInAppPrimiumItem(eGlobalTextKey.eStoreItemBuff_3.ToString()) == true)
                    {
                        objGoldSelect.SetActive(true);
                    }
                    else
                    {
                        objGoldSelect.SetActive(false);
                    }

                    // objGoldSelect.SetActive(UserInfoManager.Instance.GetSaveFairyGiftItem(eFairyGiftItem.GOLD.ToString()));
                    break;
                }

            case eFairyGiftItem.TIME:
                {
                    if (UserInfoManager.Instance.GetSaveFairyGiftItem(eFairyGiftItem.TIME.ToString()) == true ||
                    UserInfoManager.Instance.GetSaveInAppPrimiumItem(eGlobalTextKey.eStoreItemBuff_4.ToString()) == true)
                    {
                        objTimeSelect.SetActive(true);
                    }
                    else
                    {
                        objTimeSelect.SetActive(false);
                    }

                    // objTimeSelect.SetActive(UserInfoManager.Instance.GetSaveFairyGiftItem(eFairyGiftItem.TIME.ToString()));
                    break;
                }
        }
    }


    ///<summary>
    /// 시작하기 
    ///</summary>
    void OnClickStart()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        isScheduleStart = true;
        OnClosed();
    }

    ///<summary>
    ///
    ///</summary>
    void OnClickEvent_Video()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        if (UserInfoManager.Instance.GetSaveFairyGiftItem(Enums.eFairyGiftItem.EVENT.ToString()) == true)
        {
            // SceneBase.Instance.AddEmptyPurchasePopup("아이템을 이미 구매하셨습니다.");
            string _msg = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eTextBuyCheck);
            SceneBase.Instance.AddEmptyPurchasePopup(_msg);
            return;
        }
        else
        {
            Transform _root = PopupManager.Instance.RootPopup;
            object[] _data = new object[2];
            _data[0] = "VIDEO";
            _data[1] = eGlobalTextKey.eStoreItemBuff_2;
            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupBuyFiaryItem, _data);
        }
    }

    ///<summary>
    ///
    ///</summary>
    void OnClickEvent_Dia()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        if (UserInfoManager.Instance.GetSaveFairyGiftItem(Enums.eFairyGiftItem.EVENT.ToString()) == true)
        {
            // SceneBase.Instance.AddEmptyPurchasePopup("아이템을 이미 구매하셨습니다.");
            string _msg = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eTextBuyCheck);
            SceneBase.Instance.AddEmptyPurchasePopup(_msg);
            return;
        }

        int dia = int.Parse(UserInfoManager.Instance.GetDia());
        if (dia < UserInfoManager.FAIRY_GIFT_DIA)
        {
            Transform _root = PopupManager.Instance.RootPopup;
            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupBuyFiaryFail);
            // SceneBase.Instance.AddEmptyPurchasePopup("다이아가 모자랍니다.");
            return;
        }
        else
        {
            Transform _root = PopupManager.Instance.RootPopup;
            object[] _data = new object[2];
            _data[0] = "DIA";
            _data[1] = eGlobalTextKey.eStoreItemBuff_2;
            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupBuyFiaryItem, _data);
        }

    }
    ///<summary>
    ///
    ///</summary>
    void OnClickGold_Video()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        if (UserInfoManager.Instance.GetSaveFairyGiftItem(Enums.eFairyGiftItem.GOLD.ToString()) == true)
        {
            // SceneBase.Instance.AddEmptyPurchasePopup("아이템을 이미 구매하셨습니다.");
            string _msg = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eTextBuyCheck);
            SceneBase.Instance.AddEmptyPurchasePopup(_msg);
            return;
        }
        else
        {
            Transform _root = PopupManager.Instance.RootPopup;
            object[] _data = new object[2];
            _data[0] = "VIDEO";
            _data[1] = eGlobalTextKey.eStoreItemBuff_3;
            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupBuyFiaryItem, _data);
        }

    }
    ///<summary>
    ///
    ///</summary>
    void OnClickGold_Dia()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        if (UserInfoManager.Instance.GetSaveFairyGiftItem(Enums.eFairyGiftItem.GOLD.ToString()) == true)
        {
            // SceneBase.Instance.AddEmptyPurchasePopup("아이템을 이미 구매하셨습니다.");
            string _msg = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eTextBuyCheck);
            SceneBase.Instance.AddEmptyPurchasePopup(_msg);
            return;
        }

        int dia = int.Parse(UserInfoManager.Instance.GetDia());
        if (dia < UserInfoManager.FAIRY_GIFT_DIA)
        {
            // SceneBase.Instance.AddEmptyPurchasePopup("다이아가 모자랍니다.");
            Transform _root = PopupManager.Instance.RootPopup;
            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupBuyFiaryFail);
            return;
        }
        else
        {
            Transform _root = PopupManager.Instance.RootPopup;
            object[] _data = new object[2];
            _data[0] = "DIA";
            _data[1] = eGlobalTextKey.eStoreItemBuff_3;
            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupBuyFiaryItem, _data);

        }

        // SetFairyGiftUI(eFairyGiftItem.GOLD);
    }
    ///<summary>
    ///
    ///</summary>
    void OnClickTime_Video()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        if (UserInfoManager.Instance.GetSaveFairyGiftItem(Enums.eFairyGiftItem.TIME.ToString()) == true)
        {
            // SceneBase.Instance.AddEmptyPurchasePopup("아이템을 이미 구매하셨습니다.");
            string _msg = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eTextBuyCheck);
            SceneBase.Instance.AddEmptyPurchasePopup(_msg);
            return;
        }
        else
        {
            Transform _root = PopupManager.Instance.RootPopup;
            object[] _data = new object[2];
            _data[0] = "VIDEO";
            _data[1] = eGlobalTextKey.eStoreItemBuff_4;
            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupBuyFiaryItem, _data);
        }

    }
    ///<summary>
    ///
    ///</summary>
    void OnClickTime_Dia()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        if (UserInfoManager.Instance.GetSaveFairyGiftItem(Enums.eFairyGiftItem.TIME.ToString()) == true)
        {
            // SceneBase.Instance.AddEmptyPurchasePopup("아이템을 이미 구매하셨습니다.");
            string _msg = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eTextBuyCheck);
            SceneBase.Instance.AddEmptyPurchasePopup(_msg);
            return;
        }

        int dia = int.Parse(UserInfoManager.Instance.GetDia());
        if (dia < UserInfoManager.FAIRY_GIFT_DIA)
        {
            // SceneBase.Instance.AddEmptyPurchasePopup("다이아가 모자랍니다.");
            Transform _root = PopupManager.Instance.RootPopup;
            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupBuyFiaryFail);
            return;
        }
        else
        {
            Transform _root = PopupManager.Instance.RootPopup;
            object[] _data = new object[2];
            _data[0] = "DIA";
            _data[1] = eGlobalTextKey.eStoreItemBuff_4;
            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupBuyFiaryItem, _data);

        }

        // SetFairyGiftUI(eFairyGiftItem.TIME);
    }


    void OnClickCheckSchedule()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);
        
        string _msg = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eTextScheduleCheckBuyFairyItem);
        SceneBase.Instance.AddEmptyPurchasePopup(_msg);
    }


    string ConverGlobalText(Enums.eGlobalTextKey _key)
    {

        string _text = SceneBase.Instance.dataManager.GetDicGlobalTextData(_key);
        return _text;
    }

    void RefrashVideo()
    {
        int crrCount = UserInfoManager.Instance.GetFreeVideoCount();
        UserInfoManager.Instance.SetFreeVideoCount(crrCount + 1);

        SceneBase.RefrashGoldDia();
    }


    //// 광고 시청시 매니저 스킬 변경
    bool isActionVideoReward;
    ///<summary>
    /// 광고 성공 시 보냄
    ///</summary>
    void SucessAdMob(bool flag)
    {
        if (flag == true)
        {
            isActionVideoReward = false;
            // eFairyGiftItem _efairyType = eFairyGiftItem.NONE;
            switch (_efairyType)
            {
                case eFairyGiftItem.EVENT:
                    {
                        UserInfoManager.Instance.SetSaveInAppPrimiumItem(eGlobalTextKey.eStoreItemBuff_2.ToString(), true);
                        UserInfoManager.Instance.SetSaveFairyGiftItem(Enums.eFairyGiftItem.EVENT.ToString(), true);
                        break;
                    }

                case eFairyGiftItem.GOLD:
                    {
                        UserInfoManager.Instance.SetSaveInAppPrimiumItem(eGlobalTextKey.eStoreItemBuff_3.ToString(), true);
                        UserInfoManager.Instance.SetSaveFairyGiftItem(Enums.eFairyGiftItem.GOLD.ToString(), true);
                        break;
                    }
                case eFairyGiftItem.TIME:
                    {
                        UserInfoManager.Instance.SetSaveInAppPrimiumItem(eGlobalTextKey.eStoreItemBuff_4.ToString(), true);
                        UserInfoManager.Instance.SetSaveFairyGiftItem(Enums.eFairyGiftItem.TIME.ToString(), true);
                        break;
                    }

            }

            SetFairyGiftUI(_efairyType);

        }
        _efairyType = eFairyGiftItem.NONE;
        RefrashVideo();
    }


}
