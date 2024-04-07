using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PopupSkillSBuyConfirm : JHPopup
{

    public UILabel labelSkillConfirmTitle;
    public UILabel labelSkillConfirmDesc;

    public UISprite spriteType;

    public UILabel labelSkillName;
    public UISprite spriteSkillGrade;
    public UILabel labelSkillesc;

    public UIButton btnConfirm;
    public UILabel labelBtnConfirm;


    protected override void OnAwake()
    {
        base.OnAwake();

        labelSkillConfirmTitle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.ePopupBuySkillConfirmTitle);
        labelSkillConfirmDesc.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.ePopupBuySkillConfirmDesc);
        labelBtnConfirm.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eCheck);
        btnConfirm.onClick.Add(new EventDelegate(OnClosed));

    }

    protected override void OnDestroied()
    {
        base.OnDestroied();

    }

    protected override void OnClosed()
    {
        base.OnClosed();
    }
    STManagerSkill skillData;
    public override void SetData(object[] _data)
    {
        if (_data != null)
        {
            string _type = (string)_data[0];
            //// eStoreTitle_Select_Skill_S <= 스킬선택 
            //// 스킬랜덤
            if(_type.Equals("SELECT"))
            {

            }
            else
            {
                
                SceneBase.Instance.PLAY_SFX(eSFX.SFX_28_item);
            }


            skillData = (STManagerSkill)_data[1];
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
                    skillDesc = string.Format(SceneBase.Instance.dataManager.GetDicGlobalTextData(info_value), prob, pfobValue);
                }

            }

            labelSkillName.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(_skillName);
            if (labelSkillName.text.Length > 8)
            {
                Vector2 pos = spriteSkillGrade.transform.localPosition;
                pos.x += 10;
                spriteSkillGrade.transform.localPosition = pos;
            }

            spriteSkillGrade.spriteName = ConvertSkillGradeSprite(UserInfoManager.Instance.GetMySkillGrade());
            labelSkillesc.text = skillDesc;

        }
    }



    string ConvertSkillGradeSprite(string grage)
    {
        eSkillGrade _garade = RayUtils.Utils.ConvertEnumData<eSkillGrade>(grage);
        string AAAA = "";
        switch (_garade)
        {
            case eSkillGrade.C: { AAAA = "level_c_48x48"; break; }
            case eSkillGrade.B: { AAAA = "level_b_48x48"; break; }
            case eSkillGrade.A: { AAAA = "level_a_48x48"; break; }
            case eSkillGrade.S: { AAAA = "level_s_48x48"; break; }
        }

        return AAAA;
    }


}