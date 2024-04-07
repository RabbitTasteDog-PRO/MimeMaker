using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;
public class PopupChanageCardGold : JHPopup
{

    public static Action<bool> ACTION_CARD_CHANGE;


    public UILabel labelTitle;
    public UILabel labelDesc;

    public UIButton btnCardChanage;
    public UILabel labelBtnCardChange;
    public UILabel labelBtnCardChangePrice;

    int price;
    protected override void OnAwake()
    {
        base.OnAwake();

        //  PopupChanageCardTitle,//	카드 바꾸기
        // PopupChanageCardDesc,   //코인을 소모하여\n카드를 바꾸겠습니까?
        // PopupChanageCardBtn,    //카드 바꾸기
        // PopupChanageCardCoinEmpty,  //코인이 충분하지 않습니다.\n코인을 획득한 후 다시 시도하세요.
        // PopupChanageCardAds,	//광고를 보면 남은 카드 중\n1장을 다시 뽑을 수 있습니다.

        labelTitle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupChanageCardTitle);
        labelDesc.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupChanageCardDesc);
        labelBtnCardChange.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.PopupChanageCardBtn);


        btnCardChanage.onClick.Add(new EventDelegate(OnClickCardChange));
    }

    public override void SetData(object _data)
    {
        base.SetData();
        if (_data != null)
        {
            price = (int)_data;
            labelBtnCardChangePrice.text = string.Format("-{0:00}", price);
        }
    }

    protected override void OnClosed()
    {
        base.OnClosed();
    }



    protected override void OnDestroied()
    {
        base.OnDestroied();
        if (ACTION_CARD_CHANGE != null && ACTION_CARD_CHANGE.GetInvocationList().Length > 0)
        {
            ACTION_CARD_CHANGE(isActionChanage);
        }
    }

    bool isActionChanage = false;
    void OnClickCardChange()
    {

        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);
        
        int gold = int.Parse(UserInfoManager.Instance.GetGold());

        if (price > gold)
        {
            Transform _root = PopupManager.Instance.RootPopup;
            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupChanageCardGoldEmpty);
        }
        else
        {
            isActionChanage = true;
            UserInfoManager.Instance.SetGold((gold - price).ToString());
            OnClosed();
        }

    }

}
