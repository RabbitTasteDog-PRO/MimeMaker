using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;

public class PopupSkillUpGrade : JHPopup
{
    public static Action<string, STManagerSkill, bool> ACTION_BUY_SKILL_UPGRADE;

    public UIButton btnClose;
    public UILabel labelSkillBuyTitle;
    public UILabel labelSkillBuyDesc;

    public UIButton btnBuy;
    public UILabel labelBuyPrice;


    protected override void OnAwake()
    {
        base.OnAwake();

        btnClose.onClick.Add(new EventDelegate(OnClosed));
        labelSkillBuyTitle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupSkillUpGradeTitle);
        labelSkillBuyDesc.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.ePopupBuySkillSSelectDesc);
        // labelBuyPrice.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.ePopupBuySkillBtnPrice);

        btnBuy.onClick.Add(new EventDelegate(OnClickBuySkill));

    }

    protected override void OnDestroied()
    {
        base.OnDestroied();

        if (ACTION_BUY_SKILL_UPGRADE != null && ACTION_BUY_SKILL_UPGRADE.GetInvocationList().Length > 0)
        {
            ACTION_BUY_SKILL_UPGRADE("UPGRADE", skillData, isActionBuy);
        }
    }

    protected override void OnClosed()
    {
        base.OnClosed();
    }

    int _price = 0;
    public override void SetData(object data)
    {
        base.SetData();
        if (data != null)
        {
            _price = (int)data;
            labelBuyPrice.text = string.Format("x {0:00}", _price);
        }
    }

    STManagerSkill skillData;

    bool isActionBuy = false;
    void OnClickBuySkill()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);
        
        if (isActionBuy == true)
        {
            return;
        }

        int dai = int.Parse(UserInfoManager.Instance.GetDia());
        if (dai < _price)
        {
            string _msg = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.TextEmptyDia);
            SceneBase.Instance.AddEmptyPurchasePopup(_msg);
            return;
        }


        // int crrDia = dai - _price;
        UserInfoManager.Instance.SetDia((dai - _price).ToString());
        eSkillGrade myGrade = RayUtils.Utils.ConvertEnumData<eSkillGrade>(UserInfoManager.Instance.GetMySkillGrade());
        int plusGrde = (int)myGrade + 1;
        UserInfoManager.Instance.SetMySkillGrade(((eSkillGrade)plusGrde).ToString());
        isActionBuy = true;
        Invoke("InvokeIsActionBuy", 1.0f);

        eManager_Skill _mySkill = RayUtils.Utils.ConvertEnumData<eManager_Skill>(UserInfoManager.Instance.GetMySkill());
        Dictionary<eSkillGrade, STManagerSkill> _dic = SceneBase.Instance.dataManager.GetDicManagerSkillGradeData(_mySkill);
        skillData = _dic[(eSkillGrade)plusGrde];
        SceneBase.RefrashGoldDia();
        OnClosed();


    }

    void InvokeIsActionBuy()
    {
        isActionBuy = false;
    }


}
