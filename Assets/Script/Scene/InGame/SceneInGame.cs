using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using StructuredDataType;

public class SceneInGame : JHScene
{

    [Header("케릭터")]
    public GameObject objCharacter;
    public UI2DSprite spriteCharacter;
    public UI2DSpriteAnimation animCharacter;
    public UISprite spriteHPValue;
    public UILabel labelHP;
    public GameObject objChMessage;
    public UILabel labelChMessage;

    string[,] CH_MOVE = new string[,]
    {
        {"Left_01","Left_02","Left_03","Left_04","Left_05","Left_06","Left_07"},
        {"Right_01","Right_02","Right_03","Right_04","Right_05","Right_06","Right_07"},

    };

    [Header("밈 대사")]
    public UIButton btnMimTalk;
    public GameObject objMimTalk;
    public UILabel labelMiomTalk;


    [Header("상단 버튼")]
    public GameObject objTop;
    public GameObject objTopBar; /// 화면비율에 따라 탑바 조정
    public UIButton btnCoin;
    public UILabel labelCoin;
    public UIButton btnDia;
    public UILabel labelDia;
    public UIButton btnSetting;
    public UIButton btnGift; /// 요정의 아이템
    public UILabel labelDayCount;


    [Header("하단 버튼들")]
    public GameObject objActionButtons;
    public UIButton btnState;
    public UILabel labelbtnState;
    public UIButton btnSchedule;
    public UILabel labelbtnSchedule;
    public UIButton btnStore;
    public UILabel labelbtnStore;
    public UIButton btnFunyEve;
    public UILabel labelbtnFunyEve;
    /*********************************************************************************/

    [Header("엔딩")]
    public EndingCheck objEndingCheck;
    public UIButton btnStateRandom;
    public UILabel labelBtnStateRandom;
    public UILabel labelVideoCount;

    public UIButton btnStateFixUp;
    public UILabel labelBtnStateFixtUp;
    public UILabel labelEnidngDiaAmount;


    public UIButton btnShowEnding;
    public UILabel labelBtnShowEnding;

    public TweenAlpha _fadeInOut;


    /*********************************************************************************/
    [Header("테스트 버튼")]
    public UIButton btnStart;
    public UIButton btnTestPopupWeeklyScheduleResult;
    public UIButton btnTestManagerTest;
    public UIButton btnSuddneEventTest;
    public UIButton btnDreamEventTest;
    public UIButton btnEndingTest;
    public UIButton btnEndingAlbumTest;
    public UIButton btnEndingResult;
    public UIButton btnEndingBuy;
    public UIButton btnStateTest;

    /*********************************************************************************/

    Dictionary<int, STMimTalkData> dicMimTalk;
    Dictionary<eWeekly, WeeklyScheduleData> dicWeekSchedule = new Dictionary<eWeekly, WeeklyScheduleData>();
    List<WeeklyScheduleData> listSchedule = new List<WeeklyScheduleData>();

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // SceneBase.Instance.PLAY_BGM(eBGM.BGM_MAIN);

        _fadeInOut.gameObject.SetActive(false);

        SceneBase.labelGold = labelCoin;
        SceneBase.labelDia = labelDia;


        PopupAdMobStart.ACTION_CLOSE_ADMOB_START_POPUP = ACTION_CLOSE_ADMOB_START_POPUP; /// 광고
        PopupAdMobStart.ACTION_RANDOM_STATE = ACTION_CLOSE_ADMOB_START_POPUP;

        PopupScheduleCheck.ACTION_SCHEDULE_DATA = ACTION_WEEK_DATA; /// 스케줄 데이터 
        PopupScheduleStart.ACTION_SCHEDULE_START = ACTION_SCHEDULE_START;
        PopupFairyItem.ACTION_FAIRY_ITEM_STAR = ACTION_FAIRY_ITEM_STAR;


        PopupScheduleAct.ACTION_WEEKLY_RESULT = ACTION_WEEKLY_RESULT; /// 스케줄 진행후 결과 이벤트
        PopupScheduleAct.ACTION_ENDING_CHECK = ACTION_ENDING_CHECK;  //// HP 0 엔딩 진행
        PopupEndingHPEmpty.ACTION_HP_END = ACTION_HP_END;

        PopupScheduleStart.ACTION_ALL_CANCEL = ACTION_ALL_CANCEL;/// 다시선택일 경우 올켄슬
        PopupDreamEvent.ACTION_DREAM_END = ACTION_DREAM_END; /// 꿈이벤트 종료 후 결과 팝업 띄움

        PopupWeeklyScheduleResult.ACTION_SCHEDULE_RESULT = ACTION_SCHEDULE_RESULT; /// 굿잡 나온뒤 액션
        PopupManagerLicense.ACTION_LICENCE = ACTION_LICENCE;
        /// 아이템 구매 후 체력체크 
        PopupStore_ItemBuff.ACTION_ITEM_REFRESH = SetHP;

        PopupSetting.ACTION_RESET = ACTION_RESET;

        // SceneBase.RefreshNotification += RefrashGoldDia;

        btnSetting.onClick.Add(new EventDelegate(OnClickSetting));

        btnState.onClick.Add(new EventDelegate(OnclickState));
        btnSchedule.onClick.Add(new EventDelegate(OnClickWeekSchedule));

        btnStart.onClick.Add(new EventDelegate(OnClickStart));
        btnGift.onClick.Add(new EventDelegate(OnClickGift));

        btnStore.onClick.Add(new EventDelegate(OnClickStore));


        btnMimTalk.onClick.Add(new EventDelegate(OnClickMimTalk));

        /*********************************************************************************/
        //// 엔딩 
        btnStateRandom.onClick.Add(new EventDelegate(OnClickStateRandom));
        btnStateFixUp.onClick.Add(new EventDelegate(OnClickStateFixUp));
        btnShowEnding.onClick.Add(new EventDelegate(OnClickShowEnding));

        /*********************************************************************************/
        btnTestManagerTest.onClick.Add(new EventDelegate(OnClickManagerTest));
        btnSuddneEventTest.onClick.Add(new EventDelegate(OnClickSuddenEventTest));
        btnTestPopupWeeklyScheduleResult.onClick.Add(new EventDelegate(OnClickRewardAnimation));
        btnDreamEventTest.onClick.Add(new EventDelegate(OnClickDreamEventTest));
        btnEndingTest.onClick.Add(new EventDelegate(OnClickEndingTest));
        btnEndingAlbumTest.onClick.Add(new EventDelegate(OnClickEndingAlbumTest));
        btnEndingResult.onClick.Add(new EventDelegate(OnClickEndingResultTest));
        btnEndingBuy.onClick.Add(new EventDelegate(OnClickTestEndingBuy));
        btnStateTest.onClick.Add(new EventDelegate(OnclickState));


        SceneBase.RefrashGoldDia();

        labelDayCount.text = UserInfoManager.Instance.GetDayCount() == 0 ? "1" : (UserInfoManager.Instance.GetDayCount() + 1).ToString();
        labelBtnShowEnding.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eEndingLastAudition);

        int move_x = Random.Range(-240, 247);
        int move_y = Random.Range(0, 298);

        TweenPosition tween = objCharacter.GetComponent<TweenPosition>();
        tween.ResetToBeginning();
        tween.duration = 10;
        tween.enabled = true;
        tween.from = new Vector2(0, 0);
        tween.to = new Vector2(move_x, move_y);
        tween.SetOnFinished(new EventDelegate(TweenPositionFinish));
        // tween.onFinished.Add(new EventDelegate(TweenPositionFinish));

        dicMimTalk = SceneBase.Instance.dataManager.mGetDicMimTalkData();


        float defaultRatio = 9f / 16f;
        float currentRatio = (float)Screen.width / (float)Screen.height;
        Debug.LogError("AAAAAAAAAAA : " + 1440.0f / 3040.0f);

        // Debug.LogError("Screen.width  : " + Screen.width + " // Screen.height : " + Screen.height);
        // Debug.LogError("##################### defaultRatio :  " + defaultRatio);
        // Debug.LogError("##################### currentRatio : " + currentRatio);

        if (currentRatio < defaultRatio)
        {

            if (currentRatio < 0.47f)
            {
                objTopBar.transform.localPosition = new Vector2(0, 140);
                objActionButtons.transform.localPosition = new Vector2(0, -190);
            }
            else
            {
                if (currentRatio < 0.48f)
                {
                    objTopBar.transform.localPosition = new Vector2(0, 100);
                    objActionButtons.transform.localPosition = new Vector2(0, -150);
                }
                else
                {
                    objTopBar.transform.localPosition = new Vector2(0, 70);
                    objActionButtons.transform.localPosition = new Vector2(0, -120);

                }

            }

        }
        string _year = System.DateTime.Now.ToString("yyyy");
        string _month = System.DateTime.Now.ToString("MM");
        string _day = System.DateTime.Now.ToString("dd");
        string strDate = (string.Format("{0}{1}{2}", _year, _month, _day));
        if (string.IsNullOrEmpty(UserInfoManager.Instance.GetFreeVideoDate()) == true)
        {
            UserInfoManager.Instance.SetFreeVideoDate(strDate);
        }
        //// 비디오카운트 초기화
        int beforDay = string.IsNullOrEmpty(UserInfoManager.Instance.GetFreeVideoDate()) == true ? 0 : int.Parse(UserInfoManager.Instance.GetFreeVideoDate());
        int crrDay = int.Parse(strDate);
        if (crrDay > beforDay)
        {
            UserInfoManager.Instance.SetFreeVideoCount(0);
            UserInfoManager.Instance.SetFreeVideoDate(crrDay.ToString());
        }

    }

    void ACTION_RESET(bool flag)
    {
        if (flag == true)
        {
            SceneBase.RefrashGoldDia();

            labelDayCount.text = UserInfoManager.Instance.GetDayCount() == 0 ? "1" : (UserInfoManager.Instance.GetDayCount() + 1).ToString();
            labelBtnShowEnding.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eEndingLastAudition);

            int move_x = Random.Range(-240, 247);
            int move_y = Random.Range(0, 298);

            TweenPosition tween = objCharacter.GetComponent<TweenPosition>();
            tween.ResetToBeginning();
            tween.duration = 10;
            tween.enabled = true;
            tween.from = new Vector2(0, 0);
            tween.to = new Vector2(move_x, move_y);
            tween.SetOnFinished(new EventDelegate(TweenPositionFinish));
            // tween.onFinished.Add(new EventDelegate(TweenPositionFinish));

            dicMimTalk = SceneBase.Instance.dataManager.mGetDicMimTalkData();


            string _year = System.DateTime.Now.ToString("yyyy");
            string _month = System.DateTime.Now.ToString("MM");
            string _day = System.DateTime.Now.ToString("dd");
            string strDate = (string.Format("{0}{1}{2}", _year, _month, _day));
            if (string.IsNullOrEmpty(UserInfoManager.Instance.GetFreeVideoDate()) == true)
            {
                UserInfoManager.Instance.SetFreeVideoDate(strDate);
            }
            //// 비디오카운트 초기화
            int beforDay = string.IsNullOrEmpty(UserInfoManager.Instance.GetFreeVideoDate()) == true ? 0 : int.Parse(UserInfoManager.Instance.GetFreeVideoDate());
            int crrDay = int.Parse(strDate);
            if (crrDay > beforDay)
            {
                UserInfoManager.Instance.SetFreeVideoCount(0);
                UserInfoManager.Instance.SetFreeVideoDate(crrDay.ToString());
            }

            SetHP();
        }
    }



    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        /**********************************************************************************************/
        PopupAdMobStart.ACTION_CLOSE_ADMOB_START_POPUP = null; /// 광고
        PopupAdMobStart.ACTION_RANDOM_STATE = null;

        PopupScheduleCheck.ACTION_SCHEDULE_DATA = null; /// 스케줄 데이터 
        PopupScheduleStart.ACTION_SCHEDULE_START = null;
        PopupFairyItem.ACTION_FAIRY_ITEM_STAR = null;


        PopupScheduleAct.ACTION_WEEKLY_RESULT = null; /// 스케줄 진행후 결과 이벤트
        PopupScheduleAct.ACTION_ENDING_CHECK = null;  //// HP 0 엔딩 진행
        PopupEndingHPEmpty.ACTION_HP_END = null;

        PopupScheduleStart.ACTION_ALL_CANCEL = null;/// 다시선택일 경우 올켄슬
        PopupDreamEvent.ACTION_DREAM_END = null; /// 꿈이벤트 종료 후 결과 팝업 띄움

        PopupWeeklyScheduleResult.ACTION_SCHEDULE_RESULT = null; /// 굿잡 나온뒤 액션
        PopupManagerLicense.ACTION_LICENCE = null;
        /// 아이템 구매 후 체력체크 
        PopupStore_ItemBuff.ACTION_ITEM_REFRESH = null;
    }


    ///<summary>
    /// 매니저 뽑고 하단 UI Enable
    ///</summary>
    void ACTION_LICENCE(bool flag)
    {
        if (flag == true)
        {
            objTop.SetActive(true);
            objActionButtons.SetActive(flag);
            objCharacter.SetActive(true);
        }
    }



    IEnumerator IERePosiotnMim()
    {
        Vector2 _oriPos = new Vector2(objCharacter.transform.localPosition.x, objCharacter.transform.localPosition.y);
        TweenPosition tween = TweenPosition.Begin(objCharacter, 5, Vector2.zero);
        tween.RemoveOnFinished(new EventDelegate(TweenPositionFinish));
        tween.ResetToBeginning();
        int move_x = Random.Range(-240, 247);
        int move_y = Random.Range(0, 298);

        int _index = move_x >= 0 ? 1 : 0;
        // Debug.LogError("move_x : " + move_x + "// _index : " + _index);
        animCharacter.frames[0] = Resources.Load<Sprite>(string.Format("Image/MainRobby/MimMotion/{0}", CH_MOVE[_index, 0]));
        animCharacter.frames[1] = Resources.Load<Sprite>(string.Format("Image/MainRobby/MimMotion/{0}", CH_MOVE[_index, 1]));
        animCharacter.frames[2] = Resources.Load<Sprite>(string.Format("Image/MainRobby/MimMotion/{0}", CH_MOVE[_index, 2]));
        animCharacter.frames[3] = Resources.Load<Sprite>(string.Format("Image/MainRobby/MimMotion/{0}", CH_MOVE[_index, 3]));
        animCharacter.frames[4] = Resources.Load<Sprite>(string.Format("Image/MainRobby/MimMotion/{0}", CH_MOVE[_index, 4]));
        animCharacter.frames[5] = Resources.Load<Sprite>(string.Format("Image/MainRobby/MimMotion/{0}", CH_MOVE[_index, 5]));
        animCharacter.frames[6] = Resources.Load<Sprite>(string.Format("Image/MainRobby/MimMotion/{0}", CH_MOVE[_index, 6]));

        tween.from = _oriPos;
        tween.to = new Vector2(move_x, move_y);
        tween.enabled = true;
        tween.PlayForward();
        tween.SetOnFinished(new EventDelegate(TweenPositionFinish));
        yield return null;
    }

    void TweenPositionFinish()
    {
        StartCoroutine(IERePosiotnMim());
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        //// hp 설정
        SetHP();
    }

    ///<summary>
    /// HP 체크  
    ///</summary>
    void SetHP()
    {
        //// 날짜 카운트
        labelDayCount.text = UserInfoManager.Instance.GetDayCount() == 0 ? "1" : (UserInfoManager.Instance.GetDayCount() + 1).ToString();

        if (UserInfoManager.Instance.GetSaveInAppPrimiumItem(eShopPurchaseKey.eStorePrimium_1.ToString()) == true)
        {
            /// eTextHPInfinity
            labelHP.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eTextHPInfinity);
            labelHP.fontSize = 50;

            spriteHPValue.fillAmount = 1.0f;
        }
        else
        {
            labelHP.text = string.Format("{0}/100", UserInfoManager.Instance.GetSaveHP());
            //// HP 체크
            float crrHP = (float)UserInfoManager.Instance.GetSaveHP() * 0.01f;
            spriteHPValue.fillAmount = crrHP;
        }


        SceneBase.RefrashGoldDia();

    }

    /************************************************************************************************/
    ///// 테스트용 함수
    void OnClickManagerTest()
    {
        Transform _root = PopupManager.Instance.RootPopup;
        SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupManagerTest);
    }

    void OnClickSuddenEventTest()
    {
        Transform _root = PopupManager.Instance.RootPopup;

        // eActSns_1_Event_0_NORMAL

        RayUtils.FormulaManager.SuddenEventFormula _sudden = new RayUtils.FormulaManager.SuddenEventFormula();
        List<STSuddenEventData> listSudden = _sudden.GetSuddenEventLogic(eAct.eActSns, eSchedule.eActSns_1.ToString());
        STSuddenEventCallBackData data = new STSuddenEventCallBackData(listSudden, 0, callbackTest);


        SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupSuddenEvent, data);
    }

    void callbackTest(bool flag)
    {

    }

    void OnClickRewardAnimation()
    {
        Transform _root = PopupManager.Instance.RootPopup;
        SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupWeeklyScheduleResult);
    }

    void OnClickDreamEventTest()
    {
        Transform _root = PopupManager.Instance.RootPopup;
        SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupDreamEvent);
    }

    void OnClickEndingAlbumTest()
    {

    }

    void OnClickEndingResultTest()
    {
        // Transform _root = PopupManager.Instance.RootPopup;
        // STEndingResult data = new STEndingResult(Enums.eEndingNumber.NO_1, true);
        // SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupEndingResult, data);
    }

    void OnClickEndingTest()
    {
        // ACTION_ENDING_CHECK();
        Transform _root = PopupManager.Instance.RootPopup;
        eEndingNumber NO = eEndingNumber.NO_1;
        bool isback = true;
        object[] data = new object[2];
        data[0] = NO;
        data[1] = isback;
        data[2] = false;
        SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupEnding, data);
    }

    void OnClickTestEndingBuy()
    {
        Transform _root = PopupManager.Instance.RootPopup;
        SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupBuyEnding);
    }
    /************************************************************************************************/
    void Update()
    {
        if (PopupManager.Instance.GetPopupCount() > 0)
        {

        }
        else
        {
            if (isActionGame == true)
            {
                return;
            }

        }
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        //// 재화 갱신
        // SceneBase.refresh();
        SetHP();

        if (UserInfoManager.Instance.GetSaveGetManager() == false)
        {
            objTop.SetActive(false);
            objActionButtons.SetActive(false);
            objCharacter.SetActive(false);
            Transform _root = PopupManager.Instance.RootPopup;
            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupManagerTest);
            yield break;
        }
        else
        {
            yield return StartCoroutine(PlayedScheduleCheck());
        }

        //// 날짜가 모두 찼을경우 엔딩 이동
        if (UserInfoManager.Instance.GetDayCount() >= 49 || UserInfoManager.Instance.GetSaveHP() <= 0)
        {
            ACTION_ENDING_CHECK();
        }

        yield return new WaitForSeconds(3.0f);

    }


    // public override void BackPressed()
    // {
    //     base.BackPressed();
    // }

    /**************************************************************************************************************************************************/

    ///<summary>
    /// 스케줄 모두 정하고 종료시 콜백 이벤트
    ///</summary>
    void ACTION_WEEK_DATA(Dictionary<eWeekly, WeeklyScheduleData> data)
    {
        int count = 0;
        if (data != null)
        {
            dicWeekSchedule.Clear();
            dicWeekSchedule = data;
            Debug.LogError("###################### dicWeekSchedule.Count : " + dicWeekSchedule.Count);
            for (int i = 0; i < data.Count; i++)
            {
                count++;
            }
            if (count >= (int)eWeekly.COUNT)
            {
                listSchedule = new List<WeeklyScheduleData>();
                if (listSchedule != null)
                {
                    listSchedule.Clear();
                }

                for (int i = 0; i < (int)eWeekly.COUNT; i++)
                {
                    listSchedule.Add(data[(eWeekly)i]);
                }

            }

            OnClickStart();
        }

    }

    ///<summary>
    /// 인게임 진입시 데이터 체크
    ///</summary>
    IEnumerator PlayedScheduleCheck()
    {
        int realCnt = 0;
        if (listSchedule != null)
        {
            listSchedule.Clear();
        }
        for (int i = 0; i < (int)eWeekly.COUNT; i++)
        {
            yield return new WaitForEndOfFrame();
            string week = UserInfoManager.Instance.GetWeeklyData(((eWeekly)i).ToString());
            // Debug.LogError("########### week : " + week);
            if (week.Equals("") || week.Equals("@"))
            {
                continue;
            }
            else
            {
                /// 데이터가 저장되어있으면 딕셔너리 세팅
                eWeekly _weekly = (eWeekly)i;
                eAct _act = RayUtils.Utils.ConvertEnumData<eAct>(week.Split('@')[0]);
                _act = (_act == eAct.NONE) ? eAct.eActBrod : _act;
                eSchedule _schedule = RayUtils.Utils.ConvertEnumData<eSchedule>(week.Split('@')[1]);

                WeeklyScheduleData data = new WeeklyScheduleData();

                data.weekly = _weekly;
                data.act = _act;
                data.schedule = _schedule;

                dicWeekSchedule.Add(_weekly, data);

                listSchedule.Add(data);
                realCnt++;
            }
            // Debug.LogError("################################# PlayedScheduleCheck : " + week);
        }

        // btnStart.gameObject.SetActive(realCnt >= (int)eWeekly.COUNT);

    }

    /**************************************************************************************************************************************************/
    bool isActionGame = false;
    /// 일정확인 후 진행할 액션함수
    void ACTION_SCHEDULE_START(List<WeeklyScheduleData> list, bool quickStart)
    {
        if (list != null)
        {
            // StartCoroutine(IEBGFade(0, 1));

            object[] data = new object[2];
            data[0] = list;
            data[1] = quickStart;

            Transform _trans = PopupManager.Instance.RootPopup;
            SceneBase.Instance.AddPopup(_trans, ePopupLayer.PopupFairyItem, data);

            // Transform rootPopup = PopupManager.Instance.RootPopup;
            // SceneBase.Instance.AddPopup(rootPopup, ePopupLayer.PopupScheduleAct, data);
            // isActionGame = true;
        }
    }

    void ACTION_FAIRY_ITEM_STAR(List<WeeklyScheduleData> list, bool check)
    {

        if (check == true)
        {
            if (listSchedule.Count >= (int)eWeekly.COUNT)
            {
                // OnClickStart();
                object[] data = new object[2];
                data[0] = list;
                data[1] = UserInfoManager.Instance.GetQuickStart();

                StartCoroutine(IEBGFade(0, 1));
                Transform rootPopup = PopupManager.Instance.RootPopup;
                SceneBase.Instance.AddPopup(rootPopup, ePopupLayer.PopupScheduleAct, data);
                isActionGame = true;
            }
        }
        else
        {
            Transform rootPopup = PopupManager.Instance.RootPopup;
            object[] data = new object[2];
            data[0] = listSchedule;
            data[1] = UserInfoManager.Instance.GetQuickStart();

            SceneBase.Instance.AddPopup(rootPopup, ePopupLayer.PopupScheduleStart, data);
        }

        SceneBase.RefrashGoldDia();
    }



    /**************************************************************************************************************************************************/

    void ACTION_WEEKLY_RESULT()
    {
        //// 꿈이벤트 실행
        //// 7일마다 꿈이벤트 실행
        int dreamEvent = (UserInfoManager.Instance.GetDayCount() + 1);
        if (UserInfoManager.Instance.GetSaveDreamEvent(dreamEvent) == false)
        {
            if ((dreamEvent % 7) == 0)
            {
                //// 꿈이벤트 실행 시 날자 ++;
                //// TODO : 테스트 후 제거
                Transform _root = PopupManager.Instance.RootPopup;
                SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupDreamEvent);
            }
        }

        SetHP();
    }

    ///<summary>
    /// 꿈이벤트 종료 후 실행
    ///</summary>
    void ACTION_DREAM_END()
    {
        STWeeklyResultData data = new STWeeklyResultData(resultCallback);
        Transform rootPopup = PopupManager.Instance.RootPopup;

        //// 주차 체크
        UserInfoManager.Instance.SetWeeklyCount(UserInfoManager.Instance.GetWeeklyCount() + 1);

        //// 꿈이벤트 동작 유무 체크 
        UserInfoManager.Instance.SetSaveDreamEvent(UserInfoManager.Instance.GetDayCount(), true);
        //// 꿈이벤트 종료 후 날짜 카운팅
        UserInfoManager.Instance.SetDayCount(UserInfoManager.Instance.GetDayCount() + 1);

        //// 저장되어있는 요일별데이터 세팅
        for (int i = 0; i < (int)Enums.eWeekly.COUNT; i++)
        {
            UserInfoManager.Instance.SetProgressedWeeklyData(((eWeekly)i).ToString(), false);
            UserInfoManager.Instance.SetWeeklyData(((eWeekly)i).ToString(), "", "");
        }

        dicWeekSchedule.Clear();
        listSchedule.Clear();
        SceneBase.Instance.AddPopup(rootPopup, ePopupLayer.PopupWeeklyScheduleResult, data);

        SetHP();
    }

    /// 결과 콜백
    void resultCallback()
    {
        if (listSchedule != null)
        {
            /// 스케줄데이터 최기화
            dicWeekSchedule.Clear();
            /// 스케줄데이터 리스트 초기화
            listSchedule.Clear();
            btnStart.gameObject.SetActive(false);

            isActionGame = true;
        }

        SetHP();

    }

    /// 결과 콜백 
    void ACTION_SCHEDULE_RESULT()
    {
        Transform _root = PopupManager.Instance.RootPopup;
        SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupSunday);


        if (listSchedule != null)
        {
            listSchedule.Clear();
            btnStart.gameObject.SetActive(false);

            // labelDayCount.text = (UserInfoManager.Instance.GetDayCount() + 1).ToString();
            labelDayCount.text = UserInfoManager.Instance.GetDayCount() == 0 ? "1" : UserInfoManager.Instance.GetDayCount().ToString();

        }

        SceneBase.RefrashGoldDia();




        //// 날짜가 모두 찼을경우 엔딩 이동
        if (UserInfoManager.Instance.GetDayCount() >= 49)
        {
            ACTION_ENDING_CHECK();
        }

        // _fadeInOut.gameObject.SetActive(true);
        // _fadeInOut.ResetToBeginning();
        // _fadeInOut.from = 1;
        // _fadeInOut.to = 0;
        // _fadeInOut.Play();

        StartCoroutine(IEBGFade(1, 0));

        SetHP();

        SceneBase.Instance.PLAY_BGM(eBGM.BGM_MAIN);
    }

    ///<summary>
    /// 스케줄 선택창에서 다시 선택(all Cancel)
    ///</summary>
    void ACTION_ALL_CANCEL(bool flag)
    {
        if (flag == true)
        {
            dicWeekSchedule.Clear();
            listSchedule.Clear();

            OnClickWeekSchedule();

        }
    }

    /**************************************************************************************************************************************************/


    void OnclickState()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        // AddLayer(prefabScheduleResult, "");
        Transform rootPopup = PopupManager.Instance.RootPopup;
        SceneBase.Instance.AddPopup(rootPopup, ePopupLayer.PopupStateDetaile, "");
    }

    ///<summary>
    /// 스케줄 데이터 Set 온클릭 
    ///</summary>
    void OnClickWeekSchedule()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        bool isCheck = false;

        int dreamEvent = (UserInfoManager.Instance.GetDayCount() + 1);
        if ((dreamEvent % 7) == 0)
        {
            if (UserInfoManager.Instance.GetSaveDreamEvent(dreamEvent) == false)
            {
                //// 꿈이벤트 실행 시 날자 ++;
                //// TODO : 테스트 후 제거
                Transform _root = PopupManager.Instance.RootPopup;
                SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupDreamEvent);
                return;
            }
        }


        for (int i = 0; i < (int)eWeekly.COUNT; i++)
        {
            isCheck = UserInfoManager.Instance.GetProgressedWeeklyData(((eWeekly)i).ToString());
        }

        /// 게임이 진행 중이라면 스케줄 체크 못하게 
        if (isCheck == true)
        {
            Debug.LogError("스케줄변경 요지가 있어 일정이 진행중일땐 스케줄변경 못하게 하기위해 return");
            OnClickStart();
            return;
        }
        else
        {
            int dataCount = 0;
            for (int i = 0; i < (int)eWeekly.COUNT; i++)
            {
                if (dicWeekSchedule.Count <= 0)
                {
                    break;
                }
                try
                {
                    WeeklyScheduleData _data = dicWeekSchedule[(eWeekly)i];
                    if (_data != null)
                    {
                        dataCount++;
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("OnClickWeekSchedule WeeklySchedul Dic is Null");
                }
            }

            Transform rootPopup = PopupManager.Instance.RootPopup;
            if (dataCount >= (int)eWeekly.COUNT)
            {
                OnClickStart();
            }
            else
            {
                SceneBase.Instance.AddPopup(rootPopup, ePopupLayer.PopupScheduleCheck);
            }
        }

    }

    void OnClickStart()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);


        Transform rootPopup = PopupManager.Instance.RootPopup;
        object[] data = new object[2];
        data[0] = listSchedule;
        data[1] = UserInfoManager.Instance.GetQuickStart();

        SceneBase.Instance.AddPopup(rootPopup, ePopupLayer.PopupScheduleStart, data);
    }


    ///<summary>
    /// 스케줄 Act 팝업 실행 온클릭F
    ///</summary>
    void OnClickScheduleStart()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        Transform rootPopup = PopupManager.Instance.RootPopup;
        SceneBase.Instance.AddPopup(rootPopup, ePopupLayer.PopupScheduleStart, listSchedule);
    }
    /**************************************************************************************************************************************************/
    ///// 엔딩체크 
    ///<summary>
    /// 게임 중 HP가 0이어서 엔딩이 날경우 
    /// 액션을 태워서 엔딩 체크
    ///</summary>
    void ACTION_ENDING_CHECK()
    {
        if (UserInfoManager.Instance.GetSaveHP() <= 0)
        {
            spriteHPValue.fillAmount = 0;
            labelHP.text = string.Format("{0}/100", 0);

            objEndingCheck.gameObject.SetActive(false);
            objActionButtons.SetActive(false);

            Transform _transform = PopupManager.Instance.RootPopup;
            SceneBase.Instance.AddPopup(_transform, Enums.ePopupLayer.PopupEndingHPEmpty);
        }
        else
        {
            //// HP 체크
            float crrHP = (float)UserInfoManager.Instance.GetSaveHP() * 0.01f;
            spriteHPValue.fillAmount = crrHP;
            labelHP.text = string.Format("{0}/100", UserInfoManager.Instance.GetSaveHP());
            objEndingCheck.gameObject.SetActive(true);
            objActionButtons.SetActive(false);
        }


        StopCoroutine(IERePosiotnMim());
    }

    void ACTION_HP_END(bool flag)
    {
        if (flag == true)
        {
            object[] data = new object[3];
            data[0] = eEndingNumber.NO_1;
            data[1] = false;
            data[2] = false;
            Transform _transform = PopupManager.Instance.RootPopup;
            SceneBase.Instance.AddPopup(_transform, Enums.ePopupLayer.PopupEnding, data);
        }
    }


    bool isActionVideoRandomState = false;
    ///<summary>
    /// 비디오로 랜덤 스텟 상승
    ///<summary>
    void OnClickStateRandom()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        if (UserInfoManager.Instance.GetStateBuyVideo() == false || isActionRewardSucess == false)
        {

            Transform _root = PopupManager.Instance.RootPopup;
            STAdMobStartTextData data = new STAdMobStartTextData();

            data.strTitle = eGlobalTextKey.eStateUpTitle.ToString();
            data.strDesc = eGlobalTextKey.eStateUpMessage.ToString();
            data.strDescSub = eGlobalTextKey.eStateUpMessageDesc.ToString();
            data.strBtnText = eGlobalTextKey.eBtnStateUp.ToString();

            //// 랜덤스텟
            data.vidoeKey = eVideoAds.eVideoRandomStateUp.ToString();

            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupAdMobStart, data);
        }
        else
        {
            string _text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.TextBuyRandomState);
            SceneBase.Instance.AddTestTextPopup(_text);
        }
    }

    bool isActionRewardSucess = false;
    void ACTION_ADMOB_SUCESS(bool flag)
    {
        if (flag == true)
        {
            isActionRewardSucess = flag;
            Transform _root = PopupManager.Instance.RootPopup;
            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupStateRandomUp);
        }
    }

    void ACTION_CLOSE_ADMOB_START_POPUP(bool flag)
    {
        if (flag == true)
        {
            SceneBase.Instance.adsManager.AsyncShowAdMobVideo(eVideoAds.eVideoRandomStateUp);
            if (JHAdsManager.ACTION_ADS_COMPLETE != null)
            {
                JHAdsManager.ACTION_ADS_COMPLETE = null;
            }
            JHAdsManager.ACTION_ADS_COMPLETE = ACTION_ADMOB_SUCESS;
        }
    }



    ///<summary>
    /// 다이아로 확정 스텟 상승
    ///<summary>
    void OnClickStateFixUp()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        if (UserInfoManager.Instance.GetStateBuyDia() == false)
        {
            Transform _root = PopupManager.Instance.RootPopup;
            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupStateFixUp);
        }
        else
        {
            SceneBase.Instance.AddEmptyPurchasePopup("확정 스텟은 1회만 구입가능합니다.");
        }
    }
    ///<summary>
    /// 엔딩보기
    ///<summary>
    void OnClickShowEnding()
    {
        Transform _root = PopupManager.Instance.RootPopup;
        SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupEndingAudition);
    }

    IEnumerator IEBGFade(int from, int to)
    {
        _fadeInOut.gameObject.SetActive(true);
        _fadeInOut.ResetToBeginning();
        _fadeInOut.enabled = true;
        _fadeInOut.from = from;
        _fadeInOut.to = to;
        _fadeInOut.Play();
        yield return new WaitForSeconds(0.7f);
        if (to == 0)
        {
            _fadeInOut.gameObject.SetActive(false);

        }

    }

    /**************************************************************************************************************************************************/
    void MimTouchMessage(Dictionary<int, STMimTalkData> _dic)
    {

        // int _random = Random.Range(0, (_dic.Count - 1));
        // STMimTalkData _data = _dic[_random];
        // string _talk = _data.minTalk;
        // labelMiomTalk.text = _talk;
        // objMimTalk.SetActive(true);
        // Invoke("InvokeTalkEnable", 10.0f);
    }

    void InvokeTalkEnable()
    {
        // objMimTalk.SetActive(false);
    }

    void OnClickMimTalk()
    {
        // MimTouchMessage(dicMimTalk);
    }



    /**************************************************************************************************************************************************/


    void OnClickStore()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        Transform _tran = PopupManager.Instance.RootPopup;
        SceneBase.Instance.AddPopup(_tran, ePopupLayer.PopupStore);

    }

    void OnClickSetting()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        Transform _tran = PopupManager.Instance.RootPopup;
        SceneBase.Instance.AddPopup(_tran, ePopupLayer.PopupSetting);
    }

    void OnClickFunyEve()
    {

    }


    void OnClickGift()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        Transform _root = PopupManager.Instance.RootPopup;
        SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupFairyItem);
    }

}
