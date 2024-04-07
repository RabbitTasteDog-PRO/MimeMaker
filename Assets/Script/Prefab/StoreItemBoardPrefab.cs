using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Linq.Expressions;
using Enums;

public class StoreItemBoardPrefab : MonoBehaviour
{

    [Header("아이템")]
    public StoreItemPrefab prefabStoreItem;
    List<StoreItemPrefab> listStoreItem;

    [Header("아이템 담을 스크롤")]
    public UIScrollView scrollItemBoard;
    public UIGrid gridItemScroll;
    [Header("아이템 종류")]
    public UILabel labelItemKind;


    Dictionary<eManagerItem, STManagerItemData> dicItem;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        // StoreItemPrefab.ACTION_ITEM_REFRASH = null;
    }

    ///<summary>
    /// 아이템 UI 세팅
    ///</summary>
    public void SetStoreItemBoardUI(Dictionary<eManagerItem, STManagerItemData> dic, int panelDepth)
    {
        scrollItemBoard.panel.depth = panelDepth + 2;
        if (listStoreItem == null)
        {
            listStoreItem = new List<StoreItemPrefab>();
            for (int i = 0; i < dic.Count; i++)
            {
                eManagerItem key = dic.ElementAt(i).Key;
                STManagerItemData data = dic[key];

                eGlobalTextKey textKey = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(string.Format("eStore_{0}", data.eAbility.ToString()));
                labelItemKind.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(textKey);

                StoreItemPrefab prefab = Instantiate(prefabStoreItem, gridItemScroll.transform) as StoreItemPrefab;
                prefab.transform.localPosition = Vector2.zero;
                prefab.transform.localScale = Vector3.one;
                prefab.SetItemUI(data);
                listStoreItem.Add(prefab);
            }
        }
        else
        {
            for (int i = 0; i < listStoreItem.Count; i++)
            {
                eManagerItem key = dic.ElementAt(i).Key;
                STManagerItemData data = dic[key];
                listStoreItem[i].SetItemUI(data);
            }

        }
        dicItem = dic;
        gridItemScroll.Reposition();
    }

    


}
