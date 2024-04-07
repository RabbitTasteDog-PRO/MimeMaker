using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupLogAll : JHPopup
{

    public UIScrollView scrollView;
    public UITable tableLog;


    protected override void OnAwake()
    {
        base.OnAwake();

        if(tableLog.transform.childCount > 0)
        {
            tableLog.transform.DestroyChildren();
        }
    }

    protected override void OnClosed()
    {
        base.OnClosed();
    }

    public override void SetData(object _data)
    {
        base.SetData();

        
        if (_data != null)
        {
            List<GameObject> _list = (List<GameObject>)_data;
            SetLogData(_list);
        }


    }

    protected override void OnStart()
    {
        base.OnStart();

        scrollView.panel.depth = GetDepth() + 1;
    }



    void SetLogData(List<GameObject> objLog)
    {

        for (int i = 0; i < objLog.Count; i++)
        {
            GameObject obj = Instantiate(objLog[i], tableLog.transform) as GameObject;
            obj.transform.localPosition = Vector2.zero;
            obj.transform.localScale = Vector3.one;
            if (obj.name.Contains("PrefabLogLine(Clone)"))
            {
                obj.GetComponent<UIDragScrollView>().scrollView = scrollView;
            }
            else
            {
                obj.transform.Find("labelLog").GetComponent<UIDragScrollView>().scrollView = scrollView;
            }
        }

        scrollView.ResetPosition();
        tableLog.repositionNow = true;
        tableLog.Reposition();
    }
}
