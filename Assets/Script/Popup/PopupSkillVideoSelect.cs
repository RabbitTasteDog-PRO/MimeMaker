using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;

public class PopupSkillVideoSelect : JHPopup
{
    
    public static Action<bool> ACTION_SKILL_VIDEO_CHANGE;


    public UILabel labelTitle;
    public UILabel labelDesc;
    public UILabel labelDescSub;

    public UIButton btnCardChanage;
    public UILabel labelBtnCardChange;

    int price;
    protected override void OnAwake()
    {
        base.OnAwake();

        //  PopupSkillVideoSelectTitle, //스킬 다시선택
        // PopupSkillVideoSelectDesc,  //광고를 보면 매니저 스킬을\n다시 선택할 수 있습니다.
        // PopupSkillVideoSelectDescSub,   //스킬을 다시 선택하면 이전\n스킬을 되돌릴 수 없습니다.
        // PopupSkillVideoSelectBtn,	//다시선택


        labelTitle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupSkillVideoSelectTitle);
        labelDesc.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupSkillVideoSelectDesc);
        labelDescSub.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupSkillVideoSelectDescSub);
        labelBtnCardChange.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupSkillVideoSelectBtn);


        btnCardChanage.onClick.Add(new EventDelegate(OnClickCardChange));
    }

    protected override void OnClosed()
    {
        base.OnClosed();
    }



    protected override void OnDestroied()
    {
        base.OnDestroied();
        if (ACTION_SKILL_VIDEO_CHANGE != null && ACTION_SKILL_VIDEO_CHANGE.GetInvocationList().Length > 0)
        {
            ACTION_SKILL_VIDEO_CHANGE(isActionChanage);
        }
    }

    bool isActionChanage = false;
    void OnClickCardChange()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);
        
        isActionChanage = true;
        OnClosed();

    }

}
