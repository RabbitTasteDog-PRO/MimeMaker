using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Enums;
public class PopupSkillSRandomBuy : JHPopup
{

    // public static Action<eManager_Skill, bool> ACTION_BUY_SKILL_RANDOM_COMPLETE;

    public UIButton btnClose;
    public UILabel labelSkillBuyTitle;
    public UILabel labelSkillBuyDesc;

    public UIButton btnBuy;
    public UILabel labelBuyPrice;


    protected override void OnAwake()
    {
        base.OnAwake();

        btnClose.onClick.Add(new EventDelegate(OnClosed));
        labelSkillBuyTitle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eStoreTitle_Random_Skill_S);
        labelSkillBuyDesc.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eStoreRandomSkillS_Desc);
        labelBuyPrice.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.ePopupBuySkillRandomBtnPrice);

        btnBuy.onClick.Add(new EventDelegate(OnClickBuySkill));

    }

    protected override void OnDestroied()
    {
        base.OnDestroied();

        // if (ACTION_BUY_SKILL_RANDOM_COMPLETE != null && ACTION_BUY_SKILL_RANDOM_COMPLETE.GetInvocationList().Length > 0)
        // {
        //     ACTION_BUY_SKILL_RANDOM_COMPLETE(skillData, isBuyCheck);
        // }
    }

    protected override void OnClosed()
    {
        base.OnClosed();
    }
    STManagerSkill skillData;

    protected override void OnStart()
    {
        base.OnStart();

        List<eManager_Skill> _skill = new List<eManager_Skill>();
        for (int i = 0; i < (int)eManager_Skill.COUNT; i++)
        {
            if ((eManager_Skill)i == eManager_Skill.SKILL_F)
            {
                continue;
            }

            if (UserInfoManager.Instance.GetSaveInappManagerSKill(((eManager_Skill)i).ToString()) == false)
            {
                _skill.Add((eManager_Skill)i);
            }
        }

        eManager_Skill _random = (eManager_Skill)UnityEngine.Random.Range(0, _skill.Count - 1);
        Dictionary<eManager_Skill, Dictionary<eSkillGrade, STManagerSkill>> _dic = SceneBase.Instance.dataManager.GetDicManagerSkillData();
        Dictionary<eSkillGrade, STManagerSkill> _tempDic = _dic[_random];
        skillData = _tempDic[eSkillGrade.S];

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
        STShopData _stShopData = SceneBase.Instance.dataManager.mGetSTShopData(eShopPurchaseKey.eStorePrimium_8);

#if UNITY_EDITOR
        UserInfoManager.Instance.SetSaveInappManagerSKill(skillData.managerSkill.ToString(), true);
        UserInfoManager.Instance.SetMySkill(skillData.managerSkill.ToString());
        UserInfoManager.Instance.SetMySkillGrade(eSkillGrade.S.ToString());

        isBuyCheck = true;
        OnClosed();
#elif UNITY_ANDROID
        _key = _stShopData.AOS_INAPP_KEY;
        SceneBase.Instance.iapManager.BuyProduct(_key, ReFrashSucessItem);
#elif UNITY_IOS
        _key = _stShopData.IOS_INAPP_KEY;
        SceneBase.Instance.iapManager.BuyProduct(_key, ReFrashSucessItem);
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
            OnClosed();
        }
        //// TODO : 구매 실패 팝업띄우기 
        else
        {

        }
    }


}