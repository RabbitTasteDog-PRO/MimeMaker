using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupStateUp : JHPopup
{

    public UILabel labelStateRandomDesc;
    public UILabel labelStateRandomPoint;

    public UIButton btnConfirm;
    public UILabel labelBtnConfirm;



    protected override void OnAwake()
    {
        base.OnAwake();

        labelStateRandomPoint.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eConfirm);
        btnConfirm.onClick.Add(new EventDelegate(OnClosed));

        UserInfoManager.Instance.SetStateBuyVideo(true);

    }

    protected override void OnStart()
    {
        base.OnStart();
        Enums.eState _randomState = (Enums.eState)UnityEngine.Random.Range((int)Enums.eState.eFACE, (int)Enums.eState.eCHARACTER);
        int upPoint = Random.Range(1, 10);
        int crrState = UserInfoManager.Instance.getSaveState(_randomState.ToString());

        labelStateRandomDesc.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eRandomStateUpDesc);

        Enums.eGlobalTextKey _eState = RayUtils.Utils.ConvertEnumData<Enums.eGlobalTextKey>(_randomState.ToString());
        string _strState = SceneBase.Instance.dataManager.GetDicGlobalTextData(_eState);
        string _randomPoint = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eRandomStateUpPoint);

        labelStateRandomPoint.text = string.Format(_randomPoint, _strState, upPoint);
        UserInfoManager.Instance.setSaveState(_randomState.ToString(), (upPoint + crrState));
    }





    protected override void OnClosed()
    {
        base.OnClosed();
    }



}
