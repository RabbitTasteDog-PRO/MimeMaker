using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class StateDetaileMeme : MonoBehaviour
{
    [Header("Meme")]
    public UILabel labelNameMeme; // 밈
    public UI2DSprite spriteMeme; /// 밈 이미지 
    public UISprite spriteFillHp;
    public UILabel labelMemeHp; // 체력
    public UISprite spriteFillAwareness;
    public UILabel labelMemeAwaren; // 인지도 
    public UILabel labelStateTitle; // 스텟
    public UILabel[] labelState;
    public UILabel[] labelStatePoint;
    public UISprite[] spriteFillState;


    public void SetMemeUI()
    {
        for (int i = 0; i < (int)eState.COUNT; i++)
        {
            string _state = ((eState)i).ToString();
            eGlobalTextKey _key = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(_state);
            string globalText = SceneBase.Instance.dataManager.GetDicGlobalTextData(_key);

            labelState[i].text = globalText;

            int point = UserInfoManager.Instance.getSaveState(((eState)i).ToString());
            spriteFillState[i].fillAmount = (float)(point * 0.01f);
            labelStatePoint[i].text = point.ToString();
        }


        int HP = UserInfoManager.Instance.GetSaveHP();
        int awar = UserInfoManager.Instance.GetSaveAwareness();

        spriteFillHp.fillAmount = (float)HP * 0.01f;
        spriteFillAwareness.fillAmount = (float)awar * 0.01f;

        if(UserInfoManager.Instance.GetSaveInAppPrimiumItem( eShopPurchaseKey.eStorePrimium_1.ToString() ) == true)
        {
            /// eTextHPInfinity

            labelMemeHp.text =  SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eTextHPInfinity);
            labelMemeHp.width = 163;
            labelMemeHp.height = 69;
            labelMemeHp.fontSize = 100;
        }
        else
        {
            labelMemeHp.text = UserInfoManager.Instance.GetSaveHP().ToString();
        }

        labelMemeAwaren.text = UserInfoManager.Instance.GetSaveAwareness().ToString();

    }

}
