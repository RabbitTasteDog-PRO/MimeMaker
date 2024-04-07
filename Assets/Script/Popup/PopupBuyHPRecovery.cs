using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;
public class PopupBuyHPRecovery : JHPopup
{

    public static Action<bool> ACTION_BUY_RECOVERY_VIDEO;

    public UILabel labelTitle;
    public UILabel labelDesc;

    public UIButton btnVideo;
    public UILabel labelBtnVideo;


    //   PopupBuyHPRecoveryTitle,    //긴급 체력 회복
    //     PopupBuyHPRecoveryDesc, //비상! 체력이 많이 떨어졌습니다.\n지금 광고를 보면\n체력을 10만큼 획복합니다.
    //     ,//	체력 회복

    protected override void OnAwake()
    {
        base.OnAwake();

        labelTitle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupBuyHPRecoveryTitle);
        labelDesc.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupBuyHPRecoveryDesc);
        labelBtnVideo.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupBuyHPRecoveryBtn);


        btnVideo.onClick.Add(new EventDelegate(OnClickVideo));

    }


    protected override void OnDestroied()
    {
        base.OnDestroied();

        if (isActionVideo == true)
        {
            if (ACTION_BUY_RECOVERY_VIDEO != null && ACTION_BUY_RECOVERY_VIDEO.GetInvocationList().Length > 0)
            {
                ACTION_BUY_RECOVERY_VIDEO(isActionVideo);
            }
        }
    }



    protected override void OnClosed()
    {
        base.OnClosed();
    }

    bool isActionVideo = false;
    void OnClickVideo()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        isActionVideo = true;
        OnClosed();
    }
}
