using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PopupStateFixUp : JHPopup
{

    public UILabel labelFixUpDesc_0;
    public UILabel labelFexUpDesc_1;

    public GameObject[] objState;

    public UIButton btnStateFixUp;
    public UISprite spriteBtnStateDia;
    public UILabel labelBtnStateFixUp;
    public UILabel labelDiaAmount;



    Color32 LABEL_STATE_ON = new Color32(99, 31, 22, 255);
    Color32 LABEL_STATE_OFF = new Color32(162, 130, 224, 255);


    Color32 LABEL_FIX_UP_OFF = new Color32(127, 127, 127, 255);
    Color32 LABEL_FIX_UP_ON = Color.white;



    string SPRITE_FIX_UP_ON = "bar_4";
    string SPRITE_FIX_UP_OFF = "bar_1";



    string BTN_DIA_ON = "icon_dia_on";
    string BTN_DIA_OFF = "icon_dia_off";
    string BTN_FIX_UP_OFF = "button_off";
    string BTN_FIX_UP_ON = "button_on";


    protected override void OnAwake()
    {
        base.OnAwake();

        for (int i = 0; i < objState.Length; i++)
        {
            objState[i].name = ((Enums.eState)i).ToString();

            EventDelegate del = new EventDelegate(this, "OnClickState");
            EventDelegate.Parameter parm = new EventDelegate.Parameter();
            parm.value = (Enums.eState)i;
            parm.expectedType = typeof(Enums.eState);
            del.parameters[0] = parm;
            EventDelegate.Add(objState[i].GetComponent<UIButton>().onClick, del);
        }

        /// 현재 내 스텟 
        for (int i = 0; i < objState.Length; i++)
        {
            Enums.eGlobalTextKey _key = RayUtils.Utils.ConvertEnumData<Enums.eGlobalTextKey>(((Enums.eState)i).ToString());
            int crrState = UserInfoManager.Instance.getSaveState(((Enums.eState)i).ToString());

            objState[i].transform.Find("LabelState").GetComponent<UILabel>().text = SceneBase.Instance.dataManager.GetDicGlobalTextData(_key);
            objState[i].transform.Find("LabelValue").GetComponent<UILabel>().text = crrState.ToString();
            objState[i].transform.Find("Value").GetComponent<UISprite>().fillAmount = (float)(crrState) * 0.01f;
        }

        btnStateFixUp.onClick.Add(new EventDelegate(OnClickStateUp));

    }


    protected override void OnClosed()
    {
        base.OnClosed();
    }



    int SelectIndex = -1;
    void OnClickState(Enums.eState _state)
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        if (SelectIndex == (int)_state)
        {
            return;
        }
        Debug.LogError("Select " + _state.ToString());

        SelectIndex = (int)_state;
        for (int i = 0; i < objState.Length; i++)
        {
            if (i == (int)_state)
            {
                objState[i].transform.Find("ProgressCover").GetComponent<UISprite>().spriteName = SPRITE_FIX_UP_ON;
                objState[i].transform.Find("LabelState").GetComponent<UILabel>().color = LABEL_STATE_ON;
                objState[i].transform.Find("LabelValue").GetComponent<UILabel>().color = LABEL_STATE_ON;
            }
            else
            {
                objState[i].transform.Find("ProgressCover").GetComponent<UISprite>().spriteName = SPRITE_FIX_UP_OFF;
                objState[i].transform.Find("LabelState").GetComponent<UILabel>().color = LABEL_STATE_OFF;
                objState[i].transform.Find("LabelValue").GetComponent<UILabel>().color = LABEL_STATE_OFF;
            }
        }

        btnStateFixUp.GetComponent<BoxCollider2D>().enabled = true;
        btnStateFixUp.normalSprite = BTN_FIX_UP_ON;
        spriteBtnStateDia.spriteName = BTN_DIA_ON;
        labelDiaAmount.color = LABEL_STATE_ON;
        labelBtnStateFixUp.color = LABEL_FIX_UP_ON;
        labelBtnStateFixUp.effectStyle = UILabel.Effect.Outline;
    }


    void OnClickStateUp()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        /// TODO : 테스트용으로 주석 가림 , 풀것
        int dia = int.Parse(UserInfoManager.Instance.GetDia());
        if (dia < 10)
        {
            // SceneBase.Instance.AddTestTextPopup("Empty Dia // MyDia : " + dia + " // buyDia : " + 10);
            // SceneBase.Instance.AddTestTextPopup("Empty Dia // My Dia : " + dia + " // price : " + 10);
            string _msg = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.TextEmptyDia);
            SceneBase.Instance.AddEmptyPurchasePopup(_msg);
            // Debug.LogError("Empty Dia");
            return;
        }

        if (UserInfoManager.Instance.GetStateBuyDia() == true)
        {
            string _msg = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eTextBuyOnChance);
            SceneBase.Instance.AddEmptyPurchasePopup(_msg);
            // SceneBase.Instance.AddEmptyPurchasePopup("최초 1회만 구입가능합니다.");
            return;
        }

        StartCoroutine(IEStateProgress((Enums.eState)SelectIndex));


    }
    bool isActionAnimation = false;
    ///<summary>
    /// 스텟 선택 후 올릴 프로그레스 
    ///</summary>
    IEnumerator IEStateProgress(Enums.eState _state)
    {
        isActionAnimation = true;
        UserInfoManager.Instance.SetStateBuyDia(isActionAnimation);

        Debug.LogError("Start Up Point " + _state.ToString());

        int crrPoint = UserInfoManager.Instance.getSaveState(_state.ToString());
        int plusPoint = 10;

        //// TODO : 테스트용으로 주석, 주석제거 풀것 
        UserInfoManager.Instance.setSaveState(_state.ToString(), (crrPoint + plusPoint));

        btnStateFixUp.GetComponent<BoxCollider2D>().enabled = false;
        // btnStateFixUp.normalSprite = BTN_FIX_UP_OFF;
        // spriteBtnStateDia.spriteName = BTN_DIA_OFF;

        // labelDiaAmount.color = LABEL_STATE_OFF;
        // labelBtnStateFixUp.color = LABEL_FIX_UP_OFF;
        // labelBtnStateFixUp.effectStyle = UILabel.Effect.None;

        UISprite _fill = objState[(int)_state].transform.Find("Value").GetComponent<UISprite>();
        UILabel _value = objState[(int)_state].transform.Find("LabelValue").GetComponent<UILabel>();
        float amount = objState[(int)_state].transform.Find("Value").GetComponent<UISprite>().fillAmount;

        yield return new WaitForEndOfFrame();
        while (true)
        {
            Debug.LogError("############## amount : " + amount + " // (crrPoint + plusPoint) : " + (float)(crrPoint + plusPoint) * 0.01f);
            if (amount >= (float)(crrPoint + plusPoint) * 0.01f)
            {
                _fill.fillAmount = (float)(crrPoint + plusPoint) * 0.01f;
                _value.text = (crrPoint + plusPoint).ToString();
                isActionAnimation = false;
                break;
            }
            yield return new WaitForSeconds(0.01f);
            amount += 0.01f;
            _fill.fillAmount = amount;
        }

        btnStateFixUp.GetComponent<BoxCollider2D>().enabled = true;
        btnStateFixUp.normalSprite = BTN_FIX_UP_ON;
        spriteBtnStateDia.spriteName = BTN_DIA_ON;
        labelDiaAmount.color = LABEL_STATE_ON;
        labelBtnStateFixUp.color = LABEL_FIX_UP_ON;
        labelBtnStateFixUp.effectStyle = UILabel.Effect.Outline;



    }


}
