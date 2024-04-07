using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PrefabItemCard : MonoBehaviour
{

    public Animation cardAnimation;
    public GameObject objFirstCard;
    public UI2DSprite spriteFirstCardImg;
    public GameObject objLastCard;
    public UI2DSprite spriteResultImage;
    // public UILabel labelState;
    public UILabel labelResult;

    public GameObject objEffect;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        spriteResultImage.gameObject.SetActive(false);
    }


    public void SetStartCarAnimation()
    {
        // cardAnimation.enabled = true;
        // cardAnimation.Play();

    }



    public void SetSartAnimation(STDreamEventData _data)
    {
        cardAnimation.enabled = true;
        cardAnimation.Play();

        // Debug.LogError("cardAnimation.Play : " + cardAnimation.IsPlaying("ItemCardRotationAnim"));

        //// 카드 이미지 설정

        //// 카드 테스트 설정 
        if (_data != null)
        {
            SetSelectItemCardData(_data);
        }

    }
    /// eD_AWARENESS// _eventData.key_skillType : eAWARENESS
    public void SetFirstItemDataUICard(STDreamEventData _eventData)
    {
        spriteFirstCardImg.enabled = false;
        string _image = _eventData.image;

        Enums.eGlobalTextKey _key = RayUtils.Utils.ConvertEnumData<Enums.eGlobalTextKey>(_eventData.key_dreamEvent.ToString());
        string msg = "";
        int _value = int.Parse(_eventData.probValue.ToString());
        if (_eventData.key_dreamEvent == Enums.eDremEvent.eD_STATE || _eventData.key_dreamEvent == Enums.eDremEvent.eD_AWARENESS)
        {
            // eStateTitle     eAwarenessTitle
            Enums.eGlobalTextKey _stateTitle = Enums.eGlobalTextKey.eStateTitle;
            Enums.eGlobalTextKey _awsTitle = Enums.eGlobalTextKey.eAwarenessTitle;
            // labelState.text = _eventData.key_dreamEvent == Enums.eDremEvent.eD_STATE ? SceneBase.Instance.dataManager.GetDicGlobalTextData(_stateTitle) : SceneBase.Instance.dataManager.GetDicGlobalTextData(_awsTitle);

            Enums.eGlobalTextKey _skill = RayUtils.Utils.ConvertEnumData<Enums.eGlobalTextKey>(_eventData.key_skillType.ToString());
            Debug.LogError("############## _skill : " + _skill);
            switch (_eventData.key_dreamEvent)
            {
                case Enums.eDremEvent.eD_STATE:
                    {
                        objLastCard.GetComponent<UI2DSprite>().sprite2D = Resources.Load<Sprite>(string.Format("Image/Event/DreamEvent/CardUI/CARD_{0}", _skill.ToString()));
                        break;
                    }
                case Enums.eDremEvent.eD_AWARENESS:
                    {
                        objLastCard.GetComponent<UI2DSprite>().sprite2D = Resources.Load<Sprite>("Image/Event/DreamEvent/CardUI/CARD_AWARENESS");
                        break;
                    }
            }

            string strSkill = SceneBase.Instance.dataManager.GetDicGlobalTextData(_skill);
            msg = string.Format(SceneBase.Instance.dataManager.GetDicGlobalTextData(_key), strSkill, _value);

        }
        else
        {
            if (_eventData.key_dreamEvent == Enums.eDremEvent.eD_DIA)
            {
                objLastCard.GetComponent<UI2DSprite>().sprite2D = Resources.Load<Sprite>("Image/Event/DreamEvent/CardUI/CARD_DIA");
            }

            if (_eventData.key_dreamEvent == Enums.eDremEvent.eD_GOLD)
            {
                objLastCard.GetComponent<UI2DSprite>().sprite2D = Resources.Load<Sprite>("Image/Event/DreamEvent/CardUI/CARD_GOLD");
            }

            if (_eventData.key_dreamEvent == Enums.eDremEvent.eD_HP)
            {
                objLastCard.GetComponent<UI2DSprite>().sprite2D = Resources.Load<Sprite>("Image/Event/DreamEvent/CardUI/CARD_HART");
            }


            // spriteResultImage.gameObject.SetActive(true);
            msg = string.Format(SceneBase.Instance.dataManager.GetDicGlobalTextData(_key), _value.ToString());
        }
        Debug.LogError("############# msg : " + msg + " // _value : " + _value);
        labelResult.text = msg;
        objLastCard.SetActive(true);
    }

    ///<summary>
    /// 아이템 카드 선택 후 UI 변경
    ///</summary>
    void SetSelectItemCardData(STDreamEventData _eventData)
    {
        spriteFirstCardImg.enabled = true;
        objLastCard.SetActive(false);

        Enums.eGlobalTextKey _key = RayUtils.Utils.ConvertEnumData<Enums.eGlobalTextKey>(_eventData.key_dreamEvent.ToString());
        string msg = "";
        int _value = int.Parse(_eventData.probValue.ToString());
        if (_eventData.key_dreamEvent == Enums.eDremEvent.eD_STATE || _eventData.key_dreamEvent == Enums.eDremEvent.eD_AWARENESS)
        {
            // eStateTitle     eAwarenessTitle
            Enums.eGlobalTextKey _stateTitle = Enums.eGlobalTextKey.eStateTitle;
            Enums.eGlobalTextKey _awsTitle = Enums.eGlobalTextKey.eAwarenessTitle;
            // labelState.text = _eventData.key_dreamEvent == Enums.eDremEvent.eD_STATE ? SceneBase.Instance.dataManager.GetDicGlobalTextData(_stateTitle) : SceneBase.Instance.dataManager.GetDicGlobalTextData(_awsTitle);

            Enums.eGlobalTextKey _skill = RayUtils.Utils.ConvertEnumData<Enums.eGlobalTextKey>(_eventData.key_skillType.ToString());

            switch (_eventData.key_dreamEvent)
            {
                case Enums.eDremEvent.eD_STATE:
                    {
                        objLastCard.GetComponent<UI2DSprite>().sprite2D = Resources.Load<Sprite>(string.Format("Image/Event/DreamEvent/CardUI/CARD_{0}", _skill.ToString()));
                        break;
                    }
                case Enums.eDremEvent.eD_AWARENESS:
                    {
                        objLastCard.GetComponent<UI2DSprite>().sprite2D = Resources.Load<Sprite>("Image/Event/DreamEvent/CardUI/CARD_AWARENESS");
                        break;
                    }
            }

            string strSkill = SceneBase.Instance.dataManager.GetDicGlobalTextData(_skill);
            msg = string.Format(SceneBase.Instance.dataManager.GetDicGlobalTextData(_key), strSkill, _value);

            // spriteResultImage.gameObject.SetActive(false);
            // labelState.gameObject.SetActive(true);
        }
        else
        {
            if (_eventData.key_dreamEvent == Enums.eDremEvent.eD_DIA)
            {
                objLastCard.GetComponent<UI2DSprite>().sprite2D = Resources.Load<Sprite>("Image/Event/DreamEvent/CardUI/CARD_DIA");
            }

            if (_eventData.key_dreamEvent == Enums.eDremEvent.eD_GOLD)
            {
                objLastCard.GetComponent<UI2DSprite>().sprite2D = Resources.Load<Sprite>("Image/Event/DreamEvent/CardUI/CARD_GOLD");
            }

            if (_eventData.key_dreamEvent == Enums.eDremEvent.eD_HP)
            {
                objLastCard.GetComponent<UI2DSprite>().sprite2D = Resources.Load<Sprite>("Image/Event/DreamEvent/CardUI/CARD_HART");
            }


            // spriteResultImage.gameObject.SetActive(true);
            msg = string.Format(SceneBase.Instance.dataManager.GetDicGlobalTextData(_key), _value.ToString());
        }
        // Debug.LogError("############# msg : " + msg + " // _value : " + _value);
        labelResult.text = msg;
    }

    ///<summary>
    /// 아이템 카드 애니메이션 종료 후 UI Set
    ///</summary>
    public void SetItemCardUI()
    {
        spriteFirstCardImg.enabled = true;
        objLastCard.SetActive(false);
        objEffect.SetActive(true);
    }

    public void SetEndItemCardUI()
    {
        spriteFirstCardImg.enabled = false;
        objLastCard.SetActive(true);
        objEffect.SetActive(true);
    }


    ///<summary>
    /// 아이템 카드 리셋
    ///</summary>
    public void ResetCardAnimation()
    {
        cardAnimation.enabled = false;
        objLastCard.SetActive(false);
        objEffect.SetActive(false);

        spriteResultImage.sprite2D = null;
        labelResult.text = string.Empty;
    }

}
