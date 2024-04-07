using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
public class PopupAdsErrorNetwork : JHPopup
{
    public UILabel labelTitle;
    public UILabel labelDesc;

    protected override void OnAwake()
    {
        base.OnAwake();

        PopupManager.Instance.objNoneTouchBlock.SetActive(false);

        labelTitle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupAdsErrorNetworkTitle);
        labelDesc.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupAdsErrorNetworkDesc);
    }


    protected override void OnClosed()
    {
        base.OnClosed();
    }

    protected override void OnDestroied()
    {
        base.OnDestroied();
    }








}
