using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class STMimTalkData
{
    int index;
    public string minTalk;

    public STMimTalkData(int _index, string _minTalk)
    {
        minTalk = _minTalk;
        index = _index;
    }

}

///<summary>
/// 글로벌 텍스트 구조체 
///</summary>
public struct STGlobalText
{
    public string key;
    public string kr;
    public string en;
    public string tw;

    public STGlobalText(string _key, string _kr, string _en, string _tw)
    {
        key = _key;
        kr = _kr;
        en = _en;
        tw = _tw;
    }
}


///<summary>
/// 행동 구조체
///</summary>
public struct STActData
{
    // #int	#eActice	#Image	#eScedule	#eScedule	#eScedule	#eScedule
    public eAct _act;
    public eSchedule[] arrScedule;
    public string actImg;

    public STActData(eAct act_, eSchedule[] arr, string _actImg)
    {
        _act = act_;
        arrScedule = arr;
        actImg = _actImg;
    }
}

/// <summary>
/// 매니저 데이터 클래스
/// </summary>
public class xmlManagerTest
{
    public int index = 0;
    public string quest = "";
    public string choice_0 = "";
    public string choice_1 = "";
    public string choice_2 = "";
    public string choice_3 = "";
    public string choice_img_0 = "";
    public string choice_img_1 = "";
    public string choice_img_2 = "";
    public string choice_img_3 = "";
    public string choice_value_0 = "";
    public string choice_value_1 = "";
    public string choice_value_2 = "";
    public string choice_value_3 = "";

}



///<summary>
/// 행동 후 스케쥴 구조체
///</summary>
public struct STScheduleData
{

    public eSchedule key;

    public eState upKey_0;
    public eState upKey_1;
    public eState upKey_2;
    public string upPoint_0;
    public string upPoint_1;
    public string upPoint_2;

    public eState downKey_0;
    public eState downKey_1;
    public eState downKey_2;
    public string downPoint_0;
    public string downPoint_1;
    public string downPoint_2;

    public string background;
    public string anim_img_1;
    public string anim_img_2;

    public string icon;

    public string desc;
    public STScheduleData(
    eSchedule _key, eState _upKey_0, eState _upKey_1, eState _upKey_2,
    string _upPoint_0, string _upPoint_1, string _upPoint_2,
    eState _downKey_0, eState _downKey_1, eState _downKey_2,
    string _downPoint_0, string _downPoint_1, string _downPoint_2,
    string _background, string _anim_img_1, string _anim_img_2,
    string _icon, string _desc)
    {
        key = _key;
        upKey_0 = _upKey_0;
        upKey_1 = _upKey_1;
        upKey_2 = _upKey_2;

        upPoint_0 = _upPoint_0;
        upPoint_1 = _upPoint_1;
        upPoint_2 = _upPoint_2;

        downKey_0 = _downKey_0;
        downKey_1 = _downKey_1;
        downKey_2 = _downKey_2;

        downPoint_0 = _downPoint_0;
        downPoint_1 = _downPoint_1;
        downPoint_2 = _downPoint_2;

        background = _background;
        anim_img_1 = _anim_img_1;
        anim_img_2 = _anim_img_2;

        icon = _icon;

        desc = _desc;
    }

}


public struct STManagerSkill
{
    public eManager_Skill managerSkill;
    public string skillName;
    public eSkillGrade skillGrade;
    public eSkillType skillType;
    public float probability;
    public float probabilityValue;

    public eGlobalTextKey keySkilllName;
    public eGlobalTextKey info_0;
    public eGlobalTextKey info_1;
    public eGlobalTextKey info_value;

    public STManagerSkill(eManager_Skill _managerSkill, string _skillName, eSkillGrade _skillGrade, eSkillType _skillType,
    float _probability, float _probabilityValue, eGlobalTextKey _info_0, eGlobalTextKey _info_1, eGlobalTextKey _info_value,
    eGlobalTextKey _keySkillName)
    {

        managerSkill = _managerSkill;
        skillName = _skillName;
        skillGrade = _skillGrade;
        skillType = _skillType;
        probability = _probability;
        probabilityValue = _probabilityValue;

        info_0 = _info_0;
        info_1 = _info_1;
        info_value = _info_value;

        keySkilllName = _keySkillName;

    }

}

///<summary>
/// 매니저 아이템 데이터 구조체
///</summary>
public struct STManagerItemData
{
    public eManagerItem eItem; /// 아이템 네임
    public string itemImage;
    public eManagerItemAbility eAbility; /// 어떤 능력치 올린건지
    public int prob; /// 확률  (0 ~99 까지 랜덤으로)
    public int upPoint; /// 스텟,체력,인지도 상승 포인트, 돌발이벤트는 획득스텟없고 퍼센트만
    public int price; // 아이템 가격
    public eGlobalTextKey itemDesc;

    public STManagerItemData(eManagerItem _eItem, string _itemImage, eManagerItemAbility _eAbility, int _prob, int _upPoint, int _price, eGlobalTextKey _itemDesc)
    {
        eItem = _eItem;
        itemImage = _itemImage;
        eAbility = _eAbility;
        prob = _prob;
        upPoint = _upPoint;
        price = _price;
        itemDesc = _itemDesc;

    }

}


/// 매니저 기본 구조체
public struct STManagerData
{
    public eManager_Type manager;
    public string managerName;
    public eManager_Skill skill;
    public eGlobalTextKey skillDesc;
    public string managerImage;
    public eGlobalTextKey managerDesc;
    public ePersonality_Type managerPersonality;

    public STManagerData(eManager_Type _manager, string _managerName, eManager_Skill _skill, eGlobalTextKey _skillDesc, string _managerImage, eGlobalTextKey _managerDesc, ePersonality_Type _managerPersonality)
    {
        manager = _manager;
        managerName = _managerName;
        skill = _skill;
        skillDesc = _skillDesc;
        managerImage = _managerImage;
        managerDesc = _managerDesc;
        managerPersonality = _managerPersonality;
    }
}


///<summary>
/// 꿈이벤트 데이터 구조체
///</summary>
public class STDreamEventData
{
    public eDremEvent key_dreamEvent;
    public eSkillType key_skillType;
    public eDreamEventGrade key_grade;
    public int prob;
    public int probValue;

    public string image;
    public string desc;

    public STDreamEventData(eDreamEventGrade _key_grade, eDremEvent _key_dreamEvent, eSkillType _key_skillType, int _prob, int _probValue, string _image, string _desc)
    {
        key_grade = _key_grade;
        key_dreamEvent = _key_dreamEvent;
        key_skillType = _key_skillType;
        prob = _prob;
        probValue = _probValue;
        image = _image;
        desc = _desc;
    }

}

///<summary>
/// 엔딩데이터 구조체 
///</summary>
public struct STEndingData
{
    public eEndingNumber keyNum;
    public eGlobalTextKey keyGlobal;
    public string endingType;
    public string image;
    public string fileName;

    public STEndingData(eEndingNumber _keyNum, eGlobalTextKey _keyGlobal, string _endingType, string _image, string _fileName)
    {
        keyNum = _keyNum;
        keyGlobal = _keyGlobal;
        endingType = _endingType;
        image = _image;
        fileName = _fileName;
    }
}

///<summary>
/// 돌발이벤트 구조체
///</summary>
public class STSuddenEventData
{
    public eAct keyAct;
    public string keySchedule; /// <= 데이터중 공통데이터가 스케줄_4 exception, 스트링으로 받아서 처리하라것 
    public eSuddenEventGrade keyGrade;
    public eSkillType keyState;
    public int value;
    public string fileName;


    public STSuddenEventData(eAct _keyAct, string _keySchedule, eSuddenEventGrade _keyGrade, eSkillType _keyState, int _value, string _fileName)
    {
        keyAct = _keyAct;
        keySchedule = _keySchedule;
        keyGrade = _keyGrade;
        keyState = _keyState;
        value = _value;
        fileName = _fileName;
    }

}


///<summary>
/// 오디션 패널 데이터 
///</summary>
public class STAuditionPanelData
{
    public int index;
    public eAuditionPanel auditionPanel;
    public eAuditionState auditionState;
    public int value;
    public string message;


    public STAuditionPanelData(eAuditionPanel _auditionPanel, eAuditionState _auditionState, int _value, string _message)
    {
        auditionPanel = _auditionPanel;
        auditionState = _auditionState;
        value = _value;
        message = _message;
    }
}

///<summary>
/// 상점 구조체
///</summary>
public struct STShopData
{
    //// DATA_SHOP
    // 인덱스	결제키	결제타입  인앱키_AOS	    인앱키_IOS	     타이틀	    #확인용 한글	설명	수량	가격	할인가격	이미지
    // index	key	type	inappKey_AOS	inappKey_IOS	title	checkHangle	  desc	amount	price	fakePrice	image


    public eShopPurchaseKey eShopKey;
    public eShopPurchaseType ePurchaseType;
    public string AOS_INAPP_KEY;
    public string IOS_INAPP_KEY;
    public string title;
    public string desc;
    public int amount;
    public int price;
    public int fakePrice;
    public string image;

    public STShopData(eShopPurchaseKey _eShopKey, eShopPurchaseType _ePurchaseType,
    string _AOS_INAPP_KEY, string _IOS_INAPP_KEY, string _title, string _desc, int _amount,
    int _price, int _fakePrice, string _image)
    {

        eShopKey = _eShopKey;
        ePurchaseType = _ePurchaseType;
        AOS_INAPP_KEY = _AOS_INAPP_KEY;
        IOS_INAPP_KEY = _IOS_INAPP_KEY;
        title = _title;
        desc = _desc;
        amount = _amount;
        price = _price;
        fakePrice = _fakePrice;
        image = _image;

    }

}

///<summary>
/// 비디오광고 구조체
///</summary>
public struct STVideoAdsData
{
    public eVideoAds eVideoKey;
    public string desc;
    public string aos_id;
    public string ios_id;


    public STVideoAdsData(eVideoAds _videoKey, string _desc, string _aosID, string _iosID)
    {
        eVideoKey = _videoKey;
        desc = _desc;
        aos_id = _aosID;
        ios_id = _iosID;
    }
}


public class JHDataManager : Ray_Singleton<JHDataManager>
{
    // /*************************************************************************/
    /// 글로벌 텍스트
    protected Dictionary<string, STGlobalText> mDicGlobalTextData;
    public string GetDicGlobalTextData(eGlobalTextKey key)
    {
        if (mDicGlobalTextData != null)
        {
            STGlobalText data = mDicGlobalTextData[key.ToString()];

            string text_ = "";
            switch (SceneBase.Instance.SYSTEM_LANGUGE)
            {
                case "kr": { text_ = data.kr; break; }
                case "en": { text_ = data.en; break; }
                case "tw": { text_ = data.tw; break; }

                default: { text_ = data.kr; break; }
            }
            if (string.IsNullOrEmpty(text_) == false)
            {
                return text_.Replace("\\n", "\n");
            }

        }
        return "NULL";
    }


    // /*************************************************************************/
    /// 행동 구조체
    protected Dictionary<eAct, STActData> mDicActData;
    public Dictionary<eAct, STActData> GetDicActiveData()
    {
        return mDicActData;
    }
    public STActData GetSTActiveData(eAct active)
    {
        return mDicActData[active];
    }

    // /*************************************************************************/
    /// 행동 선택 후 스케쥴 데이터 

    protected Dictionary<eAct, Dictionary<eSchedule, STScheduleData>> mDicSceduleData;
    public Dictionary<eAct, Dictionary<eSchedule, STScheduleData>> GetDicSceduleData()
    {
        return mDicSceduleData;
    }
    public Dictionary<eSchedule, STScheduleData> GetDicSceduleData(eAct _active)
    {
        return mDicSceduleData[_active];
    }


    // /*************************************************************************/
    // 매니저 데이터 

    protected Dictionary<eManager_Skill, Dictionary<eSkillGrade, STManagerSkill>> mDicManagerSkillData;
    /// 모든 스킬 데이터
    public Dictionary<eManager_Skill, Dictionary<eSkillGrade, STManagerSkill>> GetDicManagerSkillData()
    {
        return mDicManagerSkillData;
    }
    /// 특정 스킬 데이터
    public Dictionary<eSkillGrade, STManagerSkill> GetDicManagerSkillGradeData(eManager_Skill skill)
    {
        return mDicManagerSkillData[skill];
    }


    /// 매니저 기본 정보 
    protected Dictionary<eManager_Type, STManagerData> mDicManagerData;
    public Dictionary<eManager_Type, STManagerData> GetDicManagerData()
    {
        return mDicManagerData;
    }
    public STManagerData GetSTManagerData(eManager_Type _key)
    {
        return mDicManagerData[_key];
    }

    ///// 매니저 아이템 메타 데이터 
    protected Dictionary<eManagerItem, STManagerItemData> mDicManagerItemData;
    public Dictionary<eManagerItem, STManagerItemData> GetDicManagerItemData()
    {
        return mDicManagerItemData;
    }
    public STManagerItemData GetManagerItemData(eManagerItem _item)
    {
        return mDicManagerItemData[_item];
    }

    //// 상점 정렬용 카테고리별 딕셔너리
    public Dictionary<eManagerItemAbility, Dictionary<eManagerItem, STManagerItemData>> mDicCategoryItem;

    public Dictionary<eManagerItem, STManagerItemData> GetmDicCategoryAbilityItem(eManagerItemAbility _category)
    {
        return mDicCategoryItem[_category];
    }
    public Dictionary<eManagerItemAbility, Dictionary<eManagerItem, STManagerItemData>> GetmDicCategoryAllItem()
    {
        return mDicCategoryItem;
    }


    // /*************************************************************************/
    /// 주차 결과 보상 테이블 
    protected Dictionary<int, int> mDicWeekResultGold;
    public Dictionary<int, int> GetDicWeekResultGold()
    {
        return mDicWeekResultGold;
    }
    public int GetWeekResultGold(int week)
    {
        return mDicWeekResultGold[week];
    }

    // /*************************************************************************/
    //// 꿈 이벤트 테이블 
    protected Dictionary<eDreamEventGrade, List<STDreamEventData>> mDicDreamEventData;
    public Dictionary<eDreamEventGrade, List<STDreamEventData>> GetDicDreamEvent()
    {
        return mDicDreamEventData;
    }
    public List<STDreamEventData> GetListSTDreamEventData(eDreamEventGrade _event)
    {
        return mDicDreamEventData[_event];
    }

    // /*************************************************************************/
    /// 엔딩 테이블 
    protected Dictionary<eEndingNumber, STEndingData> mDicEndingData;
    public Dictionary<eEndingNumber, STEndingData> GetDicEndingData()
    {
        return mDicEndingData;
    }
    public STEndingData GetSTEndingData(eEndingNumber _ending)
    {
        return mDicEndingData[_ending];
    }

    // /*************************************************************************/
    /// 돌발이벤트 데이터 
    protected Dictionary<eAct, Dictionary<string, Dictionary<eSuddenEventGrade, List<STSuddenEventData>>>> mDicSuddenEvent;
    /// 스케줄별 딕셔러니 
    public Dictionary<eAct, Dictionary<string, Dictionary<eSuddenEventGrade, List<STSuddenEventData>>>> GetDicScheduleToSuddenVent()
    {
        return mDicSuddenEvent;
    }

    //// 등급별 딕셔너리
    // public Dictionary<eAct, Dictionary<eSuddenEventGrade, Dictionary<string, List<STSuddenEventData>>>> GetDicGradeToSuddenVent()
    // {
    //     return mDicSuddenEvent;
    // }
    public Dictionary<string, Dictionary<eSuddenEventGrade, List<STSuddenEventData>>> GetDicSuddenEvent(eAct _eSchedule)
    {
        return mDicSuddenEvent[_eSchedule];
    }
    public Dictionary<eSuddenEventGrade, List<STSuddenEventData>> GetDicListSuddenEvent(eAct _eSchedule, string _schedule)
    {
        Dictionary<string, Dictionary<eSuddenEventGrade, List<STSuddenEventData>>> temp = GetDicSuddenEvent(_eSchedule);
        Dictionary<eSuddenEventGrade, List<STSuddenEventData>> _temp = temp[_schedule];
        return _temp;
    }


    // /*************************************************************************/
    ///// 밈 대사 데이터
    protected Dictionary<int, STMimTalkData> mDicMimTalkData;
    public Dictionary<int, STMimTalkData> mGetDicMimTalkData()
    {
        return mDicMimTalkData;
    }
    public STMimTalkData mGetMimTalkData(int _index)
    {
        return mDicMimTalkData[_index];
    }

    // /*************************************************************************/
    //// 오디션 페널 데이터
    protected Dictionary<eAuditionPanel, Dictionary<eAuditionState, List<STAuditionPanelData>>> mDicAuditionPanelData;
    /// 전체 
    public Dictionary<eAuditionPanel, Dictionary<eAuditionState, List<STAuditionPanelData>>> mGetDicAuditionPanelData()
    {
        return mDicAuditionPanelData;
    }
    /// 패널별
    public Dictionary<eAuditionState, List<STAuditionPanelData>> mGetDicListAuditionPanel(eAuditionPanel _paenl)
    {
        return mDicAuditionPanelData[_paenl];
    }
    /// 패널에 해당하는 스텟 리스트
    public List<STAuditionPanelData> GetSTAuditionPanelData(eAuditionPanel _panel, eAuditionState _state)
    {
        Dictionary<eAuditionState, List<STAuditionPanelData>> _data = mDicAuditionPanelData[_panel];
        List<STAuditionPanelData> _list = _data[_state];

        return _list;
    }


    // /*************************************************************************/
    //// 상품데이터

    protected Dictionary<eShopPurchaseKey, STShopData> mDicShopData;

    public Dictionary<eShopPurchaseKey, STShopData> mGetDicShopData()
    {
        return mDicShopData;
    }

    public STShopData mGetSTShopData(eShopPurchaseKey _key)
    {
        return mDicShopData[_key];
    }

    protected Dictionary<eVideoAds, STVideoAdsData> mDicVideoData;
    public STVideoAdsData mGetDicVideoData(eVideoAds _key)
    {
        return mDicVideoData[_key];
    }


    ///<summary>
    /// 데이터 생성 
    ///</summary>
    public void initRelease()
    {
        if (mDicActData == null)
        {
            mDicActData = new Dictionary<eAct, STActData>();
        }

        if (mDicSceduleData == null)
        {
            mDicSceduleData = new Dictionary<eAct, Dictionary<eSchedule, STScheduleData>>();
        }

        if (mDicGlobalTextData == null)
        {
            mDicGlobalTextData = new Dictionary<string, STGlobalText>();
        }

        if (mDicManagerSkillData == null)
        {
            mDicManagerSkillData = new Dictionary<eManager_Skill, Dictionary<eSkillGrade, STManagerSkill>>();
        }

        if (mDicManagerData == null)
        {
            mDicManagerData = new Dictionary<eManager_Type, STManagerData>();
        }

        if (mDicWeekResultGold == null)
        {
            mDicWeekResultGold = new Dictionary<int, int>();
        }

        if (mDicManagerItemData == null)
        {
            mDicManagerItemData = new Dictionary<eManagerItem, STManagerItemData>();
        }

        if (mDicCategoryItem == null)
        {
            mDicCategoryItem = new Dictionary<eManagerItemAbility, Dictionary<eManagerItem, STManagerItemData>>();
        }

        if (mDicDreamEventData == null)
        {
            mDicDreamEventData = new Dictionary<eDreamEventGrade, List<STDreamEventData>>();
        }

        if (mDicEndingData == null)
        {
            mDicEndingData = new Dictionary<eEndingNumber, STEndingData>();
        }

        if (mDicSuddenEvent == null)
        {
            mDicSuddenEvent = new Dictionary<eAct, Dictionary<string, Dictionary<eSuddenEventGrade, List<STSuddenEventData>>>>();
        }

        if (mDicMimTalkData == null)
        {
            mDicMimTalkData = new Dictionary<int, STMimTalkData>();
        }

        if (mDicAuditionPanelData == null)
        {
            mDicAuditionPanelData = new Dictionary<eAuditionPanel, Dictionary<eAuditionState, List<STAuditionPanelData>>>();
        }
        if (mDicShopData == null)
        {
            mDicShopData = new Dictionary<eShopPurchaseKey, STShopData>();
        }

        if (mDicVideoData == null)
        {
            mDicVideoData = new Dictionary<eVideoAds, STVideoAdsData>();
        }

        /// TODO 
        /// 엔딩 데이터 추가 
        /// 매니저 데이터 추가 



    }

    ///<summary>
    /// 데이터 초기화
    ///</summary>
    public void initData()
    {
        if (mDicActData != null)
        {
            mDicActData.Clear();
        }

        if (mDicSceduleData != null)
        {
            mDicSceduleData.Clear();
        }

        if (mDicGlobalTextData != null)
        {
            mDicGlobalTextData.Clear();
        }

        if (mDicManagerSkillData != null)
        {
            mDicManagerSkillData.Clear();
        }
        if (mDicManagerData != null)
        {
            mDicManagerData.Clear();
        }

        if (mDicWeekResultGold != null)
        {
            mDicWeekResultGold.Clear();
        }

        if (mDicManagerItemData != null)
        {
            mDicManagerItemData.Clear();
        }
        if (mDicCategoryItem != null)
        {
            mDicCategoryItem.Clear();
        }

        if (mDicDreamEventData != null)
        {
            mDicDreamEventData.Clear();
        }

        if (mDicEndingData != null)
        {
            mDicEndingData.Clear();
        }

        if (mDicSuddenEvent != null)
        {
            mDicSuddenEvent.Clear();
        }

        if (mDicMimTalkData != null)
        {
            mDicMimTalkData.Clear();
        }

        if (mDicAuditionPanelData != null)
        {
            mDicAuditionPanelData.Clear();
        }
        if (mDicShopData != null)
        {
            mDicShopData.Clear();
        }

        if (mDicVideoData != null)
        {
            mDicVideoData.Clear();
        }


    }

    public void LoadTextData()
    {
        /// DATA_STRING
        SceneBase.Instance.dataReader.ReadTextData("DATA_GLOBAL_TEXT", OnGlobalTextReadLine);
        // DATA_ACTIVE
        SceneBase.Instance.dataReader.ReadTextData("DATA_ACTIVE", OnActDataReadLine);
        // /// DATA_SCHEDULE
        SceneBase.Instance.dataReader.ReadTextData("DATA_SCHEDULE", OnSceduleReadLine);
        /// 스킬테이블
        SceneBase.Instance.dataReader.ReadTextData("DATA_SKILL", OnManagerSkillReadLine);
        // 매니저 테이블 
        SceneBase.Instance.dataReader.ReadTextData("DATA_MANAGER", OnManagerReadLine);
        /// 주차별 골드 테이블
        SceneBase.Instance.dataReader.ReadTextData("DATA_WEEK_GOLD", OnWeekResultGoldReadLine);
        /// 매니저 아이템 테이블
        SceneBase.Instance.dataReader.ReadTextData("DATA_MANAGER_ITEM", OnItem_ManagerReadLine);
        /// 꿈이벤트 테이블 =
        SceneBase.Instance.dataReader.ReadTextData("DATA_DREAM_EVENT", OnDreamEventReadLine);
        //// 엔딩테이블
        SceneBase.Instance.dataReader.ReadTextData("DATA_ENDING", OnEndingDataReadLine);
        //// 돌발이벤트 테이블
        SceneBase.Instance.dataReader.ReadTextData("DATA_SUDDEND_EVENT", OnSuddenEventDataReadLine);
        //// 밈 대사파일
        SceneBase.Instance.dataReader.ReadTextData("DATA_MIM_MESSAGE", OnMimTalkDataReadLine);
        //// 오디션 패널 데이터 
        SceneBase.Instance.dataReader.ReadTextData("DATA_AUDITION_PANEL", OnAuditionPanelDataReadLine);
        /// 상품데이터
        SceneBase.Instance.dataReader.ReadTextData("DATA_SHOP", OnShopDataReadLine);
        /// 상품데이터
        SceneBase.Instance.dataReader.ReadTextData("DATA_VIDOE_ADS", OnVideoAdsDataReadLine);

        // // ENDING_TABLE
        // OnCatEndingTableReadLine();
        // // STORE_DATA
        // OnCatStoreTableReadLine();

    }

    /// <summary>
    /// 글로벌 텍스트 데이터 파싱
    /// </summary>
    void OnGlobalTextReadLine(string _receiveData)
    {
        try
        {
            Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(_receiveData);

            if (dic.ContainsKey("word") == true)
            {
                // 재 료 정 보 갱 신
                JArray arrData = JArray.Parse(dic["word"].ToString());
                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject jObject = arrData[i].ToObject<JObject>();
                    string key = jObject["key"].ToString();

                    string kr = jObject["kr"].ToString();
                    string en = jObject["en"].ToString();
                    string tw = jObject["tw"].ToString();


                    STGlobalText data = new STGlobalText(key, kr, en, tw);

                    if (mDicGlobalTextData.ContainsKey(key) == false)
                    {
                        mDicGlobalTextData.Add(key, data);
                    }

                }

            }
        }
        catch (UnityException e)
        {
            Debug.LogError("OnGlobalTextReadLine Error Msg: " + e.ToString());
        }
    }



    ///<summary>
    /// 행동 데이터 파싱
    /// </summary>
    void OnActDataReadLine(string _receiveData)
    {
        try
        {
            Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(_receiveData);

            // 재 화 정 보 갱 신 
            if (dic.ContainsKey("active") == true)
            {
                // 재 료 정 보 갱 신
                JArray arrActive = JArray.Parse(dic["active"].ToString());

                for (int i = 0; i < arrActive.Count; i++)
                {
                    // JObject boxObject = (JObject)_materials[i];
                    JObject jObject = arrActive[i].ToObject<JObject>();
                    eAct active = RayUtils.Utils.ConvertEnumData<eAct>(jObject["active"].ToString());
                    if (active != eAct.eActRest || active != eAct.NONE)
                    {
                        eSchedule[] arrScedule = new eSchedule[4];

                        arrScedule[0] = RayUtils.Utils.ConvertEnumData<eSchedule>(jObject["schedule_0"].ToString());
                        arrScedule[1] = RayUtils.Utils.ConvertEnumData<eSchedule>(jObject["schedule_1"].ToString());
                        arrScedule[2] = RayUtils.Utils.ConvertEnumData<eSchedule>(jObject["schedule_2"].ToString());
                        arrScedule[3] = RayUtils.Utils.ConvertEnumData<eSchedule>(jObject["schedule_3"].ToString());

                        string _img = jObject["schedule_img"].ToString();

                        STActData data = new STActData(active, arrScedule, _img);

                        mDicActData.Add(active, data);
                    }

                }
            }
        }
        catch (UnityException e)
        {
            Debug.LogError("OnActDataReadLine Error Msg: " + e.ToString());
        }
    }

    ///<summary>
    /// 스케줄 메타 데이터 파싱 
    ///</summary>
    void OnSceduleReadLine(string _receiveData)
    {
        try
        {
            Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(_receiveData);
            if (dic.ContainsKey("state") == true)
            {
                JArray arrData = JArray.Parse(dic["state"].ToString());

                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject objData = arrData[i].ToObject<JObject>();
                    eAct _act = RayUtils.Utils.ConvertEnumData<eAct>(objData["act"].ToString());
                    eSchedule _scedule = RayUtils.Utils.ConvertEnumData<eSchedule>(objData["key"].ToString());
                    eState _upState_0 = RayUtils.Utils.ConvertEnumData<eState>(objData["upKey_1"].ToString());
                    eState _upState_1 = RayUtils.Utils.ConvertEnumData<eState>(objData["upKey_2"].ToString());
                    eState _upState_2 = RayUtils.Utils.ConvertEnumData<eState>(objData["upKey_3"].ToString());

                    string _upPoint_0 = objData["upPoint_1"].ToString();
                    string _upPoint_1 = objData["upPoint_2"].ToString();
                    string _upPoint_2 = objData["upPoint_3"].ToString();

                    eState _downState_0 = RayUtils.Utils.ConvertEnumData<eState>(objData["downKey_1"].ToString());
                    eState _downState_1 = RayUtils.Utils.ConvertEnumData<eState>(objData["downKey_2"].ToString());
                    eState _downState_2 = RayUtils.Utils.ConvertEnumData<eState>(objData["downKey_3"].ToString());


                    string _downPoint_0 = objData["downPoint_1"].ToString();
                    string _downPoint_1 = objData["downPoint_2"].ToString();
                    string _downPoint_2 = objData["downPoint_3"].ToString();

                    string _background = objData["background"].ToString();
                    string _anim_img_1 = objData["anim_img_1"].ToString();
                    string _anim_img_2 = objData["anim_img_2"].ToString();

                    string icon = objData["icon"].ToString();

                    string desc = objData["desc"].ToString();

                    STScheduleData sceduleData = new STScheduleData
                    (
                        _scedule, _upState_0, _upState_1, _upState_2,
                        _upPoint_0, _upPoint_1, _upPoint_2,
                        _downState_0, _downState_1, _downState_2,
                        _downPoint_0, _downPoint_1, _downPoint_2,
                        _background, _anim_img_1, _anim_img_2,
                        icon, desc
                    );

                    if (mDicSceduleData.ContainsKey(_act) == false)
                    {
                        Dictionary<eSchedule, STScheduleData> data = new Dictionary<eSchedule, STScheduleData>();
                        data.Add(_scedule, sceduleData);
                        mDicSceduleData.Add(_act, data);
                    }
                    else
                    {
                        Dictionary<eSchedule, STScheduleData> data = mDicSceduleData[_act];
                        data[_scedule] = sceduleData;
                    }
                }

            }
        }
        catch (UnityException e)
        {
            Debug.LogError("OnSceduleReadLine Error Msg: " + e.ToString());
        }

    }

    ///<summary>
    /// 매니저 스킬 메타데이터 파싱
    ///</summary>
    void OnManagerSkillReadLine(string _receiveData)
    {
        try
        {
            Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(_receiveData);

            if (dic.ContainsKey("skill") == true)
            {
                JArray arrData = JArray.Parse(dic["skill"].ToString());

                for (int i = 0; i < arrData.Count; i++)
                {

                    JObject objData = arrData[i].ToObject<JObject>();

                    eManager_Skill _skill = RayUtils.Utils.ConvertEnumData<eManager_Skill>(objData["skill"].ToString());
                    string _skillName = objData["skill_name"].ToString();
                    eSkillGrade _grade = RayUtils.Utils.ConvertEnumData<eSkillGrade>(objData["skill_grade"].ToString());
                    eSkillType _skillType = RayUtils.Utils.ConvertEnumData<eSkillType>(objData["skll_type"].ToString());
                    float _probability = float.Parse(objData["probability"].ToString());
                    float _probabilityValue = float.Parse(objData["value"].ToString());


                    eGlobalTextKey _keySkillName = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(objData["skill_name"].ToString());
                    eGlobalTextKey info_0 = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(objData["skill_info_0"].ToString());
                    eGlobalTextKey info_1 = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(objData["skill_info_1"].ToString());
                    eGlobalTextKey info_value = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(objData["skill_info_value_desc"].ToString());


                    STManagerSkill stSkillData
                    = new STManagerSkill(
                        _skill,
                        _skillName,
                         _grade,
                         _skillType,
                         _probability,
                         _probabilityValue,
                         info_0,
                         info_1,
                         info_value,
                         _keySkillName
                         );

                    if (mDicManagerSkillData.ContainsKey(_skill) == false)
                    {
                        Dictionary<eSkillGrade, STManagerSkill> dicData = new Dictionary<eSkillGrade, STManagerSkill>();
                        dicData.Add(_grade, stSkillData);
                        mDicManagerSkillData.Add(_skill, dicData);
                    }
                    else
                    {
                        Dictionary<eSkillGrade, STManagerSkill> _dicData = mDicManagerSkillData[_skill];
                        if (_dicData.ContainsKey(_grade) == false)
                        {
                            _dicData.Add(_grade, stSkillData);
                        }

                        mDicManagerSkillData[_skill] = _dicData;

                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("OnManagerSkillReadLine error : " + e.ToString());
        }
    }

    ///<summary>
    /// 매니저 메타 데이터 파싱
    ///</summary>
    void OnManagerReadLine(string _receiveData)
    {
        try
        {
            Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(_receiveData);

            if (dic.ContainsKey("manager") == true)
            {
                JArray arrData = JArray.Parse(dic["manager"].ToString());

                for (int i = 0; i < arrData.Count; i++)
                {
                    // 매니저 유저저장 이름	매니저 기본스킬	매니저 스킬 설명	매니저 이미지(향후 변경)	매니저 성격
                    // manager_name	manager_skill	manager_skill_desc	manager_img	manager_desc

                    JObject objData = arrData[i].ToObject<JObject>();
                    eManager_Type manager = RayUtils.Utils.ConvertEnumData<eManager_Type>(objData["manager_key"].ToString());
                    string managerName = objData["manager_name"].ToString();
                    eManager_Skill skill = RayUtils.Utils.ConvertEnumData<eManager_Skill>(objData["manager_skill"].ToString());
                    eGlobalTextKey skillDesc = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(objData["manager_skill_desc"].ToString());
                    string managerImage = objData["manager_img"].ToString();
                    eGlobalTextKey managerDesc = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(objData["manager_desc"].ToString());
                    ePersonality_Type managerPersonality = RayUtils.Utils.ConvertEnumData<ePersonality_Type>(objData["manager_personality"].ToString());

                    STManagerData _data = new STManagerData(manager, managerName, skill, skillDesc, managerImage, managerDesc, managerPersonality);
                    if (mDicManagerData.ContainsKey(manager) == false)
                    {
                        mDicManagerData.Add(manager, _data);
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("OnManagerReadLine error : " + e.ToString());
        }
    }

    ///<summary>
    /// 주차별 보상 골드 파싱
    ///</summary>
    void OnWeekResultGoldReadLine(string _receiveData)
    {
        try
        {
            Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(_receiveData);

            if (dic.ContainsKey("weekgold") == true)
            {
                JArray arrData = JArray.Parse(dic["weekgold"].ToString());

                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject objData = arrData[i].ToObject<JObject>();
                    string _gold = objData["gold"].ToString();

                    mDicWeekResultGold.Add(i, int.Parse(_gold));

                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("OnManagerReadLine error : " + e.ToString());
        }
    }

    void OnItem_ManagerReadLine(string _receiveData)
    {
        try
        {
            Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(_receiveData);

            if (dic.ContainsKey("mngItem") == true)
            {
                JArray arrData = JArray.Parse(dic["mngItem"].ToString());

                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject objData = arrData[i].ToObject<JObject>();
                    /// 아이템키 
                    eManagerItem _itemKey = RayUtils.Utils.ConvertEnumData<eManagerItem>(objData["itemKey"].ToString());
                    /// 올려질 능력치 종류
                    eManagerItemAbility _itemAbility = RayUtils.Utils.ConvertEnumData<eManagerItemAbility>(objData["itemAbility"].ToString());
                    /// 확률
                    int _prob = int.Parse(objData["prob"].ToString());
                    /// 올라갈 수치
                    int _upPoint = int.Parse(objData["upPoint"].ToString());
                    /// 가격
                    int _price = int.Parse(objData["price"].ToString());
                    /// 아이템 이미지 
                    string _image = objData["image"].ToString();
                    eGlobalTextKey _itemDesc = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(objData["itemDesc"].ToString());

                    STManagerItemData _itemData = new STManagerItemData(_itemKey, _image, _itemAbility, _prob, _upPoint, _price, _itemDesc);
                    //// 매니저 아이템 세팅
                    if (mDicManagerItemData.ContainsKey(_itemKey) == false)
                    {
                        mDicManagerItemData.Add(_itemKey, _itemData);
                    }

                    ///// 상점 정렬용 카테고리별 아이템 분류
                    if (mDicCategoryItem.ContainsKey(_itemAbility) == false)
                    {
                        Dictionary<eManagerItem, STManagerItemData> dicCategory = new Dictionary<eManagerItem, STManagerItemData>();
                        dicCategory.Add(_itemKey, _itemData);
                        mDicCategoryItem.Add(_itemAbility, dicCategory);
                    }
                    else
                    {
                        Dictionary<eManagerItem, STManagerItemData> dicCategory = mDicCategoryItem[_itemAbility];
                        if (dicCategory.ContainsKey(_itemKey) == false)
                        {
                            dicCategory.Add(_itemKey, _itemData);
                        }
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("OnManagerReadLine error : " + e.ToString());
        }
    }

    ///<summary>
    /// 꿈이벤트 데이터 파싱
    ///</summary>
    void OnDreamEventReadLine(string _receiveData)
    {
        try
        {
            Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(_receiveData);

            if (dic.ContainsKey("dream") == true)
            {
                JArray arrData = JArray.Parse(dic["dream"].ToString());

                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject objData = arrData[i].ToObject<JObject>();
                    //// 꿈이벤트 등급
                    eDreamEventGrade _grade = RayUtils.Utils.ConvertEnumData<eDreamEventGrade>(objData["grade"].ToString());
                    //// 이벤트 키
                    eDremEvent _dreamEvent = RayUtils.Utils.ConvertEnumData<eDremEvent>(objData["key"].ToString());
                    //// 올릴 이벤트 종류
                    eSkillType _skillType = RayUtils.Utils.ConvertEnumData<eSkillType>(objData["skillType"].ToString());

                    /// 확률 및 올리값
                    int _prob = int.Parse(objData["prob"].ToString());
                    int _probValue = int.Parse(objData["value"].ToString());
                    //// 이미지 및 설명
                    string _image = objData["image"].ToString();
                    string _desc = objData["desc"].ToString();

                    STDreamEventData stData = new STDreamEventData(_grade, _dreamEvent, _skillType, _prob, _probValue, _image, _desc);


                    if (mDicDreamEventData.ContainsKey(_grade) == false)
                    {
                        List<STDreamEventData> _list = new List<STDreamEventData>();
                        _list.Add(stData);
                        mDicDreamEventData.Add(_grade, _list);
                    }
                    else
                    {
                        List<STDreamEventData> _list = mDicDreamEventData[_grade];
                        _list.Add(stData);
                        // mDicDreamEventData[_dreamEvent] = _list;

                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("OnManagerReadLine error : " + e.ToString());
        }
    }


    ///<summary>
    /// 엔딩 데이터 파싱
    ///</summary>
    void OnEndingDataReadLine(string _receiveData)
    {
        try
        {
            Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(_receiveData);

            if (dic.ContainsKey("ending") == true)
            {
                JArray arrData = JArray.Parse(dic["ending"].ToString());

                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject objData = arrData[i].ToObject<JObject>();

                    eEndingNumber _endingNum = RayUtils.Utils.ConvertEnumData<eEndingNumber>(objData["endNum"].ToString());
                    eGlobalTextKey _globalKey = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(objData["endTitle"].ToString());
                    string endingType = objData["endintType"].ToString();
                    string endImage = objData["endImage"].ToString();
                    string endFile = objData["endFile"].ToString();

                    STEndingData stData = new STEndingData(_endingNum, _globalKey, endingType, endImage, endFile);

                    if (mDicEndingData.ContainsKey(_endingNum) == false)
                    {
                        mDicEndingData.Add(_endingNum, stData);
                    }
                    else
                    {
                        mDicEndingData[_endingNum] = stData;
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("OnEndingDataReadLine error : " + e.ToString());
        }
    }


    ///<summary>
    /// 돌발이벤트 메타 데이터 처리
    ///</summary>
    void OnSuddenEventDataReadLine(string _receiveData)
    {
        try
        {
            Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(_receiveData);

            if (dic.ContainsKey("suddenEvent") == true)
            {
                JArray arrData = JArray.Parse(dic["suddenEvent"].ToString());

                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject objData = arrData[i].ToObject<JObject>();

                    //// 행동
                    eAct _act = RayUtils.Utils.ConvertEnumData<eAct>(objData["act"].ToString());
                    //// 공통은 스케줄_4로 묶어서 이넘값이 없음, 스트링으로 받아 공통일경우 예외처리로 따로 개발
                    string _eSchedule = objData["schedule"].ToString();
                    /// 돌발이벤트 등급
                    eSuddenEventGrade _eSuddenGrade = RayUtils.Utils.ConvertEnumData<eSuddenEventGrade>(objData["grade"].ToString());
                    /// 스페셜 때문에 해당 등급 필요함 
                    eSkillType _eState = RayUtils.Utils.ConvertEnumData<eSkillType>(objData["state"].ToString());
                    /// 스페셜에 필요한 스텟 수치
                    int _value = int.Parse(objData["value"].ToString());
                    /// 파일이름
                    string _fileName = objData["fileName"].ToString();

                    STSuddenEventData stData = new STSuddenEventData(_act, _eSchedule, _eSuddenGrade, _eState, _value, _fileName);

                    // Dictionary<eAct, Dictionary<string, Dictionary<eSuddenEventGrade, List<STSuddenEventData>>>> 
                    if (mDicSuddenEvent.ContainsKey(_act) == false)
                    {
                        Dictionary<string, Dictionary<eSuddenEventGrade, List<STSuddenEventData>>> temp = new Dictionary<string, Dictionary<eSuddenEventGrade, List<STSuddenEventData>>>();
                        Dictionary<eSuddenEventGrade, List<STSuddenEventData>> dicTemp_0 = new Dictionary<eSuddenEventGrade, List<STSuddenEventData>>();
                        List<STSuddenEventData> tempList = new List<STSuddenEventData>();

                        tempList.Add(stData);
                        dicTemp_0.Add(_eSuddenGrade, tempList);
                        temp.Add(_eSchedule, dicTemp_0);
                        mDicSuddenEvent.Add(_act, temp);

                    }
                    else
                    {

                        Dictionary<string, Dictionary<eSuddenEventGrade, List<STSuddenEventData>>> temp = mDicSuddenEvent[_act];
                        if (temp.ContainsKey(_eSchedule) == false)
                        {
                            Dictionary<eSuddenEventGrade, List<STSuddenEventData>> _dicTemp = new Dictionary<eSuddenEventGrade, List<STSuddenEventData>>();
                            List<STSuddenEventData> _listTemp = new List<STSuddenEventData>();
                            _listTemp.Add(stData);
                            _dicTemp.Add(_eSuddenGrade, _listTemp);
                            temp.Add(_eSchedule, _dicTemp);

                        }
                        else
                        {
                            Dictionary<eSuddenEventGrade, List<STSuddenEventData>> _dicTemp = temp[_eSchedule];

                            if (_dicTemp.ContainsKey(_eSuddenGrade) == false)
                            {
                                List<STSuddenEventData> _listTemp = new List<STSuddenEventData>();
                                _listTemp.Add(stData);
                                _dicTemp.Add(_eSuddenGrade, _listTemp);
                            }
                            else
                            {
                                List<STSuddenEventData> _listTemp = _dicTemp[_eSuddenGrade];
                                if (_listTemp == null)
                                {
                                    _listTemp = new List<STSuddenEventData>();
                                    _listTemp.Add(stData);
                                }
                                else
                                {
                                    _listTemp.Add(stData);
                                }
                            }
                        }

                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("OnSuddenEventDataReadLine error : " + e.ToString());
        }
    }



    void OnMimTalkDataReadLine(string _receiveData)
    {
        try
        {
            Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(_receiveData);

            if (dic.ContainsKey("mimTalk") == true)
            {
                JArray arrData = JArray.Parse(dic["mimTalk"].ToString());

                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject objData = arrData[i].ToObject<JObject>();
                    string _message = objData["message"].ToString();

                    STMimTalkData stData = new STMimTalkData(i, _message);

                    if (mDicMimTalkData.ContainsKey(i) == false)
                    {
                        mDicMimTalkData.Add(i, stData);
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("OnEndingDataReadLine error : " + e.ToString());
        }

    }

    ///<summary>
    /// 오디션 최종 대사
    ///</summary>
    void OnAuditionPanelDataReadLine(string _receiveData)
    {
        try
        {
            Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(_receiveData);

            if (dic.ContainsKey("auditionPanel") == true)
            {
                JArray arrData = JArray.Parse(dic["auditionPanel"].ToString());

                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject objData = arrData[i].ToObject<JObject>();

                    eAuditionPanel _panel = RayUtils.Utils.ConvertEnumData<eAuditionPanel>(objData["panel"].ToString());
                    eAuditionState _state = RayUtils.Utils.ConvertEnumData<eAuditionState>(objData["type"].ToString());
                    int _value = int.Parse(objData["value"].ToString());
                    string _message = objData["message"].ToString();


                    STAuditionPanelData _panelData = new STAuditionPanelData(_panel, _state, _value, _message);

                    if (mDicAuditionPanelData.ContainsKey(_panel) == false)
                    {
                        Dictionary<eAuditionState, List<STAuditionPanelData>> _dicData = new Dictionary<eAuditionState, List<STAuditionPanelData>>();
                        List<STAuditionPanelData> _list = new List<STAuditionPanelData>();
                        _list.Add(_panelData);

                        _dicData.Add(_state, _list);

                        mDicAuditionPanelData.Add(_panel, _dicData);
                    }
                    else
                    {
                        Dictionary<eAuditionState, List<STAuditionPanelData>> _dicData = mDicAuditionPanelData[_panel];

                        if (_dicData.ContainsKey(_state) == false)
                        {
                            List<STAuditionPanelData> _list = new List<STAuditionPanelData>();
                            _list.Add(_panelData);
                            _dicData.Add(_state, _list);
                        }
                        else
                        {
                            List<STAuditionPanelData> _list = _dicData[_state];
                            _list.Add(_panelData);
                        }
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("OnAuditionPanelDataReadLine error : " + e.ToString());
        }
    }

    ///<summary>
    /// 상점데이터
    ///</summary>
    void OnShopDataReadLine(string _receiveData)
    {
        try
        {
            Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(_receiveData);

            if (dic.ContainsKey("inapp") == true)
            {
                JArray arrData = JArray.Parse(dic["inapp"].ToString());

                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject objData = arrData[i].ToObject<JObject>();

                    // 	결제키	결제타입  인앱키_AOS	    인앱키_IOS	     타이틀	    #확인용 한글	설명	수량	가격	할인가격	이미지
                    // 	key	type	inappKey_AOS	inappKey_IOS	title	checkHangle	  desc	amount	price	fakePrice	image

                    eShopPurchaseKey _shopKey = RayUtils.Utils.ConvertEnumData<eShopPurchaseKey>(objData["key"].ToString());
                    eShopPurchaseType _type = RayUtils.Utils.ConvertEnumData<eShopPurchaseType>(objData["type"].ToString());

                    string _inapp_aos = objData["inappKey_AOS"].ToString();
                    string _inapp_ios = objData["inappKey_IOS"].ToString();

                    string _title = objData["title"].ToString();
                    string _desc = objData["desc"].ToString();

                    int _amount = int.Parse(objData["amount"].ToString());
                    int _price = int.Parse(objData["price"].ToString());
                    int _fakePrice = int.Parse(objData["fakePrice"].ToString());

                    string _image = objData["image"].ToString();

                    STShopData _data = new STShopData(_shopKey, _type, _inapp_aos, _inapp_ios,
                    _title, _desc, _amount, _price, _fakePrice, _image);

                    if (mDicShopData.ContainsKey(_shopKey) == false)
                    {
                        mDicShopData.Add(_shopKey, _data);
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("OnShopDataReadLine error : " + e.ToString());
        }
    }


    ///<summary>
    /// 상점데이터
    ///</summary>
    void OnVideoAdsDataReadLine(string _receiveData)
    {
        try
        {
            Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(_receiveData);

            if (dic.ContainsKey("videoads") == true)
            {
                JArray arrData = JArray.Parse(dic["videoads"].ToString());

                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject objData = arrData[i].ToObject<JObject>();

                    // 	결제키	결제타입  인앱키_AOS	    인앱키_IOS	     타이틀	    #확인용 한글	설명	수량	가격	할인가격	이미지
                    // 	key	type	inappKey_AOS	inappKey_IOS	title	checkHangle	  desc	amount	price	fakePrice	image

                    eVideoAds _videoKey = RayUtils.Utils.ConvertEnumData<eVideoAds>(objData["key"].ToString());
                    string _desc = objData["Desc"].ToString();
                    string aos_id = objData["vidoe_id_AOS"].ToString();
                    string ios_id = objData["video_id_IOS"].ToString();

                    STVideoAdsData _data = new STVideoAdsData(_videoKey, _desc, aos_id, ios_id);

                    if (mDicVideoData.ContainsKey(_videoKey) == false)
                    {
                        mDicVideoData.Add(_videoKey, _data);
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("OnVideoAdsDataReadLine error : " + e.ToString());
        }
    }





}


