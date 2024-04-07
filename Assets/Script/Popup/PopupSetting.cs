using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PopupSetting : JHPopup
{

    public static System.Action<bool> ACTION_RESET;

    public UIButton btnSound;
    public UILabel labelBtnSound; /// 사운드

    public UIButton btnPush;
    public UILabel labelBtnPush; /// 푸시 알림

    public UIButton btnGameInfo;
    public UILabel labelBtnGameInfo; /// 게임정보

    public UIButton btnBackUp;
    public UILabel labelBtnBackUp; /// 데이터 백업 


    public UIButton btnReset;
    public UILabel labelBtnReset; /// 게임 리셋

    public UIButton btnEnding;
    public UILabel labelBtnEnding; /// 엔딩 노트 



    protected override void OnAwake()
    {
        base.OnAwake();

        PopupResetCheck.ACTION_RESET_CHECK = ACTION_RESET_CHECK;

        // btnBackUp.gameObject.SetActive(false);
        // btnPush.gameObject.SetActive(false);
        // btnGameInfo.gameObject.SetActive(false);

        btnSound.onClick.Add(new EventDelegate(OnClickSound));
        btnPush.onClick.Add(new EventDelegate(OnClickPush));
        btnGameInfo.onClick.Add(new EventDelegate(OnClickGameInfo));
        btnBackUp.onClick.Add(new EventDelegate(OnClickBackUp));
        btnReset.onClick.Add(new EventDelegate(OnClickReset));
        btnEnding.onClick.Add(new EventDelegate(OnClickEnding));
    }


    public override void SetData()
    {
        base.SetData();
    }


    protected override void OnClosed()
    {
        base.OnClosed();
    }

    protected override void OnDestroied()
    {
        base.OnDestroied();

        if (ACTION_RESET != null)
        {
            ACTION_RESET(isActionReset);
        }
    }

    void OnClickSound()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        Transform _tran = PopupManager.Instance.RootPopup;
        SceneBase.Instance.AddPopup(_tran, Enums.ePopupLayer.PopupOptionSound);
    }

    void OnClickPush()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        Transform _tran = PopupManager.Instance.RootPopup;
        string _messgae = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eTextUpdate);
        SceneBase.Instance.AddTestTextPopup(_messgae);

    }

    void OnClickGameInfo()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        Transform _tran = PopupManager.Instance.RootPopup;
        string _messgae = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eTextUpdate);
        SceneBase.Instance.AddTestTextPopup(_messgae);
    }

    void OnClickBackUp()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        Transform _tran = PopupManager.Instance.RootPopup;
        string _messgae = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eTextUpdate);
        SceneBase.Instance.AddTestTextPopup(_messgae);
    }


    void OnClickReset()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        Transform _root = PopupManager.Instance.RootPopup;
        SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupResetCheck);
        // initData();
    }

    void ACTION_RESET_CHECK(bool flag)
    {
        if (flag == true)
        {
            initData();
        }
    }

    bool isActionReset = false;
    ///<summary>
    ///데이터 초기화 
    ///</summary>
    void initData()
    {
        Debug.LogError("initData 데이터 초기화 ");
        //// 엔딩횟수 증가 
        // int crrEndingCnt = UserInfoManager.Instance.GetEndingCount();
        // UserInfoManager.Instance.SetEnidngCount(crrEndingCnt + 1);
        /// 날짜 초기화
        UserInfoManager.Instance.SetWeeklyCount(0);
        /// 요일 초기화
        UserInfoManager.Instance.SetSaveDreamEvent(UserInfoManager.Instance.GetDayCount(), false);
        UserInfoManager.Instance.SetDayCount(0);
        // HP초기화
        UserInfoManager.Instance.SetSaveHP(100);
        /// 인지도 초기화 
        UserInfoManager.Instance.SetSaveAwareness(0);
        /// 비디오날짜 초기화
        UserInfoManager.Instance.SetFreeVideoDate("");

        /// 스케줄 초기화
        for (int i = 0; i < (int)Enums.eWeekly.COUNT; i++)
        {
            UserInfoManager.Instance.SetProgressedWeeklyData(((eWeekly)i).ToString(), false);
            SecurityPlayerPrefs.DeleteKey(Strings.WEEKLY + "_" + ((eWeekly)i).ToString());
        }

        /// 스텟 초기화
        for (int i = 0; i < (int)Enums.eState.COUNT; i++)
        {
            UserInfoManager.Instance.setSaveState(((Enums.eState)i).ToString(), 0);
        }


        isActionReset = true;

    }


    void OnClickEnding()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        Transform _root = PopupManager.Instance.RootPopup;
        SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupEndingAlbum);
    }



}
