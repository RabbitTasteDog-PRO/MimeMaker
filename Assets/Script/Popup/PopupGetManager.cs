using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PopupGetManager : JHPopup
{
    public static System.Action ACTION_MANAGER_UI;

    public GameObject objTitle;

    public UILabel labelManagerName;

    public GameObject objLight;
    public GameObject objStar;
    public GameObject objCharactor;
    public GameObject objResultBubbleTalk;
    public GameObject objParticle;
    public UI2DSprite spriteCharactor;

    public UILabel labelPersonality;
    public UILabel labelPersonlInfo;

    public UILabel labelSkill;
    public UILabel lableSkillInfo;
    public UIButton btnSkillDetail;
    public UISprite spriteGrade;


    public GameObject objGender;
    public UISprite spriteWoman;
    public UISprite spriteMan;
    public UIButton btnWoman;
    public UIButton btnMan;


    public UIButton btnViedoRetry;
    public UILabel labelBtnViedoRetry;
    public UIButton btnConfirm;
    public UILabel labelBtnConfirm;

    eManager_Type managerType;
    eManager_Skill mangerSkill;
    eSkillGrade managerSkillGrade;
    ePersonality_Type managerPersonlity;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void OnAwake()
    {
        base.OnAwake();

        JHAdsManager.ACTION_ADS_COMPLETE = ACTION_REWARD;

        btnSkillDetail.onClick.Add(new EventDelegate(OnClickSkillDetail));
        btnViedoRetry.onClick.Add(new EventDelegate(OnClickViedoRetry));
        btnConfirm.onClick.Add(new EventDelegate(OnClickConfirm));

        btnWoman.onClick.Add(new EventDelegate(OnClickManagerGender));
        btnMan.onClick.Add(new EventDelegate(OnClickManagerGender));


        labelBtnViedoRetry.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eBtnVideoRetry);
    }

    object[] objReturnData = new object[4];


    public override void SetData(object[] data_)
    {
        if (data_ != null)
        {
            string fixManager = (string)data_[0];
            int fixValue = (int)data_[1];

            managerPersonlity = RayUtils.Utils.ConvertEnumData<ePersonality_Type>(fixManager);
            managerType = returnManagerType(fixManager);
            //// 매니저 결정
            // UserInfoManager.Instance.SetMyManager(managerType.ToString());

            //// 재능 발굴일경우 랜덤스킬중 하나이기 때문에 따로 관리
            if (managerType == eManager_Type.MANAGER_F)
            {
                int randomSkill = Random.Range((int)eManager_Skill.SKILL_F_A, ((int)eManager_Skill.COUNT - 1));
                mangerSkill = (eManager_Skill)randomSkill;
                managerSkillGrade = Random.Range(0, 100) >= 90 ? eSkillGrade.B : eSkillGrade.C;
            }
            else
            {

                int mngSkill = (int)managerType;
                mangerSkill = (eManager_Skill)mngSkill;
                managerSkillGrade = Random.Range(0, 100) >= 90 ? eSkillGrade.B : eSkillGrade.C;
            }
            spriteGrade.spriteName = ConverGradeIcon(managerSkillGrade);
            //// 매니저 설정
            JHManagerManger.Instance.SetMyManager(managerType, mangerSkill, managerSkillGrade);

            objReturnData[0] = managerType; /// 매니저 타입 
            objReturnData[1] = mangerSkill; /// 매니저 스킬
            objReturnData[2] = managerSkillGrade; /// 매니저 스킬 등급
            objReturnData[3] = managerPersonlity; /// 매니저 성격

            Debug.LogError(string.Format("MyManager : {0}, My Skill : {1}, My Skill Grade : {2}", managerType, mangerSkill, managerSkillGrade));

            Debug.LogError("My Manager : " + UserInfoManager.Instance.GetMyManager());
            Debug.LogError("My Skill : " + UserInfoManager.Instance.GetMySkill());
            Debug.LogError("My Skill Grade : " + UserInfoManager.Instance.GetMySkillGrade());

            STManagerData managerData = SceneBase.Instance.dataManager.GetSTManagerData(managerType);

            eGlobalTextKey _personalKey = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(string.Format("e{0}", managerData.managerPersonality.ToString()));
            labelPersonlInfo.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(_personalKey);
            eGlobalTextKey _skillKey = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(string.Format("e{0}", mangerSkill.ToString()));
            string skillGrade = UserInfoManager.Instance.GetMySkillGrade();
            lableSkillInfo.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(_skillKey);//string.Format("{0} {1}", SceneBase.Instance.dataManager.GetDicGlobalTextData(_skillKey), skillGrade);

            string managerName = UserInfoManager.Instance.GetMyManagerName();
            if (string.IsNullOrEmpty(managerName) == true)
            {
                eGlobalTextKey _name = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(string.Format("e{0}", managerData.manager.ToString()));
                labelManagerName.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(_name);
            }
            else
            {
                labelManagerName.text = UserInfoManager.Instance.GetMyManagerName();
            }

            bool gender = UserInfoManager.Instance.GetSaveManagerGender();
            string _ch = gender == true ? "manager_1_172x268" : "character_manager_a_180x280";

            spriteCharactor.sprite2D = Resources.Load<Sprite>(string.Format("Image/Character/{0}", _ch));



            SetGenderUI(gender);

        }
    }

    protected override void OnStart()
    {
        base.OnStart();

        StartCoroutine(SetUIProssce());
    }



    ///<summary>
    /// 스킬소개 옆 돋보기 
    ///</summary>
    void OnClickSkillDetail()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        Transform _root = PopupManager.Instance.RootPopup;
        SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupManagerSkillDetail);
    }

    void OnClickManagerGender()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        bool _flag = UserInfoManager.Instance.GetSaveGetManager();
        UserInfoManager.Instance.SetSaveGetManager(!_flag);

        SetGenderUI(UserInfoManager.Instance.GetSaveGetManager());
    }

    void SetGenderUI(bool flag)
    {
        btnWoman.GetComponent<BoxCollider2D>().enabled = flag == true ? false : true;
        btnMan.GetComponent<BoxCollider2D>().enabled = flag == true ? true : false;

        btnWoman.normalSprite = flag == true ? "woman_on_85x77" : "woman_off_85x77";
        btnMan.normalSprite = flag == true ? "man_off_85x77" : "man_on_85x77";

        spriteWoman.spriteName = flag == true ? "woman_on_85x77" : "woman_off_85x77";
        spriteMan.spriteName = flag == true ? "man_off_85x77" : "man_on_85x77";

        // bool gender = UserInfoManager.Instance.GetSaveManagerGender();
        string _ch = flag == true ? "manager_1_172x268" : "character_manager_a_180x280";

        spriteCharactor.sprite2D = Resources.Load<Sprite>(string.Format("Image/Character/{0}", _ch));
    }

    bool isActionAds = false;
    void OnClickViedoRetry()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);
        
        if (isActionAds == true)
        {
            return;
        }
        isActionAds = true;
        Invoke("InvkeIsAds", 4.0f);
        SceneBase.Instance.adsManager.AsyncShowAdMobVideo(eVideoAds.eVideoManagerTest);
        // StartCoroutine(SceneBase.Instance.adsManager.IEShowAdMobVideo(eVideoAds.eVideoManagerTest));
    }
    void InvkeIsAds()
    {
        isActionAds = false;
    }

    bool isActionRewardSucess = false;
    void ACTION_REWARD(bool flag)
    {
        if (flag == true)
        {
            isActionRewardSucess = flag;
            OnClosed();
        }
    }


    void OnClickConfirm()
    {
        isComponyNameTag = true;
        OnClosed();
    }


    bool isComponyNameTag = false;
    protected override void OnClosed()
    {
        base.OnClosed();
    }

    protected override void OnDestroied()
    {
        base.OnDestroied();

        /// 광고 완료 후 매니저 다시뽑기
        if (isActionRewardSucess == true)
        {
            Transform _root = PopupManager.Instance.RootPopup;
            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupManagerTest);

        }
        else
        {
            //// 매니저 확인
            if (isComponyNameTag == true)
            {
                Transform _root = PopupManager.Instance.RootPopup;
                //// 최초 1회 실행
                UserInfoManager.Instance.SetSaveGetManager(true);
                SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupManagerLicense, objReturnData);

                if (ACTION_MANAGER_UI != null && ACTION_MANAGER_UI.GetInvocationList().Length > 0)
                {
                    ACTION_MANAGER_UI();
                }
            }
            objReturnData.Initialize();
        }
        JHAdsManager.ACTION_ADS_COMPLETE = null;
    }



    eManager_Type returnManagerType(string fixManager)
    {

        ePersonality_Type _Type = RayUtils.Utils.ConvertEnumData<ePersonality_Type>(fixManager);
        eManager_Type _managerType = eManager_Type.NONE;


        switch (_Type)
        {
            case ePersonality_Type.PERSOLANLITY_TYPE_A: { _managerType = eManager_Type.MANAGER_A; break; }
            case ePersonality_Type.PERSOLANLITY_TYPE_B: { _managerType = eManager_Type.MANAGER_B; break; }
            case ePersonality_Type.PERSOLANLITY_TYPE_C: { _managerType = eManager_Type.MANAGER_C; break; }
            case ePersonality_Type.PERSOLANLITY_TYPE_D: { _managerType = eManager_Type.MANAGER_D; break; }
            case ePersonality_Type.PERSOLANLITY_TYPE_E: { _managerType = eManager_Type.MANAGER_E; break; }
            case ePersonality_Type.PERSOLANLITY_TYPE_F: { _managerType = eManager_Type.MANAGER_F; break; }
        }



        return _managerType;

    }

    IEnumerator SetUIProssce()
    {
        yield return YieldHelper.waitForSeconds(500);
        objTitle.SetActive(true);
        yield return YieldHelper.waitForSeconds(500);
        objLight.SetActive(true);
        yield return YieldHelper.waitForSeconds(500);
        objCharactor.SetActive(true);
        yield return YieldHelper.waitForSeconds(500);
        // objResultBubbleTalk.SetActive(true);
        objParticle.SetActive(true);
        yield return YieldHelper.waitForSeconds(500);
        objGender.SetActive(true);

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
