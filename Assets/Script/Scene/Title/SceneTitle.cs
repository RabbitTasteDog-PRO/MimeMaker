// #define TEST 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;


public class SceneTitle : JHScene
{


    public UIButton btnIngame;

    [Header("테스트용 버튼")]
    public UIButton btnDelete;
    public UIButton btnGold;
    public UIButton btnDia;
    public UIButton btnDayPlus;
    public UIButton btnHPZero;
    public UILabel labelVersion;
    public UIButton btnHPTen;

    public GameObject objLogo;

    public UIButton btnIngameScene;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        btnIngame.onClick.Add(new EventDelegate(OnClickIngame));
        btnDelete.onClick.Add(new EventDelegate(OnClickDelte));
        btnGold.onClick.Add(new EventDelegate(OnClickGoldAdd));
        btnDia.onClick.Add(new EventDelegate(OnClickDiaAdd));
        btnDayPlus.onClick.Add(new EventDelegate(OnClickDayPlus));
        btnHPZero.onClick.Add(new EventDelegate(OnClickHPZero));
        btnHPTen.onClick.Add(new EventDelegate(OnCickHP10));

        SceneBase.Instance.PLAY_BGM(eBGM.BGM_MAIN);

        btnIngameScene.onClick.Add(new EventDelegate(OnClickIngame));
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        // StartCoroutine(IEFreeVideoTimeCount());
        yield return new WaitForSeconds(1.0f);
        objLogo.SetActive(true);

        // UserInfoManager.Instance.SetSaveInAppPrimiumItem( eShopPurchaseKey.eStorePrimium_1.ToString() , true);

        // UserInfoManager.Instance.setSaveState(eState.eFACE.ToString(), 100);
        // UserInfoManager.Instance.setSaveState(eState.eACTING.ToString(), 50);
        // UserInfoManager.Instance.SetSaveAwareness(100);
        // UserInfoManager.Instance.setSaveState(eState.eSINGING.ToString(), 100);
        // UserInfoManager.Instance.setSaveState(eState.eHUMOR.ToString(), 100);
        // UserInfoManager.Instance.setSaveState(eState.eFLEX.ToString(), 100);
        // UserInfoManager.Instance.setSaveState(eState.eKNOW.ToString(), 100);
        // UserInfoManager.Instance.setSaveState(eState.ePERSONALITY.ToString(), 100);
        // UserInfoManager.Instance.setSaveState(eState.eCHARACTER.ToString(), 100);

        // UserInfoManager.Instance.SetSaveAwareness(30);

        // UserInfoManager.Instance.setSaveState(eState.eFACE.ToString(), 1);
        // UserInfoManager.Instance.setSaveState(eState.eACTING.ToString(), 12);
        // UserInfoManager.Instance.setSaveState(eState.eSINGING.ToString(), 12);
        // UserInfoManager.Instance.setSaveState(eState.eHUMOR.ToString(), 9);
        // UserInfoManager.Instance.setSaveState(eState.eFLEX.ToString(), 75);
        // UserInfoManager.Instance.setSaveState(eState.eKNOW.ToString(), 18);
        // UserInfoManager.Instance.setSaveState(eState.ePERSONALITY.ToString(), 0);
        // UserInfoManager.Instance.setSaveState(eState.eCHARACTER.ToString(), 2);




    }

    void OnClickIngame()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        JHSceneManager.Instance.Action(JHSceneManager.ACTION.ACTION_PUSH, Strings.SCENE_INGAME);
    }

    void OnClickDelte()
    {
        SecurityPlayerPrefs.DeleteAll();
    }

    void OnClickGoldAdd()
    {
        UserInfoManager.Instance.SetGold(1000000.ToString());
    }
    void OnClickDiaAdd()
    {
        UserInfoManager.Instance.SetDia(10000.ToString());
    }

    void OnClickDayPlus()
    {
        UserInfoManager.Instance.SetDayCount(49);
    }

    void OnClickHPZero()
    {
        UserInfoManager.Instance.SetSaveHP(0);
    }

    void OnCickHP10()
    {
        UserInfoManager.Instance.SetSaveHP(10);
    }

}
