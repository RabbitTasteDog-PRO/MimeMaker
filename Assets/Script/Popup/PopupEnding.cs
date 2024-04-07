using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PopupEnding : JHPopup
{


    public UIButton btnConfirm;
    public UILabel labelConfirm;
    public UIButton btnVideoRetry;
    public UILabel labelVideoRetry;


    public UIScrollView scrollView;
    public UITable tableEnding;
    public UILabel labelEnidngTitle;
    public UILabel labelEndingMent;
    public UI2DSprite spriteEndingImg;

    eEndingNumber _eEnding = eEndingNumber.NONE;

    bool isBuyCheck = false;
    float endingScorllValue = 0;
    public override void SetData(object[] data)
    {
        base.SetData();

        if (data != null)
        {
            _eEnding = (eEndingNumber)data[0];
            isBackButton = (bool)data[1];
            isBuyCheck = (bool)data[2];
            
            //labelVideoRetry = 
            SetEndingUI(_eEnding);
        }

    }
    bool firstCheck = false;
    protected override void OnAwake()
    {

        

        base.OnAwake();

        SceneBase.Instance.PLAY_SFX(eSFX.SFX_24_ending);

        btnVideoRetry.gameObject.SetActive(false);
        isBackButton = false;
        labelVideoRetry.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eBtnEdningVideoTry);
        labelConfirm.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eCheck);

        btnConfirm.onClick.Add(new EventDelegate(OnClickConfirm));
        btnVideoRetry.onClick.Add(new EventDelegate());

        labelEnidngTitle.GetComponent<BoxCollider2D>().enabled = false;
        labelEndingMent.GetComponent<BoxCollider2D>().enabled = false;
        spriteEndingImg.GetComponent<BoxCollider2D>().enabled = false;

        firstCheck = UserInfoManager.Instance.GetSaveEnding(_eEnding.ToString());
    }
    protected override void OnDestroied()
    {
        base.OnDestroied();

        if(isBackButton == false)
        {
            Transform _root = PopupManager.Instance.RootPopup;
            STEndingResult data = new STEndingResult(_eEnding, firstCheck, isBuyCheck);
            SceneBase.Instance.AddPopup(_root, ePopupLayer.PopupEndingResult, data);
        }
    }

    protected override void OnStart()
    {
        base.OnStart();
        scrollView.GetComponent<UIPanel>().depth = GetDepth() + 1;

    }

    protected override void OnClosed()
    {
        base.OnClosed();
    }

    //// 
    void SetEndingUI(eEndingNumber _endig)
    {
        List<xmlEnding> _listED = XmlParser.Read<xmlEnding>("XML/Ending/xmlEndingData");
        xmlEnding dataED = _listED[(int)_endig];

        labelEnidngTitle.text = dataED.ending_Title;
        string strImage = string.Format("NO_{0}", (int)_endig);
        spriteEndingImg.sprite2D = Resources.Load<Sprite>(string.Format("Image/Ending/Album/{0}", strImage));
        labelEndingMent.text = dataED.ment;

        StartCoroutine(IEEndingEnable());

    }

    IEnumerator IEEndingEnable()
    {
        yield return new WaitForSeconds(.5f);
        spriteEndingImg.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        labelEnidngTitle.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        labelEndingMent.gameObject.SetActive(true);
        /// 스크롤 계산
        endingScorllValue = labelEnidngTitle.height + spriteEndingImg.height + labelEndingMent.height;
        yield return new WaitForSeconds(1.0f);

        labelEnidngTitle.GetComponent<BoxCollider2D>().enabled = isBackButton;
        labelEndingMent.GetComponent<BoxCollider2D>().enabled = isBackButton;
        spriteEndingImg.GetComponent<BoxCollider2D>().enabled = isBackButton;

        if (isBackButton == false)
        {
            StartCoroutine(EndingStoryAnimation());
        }


    }

    ///<summary>
    /// 엔딩본후 초기화,횟수카운딩 및 씬으로 이동
    ///</summary>
    void OnClickConfirm()
    {

        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        if (isActionAnimation == true)
        {
            return;
        }
        OnClosed();
    }


    ///<summary>
    ///데이터 초기화 
    ///</summary>
    void initData()
    {
        Debug.LogError("initData 데이터 초기화 ");
        //// 엔딩횟수 증가 
        int crrEndingCnt = UserInfoManager.Instance.GetEndingCount();
        UserInfoManager.Instance.SetEnidngCount(crrEndingCnt + 1);
        /// 날짜 초기화
        UserInfoManager.Instance.SetWeeklyCount(0);
        /// 요일 초기화
        UserInfoManager.Instance.SetDayCount(0);
        // HP초기화
        UserInfoManager.Instance.SetSaveHP(100);
        /// 인지도 초기화 
        UserInfoManager.Instance.SetSaveAwareness(0);
        /// 스케줄 초기화


        UserInfoManager.Instance.SetSaveDreamEvent(UserInfoManager.Instance.GetDayCount(), true);

        for (int i = 0; i < (int)Enums.eWeekly.COUNT; i++)
        {
            UserInfoManager.Instance.SetProgressedWeeklyData(((eWeekly)i).ToString(), false);
            SecurityPlayerPrefs.DeleteKey(Strings.WEEKLY + "_" + ((eWeekly)i).ToString());
        }

        /// 스텟 초기화
        for (int i = 0; i < (int)Enums.eState.COUNT; i++)
        {
            UserInfoManager.Instance.setSaveState(((Enums.eState)i).ToString(), 0);
        }
    }

    bool isActionAnimation = false;
    IEnumerator EndingStoryAnimation()
    {
        isActionAnimation = true;
        float scrollPosY = scrollView.panel.GetViewSize().y;
        float scrollOffsetY = scrollView.panel.clipOffset.y;
        float _calcPos = Mathf.Abs(endingScorllValue - scrollPosY) + (labelEndingMent.height - 100);

        float upPosY = 0.0f;
        while (true)
        {
            upPosY += 1.0f;
            tableEnding.transform.localPosition = new Vector2(-300f, tableEnding.transform.localPosition.y + 0.5f);
            // Debug.LogError("############## _calcPos : " + _calcPos + " // posY : " + upPosY);
            if (_calcPos <= upPosY)
            {
                isActionAnimation = false;
                labelEnidngTitle.GetComponent<BoxCollider2D>().enabled = !isActionAnimation;
                labelEndingMent.GetComponent<BoxCollider2D>().enabled = !isActionAnimation;
                spriteEndingImg.GetComponent<BoxCollider2D>().enabled = !isActionAnimation;
                break;
            }
            yield return new WaitForSeconds(0.01f);

        }
    }
}
