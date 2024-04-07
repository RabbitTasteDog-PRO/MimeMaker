using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PopupOptionPush : JHPopup
{
    public UILabel labelTitlePush;

    [Header("푸시알림")]
    public UIButton btnPush;
    public UILabel labelBtnPush;
    public GameObject[] objPushOnOff;

    [Header("광고성 푸시")]
    public UIButton btnPushAds;
    public UILabel labelBtnPushAds;
    public GameObject[] objPushAdsOnOff;


    [Header("야간푸시")]
    public UIButton btnPushNight;
    public UILabel labelBtnPushNight;
    public UILabel labelBtnPushNightSub;
    public GameObject[] objPushNight;


    Color32 COLOR_ON = new Color32(80, 38, 21, 255);
    Color32 COLOR_OFF = new Color32(119, 119, 119, 255);

    protected override void OnAwake()
    {
        base.OnAwake();

        btnPush.onClick.Add(new EventDelegate(OnClickPush));
        btnPushAds.onClick.Add(new EventDelegate(OnClickPushAds));
        btnPushNight.onClick.Add(new EventDelegate(OnClickPushNight));

        objPushOnOff[0].SetActive(UserInfoManager.Instance.GetSavePush());
        objPushOnOff[1].SetActive(!UserInfoManager.Instance.GetSavePush());
        labelBtnPush.color = UserInfoManager.Instance.GetSavePush() == true ? COLOR_ON : COLOR_OFF;

        objPushAdsOnOff[0].SetActive(UserInfoManager.Instance.GetSavePushAds());
        objPushAdsOnOff[1].SetActive(!UserInfoManager.Instance.GetSavePushAds());
        labelBtnPushAds.color = UserInfoManager.Instance.GetSavePushAds() == true ? COLOR_ON : COLOR_OFF;

        objPushNight[0].SetActive(UserInfoManager.Instance.GetSavePushNight());
        objPushNight[1].SetActive(!UserInfoManager.Instance.GetSavePushNight());
        labelBtnPushNight.color = UserInfoManager.Instance.GetSavePushNight() == true ? COLOR_ON : COLOR_OFF;
        labelBtnPushNightSub.color = UserInfoManager.Instance.GetSavePushNight() == true ? COLOR_ON : COLOR_OFF;

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


    public void OnClickPush()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        bool isPush = UserInfoManager.Instance.GetSavePush();
        UserInfoManager.Instance.SetSavePush(!isPush);

        if (UserInfoManager.Instance.GetSavePush() == true)
        {
            objPushOnOff[0].SetActive(true);
            objPushOnOff[1].SetActive(false);
            labelBtnPush.color = COLOR_ON;
        }
        else
        {
            objPushOnOff[0].SetActive(false);
            objPushOnOff[1].SetActive(true);
            labelBtnPush.color = COLOR_OFF;
        }
    }


    public void OnClickPushAds()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        bool pushAds = UserInfoManager.Instance.GetSavePushAds();
        UserInfoManager.Instance.SetSavePushAds(!pushAds);

        if (UserInfoManager.Instance.GetSavePushAds() == true)
        {
            objPushAdsOnOff[0].SetActive(true);
            objPushAdsOnOff[1].SetActive(false);

            labelBtnPushAds.color = COLOR_ON;
        }
        else
        {
            objPushAdsOnOff[0].SetActive(false);
            objPushAdsOnOff[1].SetActive(true);

            labelBtnPushAds.color = COLOR_OFF;
        }
    }

    public void OnClickPushNight()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        bool pushNight = UserInfoManager.Instance.GetSavePushNight();
        UserInfoManager.Instance.SetSavePushNight(!pushNight);

        if (UserInfoManager.Instance.GetSavePushNight() == true)
        {
            objPushNight[0].SetActive(true);
            objPushNight[1].SetActive(false);

            labelBtnPushNight.color = COLOR_ON;
            labelBtnPushNightSub.color = COLOR_ON;
        }
        else
        {
            objPushNight[0].SetActive(false);
            objPushNight[1].SetActive(true);

            labelBtnPushNight.color = COLOR_OFF;
            labelBtnPushNightSub.color = COLOR_OFF;
        }
    }

}
