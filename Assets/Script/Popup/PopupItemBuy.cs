using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;

public class PopupItemBuy : JHPopup
{

    public static Action ACTION_ITEM_REFRASH;
    public UILabel labelITemName;
    public UI2DSprite spriteItemIcon;
    public UILabel labelItemSkillInfo;
    public UILabel labelItemDesc;
    public UILabel labelPrice;


    public UIButton btnBuy;


    [Header("Equip")]
    public UITable tableItem;
    public GameObject objEquip;
    public UISprite spriteEquip;
    public UILabel labelEquip;
    public UIButton btnEquip;
    public UILabel labelBtnEquip;
    public UIButton btnUnEquip;
    public UILabel labelBtnUnEquip;
    public UIGrid gridEquipBtn;

    protected override void OnAwake()
    {
        base.OnAwake();

        labelBtnEquip.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eBtnItemEquip);//   장착
        labelBtnUnEquip.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eBtnItemUnEquip);// 장착해제


        btnBuy.onClick.Add(new EventDelegate(OnClickBuy));

        btnEquip.onClick.Add(new EventDelegate(OnClickEquip));
        btnUnEquip.onClick.Add(new EventDelegate(OnClickUnEquip));



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

            // bool isHave = UserInfoManager.Instance.GetSaveManagerItem(_item.ToString());
            // btnBuy.gameObject.SetActive(!isHave);

            // btnEquip.gameObject.SetActive(isHave);
            // btnUnEquip.gameObject.SetActive(isHave);
            // spriteEquip.gameObject.SetActive(isHave);

            tableItem.Reposition();
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
        //// main_02_btn_02_211x112 on
        //// test_btn_ok_off_211x112 off
        //// C2C3C5 장착일경우 컬라  
        //// 575757 아웃라인

        //// 기본컬러
        /// 502615


        //// 아이템 있을때
        if (UserInfoManager.Instance.GetSaveManagerItem(_item.ToString()) == true)
        {

            objEquip.SetActive(true);
            btnEquip.gameObject.SetActive(true);
            // btnUnEquip.gameObject.SetActive(true);
            spriteEquip.gameObject.SetActive(true);

            tableItem.Reposition();

            btnBuy.gameObject.SetActive(false);

            eGlobalTextKey _itemKey;
            if (UserInfoManager.Instance.GetEquipManagerItem(_abily.ToString()) == _item.ToString())
            {
                spriteEquip.spriteName = "box4";
                _itemKey = eGlobalTextKey.eEquip;

                btnEquip.normalSprite = "test_btn_ok_off_211x112";
                btnUnEquip.gameObject.SetActive(true);

            }
            else
            {
                spriteEquip.spriteName = "box3";
                _itemKey = eGlobalTextKey.eHave;

                btnEquip.normalSprite = "main_02_btn_02_211x112";
                btnUnEquip.gameObject.SetActive(false);
            }
            labelEquip.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(_itemKey);

            gridEquipBtn.Reposition();
        }
        else
        {

            objEquip.SetActive(false);
            btnEquip.gameObject.SetActive(false);
            btnUnEquip.gameObject.SetActive(false);
            spriteEquip.gameObject.SetActive(false);

            tableItem.Reposition();

            btnBuy.gameObject.SetActive(true);
        }
        //// 아이템 없을때
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
            else
            {
                string _msg = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.TextEmptyGold);
                SceneBase.Instance.AddTestTextPopup(_msg);
            }
        }
    }

    void OnClickEquip()
    {
        // SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_30_manager_item_on);



        UserInfoManager.Instance.SetEquipManagerItem(_abily.ToString(), _item.ToString());
        SetItemUI(_item);
    }
    //// TODO 
    //// 해제시 안돌아감 확인해봐야함 
    void OnClickUnEquip()
    {
        // SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_31_manager_item_off);
        
        if (_item.ToString() != UserInfoManager.Instance.GetEquipManagerItem(_abily.ToString()))
        {
            return;
        }

        UserInfoManager.Instance.SetEquipManagerItem(_abily.ToString(), "");
        SetItemUI(_item);
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

    }


}
