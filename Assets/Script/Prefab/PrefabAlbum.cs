using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PrefabAlbum : MonoBehaviour
{

    eEndingNumber _eEndingNum;


    public UIButton btnAlbum;
    public GameObject objLabel;
    public UILabel labelEndingNum;
    public UILabel labelEndingTitle;

    ///<summary>
    /// 엔딩 데이터 및 UI set
    ///</summary>
    public void SetEndingAlbumData(eEndingNumber _num)
    {
        _eEndingNum = _num;

        labelEndingNum.text = string.Format("No.{0}", (int)(_num + 1));
        if (UserInfoManager.Instance.GetSaveEnding(_eEndingNum.ToString()) == false)
        {
            if (btnAlbum.onClick != null)
            {
                btnAlbum.onClick.Clear();
            }

            objLabel.SetActive(false);
            btnAlbum.GetComponent<UI2DSprite>().sprite2D = Resources.Load<Sprite>("Image/Ending/Album/NO_NONE");
            btnAlbum.normalSprite2D = Resources.Load<Sprite>("Image/Ending/Album/NO_NONE");
        }
        else
        {
            btnAlbum.GetComponent<UI2DSprite>().sprite2D = Resources.Load<Sprite>(string.Format("Image/Ending/Album/NO_{0}", ((int)_num)));
            btnAlbum.normalSprite2D = Resources.Load<Sprite>(string.Format("Image/Ending/Album/NO_{0}", ((int)_num)));

            EventDelegate _delegate = new EventDelegate(this, "OnClickAlbum");
            EventDelegate.Parameter parm = new EventDelegate.Parameter();
            parm.value = _num;
            parm.expectedType = typeof(eEndingNumber);
            _delegate.parameters[0] = parm;
            EventDelegate.Add(btnAlbum.onClick, _delegate);

            List<xmlEnding> _listED = XmlParser.Read<xmlEnding>("XML/Ending/xmlEndingData");

            /// 텍스트 데이터 입력
            // eGlobalTextKey key = SceneBase.Instance.dataManager.GetDicGlobalTextData(); 
            labelEndingTitle.text = _listED[(int)_num].ending_Title;

            objLabel.SetActive(true);
        }

    }


    bool isAction = false;
    ///<summary>
    /// 엔딩 프리팹 온글릭 메소드 
    ///</summary>
    void OnClickAlbum(eEndingNumber number)
    {

        if (UserInfoManager.Instance.GetSaveEnding(_eEndingNum.ToString()) == false)
        {
            return;
        }

        if (isAction == false)
        {
            isAction = true;

            object[] _data = new object[3];
            /// 엔딩넘버
            _data[0] = number;
            //// 백버튼
            _data[1] = true;
            //// 상점에서 구매여부
            _data[2] = false;

            Transform _trans = PopupManager.Instance.RootPopup;
            SceneBase.Instance.AddPopup(_trans, ePopupLayer.PopupEnding, _data);

            Invoke("InvokeIsAction", 0.5f);
        }

    }

    void InvokeIsAction()
    {
        isAction = true;
    }
}
