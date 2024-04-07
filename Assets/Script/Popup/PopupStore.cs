using System.Collections.Generic;
using UnityEngine;
using Enums;
using System.Linq;
using System.Linq.Expressions;

enum eBtnStore
{
    NONE = -1,

    btnMoney,
    btnPrimium,
    btnItemBuff,
    btnManagerIten,
    COUNT
}

public class PopupStore : JHPopup
{

    // [Header("장비 토글")]
    // public StoreItemBoardPrefab prefabStoreBoard;
    // public UIScrollView scrollItemManager;
    // public UIGrid gridItemManager;
    // List<StoreItemBoardPrefab> listItemBoard;

    [Header("토글 텍스트")]
    public UILabel labelMoney;
    public UILabel labelPriminum;
    public UILabel labelItemBuff;
    public UILabel labelItemManager;

    public UILabel labelMoneyOff;
    public UILabel labelPriminumOff;
    public UILabel labelItemBuffOff;
    public UILabel labelItemManagerOff;

    [Header("토글")]
    public UIButton[] btnStore;


    [Header("인앱 재화")]
    public PopupStore_Money store_Money;
    [Header("인앱 프리미엄")]
    public PopupStore_Primium store_Primium;
    [Header("다이아 아이템")]
    public PopupStore_ItemBuff store_ItemBuff;
    [Header("매니저 아이템")]
    public PopupStore_ManagerItem store_ManagerItem;


    [Header("각각의 스크롤뷰")]
    public UIScrollView scrollMoney;
    public UIScrollView scrollPriminum;
    public UIScrollView scrollItemBuff;
    public UIScrollView scrollItemManager;


    public GameObject[] btnToggleOn;
    public UIButton[] btnToggleOff;



    protected override void OnAwake()
    {
        base.OnAwake();

        labelMoney.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eStoreToggleMoney);
        labelPriminum.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eStoreTogglePrimium);
        labelItemBuff.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eStoreToggleItemBuff);
        labelItemManager.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eStoreToggleItemManager);

        labelMoneyOff.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eStoreToggleMoney);
        labelPriminumOff.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eStoreTogglePrimium);
        labelItemBuffOff.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eStoreToggleItemBuff);
        labelItemManagerOff.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eStoreToggleItemManager);


        for (int i = 0; i < (int)eBtnStore.COUNT; i++)
        {
            eBtnStore _type = (eBtnStore)i;
            btnToggleOff[i].onClick.Add(new EventDelegate(() => OnClickToggle(_type)));
        }

    }


    public override void SetData()
    {
        base.SetData();

        for (int i = 0; i < btnToggleOff.Length; i++)
        {
            btnToggleOff[i].gameObject.SetActive(((eBtnStore)i == eBtnStore.btnMoney) ? false : true);
            btnToggleOn[i].SetActive(((eBtnStore)i == eBtnStore.btnMoney) ? true : false);
        }

        // scrollMoney.panel.depth = GetDepth() + 1;
        // scrollPriminum.panel.depth = GetDepth() + 1;
        // scrollItemBuff.panel.depth = GetDepth() + 1;
        // scrollItemManager.panel.depth = GetDepth() + 1;

        store_Money.gameObject.SetActive(true);

    }

    protected override void OnStart()
    {
        base.OnStart();

        scrollMoney.panel.depth = GetDepth() + 1;

        // scrollMoney.panel.depth = GetDepth() + 1;
        // scrollPriminum.panel.depth = GetDepth() + 1;
        // scrollItemBuff.panel.depth = GetDepth() + 1;
        // scrollItemManager.panel.depth = GetDepth() + 1;
    }


    protected override void OnClosed()
    {
        base.OnClosed();
    }

    protected override void OnDestroied()
    {
        base.OnDestroied();

        PopupItemBuy.ACTION_ITEM_REFRASH = null;
        StoreItemPrefab.ACTION_ITEM_REFRASH = null;

    }


    void OnClickToggle(eBtnStore _type)
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);
        
        for (int i = 0; i < btnToggleOff.Length; i++)
        {
            btnToggleOff[i].gameObject.SetActive(((eBtnStore)i == _type) ? false : true);
            btnToggleOn[i].SetActive(((eBtnStore)i == _type) ? true : false);
        }

        switch (_type)
        {
            case eBtnStore.btnMoney:
                {
                    store_Money.SetMoneyUI();

                    store_Money.gameObject.SetActive(true);
                    store_Primium.gameObject.SetActive(false);
                    store_ItemBuff.gameObject.SetActive(false);
                    store_ManagerItem.gameObject.SetActive(false);

                    scrollMoney.panel.depth = GetDepth() + 1;

                    break;
                }

            case eBtnStore.btnPrimium:
                {

                    store_Money.gameObject.SetActive(false);
                    store_Primium.gameObject.SetActive(true);
                    store_ItemBuff.gameObject.SetActive(false);
                    store_ManagerItem.gameObject.SetActive(false);

                    store_Primium.SetPrimiumUI();

                    scrollPriminum.panel.depth = GetDepth() + 1;
                    break;
                }

            case eBtnStore.btnItemBuff:
                {

                    store_Money.gameObject.SetActive(false);
                    store_Primium.gameObject.SetActive(false);
                    store_ItemBuff.gameObject.SetActive(true);
                    store_ManagerItem.gameObject.SetActive(false);

                    store_ItemBuff.SetBuffUI();

                    scrollItemBuff.panel.depth = GetDepth() + 1;
                    break;
                }

            case eBtnStore.btnManagerIten:
                {
                    store_Money.gameObject.SetActive(false);
                    store_Primium.gameObject.SetActive(false);
                    store_ItemBuff.gameObject.SetActive(false);
                    store_ManagerItem.gameObject.SetActive(true);

                    store_ManagerItem.SetItemUI();

                    scrollItemManager.panel.depth = GetDepth() + 1;

                    break;
                }
        }
    }


}
