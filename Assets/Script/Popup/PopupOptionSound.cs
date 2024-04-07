using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
public class PopupOptionSound : JHPopup
{

    public UILabel labelTitleSound;

    [Header("BGM")]
    public UIButton btnBGM;
    public GameObject[] objBGMOnOff;
    public UILabel labelBtnGBM; /// 배경음 


    [Header("SFX")]
    public UIButton btnSFX;
    public GameObject[] objSFXOnOff;
    public UILabel labelBtnSFX;/// 효과음

    Color32 COLOR_ON = new Color32(80, 38, 21, 255);
    Color32 COLOR_OFF = new Color32(119, 119, 119, 255);



    protected override void OnAwake()
    {
        base.OnAwake();

        btnBGM.onClick.Add(new EventDelegate(OnClickBGM));
        btnSFX.onClick.Add(new EventDelegate(OnClickSFX));


        if (UserInfoManager.Instance.GetSaveBGM() == true)
        {
            objBGMOnOff[0].SetActive(true);
            objBGMOnOff[1].SetActive(false);
            labelBtnGBM.color = COLOR_ON;
        }
        else
        {
            objBGMOnOff[0].SetActive(false);
            objBGMOnOff[1].SetActive(true);
            labelBtnGBM.color = COLOR_OFF;
        }

        if (UserInfoManager.Instance.GetSaveSFX() == true)
        {
            objSFXOnOff[0].SetActive(true);
            objSFXOnOff[1].SetActive(false);

            labelBtnSFX.color = COLOR_ON;
        }
        else
        {
            objSFXOnOff[0].SetActive(false);
            objSFXOnOff[1].SetActive(true);

            labelBtnSFX.color = COLOR_OFF;
        }

    }


    public override void SetData()
    {
        base.SetData();
    }


    protected override void OnClosed()
    {
        base.OnClosed();
    }

    protected override void OnDestroied()
    {
        base.OnDestroied();
    }


    public void OnClickBGM()
    {

        bool isBGM = UserInfoManager.Instance.GetSaveBGM();
        UserInfoManager.Instance.SetSaveBGM(!isBGM);

        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);
        if (UserInfoManager.Instance.GetSaveBGM() == true)
        {
            objBGMOnOff[0].SetActive(true);
            objBGMOnOff[1].SetActive(false);
            labelBtnGBM.color = COLOR_ON;

            SceneBase.Instance.PLAY_BGM(eBGM.BGM_MAIN);
        }
        else
        {
            objBGMOnOff[0].SetActive(false);
            objBGMOnOff[1].SetActive(true);
            labelBtnGBM.color = COLOR_OFF;

            SceneBase.Instance.PLAY_BGM(eBGM.NONE);
        }
    }


    public void OnClickSFX()
    {
        bool isFxs = UserInfoManager.Instance.GetSaveSFX();
        UserInfoManager.Instance.SetSaveSFX(!isFxs);

        if (UserInfoManager.Instance.GetSaveSFX() == true)
        {
            objSFXOnOff[0].SetActive(true);
            objSFXOnOff[1].SetActive(false);

            labelBtnSFX.color = COLOR_ON;

            SceneBase.Instance.PLAY_SFX(eSFX.NONE);
        }
        else
        {
            objSFXOnOff[0].SetActive(false);
            objSFXOnOff[1].SetActive(true);

            labelBtnSFX.color = COLOR_OFF;

            SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);
        }
    }

}
