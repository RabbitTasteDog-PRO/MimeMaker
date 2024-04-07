using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using RayUtils;

public class ResultStateProgress : MonoBehaviour
{

    public UIGrid gridBonus;

    public UILabel labelState;
    public UILabel labelBeforPoint;
    public UILabel labelAdjustPoint;
    public UISprite spriteFillValue;

    public GameObject objManagerPoint;
    public UILabel labelManagerPoint;
    public GameObject objBunusPoint;
    public UILabel labelBonusPoint;


    /***************************************************************/
    /// 마이너스 색상 
    Color32 COLOR_MINUS = new Color32(255, 109, 0, 255);
    string SPRITE_MINUS = "gauge_minus";
    Color32 COLOR_PLUS = new Color32(21, 144, 255, 255);
    string SPRITE_PLUS = "gauge_plus";

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        labelManagerPoint.text = string.Empty;
        labelBonusPoint.text = string.Empty;
        labelAdjustPoint.text = string.Empty;

        objManagerPoint.SetActive(false);
        objBunusPoint.SetActive(false);
    }

    public void SetProgress(string _state, int beforPoint, int adjustPoint, string pintType)
    {
        gameObject.SetActive(true);

        spriteFillValue.fillAmount = 0.0f;

        eGlobalTextKey key = Utils.ConvertEnumData<eGlobalTextKey>(_state);
        labelState.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(key);
        string _point = adjustPoint > 0 ? string.Format("+{0:00}", adjustPoint) : string.Format("{0:00}", adjustPoint);
        spriteFillValue.spriteName = adjustPoint > 0 ? SPRITE_PLUS : SPRITE_MINUS;

        if (adjustPoint == 0)
        {
            labelAdjustPoint.text = adjustPoint.ToString();
        }
        else
        {
            labelAdjustPoint.color = adjustPoint > 0 ? COLOR_PLUS : COLOR_MINUS;
            // Debug.LogError("@@@@@@@@@@@ beforPoint : " + beforPoint + " // adjustPoint : " + adjustPoint);

            labelAdjustPoint.text = adjustPoint.ToString();
            float _adjustValue = (float)(adjustPoint * 0.01f);

            spriteFillValue.fillAmount = (float)(beforPoint * 0.01f);

            if (pintType.Equals("") == true)
            {
                // objManagerPoint.SetActive(false);
                // objBunusPoint.SetActive(false);
            }
            StartCoroutine(IEProgressProcessed(_adjustValue));
        }

        gridBonus.Reposition();
    }

    ///<summary>
    /// _state : 스텟종류,  _objManager: 매너저확률 , objItem : 아이템확률
    ///</summary>
    public void SetResultProgress(string _state, object[] _objManager, object[] objItem)
    {
        gameObject.SetActive(true);

        spriteFillValue.fillAmount = 0.0f;

        int adjustPoint = UserInfoManager.Instance.getSaveState(_state);

        eGlobalTextKey key = Utils.ConvertEnumData<eGlobalTextKey>(_state);
        labelState.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(key);
        // objManagerPoint.SetActive(false);
        // objBunusPoint.SetActive(false);

        float _adjustValue = 0.0f;
        /// 매니저 확률 적용 
        if (_objManager == null)
        {
            //// 아이템 확률 적용 안됨 
            if (objItem != null)
            {
                // objBunusPoint.SetActive(true);
                bool _itemFlag = (bool)objItem[0];
                eState stateRandom = (eState)objItem[1];
                int upPoint = (int)objItem[2];
                adjustPoint += upPoint;

                /// 최종 데이터 저장
                UserInfoManager.Instance.setSaveState(stateRandom.ToString(), adjustPoint);


                string _bonus = string.Format(SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eBonusEffect), upPoint);
                labelBonusPoint.text = _bonus;
            }

            _adjustValue = (float)(adjustPoint * 0.01f);
        }
        else
        {
            //// 매니저 스킬 효과 적용 => 츠텟 올라감
            STManagerData _managerData = JHManagerManger.Instance.GetMyManager();
            eManager_Type _manager = _managerData.manager;

            if (_manager == eManager_Type.MANAGER_F)
            {
                eState crrState = RayUtils.Utils.ConvertEnumData<eState>(_state);

                if ((bool)_objManager[0] == true && crrState == (eState)_objManager[1])
                {
                    // objManagerPoint.SetActive(true);
                    eState mngState = (eState)_objManager[1];
                    int upPoint = (int)_objManager[2];
                    adjustPoint += upPoint;

                    /// 최종 데이터 저장
                    UserInfoManager.Instance.setSaveState(mngState.ToString(), adjustPoint);

                    string managerPoint = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eManagerEffect);
                    labelManagerPoint.text = string.Format(managerPoint, (int)_objManager[2]);

                    // SceneBase.Instance.AddTestTextPopup(string.Format(((eState)_objManager[1]).ToString() + " 현재 스텟 : {0}, 스킬 스텟 : {1}, 적용 스텟 : {2}", UserInfoManager.Instance.getSaveState(((eState)_objManager[1]).ToString()), (int)_objManager[2], adjustPoint));
                }
            }
            //// 매니저 아이템 적용
            if (objItem != null)
            {
                // objBunusPoint.SetActive(true);

                bool _itemFlag = (bool)objItem[0];
                eState stateRandom = (eState)objItem[1];
                int upPoint = (int)objItem[2];
                adjustPoint += upPoint;
                _adjustValue = (float)(adjustPoint * 0.01f);

                /// 최종 데이터 저장
                UserInfoManager.Instance.setSaveState(stateRandom.ToString(), adjustPoint);

                string _bonus = string.Format(SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eBonusEffect), upPoint);
                labelBonusPoint.text = _bonus;
            }

            _adjustValue = (float)(adjustPoint * 0.01f);
        }

        gridBonus.Reposition();

        labelAdjustPoint.text = (Mathf.Max(0, adjustPoint)).ToString();
        StartCoroutine(IEProgressProcessed(Mathf.Max(0.0f, _adjustValue)));

    }


    IEnumerator IEProgressProcessed(float _adjustValue)
    {
        yield return YieldHelper.waitForSeconds(300);

        float plusValue = 0.00f;

        while (true)
        {
            if (plusValue > _adjustValue)
            {
                break;
            }

            yield return new WaitForSeconds(0.001f);
            plusValue += 0.01f;
            spriteFillValue.fillAmount = plusValue;
            // Debug.LogError("#################### plusValue : " + plusValue);

        }
    }

}
