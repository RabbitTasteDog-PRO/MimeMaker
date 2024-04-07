using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using StructuredDataType;

public class PopupStateDetaile : JHPopup
{

    [Header("GameObject")]

    public StateDetaileMeme objMeme;
    public StateDetaileManager objManager;

    public UIButton btnConfirm;

    [Header("Toogle")]
    public UIButton btnMemeOff;
    public GameObject objMemeOn;
    public UIButton btnManagerOff;
    public GameObject objManagerOn;

    public UILabel labelTitle; // 상태보기

    protected override void OnAwake()
    {
        base.OnAwake();

        /// 광고 이벤트 Set
        btnMemeOff.onClick.Add(new EventDelegate(OnClickMemeOff));
        btnManagerOff.onClick.Add(new EventDelegate(OnClickManagerOff));
        btnConfirm.onClick.Add(new EventDelegate(OnClosed));
    }

    protected override void OnDestroied()
    {
        base.OnDestroied();
    }


    protected override void OnStart()
    {
        base.OnStart();
        // SetUI();

        btnMemeOff.gameObject.SetActive(false);
        objMemeOn.SetActive(true);

        btnManagerOff.gameObject.SetActive(true);
        objManagerOn.SetActive(false);

        objMeme.gameObject.SetActive(true);
        objManager.gameObject.SetActive(false);
        objMeme.SetMemeUI();
    }

    protected override void OnClosed()
    {
        base.OnClosed();
    }


    void OnClickMemeOff()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        btnMemeOff.gameObject.SetActive(false);
        objMemeOn.SetActive(true);

        btnManagerOff.gameObject.SetActive(true);
        objManagerOn.SetActive(false);

        objMeme.gameObject.SetActive(true);
        objManager.gameObject.SetActive(false);
        objMeme.SetMemeUI();
    }


    void OnClickManagerOff()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);
        

        btnMemeOff.gameObject.SetActive(true);
        objMemeOn.SetActive(false);

        btnManagerOff.gameObject.SetActive(false);
        objManagerOn.SetActive(true);

        objMeme.gameObject.SetActive(false);
        objManager.gameObject.SetActive(true);

        objManager.SetManagerUI();

    }

}
