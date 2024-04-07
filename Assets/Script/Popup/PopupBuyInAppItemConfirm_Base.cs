using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;


public class PopupBuyInAppItemConfirm_Base : MonoBehaviour
{

    public UILabel labelTitle;
    public UISprite spriteItemImg;
    public UILabel labelItemName;
    public UILabel labelItemBuyDesc;


    STShopData _shopData;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        labelTitle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupBuyInAppItemTitle);
        labelItemBuyDesc.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupBuyInAppItemDesc);
    }

    public void SetPopupBuyInAppItemConfirm_Base(eShopPurchaseKey _key)
    {
        Dictionary<eShopPurchaseKey, STShopData> _dic = SceneBase.Instance.dataManager.mGetDicShopData();
        STShopData _shopData = _dic[_key];
        string strInappKey = _shopData.eShopKey.ToString();
        eGlobalTextKey itemName = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(strInappKey);
        labelItemName.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(itemName);
        spriteItemImg.spriteName = _shopData.image;


    }


    void OnClickClose()
    {

        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);
        
        Destroy(this.gameObject);
    }

}
