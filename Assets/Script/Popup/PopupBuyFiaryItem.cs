using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;
public class PopupBuyFiaryItem : JHPopup
{
    public static Action<bool> ACTION_BUY_FAIRY_ITEM;

    public UILabel labelTitle;

    public UILabel labelItemName;
    public UISprite spriteItemImag;
    public UILabel labelDesc;

    public UIButton btnVideo;
    public UILabel labelVideoPrice;
    public UIButton btnDia;
    public UILabel labelDiaPrice;

    protected override void OnAwake()
    {
        base.OnAwake();

        //         PopupBuyFiaryItemTitle	아이템 구입
        // PopupBuyFiaryItemDesc	아이템을 구입하식겠습니까?

        JHAdsManager.ACTION_ADS_COMPLETE = SucessAdMob;

        PopupManager.Instance.objNoneTouchBlock.SetActive(false);

        labelTitle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupBuyFiaryItemTitle);
        labelDesc.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupBuyFiaryItemDesc);

        labelVideoPrice.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eFairyItemAds);


        btnVideo.onClick.Add(new EventDelegate(OnClickVideo));
        btnDia.onClick.Add(new EventDelegate(OnClickDia));

    }

    string buyType;
    eGlobalTextKey _itemName;
    public override void SetData(object[] _data)
    {
        base.SetData();

        if (_data != null)
        {
            buyType = (string)_data[0];
            if (buyType == "DIA")
            {
                btnVideo.gameObject.SetActive(false);
                btnDia.gameObject.SetActive(true);
                labelDiaPrice.text = string.Format("x {0}", UserInfoManager.FAIRY_GIFT_DIA);
            }
            else
            {
                btnVideo.gameObject.SetActive(true);
                btnDia.gameObject.SetActive(false);
            }
            _itemName = (eGlobalTextKey)_data[1];
            eShopPurchaseKey _shopKey = RayUtils.Utils.ConvertEnumData<eShopPurchaseKey>(_itemName.ToString());
            Dictionary<eShopPurchaseKey, STShopData> _dic = SceneBase.Instance.dataManager.mGetDicShopData();
            STShopData _dataShop = _dic[_shopKey];
            spriteItemImag.spriteName = _dataShop.image;

            labelItemName.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(_itemName);
        }


    }


    protected override void OnClosed()
    {
        base.OnClosed();
    }

    protected override void OnDestroied()
    {
        base.OnDestroied();

        JHAdsManager.ACTION_ADS_COMPLETE = null;

        if (isActionBuy == true)
        {
            Transform _root = PopupManager.Instance.RootPopup;
            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupBuyFiaryItemConfirm, _itemName);
        }

        if (ACTION_BUY_FAIRY_ITEM != null)
        {
            ACTION_BUY_FAIRY_ITEM(isActionBuy);
        }
    }

    bool isActionVideo;
    void OnClickVideo()
    {
        if (isActionVideo == false)
        {

            SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

            isActionVideo = true;

            SceneBase.Instance.adsManager.AsyncShowAdMobVideo(converVideoFiaryItem(_itemName));

            Invoke("InvokeIsActionVideo", 3.0f);
        }
    }


    void InvokeIsActionVideo()
    {
        isActionVideo = false;
    }



    bool isActionBuy = false;
    void OnClickDia()
    {

        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);
        
        int dia = int.Parse(UserInfoManager.Instance.GetDia());

        if (UserInfoManager.FAIRY_GIFT_DIA > dia)
        {
            //// ㄷㅏ이ㅇ 부족 팝업
            Transform _root = PopupManager.Instance.RootPopup;
            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupBuyFiaryFail);

        }
        else
        {
            UserInfoManager.Instance.SetDia((dia - UserInfoManager.FAIRY_GIFT_DIA).ToString());
            UserInfoManager.Instance.SetSaveFairyGiftItem(converFiaryItm(_itemName).ToString(), true);
            eShopPurchaseKey _buff = RayUtils.Utils.ConvertEnumData<eShopPurchaseKey>(_itemName.ToString());
            UserInfoManager.Instance.SetSaveItemBuff(_buff.ToString(), true);

            isActionBuy = true;
            OnClosed();
        }
    }


    eFairyGiftItem converFiaryItm(eGlobalTextKey _key)
    {
        eFairyGiftItem _fairy = eFairyGiftItem.NONE;
        switch (_key)
        {
            case eGlobalTextKey.eStoreItemBuff_2:
                {
                    _fairy = eFairyGiftItem.EVENT;
                    break;
                }
            case eGlobalTextKey.eStoreItemBuff_3:
                {
                    _fairy = eFairyGiftItem.GOLD;
                    break;
                }
            case eGlobalTextKey.eStoreItemBuff_4:
                {
                    _fairy = eFairyGiftItem.TIME;
                    break;
                }

        }
        return _fairy;
    }


    eVideoAds converVideoFiaryItem(eGlobalTextKey _key)
    {
        eVideoAds _fairy = eVideoAds.NONE;
        switch (_key)
        {
            case eGlobalTextKey.eStoreItemBuff_2:
                {
                    _fairy = eVideoAds.eVideoFairyEvent;
                    break;
                }
            case eGlobalTextKey.eStoreItemBuff_3:
                {
                    _fairy = eVideoAds.eVideoFairyGold;
                    break;
                }
            case eGlobalTextKey.eStoreItemBuff_4:
                {
                    _fairy = eVideoAds.eVideoFairyTime;
                    break;
                }

        }
        return _fairy;
    }

    ///<summary>
    /// 광고 성공 시 보냄
    ///</summary>
    void SucessAdMob(bool flag)
    {
        if (flag == true)
        {
            eFairyGiftItem _efairyType = converFiaryItm(_itemName);
            switch (_efairyType)
            {
                case eFairyGiftItem.EVENT:
                    {
                        UserInfoManager.Instance.SetSaveFairyGiftItem(Enums.eFairyGiftItem.EVENT.ToString(), true);
                        UserInfoManager.Instance.SetSaveInAppPrimiumItem(Enums.eShopPurchaseKey.eStoreItemBuff_2.ToString(), true);
                        eShopPurchaseKey _buff = RayUtils.Utils.ConvertEnumData<eShopPurchaseKey>(_itemName.ToString());
                        UserInfoManager.Instance.SetSaveItemBuff(_buff.ToString(), true);
                        break;
                    }

                case eFairyGiftItem.GOLD:
                    {
                        UserInfoManager.Instance.SetSaveFairyGiftItem(Enums.eFairyGiftItem.GOLD.ToString(), true);
                        UserInfoManager.Instance.SetSaveInAppPrimiumItem(Enums.eShopPurchaseKey.eStoreItemBuff_3.ToString(), true);
                        
                        eShopPurchaseKey _buff = RayUtils.Utils.ConvertEnumData<eShopPurchaseKey>(_itemName.ToString());
                        UserInfoManager.Instance.SetSaveItemBuff(_buff.ToString(), true);
                        break;
                    }
                case eFairyGiftItem.TIME:
                    {
                        UserInfoManager.Instance.SetSaveFairyGiftItem(Enums.eFairyGiftItem.TIME.ToString(), true);
                        UserInfoManager.Instance.SetSaveInAppPrimiumItem(Enums.eShopPurchaseKey.eStoreItemBuff_4.ToString(), true);
                        
                        eShopPurchaseKey _buff = RayUtils.Utils.ConvertEnumData<eShopPurchaseKey>(_itemName.ToString());
                        UserInfoManager.Instance.SetSaveItemBuff(_buff.ToString(), true);
                        break;
                    }

            }
            isActionBuy = flag;
            OnClosed();
        }
        else
        {
            Transform _root = PopupManager.Instance.RootPopup;
            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupBuyFiaryFail);
        }
    }


}


