using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Enums;

///<summary>
/// 엔딩 보여주기만 하는 데이터 전달용 
///</summary>
public struct STEndingResult
{
    public Enums.eEndingNumber endingNumber;
    public bool isFirstNew;
    public bool isBuyCheck;


    public STEndingResult(Enums.eEndingNumber _endingNumber, bool _isFirstNew, bool _isBuyCheck)
    {
        endingNumber = _endingNumber;
        isFirstNew = _isFirstNew;
        isBuyCheck = _isBuyCheck;

    }
}

public class PopupEndingResult : JHPopup
{

    public GameObject objNewFirstAnimation;

    public UI2DSprite spriteEngingImg;
    public UILabel labelEndingTitle;
    public UILabel labelEndingTitleSub;

    public UIButton btnVideoReTry;
    public UILabel labelBtnVideoReTry;
    public UILabel labelBtnVideoReTrySub;

    public UIButton btnEnd;
    public UILabel labelBtnEnd;


    public UIButton btnClose; //// 엔딩 구매로 샀을경우 닫기버튼만 보이도록
    public UILabel labelBtnClose;


    STEndingResult sTEdningResultData;



    protected override void OnAwake()
    {
        base.OnAwake();

        SceneBase.Instance.PLAY_SFX(eSFX.SFX_25_ending);

        // PopupAdMobStart.ACTION_CLOSE_ADMOB_START_POPUP = ACTION_CLOSE_ADMOB_START_POPUP; /// 광고
        PopupEndingVideoRetry.ACTION_ENDING_VIDEO_RETRY = ACTION_ENDING_VIDEO_RETRY;
        JHAdsManager.ACTION_ADS_COMPLETE = ACTION_ADMOB_SUCESS;

        isBackButton = false;

        btnVideoReTry.onClick.Add(new EventDelegate(OnClickVideoReTry));
        btnEnd.onClick.Add(new EventDelegate(OnClickClose));

        btnClose.onClick.Add(new EventDelegate(OnClosed));
        labelBtnClose.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eCheck);

        labelEndingTitle.text = "";
        labelEndingTitleSub.text = "";
    }


    protected override void OnClosed()
    {
        base.OnClosed();
    }

    protected override void OnDestroied()
    {
        base.OnDestroied();

        // PopupAdMobStart.ACTION_CLOSE_ADMOB_START_POPUP = null;
        PopupEndingVideoRetry.ACTION_ENDING_VIDEO_RETRY = null;
        JHAdsManager.ACTION_ADS_COMPLETE = null;
    }

    bool isBuyCheck = false;
    public override void SetData(object data)
    {
        base.SetData();

        if (data != null)
        {
            sTEdningResultData = (STEndingResult)data;
            Enums.eEndingNumber num = sTEdningResultData.endingNumber;
            bool isFirst = sTEdningResultData.isFirstNew;
            isBuyCheck = sTEdningResultData.isBuyCheck;

            SetEnidngUI(num, isFirst);

        }
    }


    void SetEnidngUI(Enums.eEndingNumber no, bool flag)
    {
        STEndingData data = SceneBase.Instance.dataManager.GetSTEndingData(no);

        spriteEngingImg.sprite2D = Resources.Load<Sprite>("Image/Ending/Album/" + data.image);

        Enums.eGlobalTextKey titleKey = data.keyGlobal;
        labelEndingTitle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(titleKey);

        Enums.eGlobalTextKey subKey = flag == true ? Enums.eGlobalTextKey.eEnidngTitleSubNew : Enums.eGlobalTextKey.eEnidngTitleSubHave;
        labelEndingTitleSub.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(subKey);

        labelBtnVideoReTry.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eBtnEdningVideoTry);
        labelBtnVideoReTrySub.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eBtnEdningVideoTrySub);
        labelBtnEnd.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eBtnEdningTry);

        if (isBuyCheck == true)
        {
            isBackButton = false;
            objNewFirstAnimation.SetActive(true);
            btnVideoReTry.gameObject.SetActive(false);
            btnEnd.gameObject.SetActive(false);

            btnClose.gameObject.SetActive(true);

        }
        else
        {
            btnVideoReTry.gameObject.SetActive(true);
            btnEnd.gameObject.SetActive(true);

            objNewFirstAnimation.SetActive(flag == false ? true : false);
            isBackButton = flag;
            btnClose.gameObject.SetActive(false);
        }


    }



    ///<summary>
    /// 비디오 광고 보고 스텟 10%보유 안하고 시작 
    ///</summary>
    void OnClickVideoReTry()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        Transform _root = PopupManager.Instance.RootPopup;
        SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupEndingVideoRetry);

    }

    bool isActionRewardSucess = false;
    void ACTION_CLOSE_ADMOB_START_POPUP(bool flag)
    {
        if (flag == true)
        {
            isActionRewardSucess = flag;
            // if (JHAdsManager.ACTION_ADS_COMPLETE != null)
            // {
            //     JHAdsManager.ACTION_ADS_COMPLETE = null;
            // }
            // JHAdsManager.ACTION_ADS_COMPLETE = ACTION_ADMOB_SUCESS;
            SceneBase.Instance.adsManager.AsyncShowAdMobVideo(eVideoAds.eVideoStateFixStart);
        }
    }

    void ACTION_ENDING_VIDEO_RETRY(bool flag)
    {
        if (flag == true)
        {
            isActionRewardSucess = flag;
            // if (JHAdsManager.ACTION_ADS_COMPLETE != null)
            // {
            //     JHAdsManager.ACTION_ADS_COMPLETE = null;
            // }
            // JHAdsManager.ACTION_ADS_COMPLETE = ACTION_ADMOB_SUCESS;
            SceneBase.Instance.adsManager.AsyncShowAdMobVideo(eVideoAds.eVideoStateFixStart);
        }
    }

    ///<summary>
    /// 비디오 광고 보고 스텟 초기화 안하고 시작 
    ///</summary>
    async void ACTION_ADMOB_SUCESS(bool flag)
    {
        if (flag == true)
        {
            Debug.LogError("ACTION_ADMOB_SUCESS initData 데이터 초기화 ");
            //// 엔딩횟수 증가 
            int crrEndingCnt = UserInfoManager.Instance.GetEndingCount();
            UserInfoManager.Instance.SetEnidngCount(crrEndingCnt + 1);
            /// 날짜 초기화
            UserInfoManager.Instance.SetWeeklyCount(0);
            /// 요일 초기화
            UserInfoManager.Instance.SetDayCount(0);
            // HP초기화
            UserInfoManager.Instance.SetSaveHP(100);
            /// 인지도 초기화 
            UserInfoManager.Instance.SetSaveAwareness(0);
            /// 스케줄 초기화
            UserInfoManager.Instance.SetSaveDreamEvent(UserInfoManager.Instance.GetDayCount(), true);

            for (int i = 0; i < (int)Enums.eWeekly.COUNT; i++)
            {
                UserInfoManager.Instance.SetProgressedWeeklyData(((Enums.eWeekly)i).ToString(), false);
                SecurityPlayerPrefs.DeleteKey(Strings.WEEKLY + "_" + ((Enums.eWeekly)i).ToString());
            }

            /// 현재 내 스텟에 10% 저장
            for (int i = 0; i < (int)Enums.eState.COUNT; i++)
            {
                int crrState = UserInfoManager.Instance.getSaveState(((Enums.eState)i).ToString());
                float division = (float)(crrState * 0.1f);
                int _celling = (int)System.Math.Ceiling(division);

                // Debug.LogError("######## " + (Enums.eState)i + " // crrState : " + crrState + " // division : " + division + " // _celling : " + _celling);

                /// 스텟 초기화
                UserInfoManager.Instance.setSaveState(((Enums.eState)i).ToString(), _celling);
            }

            /// 인지도 초기화
            UserInfoManager.Instance.SetSaveAwareness(0);
            /// 체력초기화
            UserInfoManager.Instance.SetSaveHP(100);
            //// 엔딩시 고정스텟 상승 초기화
            UserInfoManager.Instance.SetStateBuyDia(false);
            UserInfoManager.Instance.SetStateBuyVideo(false);

            UserInfoManager.Instance.SetSaveFairyGiftItem(Enums.eFairyGiftItem.EVENT.ToString(), false);
            UserInfoManager.Instance.SetSaveFairyGiftItem(Enums.eFairyGiftItem.GOLD.ToString(), false);
            UserInfoManager.Instance.SetSaveFairyGiftItem(Enums.eFairyGiftItem.TIME.ToString(), false);

            await Task.Delay(300);

            JHSceneManager.Instance.Action(JHSceneManager.ACTION.ACTION_POP);

        }

    }

    ///<summary>
    /// 게임초기화 초기화
    ///</summary>
    async void OnClickClose()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        Debug.LogError("initData 데이터 초기화 ");
        //// 엔딩횟수 증가 
        int crrEndingCnt = UserInfoManager.Instance.GetEndingCount();
        UserInfoManager.Instance.SetEnidngCount(crrEndingCnt + 1);
        /// 날짜 초기화
        UserInfoManager.Instance.SetWeeklyCount(0);
        /// 요일 초기화
        UserInfoManager.Instance.SetDayCount(0);
        // HP초기화
        UserInfoManager.Instance.SetSaveHP(100);
        /// 인지도 초기화 
        UserInfoManager.Instance.SetSaveAwareness(0);
        /// 스케줄 초기화
        UserInfoManager.Instance.SetSaveDreamEvent(UserInfoManager.Instance.GetDayCount(), true);

        for (int i = 0; i < (int)Enums.eWeekly.COUNT; i++)
        {
            UserInfoManager.Instance.SetProgressedWeeklyData(((Enums.eWeekly)i).ToString(), false);
            SecurityPlayerPrefs.DeleteKey(Strings.WEEKLY + "_" + ((Enums.eWeekly)i).ToString());
        }

        for (int i = 0; i < (int)Enums.eState.COUNT; i++)
        {
            /// 스텟 초기화
            UserInfoManager.Instance.setSaveState(((Enums.eState)i).ToString(), 0);
        }
        /// 인지도 초기화
        UserInfoManager.Instance.SetSaveAwareness(0);
        /// 체력초기화
        UserInfoManager.Instance.SetSaveHP(100);
        //// 엔딩시 고정스텟 상승 초기화
        UserInfoManager.Instance.SetStateBuyDia(false);
        UserInfoManager.Instance.SetStateBuyVideo(false);

        UserInfoManager.Instance.SetSaveFairyGiftItem(Enums.eFairyGiftItem.EVENT.ToString(), false);
        UserInfoManager.Instance.SetSaveFairyGiftItem(Enums.eFairyGiftItem.GOLD.ToString(), false);
        UserInfoManager.Instance.SetSaveFairyGiftItem(Enums.eFairyGiftItem.TIME.ToString(), false);

        await Task.Delay(300);

        JHSceneManager.Instance.Action(JHSceneManager.ACTION.ACTION_POP);
    }

    ///<summary>
    ///데이터 초기화 
    ///</summary>
    void initData()
    {
        /// 스텟 초기화
        for (int i = 0; i < (int)Enums.eState.COUNT; i++)
        {
            UserInfoManager.Instance.setSaveState(((Enums.eState)i).ToString(), 0);
        }
    }

}
