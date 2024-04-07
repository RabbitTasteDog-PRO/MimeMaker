using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using Enums;

public class PopupManagerLicense : JHPopup
{
    public static System.Action ACTION_MANAGER_UI;
    public static Action<bool> ACTION_LICENCE;

    public UISprite spriteManagerPhoto;

    public UILabel labelNameString;
    public UILabel labelManagerName;
    public UILabel labelCharacterString;
    public UILabel labelCharacter;
    public UILabel labelSkillString;
    public UILabel labelSkill;
    public UIButton btnSkillDetail;

    public UIButton btnViedoRetry;
    public UILabel labelBtnViedoRetry;
    public UIButton btnConfirm;
    public UILabel labelBtnConfirm;

    public GameObject objLicenceAnim;
    public GameObject objButtons;


    eManager_Type managerType;
    eManager_Skill mangerSkill;
    eSkillGrade managerSkillGrade;
    ePersonality_Type managerPersonalType;


    public UISprite spriteGrade;

    string man = "main_02_manager_profile_character_260x260";
    string woman = "manager_2_192x224";


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void OnAwake()
    {

        PopupAdMobStart.ACTION_CLOSE_ADMOB_START_POPUP = ACTION_CLOSE_ADMOB_START_POPUP;

        if (UserInfoManager.Instance.GetSaveManagerGender() == true)
        {
            spriteManagerPhoto.spriteName = woman;
            spriteManagerPhoto.transform.localPosition = new Vector2(-3, -13);

        }
        else
        {
            spriteManagerPhoto.spriteName = man;
            spriteManagerPhoto.transform.localPosition = Vector2.zero;
        }

        btnViedoRetry.onClick.Add(new EventDelegate(OnClickVideoRetry));
        btnConfirm.onClick.Add(new EventDelegate(OnClickConfirm));
        btnSkillDetail.onClick.Add(new EventDelegate(OnClickSkillDetail));

        labelBtnViedoRetry.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eBtnVideoRetry);
    }

    public override void SetData(object[] data_)
    {
        if (data_ != null)
        {

            managerType = (eManager_Type)data_[0];
            mangerSkill = (eManager_Skill)data_[1];
            managerSkillGrade = (eSkillGrade)data_[2];
            managerPersonalType = (ePersonality_Type)data_[3];


            Dictionary<eManager_Type, STManagerData> dicManager = SceneBase.Instance.dataManager.GetDicManagerData();
            STManagerData managerData = dicManager[managerType];

            Dictionary<eManager_Skill, Dictionary<eSkillGrade, STManagerSkill>> dicSkill = SceneBase.Instance.dataManager.GetDicManagerSkillData();
            Dictionary<eSkillGrade, STManagerSkill> _dicSkill = dicSkill[mangerSkill];
            STManagerSkill skillData = _dicSkill[managerSkillGrade];

            /// 매니저 이름 
            labelManagerName.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(string.Format("e{0}", managerType)));
            /// 매니저 성격
            labelCharacter.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(string.Format("e{0}", managerPersonalType)));
            string _managerSkill = SceneBase.Instance.dataManager.GetDicGlobalTextData(RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(string.Format("e{0}", mangerSkill)));
            string _skillGrade = UserInfoManager.Instance.GetMySkillGrade();

            spriteGrade.spriteName = ConverGradeIcon(managerSkillGrade);
            spriteGrade.MakePixelPerfect();

            labelSkill.text = _managerSkill; //string.Format("{0} {1}", _managerSkill, _skillGrade);

        }
    }

    protected override void OnDestroied()
    {
        base.OnDestroied();

        /// 광고 완료 후 매니저 다시뽑기
        if (isActionRewardSucess == true)
        {
            isActionRewardSucess = false;
            Debug.LogError("애안뚜냐ㅑㅑㅑㅑㅑㅑㅑㅑㅑㅑㅑㅑㅑㅑㅑㅑㅑㅑㅑㅑㅑㅑㅑㅑㅑ : " + isActionRewardSucess);
            Transform _root = PopupManager.Instance.RootPopup;
            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupManagerTest);

        }
        else
        {

            if (isActionLicence == true)
            {
                if (ACTION_LICENCE != null && ACTION_LICENCE.GetInvocationList().Length > 0)
                {
                    ACTION_LICENCE(isActionLicence);
                }

                if (ACTION_MANAGER_UI != null && ACTION_MANAGER_UI.GetInvocationList().Length > 0)
                {
                    ACTION_MANAGER_UI();
                }
            }

        }

        PopupAdMobStart.ACTION_CLOSE_ADMOB_START_POPUP = null;
        JHAdsManager.ACTION_ADS_COMPLETE = null;
    }

    protected override void OnClosed()
    {
        base.OnClosed();
    }

    protected override void OnStart()
    {
        base.OnStart();

        StartCoroutine(LicenceAnimation());
    }

    bool isVideoRetry = false;
    void OnClickVideoRetry()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        Transform _root = PopupManager.Instance.RootPopup;
        STAdMobStartTextData data = new STAdMobStartTextData();

        data.strTitle = eGlobalTextKey.eManagerChangeTitle.ToString();
        data.strDesc = eGlobalTextKey.eManagerChagneMessage.ToString();
        data.strDescSub = eGlobalTextKey.eManagerChangeMessageDesc.ToString();
        data.strBtnText = eGlobalTextKey.eBtnManagerCnage.ToString();

        data.vidoeKey = eVideoAds.eVideoManagerTest.ToString();

        SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupAdMobStart, data);
    }

    bool isActionRewardSucess = false;
    void ACTION_ADMOB_SUCESS(bool flag)
    {
        Debug.LogError("#############################  ACTION_ADMOB_SUCESS : " + flag);
        if (flag == true)
        {
            isActionRewardSucess = flag;
            OnClosed();
        }
    }



    void ACTION_CLOSE_ADMOB_START_POPUP(bool flag)
    {
        if (flag == true)
        {
            SceneBase.Instance.adsManager.AsyncShowAdMobVideo(eVideoAds.eVideoManagerTest);
            if (JHAdsManager.ACTION_ADS_COMPLETE.GetInvocationList().Length > 0)
            {
                JHAdsManager.ACTION_ADS_COMPLETE = null;
            }
            JHAdsManager.ACTION_ADS_COMPLETE = ACTION_ADMOB_SUCESS;
            // StartCoroutine(SceneBase.Instance.adsManager.IEShowAdMobVideo(eVideoAds.eVideoManagerTest));
        }
    }


    void OnClickSkillDetail()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        Transform _root = PopupManager.Instance.RootPopup;
        SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupManagerSkillDetail);
    }

    bool isActionLicence;
    void OnClickConfirm()
    {
        // SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        isActionLicence = true;
        OnClosed();
        /// 매니저 및 스킬저장
        // UserInfoManager.Instance.
    }


    IEnumerator LicenceAnimation()
    {
        yield return new WaitForSeconds(0.3f);
        objLicenceAnim.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        objLicenceAnim.GetComponent<TweenRotation>().gameObject.SetActive(true);
        objLicenceAnim.GetComponent<TweenRotation>().enabled = true;
        yield return new WaitForSeconds(0.3f);
        objButtons.SetActive(true);
    }

    string ConverGradeIcon(eSkillGrade _grade)
    {
        string convert = "level_c_48x48";
        switch (_grade)
        {
            case eSkillGrade.C: { convert = "level_c_48x48"; break; }
            case eSkillGrade.B: { convert = "level_b_48x48"; break; }
            case eSkillGrade.A: { convert = "level_a_48x48"; break; }
            case eSkillGrade.S: { convert = "level_s_48x48"; break; }
        }

        return convert;
    }
}
