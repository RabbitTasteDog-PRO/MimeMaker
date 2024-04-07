using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfoManager : MonoBehaviour
{

    // private static SceneBase _instance = null;

    // public static SceneBase Instance
    // {
    //     ///중복 호출 방지
    //     // [MethodImpl(MethodImplOptions.Synchronized)]
    //     get
    //     {
    //         if (_instance == null)
    //         {
    //             ///싱글톤 객체를 찾아서 넣는다.
    //             _instance = (SceneBase)FindObjectOfType(typeof(SceneBase));

    //             ///없다면 생성한다.
    //             if (_instance == null)
    //             {
    //                 string goName = typeof(SceneBase).ToString();
    //                 GameObject go = GameObject.Find(goName);
    //                 if (go == null)
    //                 {
    //                     go = new GameObject();
    //                     go.name = goName;
    //                 }
    //                 _instance = go.AddComponent<SceneBase>();
    //             }
    //         }
    //         return _instance;
    //     }
    // }

    // public static UserInfoManager Instance()
    // {
    //     if (_instance == null)
    //     {
    //         _instance = new UserInfoManager();
    //     }
    //     return _instance;
    // }


    public static UserInfoManager _instance;
    public static UserInfoManager Instance
    {
        //중복 호출 방지
        // [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            if (_instance == null)
            {
                ///싱글톤 객체를 찾아서 넣는다.
                _instance = (UserInfoManager)FindObjectOfType(typeof(UserInfoManager));

                ///없다면 생성한다.
                if (_instance == null)
                {
                    string goName = typeof(UserInfoManager).ToString();
                    GameObject go = GameObject.Find(goName);
                    if (go == null)
                    {
                        go = new GameObject();
                        go.name = goName;
                    }
                    _instance = go.AddComponent<UserInfoManager>();
                }
            }
            return _instance;

        }
    }

    /// 선물요정 
    public const int FAIRY_GIFT_EVENT_VALE_10 = 10;
    public const int FAIRY_GIFT_GOLD_VALE_2 = 2;
    public const int FAIRY_GIFT_TIME_VALE_3 = 3;

    public const int FAIRY_GIFT_DIA = 2;

    /// 상점 버프아이템
    public const int BUFF_RECOVERY_HP_30 = 30;
    public const int BUFF_RECOVERY_HP_10 = 10;
    public const int BUFF_TIME_VALE_2 = 2;

    public const int PRIMIUM_TIME_VALUE = 5;


    const string TUTORIAL = "tutorial";
    const string GET_MANAGER = "get_manager";
    const string ENDING = "ending"; // 엔딩 타입 
    string DAY_CNT = "day_cnt"; // 데이 카운트 
    string GOLD = "coin";
    string DIA = "dia";
    string DREAM_EVENT = "dream_event";
    string SELECT_BUY_DIA = "select_buy_dia";
    string SELECT_BUY_VIDEO = "select_buy_video";

    string FREE_VIDEO_COUNT = "free_video_count";
    string INAPP_ITEM = "inapp_item";
    string INAPP_ITEM_BUFF = "inapp_item_buff";

    string FAIRY_GIFT_ITEM = "fairy_gift_item";

    string MANAGER_GENDER = "manager_gender";

    string BGM = "bgm";
    string SFX = "sfx";
    string PUSH = "push";
    string PUSH_ADS = "push_ads";
    string PUSH_NIGHT = "push_night";

    //// 최초 시작인지 메니저로 판단 
    public void SetSaveGetManager(bool flag)
    {
        SecurityPlayerPrefs.SetBool(GET_MANAGER, flag);
    }

    public bool GetSaveGetManager()
    {
        return SecurityPlayerPrefs.GetBool(GET_MANAGER, false);
    }

    ///<summary>
    /// 꿈이벤트 중복 안되게 추가 
    ///</summary>
    public void SetSaveDreamEvent(int day, bool flag)
    {
        SecurityPlayerPrefs.SetBool(DREAM_EVENT + "_" + day, flag);
    }
    public bool GetSaveDreamEvent(int day)
    {
        return SecurityPlayerPrefs.GetBool(DREAM_EVENT + "_" + day, false);
    }

    ///<summary>
    /// 일주일 스케줄 세팅 데이터 세이브 Set
    ///</summary>
    public void SetWeeklyData(string week, string act, string schedule)
    {
        SecurityPlayerPrefs.SetString(Strings.WEEKLY + "_" + week, act + "@" + schedule);
    }

    ///<summary>
    /// 일주일 스케줄 세팅 데이터 세이브 Get
    ///</summary>
    public string GetWeeklyData(string week)
    {
        return SecurityPlayerPrefs.GetString(Strings.WEEKLY + "_" + week, "");
    }

    ///<summary>
    /// 몇요일 몇요일이 진행되었는지
    ///</summary>
    public void SetProgressedWeeklyData(string week, bool flag)
    {
        SecurityPlayerPrefs.SetBool(Strings.PROGRESSED + "_" + week, flag);
    }

    public bool GetProgressedWeeklyData(string week)
    {
        bool check = SecurityPlayerPrefs.GetBool(Strings.PROGRESSED + "_" + week, false);
        return check;
    }


    /***********************************************************************************************/
    ///<summary>
    /// 매니저 이름 Set
    ///</summary>
    public void SetMyManagerName(string name)
    {
        SecurityPlayerPrefs.SetString(Strings.MY_MANAGER_NAME, name);
    }
    ///<summary>
    /// 매니저 이름 Get
    ///</summary>
    public string GetMyManagerName()
    {
        return SecurityPlayerPrefs.GetString(Strings.MY_MANAGER_NAME, "");
    }

    ///<summary>
    /// 현재 나의 매니저 Set
    ///</summary>
    public void SetMyManager(string manager)
    {
        SecurityPlayerPrefs.SetString(Strings.MY_MANAGER, manager);
    }
    ///<summary>
    /// 현재 나의 매니저 Get
    ///</summary>
    public string GetMyManager()
    {
        return SecurityPlayerPrefs.GetString(Strings.MY_MANAGER, "MANAGER_A");
    }

    ///<summary>
    /// 현재 매니저의 스킬 Set
    ///</summary>
    public void SetMySkill(string skill)
    {
        SecurityPlayerPrefs.SetString(Strings.MY_SKILL, skill);
    }
    ///<summary>
    /// 현재 매니저의 스킬 Get
    ///</summary>
    public string GetMySkill()
    {
        return SecurityPlayerPrefs.GetString(Strings.MY_SKILL, "SKILL_A");
    }
    ///<summary>
    /// 현재 매니저의 스킬 등급 Set
    ///</summary>
    public void SetMySkillGrade(string grade)
    {
        SecurityPlayerPrefs.SetString(Strings.MY_SKILL_GRADE, grade);
    }
    ///<summary>
    /// 현재 매니저의 스킬 등급 GEt
    ///</summary>
    public string GetMySkillGrade()
    {
        return SecurityPlayerPrefs.GetString(Strings.MY_SKILL_GRADE, "C");
    }

    ///<summary>
    /// 매니저 아이템 획득 유무 Set
    ///</summary>
    public void SetSaveManagerItem(string _mngItem, bool flag)
    {
        SecurityPlayerPrefs.SetBool(Strings.MANAGER_ITEM + "_" + _mngItem, flag);
    }
    ///<summary>
    /// 매니저 아이템 획득 유무 Get
    ///</summary>
    public bool GetSaveManagerItem(string _mngItem)
    {
        return SecurityPlayerPrefs.GetBool(Strings.MANAGER_ITEM + "_" + _mngItem, false);
    }

    ///<summary>
    /// 매니저 아이템 장비 Set
    ///</summary>
    public void SetEquipManagerItem(string equipType, string _item)
    {
        //// eManagerItemAbility  
        // eState, /// 스텟 추가 
        //// eHP, // 체력 추가 
        // eAwareness, /// 인지도 추가 
        // eSuddenEvent, /// 돌발이벤트 확률 상승
        SecurityPlayerPrefs.SetString(Strings.MANAGER_ITEM_EQUIP + "_" + equipType, _item);
    }

    ///<summary>
    /// 매니저 아이템 장비 Get
    ///</summary>
    public string GetEquipManagerItem(string equipType)
    {
        return SecurityPlayerPrefs.GetString(Strings.MANAGER_ITEM_EQUIP + "_" + equipType, "");
    }

    ///<summary>
    /// 인앱상점 스킬 구매 
    ///</summary>
    public void SetSaveInappManagerSKill(string _skill, bool flag)
    {
        SecurityPlayerPrefs.SetBool(Strings.INAPP_BUY_SKILL + "_" + _skill, flag);
    }

    ///<summary>
    /// 인앱상점 스킬 구매 
    ///</summary>
    public bool GetSaveInappManagerSKill(string _skill)
    {
        return SecurityPlayerPrefs.GetBool(Strings.INAPP_BUY_SKILL + "_" + _skill, false);
    }

    /***********************************************************************************************/

    /// 한주가 끝났을 경우에 카운트 총 7주
    public void SetWeeklyCount(int weekCount)
    {
        SecurityPlayerPrefs.SetInt(Strings.WEEK_COUNT, weekCount);
    }

    public int GetWeeklyCount()
    {
        return SecurityPlayerPrefs.GetInt(Strings.WEEK_COUNT, 0);
    }

    //// 요일 카툰트 
    public void SetDayCount(int dayCount)
    {
        SecurityPlayerPrefs.SetInt(Strings.DAY_COUNT, dayCount);
    }

    public int GetDayCount()
    {
        return SecurityPlayerPrefs.GetInt(Strings.DAY_COUNT, 0);
    }

    public void setSaveTutorial(bool flag)
    {
        SecurityPlayerPrefs.SetBool(TUTORIAL, flag);
    }

    public bool getSaveTutorial()
    {
        return SecurityPlayerPrefs.GetBool(TUTORIAL, false);
    }


    public void setSaveState(string state_type, int state)
    {
        if (state <= 0)
        {
            state = 0;
        }

        if(state >= 100)
        {
            state = 100;
        }

        SecurityPlayerPrefs.SetInt(state_type,  state);
    }

    public int getSaveState(string state_type)
    {
        return SecurityPlayerPrefs.GetInt(state_type, 0);
    }

    ///<summary>
    /// 애니메이션 저장용 현재 스텟값 Set
    ///</summary>
    public void SetTempCrrStatePoint(string state, int point)
    {
        SecurityPlayerPrefs.SetString(Strings.STATE_CRR_TEMP + "_" + state, state + "@" + point);
    }

    ///<summary>
    /// 애니메이션 저장용 현재 스텟값 Get
    ///</summary>
    public string GetTempCrrStatePoint(string state)
    {
        return SecurityPlayerPrefs.GetString(Strings.STATE_CRR_TEMP + "_" + state, "");
    }


    ///<summary>
    /// 애니메이션용 가감연출값 Set
    ///</summary>
    public void SetTempAdjustStatePoint(string state, int point)
    {
        SecurityPlayerPrefs.SetString(Strings.STATE_ADJUST_TEMP + "_" + state, state + "@" + point);
    }

    ///<summary>
    /// 애니메이션용 가감연출값 Get
    ///</summary>
    public string GetTempAdjustStatePoint(string state)
    {
        return SecurityPlayerPrefs.GetString(Strings.STATE_ADJUST_TEMP + "_" + state, "");
    }

    ///<summary>
    /// 인게임시 시간줄임 Set
    ///</summary>
    public void SetQuickStart(bool flag)
    {
        SecurityPlayerPrefs.SetBool(Strings.QUICK_START, flag);
    }
    ///<summary>
    /// 인게임시 시간줄임 Get
    ///</summary>
    public bool GetQuickStart()
    {
        return SecurityPlayerPrefs.GetBool(Strings.QUICK_START, false);
    }


    /********************************************************************************/
    /// 골드
    public void SetGold(string coin)
    {

        if (int.Parse(coin) <= 0)
        {
            coin = "0";
        }
        //// 프리미엄아이템 "수익 2배" 적용
        if (GetSaveInAppPrimiumItem(Enums.eShopPurchaseKey.eStorePrimium_4.ToString()) == true)
        {
            int _gDouble = int.Parse(coin) * 2;
            coin = _gDouble.ToString();
        }

        SecurityPlayerPrefs.SetString(GOLD, coin);
    }

    public string GetGold()
    {
        string coin = (SecurityPlayerPrefs.GetInt(GOLD, 0)).ToString();
        return coin;
    }
    /********************************************************************************/
    /// 다이아
    public void SetDia(string dia)
    {
        if (int.Parse(dia) <= 0)
        {
            dia = "0";
        }
        SecurityPlayerPrefs.SetString(DIA, dia);
    }

    public string GetDia()
    {
        string dia = (SecurityPlayerPrefs.GetInt(DIA, 0)).ToString();
        return dia;
    }

    /********************************************************************************/
    /// 주차 결과 보상용 엔딩 카운트 
    public void SetEnidngCount(int cnt)
    {
        int amount = Mathf.Max(0, Mathf.Min(cnt, 70));
        SecurityPlayerPrefs.SetInt(Strings.ENDING_COUNT, cnt);
    }
    /// 주차 결과 보상용 엔딩 카운트 
    public int GetEndingCount()
    {
        return SecurityPlayerPrefs.GetInt(Strings.ENDING_COUNT, 0);
    }


    /// 체력 Save
    public void SetSaveHP(int hp)
    {
        //// 프리미엄 아이템 "무한의 체력"
        if (GetSaveInAppPrimiumItem(Enums.eShopPurchaseKey.eStorePrimium_1.ToString()) == true)
        {
            hp = 100;
        }

        SecurityPlayerPrefs.SetInt(Strings.SAVE_HP, Mathf.Min(100, Mathf.Max(0, hp)));
    }

    /// 체력 Get
    public int GetSaveHP()
    {
        return SecurityPlayerPrefs.GetInt(Strings.SAVE_HP, 100);
    }

    //// 인지도 Save
    public void SetSaveAwareness(int awarness)
    {
        SecurityPlayerPrefs.SetInt(Strings.SAVE_AWARENESS, Mathf.Min(100, Mathf.Max(0, awarness)));
    }
    /// 인지도 Get
    public int GetSaveAwareness()
    {
        return SecurityPlayerPrefs.GetInt(Strings.SAVE_AWARENESS, 0);
    }
    //// 엔딩 세이브 
    public void SetSaveEnding(string id_ed, bool flag)
    {
        SecurityPlayerPrefs.SetBool(ENDING + "_" + id_ed, flag);
    }
    //// 엔딩 세이브
    public bool GetSaveEnding(string id_ed)
    {
        return SecurityPlayerPrefs.GetBool(ENDING + "_" + id_ed, false);
    }

    ///<summary>
    /// 엔딩 후 스텟 비디오 구매 Set 
    ///</summary>
    public void SetStateBuyVideo(bool flag)
    {
        SecurityPlayerPrefs.SetBool(SELECT_BUY_VIDEO, flag);
    }
    ///<summary>
    /// 엔딩 후 스텟 비디오 구매 Get 
    ///</summary>
    public bool GetStateBuyVideo()
    {
        return SecurityPlayerPrefs.GetBool(SELECT_BUY_VIDEO, false);
    }
    ///<summary>
    /// 엔딩 후 스텟 다이아 구매 Set
    ///</summary>
    public void SetStateBuyDia(bool flag)
    {
        SecurityPlayerPrefs.SetBool(SELECT_BUY_DIA, flag);
    }
    ///<summary>
    /// 엔딩 후 스텟 다이하 구매 Get 
    ///</summary>
    public bool GetStateBuyDia()
    {
        return SecurityPlayerPrefs.GetBool(SELECT_BUY_DIA, false);
    }

    ///<summary>
    /// 선물요정 item set
    ///</summary>
    public void SetSaveFairyGiftItem(string key, bool flag)
    {
        SecurityPlayerPrefs.SetBool(FAIRY_GIFT_ITEM + "_" + key, flag);
    }

    ///<summary>
    /// 선물요정 item get
    ///</summary>
    public bool GetSaveFairyGiftItem(string key)
    {
        return SecurityPlayerPrefs.GetBool(FAIRY_GIFT_ITEM + "_" + key, false);
    }

    ///<summary>
    /// 버프아이템 item set
    ///</summary>
    public void SetSaveItemBuff(string key, bool flag)
    {
        SecurityPlayerPrefs.SetBool(INAPP_ITEM_BUFF + "_" + key, flag);
    }

    ///<summary>
    /// 선물요정 item get
    ///</summary>
    public bool GetSaveItemBuff(string key)
    {
        return SecurityPlayerPrefs.GetBool(INAPP_ITEM_BUFF + "_" + key, false);
    }

    ///<summary>
    /// 인앱 프리미엄 아이템
    ///</summary>
    public void SetSaveInAppPrimiumItem(string _key, bool flag)
    {
        SecurityPlayerPrefs.SetBool(INAPP_ITEM + "_" + _key, flag);
    }
    ///<summary>
    /// 인앱 프리미엄 아이템
    ///</summary>
    public bool GetSaveInAppPrimiumItem(string key)
    {
        return SecurityPlayerPrefs.GetBool(INAPP_ITEM + "_" + key, false);
    }



    ///<summary>
    /// 비디오광고 10번 카운트 
    ///</summary>
    public void SetFreeVideoCount(int cnt)
    {
        SecurityPlayerPrefs.SetInt(FREE_VIDEO_COUNT, cnt);

    }

    public int GetFreeVideoCount()
    {
        return SecurityPlayerPrefs.GetInt(FREE_VIDEO_COUNT, 0);
    }

    public void SetFreeVideoDate(string date)
    {
        SecurityPlayerPrefs.SetString("FreeVideoData", date);
    }

    public string GetFreeVideoDate()
    {
        return SecurityPlayerPrefs.GetString("FreeVideoData", "");
    }

    public void SetFreeVideoTimeCalc(string _time)
    {
        SetFreeVideoDate(System.DateTime.Now.ToString("yyyyMMdd"));
        SecurityPlayerPrefs.SetString("FreeVideoTimeCount", _time);
    }

    public string GetFreeVideoTimeClac()
    {
        return SecurityPlayerPrefs.GetString("FreeVideoTimeCount", "");
    }


    ///<summary>
    /// 매니저 성별 false: 남자 , true:여자
    ///</summary>
    public void SetSaveManagerGender(bool gender)
    {
        SecurityPlayerPrefs.SetBool(MANAGER_GENDER, gender);
    }

    ///<summary>
    /// 매니저 성별 false: 남자 , true:여자
    ///</summary>
    public bool GetSaveManagerGender()
    {
        return SecurityPlayerPrefs.GetBool(MANAGER_GENDER, true);
    }

    // 재생 중 true 
    public void SetSaveBGM(bool check)
    {
        SecurityPlayerPrefs.SetBool(BGM, check);
    }

    public bool GetSaveBGM()
    {
        return SecurityPlayerPrefs.GetBool(BGM, true);
    }


    public void SetSaveSFX(bool check)
    {
        SecurityPlayerPrefs.SetBool(SFX, check);
    }

    public bool GetSaveSFX()
    {
        return SecurityPlayerPrefs.GetBool(SFX, true);
    }


    public void SetSavePush(bool check)
    {
        SecurityPlayerPrefs.SetBool(PUSH, check);
    }

    public void SetSavePushAds(bool check)
    {
        SecurityPlayerPrefs.SetBool(PUSH_ADS, check);
    }

    public void SetSavePushNight(bool check)
    {
        SecurityPlayerPrefs.SetBool(PUSH_NIGHT, check);
    }

    public bool GetSavePush()
    {
        return SecurityPlayerPrefs.GetBool(PUSH, true); ;
    }

    public bool GetSavePushAds()
    {
        return SecurityPlayerPrefs.GetBool(PUSH_ADS, true); ;
    }

    public bool GetSavePushNight()
    {
        return SecurityPlayerPrefs.GetBool(PUSH_NIGHT, true); ;
    }

}
