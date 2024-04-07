using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;



public class PrefabItemSkillS : MonoBehaviour
{
    public UILabel labelSkillTitle;
    public UILabel labelSkillDesc;

    public UIButton btnBuySkill;
    public UILabel labelBtnBuySkill;
    public UISprite spriteGrade;

    STManagerSkill skillData;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (UserInfoManager.Instance.GetSaveInappManagerSKill(skillData.managerSkill.ToString()) == false)
        {
            btnBuySkill.onClick.Add(new EventDelegate(OnClickBuySkill));
        }
        else
        {
            btnBuySkill.onClick.Add(new EventDelegate(OnClickEquipSkill));
        }
        labelBtnBuySkill.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.ePopupBuySkillSBtnSelect);

    }


    public void SetItemSkillUI(STManagerSkill _skillData)
    {
        skillData = _skillData;
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

        labelSkillTitle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(_skillName);
        // if(labelSkillTitle.text.Length > 8)
        // {
        //     UISprite _grade = labelSkillTitle.transform.GetChild(0).gameObject.GetComponent<UISprite>();
        // }
        labelSkillDesc.text = skillDesc;

        if (UserInfoManager.Instance.GetSaveInappManagerSKill(skillData.managerSkill.ToString()) == false)
        {
            if (btnBuySkill.onClick != null)
            {
                btnBuySkill.onClick.Clear();
            }
            btnBuySkill.onClick.Add(new EventDelegate(OnClickBuySkill));
        }
        else
        {
            if (btnBuySkill.onClick != null)
            {
                btnBuySkill.onClick.Clear();
            }
            btnBuySkill.onClick.Add(new EventDelegate(OnClickEquipSkill));
        }
    }



    void OnClickBuySkill()
    {
        //// 테스트용
        Transform _root = PopupManager.Instance.RootPopup;
        SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupSkillSBuy, skillData);
    }

    void OnClickEquipSkill()
    {
        //// 스킬변경 토스트 매세지

        UserInfoManager.Instance.SetMySkill(skillData.managerSkill.ToString());
        UserInfoManager.Instance.SetMySkillGrade(eSkillGrade.S.ToString());

    }


}
