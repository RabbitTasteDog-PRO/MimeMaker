using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;

public class PopupSkillSBuy : JHPopup
{

    public static Action<STManagerSkill, bool> ACTION_BUY_SKILL_COMPLETE;

    public UIButton btnClose;
    public UILabel labelSkillBuyTitle;
    public UILabel labelSkillBuyDesc;

    public UIButton btnBuy;
    public UILabel labelBuyPrice;


    protected override void OnAwake()
    {
        base.OnAwake();

        btnClose.onClick.Add(new EventDelegate(OnClosed));
        labelSkillBuyTitle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.ePopupBuySkillSSelectTitle);
        labelSkillBuyDesc.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.ePopupBuySkillSSelectDesc);
        labelBuyPrice.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.ePopupBuySkillBtnPrice);

        btnBuy.onClick.Add(new EventDelegate(OnClickBuySkill));

    }

    protected override void OnDestroied()
    {
        base.OnDestroied();

        if (ACTION_BUY_SKILL_COMPLETE != null && ACTION_BUY_SKILL_COMPLETE.GetInvocationList().Length > 0)
        {
            ACTION_BUY_SKILL_COMPLETE(skillData, isBuyCheck);
            SceneBase.INAPP_SELECT_SKILL_DATA = null;
        }
    }

    protected override void OnClosed()
    {
        base.OnClosed();
    }
    STManagerSkill skillData;
    public override void SetData(object _data)
    {
        if (_data != null)
        {
            skillData = (STManagerSkill)_data;
        }
    }


    bool isActionBuy = false;
    void OnClickBuySkill()
    {

        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);
        
        if (isActionBuy == true)
        {
            return;
        }

        isActionBuy = true;
        Invoke("InvokeIsActionBuy", 1.0f);

        string _key = "";
        STShopData _stShopData = SceneBase.Instance.dataManager.mGetSTShopData(eShopPurchaseKey.eStorePrimium_7);

#if UNITY_EDITOR
        UserInfoManager.Instance.SetSaveInappManagerSKill(skillData.managerSkill.ToString(), true);
        UserInfoManager.Instance.SetMySkill(skillData.managerSkill.ToString());
        UserInfoManager.Instance.SetMySkillGrade(eSkillGrade.S.ToString());

        isBuyCheck = true;
        OnClosed();
#elif UNITY_ANDROID
        _key = _stShopData.AOS_INAPP_KEY;
        SceneBase.Instance.iapManager.BuyProduct(_key, ReFrashSucessItem);
        SceneBase. INAPP_SELECT_SKILL_DATA = skillData;
#elif UNITY_IOS
        _key = _stShopData.IOS_INAPP_KEY;
        SceneBase.Instance.iapManager.BuyProduct(_key, ReFrashSucessItem);
        SceneBase. INAPP_SELECT_SKILL_DATA = skillData;
#endif
    }

    void InvokeIsActionBuy()
    {
        isActionBuy = false;
    }

    bool isBuyCheck = false;
    void ReFrashSucessItem(eInAppActionCoce A, int B)
    {
        if (PopupManager.Instance != null)
        {
            PopupManager.Instance.objNoneTouchBlock.SetActive(false);
        }

        if (A == eInAppActionCoce.COMPLETE)
        {
            isBuyCheck = true;
            Destroy(this.gameObject);
            // OnClosed();
        }
        //// TODO : 구매 실패 팝업띄우기 
        else
        {

        }
    }


}
