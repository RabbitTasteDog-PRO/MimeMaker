using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;


public struct STAdMobStartTextData
{
    public string strTitle;
    public string strDesc;
    public string strDescSub;

    public string strBtnText;

    public string vidoeKey;

    public STAdMobStartTextData(string _admobKey, string _strTitle, string _strDesc, string _strDescSub, string _strBtnText)
    {
        strTitle = _strTitle;
        strDesc = _strDesc;
        strDescSub = _strDescSub;
        strBtnText = _strBtnText;
        vidoeKey = _admobKey;
    }
}


public class PopupAdMobStart : JHPopup
{

    public static System.Action<bool> ACTION_SKILL_CHAGE;
    public static System.Action<bool> ACTION_RANDOM_STATE;
    // public static System.Action<bool> ACTION_REWARD_SUCESS;
    public static System.Action<bool> ACTION_CLOSE_ADMOB_START_POPUP;


    public UILabel labelTitle;
    public UILabel labelChnageDesc;
    public UILabel labelChangeDescSub;

    public UIButton btnChange;
    public UILabel labelBntChage;

    eVideoAds _eVideobKey = eVideoAds.NONE;
    public override void SetData(object data)
    {
        if (data != null)
        {
            Debug.LogError("############### PopupAdMobStart SetDAta");
            STAdMobStartTextData _textData = (STAdMobStartTextData)data;

            eGlobalTextKey _eTitle = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(_textData.strTitle);
            eGlobalTextKey _eDesc = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(_textData.strDesc);
            eGlobalTextKey _eDescSub = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(_textData.strDescSub);
            eGlobalTextKey _eBtnText = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(_textData.strBtnText);


            _eVideobKey = RayUtils.Utils.ConvertEnumData<eVideoAds>(_textData.vidoeKey);
            STVideoAdsData _data = SceneBase.Instance.dataManager.mGetDicVideoData(_eVideobKey);
            SetText(_eTitle, _eDesc, _eDescSub, _eBtnText);
        }
    }


    protected override void OnAwake()
    {
        base.OnAwake();

        labelTitle.text = "";
        labelChnageDesc.text = "";
        labelChangeDescSub.text = "";
        labelBntChage.text = "";

        /// 광고 이벤트 등록
        // JHAdsManager.ACTION_ADMOB_SUCESS = SucessAdMob;
        // labelTitle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eSkillChanageTitle);
        // labelChnageDesc.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eSkillChagneMessage);
        // labelChangeDescSub.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eSkillChangeMessageDesc);
        // labelBntChage.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eBtnSkillCnage);
        btnChange.onClick.Add(new EventDelegate(OnClickVideoStart));

        Debug.LogError("############### PopupAdMobStart OnAwkae");
    }


    void SetText(eGlobalTextKey title, eGlobalTextKey desc, eGlobalTextKey descSub, eGlobalTextKey btnText)
    {
        labelTitle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(title);
        labelChnageDesc.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(desc);
        labelChangeDescSub.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(descSub);
        labelBntChage.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(btnText);

        Debug.LogError("############### PopupAdMobStart SetText");
    }


    protected override void OnClosed()
    {
        base.OnClosed();
    }


    protected override void OnDestroied()
    {
        base.OnDestroied();
        // Debug.LogError("############### OnClickVideoStart OnDestroied 111111 : " + isActionVideoStart);
        if (ACTION_CLOSE_ADMOB_START_POPUP != null && ACTION_CLOSE_ADMOB_START_POPUP.GetInvocationList().Length > 0)
        {
            Debug.LogError("############### OnClickVideoStart OnDestroied 22222 : " + isActionVideoStart);
            ACTION_CLOSE_ADMOB_START_POPUP(isActionVideoStart);
        }
        Debug.LogError("############### ACTION_SKILL_CHAGE ACTION_SKILL_CHAGE 00000 : " + isActionVideoStart);
        if (ACTION_SKILL_CHAGE != null)
        {
            Debug.LogError("############### ACTION_SKILL_CHAGE ACTION_SKILL_CHAGE 111111 : " + isActionVideoStart);
            ACTION_SKILL_CHAGE(isActionVideoStart);
        }

        if(ACTION_RANDOM_STATE != null)
        {
            ACTION_RANDOM_STATE(isActionVideoStart);
        }
    }

    bool isActionVideoStart = false;
    //// 광고 시청시 매니저 스킬 변경
    ///// TODO : 광고 처리 해야함
    void OnClickVideoStart()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        isActionVideoStart = true;
        Debug.LogError("############### OnClickVideoStart isActionVideoStart : " + isActionVideoStart);
        Destroy(this.gameObject);
    }
}
