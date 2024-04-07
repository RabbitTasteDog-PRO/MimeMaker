using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;
public class PopupChanageCardVideo : JHPopup
{
    
    public static Action<bool> ACTION_CARD_VIDEO_CHANGE;


    public UILabel labelTitle;
    public UILabel labelDesc;

    public UIButton btnCardChanage;
    public UILabel labelBtnCardChange;

    int price;
    protected override void OnAwake()
    {
        base.OnAwake();


        labelTitle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupChanageCardTitle);
        labelDesc.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupChanageCardAds);
        labelBtnCardChange.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupChanageCardAdsBtn);


        btnCardChanage.onClick.Add(new EventDelegate(OnClickCardChange));
    }

    protected override void OnClosed()
    {
        base.OnClosed();
    }



    protected override void OnDestroied()
    {
        base.OnDestroied();
        if (ACTION_CARD_VIDEO_CHANGE != null && ACTION_CARD_VIDEO_CHANGE.GetInvocationList().Length > 0)
        {
            ACTION_CARD_VIDEO_CHANGE(isActionChanage);
        }
    }

    bool isActionChanage = false;
    void OnClickCardChange()
    {
        isActionChanage = true;
        OnClosed();

    }

}
