using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PopupBuyFiaryFail : JHPopup
{
   public UILabel labelTitl;
   public UILabel labelDesc;


    protected override void OnAwake()
    {
        base.OnAwake();

        // PopupBuyFiaryFailTitle,//	구입 실패
        // PopupBuyFiaryFailDesc,	//구입에 실패했습니다.\n보석 보유량 혹은 네트워크 연결을\n확인하고 다시 시도하세요.

        labelTitl.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupBuyFiaryFailTitle);
        labelDesc.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupBuyFiaryFailDesc);
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
