using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System.Linq;
using System.Linq.Expressions;



public class PopupStore_ItemBuff : MonoBehaviour
{

    //// 체력회복시 체력게이지 변경
    public static System.Action ACTION_ITEM_REFRESH;

    public GameObject[] objBuff;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        for (int i = 0; i < objBuff.Length; i++)
        {
            SetInAppBuffUI(objBuff[i], i);
        }

        PopupBuyFiaryItem.ACTION_BUY_FAIRY_ITEM = ItemRerash;
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        for (int i = 0; i < objBuff.Length; i++)
        {
            SetInAppBuffUI(objBuff[i], i);
        }
    }


    public void SetBuffUI()
    {
        for (int i = 0; i < objBuff.Length; i++)
        {
            SetInAppBuffUI(objBuff[i], i);
        }
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        if (ACTION_ITEM_REFRESH != null)
        {
            ACTION_ITEM_REFRESH();
        }
    }

    void ItemRerash(bool flag)
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_28_item);

        for (int i = 0; i < objBuff.Length; i++)
        {
            SetInAppBuffUI(objBuff[i], i);
        }
    }


    void SetInAppBuffUI(GameObject obj, int objIndex)
    {

        eShopPurchaseKey _key = RayUtils.Utils.ConvertEnumData<eShopPurchaseKey>(string.Format("eStoreItemBuff_{0}", objIndex));
        STShopData _data = SceneBase.Instance.dataManager.mGetSTShopData(_key);

        UISprite itemImg = obj.transform.Find("Light").transform.Find("Sprite").gameObject.GetComponent<UISprite>();
        UILabel labelTitle = obj.transform.Find("Light").transform.Find("labelFree").gameObject.GetComponent<UILabel>();
        UILabel labelDesc = obj.transform.Find("Light").transform.Find("labelFreeDesc").gameObject.GetComponent<UILabel>();

        UIButton btnBuy = obj.transform.Find("btnBuy").gameObject.GetComponent<UIButton>();

        UILabel labelPrice = btnBuy.transform.Find("labelPrice").gameObject.GetComponent<UILabel>();

        if (_data.fakePrice != -1)
        {
            UILabel labelFakePrice = btnBuy.transform.Find("labelPrice_Fake").gameObject.GetComponent<UILabel>();
            labelFakePrice.text = string.Format("x {0:N0}", _data.fakePrice);
            labelFakePrice.gameObject.SetActive(true);
        }

        labelPrice.text = string.Format("x {0:N0}", _data.price);
        itemImg.spriteName = _data.image;

        eGlobalTextKey _titleKey = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(_data.title);
        eGlobalTextKey _descKey = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(_data.desc);

        labelTitle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(_titleKey);
        labelDesc.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(_descKey);


        btnBuy.onClick.Add(new EventDelegate(() => OnClickItemBuff(_key)));


        bool isCheck = false;
        for (int i = 0; i < (int)eWeekly.COUNT; i++)
        {
            isCheck = UserInfoManager.Instance.GetProgressedWeeklyData(((eWeekly)i).ToString());
            if (isCheck == true)
            {
                if (objIndex == 2 || objIndex == 3 || objIndex == 4)
                {
                    btnBuy.onClick.Clear();
                    btnBuy.onClick.Add(new EventDelegate(OnClickCheckSchedule));
                }
                break;
            }
        }
    }

    bool isActionBuy = false;
    void OnClickItemBuff(eShopPurchaseKey _key)
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        // Debug.LogError("##################### eShopPurchaseKey : " + _key);
        if (isActionBuy == true)
        {
            return;
        }
        isActionBuy = true;
        Invoke("InvokeIsActionBuy", 0.5f);

        STShopData _data = SceneBase.Instance.dataManager.mGetSTShopData(_key);
        eGlobalTextKey _titleKey = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(_data.title);
        int dia = int.Parse(UserInfoManager.Instance.GetDia());

        if (dia < _data.price)
        {
            // SceneBase.Instance.AddTestTextPopup("다이아가 부족합니다");
            Transform _root = PopupManager.Instance.RootPopup;
            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupBuyFiaryFail);
            return;

        }
        else
        {

            switch (_key)
            {
                //// 체력회복 30
                case eShopPurchaseKey.eStoreItemBuff_0:
                    {
                        UserInfoManager.Instance.SetDia((dia - _data.price).ToString());

                        int plusHp = UserInfoManager.Instance.GetSaveHP() + UserInfoManager.BUFF_RECOVERY_HP_30;
                        UserInfoManager.Instance.SetSaveHP(plusHp);
                        Transform _root = PopupManager.Instance.RootPopup;
                        SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupBuyConfirm, _key);

                        // 
                        // string message = string.Format("{0} 구매하였습니다.", SceneBase.Instance.dataManager.GetDicGlobalTextData(_titleKey));
                        // string _format = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eTextBuyCheck_0);
                        // string message = string.Format(_format, SceneBase.Instance.dataManager.GetDicGlobalTextData(_titleKey));
                        // SceneBase.Instance.AddTestTextPopup(message);
                        break;
                    }

                //// 체력회복 10
                case eShopPurchaseKey.eStoreItemBuff_1:
                    {
                        UserInfoManager.Instance.SetDia((dia - _data.price).ToString());

                        int plusHp = UserInfoManager.Instance.GetSaveHP() + UserInfoManager.BUFF_RECOVERY_HP_10;
                        UserInfoManager.Instance.SetSaveHP(plusHp);

                        Transform _root = PopupManager.Instance.RootPopup;
                        SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupBuyConfirm, _key);

                        // string message = string.Format("{0} 구매하였습니다.", SceneBase.Instance.dataManager.GetDicGlobalTextData(_titleKey));
                        // string _format = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eTextBuyCheck_0);
                        // string message = string.Format(_format, SceneBase.Instance.dataManager.GetDicGlobalTextData(_titleKey));
                        // SceneBase.Instance.AddTestTextPopup(message);

                        // SceneBase.Instance.AddTestTextPopup(message);
                        break;
                    }

                //// 요정의 축복 , 돌발이벤트 버프
                case eShopPurchaseKey.eStoreItemBuff_2:
                    {
                        if (UserInfoManager.Instance.GetSaveFairyGiftItem(Enums.eFairyGiftItem.EVENT.ToString()) == true)
                        {
                            string _msg = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eTextBuyCheck);
                            SceneBase.Instance.AddTestTextPopup(_msg);
                            // SceneBase.Instance.AddTestTextPopup("이미 구매하였습니다.");
                            return;
                        }
                        else
                        {
                            Transform _root = PopupManager.Instance.RootPopup;
                            object[] _dataInfo = new object[2];
                            _dataInfo[0] = "DIA";
                            _dataInfo[1] = eGlobalTextKey.eStoreItemBuff_2;
                            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupBuyFiaryItem, _dataInfo);
                        }

                        break;
                    }

                //// 비밀금고 , 한주차 금액 두대
                case eShopPurchaseKey.eStoreItemBuff_3:
                    {
                        if (UserInfoManager.Instance.GetSaveFairyGiftItem(Enums.eFairyGiftItem.GOLD.ToString()) == true)
                        {
                            // SceneBase.Instance.AddTestTextPopup("이미 구매하였습니다.");
                            string _msg = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eTextBuyCheck);
                            SceneBase.Instance.AddTestTextPopup(_msg);
                            return;
                        }
                        else
                        {
                            Transform _root = PopupManager.Instance.RootPopup;
                            object[] _dataInfo = new object[2];
                            _dataInfo[0] = "DIA";
                            _dataInfo[1] = eGlobalTextKey.eStoreItemBuff_3;
                            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupBuyFiaryItem, _dataInfo);
                        }
                        break;
                    }

                //// 체력회복 시간 5배 
                case eShopPurchaseKey.eStoreItemBuff_4:
                    {
                        if (UserInfoManager.Instance.GetSaveFairyGiftItem(Enums.eFairyGiftItem.TIME.ToString()) == true)
                        {
                            // SceneBase.Instance.AddTestTextPopup("이미 구매하였습니다.");
                            string _msg = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eTextBuyCheck);
                            SceneBase.Instance.AddTestTextPopup(_msg);
                            return;
                        }
                        else
                        {
                            Transform _root = PopupManager.Instance.RootPopup;
                            object[] _dataInfo = new object[2];
                            _dataInfo[0] = "DIA";
                            _dataInfo[1] = eGlobalTextKey.eStoreItemBuff_4;
                            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupBuyFiaryItem, _dataInfo);
                        }

                        break;
                    }
            }


        }

        SceneBase.RefrashGoldDia();

    }

    void OnClickCheckSchedule()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);
        
        string _msg = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eTextScheduleCheckBuyFairyItem);
        SceneBase.Instance.AddEmptyPurchasePopup(_msg);
    }


    void InvokeIsActionBuy()
    {
        isActionBuy = false;
    }


}
