using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupResetCheck : JHPopup
{
    public UILabel labelTitle;
    public UILabel labelDesc;
    public UIButton btnResetChecek;
    public UILabel labelBtnResetCheck;
    

    public static System.Action<bool> ACTION_RESET_CHECK;

    protected override void OnAwake()
    {
        base.OnAwake();

        labelTitle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.PopupResetCheckTitle);
        labelDesc.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.PopupResetCheckDesc);
        labelBtnResetCheck.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.PopupResetCheckBtnCheck);

        btnResetChecek.onClick.Add(new EventDelegate(OnClickClose));

    }



    protected override void OnDestroied()
    {
        base.OnDestroied();

        if(ACTION_RESET_CHECK != null)
        {
            ACTION_RESET_CHECK(isActionReset);
        }
    }



    protected override void OnClosed()
    {
        base.OnClosed();
    }

    bool isActionReset = false;
    void OnClickClose()
    {
        SceneBase.Instance.PLAY_SFX(Enums.eSFX.SFX_4_touch1);

        isActionReset = true;
        OnClosed();
    }




}
