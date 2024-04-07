using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Enums;

public class PopupItemEquip : JHPopup
{

    public static Action ACTION_ITEM_REFRASH;
    public static Action ACTION_ITEM_UPDATE;
    public UILabel labelITemName;
    public UI2DSprite spriteItemIcon;
    public UILabel labelItemSkillInfo;
    public UILabel labelItemDesc;
    public UILabel labelPrice;


    public UIButton btnBuy;

    protected override void OnAwake()
    {
        base.OnAwake();

        btnBuy.onClick.Add(new EventDelegate(OnClickBuy));

    }
    eManagerItemAbility _abily = eManagerItemAbility.NONE;
    eManagerItem _item = eManagerItem.NONE;
    public override void SetData(object[] data)
    {

        //// eBtnItemBuy : 장비구매
        base.SetData(data);
        /// 가격버튼 요청으로 false
        if (data != null)
        {
            _abily = (eManagerItemAbility)data[0];
            _item = (eManagerItem)data[1];

            Debug.LogError("############### PopupItemBuy _abily : " + _abily.ToString() + " // _item : " + _item);

            SetItemUI(_item);
        }
    }

    protected override void OnStart()
    {
        base.OnStart();

        PopupObject.GetComponent<UIPanel>().depth += 100;
    }


    STManagerItemData returnData;
    void SetItemUI(eManagerItem eItem)
    {
        STManagerItemData data = SceneBase.Instance.dataManager.GetManagerItemData(eItem);

        eGlobalTextKey itemNameKey = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(data.eItem.ToString());
        labelITemName.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(itemNameKey);


        eGlobalTextKey itemDesc = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(string.Format("{0}_Text", data.eItem));
        labelItemDesc.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(itemDesc);

        eGlobalTextKey itemInfo = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(data.itemDesc.ToString());
        labelItemSkillInfo.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(itemInfo);

        //// TODO : 아이템 이미지 교체예정
        string _path = string.Format("Image/ManagerItem/{0}", data.itemImage);
        spriteItemIcon.sprite2D = Resources.Load<Sprite>(_path);


        labelPrice.text = string.Format("-{0}", data.price.ToString("N0"));

    }


    void OnClickBuy()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);
        
        if (_abily != eManagerItemAbility.NONE && _item != eManagerItem.NONE)
        {
            int crrGold = int.Parse(UserInfoManager.Instance.GetGold());
            STManagerItemData data = SceneBase.Instance.dataManager.GetManagerItemData(_item);

            if (crrGold >= data.price)
            {
                //// 골드 차감  
                UserInfoManager.Instance.SetGold((crrGold - data.price).ToString());
                //// 아이템 구입 
                UserInfoManager.Instance.SetSaveManagerItem(_item.ToString(), true);
                //// 장비 설정
                UserInfoManager.Instance.SetEquipManagerItem(_abily.ToString(), _item.ToString());

                SceneBase.RefrashGoldDia();

                returnData = data;

                OnClosed();
            }
        }
    }



    protected override void OnClosed()
    {
        base.OnClosed();
    }


    protected override void OnDestroied()
    {
        base.OnDestroied();

        if (ACTION_ITEM_REFRASH != null && ACTION_ITEM_REFRASH.GetInvocationList().Length > 0)
        {
            ACTION_ITEM_REFRASH();
        }

        if (ACTION_ITEM_UPDATE != null && ACTION_ITEM_UPDATE.GetInvocationList().Length > 0)
        {
            ACTION_ITEM_UPDATE();
        }
    }


}
