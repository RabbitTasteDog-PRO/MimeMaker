using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;

///<summary>
/// 일정종료 후 처리할 데이터 세팅
///</summary>
public class STSuddenEventCallBackData
{
    public delegate void CallBackDelegate(bool flag);
    public CallBackDelegate noramlCallback = null;

    public int suddenDay;

    public void init()
    {
        if (noramlCallback != null)
        {
            noramlCallback = null;
        }
    }
    public List<STSuddenEventData> listSuddenEvent;
    public STSuddenEventCallBackData(List<STSuddenEventData> _listSuddenEvent, int _suddenDay, CallBackDelegate callback)
    {
        init();
        noramlCallback = callback;
        listSuddenEvent = _listSuddenEvent;
        suddenDay = _suddenDay;
    }

    public void CallBack(bool flag)
    {
        // NormalCallBack callback = 
        if (noramlCallback != null)
        {
            noramlCallback(flag);
        }
    }

}



public class PopupSuddenEvent : JHPopup
{

    //// 타입, 제목
    public static Action<string, string> ACTION_SUDDEN_START_LOG;
    //// 타입, 제목, 결과스텟, 결과벨류
    public static Action<string, string, string[], string[]> ACTION_SUDDEN_END_LOG;


    enum BUTTON_INDEX
    {
        NONE = -1,
        BTN_0,
        BTN_1,
        BTN_2,
        COUNT

    }


    enum STORY_TYPE
    {
        STORY_NORMA = 1000,
        STORY_CHOICE = 1001,
        STORY_CHOICE_END = 1002,
        STORY_END = 1003,
        CHOICE_RESULT = 1004
    }


    public GameObject objAnimation;

    public GameObject objTitle;
    public GameObject objCharactor;
    public GameObject objStory;
    public GameObject objChoice;



    public UI2DSprite imgStoryBG;


    public UILabel labelSuddenEventTitle;

    public UI2DSprite sprite_Ch;
    public UILabel labelName;
    public UILabel labelMessage;

    public GameObject objChoiceResult;
    public UILabel labelChoiceResult;
    public UIButton btnChoiceResult;

    public UIButton btnNext;


    public UIGrid gridChoice;
    public UIButton[] btnChoice;
    public UILabel[] labelBtnChoice;
    public GameObject objQuestionCharacter;
    public UI2DSprite spriteQuestionCh;


    public GameObject objResult;
    public UILabel labelResultText;
    public UIGrid gridState;
    public GameObject[] objResultState;
    public UILabel labelSuddenEventResult;



    // public GameObject objResultLog;
    // public UITable tableResultLog;
    // public PrefabLog _prefabLog;

    public UIButton btnResultColse;


    List<xmlSudenEvent> listSuddenData;
    List<STSuddenEventData> listSuddenEventData;


    string UP_STATE = "stat_plus";
    string DOWN_STATE = "stat_minus";

    string UP_ARROW = "arrow_up_32x36";
    string DOWN_ARROW = "arrow_down_32x36";

    Color32 UP_COLOR = new Color32(21, 144, 255, 255);
    Color32 DOWN_COLOR = new Color32(255, 69, 0, 255);




    int mNext = 0;
    int mTag = 0;

    STSuddenEventCallBackData _dataCallback;
    protected override void OnAwake()
    {
        base.OnAwake();

        btnNext.onClick.Add(new EventDelegate(OnClickNext));
        btnChoiceResult.onClick.Add(new EventDelegate(OnClickNext));
        btnResultColse.onClick.Add(new EventDelegate(OnClosed));

        for (int i = 0; i < btnChoice.Length; i++)
        {
            EventDelegate del = new EventDelegate(this, "OnClickChoice");
            EventDelegate.Parameter parm = new EventDelegate.Parameter();
            parm.value = (BUTTON_INDEX)i;
            parm.expectedType = typeof(BUTTON_INDEX);
            del.parameters[0] = parm;
            EventDelegate.Add(btnChoice[i].onClick, del);
        }
        objChoice.SetActive(false);

        objTitle.SetActive(false);



    }
    int crrSuddenDay = 0;
    STSuddenEventData _randomSuddenData;
    ///<summary>
    /// 행동, 스케줄 데이터 넘겨줌
    ///</summary>
    public override void SetData(object data)
    {
        if (data != null)
        {
            _dataCallback = (STSuddenEventCallBackData)data;

            listSuddenEventData = _dataCallback.listSuddenEvent;
            crrSuddenDay = _dataCallback.suddenDay;
            //// 봉사활동 eActService2 없음 확인필요
            int _random = Mathf.Max(0, UnityEngine.Random.Range(0, listSuddenEventData.Count - 1));
            _randomSuddenData = listSuddenEventData[_random];

        }

    }

    protected override void OnStart()
    {
        base.OnStart();
        if (_randomSuddenData != null)
        {
            //SceneBase.Instance.AddEmptyPurchasePopup("돌발이벤트 적용 : " + _randomSuddenData.fileName);

            StartCoroutine(SetXmlData(_randomSuddenData));
            //// 테스트용
            // StartCoroutine(TESTSuddenEvent());
        }
    }

    protected override void OnDestroied()
    {
        base.OnDestroied();
        if (_dataCallback.noramlCallback != null)
        {
            _dataCallback.CallBack(true);
            _dataCallback.init();
        }

    }


    protected override void OnClosed()
    {
        if (isTyping == true)
        {
            return;
        }

        base.OnClosed();
    }


    IEnumerator TESTSuddenEvent()
    {
        // Debug.LogError("###############################");
        yield return new WaitForEndOfFrame();
        objAnimation.SetActive(true);
        //// 돌발이벤트 애니메이션 적용
        yield return new WaitForSeconds(3.0f);
        objAnimation.SetActive(false);

        if (listSuddenData == null)
        {
            listSuddenData = XmlParser.Read<xmlSudenEvent>("XML/SuddenEvent/eActBrod_1_Event_1_NORMAL");
        }
        /// 타이틀 체크
        labelSuddenEventTitle.text = listSuddenData[0].event_title;
        mNext++;
        SetXmlLayout(listSuddenData[mNext]);
        yield return new WaitForEndOfFrame();
        objTitle.SetActive(true);
        yield return new WaitForEndOfFrame();
        objCharactor.SetActive(true);
        yield return new WaitForEndOfFrame();
        objStory.SetActive(true);
        yield return new WaitForEndOfFrame();
    }


    IEnumerator SetXmlData(STSuddenEventData data)
    {
        objAnimation.SetActive(true);
        //// 돌발이벤트 애니메이션 적용
        yield return new WaitForSeconds(2.0f);
        objAnimation.SetActive(false);
        // string _action = "";
        // string _schedule = "";
        // int _eventIndex = Random.Range(0, 3);

        string eventName = data.fileName;
        string _path = string.Format("XML/SuddenEvent/{0}", eventName);
        Debug.LogError("Suddne Event Naming : " + _path);
        if (listSuddenData == null)
        {
            // listSuddenData = XmlParser.Read<xmlSudenEvent>("XML/SuddenEvent/eActBrod/eActBrod_0/eActBrod_0_Event_0_NORMAL");
            listSuddenData = XmlParser.Read<xmlSudenEvent>(_path);
        }
        /// 타이틀 체크
        labelSuddenEventTitle.text = listSuddenData[0].event_title;

        if (ACTION_SUDDEN_START_LOG != null && ACTION_SUDDEN_START_LOG.GetInvocationList().Length > 0)
        {
            ACTION_SUDDEN_START_LOG("SUDDEN_START", labelSuddenEventTitle.text);
        }


        mNext++;
        SetXmlLayout(listSuddenData[mNext]);
        yield return new WaitForEndOfFrame();
        objCharactor.SetActive(true);
        yield return new WaitForEndOfFrame();
        objStory.SetActive(true);
        objTitle.SetActive(true);
        yield return new WaitForEndOfFrame();



    }

    Color32 COLOR_WHITE = new Color32(80, 38, 21, 255);
    Color32 COLOR_DEFAULT = new Color32(80, 38, 21, 255);
    ///<summary>
    /// 일반 UI 세팅
    ///</summary>
    void SetXmlLayout(xmlSudenEvent data)
    {
        try
        {
            //// 결과화면일 경우
            imgStoryBG.enabled = data.type == (int)STORY_TYPE.CHOICE_RESULT ? false : true;
            labelMessage.color = data.type == (int)STORY_TYPE.CHOICE_RESULT ? COLOR_WHITE : COLOR_DEFAULT;

            /// 이름
            labelName.text = string.IsNullOrEmpty(data.name) == false ? data.name : "";

            /// 케릭터 이미지 
            objCharactor.SetActive(!(string.IsNullOrEmpty(data.ch_img) == true));
            // Debug.LogError("Image/SuddenEvent/Character/ : " + data.ch_img);
            string _image = string.Format("Image/SuddenEvent/Character/{0}", converCHSprite(data.ch_img));
            sprite_Ch.sprite2D = Resources.Load<Sprite>(_image);
            string _message = data.message;

            if (data.type == (int)STORY_TYPE.CHOICE_RESULT)
            {
                objStory.SetActive(false);
                objChoiceResult.SetActive(true);
                labelMessage.gameObject.SetActive(false);
                labelChoiceResult.text = "";
                // labelChoiceResult.text = labelMessage.text = string.IsNullOrEmpty(data.message) == false ? data.message : "";
                StartCoroutine(IETypingText(_message, labelChoiceResult));
            }
            else
            {
                labelMessage.gameObject.SetActive(true);
                objStory.SetActive(true);
                objChoiceResult.SetActive(false);
                if (data.message.Equals("CHOICE_END"))
                {
                    objStory.SetActive(false);
                    labelMessage.text = "";
                }
                else
                {
                    //// 대사출력
                    labelMessage.text = _message;
                }

            }




        }

        catch (System.Exception e)
        {
            Debug.LogError("SetXmlLayout Error : " + e.ToString());
        }

    }

    ///<summary>
    /// 선택지 UI 세팅 
    ///</summary>
    void SetXmlChoiceUI(xmlSudenEvent data)
    {
        string[] msg = new string[3];

        msg[0] = data.choice_0;
        msg[1] = data.choice_1;
        msg[2] = data.choice_2;

        for (int i = 0; i < msg.Length; i++)
        {
            btnChoice[i].gameObject.SetActive(string.IsNullOrEmpty(msg[i]) == false ? true : false);
            labelBtnChoice[i].text = msg[i];
        }

        msg.Initialize();
        gridChoice.Reposition();


    }


    ///<summary>
    /// 선택지 온클릭 매소드
    ///</summary>
    void OnClickChoice(int index)
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);


        SetStateProcess(index);

        mTag = index + 1;
        int nexIndex = mNext;

        objChoice.SetActive(false);
        objQuestionCharacter.SetActive(false);

        while (true)
        {
            if (listSuddenData[nexIndex].tag == mTag)
            {
                mNext = nexIndex;
                SetXmlLayout(listSuddenData[mNext]);
                break;
            }
            nexIndex++;
        }

        objStory.SetActive(true);

    }

    bool isActionNext = false;
    void InvokeIsActionNext()
    {
        isActionNext = false;
    }

    void OnClickNext()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        if (isTyping == true)
        {
            return;
        }

        if (isActionNext == true)
        {
            return;
        }
        isActionNext = true;
        Invoke("InvokeIsActionNext", 0.5f);


        if (mNext >= listSuddenData.Count)
        {
            //// 이벤트 종료 
            Debug.LogError("리스트보다 크크ㅡ크크크크ㅡ크크ㅡ");
        }
        else
        {
            mNext++;
            STORY_TYPE story_type = (STORY_TYPE)listSuddenData[mNext].type;
            switch (story_type)
            {

                case STORY_TYPE.STORY_NORMA:
                    {
                        SetXmlLayout(listSuddenData[mNext]);
                        break;
                    }
                case STORY_TYPE.STORY_CHOICE:
                    {
                        objChoice.SetActive(true);
                        objStory.SetActive(false);
                        objCharactor.SetActive(false);
                        // string strQeustionCh = listSuddenData[mNext - 1].ch_img;
                        string strQeustionCh = listSuddenData[mNext].ch_img;
                        string _imagePath = string.Format("Image/SuddenEvent/Character/{0}", converCHSprite(strQeustionCh));
                        spriteQuestionCh.sprite2D = Resources.Load<Sprite>(_imagePath);
                        objQuestionCharacter.SetActive(true);

                        SetXmlChoiceUI(listSuddenData[mNext]);
                        break;
                    }
                case STORY_TYPE.CHOICE_RESULT:
                    {
                        SetXmlLayout(listSuddenData[mNext]);

                        break;
                    }
                case STORY_TYPE.STORY_CHOICE_END:
                    {
                        int endIndex = mNext;

                        while (true)
                        {
                            if (listSuddenData[endIndex].tag == 0)
                            {
                                mNext = endIndex;
                                break;
                            }
                            endIndex++;
                        }
                        labelName.text = "";
                        if (listSuddenData[mNext].type == (int)STORY_TYPE.STORY_END)
                        {
                            string _text = objResultState[0].transform.Find("LabelState").GetComponent<UILabel>().text;
                            if (string.IsNullOrEmpty(_text) == true || _text.Contains("NONE"))
                            {
                                OnClosed();
                            }
                            else
                            {
                                // 마음에 안들면 롤백 
                                objChoice.SetActive(false);
                                objCharactor.SetActive(false);
                                objResult.SetActive(true);
                                gridState.Reposition();

                                imgStoryBG.enabled = false;
                                labelMessage.color = Color.white;

                                labelResultText.text = listSuddenData[mNext - 1].message;
                                labelResultText.alignment = NGUIText.Alignment.Center;


                            }
                            Debug.LogError("STORY_TYPE.STORY_END : " + listSuddenData[mNext - 1].message + "// mNext : " + mNext);

                        }
                        else
                        {
                            // 마음에 안들면 롤백 
                            objResult.SetActive(true);
                            gridState.Reposition();
                            imgStoryBG.enabled = false;
                            labelMessage.color = Color.white;
                            labelResultText.text = listSuddenData[mNext - 1].message;
                            SetXmlLayout(listSuddenData[mNext]);
                        }

                        labelSuddenEventResult.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eSuddenEventResult);
                        objStory.SetActive(false);

                        break;
                    }
                case STORY_TYPE.STORY_END:
                    {
                        string _text = objResultState[0].transform.Find("LabelState").GetComponent<UILabel>().text;
                        if (string.IsNullOrEmpty(_text) == true || _text.Contains("NONE"))
                        {
                            SceneBase.Instance.PLAY_SFX(eSFX.SFX_13_result_event);

                            OnClosed();
                        }
                        else
                        {
                            objChoice.SetActive(false);
                            objStory.SetActive(false);
                            objCharactor.SetActive(false);
                            // objResultLog.SetActive(false);
                        }

                        Debug.LogError("STORY_TYPE.STORY_END : " + listSuddenData[mNext - 1].message + "// mNext : " + mNext);
                        break;
                    }
            }

        }
    }


    ///<summary>
    /// 스텟 포인트 가감 로직 
    ///</summary>
    void SetStateProcess(int index)
    {
        // OnClosed();

        string[,] arrState = new string[,]
        {
            { listSuddenData[mNext].stateKey_0 , listSuddenData[mNext].stateValue_0 },
            { listSuddenData[mNext].stateKey_1 , listSuddenData[mNext].stateValue_1 },
            { listSuddenData[mNext].stateKey_2 , listSuddenData[mNext].stateValue_2 },
        };


        if (string.IsNullOrEmpty(arrState[index, 0]) == false)
        {
            string[] _state = arrState[index, 0].Split(',');
            // Debug.LogError("#################### _state Lenght :" + _state.Length);
            string[] _stateValue = arrState[index, 1].Split(',');



            //// TODO 결과스텟 
            for (int i = 0; i < objResultState.Length; i++)
            {
                if (_state.Length <= i)
                {
                    objResultState[i].SetActive(false);
                }
                else
                {
                    objResultState[i].SetActive(true);
                }
            }

            for (int i = 0; i < _state.Length; i++)
            {

                Debug.LogError("##################### arrState[ " + i + " , 1] : " + _state[i]);
                Debug.LogError("##################### arrState[ " + i + " , ] : " + _stateValue[i]);

                // 마음에 안들면 롤백 
                eGlobalTextKey _eState = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(_state[i]);
                string _strState = SceneBase.Instance.dataManager.GetDicGlobalTextData(_eState);
                objResultState[i].transform.Find("LabelState").GetComponent<UILabel>().text = _strState;

                int stateValue = int.Parse(_stateValue[i]);
                objResultState[i].transform.Find("LabelPoint").GetComponent<UILabel>().text = stateValue < 0 ? stateValue.ToString() : string.Format("+{0}", stateValue);
                objResultState[i].transform.Find("LabelPoint").GetComponent<UILabel>().color = stateValue < 0 ? DOWN_COLOR : UP_COLOR;

                string imageArrow = stateValue > 0 ? string.Format("Image/Event/{0}", UP_ARROW) : string.Format("Image/Event/{0}", DOWN_ARROW);
                // objResultState[i].transform.Find("ImgState").GetComponent<UI2DSprite>().sprite2D = Resources.Load<Sprite>(imageArrow);

                UI2DSprite _sprite = objResultState[i].transform.Find("ImgState").GetComponent<UI2DSprite>();
                _sprite.sprite2D = Resources.Load<Sprite>(imageArrow);

                if (_sprite.GetComponent<TweenPosition>() == null)
                {
                    _sprite.gameObject.AddComponent<TweenPosition>();
                }

                int _move = stateValue < 0 ? -10 : 10;
                TweenPosition _tween = _sprite.GetComponent<TweenPosition>();
                _tween.ResetToBeginning();
                _tween.from = new Vector2(130.06f, 2);
                _tween.to = new Vector2(130.06f, 2 + _move);
                _tween.style = UITweener.Style.PingPong;
                _tween.duration = 0.5f;
                _tween.PlayForward();


                // string imageBg = stateValue > 0 ? string.Format("Image/Event/{0}", UP_STATE) : string.Format("Image/Event/{0}", DOWN_STATE);
                // objResultState[i].GetComponent<UI2DSprite>().sprite2D = Resources.Load<Sprite>(imageBg);

                // switch (_state[i])
                // {
                //     case "eHP":
                //         {
                //             string eHP = _state[i];
                //             int eValue = int.Parse(_stateValue[i]);
                //             int plusHP = UserInfoManager.Instance.GetSaveHP() + eValue;
                //             UserInfoManager.Instance.SetSaveHP(plusHP);
                //             // Debug.LogError(string.Format("key : {0}, value : {1}, crrHP : {2} ", eHP, eValue, plusHP));
                //             break;
                //         }
                //     case "eGOLD":
                //         {
                //             int eGold = int.Parse(_stateValue[i]) + int.Parse(UserInfoManager.Instance.GetGold());
                //             UserInfoManager.Instance.SetGold(eGold.ToString());
                //             // Debug.LogError(string.Format("key : eGOLD, value : {0}, upValue : {1}, crrValue : {2} ", UserInfoManager.Instance.GetGold(), _stateValue[i], eGold));
                //             break;
                //         }
                //     case "eAWARENESS":
                //         {
                //             string eAWARENESS = _state[i];
                //             int eValue = int.Parse(_stateValue[i]);
                //             int plusAwar = UserInfoManager.Instance.GetSaveAwareness() + eValue;
                //             UserInfoManager.Instance.SetSaveAwareness(plusAwar);
                //             // Debug.LogError(string.Format("key : {0}, value : {1}, CrrAwar : {2} ", eAWARENESS, eValue, plusAwar));
                //             break;
                //         }
                //     default:
                //         {
                //             int _value = int.Parse(_stateValue[i]) + UserInfoManager.Instance.getSaveState(_state[i]);
                //             UserInfoManager.Instance.setSaveState(_state[i], _value);
                //             // Debug.LogError(string.Format("key : {0}, value : {1}, upState : {2} ", _state[i], int.Parse(_stateValue[i]), _value));
                //             break;

                //         }
                // }
            }

            if (ACTION_SUDDEN_END_LOG != null && ACTION_SUDDEN_END_LOG.GetInvocationList().Length > 0)
            {
                ACTION_SUDDEN_END_LOG("SUDDEN_END", labelSuddenEventTitle.text, _state, _stateValue);
            }

            objTitle.SetActive(true);
            // SetCreateSuddenLog(labelSuddenEventTitle.text, _state, _stateValue);
        }

        // gridState.Reposition();

    }

    // ///<summary>
    // /// 돌발이벤트 종료시 가감 로그 생성F
    // ///</summary>
    // async void SetCreateSuddenLog(string _title, string[] _state, string[] _value)
    // {

    //     eGlobalTextKey _eday = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(((eWeekly)crrSuddenDay).ToString());
    //     string _day = SceneBase.Instance.dataManager.GetDicGlobalTextData(_eday);

    //     for (int i = 0; i < _state.Length; i++)
    //     {
    //         switch (_state[i])
    //         {
    //             case "eHP":
    //                 {
    //                     // eLogSuddenEventEndHpUp,//	돌발이벤트 : '{0}' 결과로 체력 {1}이 올랐습니다.
    //                     // eLogSuddenEventEndHpDown,//	돌발이벤트 : '{0}' 결과로 체력 {1}이 떨어집니다.
    //                     int _suddenValue = int.Parse(_value[i]);
    //                     eGlobalTextKey _key = _suddenValue > 0 ? eGlobalTextKey.eLogSuddenEventEndHpUp : eGlobalTextKey.eLogSuddenEventEndHpDown;
    //                     string _text = SceneBase.Instance.dataManager.GetDicGlobalTextData(_key);
    //                     string _strText = string.Format(_text, _title, _suddenValue);

    //                     SetCreateLog(_day, _suddenValue, _strText);

    //                     break;
    //                 }
    //             case "eGOLD":
    //                 {
    //                     // eLogSuddenEventEndGoldUp,//	돌발이벤트 : '{0}' 결과로 코인을 {1} 잃었습니다.
    //                     // eLogSuddenEventEndGoldDown,//	돌발이벤트 : '{0}' 결과로 코인을 {1} 얻었습니다.
    //                     int _suddenValue = int.Parse(_value[i]);
    //                     eGlobalTextKey _key = _suddenValue > 0 ? eGlobalTextKey.eLogSuddenEventEndGoldUp : eGlobalTextKey.eLogSuddenEventEndGoldDown;
    //                     string _text = SceneBase.Instance.dataManager.GetDicGlobalTextData(_key);
    //                     string _strText = string.Format(_text, _title, _suddenValue);
    //                     Debug.LogError("_strText : " + _strText);
    //                     SetCreateLog(_day, _suddenValue, _strText);



    //                     break;
    //                 }
    //             case "eAWARENESS":
    //                 {
    //                     // eLogSuddenEventEndAwarenssUp,//	돌발이벤트 : '{0}' 결과로 인지도 {1}가 올랐습니다.
    //                     // eLogSuddenEventEndAwarenssDown,//	돌발이벤트 : '{0}' 결과로 인지도 {1}가 떨어졌습니다.
    //                     int _suddenValue = int.Parse(_value[i]);
    //                     eGlobalTextKey _key = _suddenValue > 0 ? eGlobalTextKey.eLogSuddenEventEndAwarenssUp : eGlobalTextKey.eLogSuddenEventEndAwarenssDown;
    //                     string _text = SceneBase.Instance.dataManager.GetDicGlobalTextData(_key);
    //                     string _strText = string.Format(_text, _title, _suddenValue);
    //                     SetCreateLog(_day, _suddenValue, _strText);


    //                     break;
    //                 }
    //             default:
    //                 {
    //                     // eLogSuddenEventEndStateUp,//	돌발이벤트 : '{0}' 결과로 {1}이(가) {2} 올랐습니다.
    //                     // eLogSuddenEventEndStateDown,//	돌발이벤트 : '{0}' 결과로 {1}이(가) {2} 떨어집니다.
    //                     eState _eState = RayUtils.Utils.ConvertEnumData<eState>(_state[i]);
    //                     eGlobalTextKey _stateKey = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(_eState.ToString());
    //                     string _strState = SceneBase.Instance.dataManager.GetDicGlobalTextData(_stateKey);

    //                     int _suddenValue = int.Parse(_value[i]);
    //                     eGlobalTextKey _key = _suddenValue > 0 ? eGlobalTextKey.eLogSuddenEventEndStateUp : eGlobalTextKey.eLogSuddenEventEndStateDown;

    //                     string _text = SceneBase.Instance.dataManager.GetDicGlobalTextData(_key);
    //                     string _strText = string.Format(_text, _title, _strState, _suddenValue);

    //                     SetCreateLog(_day, _suddenValue, _strText);

    //                     break;
    //                 }
    //         }
    //         await System.Threading.Tasks.Task.Delay(500);

    //     }
    // }

    // void SetCreateLog(string day, int value, string msg)
    // {
    //     PrefabLog _log = Instantiate(_prefabLog, tableResultLog.transform) as PrefabLog;
    //     _log.transform.localScale = Vector3.one;
    //     _log.SetDayLog(day, value, msg);
    //     tableResultLog.repositionNow = true;
    // }


    bool isTyping = false;

    IEnumerator IETypingText(string _message, UILabel label)
    {
        isTyping = true;
        int count = 0;
        // char[] _text = _type.ToCharArray();
        string strReplace = _message.Replace("\\n", "\n");
        // Debug.LogError("strReplace : " + strReplace);
        while (true)
        {
            if (count >= strReplace.Length)
            {
                isTyping = false;
                break;
            }

            yield return new WaitForSeconds(0.05f);
            SceneBase.Instance.PLAY_SFX(eSFX.SFX_12_Talk2);
            
            // string _strText = strReplace[count].ToString();
            label.text += strReplace[count].ToString(); ;
            count += 1;
        }

        yield return new WaitForSeconds(2.0f);

    }


    ///<summary>
    /// 캐릭터별 이미지 네이밍
    ///</summary>
    string converCHSprite(string image)
    {

        string ch = "";

        switch (image)
        {
            case "MEME":
            case "MiM": { ch = "event_profile_02_MiM_340x340"; break; }
            case "MANAGAER":
            case "MANAGER": { 
                if(UserInfoManager.Instance.GetSaveManagerGender() == false)
                {
                    ch = "event_profile_02_MANAGER_340x340"; 
                }
                else
                {
                    ch = "manager_3_340x340"; 
                }
                
                
                break; }
            case "PD": { ch = "event_profile_02_PD_340x340"; break; }
            case "SENIOR_A": { ch = "event_profile_02_SENIOR_A_340x340"; break; }
            case "AD": { ch = "event_profile_02_AD_340x340"; break; }
            case "COUPLE": { ch = "event_profile_02_couple_340x340"; break; }
            case "PAPARAZZI": { ch = "event_profile_02_PAPARAZZI_340x340"; break; }
            case "SENIOR_I": { ch = "event_profile_02_SENIOR_I_340x340"; break; }
            case "BAD_FAN": { ch = "event_profile_02_BAD_FAN_340x340"; break; }
            case "ATS": { ch = "event_profile_02_ATS_340x340"; break; }
            case "SENIOR_T": { ch = "event_profile_02_SENIOR_T_340x340"; break; }
            case "CEO": { ch = "event_profile_02_CEO_340x340"; break; }
            case "SENIOR_M": { ch = "event_profile_02_SENIOR_M_340x340"; break; }
            case "DANCE_T": { ch = "event_profile_02_DANCE_T_340x340"; break; }
            case "TRAINEE": { ch = "event_profile_02_TRAINEE_340x340"; break; }
            case "PD_NP": { ch = "event_profile_02_PD_NP_340x340"; break; }
            
            case "Audience": 
            case "AUDIENCE_STREET": { ch = "event_profile_02_AUDIENCE_STREET_340x340"; break; }

            case "MISTER_STREET": { ch = "event_profile_02_MISTER_STREET_340x340"; break; }
            case "BOY_STREET": { ch = "event_profile_02_BOY_STREET_340x340"; break; }
            case "DANCE_TEAM": { ch = "event_profile_02_DANCE_TEAM_340x340"; break; }
            case "MC": { ch = "event_profile_02_MC_340x340"; break; }
            case "CUTIE_GIRLS": { ch = "event_profile_02_CUTIE_GIRLS_340x340"; break; }
            case "FRIEND_PHONE": { ch = "event_profile_02_FRIEND_PHONE_340x340"; break; }
            case "DJ": { ch = "event_profile_02_DJ_340x340"; break; }

            case "HEARTCAT": { ch = "event_profile_02_HEARTCAT_340x340"; break; }
            case "YAM":
            case "YaM": { ch = "event_profile_02_YaM_340x340"; break; }
            case "NURSE": { ch = "event_profile_02_NURSE_340x340"; break; }

            case "MRS_HOS": { ch = "event_profile_02_MRS_HOS_340x340"; break; }
            case "MASSAGE": { ch = "event_profile_02_MASSAGE_340x340"; break; }
            case "BEAUTY_FAN": { ch = "event_profile_02_BEAUTY_FAN_340x340"; break; }
            case "TRAINER": { ch = "event_profile_02_TRAINER_340x340"; break; }
            case "CHARLES": { ch = "event_profile_02_CHARLES_340x340"; break; }

            case "MRS_MARKET": { ch = "event_profile_02_MRS_MARKET_340x340"; break; }
            case "CVS": { ch = "event_profile_02_CVS_340x340"; break; }
            case "FRIEND_LTNS": { ch = "event_profile_02_FRIEND_LTNS_340x340"; break; }
            case "FRIEND_LTNS_PHONE": { ch = "event_profile_02_FRIEND_LTNS_PHONE_340x340"; break; }
            case "PHONE": { ch = "event_profile_02_PHONE_340x340"; break; }
        }

        return ch;
    }


}
