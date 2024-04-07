using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StoreItemPrefab : MonoBehaviour
{

    public static Action ACTION_ITEM_REFRASH;

    [Header("아이템유무")]
    public UI2DSprite imgItem;
    public GameObject objHanveItem;
    public UIButton btnHave;
    public UILabel labelHaveItem;

    [Header("아이템 장비")]
    public GameObject objEquipItem;
    public UIButton btnEquip;
    public UILabel labelEquipItem;

    [Header("아이템구매")]
    public GameObject objBuyItem;
    public UIButton btnBuyItem;
    public UILabel labelItemPrice;


    Enums.eManagerItem _eShopItem;
    STManagerItemData itemData;


    Enums.eManagerItemAbility _ability = Enums.eManagerItemAbility.NONE;

    void Awake()
    {
        btnBuyItem.onClick.Add(new EventDelegate(OnClickBuy));


        labelHaveItem.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eHave);//"보유중";
        labelEquipItem.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eEquip);//"착용중";

        btnHave.onClick.Add(new EventDelegate(OnClickHave));
        btnEquip.onClick.Add(new EventDelegate(OnClickEquip));
    }

    public void SetItemUI(STManagerItemData item)
    {
        _eShopItem = item.eItem;
        gameObject.SetActive(true);

        bool isHave = UserInfoManager.Instance.GetSaveManagerItem(item.eItem.ToString());
        bool isEquip = item.eItem.ToString() == UserInfoManager.Instance.GetEquipManagerItem(item.eAbility.ToString());

        imgItem.sprite2D = Resources.Load<Sprite>(string.Format("Image/ManagerItem/{0}", item.itemImage));

        //// TODO : 아이템 UI 처리할것
        if (isHave == true)
        {
            for (int i = 0; i < (int)Enums.eManagerItemAbility.COUNT; i++)
            {
                objHanveItem.SetActive(!isEquip);
                objBuyItem.SetActive(false);
                objEquipItem.SetActive(isEquip);
            }
        }
        else
        {
            objHanveItem.SetActive(isHave);
            objEquipItem.SetActive(isHave);
            objBuyItem.SetActive(true);


            labelItemPrice.text = itemData.price.ToString("N0");

        }
        itemData = SceneBase.Instance.dataManager.GetManagerItemData(_eShopItem);
        // Debug.LogError("################### eManagerItemAbility : " + item.eAbility.ToString() + " // have : " + isHave + " // isEquip : " + isEquip);
    }

    ///<summary>
    /// 아이템 해제
    ///</summary>
    void OnClickEquip()
    {
        if (UserInfoManager.Instance.GetSaveManagerItem(_eShopItem.ToString()) == false)
        {
            //// 매니저 아이템 없을경우 리턴
            return;
        }

        Transform _root = PopupManager.Instance.RootPopup;
        Enums.eManagerItemAbility _abil = itemData.eAbility;
        object[] _data = new object[2];
        _data[0] = _abil;
        _data[1] = _eShopItem;

        SceneBase.Instance.AddPopup(_root, Enums.ePopupLayer.PopupItemBuy, _data);

        // Debug.LogError("@@@@@ OnClickEquip itemData.eItem : " + itemData.eItem + "  // itemData.eAbility : " + itemData.eAbility);

        if (ACTION_ITEM_REFRASH != null && ACTION_ITEM_REFRASH.GetInvocationList().Length > 0)
        {
            ACTION_ITEM_REFRASH();
        }


    }

    ///<summary>
    /// 아이템 장착
    ///</summary>
    void OnClickHave()
    {

        if (UserInfoManager.Instance.GetSaveManagerItem(_eShopItem.ToString()) == false)
        {
            //// 매니저 아이템 없을경우 리턴
            return;
        }

        Transform _root = PopupManager.Instance.RootPopup;
        Enums.eManagerItemAbility _abil = itemData.eAbility;
        object[] _data = new object[2];
        _data[0] = _abil;
        _data[1] = _eShopItem;

        // Debug.LogError("OnClickHave########   itemData.eItem : " + itemData.eItem + "  // itemData.eAbility : " + itemData.eAbility);

        SceneBase.Instance.AddPopup(_root, Enums.ePopupLayer.PopupItemBuy, _data);


        if (ACTION_ITEM_REFRASH != null && ACTION_ITEM_REFRASH.GetInvocationList().Length > 0)
        {
            ACTION_ITEM_REFRASH();
        }



    }


    void OnClickBuy()
    {
        ///// TODO 
        ///// 구매용 팝업 요청

        // int gold = int.Parse(UserInfoManager.Instance.GetGold());
        // int price = itemData.price;
        // if (gold < itemData.price)
        // {

        //     SceneBase.Instance.AddEmptyPurchasePopup("Empty Gold , My Gold : " + gold + " // price : " + price);
        //     ///TODO :  팝업 만들것 
        //     Debug.LogError("골드 부족 팝업 ");
        //     return;
        // }
        // else
        {
            //// TODO : 주석 제거할것
            //// 매니저 아이템 저장
            Transform _root = PopupManager.Instance.RootPopup;
            Enums.eManagerItemAbility _abil = itemData.eAbility;
            object[] _data = new object[2];

            _data[0] = _abil;
            _data[1] = _eShopItem;
            // 아이템 구매 팝업 띄움
            SceneBase.Instance.AddPopup(_root, Enums.ePopupLayer.PopupItemBuy, _data);
        }


    }

    public Enums.eManagerItem GetItemData()
    {
        return _eShopItem;
    }


}
