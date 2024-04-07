using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using StructuredDataType;
using System;
using System.Threading;

public class PopupScheduleAct : JHPopup
{

    const int HP_RECOVERY_ICON_ENABLE = 20;
    const int HP_RECOVERY_AMOUNT = 10;

    public static System.Action ACTION_WEEKLY_RESULT;
    //// HP 엔딩 체크
    public static System.Action ACTION_ENDING_CHECK;


    public GameObject objTopBar;
    public GameObject objHPRecovery;
    public UIButton btnHPRecovery;
    public TweenAlpha[] tweenObject;

    // public UIPanel panelUI;
    public GameObject objActive;

    [Header("스케줄 배경")]
    public UI2DSprite spriteScheduleBg;
    public UILabel labelWeeklyScheduleTitle;
    public UILabel labelWeekly;
    public UISprite spriteActIcon;


    [Header("State")]
    public GameObject objState; // 휴식일 경우에 
    public UILabel[] labelState;
    public UILabel[] labelSateUpDpwn;
    public UISprite[] spriteFillStateValue;
    public UILabel labelCountDown;

    public GameObject objAllStateMinus;
    public UILabel labelAllStateMinus;

    [Header("HP")]
    public GameObject objHP;
    public UISprite fillValeHp;
    public UILabel labelHp;
    public UILabel labelHpValue;

    [Header("인지도")]
    public GameObject objAwareness;
    public UISprite fillAwareness;
    public UILabel labelAwareness;
    public UILabel labelAwarenessValue;


    [Header("상태로그")]
    public UIScrollView scrollLog;
    public UITable tableLog;
    public PrefabLog _prefabLog;
    public GameObject objLine;
    public UIButton btnLog;


    [Header("obj")]
    public UIButton btnNext;
    public UILabel labelBtnNext;


    /*******************************************************************************************/
    /// 상수값

    eAct crrActSuddenEvent = eAct.NONE;
    eSchedule crrSchedultSuddenEvent = eSchedule.NONE;

    List<WeeklyScheduleData> listScheduleData;
    int NextScheduleCount = 0;


    /// 마이너스 색상 
    Color32 COLOR_MINUS = new Color32(255, 109, 0, 255);
    string SPRITE_MINUS = "gauge_minus";
    Color32 COLOR_PLUS = new Color32(21, 144, 255, 255);
    string SPRITE_PLUS = "gauge_plus";

    string NEXT_ON = "test_btn_ok_on_211x112";
    string NEXT_OFF = "test_btn_ok_off_211x112";

    bool isCheckCountdown = false;
    int CountDown = 0;
    int TIME_DEFAULT = 20;
    // int TIME_FAST = 10;
    bool quickStart = false;


    int HP_PLUS = 4;
    int HP_MINUS = -4;
    int AWS_PLUS = 1;
    int AWS_MINUS = 1;

    /// SCHEDULE   REST_HP   REST_STATE   SUDDEN_START   SUDDEN_END   ITEM_EFFECT
    const string KEY_SCHEDULE = "SCHEDULE";

    const string KEY_REST_HP = "REST_HP";

    const string KEY_REST_STATE = "REST_STATE";

    const string KEY_SUDDEN_START = "SUDDEN_START";

    const string KEY_SUDDEN_END = "SUDDEN_END";

    const string KEY_ITEM_EFFECT = "ITEM_EFFECT";
    const string KEY_MANAGER_EFFECT = "MANAGER_EFFECT";


    const string KEY_AWARENESS_UP = "AWARENESS_UP";
    const string KEY_HP_DOWN = "HP_DOWN";




    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void OnAwake()
    {
        base.OnAwake();

        SceneBase.Instance.PLAY_BGM(eBGM.BGM_SCHEDULE);

        PopupSuddenEvent.ACTION_SUDDEN_START_LOG = ACTION_SUDDEN_START_LOG; //// 돌발이벤트 시작 로그
        PopupSuddenEvent.ACTION_SUDDEN_END_LOG = ACTION_SUDDEN_END_LOG; //// 돌발이벤트 종료 로그;

        PopupAdMobStart.ACTION_CLOSE_ADMOB_START_POPUP = ACTION_CLOSE_ADMOB_START_POPUP; /// 동영상광고 체력 보상
        JHAdsManager.ACTION_ADS_COMPLETE = ACTION_ADMOB_SUCESS;

        PopupBuyHPRecovery.ACTION_BUY_RECOVERY_VIDEO = ACTION_BUY_RECOVERY_VIDEO;

        objState.SetActive(false);
        objAllStateMinus.SetActive(false);
        objHP.SetActive(false);
        objAwareness.SetActive(false);

        isBackButton = false;

        for (int i = 0; i < labelState.Length; i++)
        {
            labelState[i].text = "";
            labelSateUpDpwn[i].text = "";
            spriteFillStateValue[i].fillAmount = 0.0f;
        }

        float crrHP = (float)UserInfoManager.Instance.GetSaveHP() * 0.01f;
        fillValeHp.fillAmount = crrHP;

        float crrAwareness = (float)UserInfoManager.Instance.GetSaveAwareness() * 0.01f;
        fillAwareness.fillAmount = crrAwareness;

        labelBtnNext.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eNext);
        btnNext.onClick.Add(new EventDelegate(OnClickNext));
        btnLog.onClick.Add(new EventDelegate(OnClickLog));
        btnHPRecovery.onClick.Add(new EventDelegate(OnClickHPRecovery));

        //// 화면비율에 따라 상하단 오브젝트 위치 조절 
        float defaultRatio = 9f / 16f;
        float currentRatio = (float)Screen.width / (float)Screen.height;
        // Debug.LogError("Screen.width  : " + Screen.width + " // Screen.height : " + Screen.height);
        // Debug.LogError("##################### defaultRatio :  " + defaultRatio);
        // Debug.LogError("##################### currentRatio : " + currentRatio);

        if (currentRatio < defaultRatio)
        {
            if (currentRatio < defaultRatio)
            {
                if (currentRatio < 0.48f)
                {

                    objTopBar.transform.localPosition = new Vector2(objTopBar.transform.localPosition.x, objTopBar.transform.localPosition.y + 130);
                    btnNext.transform.localPosition = new Vector2(btnNext.transform.localPosition.x, btnNext.transform.localPosition.y - 130);

                }
                else
                {
                    objTopBar.transform.localPosition = new Vector2(objTopBar.transform.localPosition.x, objTopBar.transform.localPosition.y + 100);
                    btnNext.transform.localPosition = new Vector2(btnNext.transform.localPosition.x, btnNext.transform.localPosition.y - 100);
                }

            }

        }

        for (int i = 0; i < tweenObject.Length; i++)
        {
            tweenObject[i].delay = UnityEngine.Random.Range(0.1f, 1.0f);
        }

    }

    protected override void OnDestroied()
    {
        base.OnDestroied();

        PopupSuddenEvent.ACTION_SUDDEN_START_LOG = null; //// 돌발이벤트 시작 로그
        PopupSuddenEvent.ACTION_SUDDEN_END_LOG = null; //// 돌발이벤트 종료 로그;

        PopupAdMobStart.ACTION_CLOSE_ADMOB_START_POPUP = null; /// 동영상광고 체력 보상
        JHAdsManager.ACTION_ADS_COMPLETE = null;

        PopupBuyHPRecovery.ACTION_BUY_RECOVERY_VIDEO = null;

    }


    IEnumerator IECountDown(bool buyCheck)
    {
        // Debug.LogError("카운트 다운 스타트");
        isCheckCountdown = true;
        btnNext.normalSprite = NEXT_OFF;
        btnNext.GetComponent<UISprite>().spriteName = NEXT_OFF;
        btnNext.GetComponent<BoxCollider2D>().enabled = !isCheckCountdown;

        //// 실제 데이터 
        int count = TIME_DEFAULT;//buyCheck == true ? TIME_FAST : TIME_SIXTY;
        // float value = buyCheck == true ? 10.0f : 60.0f;
        // float fixValue = buyCheck == true ? 10.0f : 60.0f;

        // 테스트용 
        // int count = 5;
        // float value = 5.0f;

        if(buyCheck == true)
        {
            count -= 5;
        }

        //// 선물요정 시간 적용
        if (UserInfoManager.Instance.GetSaveFairyGiftItem(eFairyGiftItem.TIME.ToString()) == true)
        {
            count /= UserInfoManager.FAIRY_GIFT_TIME_VALE_3;
        }


        if (UserInfoManager.Instance.GetSaveInAppPrimiumItem(eShopPurchaseKey.eStorePrimium_2.ToString()) == true)
        {
            count /= UserInfoManager.PRIMIUM_TIME_VALUE;
        }
        //// 최소 5초 
        int _checkCount = count;
        count = Mathf.Min(TIME_DEFAULT, Mathf.Max(5, _checkCount));

        //// 시간감소 매니저 스킬일 경우 확률및 값 체크
        STManagerSkill _managerSkill = JHManagerManger.Instance.GetMyManagerSkill();
        if (_managerSkill.managerSkill == eManager_Skill.SKILL_C)
        {
            int _randomProb = UnityEngine.Random.Range(0, 100);
            int _prob = (int)_managerSkill.probability;
            int _probValue = (int)_managerSkill.probabilityValue;

            if (_prob >= _randomProb)
            {
                count = (count - _probValue < 0) ? 1 : (count - _probValue);
                Debug.LogError("################## ############ ###############: " + count);
                //// 매니저 효과 시간감소 로그
                /// SetCreateLog(string _type, string _weekly, eSchedule _schedule, eState _eState, int value, string _suddenTitle = "", string _suddenText = "")
                // 매니저 효과로 인해 시간이 {0}초 감소합니다.
                string _timeText = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eLogManagerTime);
                string _timeFormat = string.Format(_timeText, (_probValue * -1));

                StartCoroutine(SetCreateLog(KEY_MANAGER_EFFECT, ((eWeekly)NextScheduleCount).ToString(), eSchedule.NONE, eState.NONE, (_probValue * -1), "", _timeFormat));
            }
        }

        //// TODO
        //// 스텟 아이템 처리

        while (true)
        {

            if (count < 0)
            {
                isCheckCountdown = false;
                btnNext.normalSprite = NEXT_ON;
                btnNext.GetComponent<UISprite>().spriteName = NEXT_ON;
                btnNext.GetComponent<BoxCollider2D>().enabled = !isCheckCountdown;

                //// TODO
                //// 매니저 스텟상승 아이템 처리
                //     eLogItemState,	  eManagerItemAbility.eState
                string _itemState = UserInfoManager.Instance.GetEquipManagerItem(eManagerItemAbility.eState.ToString());
                if (_itemState.Equals("") == false)
                {
                    eManagerItem _eItemState = RayUtils.Utils.ConvertEnumData<eManagerItem>(_itemState);
                    string _strmName = SceneBase.Instance.dataManager.GetDicGlobalTextData(RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(_itemState.ToString()));

                    STManagerItemData itemState = SceneBase.Instance.dataManager.GetManagerItemData(_eItemState);

                    int _random = UnityEngine.Random.Range(0, 100);
                    int _prob = (int)itemState.prob;
                    int _probValue = (int)itemState.upPoint;
                    if (_random >= _prob)
                    {
                        //아이템 '{0}' 인해 {1}이(가) {2} 올랐습니다.
                        string _stateText = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eLogItemState);
                        eState _randomState = (eState)UnityEngine.Random.Range((int)eState.eFACE, (int)eState.eCHARACTER);
                        string _strState = SceneBase.Instance.dataManager.GetDicGlobalTextData(RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(_randomState.ToString()));
                        string _stateFormat = string.Format(_stateText, _strmName, _strState, _probValue);
                        //// 스텟 상승
                        UserInfoManager.Instance.setSaveState(_randomState.ToString(), (UserInfoManager.Instance.getSaveState(_randomState.ToString()) + _probValue));

                        // SceneBase.Instance.AddTestTextPopup(string.Format("스텟상승F 아이템발동 확률 : " + _prob + " // 포인트 : " + _probValue));

                        StartCoroutine(SetCreateLog(KEY_ITEM_EFFECT, ((eWeekly)NextScheduleCount).ToString(), eSchedule.NONE, eState.NONE, _probValue, "", _stateFormat));
                    }
                }

                break;
            }

            labelCountDown.text = string.Format("{0:00}", count);

            yield return YieldHelper.waitForSeconds(1000);

            count -= 1;
            // value -= 1.0f;
            

            // count -= 1;
            // value -= 1.0f;
            // Debug.LogError("labelCountDown.text : " + labelCountDown.text);
        }

    }




    ///<summary>
    /// 데이터 처리
    ///</summary>
    public override void SetData(object[] data_)
    {

        if (data_ != null)
        {
            this.gameObject.SetActive(true);

            listScheduleData = (List<WeeklyScheduleData>)data_[0];
            quickStart = (bool)data_[1];

            for (int i = 0; i < (int)eWeekly.COUNT; i++)
            {
                bool processed = UserInfoManager.Instance.GetProgressedWeeklyData(((eWeekly)i).ToString());
                if (processed == true)
                {
                    NextScheduleCount++;
                }
            }
            StartCoroutine(SetUISchedule(NextScheduleCount));

        }
    }

    ///<summary>
    /// 한 주의 요일마다 스케줄 체크
    ///</summary>
    IEnumerator SetUISchedule(int processed)
    {
        StartCoroutine(IECountDown(quickStart));

        //// 로그 뎁스 설정
        scrollLog.GetComponent<UIPanel>().depth = GetDepth() + 2;

        for (int i = 0; i < labelState.Length; i++)
        {
            labelState[i].text = "";
            labelSateUpDpwn[i].text = "";
        }
        yield return new WaitForEndOfFrame();

        if (listScheduleData != null)
        {
            if (processed > (listScheduleData.Count - 1))
            {
                OnClosed();
                yield break;
            }
            else
            {

                WeeklyScheduleData data = listScheduleData[processed];
                /************************************************************/
                //// 돌발이벤트용 데이처
                crrActSuddenEvent = data.act;
                crrSchedultSuddenEvent = data.schedule;
                /************************************************************/

                spriteActIcon.spriteName = data.act.ToString();
                if (data.act == eAct.eActRest)
                {
                    int _ran = UnityEngine.Random.Range(0, 1);
                    eSchedule _random = _ran == 0 ? eSchedule.eActRest_0 : eSchedule.eActRest_1;
                    //// 휴식일경우 2가지가있어 랜덤으로 나눔
                    SetActScheduleUI(_random);

                    /// 가감 UI Enable
                    // objState.SetActive(false);
                    // objAllStateMinus.SetActive(true);

                    /// 휴식일 경우에 예외처리
                    Dictionary<eSchedule, STScheduleData> dic = SceneBase.Instance.dataManager.GetDicSceduleData(data.act);
                    STScheduleData schedulST = dic[eSchedule.eActRest_0];

                    eGlobalTextKey globalKey = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(string.Format("{0}_Text", data.schedule));
                    // eGlobalTextKey globalKey = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(data.schedule);

                    string _title = SceneBase.Instance.dataManager.GetDicGlobalTextData(globalKey);
                    labelWeeklyScheduleTitle.text = _title.Replace("\n", "");

                    string week = string.Format("{0}_0", (eWeekly)processed);
                    eGlobalTextKey globalWeek = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(week);
                    string _week = SceneBase.Instance.dataManager.GetDicGlobalTextData(globalWeek);
                    labelWeekly.text = _week;

                    labelAllStateMinus.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eAllStateMinus);

                    yield return StartCoroutine(ProcessedWeeklyScheduleCheck(data));

                }
                else
                {
                    // objAllStateMinus.SetActive(false);
                    // objState.SetActive(true);

                    SetActScheduleUI(crrSchedultSuddenEvent);
                    /// 가감 UI Enable


                    Dictionary<eSchedule, STScheduleData> dic = SceneBase.Instance.dataManager.GetDicSceduleData(data.act);
                    STScheduleData schedulST = dic[data.schedule];

                    // eGlobalTextKey globalKey = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(data.schedule.ToString());
                    eGlobalTextKey globalKey = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(string.Format("{0}_Text", data.schedule));
                    string _title = SceneBase.Instance.dataManager.GetDicGlobalTextData(globalKey);
                    labelWeeklyScheduleTitle.text = _title.Replace("\n", "");

                    // labelWeeklyScheduleTitle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(globalKey);
                    string week = string.Format("{0}_0", (eWeekly)processed);
                    eGlobalTextKey globalWeek = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(week);
                    string _week = SceneBase.Instance.dataManager.GetDicGlobalTextData(globalWeek);
                    labelWeekly.text = _week;

                    yield return StartCoroutine(ProcessedWeeklyScheduleCheck(data));
                    //// 20200724 TODO
                    //// 1. 배경 이미지 및 케릭터 리소스 추가 
                    //// 2. 주말 이벤트는 따로 팝업 만들어서 진행할것, 같이하면 겹칠경우가 생길거 같음 

                    List<string> state = new List<string>();
                    List<string> point = new List<string>();

                    state.Add(schedulST.upKey_0.ToString());
                    state.Add(schedulST.upKey_1.ToString());
                    state.Add(schedulST.upKey_2.ToString());
                    state.Add(schedulST.downKey_0.ToString());
                    state.Add(schedulST.downKey_1.ToString());
                    state.Add(schedulST.downKey_2.ToString());

                    point.Add(schedulST.upPoint_0);
                    point.Add(schedulST.upPoint_1);
                    point.Add(schedulST.upPoint_2);
                    point.Add(schedulST.downPoint_0);
                    point.Add(schedulST.downPoint_1);
                    point.Add(schedulST.downPoint_2);

                    int realStateCnt = 0;
                    for (int i = 0; i < state.Count; i++)
                    {
                        if (state[i].Equals("NONE"))
                        {
                            continue;
                        }
                        else
                        {
                            eGlobalTextKey _enum = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(state[i]);
                            string key = SceneBase.Instance.dataManager.GetDicGlobalTextData(_enum);
                            string _point = int.Parse(point[i]) < 0 ? string.Format("{0:00}", point[i]) : string.Format("+{0:00}", point[i]);

                            labelState[realStateCnt].text = key;
                            labelSateUpDpwn[realStateCnt].text = _point;
                            labelSateUpDpwn[realStateCnt].color = int.Parse(point[i]) < 0 ? COLOR_MINUS : COLOR_PLUS;
                            spriteFillStateValue[realStateCnt].spriteName = int.Parse(point[i]) < 0 ? SPRITE_MINUS : SPRITE_PLUS;
                            realStateCnt++;
                        }
                    }
                }
            }
        }
    }

    ///<summary>
    /// 실제 데이터 세팅
    ///</summary>
    IEnumerator ProcessedWeeklyScheduleCheck(WeeklyScheduleData data)
    {
        Dictionary<eSchedule, STScheduleData> dic = SceneBase.Instance.dataManager.GetDicSceduleData(data.act);
        STScheduleData schedulST = dic[data.schedule];

        if (data.act == eAct.eActRest)
        {
            // objAwareness.SetActive(false);
            // objState.SetActive(false);
            /// HP 설정 
            //// 휴식일 경우 모든 스텟 -1씩 감소 
            for (int i = 0; i < (int)eState.COUNT; i++)
            {
                string _state = ((eState)i).ToString();
                int crrValue = UserInfoManager.Instance.getSaveState(_state);
                //// 스케줄 진행 안됐을경우에만 스텟 처리
                if (UserInfoManager.Instance.GetProgressedWeeklyData(NextScheduleCount.ToString()) == false)
                {
                    UserInfoManager.Instance.setSaveState(_state, (crrValue - 1));
                }
            }
            SetHPProcess(HP_PLUS);
            yield return new WaitForSeconds(0.5f);
            SetAwarenessProcess(AWS_PLUS);
            yield return new WaitForSeconds(0.5f);

            StartCoroutine(SetCreateLog(KEY_REST_STATE, data.weekly.ToString(), eSchedule.NONE, eState.NONE, -1));

            //// 휴식 예외처리
            yield return new WaitForEndOfFrame();
        }
        else
        {
            // objAwareness.SetActive(true);
            // objState.SetActive(true);

            SetHPProcess(HP_MINUS);
            yield return new WaitForSeconds(0.5f);
            SetAwarenessProcess(AWS_PLUS);

            List<string> state = new List<string>();
            List<string> point = new List<string>();

            yield return new WaitForEndOfFrame();


            state.Add(schedulST.upKey_0.ToString());
            state.Add(schedulST.upKey_1.ToString());
            state.Add(schedulST.upKey_2.ToString());
            state.Add(schedulST.downKey_0.ToString());
            state.Add(schedulST.downKey_1.ToString());
            state.Add(schedulST.downKey_2.ToString());

            point.Add(schedulST.upPoint_0);
            point.Add(schedulST.upPoint_1);
            point.Add(schedulST.upPoint_2);
            point.Add(schedulST.downPoint_0);
            point.Add(schedulST.downPoint_1);
            point.Add(schedulST.downPoint_2);

            for (int i = 0; i < (int)eState.COUNT; i++)
            {
                int beforPoint = UserInfoManager.Instance.getSaveState(((eState)i).ToString().ToString());
                /// 애니메이션용 가감값 데이터 저장
                UserInfoManager.Instance.SetTempCrrStatePoint(((eState)i).ToString(), beforPoint);
            }

            int realCount = 0;
            for (int i = 0; i < state.Count; i++)
            {
                yield return new WaitForEndOfFrame();
                if (state[i].Equals("NONE") || string.IsNullOrEmpty(state[i]) == true)
                {

                    continue;
                }
                else
                {
                    eWeekly weekly = data.weekly;
                    eState _state = RayUtils.Utils.ConvertEnumData<eState>(state[i]);
                    int beforPoint = UserInfoManager.Instance.getSaveState(_state.ToString());
                    int crrPoint = Mathf.Min(Integers.STATE_MAX, Mathf.Max(Integers.STATE_MIN, beforPoint + int.Parse(point[i])));

                    float fillValeu = (float)crrPoint * 0.01f;
                    spriteFillStateValue[realCount].fillAmount = fillValeu;

                    //// 스케줄 처리 안되었을때만 가감처리
                    // Debug.LogError("########## ProcessedWeeklyScheduleCheck crrPoint : " + crrPoint);
                    if (UserInfoManager.Instance.GetProgressedWeeklyData(NextScheduleCount.ToString()) == false)
                    {
                        UserInfoManager.Instance.setSaveState(_state.ToString(), crrPoint);
                    }


                    string strAdjust = UserInfoManager.Instance.GetTempAdjustStatePoint(_state.ToString());
                    if (strAdjust.Equals(""))
                    {
                        UserInfoManager.Instance.SetTempAdjustStatePoint(_state.ToString(), Integers.STATE_MIN);
                    }
                    if (strAdjust.Contains("NONE"))
                    {
                        continue;
                    }
                    string[] test = strAdjust.Split('@');
                    int adjustBefor = 0;
                    if (test.Length > 1)
                    {
                        adjustBefor = int.Parse(strAdjust.Split('@')[1]);
                    }
                    UserInfoManager.Instance.SetTempAdjustStatePoint(_state.ToString(), Mathf.Max(Integers.STATE_MIN, Mathf.Min(Integers.STATE_MAX, (adjustBefor + int.Parse(point[i])))));
                    realCount++;
                    yield return StartCoroutine(SetCreateLog(KEY_SCHEDULE, weekly.ToString(), data.schedule, _state, int.Parse(point[i])));
                }
            }

        }

        //// 매니저 효과 및 스텟아이템 적용

    }


    bool moveHPEnding = false;
    ///<summary>
    /// HP UI 및 데이터 세팅 
    /// UI 적용 어떻게 할지 논의
    ///</summary>
    void SetHPProcess(int value)
    {
        /// 체력 올리기
        STManagerSkill _managerSkill = JHManagerManger.Instance.GetMyManagerSkill();
        int skillHp = 0;
        if (_managerSkill.managerSkill == eManager_Skill.SKILL_A)
        {
            int _random = UnityEngine.Random.Range(0, 100);
            int _prob = (int)_managerSkill.probability;
            int _probValue = (int)_managerSkill.probabilityValue;
            if (_random >= _prob)
            {
                skillHp = (int)_managerSkill.probabilityValue;
                //// 매니저 효과 체력상승
                /// 매니저 효과로 인해 체력이 {0} 올랐습니다.
                string _hpText = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eLogManagerHP);
                string _hpFormat = string.Format(_hpText, skillHp);
                StartCoroutine(SetCreateLog(KEY_MANAGER_EFFECT, ((eWeekly)NextScheduleCount).ToString(), eSchedule.NONE, eState.NONE, skillHp, "", _hpFormat));

                // SceneBase.Instance.AddTestTextPopup(string.Format("스킬발동 현재 HP : {0}, 스킬 HP : {1}, 적용 HP : {2}", UserInfoManager.Instance.GetSaveHP(), skillHp, (value + skillHp)));
            }
        }
        /// 매니저 스킬이 HP증가일 경우 
        int clacValue = value + skillHp;
        int crrHp = UserInfoManager.Instance.GetSaveHP() + (clacValue);

        labelHp.text = "체력";
        string hpVale = clacValue > 0 ? string.Format("+{0}", clacValue) : clacValue.ToString();
        labelHpValue.text = hpVale;
        labelHpValue.color = clacValue < 0 ? COLOR_MINUS : COLOR_PLUS;
        fillValeHp.spriteName = clacValue < 0 ? SPRITE_MINUS : SPRITE_PLUS;

        float fillValeu = (float)crrHp * 0.01f;
        fillValeHp.fillAmount = fillValeu;

        UserInfoManager.Instance.SetSaveHP(crrHp);

        objHPRecovery.SetActive(crrHp <= HP_RECOVERY_ICON_ENABLE);


        if (crrHp <= 0)
        {
            moveHPEnding = true;
            OnClosed();
        }

        WeeklyScheduleData data = listScheduleData[NextScheduleCount];
        eWeekly _eDay = (eWeekly)NextScheduleCount;
        eSchedule _eSchdule = data.schedule;

        string _key = value < 0 ? KEY_HP_DOWN : KEY_REST_HP;
        StartCoroutine(SetCreateLog(_key, _eDay.ToString(), eSchedule.NONE, eState.NONE, value));

        //// TODO
        //// 체력회복 아이템 처리 
        string _itemHP = UserInfoManager.Instance.GetEquipManagerItem(eManagerItemAbility.eHP.ToString());
        //     eLogItemHp, //아이템 '{0}' 인해 체력이 {1} 회복됩니다.  eManagerItemAbility.eHP
        if (_itemHP.Equals("") == false)
        {
            eManagerItem _eItemHP = RayUtils.Utils.ConvertEnumData<eManagerItem>(_itemHP);
            string _strmName = SceneBase.Instance.dataManager.GetDicGlobalTextData(RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(_itemHP.ToString()));

            STManagerItemData itemHP = SceneBase.Instance.dataManager.GetManagerItemData(_eItemHP);

            int _random = UnityEngine.Random.Range(0, 100);
            int _prob = (int)itemHP.prob;
            int _probValue = (int)itemHP.upPoint;

            Debug.LogError("################### HP 아이템 적용 itemHP : " + itemHP + " // _random : " + _random + " // prob : " + _prob + " // value : " + _probValue);

            if (_random >= _prob)
            {
                /// //아이템 '{0}' 인해 체력이 {1} 회복됩니다.
                string _hpText = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eLogItemHp);
                string _hpFormat = string.Format(_hpText, _strmName, _probValue);
                //// HP 상승
                UserInfoManager.Instance.SetSaveHP(UserInfoManager.Instance.GetSaveHP() + _probValue);

                // SceneBase.Instance.AddTestTextPopup(string.Format("HP 아이템발동 확률 : " + _prob + " // 포인트 : " + _probValue));

                StartCoroutine(SetCreateLog(KEY_ITEM_EFFECT, _eDay.ToString(), eSchedule.NONE, eState.NONE, _probValue, "", _hpFormat));
            }
        }


    }

    ///<summary>
    /// 인지도 UI 및 데이터 세팅 
    /// UI 적용 어떻게 할지 논의
    ///</summary>
    void SetAwarenessProcess(int value)
    {
        /// 체력 올리기
        STManagerSkill _managerSkill = JHManagerManger.Instance.GetMyManagerSkill();
        /// 인지도 올리기
        int skillAwareness = 0;
        if (_managerSkill.managerSkill == eManager_Skill.SKILL_E)
        {
            int _random = UnityEngine.Random.Range(0, 100);
            int _prob = (int)_managerSkill.probability;
            int _probValue = (int)_managerSkill.probabilityValue;
            if (_random >= _prob)
            {
                skillAwareness = (int)_managerSkill.probabilityValue;

                string _awarenessText = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eLogManagerAwarenss);
                string _awarenessFormat = string.Format(_awarenessText, skillAwareness);
                StartCoroutine(SetCreateLog(KEY_MANAGER_EFFECT, ((eWeekly)NextScheduleCount).ToString(), eSchedule.NONE, eState.NONE, skillAwareness, "", _awarenessFormat));


                // SceneBase.Instance.AddTestTextPopup(string.Format("스킬발동 인지도 : {0}, 스킬 인지도 : {1}, 적용 인지도 : {2}", UserInfoManager.Instance.GetSaveAwareness(), skillAwareness, (value + skillAwareness)));
            }
        }
        /// 매니저 스킬이 인지도 증가일 경우 
        int clacValue = value + skillAwareness;

        int crrAwareness = UserInfoManager.Instance.GetSaveAwareness() + (clacValue);
        // Debug.LogError("######################### crr crrAwareness : " + crrAwareness);
        labelAwareness.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eAwareness);
        string AwarenessVale = clacValue > 0 ? string.Format("+{0}", clacValue) : clacValue.ToString();
        labelAwarenessValue.text = AwarenessVale;
        labelAwarenessValue.color = clacValue < 0 ? COLOR_MINUS : COLOR_PLUS;
        fillAwareness.spriteName = clacValue < 0 ? SPRITE_MINUS : SPRITE_PLUS;

        float fillValeu = (float)crrAwareness * 0.01f;
        fillAwareness.fillAmount = fillValeu;

        UserInfoManager.Instance.SetSaveAwareness(crrAwareness);

        // if (crrAwareness >= 100)
        // {
        //     Debug.LogError("SetAwarenessProcess SetAwarenessProcess SetAwarenessProcess SetAwarenessProcess");
        // }
        WeeklyScheduleData data = listScheduleData[NextScheduleCount];
        eWeekly _eDay = (eWeekly)NextScheduleCount;
        eSchedule _eSchdule = data.schedule;

        StartCoroutine(SetCreateLog(KEY_AWARENESS_UP, _eDay.ToString(), eSchedule.NONE, eState.NONE, value));

        //// TODO
        //// 인지도 상승 아이템 처리

        //     eLogItemAwareness,  //아이템 '{0}' 인해 인지도가 {1} 상승합니다. 
        string _itemAws = UserInfoManager.Instance.GetEquipManagerItem(eManagerItemAbility.eAwareness.ToString());

        if (_itemAws.Equals("") == false)
        {
            eManagerItem _eItemAws = RayUtils.Utils.ConvertEnumData<eManagerItem>(_itemAws);
            string _strmName = SceneBase.Instance.dataManager.GetDicGlobalTextData(RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(_itemAws.ToString()));

            STManagerItemData itemAws = SceneBase.Instance.dataManager.GetManagerItemData(_eItemAws);

            int _random = UnityEngine.Random.Range(0, 100);
            int _prob = (int)itemAws.prob;
            int _probValue = (int)itemAws.upPoint;
            if (_random >= _prob)
            {
                //아이템 '{0}' 인해 인지도가 {1} 상승합니다.
                // string _awsText = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eAwareness);
                string _awsText = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eLogItemAwareness);
                string _awsFormat = string.Format(_awsText, _strmName, _probValue);
                //// 인지도 상승 
                UserInfoManager.Instance.SetSaveAwareness(UserInfoManager.Instance.GetSaveAwareness() + _probValue);

                // SceneBase.Instance.AddTestTextPopup(string.Format("인지도 아이템발동 확률 : " + _prob + " // 포인트 : " + _probValue));

                StartCoroutine(SetCreateLog(KEY_ITEM_EFFECT, _eDay.ToString(), eSchedule.NONE, eState.NONE, _probValue, "", _awsFormat));
            }
        }

    }
    ///<summary>
    /// 스케줄별 UI Setting
    ///</summary>
    void SetActScheduleUI(eSchedule _sche)
    {
        try
        {
            if (objActive.transform.childCount > 0)
            {
                objActive.transform.DestroyChildren();
            }

            eSchedule scheCovert = _sche == eSchedule.eActRest_0 ? (eSchedule)UnityEngine.Random.Range((int)eSchedule.eActRest_0, (int)eSchedule.eActRest_1) : _sche;

            GameObject _ui = Instantiate(Resources.Load<GameObject>(string.Format("Prefab/Act/{0}", scheCovert.ToString()))) as GameObject;
            _ui.transform.SetParent(objActive.transform);
            _ui.transform.localPosition = Vector2.zero;
            _ui.transform.localScale = Vector3.one;

            string background = string.Format("Image/Act/Motion/Background/{0}", scheCovert.ToString());
            _ui.GetComponent<UI2DSprite>().sprite2D = Resources.Load<Sprite>(background);

            _ui.SetActive(true);

        }
        catch (System.Exception e)
        {
            Debug.LogError("GameObject is Null : " + _sche.ToString());
        }
    }


    //// 돌발이벤트 콜백 
    void SuddenEventCallBack(bool flag)
    {
        //// 돌발이벤트 결과화면 콜백
        if (flag == true)
        {
            // OnClickNext();
            crrActSuddenEvent = eAct.NONE;
            crrSchedultSuddenEvent = eSchedule.NONE;


            crrActSuddenEvent = eAct.NONE;
            crrSchedultSuddenEvent = eSchedule.NONE;

            /// 한주별 요일 테이터 저장
            UserInfoManager.Instance.SetProgressedWeeklyData(((eWeekly)NextScheduleCount).ToString(), true);

            NextScheduleCount++;
            StartCoroutine(SetUISchedule(NextScheduleCount));
            /// 날짜 카운팅
            UserInfoManager.Instance.SetDayCount(UserInfoManager.Instance.GetDayCount() + 1);

            isCheckCountdown = false;
            isActionSuddenEvent = false;
        }

    }
    bool isActionNext = false;
    bool isActionSuddenEvent = false;
    void OnClickNext()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        if (isActionNext == true)
        {
            return;
        }

        //// TODO
        //// 봉사활동 데이터 없음
        /// 확인 필요 
        if (crrSchedultSuddenEvent != eSchedule.eActService_2)
        {
            ///// TODO : 보낸때 랜덤 수치 적용해야함
            int _random = UnityEngine.Random.Range(0, 100);
            //// 테스트용 돌발이벤트 100%
            // int _random = 100;
            int suddenEventProb = Integers.SUDDEN_EVENT_PER;

            string _item = UserInfoManager.Instance.GetEquipManagerItem(eManagerItemAbility.eSuddenEvent.ToString());
            bool isEquip = !string.IsNullOrEmpty(_item);
            if (isEquip == true)
            {
                eManagerItem _eItem = RayUtils.Utils.ConvertEnumData<eManagerItem>(_item);
                STManagerItemData _itemData = SceneBase.Instance.dataManager.GetManagerItemData(_eItem);

                int prob = _itemData.prob;
                int upPoint = _itemData.upPoint;
                //// 확률적용
                suddenEventProb -= upPoint;
                /// 선물요정 데이터 적용
                if (UserInfoManager.Instance.GetSaveFairyGiftItem(eFairyGiftItem.EVENT.ToString()) == true)
                {
                    suddenEventProb -= UserInfoManager.FAIRY_GIFT_EVENT_VALE_10;
                }
            }
            //// SKILL_D
            //// 돌발이벤트 학률증가
            STManagerSkill _managerSkill = JHManagerManger.Instance.GetMyManagerSkill();
            int _probValue = 0;
            if (_managerSkill.managerSkill == eManager_Skill.SKILL_D)
            {
                int _randomProb = UnityEngine.Random.Range(0, 100);
                int _prob = (int)_managerSkill.probability;
                _probValue = (int)_managerSkill.probabilityValue;

                if (_prob >= _randomProb)
                {
                    suddenEventProb -= _probValue;
                }

                /// 선물요정 데이터 적용
                if (UserInfoManager.Instance.GetSaveFairyGiftItem(eFairyGiftItem.EVENT.ToString()) == true)
                {
                    suddenEventProb -= UserInfoManager.FAIRY_GIFT_EVENT_VALE_10;
                }
            }

            if (_random >= suddenEventProb)
            {
                if (isActionSuddenEvent == false)
                {
                    isActionSuddenEvent = true;
                    Transform _tran = PopupManager.Instance.RootPopup;
                    RayUtils.FormulaManager.SuddenEventFormula suddenEvent = new RayUtils.FormulaManager.SuddenEventFormula();
                    List<STSuddenEventData> _list = suddenEvent.GetSuddenEventLogic(crrActSuddenEvent, crrSchedultSuddenEvent.ToString());
                    STSuddenEventCallBackData _data = new STSuddenEventCallBackData(_list, NextScheduleCount, SuddenEventCallBack);

                    if (_probValue != 0)
                    {
                        string _suddenText = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eLogMananagerSudden);
                        StartCoroutine(SetCreateLog(KEY_MANAGER_EFFECT, ((eWeekly)NextScheduleCount).ToString(), eSchedule.NONE, eState.NONE, (_probValue * -1), "", _suddenText));
                    }
                    SceneBase.Instance.PLAY_SFX(eSFX.SFX_11_event);
                    SceneBase.Instance.AddPopup(_tran, ePopupLayer.PopupSuddenEvent, _data);
                    return;
                }
            }
        }

        isActionNext = true;
        Invoke("InvokeIsActionNext", 1.0f);

        if (isCheckCountdown == true)
        {
            return;
        }


        if (NextScheduleCount > listScheduleData.Count)
        {
            OnClosed();
            return;
        }

        crrActSuddenEvent = eAct.NONE;
        crrSchedultSuddenEvent = eSchedule.NONE;

        /// 한주별 요일 테이터 저장
        UserInfoManager.Instance.SetProgressedWeeklyData(((eWeekly)NextScheduleCount).ToString(), true);

        NextScheduleCount++;
        StartCoroutine(SetUISchedule(NextScheduleCount));
        /// 날짜 카운팅
        UserInfoManager.Instance.SetDayCount(UserInfoManager.Instance.GetDayCount() + 1);
        isActionSuddenEvent = false;

        Debug.LogError("#################### NextScheduleCount : " + NextScheduleCount + " // total day Count : " + UserInfoManager.Instance.GetDayCount());

        CreateLogLine();


    }

    void InvokeIsActionNext()
    {
        isActionNext = false;
    }

    protected override void OnClosed()
    {
        base.OnClosed();
        /// 체력이 0 일경우 강제로 종료 후 엔딩 진행
        if (moveHPEnding == true)
        {
            if (ACTION_ENDING_CHECK != null && ACTION_ENDING_CHECK.GetInvocationList().Length > 0)
            {
                ACTION_ENDING_CHECK();
            }

        }
        //// 체력이 1 이상일 경우 
        else
        {
            int initWeekly = 0;
            for (int i = 0; i < (int)eWeekly.COUNT; i++)
            {
                if (UserInfoManager.Instance.GetProgressedWeeklyData(((eWeekly)i).ToString()) == true)
                {
                    initWeekly++;
                }
            }
            if (initWeekly >= 6)
            {
                if (ACTION_WEEKLY_RESULT != null && ACTION_WEEKLY_RESULT.GetInvocationList().Length > 0)
                {
                    ACTION_WEEKLY_RESULT();
                    /// 일주일 스케줄 완료
                    // initProgressedScheduleData();
                }
            }
        }


    }

    public List<GameObject> listLog = new List<GameObject>();
    ///<summary>
    /// 로그 생성
    ///</summary>
    IEnumerator SetCreateLog(string _type, string _weekly, eSchedule _schedule, eState _eState, int value, string _title = "", string _msgText = "")
    {
        yield return new WaitForSeconds(1.0f);
        PrefabLog _log = Instantiate(_prefabLog, tableLog.transform) as PrefabLog;
        _log.transform.localScale = Vector3.one;
        // _log.transform.localPosition = Vector2.zero;
        // TweenAlpha _alpha = TweenAlpha.Begin(_log.gameObject, 0.3f, 1.0f);
        // _alpha.from = 0.0f;

        eGlobalTextKey _eday = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(_weekly);
        string _day = SceneBase.Instance.dataManager.GetDicGlobalTextData(_eday);

        //{0} : {1}이(가) {2} 올랐습니다.
        string _logUp = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eLogStateUp);
        // {0} : {1}이(가) {2} 떨어졌습니다.
        string _logDown = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eLogStateDown);
        //	휴식으로 인해 모든 스텟이 -1씩 떨어집니다
        string _logAllDown = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eLogStateAllDown);
        //	돌발이벤트 '{0}'을(를) 진행합니다.
        string _logSuddenStart = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eLogSuddenEventStart);
        //	돌발이벤트 결과로 {0}이(가) 올랐습니다.
        string _logSuddenEnd = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eLogSuddenEventStart);


        switch (_type)
        {
            case KEY_SCHEDULE:
                {
                    eGlobalTextKey _gloSch = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(string.Format("{0}_Text", _schedule));
                    eGlobalTextKey _gloState = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(_eState.ToString());

                    string _strSchedule = SceneBase.Instance.dataManager.GetDicGlobalTextData(_gloSch);
                    string _strState = SceneBase.Instance.dataManager.GetDicGlobalTextData(_gloState);

                    // if (_gloState.Equals(eState.eFACE.ToString()))
                    if (_eState == eState.eFACE)
                    {

                        _logUp = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eLogStateUp_Other);
                        _logDown = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eLogStateDown_Other);

                        if (value > 0)
                        {
                            //{0} : {1}이(가) {2} 올랐습니다.
                            string _msg = string.Format(_logUp, _strSchedule, _strState, value);
                            _log.SetDayLog(_type, value, _day, _msg);
                        }
                        else
                        {
                            //{0} : {1}이(가) {2} 떨어졌습니다.
                            string _msg = string.Format(_logDown, _strSchedule, _strState, value);
                            _log.SetDayLog(_type, value, _day, _msg);
                        }
                    }
                    else
                    {
                        if (value > 0)
                        {
                            //{0} : {1}이(가) {2} 올랐습니다.
                            string _msg = string.Format(_logUp, _strSchedule, _strState, value);
                            _log.SetDayLog(_type, value, _day, _msg);
                        }
                        else
                        {
                            //{0} : {1}이(가) {2} 떨어졌습니다.
                            string _msg = string.Format(_logDown, _strSchedule, _strState, value);
                            _log.SetDayLog(_type, value, _day, _msg);
                        }

                    }

                    break;
                }
            //// 인지도 상승
            case KEY_AWARENESS_UP:
                {
                    string _strAwareness = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eLogAwarenessUp);
                    //{0} : {1}이(가) {2} 올랐습니다.
                    string _msg = string.Format(_strAwareness, value);
                    _log.SetDayLog(_type, value, _day, _msg);
                    break;
                }
            //// 체력 다운 
            case KEY_HP_DOWN:
                {
                    string _strHp = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eLogHPDown);
                    // 스케줄 진행으로 체력이 {0} 떨어졌습니다.
                    string _msg = string.Format(_strHp, value);
                    _log.SetDayLog(_type, value, _day, _msg);
                    break;
                }
            case KEY_REST_HP:
                {
                    eGlobalTextKey _gloRest = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(eSchedule.eActRest_0.ToString());

                    string _strRest = SceneBase.Instance.dataManager.GetDicGlobalTextData(_gloRest);
                    string _strHP = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eHP);

                    //{0} : {1}이(가) {2} 올랐습니다.
                    string _msg = string.Format(_logUp, _strRest, _strHP, value);
                    _log.SetDayLog(_type, value, _day, _msg);

                    break;
                }
            case KEY_REST_STATE:
                {
                    //{0} : {1}이(가) {2} 올랐습니다.
                    string _msg = _logAllDown;
                    _log.SetDayLog(_type, -1, _day, _msg);
                    break;
                }
            case KEY_SUDDEN_START:
                {
                    // //// 돌발이벤트 '{0}'을(를) 진행합니다.
                    string _msg = _title.Equals("") == false ? _title : "NULL";
                    string _strStart = string.Format(_logSuddenStart, _msg);
                    _log.SetDayLog(_type, -1, _day, _strStart);
                    break;
                }
            /// 돌발이벤트 종료시 스텟가감
            case KEY_SUDDEN_END:
                {
                    /// string _day, int _value, string _message
                    _log.SetDayLog(_day, value, _msgText);
                    Debug.LogError("KEY_SUDDEN_END : " + _msgText);
                    break;
                }
            //// 매니저 효과
            case KEY_MANAGER_EFFECT:
                {
                    SceneBase.Instance.PLAY_SFX(eSFX.SFX_10_manager_effect);

                    _log.SetDayLog(_day, value, _msgText);
                    Debug.LogError("KEY_SUDDEN_END : " + _msgText);
                    break;
                }
            /// 아이템 효과
            case KEY_ITEM_EFFECT:
                {
                    SceneBase.Instance.PLAY_SFX(eSFX.SFX_10_manager_effect);

                    _log.SetDayLog(_day, value, _msgText);
                    Debug.LogError("KEY_SUDDEN_END : " + _msgText);
                    break;
                }
        }

        listLog.Add(_log.gameObject);

        tableLog.repositionNow = true;
    }

    ///<summary>
    /// 돌발이벤트 시작시 로그 생성
    ///</summary>
    void ACTION_SUDDEN_START_LOG(string _type, string _title)
    {
        eGlobalTextKey _eday = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(((eWeekly)NextScheduleCount).ToString());
        string _day = SceneBase.Instance.dataManager.GetDicGlobalTextData(_eday);
        StartCoroutine(SetCreateLog(_type, ((eWeekly)NextScheduleCount).ToString(), eSchedule.NONE, eState.NONE, -1, _title));
    }

    ///<summary>
    /// 돌발이벤트 종료시 가감 로그 생성F
    ///</summary>
    async void ACTION_SUDDEN_END_LOG(string _type, string _title, string[] _state, string[] _value)
    {

        eGlobalTextKey _eday = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(((eWeekly)NextScheduleCount).ToString());
        string _day = SceneBase.Instance.dataManager.GetDicGlobalTextData(_eday);


        for (int i = 0; i < _state.Length; i++)
        {
            switch (_state[i])
            {
                case "eHP":
                    {
                        // eLogSuddenEventEndHpUp,//	돌발이벤트 : '{0}' 결과로 체력 {1}이 올랐습니다.
                        // eLogSuddenEventEndHpDown,//	돌발이벤트 : '{0}' 결과로 체력 {1}이 떨어집니다.
                        int _suddenValue = int.Parse(_value[i]);
                        eGlobalTextKey _key = _suddenValue > 0 ? eGlobalTextKey.eLogSuddenEventEndHpUp : eGlobalTextKey.eLogSuddenEventEndHpDown;
                        string _text = SceneBase.Instance.dataManager.GetDicGlobalTextData(_key);
                        string _strText = string.Format(_text, _title, _suddenValue);

                        StartCoroutine(SetCreateLog(_type, ((eWeekly)NextScheduleCount).ToString(), eSchedule.NONE, eState.NONE, _suddenValue, _title, _strText));

                        break;
                    }
                case "eGOLD":
                    {
                        // eLogSuddenEventEndGoldUp,//	돌발이벤트 : '{0}' 결과로 코인을 {1} 잃었습니다.
                        // eLogSuddenEventEndGoldDown,//	돌발이벤트 : '{0}' 결과로 코인을 {1} 얻었습니다.
                        int _suddenValue = int.Parse(_value[i]);
                        eGlobalTextKey _key = _suddenValue > 0 ? eGlobalTextKey.eLogSuddenEventEndGoldUp : eGlobalTextKey.eLogSuddenEventEndGoldDown;
                        string _text = SceneBase.Instance.dataManager.GetDicGlobalTextData(_key);
                        string _strText = string.Format(_text, _title, _suddenValue);
                        Debug.LogError("_strText : " + _strText);
                        StartCoroutine(SetCreateLog(_type, ((eWeekly)NextScheduleCount).ToString(), eSchedule.NONE, eState.NONE, _suddenValue, _title, _strText));


                        break;
                    }
                case "eAWARENESS":
                    {
                        // eLogSuddenEventEndAwarenssUp,//	돌발이벤트 : '{0}' 결과로 인지도 {1}가 올랐습니다.
                        // eLogSuddenEventEndAwarenssDown,//	돌발이벤트 : '{0}' 결과로 인지도 {1}가 떨어졌습니다.
                        int _suddenValue = int.Parse(_value[i]);
                        eGlobalTextKey _key = _suddenValue > 0 ? eGlobalTextKey.eLogSuddenEventEndAwarenssUp : eGlobalTextKey.eLogSuddenEventEndAwarenssDown;
                        string _text = SceneBase.Instance.dataManager.GetDicGlobalTextData(_key);
                        string _strText = string.Format(_text, _title, _suddenValue);
                        StartCoroutine(SetCreateLog(_type, ((eWeekly)NextScheduleCount).ToString(), eSchedule.NONE, eState.NONE, _suddenValue, _title, _strText));

                        break;
                    }
                default:
                    {
                        // eLogSuddenEventEndStateUp,//	돌발이벤트 : '{0}' 결과로 {1}이(가) {2} 올랐습니다.
                        // eLogSuddenEventEndStateDown,//	돌발이벤트 : '{0}' 결과로 {1}이(가) {2} 떨어집니다.
                        eState _eState = RayUtils.Utils.ConvertEnumData<eState>(_state[i]);
                        eGlobalTextKey _stateKey = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(_eState.ToString());
                        string _strState = SceneBase.Instance.dataManager.GetDicGlobalTextData(_stateKey);

                        int _suddenValue = int.Parse(_value[i]);
                        eGlobalTextKey _key = _suddenValue > 0 ? eGlobalTextKey.eLogSuddenEventEndStateUp : eGlobalTextKey.eLogSuddenEventEndStateDown;

                        string _text = SceneBase.Instance.dataManager.GetDicGlobalTextData(_key);
                        string _strText = string.Format(_text, _title, _strState, _suddenValue);

                        StartCoroutine(SetCreateLog(_type, ((eWeekly)NextScheduleCount).ToString(), eSchedule.NONE, _eState, _suddenValue, _title, _strText));
                        break;
                    }
            }

            await System.Threading.Tasks.Task.Delay(500);
        }
    }



    void CreateLogLine()
    {
        GameObject _line = Instantiate(objLine, tableLog.transform) as GameObject;
        _line.transform.localPosition = Vector2.zero;
        _line.transform.localScale = Vector3.one;
        // tableLog.Reposition();
        tableLog.repositionNow = true;

        listLog.Add(_line.gameObject);

    }


    /************************************************************************************************************/
    //// 매니저 아이템 로그 적용 로직
    void ManagerItem()
    {

        STManagerData _managerData = JHManagerManger.Instance.GetMyManager();
        eManager_Type _manager = _managerData.manager;

        STManagerSkill _managerSkill = JHManagerManger.Instance.GetMyManagerSkill();

        int prob = (int)_managerSkill.probability;
        int probVale = (int)_managerSkill.probabilityValue;

        object[] skillData = null;
        object[] itemData = null;
        /// 결과화면
        // gridState.gameObject.SetActive(false);
        // gridResult.gameObject.SetActive(true);
        //// 매니저 스킬이 F일경우에 상승 스텟치 계산
        // for (int i = 0; i < prefabResult.Length; i++)
        for (int i = 0; i < (int)eState.COUNT; i++)
        {
            /// 매니저 고유스킬 
            if (_manager == eManager_Type.MANAGER_F && (eState)i == converManagerSkillToState(_managerSkill.managerSkill))
            {
                int _randomProb = UnityEngine.Random.Range(0, 100);

                if (prob <= _randomProb)
                {
                    skillData = new object[3];
                    /// 확률적용
                    skillData[0] = true;
                    /// 올릴 스텟 적용
                    skillData[1] = converManagerSkillToState(_managerSkill.managerSkill);
                    /// 올릴 포인트 적용 
                    skillData[2] = probVale;
                }
            }
            //// 스텟상승 아이템 가지고 있을경우 
            //// 착용아이템
            string _item = UserInfoManager.Instance.GetEquipManagerItem(eManagerItemAbility.eState.ToString());
            if (string.IsNullOrEmpty(_item) == false)
            {
                eManagerItem converItem = RayUtils.Utils.ConvertEnumData<eManagerItem>(_item);
                //// 아이템 데이터 및 확률적용
                itemData = ManagerItemApply(converItem);
            }

            // prefabResult[i].gameObject.SetActive(true);
            // prefabResult[i].SetResultProgress(((eState)i).ToString(), skillData, itemData);
        }
    }

    /// <summary>
    /// 매니저 아이템 스텟 확률적용
    ///</summary>
    object[] ManagerItemApply(eManagerItem _item)
    {
        object[] itemData = null;
        STManagerItemData _itemData = SceneBase.Instance.dataManager.GetManagerItemData(_item);
        //// 확률
        int _prob = _itemData.prob;
        /// 업포인트 
        int _upPoint = _itemData.upPoint;
        /// 랜덤 확률
        int _random = UnityEngine.Random.Range(0, 100);
        eState stateRandom = (eState)UnityEngine.Random.Range((int)eState.eFACE, (int)eState.eCHARACTER);
        if (_random > _prob)
        {
            itemData = new object[3];
            //// 매니저 아이템 확률 적용 
            itemData[0] = true;
            /// 확률적용 스텟 
            itemData[1] = stateRandom;
            //// 확률적용 포인트 
            itemData[2] = _upPoint;
        }

        return itemData;
    }

    ///<summary>
    /// 로그창 전체보기
    ///</summary>
    void OnClickLog()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        Transform _trans = PopupManager.Instance.RootPopup;
        for (int i = 0; i < listLog.Count; i++)
        {
            Debug.LogError(i + "################# listLog.Name : " + listLog[i].gameObject.name);
        }
        object _data = listLog;
        SceneBase.Instance.AddPopup(_trans, ePopupLayer.PopupLogAll, _data);

    }

    ///<summary>
    /// 매니저스킬 F일 경우 어떤 스텟인지 변환 
    ///</summary>
    eState converManagerSkillToState(eManager_Skill _skillType)
    {
        eState _state = eState.NONE;

        switch (_skillType)
        {
            /// 외모 
            case eManager_Skill.SKILL_F_A: { _state = eState.eFACE; break; }
            /// 연기력
            case eManager_Skill.SKILL_F_B: { _state = eState.eACTING; break; }
            /// 가창력
            case eManager_Skill.SKILL_F_C: { _state = eState.eSINGING; break; }
            /// 유머감각
            case eManager_Skill.SKILL_F_D: { _state = eState.eHUMOR; break; }
            /// 유연성
            case eManager_Skill.SKILL_F_E: { _state = eState.eFLEX; break; }
            /// 지식
            case eManager_Skill.SKILL_F_F: { _state = eState.eKNOW; break; }
            /// 성품
            case eManager_Skill.SKILL_F_G: { _state = eState.ePERSONALITY; break; }
            /// 개성
            case eManager_Skill.SKILL_F_H: { _state = eState.eCHARACTER; break; }
        }

        return _state;

    }

    /************************************************************************************************************/
    ///<summary>
    /// 체력회복 온클릭 메소드
    ///</summary>
    void OnClickHPRecovery()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);


        Transform _root = PopupManager.Instance.RootPopup;
        SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupBuyHPRecovery);



        // STAdMobStartTextData _data = new STAdMobStartTextData();
        // _data.strTitle = eGlobalTextKey.eHPRecoveryTitle.ToString();
        // _data.strDesc = eGlobalTextKey.eHPRecoveryMessage.ToString();
        // _data.strDescSub = eGlobalTextKey.eHPRecoveryMessageDesc.ToString();
        // _data.strBtnText = eGlobalTextKey.eBtnHPRecovery.ToString();

        // _data.vidoeKey = eVideoAds.eVideoHPRecovery.ToString();

        // SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupAdMobStart, _data);
    }

    void ACTION_BUY_RECOVERY_VIDEO(bool falg)
    {
        if (falg == true)
        {
            JHAdsManager.Instance.AsyncShowAdMobVideo(eVideoAds.eVideoHPRecovery);
        }
    }

    ///<summary>
    /// 광고 종료 후 보상
    ///</summary>
    void ACTION_ADMOB_SUCESS(bool flag)
    {
        if (flag == true)
        {
            int clacHP = UserInfoManager.Instance.GetSaveHP() + HP_RECOVERY_AMOUNT;
            UserInfoManager.Instance.SetSaveHP(clacHP);
        }
    }

    ///<summary>
    /// 광고 알림창 종료 후 광고시작
    ///</summary>
    void ACTION_CLOSE_ADMOB_START_POPUP(bool flag)
    {
        if (flag == true)
        {
            SceneBase.Instance.adsManager.AsyncShowAdMobVideo(eVideoAds.eVideoHPRecovery);
            // StartCoroutine(SceneBase.Instance.adsManager.IEShowAdMobVideo(eVideoAds.eVideoHPRecovery));
        }
    }
}
