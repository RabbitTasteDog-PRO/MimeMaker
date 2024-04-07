using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using StructuredDataType;


public class PopupScheduleCheck : JHPopup
{

    //// 시작전 데이터 넘길 Action
    public static System.Action<Dictionary<eWeekly, WeeklyScheduleData>> ACTION_SCHEDULE_DATA;
    Dictionary<eWeekly, WeeklyScheduleData> dicWeeklyScheduleData = new Dictionary<eWeekly, WeeklyScheduleData>();
    [Header("상단 이번주")]
    public UILabel labelWeekTitle;
    public UILabel labelWeek;

    [Header("요일")]
    public UILabel[] labelWeeklyDefault;
    public UILabel[] labelWeeklyChoice;
    public UILabel[] labelWeeklyComplete;
    public UIButton[] btnWeeklyDefault;
    public UIButton[] btnWeeklyChoice;
    public UIButton[] btnWeeklyComplete;


    [Header("행동")]
    // public GameObject objRootAct;
    public UIButton[] btnAct;
    public GameObject[] btnActOn;
    public UILabel[] labelBtnAct;
    public UIScrollView scrollAct;
    public UISprite[] spriteScrollCount;

    [Header("스케줄")]
    public GameObject objSchedule;
    public GameObject[] arrSchedule;
    public UIGrid gridSchedule;
    public GameObject objStateScheduleValue;
    public UIButton[] btnSchedule;
    public GameObject[] btnScheduleOn;
    public UILabel[] labelSchedule_On;
    public UILabel[] labelSchedule_Off;

    public UIGrid gridScheduleState;
    public GameObject[] objScheduleState;
    public UISprite[] spritePlusMinus;
    public GameObject[] ChoiceScheduleState_On;
    public GameObject[] ChoiceScheduleState_Off;
    public UILabel[] labelChoiceScheduleState_Off;


    [Header("확인")]
    public UIButton btnConfirm;
    public UILabel labelBtnConFirm;
    [Header("모두랜덤")]
    public UIButton bntRandomAll;
    public UILabel labelRandomAll;
    [Header("모두취소")]
    public UIButton btnCancelAll;
    public UILabel labelCancelAll;

    public UIButton btnClose;


    /********************************************************************************/
    /// 상수값
    const string BTN_WEEKLY_OFF = "day_2";
    const string BTN_WEEKLY_ON = "day_1";

    /********************************************************************************/
    /// 상수 및 데이터
    Color32 LABEL_WEEKLY_OFF = new Color32(122, 123, 126, 255);
    Color32 LABEL_WEEKLY_ON = new Color32(223, 102, 134, 255);

    Color32 ALPHA_COLOR = new Color32(255, 255, 255, 160);
    Color32 COLOR_ONE = new Color32(255, 255, 255, 255);

    eAct crrAct = eAct.NONE;
    eSchedule crrSchedule = eSchedule.NONE;
    eWeekly crrWeekly = eWeekly.NONE;
    int idxSchedule = -1;

    //// 0 : 마이너스 , 1: 플러스 
    string[,] STATE_PLUS_MINUS = new string[,]
    {
        {"schedule_bar_minus_01_145x52", "schedule_bar_minus_02_145x52", "schedule_bar_minus_02_145x52", "schedule_bar_minus_04_145x52"},
        {"schedule_bar_pluse_01_145x52", "schedule_bar_pluse_02_145x52", "schedule_bar_pluse_03_145x52", "schedule_bar_pluse_04_145x52"}

    };

    string PLUS_BAR = "schedule_bar_pluse_05_18x16";
    string MINUS_BAR = "schedule_bar_minus_05_18x16";

    /********************************************************************************/


    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        //// 스크롤 인데스 카운팅
        float clipX = scrollAct.transform.GetComponent<UIPanel>().clipOffset.x;
        int pageNum = (int)(clipX / 617);
        if (pageNum == 0)
        {
            spriteScrollCount[0].spriteName = "popup_icon_point_on_15x15";
            spriteScrollCount[1].spriteName = "popup_icon_point_off_15x15";
        }
        else
        {
            spriteScrollCount[0].spriteName = "popup_icon_point_off_15x15";
            spriteScrollCount[1].spriteName = "popup_icon_point_on_15x15";
        }
        // Debug.Log("pageNum : " + Mathf.Abs(Mathf.Round(pageNum)));

    }


    /// <summary>
    /// Awake is called when the script instance is being loaded.

    /// </summary>
    protected override void OnAwake()
    {
        base.OnAwake();
        // lbaelStateUpDown.text = "";

        btnConfirm.onClick.Add(new EventDelegate(OnClickConfirm));
        btnClose.onClick.Add(new EventDelegate(OnClosed));
        // btnClose.onClick.Add(new EventDelegate(OnClosed));

        bntRandomAll.onClick.Add(new EventDelegate(OnClickRandomAll));
        btnCancelAll.onClick.Add(new EventDelegate(OnClickCancelAll));

        labelBtnConFirm.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eConfirm);
        labelRandomAll.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eRandomAll);
        labelCancelAll.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eCancelAll);


        string top = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eSchedule_0);
        int weeklyCnt = UserInfoManager.Instance.GetWeeklyCount();
        labelWeek.text = string.Format(top, (weeklyCnt + 1));
        labelWeekTitle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eSchedule);


        // labelMonitor.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eMonitor);
        for (int i = 0; i < labelWeeklyDefault.Length; i++)
        {
            int eWeekly = ((int)eGlobalTextKey.eMonday + i);
            labelWeeklyDefault[i].text = SceneBase.Instance.dataManager.GetDicGlobalTextData((eGlobalTextKey)eWeekly);
            labelWeeklyChoice[i].text = SceneBase.Instance.dataManager.GetDicGlobalTextData((eGlobalTextKey)eWeekly);
            labelWeeklyComplete[i].text = SceneBase.Instance.dataManager.GetDicGlobalTextData((eGlobalTextKey)eWeekly);
        }


        for (int i = 0; i < labelBtnAct.Length; i++)
        {
            eAct _act = (eAct)i;
            eGlobalTextKey key = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(_act.ToString());
            labelBtnAct[i].text = SceneBase.Instance.dataManager.GetDicGlobalTextData(key);
        }

        for (int i = 0; i < btnSchedule.Length; i++)
        {
            string _strSchedule = string.Format("{0}_{1}", eAct.eActBrod.ToString(), i);
            eGlobalTextKey key = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(_strSchedule);


            labelSchedule_On[i].text = SceneBase.Instance.dataManager.GetDicGlobalTextData(key);
            labelSchedule_Off[i].text = SceneBase.Instance.dataManager.GetDicGlobalTextData(key);

            labelChoiceScheduleState_Off[i].text = SceneBase.Instance.dataManager.GetDicGlobalTextData(key);

        }

        for (int i = 0; i < labelChoiceScheduleState_Off.Length; i++)
        {
            labelChoiceScheduleState_Off[i].text = "";

            labelSchedule_On[i].text = string.Empty;
            labelSchedule_Off[i].text = string.Empty;

        }


        /*********************************************************************************************/
        /// OnClick메소드 생성 

        for (int i = 0; i < btnWeeklyDefault.Length; i++)
        {
            EventDelegate del = new EventDelegate(this, "OnClickWeekly");
            EventDelegate.Parameter parm = new EventDelegate.Parameter();
            parm.value = (eWeekly)i;
            parm.expectedType = typeof(eWeekly);
            del.parameters[0] = parm;
            EventDelegate.Add(btnWeeklyDefault[i].onClick, del);
        }

        for (int i = 0; i < btnWeeklyComplete.Length; i++)
        {
            EventDelegate del = new EventDelegate(this, "OnClickWeeklyComplete");
            EventDelegate.Parameter parm = new EventDelegate.Parameter();
            parm.value = (eWeekly)i;
            parm.expectedType = typeof(eWeekly);
            del.parameters[0] = parm;
            EventDelegate.Add(btnWeeklyComplete[i].onClick, del);

        }

        for (int i = 0; i < btnAct.Length; i++)
        {
            EventDelegate del = new EventDelegate(this, "OnClickAct");
            EventDelegate.Parameter parm = new EventDelegate.Parameter();
            parm.value = (eAct)i;
            parm.expectedType = typeof(eAct);
            del.parameters[0] = parm;
            EventDelegate.Add(btnAct[i].onClick, del);

            STActData _data = SceneBase.Instance.dataManager.GetSTActiveData((eAct)i);

            //// 이미지 설정
            btnAct[i].normalSprite2D = Resources.Load<Sprite>(string.Format("Image/Act/{0}", _data.actImg));
            btnAct[i].GetComponent<UI2DSprite>().sprite2D = Resources.Load<Sprite>(string.Format("Image/Act/{0}", _data.actImg));
        }

        for (int i = 0; i < btnSchedule.Length; i++)
        {
            EventDelegate del = new EventDelegate(this, "OnClickSchedule");
            EventDelegate.Parameter parm = new EventDelegate.Parameter();
            parm.value = i;
            parm.expectedType = typeof(int);
            del.parameters[0] = parm;
            EventDelegate.Add(btnSchedule[i].onClick, del);
        }

    }

    ///<summary>
    /// 데이터 세팅 
    ///</summary>
    void SetWeekData()
    {
        for (int i = 0; i < (int)eWeekly.COUNT; i++)
        {
            string week = UserInfoManager.Instance.GetWeeklyData(((eWeekly)i).ToString());
            // Debug.LogError("################################# SetWeekData : " + week);
            if (week.Equals("") || week.Equals("@"))
            {
                WeeklyScheduleData data = new WeeklyScheduleData();
                data.weekly = (eWeekly)i;
                data.act = eAct.NONE;
                data.schedule = eSchedule.NONE;

                dicWeeklyScheduleData.Add((eWeekly)i, data);
                continue;
            }
            else
            {
                /// 데이터가 저장되어있으면 딕셔너리 세팅
                eWeekly _weekly = (eWeekly)i;
                eAct _act = RayUtils.Utils.ConvertEnumData<eAct>(week.Split('@')[0]);
                eSchedule _schedule = RayUtils.Utils.ConvertEnumData<eSchedule>(week.Split('@')[1]);

                WeeklyScheduleData data = new WeeklyScheduleData();

                data.weekly = _weekly;
                data.act = _act;
                data.schedule = _schedule;

                if (dicWeeklyScheduleData.ContainsKey(_weekly) == false)
                {
                    dicWeeklyScheduleData.Add(_weekly, data);

                }
                else
                {
                    WeeklyScheduleData _data = data;
                    dicWeeklyScheduleData[_weekly] = _data;

                }

            }
        }
    }


    // public override void StartLayer()
    protected override void OnStart()
    {
        base.OnStart();

        SetWeekData();


        string week = UserInfoManager.Instance.GetWeeklyData(eWeekly.eMonday.ToString());

        if (week.Equals("") || week.Equals("@"))
        {
            crrWeekly = eWeekly.eMonday;
            //// 요일 세팅 
            UIWeeklySetting(crrWeekly);
            /// 일정 세팅 
            UIActSetting(eAct.NONE);
            /// 스케줄 세팅 
            UIScheduleSetting((int)eSchedule.NONE);
            // Debug.LogError("############ 노선택  week : " + week);
        }
        else
        {
            crrWeekly = eWeekly.eMonday;
            string strAct = week.Equals("") || week.Equals("@") ? eAct.NONE.ToString() : week.Split('@')[0];
            eAct converAct = week.Equals("") || week.Equals("@") ? eAct.NONE : RayUtils.Utils.ConvertEnumData<eAct>(strAct);

            string schedule = week.Equals("") || week.Equals("@") ? eSchedule.NONE.ToString() : week.Split('@')[1];
            eSchedule convertSchedule = schedule.Equals("NONE") == false ? RayUtils.Utils.ConvertEnumData<eSchedule>(schedule) : eSchedule.NONE;


            // crrWeekly = eWeekly.eMonday;
            // string strAct = week.Equals("") == false ? week.Split('@')[0] : eAct.NONE.ToString();
            // eAct converAct = week.Equals("") == false ? RayUtils.Utils.ConvertEnumData<eAct>(strAct) : eAct.NONE;

            // string schedule = week.Equals("") == false ? week.Split('@')[1] : eSchedule.NONE.ToString();
            // eSchedule convertSchedule = schedule.Equals("NONE") == false ? RayUtils.Utils.ConvertEnumData<eSchedule>(schedule) : eSchedule.NONE;

            UIWeeklySetting(eWeekly.eMonday);
            UIActSetting(converAct);
            UIScheduleSetting((int)convertSchedule);
            Debug.LogError("############ 요일별 crrWeekly : " + crrWeekly + " // converAct : " + converAct + " // convertSchedule : " + convertSchedule);
        }

        if (scrollAct != null)
        {
            scrollAct.GetComponent<UIPanel>().depth = this.gameObject.GetComponent<UIPanel>().depth + 2;
        }

    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    protected override void OnDestroied()
    {
        base.OnDestroied();
        if (ACTION_SCHEDULE_DATA != null && ACTION_SCHEDULE_DATA.GetInvocationList().Length > 0)
        {
            // bool isCheck = true;
            for (int i = 0; i < (int)eWeekly.COUNT; i++)
            {
                string weeklyData = UserInfoManager.Instance.GetWeeklyData(((eWeekly)i).ToString());
                // Debug.LogError("################## check : " + weeklyData);
                if (weeklyData.Equals("") || weeklyData.Equals("@"))
                {
                    isCheck = false;
                    break;
                }
            }
            //// 데이터가 있다면 스케줄 팝업 나오게 실행
            if (isCheck == true)
            {
                ACTION_SCHEDULE_DATA(dicWeeklyScheduleData);
            }

        }
    }


    ///<summary>
    /// 요일선택 온클릭 메소드 
    ///</summary>
    void OnClickWeekly(eWeekly _weekly)
    {
        //// 엑트 눌렀을경우 스케줄 초기화
        for (int i = 0; i < labelChoiceScheduleState_Off.Length; i++)
        {
            labelChoiceScheduleState_Off[i].text = string.Empty;

            labelSchedule_On[i].text = string.Empty;
            labelSchedule_Off[i].text = string.Empty;
        }

        // 스케줄 데이터 없을경우
        if (dicWeeklyScheduleData.ContainsKey(_weekly) == false)
        {
            WeeklyScheduleData weeklyData = new WeeklyScheduleData();
            weeklyData.weekly = _weekly;
            crrWeekly = _weekly;
            UIWeeklySetting(crrWeekly);

            crrAct = eAct.eActBrod;
            UIActSetting(crrAct);

            dicWeeklyScheduleData.Add(crrWeekly, weeklyData);

            UIScheduleSetting((int)eSchedule.NONE);
        }
        else
        {
            /// 같을경우 동작 안함
            if (_weekly != crrWeekly)
            {
                crrWeekly = _weekly;
                UIWeeklySetting(crrWeekly);

                // /// 스케줄 세팅
                eSchedule tempSche = dicWeeklyScheduleData[_weekly].schedule == eSchedule.NONE ? eSchedule.NONE : dicWeeklyScheduleData[_weekly].schedule;

                /// 행동 세팅 
                if (tempSche == eSchedule.NONE)
                {
                    UIActSetting(eAct.NONE);
                }
                else
                {
                    UIActSetting(dicWeeklyScheduleData[_weekly].act);
                }

                /// 20200731 변경된 UI 상에서 요일 누를떄마다 스케줄 뜨는건 풀필요 해서 일단 주석
                int tempIdx = tempSche == eSchedule.NONE ? -1 : int.Parse(tempSche.ToString().Split('_')[1]);
                UIScheduleSetting(tempIdx);
                // Debug.LogError("############ OnClickWeekly crrWeekly : " + crrWeekly + " // tempAct : " + tempAct + " // tempSche : " + tempSche + " //  tempScheIndex : " + tempIdx);
            }
        }

    }

    void OnClickWeeklyComplete(eWeekly _weekly)
    {
        //// 엑트 눌렀을경우 스케줄 초기화
        for (int i = 0; i < labelChoiceScheduleState_Off.Length; i++)
        {
            labelChoiceScheduleState_Off[i].text = string.Empty;
        }

        // 스케줄 데이터 없을경우
        if (dicWeeklyScheduleData.ContainsKey(_weekly) == false)
        {
            WeeklyScheduleData weeklyData = new WeeklyScheduleData();
            weeklyData.weekly = _weekly;
            crrWeekly = _weekly;
            UIWeeklySetting(crrWeekly);

            // objRootAct.SetActive(true);
            crrAct = eAct.NONE;
            UIActSetting(crrAct);

            // objRootSchedule.SetActive(false);

            dicWeeklyScheduleData.Add(crrWeekly, weeklyData);

            UIScheduleSetting((int)eSchedule.NONE);
        }
        else
        {
            /// 같을경우 동작 안함
            // if (_weekly != crrWeekly)
            {
                crrWeekly = _weekly;
                UIWeeklySetting(crrWeekly);

                /// 행동 세팅 
                eAct tempAct = dicWeeklyScheduleData[_weekly].act == eAct.NONE ? eAct.NONE : dicWeeklyScheduleData[_weekly].act;
                UIActSetting(tempAct);

                // /// 스케줄 세팅
                eSchedule tempSche = dicWeeklyScheduleData[_weekly].schedule == eSchedule.NONE ? eSchedule.NONE : dicWeeklyScheduleData[_weekly].schedule;
                int tempIdx = tempSche == eSchedule.NONE ? -1 : int.Parse(tempSche.ToString().Split('_')[1]);
                /// 20200731 변경된 UI 상에서 요일 누를떄마다 스케줄 뜨는건 풀필요 해서 일단 주석
                UIScheduleSetting(tempIdx);
                Debug.LogError("############ OnClickWeekly crrWeekly : " + crrWeekly + " // tempAct : " + tempAct + " // tempSche : " + tempSche + " //  tempScheIndex : " + tempIdx);
            }
        }
    }



    bool isActionAct = false;
    void InvokeIsActionAct()
    {
        isActionAct = false;
    }
    ///<summary>
    /// 행동 선택 온클릭 메소드 
    ///</summary>
    void OnClickAct(eAct _act)
    {
        if (isActionAct == true)
        {
            return;
        }

        for (int i = 0; i < labelChoiceScheduleState_Off.Length; i++)
        {
            labelChoiceScheduleState_Off[i].text = string.Empty;
        }

        if (crrWeekly == eWeekly.NONE)
        {
            Debug.LogError("요일을 먼저 설정해 주세요");
            return;
        }

        isActionAct = true;
        Invoke("InvokeIsActionAct", 0.3f);
        if (crrAct != _act)
        {
            crrAct = _act;
        }
        Debug.LogError("dicWeeklyScheduleData[crrWeekly]  : " + dicWeeklyScheduleData[crrWeekly]);
        if (dicWeeklyScheduleData[crrWeekly].act == eAct.NONE || dicWeeklyScheduleData[crrWeekly] == null)
        {
            if (dicWeeklyScheduleData[crrWeekly] == null)
            {
                WeeklyScheduleData _data = new WeeklyScheduleData();
                _data.act = _act;
                dicWeeklyScheduleData[crrWeekly] = _data;

            }
            else
            {
                dicWeeklyScheduleData[crrWeekly].act = _act;
            }

            UIActSetting(crrAct);
            UIScheduleSetting((int)eSchedule.NONE);
        }
        else
        {
            //// 휴식 선택일 경우엔 
            //// 스케줄 하나만 나오게 설정 
            if (_act == eAct.eActRest)
            {
                objStateScheduleValue.SetActive(false);

                crrAct = _act;
                dicWeeklyScheduleData[crrWeekly].act = _act;

                UIActSetting(crrAct);
            }
            else
            {
                objStateScheduleValue.SetActive(false);

                if (dicWeeklyScheduleData[crrWeekly].act != _act)
                {
                    /// 현재 요일이 저장되어 있지 않다면 
                    crrAct = _act;
                    // dicWeeklyScheduleData[crrWeekly].act = _act;
                    dicWeeklyScheduleData[crrWeekly].act = eAct.NONE;
                    dicWeeklyScheduleData[crrWeekly].schedule = eSchedule.NONE;
                    //// 스케줄이 정해져있지 않다면 데이터 초기화
                    UserInfoManager.Instance.SetWeeklyData(crrWeekly.ToString(), "", "");
                    crrSchedule = eSchedule.NONE;

                    if (dicWeeklyScheduleData[crrWeekly].schedule == eSchedule.NONE)
                    {
                        idxSchedule = (int)eSchedule.NONE;
                    }

                    UIActSetting(crrAct);
                    UIScheduleSetting((int)eSchedule.NONE);
                }
                else
                {
                    crrAct = _act;
                    UIActSetting(crrAct);
                }
            }
        }

    }

    ///<summary>
    /// 행동 버튼 UI Set
    ///</summary>
    void UIActSetting(eAct _act)
    {
        if (crrAct != _act)
        {
            crrAct = _act;
        }

        /// 휴식일 경우에만 예외처리
        if (_act == eAct.eActRest)
        {

            objStateScheduleValue.SetActive(false);

            arrSchedule[1].SetActive(false);
            arrSchedule[2].SetActive(false);
            arrSchedule[3].SetActive(false);

            objScheduleState[1].SetActive(false);
            objScheduleState[2].SetActive(false);
            // objScheduleState[3].SetActive(false);

            gridSchedule.Reposition();
            gridScheduleState.Reposition();

            string _week = UserInfoManager.Instance.GetWeeklyData(crrWeekly.ToString());
            string _strSaveAct = (_week.Equals("") == true || _week.Equals("@") == true) ? eAct.NONE.ToString() : _week.Split('@')[0];
            eAct _saveAct = (_week.Equals("") == true || _week.Equals("@") == true) ? eAct.NONE : RayUtils.Utils.ConvertEnumData<eAct>(_strSaveAct);


            for (int i = 0; i < btnActOn.Length; i++)
            {
                btnActOn[i].SetActive(_act == (eAct)i);
                btnAct[i].defaultColor = (_act == (eAct)i) ? COLOR_ONE : ALPHA_COLOR;
                btnAct[i].GetComponent<UI2DSprite>().color = (_act == (eAct)i) ? COLOR_ONE : ALPHA_COLOR;
            }

            string _strSchedule = string.Format("{0}_{1}", _act.ToString(), 0);
            eGlobalTextKey key = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(_strSchedule);
            labelSchedule_On[0].text = SceneBase.Instance.dataManager.GetDicGlobalTextData(key);
            labelSchedule_Off[0].text = SceneBase.Instance.dataManager.GetDicGlobalTextData(key);

            labelChoiceScheduleState_Off[3].text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eAllStateMinus);
            labelChoiceScheduleState_Off[3].text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eAllStateMinus);

            spritePlusMinus[3].spriteName = MINUS_BAR;
            spritePlusMinus[0].spriteName = PLUS_BAR;

            ChoiceScheduleState_Off[0].GetComponent<UISprite>().spriteName = STATE_PLUS_MINUS[1, 0];
            ChoiceScheduleState_Off[3].GetComponent<UISprite>().spriteName = STATE_PLUS_MINUS[0, 3];


            spritePlusMinus[1].gameObject.SetActive(false);
            spritePlusMinus[2].gameObject.SetActive(false);
            spritePlusMinus[3].gameObject.SetActive(false);

            return;
        }
        else
        {
            if (_act == eAct.NONE)
            {
                for (int i = 0; i < btnAct.Length; i++)
                {
                    btnActOn[i].SetActive(false);
                    btnAct[i].defaultColor = ALPHA_COLOR;
                    btnAct[i].GetComponent<UI2DSprite>().color = ALPHA_COLOR;
                }

                for (int i = 0; i < btnSchedule.Length; i++)
                {
                    btnScheduleOn[i].SetActive(false);
                    btnSchedule[i].gameObject.SetActive(true);

                    ChoiceScheduleState_On[i].gameObject.SetActive(false);
                    ChoiceScheduleState_Off[i].gameObject.SetActive(true);

                    labelSchedule_On[i].text = "";
                    labelSchedule_Off[i].text = "";

                    // btnWeeklyComplete[i].gameObject.SetActive(false);
                    // btnWeeklyChoice[i].gameObject.SetActive(true);
                    // labelSchedule_On[i].gameObject.SetActive(false);
                    // labelSchedule_Off[i].gameObject.SetActive(true);

                }

                objStateScheduleValue.SetActive(false);

                return;
            }

            string week = UserInfoManager.Instance.GetWeeklyData(crrWeekly.ToString());
            string strSaveAct = (!week.Equals("") || !week.Equals("@")) ? eAct.NONE.ToString() : week.Split('@')[0];
            eAct saveAct = (!week.Equals("") || !week.Equals("@")) ? eAct.NONE : RayUtils.Utils.ConvertEnumData<eAct>(strSaveAct);
            for (int i = 0; i < btnAct.Length; i++)
            {
                if (saveAct == eAct.NONE)
                {
                    btnActOn[i].SetActive(_act == (eAct)i);
                    btnAct[i].defaultColor = (_act == (eAct)i) ? COLOR_ONE : ALPHA_COLOR;
                    btnAct[i].GetComponent<UI2DSprite>().color = (_act == (eAct)i) ? COLOR_ONE : ALPHA_COLOR;

                }
                else
                {
                    if (saveAct != _act)
                    {
                        btnActOn[i].SetActive(_act == (eAct)i);
                        btnAct[i].defaultColor = (_act == (eAct)i) ? COLOR_ONE : ALPHA_COLOR;
                        btnAct[i].GetComponent<UI2DSprite>().color = (_act == (eAct)i) ? COLOR_ONE : ALPHA_COLOR;
                    }
                    else
                    {
                        if (saveAct != eAct.NONE)
                        {
                            btnActOn[i].SetActive(saveAct == (eAct)i);
                            btnAct[i].defaultColor = saveAct == (eAct)i ? COLOR_ONE : ALPHA_COLOR;
                            btnAct[i].GetComponent<UI2DSprite>().color = saveAct == (eAct)i ? COLOR_ONE : ALPHA_COLOR;
                        }
                        else
                        {

                            btnActOn[i].SetActive(_act == (eAct)i);
                            btnAct[i].defaultColor = (_act == (eAct)i) ? COLOR_ONE : ALPHA_COLOR;
                            btnAct[i].GetComponent<UI2DSprite>().color = (_act == (eAct)i) ? COLOR_ONE : ALPHA_COLOR;
                        }
                    }
                }

            }

            for (int i = 0; i < labelSchedule_Off.Length; i++)
            {
                arrSchedule[i].SetActive(true);
                objScheduleState[i].SetActive(true);

                string _strSchedule = string.Format("{0}_{1}", _act.ToString(), i);
                eGlobalTextKey key = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(_strSchedule);

                labelSchedule_On[i].text = SceneBase.Instance.dataManager.GetDicGlobalTextData(key);
                labelSchedule_Off[i].text = SceneBase.Instance.dataManager.GetDicGlobalTextData(key);
            }

            gridSchedule.Reposition();
            gridScheduleState.Reposition();

        }




    }

    ///<summary>
    /// 스케줄 선택 온클릭 메소드 
    ///</summary>
    void OnClickSchedule(int _index)
    {
        if (crrAct == eAct.NONE)
        {
            SceneBase.Instance.AddEmptyPurchasePopup("행동을 먼저 선택해 주세요");
            // Debug.LogError("################### crrAct : " + crrAct + " // 행동을 먼저 선택해 주세요");
            return;
        }

        Debug.LogError("###################### OnClickSchedule index : " + _index);

        if (dicWeeklyScheduleData[crrWeekly].schedule == eSchedule.NONE)
        {
            idxSchedule = _index;
            crrSchedule = RayUtils.Utils.ConvertEnumData<eSchedule>(string.Format("{0}_{1}", crrAct.ToString(), idxSchedule));
            dicWeeklyScheduleData[crrWeekly].schedule = crrSchedule;
            UIScheduleSetting(idxSchedule);
        }
        else
        {
            eSchedule tempSchedule = RayUtils.Utils.ConvertEnumData<eSchedule>(string.Format("{0}_{1}", crrAct.ToString(), _index));
            if (dicWeeklyScheduleData[crrWeekly].schedule != tempSchedule)
            {
                dicWeeklyScheduleData[crrWeekly].schedule = tempSchedule;
                crrSchedule = tempSchedule;
                idxSchedule = _index;
                UIScheduleSetting(idxSchedule);
            }
            else
            {
                crrSchedule = tempSchedule;
                idxSchedule = _index;
                UIScheduleSetting(idxSchedule);
            }

        }
    }


    ///<sumamry>
    /// 요일 선택 UI Set
    ///</summary>
    void UIWeeklySetting(eWeekly _weekly)
    {
        for (int i = 0; i < btnWeeklyDefault.Length; i++)
        {
            /// 한 주의 데이터 체크
            string strWeek = UserInfoManager.Instance.GetWeeklyData(string.Format("{0}", (eWeekly)i));
            // Debug.LogError("#################### strWeek : " + strWeek + " // _weekly : " + _weekly);

            if (_weekly == eWeekly.NONE)
            {
                btnWeeklyDefault[i].transform.gameObject.SetActive(true);
                btnWeeklyChoice[i].transform.gameObject.SetActive(false);
                btnWeeklyComplete[i].transform.gameObject.SetActive(false);
            }
            else
            {
                /// 데이터가 없는 상태라면
                if (string.IsNullOrEmpty(strWeek) == true || strWeek.Equals("@") == true)
                {
                    btnWeeklyComplete[i].gameObject.SetActive(false);
                    btnWeeklyDefault[i].transform.gameObject.SetActive(true);
                }
                else
                {
                    btnWeeklyDefault[i].transform.gameObject.SetActive(false);
                    btnWeeklyComplete[i].transform.gameObject.SetActive(true);
                }
                btnWeeklyChoice[i].transform.gameObject.SetActive(_weekly == (eWeekly)i ? true : false);
            }
        }

    }


    ///<summary>
    /// 스케줄 설정 UI Set
    ///</summary>
    void UIScheduleSetting(int _scheduleIndex)
    {
        if (crrAct == eAct.NONE)
        {
            Debug.LogError("###################### crrAct == eAct.NONE");
            for (int i = 0; i < btnSchedule.Length; i++)
            {
                Debug.LogError("###################### " + i);
                {
                    arrSchedule[i].SetActive(true);
                    btnSchedule[i].gameObject.SetActive(true);
                    btnScheduleOn[i].SetActive(false);
                    ChoiceScheduleState_On[i].SetActive(false);
                }
            }
            gridSchedule.Reposition();
            Debug.LogError("스케줄 미선택");
            return;
        }

        if (_scheduleIndex < 0)
        {
            objStateScheduleValue.SetActive(false);
            //// 스케줄이 정해져있지 않다면 데이터 초기화
            UserInfoManager.Instance.SetWeeklyData(crrWeekly.ToString(), "", "");

            for (int i = 0; i < btnSchedule.Length; i++)
            {
                if (crrAct == eAct.eActRest)
                {
                    if (i == 0)
                    {
                        btnSchedule[i].gameObject.SetActive(true);
                        btnScheduleOn[i].SetActive(false);
                        /// 포인트 가감값 체크
                        ChoiceScheduleState_On[i].SetActive(false);
                    }
                    else if (i == 3)
                    {
                        btnSchedule[i].gameObject.SetActive(false);
                        btnScheduleOn[i].SetActive(false);
                        ChoiceScheduleState_On[i].SetActive(false);

                    }
                    else
                    {
                        btnSchedule[i].gameObject.SetActive(false);
                        btnScheduleOn[i].SetActive(false);
                        /// 포인트 가감값 체크
                        ChoiceScheduleState_On[i].SetActive(false);

                    }

                }
                else
                {

                    btnSchedule[i].gameObject.SetActive(true);
                    btnScheduleOn[i].SetActive(false);
                    ChoiceScheduleState_On[i].SetActive(false);
                    ChoiceScheduleState_Off[i].SetActive(true);
                }

            }
            gridSchedule.Reposition();

            return;
        }

        else
        {
            //// 느낌이 여기같음 TODO
            // crrSchedule = () eSchedule()
            string week = UserInfoManager.Instance.GetWeeklyData(crrWeekly.ToString());
            Debug.LogError("############################# week : " + week);
            eSchedule eSche = eSchedule.NONE;
            if (crrAct == eAct.eActRest)
            {
                eSche = eSchedule.eActRest_0;
            }
            else
            {
                if (_scheduleIndex > 3)
                {
                    eSche = RayUtils.Utils.ConvertEnumData<eSchedule>(((eSchedule)_scheduleIndex).ToString());
                }
                else
                {
                    eSche = RayUtils.Utils.ConvertEnumData<eSchedule>(string.Format("{0}_{1}", crrAct, _scheduleIndex));
                }

                // Debug.LogError("@@@@@@@@@@@@@@@@@@ crrAct : " + crrAct + " // _schedule : " + _scheduleIndex);
            }
            //// 현재 스케줄 입력
            crrSchedule = eSche;
            for (int i = 0; i < btnSchedule.Length; i++)
            {
                eSchedule _i = RayUtils.Utils.ConvertEnumData<eSchedule>(string.Format("{0}_{1}", crrAct.ToString(), i));

                if (crrAct == eAct.eActRest)
                {
                    if (i == 0)
                    {
                        btnSchedule[i].gameObject.SetActive(true);
                        btnScheduleOn[i].SetActive(eSche == _i);
                        /// 포인트 가감값 체크
                        ChoiceScheduleState_On[i].SetActive((_i == eSche));
                    }
                    else
                    {
                        if (i == 3)
                        {
                            btnSchedule[i].gameObject.SetActive(false);
                            btnScheduleOn[i].SetActive(false);
                            ChoiceScheduleState_On[i].SetActive(false);
                        }
                        else
                        {
                            btnSchedule[i].gameObject.SetActive(false);
                            btnScheduleOn[i].SetActive(false);
                            /// 포인트 가감값 체크
                            ChoiceScheduleState_On[i].SetActive(false);

                        }

                    }
                }
                else
                {
                    btnSchedule[i].gameObject.SetActive(true);
                    btnScheduleOn[i].SetActive(eSche == _i);
                    /// 포인트 가감값 체크
                    ChoiceScheduleState_On[i].SetActive((_i == eSche));
                    // ChoiceScheduleState_Off[i].SetActive(!(_i == eSche));

                    dicWeeklyScheduleData[crrWeekly].act = crrAct;
                }

            }

            gridSchedule.Reposition();
            UIScheduleDataSetting(eSche);
            Debug.LogError("########## dicWeeklyScheduleData weekly: " +
                                dicWeeklyScheduleData[crrWeekly].weekly +
                                " // act : " + dicWeeklyScheduleData[crrWeekly].act +
                                " // schedule : " + dicWeeklyScheduleData[crrWeekly].schedule
            );
            // Debug.LogError("crrWeekly : " + crrWeekly + " // crrAct :  " + crrAct + " // crrSchedule : " + crrSchedule);
            /// 스케줄까지 정했다면 데이터 세팅
            UserInfoManager.Instance.SetWeeklyData(crrWeekly.ToString(), crrAct.ToString(), crrSchedule.ToString());
        }
    }


    /// 하단 스케줄 당 가감 데이터 세팅
    void UIScheduleDataSetting(eSchedule eSche)
    {
        // Debug.LogError("############################ UIScheduleDataSetting eSche : " + eSche.ToString() + " // crrAct : " + crrAct);

        Dictionary<eSchedule, STScheduleData> data = SceneBase.Instance.dataManager.GetDicSceduleData(crrAct);
        STScheduleData stData = data[eSche];
        string[] arrUpState = new string[] { stData.upKey_0.ToString(), stData.upKey_1.ToString(), stData.upKey_2.ToString() };
        string[] arrUpPoint = new string[] { stData.upPoint_0, stData.upPoint_1, stData.upPoint_2 };
        string[] arrDownState = new string[] { stData.downKey_0.ToString(), stData.downKey_1.ToString(), stData.downKey_2.ToString() };
        string[] arrDownPoint = new string[] { stData.downPoint_0, stData.downPoint_1, stData.downPoint_2 };

        // dadfgsdfgsdfg

        List<string> listStateValue = new List<string>();

        objStateScheduleValue.SetActive(-1 >= (int)eSche ? false : true);

        for (int i = 0; i < arrUpState.Length; i++)
        {
            if (crrAct == eAct.eActRest)
            {
                listStateValue.Add("체력 +4");
                break;
            }

            if (arrUpState[i].Contains("NONE"))
            {
                continue;
            }

            eGlobalTextKey key = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(arrUpState[i]);
            string keyText = SceneBase.Instance.dataManager.GetDicGlobalTextData(key);

            listStateValue.Add((keyText + string.Format(" +{0} ", arrUpPoint[i])));
            int check = int.Parse(arrUpPoint[i]);
            if (check < 0)
            {
                ChoiceScheduleState_Off[i].GetComponent<UISprite>().spriteName = STATE_PLUS_MINUS[0, i];
            }
            else
            {
                ChoiceScheduleState_Off[i].GetComponent<UISprite>().spriteName = STATE_PLUS_MINUS[1, i];
            }

        }


        for (int i = 0; i < arrDownState.Length; i++)
        {
            if (crrAct == eAct.eActRest)
            {
                listStateValue.Add("체력 +4");
                break;
            }

            if (arrDownState[i].Contains("NONE"))
            {
                continue;
            }
            eGlobalTextKey key = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(arrDownState[i]);
            string keyText = SceneBase.Instance.dataManager.GetDicGlobalTextData(key);

            listStateValue.Add((keyText + string.Format(" {0} ", arrDownPoint[i])));

            int check = int.Parse(arrUpPoint[i]);
            if (check > 0)
            {
                ChoiceScheduleState_Off[i].GetComponent<UISprite>().spriteName = STATE_PLUS_MINUS[1, i];
            }
            else
            {
                ChoiceScheduleState_Off[i].GetComponent<UISprite>().spriteName = STATE_PLUS_MINUS[0, i];
            }

        }



        for (int i = 0; i < listStateValue.Count; i++)
        {
            labelChoiceScheduleState_Off[i].text = listStateValue[i];
            if (listStateValue[i].Contains("-"))
            {
                spritePlusMinus[i].spriteName = MINUS_BAR;
            }
            else
            {
                spritePlusMinus[i].spriteName = PLUS_BAR;
            }
        }

        // lbaelStateUpDown.text = string.Format("{0} / {1} / {2} / {3}", test[0], test[1], test[2], test[3]);

        // labelUpState.text = strUpState.ToString();
        // labelDownState.text = strDownState.ToString();
    }
    bool isCheck = true;
    protected override void OnClosed()
    {
        isCheck = false;
        base.OnClosed();
    }

    ///<summary>
    /// 완료 온클릭 메소드 
    ///</summary>
    void OnClickConfirm()
    {
        

        for (int i = 0; i < (int)eWeekly.COUNT; i++)
        {
            string _data = UserInfoManager.Instance.GetWeeklyData(((eWeekly)i).ToString());
            isCheck = _data.Equals("") || _data.Equals("@") ? false : true;
            // isCheck = UserInfoManager.Instance.GetWeeklyData(((eWeekly)i).ToString()).Equals("") ? false : true;
            if (isCheck == false)
            {
                SceneBase.Instance.AddEmptyPurchasePopup("스케줄을 선택해주세요");
                break;
            }
        }
        SceneBase.Instance.PLAY_SFX(eSFX. SFX_8_start);
        
        // /   일정 선택 화면에서 일정을 모두 결정한 후, x(닫기) 버튼을 누르면 최종 확인 화면으로 넘어감
        // → 닫기 버튼을 눌렀을 때는 일정이 저장되지 않고, 결정 버튼을 눌렀을 때 중간 저장되는 형태가 좋지 않을까 싶음
        if (isCheck == true)
        {
            Destroy(this.gameObject);
        }
        else
        {
            OnClosed();
        }
    }


    ///<summary>
    /// 랜덤으로 일정 선택
    ///</summary>
    void OnClickRandomAll()
    {
        dicWeeklyScheduleData.Clear();

        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        for (int i = 0; i < (int)eWeekly.COUNT; i++)
        {
            eWeekly _weekly = (eWeekly)i;
            eAct randomAct = (eAct)Random.Range(0, (int)eAct.COUNT - 1);
            eSchedule randomSchedule = RayUtils.Utils.ConvertEnumData<eSchedule>(string.Format("{0}_{1}", randomAct.ToString(), Random.Range(0, 3)));

            /// 데이터가 저장되어있으면 딕셔너리 세팅
            WeeklyScheduleData data = new WeeklyScheduleData();

            data.weekly = _weekly;
            data.act = randomAct;
            if (randomAct == eAct.eActRest)
            {
                data.schedule = eSchedule.eActRest_0;
            }
            else
            {
                data.schedule = randomSchedule;
            }

            dicWeeklyScheduleData.Add(_weekly, data);

            UserInfoManager.Instance.SetWeeklyData(_weekly.ToString(), randomAct.ToString(), randomSchedule.ToString());
        }

        string week = UserInfoManager.Instance.GetWeeklyData(eWeekly.eSaturday.ToString());
        crrWeekly = eWeekly.eSaturday;

        string strAct = week.Equals("") || week.Equals("@") ? eAct.NONE.ToString() : week.Split('@')[0];
        eAct converAct = week.Equals("") || week.Equals("@") ? eAct.NONE : RayUtils.Utils.ConvertEnumData<eAct>(strAct);
        crrAct = converAct;
        string schedule = week.Equals("") == false ? week.Split('@')[1] : eSchedule.NONE.ToString();
        string[] schedule_index = schedule.Split('_');
        int lastSchIndex = int.Parse(schedule_index[1]);
        eSchedule convertSchedule = schedule.Equals("NONE") || week.Equals("") || week.Equals("@") ? eSchedule.NONE : RayUtils.Utils.ConvertEnumData<eSchedule>(schedule);

        Debug.LogError("############## lastSchIndex : " + lastSchIndex);
        UIWeeklySetting(crrWeekly);
        UIActSetting(converAct);
        UIScheduleSetting((int)lastSchIndex);
        Debug.LogError("############ 요일별 crrWeekly : " + crrWeekly + " // converAct : " + converAct + " // convertSchedule : " + convertSchedule);


    }

    ///<summary>
    /// 전체 초기화
    ///</summary>
    void OnClickCancelAll()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        dicWeeklyScheduleData.Clear();

        /// 다시 선택 이므로 
        /// 모튼 요일별 키값 삭제
        for (int i = 0; i < (int)eWeekly.COUNT; i++)
        {
            SecurityPlayerPrefs.DeleteKey(Strings.WEEKLY + "_" + ((eWeekly)i).ToString());

            //// 요일별 데이터 초기화
            WeeklyScheduleData _data = new WeeklyScheduleData();
            _data.weekly = (eWeekly)i;
            _data.act = eAct.NONE;
            _data.schedule = eSchedule.NONE;
            dicWeeklyScheduleData.Add((eWeekly)i, _data);
        }

        crrWeekly = eWeekly.eMonday;
        crrAct = eAct.NONE;

        //// 요일 세팅 
        UIWeeklySetting(crrWeekly);
        /// 일정 세팅 
        UIActSetting(crrAct);
        /// 스케줄 세팅 
        UIScheduleSetting((int)eSchedule.NONE);
        // Debug.LogError("############ 노선택  week : " + week);


    }
}
