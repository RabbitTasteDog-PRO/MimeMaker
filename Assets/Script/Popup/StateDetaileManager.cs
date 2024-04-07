using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class StateDetaileManager : MonoBehaviour
{
    [Header("Manager")]
    public UILabel labelNameManager; // 매니저 
    public UI2DSprite spriteManager;
    public UILabel labelCareer; // 경력
    public UILabel labelCareerPoint;
    public UILabel labelRevenu; // 수익
    public UILabel labelRevenuPoint;

    [Header("Skill")]
    public UILabel labelSkill; // SKILL
    public UILabel labelSkillName;
    public UILabel labelSkillInfo;
    public UILabel labelSkillDesc;
    public UISprite spriteGrade;

    [Header("Video ReTry")]
    public UIButton btnVidoeReTry;
    public UILabel labelBtnVidoeReTry;



    public UIGrid gridManagerItem;
    public UIButton[] btnMangaerItem;

    public UI2DSprite[] spriteItemImg;

    /// manager_1_172x268 <= 여자
    /// character_manager_a_180x280 <= 남자    

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {


        bool isManager = UserInfoManager.Instance.GetSaveManagerGender();

        labelRevenu.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eRevenu);

        string _path = "Image/Character/{0}";

        spriteManager.sprite2D = isManager == false ? Resources.Load<Sprite>(string.Format(_path, "character_manager_a_180x280"))
        : Resources.Load<Sprite>(string.Format(_path, "manager_1_172x268"));
        /// ACTION_SKILL_CHAGE

        for (int i = 0; i < btnMangaerItem.Length; i++)
        {
            EventDelegate del = new EventDelegate(this, "OnClickItemInfo");
            EventDelegate.Parameter parm = new EventDelegate.Parameter();
            parm.value = (eManagerItemAbility)i;
            parm.expectedType = typeof(eManagerItemAbility);
            del.parameters[0] = parm;
            EventDelegate.Add(btnMangaerItem[i].onClick, del);
        }

        btnVidoeReTry.onClick.Add(new EventDelegate(OnClickVideoRetry));
        labelBtnVidoeReTry.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eBtnVideoRetry);
        /// 매니저 스킬
        SetManagerUI();
        /// 매니저 장비
        SetManagerEquipUI();
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        PopupItemBuy.ACTION_ITEM_REFRASH = SetManagerEquipUI;
        PopupAdMobStart.ACTION_CLOSE_ADMOB_START_POPUP = ACTION_CLOSE_ADMOB_START_POPUP;
        JHAdsManager.ACTION_ADS_COMPLETE = ACTION_ADMOB_SUCESS;
        PopupSkillVideoSelect.ACTION_SKILL_VIDEO_CHANGE = ACTION_SKILL_VIDEO_CHANGE;
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {

        PopupAdMobStart.ACTION_CLOSE_ADMOB_START_POPUP = null;
        JHAdsManager.ACTION_ADS_COMPLETE = null;
        PopupItemBuy.ACTION_ITEM_REFRASH = null;
        PopupSkillVideoSelect.ACTION_SKILL_VIDEO_CHANGE = null;
    }

    public void SetManagerUI()
    {
        //// 주차 보상 결과 계산
        Dictionary<int, int> dicWeekReward = SceneBase.Instance.dataManager.GetDicWeekResultGold();
        int _endCount = (UserInfoManager.Instance.GetEndingCount() * 7);
        int week = Mathf.Max(0, UserInfoManager.Instance.GetWeeklyCount() - 1);
        int counting = (((_endCount * week) + 7) <= 7) ? week : ((_endCount * week) + 7);
        int rewardGold = dicWeekReward[counting];
        labelRevenuPoint.text = rewardGold.ToString("N0");
        Debug.LogError("RewardGold : " + rewardGold);


        int crrWeekCount = UserInfoManager.Instance.GetWeeklyCount();
        labelCareerPoint.text = crrWeekCount.ToString("N0") + "주차";

        /// TODO
        /// 경력 및 수익 추가 해야함 

        string manger = UserInfoManager.Instance.GetMyManager();
        string managerGrade = UserInfoManager.Instance.GetMySkillGrade();
        string mySkill = UserInfoManager.Instance.GetMySkill();

        eManager_Type eManager = RayUtils.Utils.ConvertEnumData<eManager_Type>(manger);
        eManager_Skill eSkillType = RayUtils.Utils.ConvertEnumData<eManager_Skill>(mySkill);


        eSkillGrade eGrade = RayUtils.Utils.ConvertEnumData<eSkillGrade>(managerGrade);
        spriteGrade.spriteName = ConverGradeIcon(eGrade);

        Dictionary<eManager_Type, STManagerData> dicManager = SceneBase.Instance.dataManager.GetDicManagerData();
        STManagerData managerData = dicManager[eManager];

        Dictionary<eManager_Skill, Dictionary<eSkillGrade, STManagerSkill>> dicSkill = SceneBase.Instance.dataManager.GetDicManagerSkillData();
        Dictionary<eSkillGrade, STManagerSkill> _dicSkill = dicSkill[eSkillType];
        STManagerSkill skillData = _dicSkill[eGrade];


        eGlobalTextKey _skillName = skillData.keySkilllName;
        eGlobalTextKey info_0 = skillData.info_0;
        eGlobalTextKey info_1 = skillData.info_1;
        eGlobalTextKey info_value = skillData.info_value;

        float prob = Mathf.Abs(skillData.probability - 100.0f);
        // float prob = skillData.probability;
        float pfobValue = skillData.probabilityValue;
        string skillDesc = "";
        if (skillData.probability <= 0)
        {
            skillDesc = string.Format(SceneBase.Instance.dataManager.GetDicGlobalTextData(info_value), pfobValue);
        }
        else
        {

            if (skillData.skillType == Enums.eSkillType.eEVENT)
            {
                skillDesc = string.Format(SceneBase.Instance.dataManager.GetDicGlobalTextData(info_value), pfobValue);
            }
            else
            {
                if (eSkillType.ToString().Contains("eSKILL_F_"))
                {
                    skillDesc = string.Format(SceneBase.Instance.dataManager.GetDicGlobalTextData(info_value), pfobValue);
                }
                else
                {
                    skillDesc = string.Format(SceneBase.Instance.dataManager.GetDicGlobalTextData(info_value), prob, pfobValue);
                }
            }

        }
        string strSkillName = SceneBase.Instance.dataManager.GetDicGlobalTextData(_skillName);
        string strSkillInfo_0 = SceneBase.Instance.dataManager.GetDicGlobalTextData(info_0);


        labelSkillName.text = strSkillName;//SceneBase.Instance.dataManager.GetDicGlobalTextData(RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(skillData.skillName));
        labelSkillInfo.text = strSkillInfo_0;//SceneBase.Instance.dataManager.GetDicGlobalTextData(info_0);
        labelSkillDesc.text = skillDesc;

        for (int i = 0; i < (int)eManagerItemAbility.COUNT; i++)
        {
            string euipItem = UserInfoManager.Instance.GetEquipManagerItem(((eManagerItemAbility)i).ToString());
            if (string.IsNullOrEmpty(euipItem) == true)
            {
                spriteItemImg[i].sprite2D = null;
                spriteItemImg[i].gameObject.SetActive(false);
            }
            else
            {
                eManagerItem _mngItem = RayUtils.Utils.ConvertEnumData<eManagerItem>(euipItem);
                STManagerItemData _data = SceneBase.Instance.dataManager.GetManagerItemData(_mngItem);
                string icon = string.Format("Image/ManagerItem/{0}", _data.itemImage);
                spriteItemImg[i].sprite2D = Resources.Load<Sprite>(icon);
                spriteItemImg[i].gameObject.SetActive(true);
            }

        }

    }

    void SetManagerEquipUI()
    {
        for (int i = 0; i < (int)eManagerItemAbility.COUNT; i++)
        {
            string euipItem = UserInfoManager.Instance.GetEquipManagerItem(((eManagerItemAbility)i).ToString());
            if (string.IsNullOrEmpty(euipItem) == true)
            {
                spriteItemImg[i].sprite2D = null;
                spriteItemImg[i].gameObject.SetActive(false);
            }
            else
            {
                eManagerItem _mngItem = RayUtils.Utils.ConvertEnumData<eManagerItem>(euipItem);
                STManagerItemData _data = SceneBase.Instance.dataManager.GetManagerItemData(_mngItem);
                string icon = string.Format("Image/ManagerItem/{0}", _data.itemImage);
                spriteItemImg[i].sprite2D = Resources.Load<Sprite>(icon);
                spriteItemImg[i].gameObject.SetActive(true);
            }

        }
    }

    void OnClickItemInfo(eManagerItemAbility _ability)
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        if (spriteItemImg[(int)_ability].sprite2D == null)
        {
            return;
        }


        string euipItem = UserInfoManager.Instance.GetEquipManagerItem(_ability.ToString());
        // //// 아이템 정보 보여주기 팝업
        eManagerItem _mngItem = RayUtils.Utils.ConvertEnumData<eManagerItem>(euipItem);
        STManagerItemData _data = SceneBase.Instance.dataManager.GetManagerItemData(_mngItem);

        // object _infoData = _mngItem;
        // SceneBase.Instance.AddPopup(_trans, ePopupLayer.PopupItemInfo, _infoData);

        Transform _trans = PopupManager.Instance.RootPopup;
        object[] _dataItem = new object[2];
        _dataItem[0] = _ability;
        _dataItem[1] = _data.eItem;
        // 아이템 구매 팝업 띄움
        SceneBase.Instance.AddPopup(_trans, Enums.ePopupLayer.PopupItemBuy, _dataItem);
    }

    void OnClickVideoRetry()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);
        
        Transform _root = PopupManager.Instance.RootPopup;
        SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupSkillVideoSelect);

        // Transform _root = PopupManager.Instance.RootPopup;
        // STAdMobStartTextData data = new STAdMobStartTextData();

        // data.strTitle = eGlobalTextKey.eSkillChanageTitle.ToString();
        // data.strDesc = eGlobalTextKey.eSkillChagneMessage.ToString();
        // data.strDescSub = eGlobalTextKey.eSkillChangeMessageDesc.ToString();
        // data.strBtnText = eGlobalTextKey.eBtnSkillCnage.ToString();

        // //// 스킬변경
        // data.vidoeKey = eVideoAds.eVideoManagerSkill.ToString();

        // SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupAdMobStart, data);

    }
    string textSkillName = "";
    ///<summary>
    /// 비디오 광고 시청시 스킬 변경
    /// TODO : C ~ S 등급 확률 물어봐야함 
    ///</summary>
    void SkillChanage()
    {
        eManager_Type managerType = RayUtils.Utils.ConvertEnumData<eManager_Type>(UserInfoManager.Instance.GetMyManager());
        eManager_Skill mangerSkill = eManager_Skill.NONE;
        eSkillGrade managerSkillGrade = eSkillGrade.NONE;

        mangerSkill = (eManager_Skill)Random.Range((int)eManager_Skill.SKILL_A, (int)eManager_Skill.SKILL_F);

        //// 재능 발굴일경우 랜덤스킬중 하나이기 때문에 따로 관리
        if (mangerSkill == eManager_Skill.SKILL_F)
        {
            int randomSkill = Random.Range((int)eManager_Skill.SKILL_F_A, ((int)eManager_Skill.COUNT - 1));
            mangerSkill = (eManager_Skill)randomSkill;
            // managerSkillGrade = Random.Range(0, 100) >= 90 ? eSkillGrade.B : eSkillGrade.C;
            managerSkillGrade = skillGrade();
        }
        else
        {
            // managerSkillGrade = Random.Range(0, 100) >= 90 ? eSkillGrade.B : eSkillGrade.C;
            managerSkillGrade = skillGrade();
        }

        Dictionary<eManager_Skill, Dictionary<eSkillGrade, STManagerSkill>> dicSkill = SceneBase.Instance.dataManager.GetDicManagerSkillData();
        Dictionary<eSkillGrade, STManagerSkill> _dicSkill = dicSkill[mangerSkill];
        STManagerSkill skillData = _dicSkill[managerSkillGrade];

        // //// 매니저 스킬 변경 텍스트
        eGlobalTextKey _eSkillName = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(skillData.skillName);
        textSkillName = SceneBase.Instance.dataManager.GetDicGlobalTextData(_eSkillName);
        JHManagerManger.Instance.SetMyManager(managerType, mangerSkill, managerSkillGrade);


    }

    void ACTION_SKILL_VIDEO_CHANGE(bool flag)
    {
        if (flag == true)
        {
            // StartCoroutine(SceneBase.Instance.adsManager.IEShowAdMobVideo(eVideoAds.eVideoManagerSkill));
            SceneBase.Instance.adsManager.AsyncShowAdMobVideo(eVideoAds.eVideoManagerSkill);
            if (JHAdsManager.ACTION_ADS_COMPLETE.GetInvocationList().Length > 0)
            {
                JHAdsManager.ACTION_ADS_COMPLETE = null;
            }
            JHAdsManager.ACTION_ADS_COMPLETE = ACTION_ADMOB_SUCESS;
        }
    }


    void ACTION_CLOSE_ADMOB_START_POPUP(bool flag)
    {
        Debug.LogError("1111111 CTION_CLOSE_ADMOB_START_POPUP : " + flag);
        if (flag == true)
        {
            Debug.LogError("222222 ACTION_CLOSE_ADMOB_START_POPUP : " + flag);
            // StartCoroutine(SceneBase.Instance.adsManager.IEShowAdMobVideo(eVideoAds.eVideoManagerSkill));
            SceneBase.Instance.adsManager.AsyncShowAdMobVideo(eVideoAds.eVideoManagerSkill);
            if (JHAdsManager.ACTION_ADS_COMPLETE.GetInvocationList().Length > 0)
            {
                JHAdsManager.ACTION_ADS_COMPLETE = null;
                Debug.LogError("33333 ACTION_CLOSE_ADMOB_START_POPUP : " + flag);
            }
            JHAdsManager.ACTION_ADS_COMPLETE = ACTION_ADMOB_SUCESS;
            Debug.LogError("44444 ACTION_CLOSE_ADMOB_START_POPUP : " + flag);
        }
    }
    string skillDesc = "";
    /// 광고 시청 완료시 스킬 변경
    async void ACTION_ADMOB_SUCESS(bool flag)
    {
        Debug.LogError("111111111  ACTION_ADMOB_SUCESS : " + flag);
        if (flag == true)
        {
            SkillChanage();
            SetManagerUI();

            Debug.LogError("222222222222  ACTION_ADMOB_SUCESS : " + flag);
            await System.Threading.Tasks.Task.Delay(300);
        }
    }


    ///<summary>
    /// 매니저 등급 확정
    ///</summary>
    eSkillGrade skillGrade()
    {
        //// C : 70%	B : 25%	 A: 4.5%	S: 0.5%
        eSkillGrade grade = eSkillGrade.NONE;

        float target = UnityEngine.Random.Range(0.0f, 100.0f); // traget과 가까운 값

        if (target >= 99.5f)
        {
            grade = eSkillGrade.S;
        }
        else if (target < 99.4f && target >= 95.5f)
        {
            grade = eSkillGrade.A;
        }
        else if (target < 95.4f && target >= 75.0f)
        {
            grade = eSkillGrade.B;
        }
        else
        {
            grade = eSkillGrade.C;
        }
        return grade;
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
