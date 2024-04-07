using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PopupSkillChanage : JHPopup
{

    // public static System.Action Action<bool> ACTION_SKILL_CHAGE;
    public static System.Action<bool> ACTION_REWARD_SUCESS;

    public UILabel labelTitle;
    public UILabel labelSkillChangeMessage;
    public UILabel labelSkillChangeMessageDesc;

    public UIButton btnChange;
    public UILabel labelBntChage;




    protected override void OnAwake()
    {
        base.OnAwake();


        /// 광고 이벤트 등록
        // JHAdsManager.DelSuccessAdMob += SucessAdMob;


        labelTitle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eSkillChanageTitle);
        labelSkillChangeMessage.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eSkillChagneMessage);
        labelSkillChangeMessageDesc.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eSkillChangeMessageDesc);
        labelBntChage.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eBtnSkillCnage);

        btnChange.onClick.Add(new EventDelegate(OnClickVideoSkillChage));
    }

    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override void OnClosed()
    {
        base.OnClosed();
    }


    protected override void OnDestroied()
    {
        base.OnDestroied();

        if (ACTION_REWARD_SUCESS != null && ACTION_REWARD_SUCESS.GetInvocationList().Length > 0)
        {
            ACTION_REWARD_SUCESS(isActionVideoReward);
        }

        /// 광고 이벤트 해제
        // JHAdsManager.DelSuccessAdMob -= SucessAdMob;
    }

    //// 광고 시청시 매니저 스킬 변경
    ///// TODO : 광고 처리 해야함
    void OnClickVideoSkillChage()
    {


        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        
        if (isActionVideoReward == true)
        {
            return;
        }
        isActionVideoReward = true;
        // StartCoroutine(SceneBase.Instance.adsManager.IEShowAdMobVideo(eVideoAds.eVideoManagerSkill));
    }

    bool isActionVideoReward;
    ///<summary>
    /// 광고 성공 시 보냄
    ///</summary>
    void SucessAdMob(bool flag)
    {
        if (flag == true)
        {
            OnClosed();
            isActionVideoReward = flag;
        }
    }


}
