using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Enums;

public class PopupManagerSkillDetail : JHPopup
{
    public UILabel labelTitle;
    public UILabel labelSkillExplain;
    public UILabel labelSkillExplainDetail;

    public UIButton btnClosed;
    public UILabel labelBtnClosed;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void OnAwake()
    {
        base.OnAwake();
        btnClosed.onClick.Add(new EventDelegate(OnClosed));
        labelBtnClosed.text =
        SceneBase.Instance.dataManager.GetDicGlobalTextData(RayUtils.Utils.ConvertEnumData<eGlobalTextKey>("eCheck"));

        string manger = UserInfoManager.Instance.GetMyManager();
        string managerGrade = UserInfoManager.Instance.GetMySkillGrade();
        string mySkill = UserInfoManager.Instance.GetMySkill();

        eManager_Type eManager = RayUtils.Utils.ConvertEnumData<eManager_Type>(manger);
        eManager_Skill eSkillType = RayUtils.Utils.ConvertEnumData<eManager_Skill>(mySkill);
        eSkillGrade eGrade = RayUtils.Utils.ConvertEnumData<eSkillGrade>(managerGrade);

        Dictionary<eManager_Type, STManagerData> dicManager = SceneBase.Instance.dataManager.GetDicManagerData();
        STManagerData managerData = dicManager[eManager];

        Dictionary<eManager_Skill, Dictionary<eSkillGrade, STManagerSkill>> dicSkill = SceneBase.Instance.dataManager.GetDicManagerSkillData();
        Dictionary<eSkillGrade, STManagerSkill> _dicSkill = dicSkill[eSkillType];
        STManagerSkill skillData = _dicSkill[eGrade];

        SceneBase.Instance.dataManager.GetDicGlobalTextData(RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(skillData.skillName));


        eGlobalTextKey info_0 = skillData.info_0;
        eGlobalTextKey info_1 = skillData.info_1;
        eGlobalTextKey info_value = skillData.info_value;

        float prob = 100 - skillData.probability;
        float pfobValue = skillData.probabilityValue; ;

        string skillDesc = "";
        if (prob <= 0)
        {
            skillDesc = string.Format(SceneBase.Instance.dataManager.GetDicGlobalTextData(info_value), pfobValue);
        }
        else
        {
            //// 이벤트일땐 {0} 하나밖에 없음 예외처리
            if (skillData.skillType == Enums.eSkillType.eEVENT)
            {
                skillDesc = string.Format(SceneBase.Instance.dataManager.GetDicGlobalTextData(info_value), pfobValue);
            }
            else
            {
                if(eSkillType.ToString().Contains("eSKILL_F_"))
                {
                    skillDesc = string.Format(SceneBase.Instance.dataManager.GetDicGlobalTextData(info_value), pfobValue);
                }
                else
                {
                    skillDesc = string.Format(SceneBase.Instance.dataManager.GetDicGlobalTextData(info_value), prob, pfobValue);
                }

            }
        }


        labelTitle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(skillData.skillName));
        labelSkillExplain.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(info_0) + "\n" + SceneBase.Instance.dataManager.GetDicGlobalTextData(info_1);
        labelSkillExplainDetail.text = skillDesc;


    }

    protected override void OnClosed()
    {
        base.OnClosed();
    }


}
