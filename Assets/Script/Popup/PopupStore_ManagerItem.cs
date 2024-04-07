using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System.Linq;
using System.Linq.Expressions;

public class PopupStore_ManagerItem : MonoBehaviour
{

    public StoreItemBoardPrefab prefabStoreBoard;
    public UIScrollView scrollItemManager;
    public UIGrid gridItemManager;
    List<StoreItemBoardPrefab> listItemBoard;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        PopupItemBuy.ACTION_ITEM_REFRASH = SetItemUI; /// 아이템 구매 후 상점 갱신 
        StoreItemPrefab.ACTION_ITEM_REFRASH = ItemRefrash; //// 아이템 On Off 시 UI갱신
        
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        PopupItemBuy.ACTION_ITEM_REFRASH = null;
        StoreItemPrefab.ACTION_ITEM_REFRASH = null;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        SetItemUI();
    }

    public void SetItemUI()
    {
        Dictionary<eManagerItemAbility, Dictionary<eManagerItem, STManagerItemData>> dicData = SceneBase.Instance.dataManager.GetmDicCategoryAllItem();
        //// 최초 실행
        if (listItemBoard == null)
        {
            listItemBoard = new List<StoreItemBoardPrefab>();
            // Debug.LogError("dicData : " + dicData);
            for (int i = 0; i < (int)eManagerItemAbility.COUNT; i++)
            {
                eManagerItemAbility key = dicData.ElementAt(i).Key;
                StoreItemBoardPrefab board = Instantiate(prefabStoreBoard, gridItemManager.transform) as StoreItemBoardPrefab;
                board.transform.localPosition = Vector2.zero;
                board.transform.localScale = Vector3.one;

                Dictionary<eManagerItem, STManagerItemData> _dic = SceneBase.Instance.dataManager.GetmDicCategoryAbilityItem(key);

                // Debug.LogError(_dic);

                board.SetStoreItemBoardUI(_dic, scrollItemManager.panel.depth);
                listItemBoard.Add(board);
            }

        }
        else
        {
            //// 리스트가 있다면 
            for (int i = 0; i < listItemBoard.Count; i++)
            {
                eManagerItemAbility key = dicData.ElementAt(i).Key;
                Dictionary<eManagerItem, STManagerItemData> _dic = SceneBase.Instance.dataManager.GetmDicCategoryAbilityItem(key);
                listItemBoard[i].SetStoreItemBoardUI(_dic, scrollItemManager.panel.depth);
            }
        }


        gridItemManager.Reposition();

    }

    ///<summary>
    /// 상점 아이템 오브젝트 have, equip UI 갱신
    ///</summary>
    void ItemRefrash()
    {
        Dictionary<eManagerItemAbility, Dictionary<eManagerItem, STManagerItemData>> dicData = SceneBase.Instance.dataManager.GetmDicCategoryAllItem();
        //// 리스트가 있다면 
        for (int i = 0; i < listItemBoard.Count; i++)
        {
            eManagerItemAbility key = dicData.ElementAt(i).Key;
            Dictionary<eManagerItem, STManagerItemData> _dic = SceneBase.Instance.dataManager.GetmDicCategoryAbilityItem(key);
            listItemBoard[i].SetStoreItemBoardUI(_dic, scrollItemManager.panel.depth);
        }
    }
}
