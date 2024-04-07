using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupTestSkillApply : JHPopup
{

    public UILabel labelApply;

    protected override void OnAwake()
    {
        base.OnAwake();

        isBackButton = false;
    }

    public override void SetData(object data)
    {
        base.SetData();
        
        if (data != null)
        {
            string _text = (string)data;
            labelApply.text = _text;
        }

        StartCoroutine(IEDestroy());
    }


    protected override void OnStart()
    {
        base.OnStart();
        this.gameObject.GetComponent<UIPanel>().depth = GetDepth() + 10000;
        // PopupObject.GetComponent<UIPanel>().depth = 10000;//(panel.depth + 10);
    }




    IEnumerator IEDestroy()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(this.gameObject);
    }
}
