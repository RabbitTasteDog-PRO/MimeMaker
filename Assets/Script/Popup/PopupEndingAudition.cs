using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PopupEndingAudition : JHPopup
{

    Enums.eEndingNumber eEndingNum = Enums.eEndingNumber.NONE;


    public GameObject objPanel_0;
    public UILabel labelPanel_0_Name;
    public UILabel labelPanel_0_Message;
    public UIButton btnPanelTalk_0;
    public UIButton btnPanelTalk_1;
    public UIButton btnPanelTalk_2;

    public GameObject objPanel_1;
    public UILabel labelPanel_1_Name;
    public UILabel labelPanel_1_Message;

    public GameObject objPanel_2;
    public UILabel labelPanel_2_Name;
    public UILabel labelPanel_2_Message;

    public GameObject objResult;
    public UILabel labelResult;
    public UILabel labelbtnResult;
    public UIButton btnResult;

    string[] message = new string[3];


    protected override void OnAwake()
    {
        base.OnAwake();

        labelPanel_0_Message.text = "";
        labelPanel_1_Message.text = "";
        labelPanel_2_Message.text = "";


        btnResult.onClick.Add(new EventDelegate(OnClickResult));

        labelbtnResult.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eBtnResult);
        labelResult.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eLabelAuditionResult);

        labelPanel_0_Name.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.AUDITION_PANEL_0);
        labelPanel_1_Name.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.AUDITION_PANEL_1);
        labelPanel_2_Name.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.AUDITION_PANEL_2);

        btnPanelTalk_0.onClick.Add(new EventDelegate(OnClickPanelTalk_0));
        btnPanelTalk_1.onClick.Add(new EventDelegate(OnClickPanelTalk_1));
        btnPanelTalk_2.onClick.Add(new EventDelegate(OnClickPanelTalk_2));

        message[0] = ReturnPanelMessage(eAuditionPanel.AUDITION_PANEL_0);
        message[1] = ReturnPanelMessage(eAuditionPanel.AUDITION_PANEL_1);
        message[2] = ReturnPanelMessage(eAuditionPanel.AUDITION_PANEL_2);


        for (int i = 0; i < (int)eAuditionPanel.COUNT; i++)
        {
            Debug.LogError(message[i]);
        }
    }

    protected override void OnDestroied()
    {
        base.OnDestroied();
        Transform _transform = PopupManager.Instance.RootPopup;

        object[] data = new object[3];
        /// 엔딩넘버
        data[0] = eEndingNum;
        //// 백버튼
        data[1] = false;
        //// 상점에서 구매여부
        data[2] = false;

        SceneBase.Instance.AddPopup(_transform, Enums.ePopupLayer.PopupEnding, data);
    }

    protected override void OnClosed()
    {
        base.OnClosed();
    }

    protected override void OnStart()
    {
        base.OnStart();
        StartCoroutine(IENextPopup());
    }


    string ReturnPanelMessage(eAuditionPanel _ePanel)
    {
        string message = "";

        Dictionary<eAuditionPanel, Dictionary<eAuditionState, List<STAuditionPanelData>>> dicData = SceneBase.Instance.dataManager.mGetDicAuditionPanelData();

        switch (_ePanel)
        {
            /// 최고스텟 
            case eAuditionPanel.AUDITION_PANEL_0:
                {
                    eState _eState = (eState)0;
                    int _value = 0;

                    for (int i = 0; i < (int)eState.COUNT; i++)
                    {
                        int crrValue = UserInfoManager.Instance.getSaveState(((eState)i).ToString());

                        if (crrValue > _value)
                        {
                            _eState = (eState)i;
                            _value = crrValue;
                        }
                    }

                    Dictionary<eAuditionState, List<STAuditionPanelData>> dicPanel = dicData[eAuditionPanel.AUDITION_PANEL_0];
                    eAuditionState _convert = RayUtils.Utils.ConvertEnumData<eAuditionState>(_eState.ToString());
                    List<STAuditionPanelData> _list = dicPanel[_convert];

                    int[] gradeValue = new int[_list.Count];

                    for (int i = 0; i < _list.Count; i++)
                    {
                        STAuditionPanelData _data = _list[i];
                        gradeValue[i] = _data.value;
                    }

                    int probIndex = ConvertIndex(_value);
                    message = _list[probIndex].message;
                    break;
                }
            /// 최고인지도
            case eAuditionPanel.AUDITION_PANEL_1:
                {
                    Dictionary<eAuditionState, List<STAuditionPanelData>> dicPanel = dicData[eAuditionPanel.AUDITION_PANEL_1];
                    List<STAuditionPanelData> _list = dicPanel[eAuditionState.eAwareness];

                    int _aws = UserInfoManager.Instance.GetSaveAwareness();
                    message = _list[ConvertIndex(_aws)].message;

                    break;
                }

            /// 평균
            case eAuditionPanel.AUDITION_PANEL_2:
                {
                    Dictionary<eAuditionState, List<STAuditionPanelData>> dicPanel = dicData[eAuditionPanel.AUDITION_PANEL_2];
                    List<STAuditionPanelData> _list = dicPanel[eAuditionState.eAverage];


                    int _totla = 0;
                    for (int i = 0; i < (int)eState.COUNT; i++)
                    {
                        _totla += UserInfoManager.Instance.getSaveState(((eState)i).ToString());
                    }

                    message = _list[ConvertIndex(_totla / (int)eState.COUNT)].message;
                    break;
                }

        }


        return message;

    }

    bool isTyping = false;

    IEnumerator IETypingText(string _type, UILabel label)
    {
        isTyping = true;
        int count = 0;
        // char[] _text = _type.ToCharArray();
        string strReplace = _type.Replace("\\n", "\n");
        // Debug.LogError("strReplace : " + strReplace);
        while (true)
        {
            if (count >= strReplace.Length)
            {
                isTyping = false;
                break;
            }

            yield return new WaitForSeconds(0.05f);
            // string _strText = strReplace[count].ToString();
            label.text += strReplace[count].ToString(); ;
            count += 1;

            SceneBase.Instance.PLAY_SFX(eSFX.SFX_12_Talk2);
        }

        yield return new WaitForSeconds(2.0f);

    }

    ///<summary>
    /// 오디션 결과 로직 
    ///</summary>
    IEnumerator IENextPopup()
    {
        // EndingManager.Instance.getEn
        RayUtils.FormulaManager.EndingFormula fomula = new RayUtils.FormulaManager.EndingFormula();
        // int hp, int day
        int crrHP = UserInfoManager.Instance.GetSaveHP();
        int crrDay = UserInfoManager.Instance.GetDayCount();
        eEndingNum = fomula.GetResultEnding(crrHP, crrDay);
        //// 엔딩세이브
        UserInfoManager.Instance.SetSaveEnding(eEndingNum.ToString(), true);

        yield return new WaitForSeconds(2.0f);
        isTyping = true;
        objPanel_0.SetActive(true);
        objPanel_1.SetActive(false);
        objPanel_2.SetActive(false);
        yield return StartCoroutine(IETypingText(message[0], labelPanel_0_Message));
    }

    void OnClickPanelTalk_0()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        if (isTyping == true)
        {
            return;
        }

        objPanel_0.SetActive(false);
        objPanel_1.SetActive(true);
        objPanel_2.SetActive(false);

        StartCoroutine(IETypingText(message[1], labelPanel_1_Message));
    }

    void OnClickPanelTalk_1()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        if (isTyping == true)
        {
            return;
        }

        objPanel_0.SetActive(false);
        objPanel_1.SetActive(false);
        objPanel_2.SetActive(true);

        StartCoroutine(IETypingText(message[2], labelPanel_2_Message));
    }

    void OnClickPanelTalk_2()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        if (isTyping == true)
        {
            return;
        }

        objPanel_0.SetActive(false);
        objPanel_1.SetActive(false);
        objPanel_2.SetActive(false);
        objResult.SetActive(true);

    }


    void OnClickResult()
    {
        OnClosed();
    }

    int ConvertIndex(int _value)
    {
        int _index = -1;
        //// 25 50 75 100
        if (_value >= 75 && _value <= 100)
        {
            _index = 3;
        }
        else if (_value >= 50 && _value <= 74)
        {
            _index = 2;
        }
        else if (_value > 25 && _value <= 49)
        {
            _index = 1;
        }
        else
        {
            _index = 0;
        }
        return _index;
    }



}
