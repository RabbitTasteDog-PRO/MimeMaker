using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PopupBuyFiaryItemConfirm : JHPopup
{
    public UILabel labelTitle;
    public UISprite spriteItem;

    public UILabel labelItemName;
    public UILabel labelDesc;

    protected override void OnAwake()
    {
        base.OnAwake();

//         PopupBuyFiaryItemConfirmTitle	구입 완료
// PopupBuyFiaryItemConfirmDesc	아이템을 구입했습니다!


        PopupManager.Instance.objNoneTouchBlock.SetActive(false);

        labelTitle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupBuyFiaryItemConfirmTitle);
        labelDesc.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupBuyFiaryItemConfirmDesc);



    }

    string buyType;
    eGlobalTextKey _itemName;
    public override void SetData(object _data)
    {
        base.SetData();

        if (_data != null)
        {
            _itemName = (eGlobalTextKey)_data;
            eShopPurchaseKey _shopKey = RayUtils.Utils.ConvertEnumData<eShopPurchaseKey>(_itemName.ToString());

            Dictionary<eShopPurchaseKey, STShopData>  _dic = SceneBase.Instance.dataManager.mGetDicShopData();
            STShopData _dataShop = _dic[_shopKey];
            spriteItem.spriteName = _dataShop.image;


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
    }


    void OnClickVideo()
    {

    }




    void OnClickDia()
    {

        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);
        
        int dia = int.Parse(UserInfoManager.Instance.GetDia());

        if(UserInfoManager.FAIRY_GIFT_DIA > dia)
        {
    //// ㄷㅏ이ㅇ 부족 팝업
        }
        else
        {
            
        }
    }







}
