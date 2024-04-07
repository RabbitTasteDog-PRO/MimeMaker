using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PopupBuyEnding : JHPopup
{

    public static System.Action<object[]> ACTION_VIEW_ENDING;
    public UILabel labelBuyEndingText;


    Enums.eEndingNumber _endingNum;
    bool isBuyEndingCheck = false;
    bool isback = false;

    protected override void OnAwake()
    {
        base.OnAwake();

        isBackButton = false;
        labelBuyEndingText.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.ePopupBuyEndingText);
    }


    protected override void OnDestroied()
    {
        base.OnDestroied();

        if (ACTION_VIEW_ENDING != null && ACTION_VIEW_ENDING.GetInvocationList().Length > 0)
        {
            object[] _data = new object[3];
            _data[0] = _endingNum;
            _data[1] = isback;
            _data[2] = isBuyEndingCheck;
            ACTION_VIEW_ENDING(_data);
        }
    }

    public override void SetData(object[] _data)
    {
        base.SetData();
        if (_data != null)
        {
            _endingNum = (Enums.eEndingNumber)_data[0];
            isback = (bool)_data[1];
            isBuyEndingCheck = (bool)_data[2];
        }

        
    }

    protected override void OnStart()
    {
        base.OnStart();

        StartCoroutine(IEDestroty());
    }



    IEnumerator IEDestroty()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(this.gameObject);
    }
}
