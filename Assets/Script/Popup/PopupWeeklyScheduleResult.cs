using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using StructuredDataType;



///<summary>
/// 일정종료 후 처리할 데이터 세팅
///</summary>
public class STWeeklyResultData
{
    public delegate void CallBackDelegate();
    public CallBackDelegate noramlCallback = null;
    public void init()
    {
        if (noramlCallback != null)
        {
            noramlCallback = null;
        }
    }

    public STWeeklyResultData(CallBackDelegate callback)
    {
        noramlCallback = callback;
    }

    public void CallBack()
    {
        // NormalCallBack callback = 
        if (noramlCallback != null)
        {
            noramlCallback();
        }
    }

}

public class PopupWeeklyScheduleResult : JHPopup
{

    public static System.Action ACTION_SCHEDULE_RESULT;

    [Header("Title")]
    public GameObject objTitle;
    public GameObject objWeekResult;
    public UILabel labelTitle; /// Week 1
    public UILabel labelResult; // Result;
    public GameObject objLight;
    public GameObject objCharactor;
    public UI2DSprite spriteCharacer;
    public GameObject objResultBubbleTalk;
    public UILabel labelBubbleTalk;
    public GameObject objParticle;

    [Header("State Progress")]
    public UIGrid gridState;
    public ResultStateProgress[] prefabProgress;

    [Header("Grid Result")]
    public UIGrid gridResult;
    public ResultStateProgress[] prefabResult;

    [Header("HP & Appearence")]
    public GameObject objHP;
    public UISprite fillHPValue;
    public UILabel labelHPVale;
    public GameObject objAppearence;
    public UISprite fillAppearenceValue;
    public UILabel labelAppearenceValue;

    public GameObject objHpBonus;
    public UILabel labelHPBouns;

    public GameObject objAppearenceBonus;
    public UILabel labelAppearenceBouns;

    [Header("Reward")]
    public GameObject objReward;
    public UILabel labelCEDReward; ///<= 결과 후 Reward => 매니저수익

    [Header("Reward Animation")]
    public GameObject objBigCoin;
    public UI2DSpriteAnimation animBigCoin;
    public GameObject objSmollCoin;
    public UI2DSpriteAnimation animSmollCoin;
    public GameObject objRewardLight;
    public UI2DSpriteAnimation animAewardLight;
    public GameObject objRewardCoinLight;
    public UI2DSpriteAnimation animAewardCoinLight;
    public UILabel labelRewardAmount;

    [Header("NEW Reward")]
    public GameObject objCoinAnimation;
    public GameObject objFirstSpark;
    public GameObject objCoin;
    public GameObject objSecoundSpin;
    public GameObject objStar;


    public UIButton btnConfirm;

    Dictionary<eState, int> dicCrrState = new Dictionary<eState, int>();
    Dictionary<eState, int> dicAdjustState = new Dictionary<eState, int>();


    eState topState = eState.NONE;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void OnAwake()
    {
        base.OnAwake();

        labelRewardAmount.text = "";
        btnConfirm.onClick.Add(new EventDelegate(OnClickConfirm));

        if (UserInfoManager.Instance.GetSaveInAppPrimiumItem(eShopPurchaseKey.eStorePrimium_1.ToString()) == true)
        {
            /// eTextHPInfinity
            labelHPVale.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eTextHPInfinity);
            labelHPVale.fontSize = 67;

            fillHPValue.fillAmount = 1.0f;
        }
        else
        {
            labelHPVale.text = UserInfoManager.Instance.GetSaveHP().ToString();

        }

        labelAppearenceValue.text = UserInfoManager.Instance.GetSaveAwareness().ToString();

        // labelResult.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eReward);
        labelResult.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eResult);

        labelTitle.text = "WEEK " + UserInfoManager.Instance.GetWeeklyCount();



    }

    public override void SetData()
    {
        base.SetData();
        StartCoroutine(IE_TESTANIMATION());
    }

    /// UI 테스트용
    IEnumerator IE_TESTANIMATION()
    {
        objLight.SetActive(true);
        objWeekResult.SetActive(false);
        objHP.SetActive(false);
        objAppearence.SetActive(false);

        objCharactor.SetActive(false);
        spriteCharacer.gameObject.SetActive(false);

        objResultBubbleTalk.SetActive(false);
        objParticle.SetActive(false);

        //// 보상 UI 
        gridState.gameObject.SetActive(false);
        gridResult.gameObject.SetActive(false);
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(IESetRewardGoldUI());

    }

    ///<summary>
    /// 실제 데이터
    ///</summary>
    public override void SetData(object data_)
    {

        for (int i = 0; i < (int)Enums.eState.COUNT; i++)
        {
            string _data = UserInfoManager.Instance.GetTempCrrStatePoint(((eState)i).ToString());
            if (string.IsNullOrEmpty(_data) == true || _data.Equals(""))
                continue;

            eState stateKey = RayUtils.Utils.ConvertEnumData<eState>(_data.Split('@')[0]);
            int statePoint = int.Parse(_data.Split('@')[1]);

            if (dicCrrState.ContainsKey(stateKey) == false)
            {
                dicCrrState.Add(stateKey, statePoint);
            }
            else
            {
                dicCrrState[stateKey] = statePoint;
            }

        }

        for (int i = 0; i < (int)eState.COUNT; i++)
        {
            string _data = UserInfoManager.Instance.GetTempAdjustStatePoint(((eState)i).ToString());
            if (string.IsNullOrEmpty(_data) == true || _data.Equals(""))
                continue;

            eState stateKey = RayUtils.Utils.ConvertEnumData<eState>(_data.Split('@')[0]);
            int statePoint = int.Parse(_data.Split('@')[1]);

            if (dicAdjustState.ContainsKey(stateKey) == false)
            {
                dicAdjustState.Add(stateKey, statePoint);
            }
            else
            {
                int beforPoint = dicAdjustState[stateKey];
                dicAdjustState[stateKey] = Mathf.Min(Integers.STATE_MAX, Mathf.Max(Integers.STATE_MIN, (beforPoint + statePoint)));
            }
        }

        int topStatePoint = 0;
        for (int i = 0; i < (int)eState.COUNT; i++)
        {
            if (dicAdjustState.ContainsKey((eState)i) == true)
            {
                int tempPoint = dicAdjustState[(eState)i];
                if (topStatePoint < tempPoint)
                {
                    topStatePoint = tempPoint;
                    topState = (eState)i;
                }
            }
        }
        gridState.gameObject.SetActive(false);
        gridState.Reposition();
    }

    void Start()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_18_card);
        

        StartCoroutine(SetUIProssce());


        for (int i = 0; i < prefabProgress.Length; i++)
        {
            prefabProgress[i].gameObject.name = ((eState)i).ToString();
            prefabResult[i].name = ((eState)i).ToString();

            if (dicCrrState.ContainsKey((eState)i) == true && dicAdjustState.ContainsKey((eState)i) == true)
            {
                prefabProgress[i].SetProgress(((eState)i).ToString(), dicCrrState[((eState)i)], dicAdjustState[((eState)i)], "");
            }
            else
            {
                if (dicCrrState.ContainsKey((eState)i) == true)
                {
                    dicCrrState[(eState)i] = 0;
                }
                else
                {
                    dicCrrState.Add((eState)i, 0);
                }

                if (dicAdjustState.ContainsKey((eState)i) == true)
                {
                    dicAdjustState[(eState)i] = 0;
                }
                else
                {
                    dicAdjustState.Add((eState)i, 0);
                }

                prefabProgress[i].SetProgress(((eState)i).ToString(), dicCrrState[((eState)i)], dicAdjustState[((eState)i)], "");
            }
            prefabResult[i].gameObject.SetActive(true);
        }

        StartCoroutine(IE_HpProgresse());
        StartCoroutine(IE_AppearenceProgresse());

        object[] skillData = null;
        object[] itemData = null;
        /// 결과화면
        gridState.gameObject.SetActive(false);
        gridResult.gameObject.SetActive(true);
        //// 매니저 스킬이 F일경우에 상승 스텟치 계산
        for (int i = 0; i < prefabResult.Length; i++)
        {
            prefabResult[i].gameObject.SetActive(true);
            prefabResult[i].SetResultProgress(((eState)i).ToString(), skillData, itemData);
        }

        gridResult.Reposition();

        SceneBase.RefrashGoldDia();

        Invoke("InvokeIsActionBtn", 3.0f);
    }


    /// 연출순서
    IEnumerator SetUIProssce()
    {
        yield return YieldHelper.waitForSeconds(500);
        objTitle.SetActive(true);
        yield return YieldHelper.waitForSeconds(500);
        objLight.SetActive(true);
        yield return YieldHelper.waitForSeconds(500);
        objCharactor.SetActive(true);
        spriteCharacer.gameObject.SetActive(true);
        yield return YieldHelper.waitForSeconds(500);
        // objResultBubbleTalk.SetActive(true);
        objParticle.SetActive(true);

    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    protected override void OnDestroied()
    {
        base.OnDestroied();

        if (ACTION_SCHEDULE_RESULT != null && ACTION_SCHEDULE_RESULT.GetInvocationList().Length > 0)
        {
            ACTION_SCHEDULE_RESULT();
        }

        /// 애니메이션용 데이터 초기화
        for (int i = 0; i < (int)eState.COUNT; i++)
        {
            SecurityPlayerPrefs.DeleteKey(Strings.STATE_CRR_TEMP + "_" + ((eState)i).ToString());
            SecurityPlayerPrefs.DeleteKey(Strings.STATE_ADJUST_TEMP + "_" + ((eState)i).ToString());
        }

    }

    int proceed = 0;
    bool isActionBtn = true;
    ///// 매니저 아이템 스케줄진행으로 옮김
    // void OnClickConfirm()
    // {
    //     if (isActionBtn == false)
    //     {
    //         isActionBtn = true;

    //         STManagerData _managerData = JHManagerManger.Instance.GetMyManager();
    //         eManager_Type _manager = _managerData.manager;

    //         STManagerSkill _managerSkill = JHManagerManger.Instance.GetMyManagerSkill();

    //         int prob = (int)_managerSkill.probability;
    //         int probVale = (int)_managerSkill.probabilityValue;

    //         switch (proceed)
    //         {
    //             case 0:
    //                 {

    //                     object[] skillData = null;
    //                     object[] itemData = null;
    //                     /// 결과화면
    //                     gridState.gameObject.SetActive(false);
    //                     gridResult.gameObject.SetActive(true);
    //                     //// 매니저 스킬이 F일경우에 상승 스텟치 계산
    //                     for (int i = 0; i < prefabResult.Length; i++)
    //                     {
    //                         /// 매니저 고유스킬 
    //                         if (_manager == eManager_Type.MANAGER_F && (eState)i == converManagerSkillToState(_managerSkill.managerSkill))
    //                         {
    //                             int _randomProb = Random.Range(0, 99);

    //                             if (prob <= _randomProb)
    //                             {
    //                                 skillData = new object[3];
    //                                 /// 확률적용
    //                                 skillData[0] = true;
    //                                 /// 올릴 스텟 적용
    //                                 skillData[1] = converManagerSkillToState(_managerSkill.managerSkill);
    //                                 /// 올릴 포인트 적용 
    //                                 skillData[2] = probVale;
    //                             }
    //                         }
    //                         //// 스텟상승 아이템 가지고 있을경우 
    //                         //// 착용아이템
    //                         string _item = UserInfoManager.Instance.GetEquipManagerItem(eManagerItemAbility.eState.ToString());
    //                         if (string.IsNullOrEmpty(_item) == false)
    //                         {
    //                             eManagerItem converItem = RayUtils.Utils.ConvertEnumData<eManagerItem>(_item);
    //                             //// 아이템 데이터 및 확률적용
    //                             itemData = ManagerItemApply(converItem);
    //                         }

    //                         prefabResult[i].gameObject.SetActive(true);
    //                         prefabResult[i].SetResultProgress(((eState)i).ToString(), skillData, itemData);
    //                     }

    //                     break;
    //                 }
    //             case 1:
    //                 {

    //                     StartCoroutine(IESetRewardGoldUI());
    //                     break;
    //                 }
    //             case 2:
    //                 {

    //                     OnClosed();
    //                     break;
    //                 }
    //         }
    //         Invoke("InvokeIsActionBtn", 1.0f);
    //         proceed++;
    //     }

    // }

    // /// <summary>
    // /// 매니저 아이템 스텟 확률적용
    // ///</summary>
    // object[] ManagerItemApply(eManagerItem _item)
    // {
    //     object[] itemData = null;
    //     STManagerItemData _itemData = SceneBase.Instance.dataManager.GetManagerItemData(_item);
    //     //// 확률
    //     int _prob = _itemData.prob;
    //     /// 업포인트 
    //     int _upPoint = _itemData.upPoint;
    //     /// 랜덤 확률
    //     int _random = Random.Range(0, 100);
    //     eState stateRandom = (eState)Random.Range((int)eState.eFACE, (int)eState.eCHARACTER);
    //     if (_random > _prob)
    //     {
    //         itemData = new object[3];
    //         //// 매니저 아이템 확률 적용 
    //         itemData[0] = true;
    //         /// 확률적용 스텟 
    //         itemData[1] = stateRandom;
    //         //// 확률적용 포인트 
    //         itemData[2] = _upPoint;
    //     }

    //     return itemData;
    // }

    // ///<summary>
    // /// 매니저스킬 F일 경우 어떤 스텟인지 변환 
    // ///</summary>
    // eState converManagerSkillToState(eManager_Skill _skillType)
    // {
    //     eState _state = eState.NONE;

    //     switch (_skillType)
    //     {
    //         /// 외모 
    //         case eManager_Skill.SKILL_F_A: { _state = eState.eFACE; break; }
    //         /// 연기력
    //         case eManager_Skill.SKILL_F_B: { _state = eState.eACTING; break; }
    //         /// 가창력
    //         case eManager_Skill.SKILL_F_C: { _state = eState.eSINGING; break; }
    //         /// 유머감각
    //         case eManager_Skill.SKILL_F_D: { _state = eState.eHUMOR; break; }
    //         /// 유연성
    //         case eManager_Skill.SKILL_F_E: { _state = eState.eFLEX; break; }
    //         /// 지식
    //         case eManager_Skill.SKILL_F_F: { _state = eState.eKNOW; break; }
    //         /// 성품
    //         case eManager_Skill.SKILL_F_G: { _state = eState.ePERSONALITY; break; }
    //         /// 개성
    //         case eManager_Skill.SKILL_F_H: { _state = eState.eCHARACTER; break; }
    //     }

    //     return _state;

    // }

    void OnClickConfirm()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        if (isActionBtn == false)
        {
            isActionBtn = true;

            STManagerData _managerData = JHManagerManger.Instance.GetMyManager();
            eManager_Type _manager = _managerData.manager;

            STManagerSkill _managerSkill = JHManagerManger.Instance.GetMyManagerSkill();

            int prob = (int)_managerSkill.probability;
            int probVale = (int)_managerSkill.probabilityValue;

            switch (proceed)
            {
                case 0:
                    {

                        // object[] skillData = null;
                        // object[] itemData = null;
                        // /// 결과화면
                        // gridState.gameObject.SetActive(false);
                        // gridResult.gameObject.SetActive(true);
                        // //// 매니저 스킬이 F일경우에 상승 스텟치 계산
                        // for (int i = 0; i < prefabResult.Length; i++)
                        // {
                        //     prefabResult[i].gameObject.SetActive(true);
                        //     prefabResult[i].SetResultProgress(((eState)i).ToString(), skillData, itemData);
                        // }
                        objCharactor.SetActive(false);
                        spriteCharacer.gameObject.SetActive(false);

                        StartCoroutine(IESetRewardGoldUI());
                        break;
                    }

                case 1:
                    {
                        OnClosed();
                        // StartCoroutine(IESetRewardGoldUI());
                        break;
                    }
                case 2:
                    {
                        // OnClosed();
                        break;
                    }
            }
            Invoke("InvokeIsActionBtn", 3.0f);
            proceed++;
        }

    }

    ///<summary>
    /// 일정 종료 후 리워드 골드 로직 및 애니메이션 적용
    ///</summary>
    IEnumerator IESetRewardGoldUI()
    {

        SceneBase.Instance.PLAY_SFX(eSFX.SFX_19_2_result_week);

        objCharactor.SetActive(false);
        int week = Mathf.Max(0, UserInfoManager.Instance.GetWeeklyCount() - 1);
        int endCount = UserInfoManager.Instance.GetEndingCount();

        string _weekTitle = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eSchedule_0);  //"REWARD";
        labelTitle.text = "WEEK " + (UserInfoManager.Instance.GetWeeklyCount() - 1); //string.Format(_weekTitle, week);

        objLight.SetActive(true);
        objWeekResult.SetActive(false);
        objHP.SetActive(false);
        objAppearence.SetActive(false);
        objResultBubbleTalk.SetActive(false);
        objParticle.SetActive(false);

        //// 보상 UI 
        gridState.gameObject.SetActive(false);
        gridResult.gameObject.SetActive(false);

        //// 주차 보상 결과 계산
        Dictionary<int, int> dicWeekReward = SceneBase.Instance.dataManager.GetDicWeekResultGold();


        /// TODO 20210102
        /// 매니저스킬은 일단 주석
        int counting = (((endCount * week) + 7) <= 7) ? week : ((endCount * week) + 7);
        int rewardGold = dicWeekReward[counting];
        Debug.LogError("RewardGold : " + rewardGold);
        STManagerSkill _managerSkill = JHManagerManger.Instance.GetMyManagerSkill();
        if (_managerSkill.managerSkill == eManager_Skill.SKILL_B)
        {
            float _probValue = _managerSkill.probabilityValue * 0.01f;
            /// 골드 퍼센트 계산
            float calcGold = (float)rewardGold * _probValue;
            rewardGold += Mathf.CeilToInt(calcGold);
            // Debug.LogError("CalcGold : " + calcGold + " // totla Gold : " + rewardGold);
        }

        int crrGold = int.Parse(UserInfoManager.Instance.GetGold());

        //// 선물요정 골드 증가값 확률적용
        if (UserInfoManager.Instance.GetSaveFairyGiftItem(eFairyGiftItem.GOLD.ToString()) == true)
        {
            rewardGold *= UserInfoManager.FAIRY_GIFT_GOLD_VALE_2;
        }

        int plusGold = crrGold + rewardGold;

        UserInfoManager.Instance.SetGold(plusGold.ToString());
        //// 선물요정 데이터 초기화
        UserInfoManager.Instance.SetSaveFairyGiftItem(eFairyGiftItem.EVENT.ToString(), false);
        UserInfoManager.Instance.SetSaveFairyGiftItem(eFairyGiftItem.GOLD.ToString(), false);
        UserInfoManager.Instance.SetSaveFairyGiftItem(eFairyGiftItem.TIME.ToString(), false);

        UserInfoManager.Instance.SetSaveInAppPrimiumItem(eShopPurchaseKey.eStoreItemBuff_2.ToString(), false);
        UserInfoManager.Instance.SetSaveInAppPrimiumItem(eShopPurchaseKey.eStoreItemBuff_3.ToString(), false);
        UserInfoManager.Instance.SetSaveInAppPrimiumItem(eShopPurchaseKey.eStoreItemBuff_4.ToString(), false);
        yield return new WaitForEndOfFrame();


        /************************************************************/
        //// 변경
        // labelResult.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eWeeklyManagerRsult);
        labelResult.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eReward);

        objReward.SetActive(true);
        objCoinAnimation.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        objFirstSpark.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        objCoin.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        objSecoundSpin.SetActive(true);
        objStar.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        objCharactor.SetActive(false);
        int rewardCnt = DEFAULT_GOLD;

        SceneBase.Instance.PLAY_SFX(eSFX.SFX_20_get_coin1);


        while (true)
        {
            if (rewardCnt >= rewardGold)
            {
                labelRewardAmount.text = string.Format("x{0:00}", rewardGold);
                break;
            }
            rewardCnt += 10;
            labelRewardAmount.text = string.Format("x{0:00}", rewardCnt);
            yield return new WaitForSeconds(0.001f);
        }
        /************************************************************/
    }

    const int DEFAULT_GOLD = 500;


    IEnumerator IE_HpProgresse()
    {
        //// 체력회복은 인게임 상에서 적용
        //// 체력회복 아이템 적용
        // string _item = UserInfoManager.Instance.GetEquipManagerItem(eManagerItemAbility.eHP.ToString());
        // Debug.LogError("################# _item IE_HpProgresse : " + _item);
        // if (string.IsNullOrEmpty(_item) == false)
        // {
        //     eManagerItem _eItem = RayUtils.Utils.ConvertEnumData<eManagerItem>(_item);
        //     STManagerItemData _itemData = SceneBase.Instance.dataManager.GetManagerItemData(_eItem);

        //     int radomProb = Random.Range(0, 100);
        //     int prob = _itemData.prob;
        //     int upPoint = _itemData.upPoint;
        //     if (radomProb > prob)
        //     {
        //         SceneBase.Instance.AddTestTextPopup("아이템 적용 : 체력");
        //         UserInfoManager.Instance.SetSaveHP(UserInfoManager.Instance.GetSaveHP() + upPoint);

        //         string _text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eHPBonus);
        //         labelHPBouns.text = string.Format(_text, upPoint);
        //         objHpBonus.SetActive(true);

        //     }
        // }

        float crrHp = (float)UserInfoManager.Instance.GetSaveHP() * 0.01f;
        float clac = 0.0f;
        while (true)
        {
            if (UserInfoManager.Instance.GetSaveInAppPrimiumItem(eShopPurchaseKey.eStorePrimium_1.ToString()) == true)
            {
                /// eTextHPInfinity
                labelHPVale.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eTextHPInfinity);
                labelHPVale.fontSize = 67;

                fillHPValue.fillAmount = 1.0f;

                break;
            }
            else
            {
                if (clac >= crrHp)
                {
                    break;
                }
                yield return new WaitForSeconds(0.001f);
                clac += 0.01f;
                fillHPValue.fillAmount = clac;

            }

        }
    }

    IEnumerator IE_AppearenceProgresse()
    {

        //// 인지도 상승 아이템 적용
        string _item = UserInfoManager.Instance.GetEquipManagerItem(eManagerItemAbility.eAwareness.ToString());
        Debug.LogError("################# _item AppearenceProgresse : " + _item);
        if (string.IsNullOrEmpty(_item) == false)
        {
            eManagerItem _eItem = RayUtils.Utils.ConvertEnumData<eManagerItem>(_item);
            STManagerItemData _itemData = SceneBase.Instance.dataManager.GetManagerItemData(_eItem);

            int radomProb = Random.Range(0, 100);
            // Debug.LogError("################# IE_AppearenceProgresse radomProb :  " + radomProb);
            int prob = _itemData.prob;
            // Debug.LogError("################# IE_AppearenceProgresse prob :  " + prob);
            int upPoint = _itemData.upPoint;
            // Debug.LogError("################# IE_AppearenceProgresse upPoint :  " + upPoint);
            if (radomProb > prob)
            {
                SceneBase.Instance.AddTestTextPopup("아이템 적용 :  인지도");
                UserInfoManager.Instance.SetSaveAwareness(UserInfoManager.Instance.GetSaveAwareness() + upPoint);
                // Debug.LogError("################# IE_AppearenceProgresse crr :  " + UserInfoManager.Instance.GetSaveAwareness() + " // upPoint : " + upPoint);
                // Debug.LogError("################# GetSaveAwareness :  " + UserInfoManager.Instance.GetSaveAwareness());
                string _text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eAppearenceBonus);
                labelAppearenceBouns.text = string.Format(_text, upPoint);
                objAppearenceBonus.SetActive(true);

            }
        }

        if (UserInfoManager.Instance.GetSaveAwareness() >= 100)
        {
            // Debug.LogError("IE_AppearenceProgresse IE_AppearenceProgresse IE_AppearenceProgresse");
            yield break;
        }

        float crrAppearence = (float)UserInfoManager.Instance.GetSaveAwareness() * 0.01f;
        float clac = 0.0f;
        while (true)
        {
            if (clac >= crrAppearence)
            {
                break;
            }
            yield return new WaitForSeconds(0.001f);
            clac += 0.01f;
            fillAppearenceValue.fillAmount = clac;
        }
    }


    void InvokeIsActionBtn()
    {
        isActionBtn = false;
    }


    protected override void OnClosed()
    {

        SceneBase.Instance.PLAY_SFX(eSFX.SFX_21_finish_week);

        base.OnClosed();
    }








}
