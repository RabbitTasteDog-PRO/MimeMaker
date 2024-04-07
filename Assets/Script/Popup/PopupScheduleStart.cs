using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using StructuredDataType;

public class PopupScheduleStart : JHPopup
{

    /// 진행 후 스케줄진행 팝업킬 액션
    public static System.Action<List<WeeklyScheduleData>, bool> ACTION_SCHEDULE_START;
    public static System.Action<bool> ACTION_ALL_CANCEL;

    public UILabel labelTitle; // 일정확인
    public UILabel labelWeeklyCount; // Week {0}?
    public UILabel labelWeekAsk; /// 이대로 일정을

    public UILabel[] labelWeekly;
    public UISprite[] spriteIcon;
    public UILabel[] labelSchedule;


    public UIButton btnAllCancel;
    public UILabel labelBtnAllCancel; /// 다시선택
    public UIButton btnQuickStart;
    public UILabel labelQuickStart;
    public UILabel labelQuickStartAmount;
    public UIButton btnStart;
    public UILabel labelBtnStart;

    List<WeeklyScheduleData> listData;


    bool isActionBtn = false;
    bool isStart = false;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void OnAwake()
    {
        base.OnAwake();
        btnAllCancel.onClick.Add(new EventDelegate(onCickAllCancel));
        btnQuickStart.onClick.Add(new EventDelegate(OnClickQuickStart));
        btnStart.onClick.Add(new EventDelegate(OnClickConfirm));


        labelTitle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eSchedule);
        int weekCnt = UserInfoManager.Instance.GetWeeklyCount() + 1;
        labelWeeklyCount.text = string.Format(SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eSchedule_0), weekCnt);
        labelWeekAsk.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eScheduleTitleSub);

        labelQuickStartAmount.text = string.Format("-{0}", 2);

        /// 디시 선택
        labelBtnAllCancel.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eReSchedule);
        labelQuickStart.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eQuickStart);
        labelBtnStart.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eConfirm);


        bool buyItemCheck = UserInfoManager.Instance.GetSaveInAppPrimiumItem(eShopPurchaseKey.eStorePrimium_2.ToString());
        btnQuickStart.gameObject.SetActive(!buyItemCheck);
        /// 종료 후 다시시작했을때 빠른진행만 보이게
        // btnStart.gameObject.SetActive(UserInfoManager.Instance.GetQuickStart());
    }

    public override void SetData(object[] data_)
    {
        if (data_ != null)
        {
            listData = (List<WeeklyScheduleData>)data_[0];

            SetScheduleUI(listData);
        }
    }


    ///<summary>
    /// UI 세팅
    ///<summary>
    void SetScheduleUI(List<WeeklyScheduleData> _data)
    {
        for (int i = 0; i < _data.Count; i++)
        {
            WeeklyScheduleData data = _data[i];
            if (data.act == eAct.NONE)
            {
                continue;
            }

            Debug.LogError(i + " act : " + data.act + " //// i : " + ((eAct)i).ToString() + " // data.schedule : " + data.schedule);
            spriteIcon[i].spriteName = data.act.ToString();

            eGlobalTextKey week = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(string.Format("{0}_0", data.weekly.ToString()));
            string _week = SceneBase.Instance.dataManager.GetDicGlobalTextData(week);
            labelWeekly[i].text = _week;


            if (data.schedule.Equals("NONE") == false)
            {
                eGlobalTextKey shcedule = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(string.Format("{0}_Text", data.schedule.ToString()));
                string _schedule = SceneBase.Instance.dataManager.GetDicGlobalTextData(shcedule);
                labelSchedule[i].text = _schedule;
            }

        }
    }

    protected override void OnStart()
    {
        base.OnStart();
    }


    protected override void OnClosed()
    {
        base.OnClosed();

        if (isStart == true)
        {
            if (ACTION_SCHEDULE_START != null && ACTION_SCHEDULE_START.GetInvocationList().Length > 0)
            {
                bool quick_start = UserInfoManager.Instance.GetQuickStart();
                ACTION_SCHEDULE_START(listData, quick_start);
            }
        }
        else
        {
            if (ACTION_SCHEDULE_START != null && ACTION_SCHEDULE_START.GetInvocationList().Length > 0)
            {
                bool quick_start = UserInfoManager.Instance.GetQuickStart();
                ACTION_SCHEDULE_START(null, false);
            }
        }

        if (isActionAllCalcel == true)
        {
            if (ACTION_ALL_CANCEL != null && ACTION_ALL_CANCEL.GetInvocationList().Length > 0)
            {
                ACTION_ALL_CANCEL(isActionAllCalcel);
            }
        }
    }


    bool isWeeklyCheck = false;
    bool isActionAllCalcel = false;
    void onCickAllCancel()
    {


        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        
        if (isActionBtn == true)
        {
            return;
        }
        isActionBtn = true;
        Invoke("InvoeIsActionBtn", 0.5f);
        for (int i = 0; i < (int)eWeekly.COUNT; i++)
        {
            isWeeklyCheck = UserInfoManager.Instance.GetProgressedWeeklyData(((eWeekly)i).ToString());
            if (isWeeklyCheck == true)
            {
                break;
            }

        }

        if (isWeeklyCheck == true)
        {
            //// 일정이 진행중이라면 다시선택 못하게
            return;
        }

        /// 다시 선택 이므로 
        /// 모튼 키값 삭제
        for (int i = 0; i < (int)eWeekly.COUNT; i++)
        {
            SecurityPlayerPrefs.DeleteKey(Strings.WEEKLY + "_" + ((eWeekly)i).ToString());
        }

        isStart = false;
        isActionAllCalcel = true;
        OnClosed();
    }

    void OnClickQuickStart()
    {

        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);
        
        if (isActionBtn == true)
        {
            return;
        }
        isActionBtn = true;
        Invoke("InvoeIsActionBtn", 0.5f);

        int diaPrice = 2;
        int dia = int.Parse(UserInfoManager.Instance.GetDia());
        if (dia - diaPrice < 0)
        {
            SceneBase.Instance.AddEmptyPurchasePopup("Empty Dia , My Dia : " + dia + " // price : " + diaPrice);
            return;
        }

        UserInfoManager.Instance.SetDia((dia - diaPrice).ToString());
        UserInfoManager.Instance.SetQuickStart(true);

        isStart = true;

        OnClosed();
    }

    void OnClickConfirm()
    {
        if (isActionBtn == true)
        {
            return;
        }
        isActionBtn = true;
        Invoke("InvoeIsActionBtn", 0.5f);

        isStart = true;

        OnClosed();
    }




    void InvoeIsActionBtn()
    {
        isActionBtn = false;
    }

}
