using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PopupItemInfo : JHPopup
{

    public UILabel labelITemName;
    public UI2DSprite spriteItemIcon;
    public UILabel labelItemDesc;


    protected override void OnAwake()
    {
        base.OnAwake();
    }

    public override void SetData(object data)
    {
        base.SetData(data);

        if (data != null)
        {
            eManagerItem _item = (eManagerItem)data;
            SetItemUI(_item);
        }
    }


    void SetItemUI(eManagerItem eItem)
    {
        STManagerItemData data = SceneBase.Instance.dataManager.GetManagerItemData(eItem);

        eGlobalTextKey itemNameKey = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(data.eItem.ToString());
        labelITemName.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(itemNameKey);


        eGlobalTextKey itemDesc = RayUtils.Utils.ConvertEnumData<eGlobalTextKey>(data.itemDesc.ToString());
        labelItemDesc.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(itemDesc);

        string _path = string.Format("Image/ManagerItem/{0}", data.itemImage);
        spriteItemIcon.sprite2D = Resources.Load<Sprite>(_path);


    }



    protected override void OnClosed()
    {
        base.OnClosed();
    }


    protected override void OnDestroied()
    {
        base.OnDestroied();
    }


}
