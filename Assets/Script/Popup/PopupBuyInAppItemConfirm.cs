using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;


public class PopupBuyInAppItemConfirm : JHPopup
{

    public UILabel labelTitle;
    public UISprite spriteItemImg;
    public UILabel labelItemName;
    public UILabel labelItemBuyDesc;


    STShopData _shopData;

    protected override void OnAwake()
    {
        base.OnAwake();

        labelTitle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupBuyInAppItemTitle);
        labelItemBuyDesc.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupBuyInAppItemDesc);
    }

    protected override void OnClosed()
    {
        base.OnClosed();

        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);
    }

    protected override void OnDestroied()
    {
        base.OnDestroied();
    }


    public override void SetData(object _data)
    {
        base.SetData();

        if (_data != null)
        {
            eShopPurchaseKey _key = (eShopPurchaseKey)_data;
            _shopData = SceneBase.Instance.dataManager.mGetSTShopData(_key);
            string strInappKey = _shopData.eShopKey.ToString();
            eGlobalTextKey itemName = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(strInappKey);
            labelItemName.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(itemName);
            spriteItemImg.spriteName = _shopData.image;
        }
    }
}
