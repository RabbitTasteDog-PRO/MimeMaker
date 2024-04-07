using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System.Linq;
using System.Linq.Expressions;

namespace RayUtils.FormulaManager
{

    public class EndingFormula
    {
        //// 일반엔딩
        const int ENTERTAIN_STANDARD = 50;
        const int ENDING_KIND = 40;

        public Enums.eEndingNumber GetResultEnding(int hp, int day)
        {

            Enums.eEndingNumber eEnding = Enums.eEndingNumber.NONE;

            ///// HP가 0 일경우 체력고갈 엔딩
            if (hp <= 0)
            {
                eEnding = Enums.eEndingNumber.NO_1;
            }
            else
            {

                /// 인지도 세팅
                int _awareness = UserInfoManager.Instance.GetSaveAwareness();

                Dictionary<eState, int> dicState = new Dictionary<eState, int>();

                for (int i = 0; i < (int)eState.COUNT; i++)
                {
                    dicState.Add((eState)i, UserInfoManager.Instance.getSaveState(((eState)i).ToString()));
                }
                // var dic = dicState.OrderBy(i => i.Value);
                var dic = dicState.OrderByDescending(i => i.Value);
                // Use OrderBy method.
                int count = 0;

                // 1,2위 스텟 및 점수 설정
                eState _first = eState.NONE;
                int _firstValue = 0;
                eState _secound = eState.NONE;
                int _secoundValue = 0;

                foreach (var item in dic)
                {
                     if (count == 0)
                    {
                        _first = item.Key;
                        _firstValue = item.Value;
                    }
                    if (count == 1)
                    {
                        _secound = item.Key;
                        _secoundValue = item.Value;
                    }

                    count++;

                    if (count >= 2)
                    {
                        break;
                    }
                }


                //// 1,2위 스텟 및 점수 설정
                // eState _first = eState.NONE;
                // int _firstValue = 0;
                // eState _secound = eState.NONE;
                // int _secoundValue = 0;

                // foreach (var item in dicState.OrderBy(i => i.Key))
                // {
                //     if (count == 0)
                //     {
                //         _first = item.Key;
                //         _firstValue = item.Value;
                //     }
                //     if (count == 1)
                //     {
                //         _secound = item.Key;
                //         _secoundValue = item.Value;
                //     }
                //     count++;

                //     if (count >= 2)
                //     {
                //         break;
                //     }
                // }

                //  예시) 가장 높은 스탯: 연기력 80, 두번째로 높은 스탯: 유머감각 20, 인지도 50인 경우								
                //  80 x 50/100 = 40이므로 비연예인 엔딩,							
                // 	80 - 20 = 60이므로 배드엔딩,							
                // 	가장 높은 스탯이 연기력이므로 연기 계열 엔딩 =	엔딩 no.5 출력							
                // int endingKind = _firstValue * (_awareness / 100);
                // Debug.LogError("################# endingKind : " + endingKind);

                float endingKind = ((float)_firstValue * 1.0f) * ( ((float)_awareness * 1.0f)  / 100.0f);
                Debug.LogError("################# endingKind Float : " + endingKind);
                
                // "가장높은 스탯 x 인지도/100 의 값이 50 미만인 경우"
                /// 비연예인 
                if (endingKind <= ENTERTAIN_STANDARD)
                {
                    // (가장 높은 스탯) - (두 번째로 높은 스탯) 의 값이 40 미만인 경우
                    int stateRank = _firstValue - _secoundValue;
                    /// 베드인딩 
                    if (stateRank >= ENDING_KIND)
                    {

                        switch (_first)
                        {
                            case eState.eFACE: { eEnding = eEndingNumber.NO_3; break; }
                            case eState.eACTING: { eEnding = eEndingNumber.NO_5; break; }
                            case eState.eSINGING: { eEnding = eEndingNumber.NO_7; break; }
                            case eState.eHUMOR: { eEnding = eEndingNumber.NO_9; break; }
                            case eState.eFLEX: { eEnding = eEndingNumber.NO_11; break; }
                            case eState.eKNOW: { eEnding = eEndingNumber.NO_13; break; }
                            case eState.ePERSONALITY: { eEnding = eEndingNumber.NO_15; break; }
                            case eState.eCHARACTER: { eEnding = eEndingNumber.NO_17; break; }
                        }
                    }
                    // (가장 높은 스탯) - (두 번째로 높은 스탯) 의 값이 41 이상인 경우
                    //// 굿엔딩
                    else
                    {
                        switch (_first)
                        {
                            case eState.eFACE: { eEnding = eEndingNumber.NO_2; break; }
                            case eState.eACTING: { eEnding = eEndingNumber.NO_4; break; }
                            case eState.eSINGING: { eEnding = eEndingNumber.NO_6; break; }
                            case eState.eHUMOR: { eEnding = eEndingNumber.NO_8; break; }
                            case eState.eFLEX: { eEnding = eEndingNumber.NO_10; break; }
                            case eState.eKNOW: { eEnding = eEndingNumber.NO_12; break; }
                            case eState.ePERSONALITY: { eEnding = eEndingNumber.NO_14; break; }
                            case eState.eCHARACTER: { eEnding = eEndingNumber.NO_16; break; }
                        }
                    }

                }
                // "가장높은 스탯 x 인지도/100 의 값이 50 이상인 경우"
                ///연예인 
                else
                {
                    // (가장 높은 스탯) - (두 번째로 높은 스탯) 의 값이 40 미만인 경우
                    int stateRank = _firstValue - _secoundValue;
                    /// 베드인딩 
                    if (stateRank >= ENDING_KIND)
                    {
                        switch (_first)
                        {
                            case eState.eFACE: { eEnding = eEndingNumber.NO_18; break; }
                            case eState.eACTING: { eEnding = eEndingNumber.NO_20; break; }
                            case eState.eSINGING: { eEnding = eEndingNumber.NO_22; break; }
                            case eState.eHUMOR: { eEnding = eEndingNumber.NO_24; break; }
                            case eState.eFLEX: { eEnding = eEndingNumber.NO_26; break; }
                            case eState.eKNOW: { eEnding = eEndingNumber.NO_28; break; }
                            case eState.ePERSONALITY: { eEnding = eEndingNumber.NO_30; break; }
                            case eState.eCHARACTER: { eEnding = eEndingNumber.NO_32; break; }
                        }
                    }
                    // (가장 높은 스탯) - (두 번째로 높은 스탯) 의 값이 41 이상 경우
                    //// 굿엔딩
                    else
                    {
                        switch (_first)
                        {
                            case eState.eFACE: { eEnding = eEndingNumber.NO_19; break; }
                            case eState.eACTING: { eEnding = eEndingNumber.NO_21; break; }
                            case eState.eSINGING: { eEnding = eEndingNumber.NO_23; break; }
                            case eState.eHUMOR: { eEnding = eEndingNumber.NO_25; break; }
                            case eState.eFLEX: { eEnding = eEndingNumber.NO_27; break; }
                            case eState.eKNOW: { eEnding = eEndingNumber.NO_29; break; }
                            case eState.ePERSONALITY: { eEnding = eEndingNumber.NO_31; break; }
                            case eState.eCHARACTER: { eEnding = eEndingNumber.NO_33; break; }
                        }
                    }
                }
                Debug.LogError("First State : " + _first.ToString() + "// 연예,비연예인 : " + (endingKind <= 50 ? "비연예인" : "연예인") + " // 넘버 : " + eEnding.ToString());
            }

            return eEnding;
        }

    }




    public class SuddenEventFormula
    {
        //// 기본 확률은 5% 
        //// 아이템에 따라 확률 번경 
        const int SUDDEN_PROB = 5;

        //// 아이템 적용에 따라서 퍼센트 변경해야함
        public int GetSuddenEvetProb()
        {
            //// 얻은 아이템에 따라 확률 달라지게 하기위함
            int _eventProb = SUDDEN_PROB;

            return _eventProb;
        }

        public List<STSuddenEventData> GetSuddenEventLogic(Enums.eAct _act, string _schedule)
        {
            try
            {
                /// 모든데이터 가져와옴 
                Dictionary<eAct, Dictionary<string, Dictionary<eSuddenEventGrade, List<STSuddenEventData>>>> _tempAll = SceneBase.Instance.dataManager.GetDicScheduleToSuddenVent();
                //// 스케줄별 데이터
                Dictionary<string, Dictionary<eSuddenEventGrade, List<STSuddenEventData>>> _tempDic = _tempAll[_act];

                string strSchedule = "";
                if (_act == eAct.eActRest)
                {
                    strSchedule = string.Format("{0}_{1}", _act, _schedule);
                }
                else
                {
                    strSchedule = eSchedule.eActRest_0.ToString();
                }
                Dictionary<eSuddenEventGrade, List<STSuddenEventData>> _temp = _tempDic[_schedule];
                List<STSuddenEventData> _listData = new List<STSuddenEventData>();

                for (int i = 0; i < (int)eSuddenEventGrade.COUNT; i++)
                {
                    if (_temp.ContainsKey((eSuddenEventGrade)i) == true)
                    {
                        if ((eSuddenEventGrade)i == eSuddenEventGrade.SPECIAL)
                        {
                            List<STSuddenEventData> _list = _temp[eSuddenEventGrade.SPECIAL];
                            for (int u = 0; u < _list.Count; u++)
                            {
                                STSuddenEventData _data = _list[u];
                                string _state = (_data.keyState).ToString();
                                int _value = _data.value;

                                if (UserInfoManager.Instance.getSaveState(_state) >= _value)
                                {
                                    _listData.Add(_data);
                                }

                            }
                        }
                        else
                        {
                            List<STSuddenEventData> _list = _temp[(eSuddenEventGrade)i];
                            for (int k = 0; k < _list.Count; k++)
                            {
                                STSuddenEventData STdata = _list[k];
                                _listData.Add(STdata);
                            }
                        }

                    }
                }

                if (_tempDic.ContainsKey(strSchedule) == true)
                {
                    Dictionary<eSuddenEventGrade, List<STSuddenEventData>> _data = _tempDic[strSchedule];
                    for (int i = 0; i < (int)eSuddenEventGrade.COUNT; i++)
                    {
                        if (_data.ContainsKey((eSuddenEventGrade)i) == true)
                        {
                            if ((eSuddenEventGrade)i == eSuddenEventGrade.SPECIAL)
                            {
                                List<STSuddenEventData> _list = _data[eSuddenEventGrade.SPECIAL];
                                for (int u = 0; u < _list.Count; u++)
                                {
                                    STSuddenEventData _dt = _list[u];
                                    string _state = (_dt.keyState).ToString();
                                    int _value = _dt.value;

                                    if (UserInfoManager.Instance.getSaveState(_state) >= _value)
                                    {
                                        _listData.Add(_dt);
                                    }
                                }
                            }
                            else
                            {
                                List<STSuddenEventData> _list = _data[(eSuddenEventGrade)i];
                                for (int k = 0; k < _list.Count; k++)
                                {
                                    STSuddenEventData stData = _list[k];
                                    _listData.Add(stData);
                                }

                            }

                        }
                    }
                }

                if (_listData == null || _listData.Count <= 0)
                {
                    return null;
                }

                return _listData;

            }
            catch (System.Exception e)
            {
                Debug.LogError("GetSuddenEventLogic _act : " + _act + " // _schedule : " + _schedule);
                return null;
            }

        }

    }
}
