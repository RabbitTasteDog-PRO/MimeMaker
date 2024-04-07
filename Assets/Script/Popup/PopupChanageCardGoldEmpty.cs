using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;

public class PopupChanageCardGoldEmpty : JHPopup
{

    public UILabel labelTitle;
    public UILabel labelDesc;


    int price;
    protected override void OnAwake()
    {
        base.OnAwake();

        labelTitle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupChanageCardTitle);
        labelDesc.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupChanageCardCoinEmpty);

         //  PopupChanageCardTitle,//	카드 바꾸기
        // PopupChanageCardDesc,   //코인을 소모하여\n카드를 바꾸겠습니까?
        // PopupChanageCardBtn,    //카드 바꾸기
        // PopupChanageCardCoinEmpty,  //코인이 충분하지 않습니다.\n코인을 획득한 후 다시 시도하세요.
        // PopupChanageCardAds,	//광고를 보면 남은 카드 중\n1장을 다시 뽑을 수 있습니다.

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
