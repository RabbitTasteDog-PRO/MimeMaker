using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;


/**********************************************************************/
/// 매니저 관리용 싱글톤
/***********************************************************************/


public class MyManager
{

    STManagerData managerData; /// 현재 나의 매니저 데이터 
    STManagerSkill skillData; /// 현재 나의 매니저 스킬


    eManager_Type managerType;
    eManager_Skill mangerSkill;
    eSkillGrade managerSkillGrade;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        managerType = RayUtils.Utils.ConvertEnumData<eManager_Type>(UserInfoManager.Instance.GetMyManager());
        mangerSkill = RayUtils.Utils.ConvertEnumData<eManager_Skill>(UserInfoManager.Instance.GetMySkill());
        managerSkillGrade = RayUtils.Utils.ConvertEnumData<eSkillGrade>(UserInfoManager.Instance.GetMySkillGrade());

        Dictionary<eManager_Type, STManagerData> dicManager = SceneBase.Instance.dataManager.GetDicManagerData();
        STManagerData managerData = dicManager[managerType];

        Dictionary<eManager_Skill, Dictionary<eSkillGrade, STManagerSkill>> dicSkill = SceneBase.Instance.dataManager.GetDicManagerSkillData();
        Dictionary<eSkillGrade, STManagerSkill> _dicSkill = dicSkill[mangerSkill];
        STManagerSkill skillData = _dicSkill[managerSkillGrade];
    }





}


public class JHManagerManger : Ray_Singleton<JHManagerManger>
{
    public delegate void MANAGER_REFRASH();
    public event MANAGER_REFRASH RefrashManager;



    public void mangerRefrash()
    {
        if (RefrashManager != null)
        {
            RefrashManager();
        }
    }

    public void SetMyManager(eManager_Type managerType, eManager_Skill mangerSkill, eSkillGrade managerSkillGrade)
    {
        UserInfoManager.Instance.SetMyManager(managerType.ToString());
        UserInfoManager.Instance.SetMySkill(mangerSkill.ToString());
        UserInfoManager.Instance.SetMySkillGrade(managerSkillGrade.ToString());
    }



    public STManagerData GetMyManager()
    {
        eManager_Type managerType = RayUtils.Utils.ConvertEnumData<eManager_Type>(UserInfoManager.Instance.GetMyManager());

        Dictionary<eManager_Type, STManagerData> dicManager = SceneBase.Instance.dataManager.GetDicManagerData();
        STManagerData managerData = dicManager[managerType];

        return managerData;
    }

    public STManagerSkill GetMyManagerSkill()
    {
        eManager_Skill mangerSkill = RayUtils.Utils.ConvertEnumData<eManager_Skill>(UserInfoManager.Instance.GetMySkill());
        eSkillGrade managerSkillGrade = RayUtils.Utils.ConvertEnumData<eSkillGrade>(UserInfoManager.Instance.GetMySkillGrade());

        Dictionary<eManager_Skill, Dictionary<eSkillGrade, STManagerSkill>> dicSkill = SceneBase.Instance.dataManager.GetDicManagerSkillData();
        Dictionary<eSkillGrade, STManagerSkill> _dicSkill = dicSkill[mangerSkill];
        STManagerSkill skillData = _dicSkill[managerSkillGrade];

        return skillData;
    }



}
