using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Enums;

enum BUTTON_INDEX
{
    NONE = -1,
    BTN_1,
    BTN_2,
    BTN_3,
    COUNT,
}

public class PopupDreamEvent : JHPopup
{

    public static System.Action ACTION_DREAM_END;

    public GameObject objExplain;

    public GameObject[] objCard;
    public UIButton[] btnCard;

    public PrefabItemCard _prefabCard;
    List<PrefabItemCard> listCard = new List<PrefabItemCard>();
    public UI2DSpriteAnimation animFeary;

    public UIButton btnVideoRetry;
    public UILabel labelBtnVideoRetry; /// 다시선택
    public UIButton btnConfirm;
    public UILabel labelBtnConfirm; /// 넘어가기
    public UIButton btnShuffle;
    public UILabel labelBtnShuffle;
    public UILabel labelShufflePrice;

    public UIGrid gridButtons;

    public UILabel labelTouchCard;


    STDreamEventData[] dreamData = null; ///
    Vector2[] CARD_ORI_POS;

    int FIRST_SHUFFLE = 500;
    int RE_SHUFFLE = 100;

    int GACHA_INDEX = 0;

    bool isActionShuffle = false;


    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        ///back 키 처리..
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isBackButton == false)
            {
                return;
            }
        }
    }



    protected override void OnAwake()
    {
        base.OnAwake();

        SceneBase.Instance.PLAY_BGM(eBGM.BGM_DREAM);

        labelTouchCard.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eDreamEventTouchCard);
        labelTouchCard.gameObject.SetActive(false);

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

                    objExplain.transform.localPosition = new Vector2(objExplain.transform.localPosition.x, objExplain.transform.localPosition.y + 130);
                    gridButtons.transform.localPosition = new Vector2(gridButtons.transform.localPosition.x, gridButtons.transform.localPosition.y - 130);

                    animFeary.transform.localPosition = new Vector3(animFeary.transform.localPosition.x, animFeary.transform.localPosition.y + 50);
                }
                else
                {
                    objExplain.transform.localPosition = new Vector2(objExplain.transform.localPosition.x, objExplain.transform.localPosition.y + 100);
                    gridButtons.transform.localPosition = new Vector2(gridButtons.transform.localPosition.x, gridButtons.transform.localPosition.y - 100);

                    animFeary.transform.localPosition = new Vector3(animFeary.transform.localPosition.x, animFeary.transform.localPosition.y + 30);
                }
            }
        }

        animFeary.framesPerSecond = 10;

        btnVideoRetry.gameObject.SetActive(false);
        btnConfirm.gameObject.SetActive(false);
        btnShuffle.gameObject.SetActive(false);

        labelBtnShuffle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eShuffle);
        labelShufflePrice.text = ShuffleCnt > 0 ? string.Format("-{0}", RE_SHUFFLE) : string.Format("-{0}", FIRST_SHUFFLE);


        for (int i = 0; i < btnCard.Length; i++)
        {
            EventDelegate del = new EventDelegate(this, "OnClickCard");
            EventDelegate.Parameter parm = new EventDelegate.Parameter();
            parm.value = (BUTTON_INDEX)i;
            parm.expectedType = typeof(BUTTON_INDEX);
            del.parameters[0] = parm;
            EventDelegate.Add(btnCard[i].onClick, del);

            objCard[i].gameObject.SetActive(true);
        }

        btnShuffle.onClick.Add(new EventDelegate(OnClickShuffle));
        btnVideoRetry.onClick.Add(new EventDelegate(OnClickVideoRetry));
        btnConfirm.onClick.Add(new EventDelegate(OnClickNext));

        labelBtnVideoRetry.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eBtnVideoRetry);
        labelBtnConfirm.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eBtnConfirmNext);

        if (CARD_ORI_POS != null)
        {
            CARD_ORI_POS.Initialize();
        }

        CARD_ORI_POS = new Vector2[objCard.Length];

        for (int i = 0; i < objCard.Length; i++)
        {
            CARD_ORI_POS[i] = objCard[i].transform.localPosition;
            if (objCard[i].transform.childCount > 0)
            {
                objCard[i].transform.DestroyChildren();
            }
        }

        PopupAdMobStart.ACTION_CLOSE_ADMOB_START_POPUP = ACTION_CLOSE_ADMOB_START_POPUP; /// <= 왜 안먹지 

        PopupAdMobStart.ACTION_SKILL_CHAGE = ACTION_CLOSE_ADMOB_START_POPUP; /// <= 왜 안먹지 

        PopupChanageCardGold.ACTION_CARD_CHANGE = ACTION_CARD_CHANGE;
        PopupChanageCardVideo.ACTION_CARD_VIDEO_CHANGE = ACTION_CARD_VIDEO_CHANGE;

        int _price = FIRST_SHUFFLE ;
        labelShufflePrice.text = string.Format("-{0:00}", _price);
    }

    protected override void OnDestroied()
    {
        base.OnDestroied();

        JHAdsManager.ACTION_ADS_COMPLETE = null;
        PopupAdMobStart.ACTION_CLOSE_ADMOB_START_POPUP = null;
        PopupAdMobStart.ACTION_SKILL_CHAGE = null;

        PopupChanageCardGold.ACTION_CARD_CHANGE = null;
    }


    protected override void OnClosed()
    {
        base.OnClosed();

        if (ACTION_DREAM_END != null && ACTION_DREAM_END.GetInvocationList().Length > 0)
        {
            ACTION_DREAM_END();
            SceneBase.RefrashGoldDia();
        }

    }

    protected override void OnStart()
    {
        base.OnStart();
        /// 카드 애니메이션 
        StartCoroutine(IECreateCardUI());
    }
    bool isActionCreateCard = false;
    ///<summary>
    /// 카드 생성
    ///</summary>
    IEnumerator IECreateCardUI()
    {
        labelTouchCard.gameObject.SetActive(false);
        isActionCreateCard = true;
        dreamData = new STDreamEventData[btnCard.Length];
        if (listCard.Count > 0)
        {
            listCard.Clear();
        }
        for (int i = 0; i < btnCard.Length; i++)
        {
            objCard[i].GetComponent<BoxCollider2D>().enabled = false;

            //// 카드 세이터 세팅 
            // Enums.eDremEvent _envetKind = DreamEventKindSelect();
            Enums.eDreamEventGrade _grade = (Enums.eDreamEventGrade)((i == btnCard.Length - 1) ? (Random.Range((int)Enums.eDreamEventGrade.A, (int)Enums.eDreamEventGrade.S)) : i);
            List<STDreamEventData> _listData = SceneBase.Instance.dataManager.GetListSTDreamEventData(_grade);
            Debug.LogError("Dream Event Grade : " + _grade.ToString());

            /// 꿈이벤트 데이터 세팅
            STDreamEventData _data = DreamEventKindSetting(_listData);
            dreamData[i] = _data;
        }

        /// 꿈이벤트 데이터 세팅
        // SetDreamEventResult();

        yield return new WaitForSeconds(1.0f);

        SceneBase.Instance.PLAY_SFX(eSFX.SFX_14_card_open);

        // while (true)
        for (int i = 0; i < btnCard.Length; i++)
        {

            yield return new WaitForSeconds(0.3f);
            //// UI 세팅

            PrefabItemCard _card = Instantiate(_prefabCard, objCard[i].transform) as PrefabItemCard;
            _card.transform.localScale = Vector3.one;
            _card.transform.localPosition = Vector2.zero;

            TweenAlpha _alpha = objCard[i].GetComponent<TweenAlpha>();
            _alpha.ResetToBeginning();
            _alpha.from = 0;
            _alpha.to = 1;
            _alpha.enabled = true;


            TweenPosition _tween = objCard[i].GetComponent<TweenPosition>();
            _tween.ResetToBeginning();
            Transform _objCard = objCard[i].transform;
            Vector2 _from = new Vector2(CARD_ORI_POS[i].x - 20, CARD_ORI_POS[i].y);
            Vector2 _to = CARD_ORI_POS[i];
            _tween.from = _from;
            _tween.to = _to;
            _tween.enabled = true;


            if (isVideoCheck == true)
            {
                if (i < GACHA_INDEX)
                {
                    _card.gameObject.SetActive(false);
                    objCard[i].SetActive(false);
                }
                else
                {
                    _card.gameObject.SetActive(true);
                    objCard[i].SetActive(true);
                }

            }
            else
            {
                _card.gameObject.SetActive(true);
                objCard[i].SetActive(true);
            }

            _card.SetFirstItemDataUICard(dreamData[i]);
            listCard.Add(_card);
        }

        yield return new WaitForEndOfFrame();

        for (int i = 0; i < objCard.Length; i++)
        {
            UIButton btn = objCard[i].GetComponent<UIButton>();
            btn.defaultColor = Color.white;
        }
        if (isVideoCheck == true)
        {
            btnVideoRetry.gameObject.SetActive(false);
        }
        else
        {
            btnVideoRetry.gameObject.SetActive(GACHA_INDEX > 0 ? true : false);
        }
        btnShuffle.gameObject.SetActive(GACHA_INDEX > 0 ? false : true);
        btnConfirm.gameObject.SetActive(true);
        gridButtons.Reposition();

        // isVideoCheck = false;
        cardIndex = -1;
        isActionCreateCard = false;

    }

    int cardIndex = -1;
    ///<summary>
    /// 카드 선택 온클릭 매소드 
    ///</summary>
    void OnClickCard(BUTTON_INDEX _index)
    {

        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);


        for (int i = 0; i < btnCard.Length; i++)
        {
            if ((int)_index != i)
            {
                objCard[i].SetActive(false);
            }

        }

        if (GACHA_INDEX >= 1)
        {
            cardIndex = Random.Range(1, 2);//(int)_index;
            Debug.LogError("OnClickCard Index : " + _index + " // cardIndex  : " + cardIndex);
            btnCard[(int)_index].GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(IEStartCardAnimation((int)_index, cardIndex));

        }
        else
        {
            cardIndex = (int)_index;
            btnCard[(int)_index].GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(IEStartCardAnimation((int)_index));
        }




    }

    
    ///<summary>
    /// 셔플 처리
    ///</summary>
    void OnClickShuffle()
    {
        int _gold = int.Parse(UserInfoManager.Instance.GetGold());
        // int _price = ShuffleCnt > 0 ? RE_SHUFFLE : FIRST_SHUFFLE;
        int _price = FIRST_SHUFFLE;
        if (ShuffleCnt >= 1)
        {
            _price = FIRST_SHUFFLE + (RE_SHUFFLE * ShuffleCnt);
        }
        Transform _root = PopupManager.Instance.RootPopup;
        // /// 카드 생성중이라면 리턴 
        if (isActionCreateCard == true)
        {
            return;
        }
        //// 셔플 동작중이라면 리턴
        if (isActionShuffle == true)
        {
            return;
        }
        isActionShuffle = true;
        //// 돈없다면 리턴
        if (_gold < _price)
        {
            // SceneBase.Instance.AddEmptyPurchasePopup("골드가 부족합니다");
            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupChanageCardGoldEmpty);
            return;
        }
        else
        {
            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupChanageCardGold, _price);
        }
        Invoke("InvokeIsActionShuffle", 3.0f);

        // dreamData.Initialize();
        // /// 카드리스트 클리어
        // listCard.Clear();

        // for (int i = 0; i < objCard.Length; i++)
        // {
        //     if (objCard[i].transform.childCount > 0)
        //     {
        //         /// 하위 오브젝트 삭제
        //         objCard[i].transform.DestroyChildren();
        //         /// 위치 초기화
        //         objCard[i].transform.localPosition = CARD_ORI_POS[i];
        //     }
        // }

        // StartCoroutine(IECreateCardUI());

        // UserInfoManager.Instance.SetGold((_gold - _price).ToString());

        // ShuffleCnt++;
        // FIRST_SHUFFLE += RE_SHUFFLE;
        // labelShufflePrice.text = ShuffleCnt > 0 ? string.Format("-{0}", FIRST_SHUFFLE + RE_SHUFFLE) : string.Format("-{0}", FIRST_SHUFFLE);


    }
    int ShuffleCnt = 0;
    void ACTION_CARD_CHANGE(bool flag)
    {
        isActionShuffle = true;
        ShuffleCnt++;

        int _gold = int.Parse(UserInfoManager.Instance.GetGold());
        int _price = FIRST_SHUFFLE;
        /// 카드 생성중이라면 리턴 
        if (isActionCreateCard == true)
        {
            return;
        }
        
        //// 돈없다면 리턴
        if (_gold < _price)
        {
            SceneBase.Instance.AddEmptyPurchasePopup("골드가 부족합니다");
            return;
        }

        dreamData.Initialize();
        /// 카드리스트 클리어
        listCard.Clear();

        for (int i = 0; i < objCard.Length; i++)
        {
            if (objCard[i].transform.childCount > 0)
            {
                /// 하위 오브젝트 삭제
                objCard[i].transform.DestroyChildren();
                /// 위치 초기화
                objCard[i].transform.localPosition = CARD_ORI_POS[i];
            }
        }
        StartCoroutine(IECreateCardUI());
        Invoke("InvokeIsActionShuffle", 3.0f);
        // labelShufflePrice.text = ShuffleCnt > 0 ? string.Format("-{0}", FIRST_SHUFFLE + RE_SHUFFLE) : string.Format("-{0}", FIRST_SHUFFLE);
        if (ShuffleCnt >= 1)
        {
            _price = FIRST_SHUFFLE + (RE_SHUFFLE * ShuffleCnt);
        }
        labelShufflePrice.text = string.Format("-{0:00}", _price);
    }

    void InvokeIsActionShuffle()
    {
        isActionShuffle = false;
    }



    bool isSelect = false;
    ///<summary>
    /// 데이터 받고 데이터 적용 
    ///</summary>
    async void OnClickNext()
    {
        if (isSelect == false)
        {
            if (GACHA_INDEX >= 1)
            {
                System.Random rand = new System.Random();
                eventDataInex = rand.Next(1, 2);
            }
            else
            {
                eventDataInex = SetDreamProb();

            }
            Debug.LogError("나올 카드 확률적용된 인덱스 : " + eventDataInex);

            for (int i = 0; i < objCard.Length; i++)
            {
                Vector2 objPos = objCard[i].transform.localPosition;

                objCard[i].GetComponent<TweenPosition>().ResetToBeginning();
                objCard[i].GetComponent<TweenPosition>().from = objPos;
                objCard[i].GetComponent<TweenPosition>().to = Vector2.zero;
                objCard[i].GetComponent<TweenPosition>().duration = 0.3f;
                objCard[i].GetComponent<TweenPosition>().PlayForward();

            }
            // objCard[cardIndex].GetComponent<TweenPosition>().ResetToBeginning();
            // objCard[cardIndex].GetComponent<TweenPosition>().from = objPos;
            // objCard[cardIndex].GetComponent<TweenPosition>().to = Vector2.zero;

            await Task.Delay(300);

            isSelect = true;
            for (int i = 0; i < objCard.Length; i++)
            {
                if (i == eventDataInex)
                {
                    listCard[eventDataInex].SetItemCardUI();
                    objCard[eventDataInex].gameObject.SetActive(true);
                    objCard[eventDataInex].GetComponent<BoxCollider2D>().enabled = true;
                }
                else
                {
                    objCard[i].gameObject.SetActive(false);
                    objCard[i].GetComponent<BoxCollider2D>().enabled = false;
                }


            }

            btnVideoRetry.gameObject.SetActive(false);
            btnShuffle.gameObject.SetActive(false);
            btnConfirm.gameObject.SetActive(false);

            labelTouchCard.gameObject.SetActive(true);
        }
    }

    void OnClickConfirm()
    {
        OnClosed();
    }

    ///<summary>
    /// 광고보고 다시뽑기
    ///</summary>
    void OnClickVideoRetry()
    {
        Transform _root = PopupManager.Instance.RootPopup;
        SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupChanageCardVideo);
    }

    void ACTION_CARD_VIDEO_CHANGE(bool flag)
    {
        if (flag == true)
        {
            // Transform _root = PopupManager.Instance.RootPopup;
            // STAdMobStartTextData data = new STAdMobStartTextData();

            // data.strTitle = eGlobalTextKey.eDreamRetryTitle.ToString();
            // data.strDesc = eGlobalTextKey.eDreamRetryMessage.ToString();
            // data.strDescSub = eGlobalTextKey.eDreamRetryMessageDesc.ToString();
            // data.strBtnText = eGlobalTextKey.eBtnDreamRetry.ToString();
            // //// 비디오키
            // data.vidoeKey = eVideoAds.eVideoDreamEvent.ToString();

            // SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupAdMobStart, data);

            Debug.LogError("PopupDreamEvent  ACTION_CLOSE_ADMOB_START_POPUP flag 2222 : " + flag);
            SceneBase.Instance.adsManager.AsyncShowAdMobVideo(eVideoAds.eVideoDreamEvent);
            JHAdsManager.ACTION_ADS_COMPLETE = ACTION_REWARD;
        }
    }

    void ACTION_CLOSE_ADMOB_START_POPUP(bool flag)
    {
        Debug.LogError("PopupDreamEvent  ACTION_CLOSE_ADMOB_START_POPUP flag 11111 : " + flag);
        if (flag == true)
        {
            Debug.LogError("PopupDreamEvent  ACTION_CLOSE_ADMOB_START_POPUP flag 2222 : " + flag);
            SceneBase.Instance.adsManager.AsyncShowAdMobVideo(eVideoAds.eVideoDreamEvent);
            JHAdsManager.ACTION_ADS_COMPLETE = ACTION_REWARD;
        }
    }

    bool isActionRewardSucess = false;
    void ACTION_REWARD(bool flag)
    {
        Debug.LogError("PopupDreamEvent  ACTION_REWARD flag 1111111 : " + flag);
        if (flag == true)
        {
            isVideoCheck = true;
            isActionRewardSucess = flag;
            // OnClosed();

            /// 데이터 초기화
            // dreamData = null;
            dreamData.Initialize();
            /// 카드리스트 클리어
            listCard.Clear();

            /// UI 초기화
            btnVideoRetry.gameObject.SetActive(false);
            btnShuffle.gameObject.SetActive(false);
            btnConfirm.gameObject.SetActive(false);

            for (int i = 0; i < objCard.Length; i++)
            {
                if (objCard[i].transform.childCount > 0)
                {
                    /// 하위 오브젝트 삭제
                    objCard[i].transform.DestroyChildren();
                    /// 위치 초기화
                    objCard[i].transform.localPosition = CARD_ORI_POS[i];
                }
            }

            /// UI 재시작
            StartCoroutine(IECreateCardUI());

            if (btnConfirm.onClick != null)
            {
                btnConfirm.onClick.Clear();
                if (isVideoCheck == false)
                {
                    btnConfirm.onClick.Add(new EventDelegate(OnClickConfirm));
                }
                else
                {
                    if (GACHA_INDEX >= 2)
                    {
                        btnConfirm.onClick.Add(new EventDelegate(OnClickConfirm));
                    }
                    else
                    {
                        btnConfirm.onClick.Add(new EventDelegate(OnClickNext));
                        labelBtnConfirm.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eBtnCardSelect);
                    }
                }

            }
            Debug.LogError("PopupDreamEvent  ACTION_REWARD flag 2222222 : " + flag);
        }

        isActionRewardSucess = false;

    }


    bool isVideoCheck = false;
    int eventDataInex;
    ///<summary>
    /// ㅋㅏ드 애니메이션 시작
    ///</summary>
    IEnumerator IEStartCardAnimation(int cardIndex, int dataIndex = -1)
    {
        labelTouchCard.gameObject.SetActive(false);
        // Vector2 objPos = objCard[cardIndex].transform.localPosition;

        // objCard[cardIndex].GetComponent<TweenPosition>().ResetToBeginning();
        // objCard[cardIndex].GetComponent<TweenPosition>().from = objPos;
        // objCard[cardIndex].GetComponent<TweenPosition>().to = Vector2.zero;
        // yield return new WaitForEndOfFrame();
        // objCard[cardIndex].GetComponent<TweenPosition>().duration = 0.3f;
        // objCard[cardIndex].GetComponent<TweenPosition>().enabled = true;
        // objCard[cardIndex].GetComponent<TweenPosition>().PlayForward();

        for (int i = 0; i < objCard.Length; i++)
        {
            objCard[i].SetActive(cardIndex == i);
        }
        yield return new WaitForSeconds(0.3f);


        PrefabItemCard _card = listCard[cardIndex];
        STDreamEventData selectData = null;
        if (GACHA_INDEX == 0)
        {
            // Debug.LogError("eventDataInex : " + eventDataInex);
            selectData = dreamData[eventDataInex];
            _card.SetSartAnimation(dreamData[eventDataInex]);
        }
        else
        {
            if (dataIndex != -1)
            {
                cardIndex = dataIndex;
            }
            selectData = dreamData[cardIndex];
            _card.SetSartAnimation(dreamData[cardIndex]);

        }

        GACHA_INDEX++;

        yield return new WaitForSeconds(2.0f);
        _card.SetEndItemCardUI();
        yield return new WaitForEndOfFrame();

        if (btnConfirm.onClick != null)
        {
            btnConfirm.onClick.Clear();
            if (isVideoCheck == false)
            {
                btnConfirm.onClick.Add(new EventDelegate(OnClickConfirm));
            }
            else
            {
                if (GACHA_INDEX >= 2)
                {
                    btnConfirm.onClick.Add(new EventDelegate(OnClickConfirm));
                }
                else
                {
                    btnConfirm.onClick.Add(new EventDelegate(OnClickNext));
                }
            }

        }
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_15_card);

        yield return new WaitForSeconds(0.5f);



        btnConfirm.gameObject.SetActive(true);
        btnVideoRetry.gameObject.SetActive(GACHA_INDEX >= 2 ? false : true);

        // string nextText = GACHA_INDEX >= 2 ? SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eBtnCardSelect)
        // : SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eBtnConfirmNext);

        string nextText = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eBtnConfirmNext);
        labelBtnConfirm.text = nextText;


        btnShuffle.gameObject.SetActive(false);
        isSelect = false;

        gridButtons.Reposition();
        //// 데이터 저장 
        SaveDreamEvent(selectData);
    }



    /*****************************************************************************************************/
    ///<summary>
    /// 꿈이벤트 종류 및 데이터 세팅
    ///</summary>
    Enums.eDremEvent DreamEventKindSelect()
    {
        //[1] Input
        Enums.eDremEvent _event = Enums.eDremEvent.NONE;
        int[] data =
        {
            (int)Enums.eDreamEventGradeProb.C, (int)Enums.eDreamEventGradeProb.B,
            (int)Enums.eDreamEventGradeProb.A, (int)Enums.eDreamEventGradeProb.S
        };
        int target = UnityEngine.Random.Range(0, 99); // traget과 가까운 값
        int near = 0; //가까운값 : 27
        int min = int.MaxValue;

        int _index = 0;
        //[2] Process      
        for (int i = 0; i < data.Length; i++)
        {
            if (Abs(data[i] - target) < min)
            {
                min = Abs(data[i] - target); //최소값 알고리즘
                near = data[i]; //최종적으로 가까운 값
                _index = i;
                _event = (Enums.eDremEvent)i;

            }
        }

        // Debug.LogError("############### : " + _index);
        //[3] Output
        Debug.LogError(string.Format("1111111111111111 {0}와 가까운값 : {1} // 이벤트종류 : {2}", target, near, _event)); //25,27     

        return _event;
    }

    ///<summary>
    /// 꿈이벤트 종류 선택 후 꿈이벤트 확률 및 데이터 세팅
    ///</summary>
    STDreamEventData DreamEventKindSetting(List<STDreamEventData> _list)
    {
        STDreamEventData _dreamData = null;

        int[] data = new int[_list.Count];

        int maxProb = 0;

        for (int i = 0; i < _list.Count; i++)
        {
            if (_list[i].prob > maxProb)
            {
                maxProb = _list[i].prob;
            }

            data[i] = _list[i].prob;
        }
        System.Random _random = new System.Random();
        int target = _random.Next(0, maxProb);//UnityEngine.Random.Range(0, maxProb); // traget과 가까운 값
        int near = 0; //가까운값 : 27
        int min = int.MaxValue;

        //[2] 근사값 체크      
        for (int i = 0; i < data.Length; i++)
        {
            if (Abs(data[i] - target) < min)
            {
                min = Abs(data[i] - target); //최소값 알고리즘
                near = data[i]; //최종적으로 가까운 값
                _dreamData = _list[i];
            }
        }

        //[3] Output
        Debug.LogError(string.Format("@@@@@@@@@@@@@@@ {0}와 가까운값 : {1} // 이벤트종류 : {2}", target, near, _dreamData.key_dreamEvent)); //25,27     

        //// 스텟올릴경우 랜덤 처리
        if (_dreamData.key_skillType == Enums.eSkillType.eRandom)
        {
            int randomValue = Random.Range((int)Enums.eSkillType.eFACE, (int)Enums.eSkillType.eCHARACTER);
            _dreamData.key_skillType = (Enums.eSkillType)randomValue;
        }
        return _dreamData;
    }

    int SetDreamProb()
    {
        Enums.eDreamEventGradeProb eProb = Enums.eDreamEventGradeProb.C;

        int[] data = new int[]
        {
            (int)Enums.eDreamEventGradeProb.C, (int)Enums.eDreamEventGradeProb.B,
            (int)Enums.eDreamEventGradeProb.A, (int)Enums.eDreamEventGradeProb.S
        };
        System.Random _random = new System.Random();
        int target = _random.Next(0, 100);
        // int target = UnityEngine.Random.Range(0, 100); // traget과 가까운 값
        int near = 0; //가까운값 : 27
        int min = int.MaxValue;

        //[2] 근사값 체크      
        for (int i = 0; i < data.Length; i++)
        {
            if (Abs(data[i] - target) < min)
            {
                min = Abs(data[i] - target); //최소값 알고리즘
                near = data[i]; //최종적으로 가까운 값
                eProb = (Enums.eDreamEventGradeProb)data[i];
            }
        }

        int cardDataIndex = 0;
        switch (eProb)
        {
            case Enums.eDreamEventGradeProb.C: { cardDataIndex = 0; break; }
            case Enums.eDreamEventGradeProb.B: { cardDataIndex = 1; break; }
            case Enums.eDreamEventGradeProb.A: { cardDataIndex = 2; break; }
            case Enums.eDreamEventGradeProb.S: { cardDataIndex = 2; break; }
        }

        // Debug.LogError(string.Format("########################## Prob {0}와 가까운값 : {1} // 그레이드 : {2} /// 뽑기 번호 : {3}", target, near, eProb.ToString(), cardDataIndex));

        return cardDataIndex;
    }


    int Abs(int p)
    {
        return (p < 0) ? -p : p;
    }

    float Abs(float p)
    {
        return (p < 0) ? -p : p;
    }
    /*****************************************************************************************************/

    ///<summary>
    /// 넘어가기 적용 후 
    /// 꿈 이벤트 데이터 적용
    ///</summary>
    async void SaveDreamEvent(STDreamEventData data)
    {
        int upPoint = int.Parse(data.probValue.ToString());
        // Debug.LogError("data.key_dreamEvent : " + data.key_dreamEvent.ToString() + " // UpPoint : " + upPoint);
        switch (data.key_dreamEvent)
        {
            case Enums.eDremEvent.eD_AWARENESS:
                {
                    int plusAwareness = UserInfoManager.Instance.GetSaveAwareness() + upPoint;
                    UserInfoManager.Instance.SetSaveAwareness(plusAwareness);
                    break;
                }
            case Enums.eDremEvent.eD_DIA:
                {
                    int plusDia = int.Parse(UserInfoManager.Instance.GetDia()) + upPoint;
                    UserInfoManager.Instance.SetDia(plusDia.ToString());
                    break;
                }
            case Enums.eDremEvent.eD_GOLD:
                {
                    int plusGold = int.Parse(UserInfoManager.Instance.GetGold()) + upPoint;
                    UserInfoManager.Instance.SetGold(plusGold.ToString());
                    break;
                }
            case Enums.eDremEvent.eD_HP:
                {
                    int plusHP = UserInfoManager.Instance.GetSaveHP() + upPoint;
                    UserInfoManager.Instance.SetSaveHP(plusHP);
                    break;
                }
            case Enums.eDremEvent.eD_STATE:
                {
                    string _state = data.key_skillType.ToString();

                    int plusState = UserInfoManager.Instance.getSaveState(_state) + upPoint;
                    UserInfoManager.Instance.setSaveState(_state, plusState);
                    break;
                }
        }

        await Task.Delay(800);

        SceneBase.Instance.PLAY_SFX(eSFX.SFX_18_card);

    }

}
