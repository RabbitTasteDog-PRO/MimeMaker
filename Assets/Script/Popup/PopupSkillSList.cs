using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
public class PopupSkillSList : JHPopup
{

    public PrefabItemSkillS _prefabItemSkillS;
    public UIScrollView scrollItem;

    public UILabel labelBuySkillSTitle;
    public UIGrid gridSkill;

    List<PrefabItemSkillS> listPrefab;


    protected override void OnAwake()
    {
        base.OnAwake();

        if (gridSkill.transform.childCount > 0)
        {
            gridSkill.transform.DestroyChildren();
        }

        PopupSkillSBuy.ACTION_BUY_SKILL_COMPLETE = ACTION_BUY_SKILL_COMPLETE;

    }

    protected override void OnDestroied()
    {
        base.OnDestroied();
        PopupSkillSBuy.ACTION_BUY_SKILL_COMPLETE = null;
        if (isComplete == true)
        {
            Transform _root = PopupManager.Instance.RootPopup;
            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupSkillSBuyConfirm, skillData);
        }
    }

    protected override void OnClosed()
    {
        base.OnClosed();
    }

    protected override void OnStart()
    {
        base.OnStart();
        scrollItem.panel.depth = GetDepth() + 1;
        SetBuySkillUI();
    }

    void SetBuySkillUI()
    {
        Dictionary<eManager_Skill, Dictionary<eSkillGrade, STManagerSkill>> _dic = SceneBase.Instance.dataManager.GetDicManagerSkillData();

        if (listPrefab == null)
        {
            listPrefab = new List<PrefabItemSkillS>();
            for (int i = 0; i < _dic.Count; i++)
            {
                if (i == (int)eManager_Skill.SKILL_F)
                {
                    continue;
                }

                PrefabItemSkillS _prefab = Instantiate(_prefabItemSkillS, gridSkill.transform) as PrefabItemSkillS;
                _prefab.transform.localPosition = Vector2.zero;
                _prefab.transform.localScale = Vector3.one;

                Dictionary<eSkillGrade, STManagerSkill> _tempDic = _dic[(eManager_Skill)i];
                STManagerSkill skillData = _tempDic[eSkillGrade.S];

                _prefab.SetItemSkillUI(skillData);
                listPrefab.Add(_prefab);
            }
        }
        else
        {
            for (int i = 0; i < listPrefab.Count; i++)
            {
                Dictionary<eSkillGrade, STManagerSkill> _tempDic = _dic[(eManager_Skill)i];
                STManagerSkill skillData = _tempDic[eSkillGrade.S];

                listPrefab[i].SetItemSkillUI(skillData);
            }
        }

        gridSkill.Reposition();
    }

    bool isComplete = false;
    STManagerSkill skillData;
    void ACTION_BUY_SKILL_COMPLETE(STManagerSkill _skillData, bool flag)
    {
        if (flag == true)
        {
            isComplete = flag;
            skillData = _skillData;
            OnClosed();

        }
    }

}
