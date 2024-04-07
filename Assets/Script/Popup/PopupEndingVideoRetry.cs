using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;

public class PopupEndingVideoRetry : JHPopup
{
    
    public static Action<bool> ACTION_ENDING_VIDEO_RETRY;


    public UILabel labelTitle;
    public UILabel labelDesc;

    public UIButton btnCardChanage;
    public UILabel labelBtnCardChange;

    int price;
    protected override void OnAwake()
    {
        base.OnAwake();

        //  PopupEndingVideoRetryTitle,//	다시하기-스텟유지
        // PopupEndingVideoRetryDesc,//	지금 광고를 보면 10%의 능력치를\n유지한 상태로\n다시 시작할 수 있습니다.

        labelTitle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupEndingVideoRetryTitle);
        labelDesc.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupEndingVideoRetryDesc);
        labelBtnCardChange.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupEndingVideoRetryBtn);


        btnCardChanage.onClick.Add(new EventDelegate(OnClickCardChange));
    }

    protected override void OnClosed()
    {
        base.OnClosed();
    }



    protected override void OnDestroied()
    {
        base.OnDestroied();
        if (ACTION_ENDING_VIDEO_RETRY != null && ACTION_ENDING_VIDEO_RETRY.GetInvocationList().Length > 0)
        {
            ACTION_ENDING_VIDEO_RETRY(isActionChanage);
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
