using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PopupEndingAlbum : JHPopup
{
    enum BTN_ALBUM
    {
        NONE = -1,
        ALL,
        STAR,
        NORMAL,
        COUNT
    }


    public UILabel labelAlbumTitle;
    public UILabel labelAlbumAmount;
    public PrefabAlbum _prefabAlbum;


    public UIGrid _gridIndex;
    public UISprite spriteIndex;
    UISprite[] objIndex;

    // public GameObject objEndingAll;
    // public GameObject objStartEnding;
    // public GameObject objNormalEnding;

    public UIGrid gridAlbum;
    List<PrefabAlbum> listAlbumAll = new List<PrefabAlbum>();

    public UIGrid gridStar;
    List<PrefabAlbum> listAlbumStar = new List<PrefabAlbum>();

    public UIGrid gridNormal;
    List<PrefabAlbum> listAlbumNormal = new List<PrefabAlbum>();

    public UIButton[] btnAlbumCategory;
    public GameObject[] btmAlbumCategoryOn;


    public UILabel labelBtnAll_On;
    public UILabel labelBtnStar_On;
    public UILabel labelBtnNormal_On;

    public UILabel labelBtnAll_Off;
    public UILabel labelBtnStar_Off;
    public UILabel labelBtnNormal_Off;


    public UIScrollView _scrollView;

    string INDEX_ON = "popup_icon_point_on_15x15";
    string INDEX_OFF = "popup_icon_point_off_15x15";

    int AllAmount;
    int starAmount;
    int normalAmount;

    BTN_ALBUM _btnIndex;

    protected override void OnAwake()
    {
        base.OnAwake();


        labelBtnAll_On.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eEndingAlbumALL);
        labelBtnStar_On.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eEndingAlbumSTAR);
        labelBtnNormal_On.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eEndingAlbumNORMAL);

        labelBtnAll_Off.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eEndingAlbumALL);
        labelBtnStar_Off.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eEndingAlbumSTAR);
        labelBtnNormal_Off.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eEndingAlbumNORMAL);


        for (int i = 0; i < btnAlbumCategory.Length; i++)
        {
            EventDelegate del = new EventDelegate(this, "OnClickCategory");
            EventDelegate.Parameter parm = new EventDelegate.Parameter();
            parm.value = (BTN_ALBUM)i;
            parm.expectedType = typeof(BTN_ALBUM);
            del.parameters[0] = parm;
            EventDelegate.Add(btnAlbumCategory[i].onClick, del);
        }

        gridAlbum.transform.DestroyChildren();

        int num = (int)Enums.eEndingNumber.COUNT / 6;
        _gridIndex.transform.DestroyChildren();

        objIndex = new UISprite[num];

        for (int i = 0; i < num; i++)
        {
            UISprite _index = Instantiate(spriteIndex, _gridIndex.transform) as UISprite;
            _index.transform.localScale = Vector3.one;
            _index.transform.localPosition = Vector2.zero;
            _index.spriteName = i == 0 ? INDEX_ON : INDEX_OFF;
            _index.gameObject.SetActive(true);
            objIndex[i] = _index;
        }
        _gridIndex.Reposition();
        //// 모든 앨범 
        for (int i = 0; i < (int)Enums.eEndingNumber.COUNT; i++)
        {
            PrefabAlbum _albumAll = Instantiate(_prefabAlbum, gridAlbum.transform);
            _albumAll.transform.localScale = Vector3.one;
            _albumAll.transform.localPosition = Vector2.zero;
            _albumAll.name = ((Enums.eEndingNumber)i).ToString();
            _albumAll.SetEndingAlbumData((Enums.eEndingNumber)i);
            if (UserInfoManager.Instance.GetSaveEnding(((Enums.eEndingNumber)i).ToString()) == true)
            {
                AllAmount++;
            }
            listAlbumAll.Add(_albumAll);
        }
        /// 연예인 앨벌
        for (int i = 0; i < (int)Enums.eEndingNumber.COUNT; i++)
        {
            STEndingData _data = SceneBase.Instance.dataManager.GetSTEndingData((Enums.eEndingNumber)i);

            if (_data.endingType.Equals("STAR") == true)
            {
                PrefabAlbum _albumStar = Instantiate(_prefabAlbum, gridStar.transform);
                _albumStar.transform.localScale = Vector3.one;
                _albumStar.transform.localPosition = Vector2.zero;
                _albumStar.name = ((Enums.eEndingNumber)i).ToString();
                _albumStar.SetEndingAlbumData((Enums.eEndingNumber)i);
                if (UserInfoManager.Instance.GetSaveEnding(((Enums.eEndingNumber)i).ToString()) == true)
                {
                    starAmount++;
                }

                listAlbumStar.Add(_albumStar);
            }

        }
        //// 비연예인 앨범
        for (int i = 0; i < (int)Enums.eEndingNumber.COUNT; i++)
        {
            STEndingData _data = SceneBase.Instance.dataManager.GetSTEndingData((Enums.eEndingNumber)i);

            if (_data.endingType.Equals("NORMAL") == true)
            {
                PrefabAlbum _albumNormal = Instantiate(_prefabAlbum, gridNormal.transform);
                _albumNormal.transform.localScale = Vector3.one;
                _albumNormal.transform.localPosition = Vector2.zero;
                _albumNormal.name = ((Enums.eEndingNumber)i).ToString();
                _albumNormal.SetEndingAlbumData((Enums.eEndingNumber)i);
                if (UserInfoManager.Instance.GetSaveEnding(((Enums.eEndingNumber)i).ToString()) == true)
                {
                    normalAmount++;
                }

                listAlbumNormal.Add(_albumNormal);
            }
        }

        gridAlbum.gameObject.SetActive(true);
        gridStar.gameObject.SetActive(false);
        gridNormal.gameObject.SetActive(false);

        SetButton(BTN_ALBUM.ALL);

        gridAlbum.Reposition();
        labelAlbumAmount.text = string.Format("{0} / {1}", AllAmount, (int)Enums.eEndingNumber.COUNT);
    }

    protected override void OnStart()
    {
        base.OnStart();

        Debug.LogError("################ GetDepth() : " + GetDepth());
        _scrollView.GetComponent<UIPanel>().depth = GetDepth() + 1;
    }
    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        if (_btnIndex == BTN_ALBUM.ALL)
        {
            //// 스크롤 인데스 카운팅
            float clipY = Mathf.Abs(_scrollView.transform.GetComponent<UIPanel>().clipOffset.y);
            int pageNum = (int)(clipY / 748);
            // Debug.LogError("################ FixedUpdate pageNum : " + pageNum);
            for (int i = 0; i < objIndex.Length; i++)
            {
                objIndex[i].spriteName = pageNum == i ? INDEX_ON : INDEX_OFF;
                objIndex[i].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < objIndex.Length; i++)
            {
                objIndex[i].gameObject.SetActive(false);
            }
        }

    }

    protected override void OnDestroied()
    {
        base.OnDestroied();
    }

    protected override void OnClosed()
    {
        base.OnClosed();
    }

    void SetButton(BTN_ALBUM _index)
    {
        for (int i = 0; i < (int)BTN_ALBUM.COUNT; i++)
        {
            if (i == (int)_index)
            {
                btnAlbumCategory[i].gameObject.SetActive(false);
                btmAlbumCategoryOn[i].gameObject.SetActive(true);
            }
            else
            {
                btnAlbumCategory[i].gameObject.SetActive(true);
                btmAlbumCategoryOn[i].gameObject.SetActive(false);
            }
        }
    }


    void OnClickCategory(BTN_ALBUM _index)
    {
        SetButton(_index);

        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        switch (_index)
        {
            case BTN_ALBUM.ALL:
                {
                    // btnAlbumCategory[0].gameObject.SetActive(false);
                    // btmAlbumCategoryOn[0].gameObject.SetActive(true);

                    // btnAlbumCategory[1].gameObject.SetActive(true);
                    // btmAlbumCategoryOn[1].gameObject.SetActive(false);

                    // btnAlbumCategory[2].gameObject.SetActive(true);
                    // btmAlbumCategoryOn[2].gameObject.SetActive(false);


                    gridAlbum.gameObject.SetActive(true);
                    gridStar.gameObject.SetActive(false);
                    gridNormal.gameObject.SetActive(false);
                    labelAlbumAmount.text = string.Format("{0} / {1}", AllAmount, (int)Enums.eEndingNumber.COUNT);
                    gridAlbum.Reposition();
                    break;
                }
            case BTN_ALBUM.STAR:
                {
                    // btnAlbumCategory[0].gameObject.SetActive(true);
                    // btmAlbumCategoryOn[0].gameObject.SetActive(false);

                    // btnAlbumCategory[1].gameObject.SetActive(false);
                    // btmAlbumCategoryOn[1].gameObject.SetActive(true);

                    // btnAlbumCategory[2].gameObject.SetActive(true);
                    // btmAlbumCategoryOn[2].gameObject.SetActive(false);

                    gridStar.gameObject.SetActive(true);
                    gridAlbum.gameObject.SetActive(false);
                    gridNormal.gameObject.SetActive(false);
                    labelAlbumAmount.text = string.Format("{0} / {1}", starAmount, listAlbumStar.Count);
                    gridStar.Reposition();
                    break;
                }
            case BTN_ALBUM.NORMAL:
                {
                    // btnAlbumCategory[0].gameObject.SetActive(true);
                    // btmAlbumCategoryOn[0].gameObject.SetActive(false);

                    // btnAlbumCategory[1].gameObject.SetActive(true);
                    // btmAlbumCategoryOn[1].gameObject.SetActive(false);

                    // btnAlbumCategory[2].gameObject.SetActive(false);
                    // btmAlbumCategoryOn[2].gameObject.SetActive(true);

                    gridNormal.gameObject.SetActive(true);
                    gridAlbum.gameObject.SetActive(false);
                    gridStar.gameObject.SetActive(false);
                    labelAlbumAmount.text = string.Format("{0} / {1}", normalAmount, listAlbumNormal.Count);
                    gridNormal.Reposition();
                    break;
                }
        }

        _scrollView.ResetPosition();
    }

}
