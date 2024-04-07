using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Enums;

public class PopupEndingHPEmpty : JHPopup
{
    public static Action<bool> ACTION_HP_END;

    public GameObject objTitle;
    public UILabel labelEndTitle;

    public UI2DSprite spriteMin;


    public UILabel labelEndDesc;

   
    protected override void OnAwake()
    {
        base.OnAwake();

        labelEndTitle.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eBtnEndingHPEmpty);

        //모든 체력이 소진되어\n더 이상 일정을 진행할 수 없습니다.\n{0}초 후 엔딩으로 이동합니다.	NONE	NONE
        string endFormat = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eEndingHPEmpty);
        labelEndDesc.text = string.Format(endFormat, 5);

        UserInfoManager.Instance.SetSaveEnding(eEndingNumber.NO_1.ToString(), true);

    }

    protected override void OnStart()
    {
        base.OnStart();


        StartCoroutine(NextProcessed());

    }

    protected override void OnDestroied()
    {
        base.OnDestroied();

        if (ACTION_HP_END != null && ACTION_HP_END.GetInvocationList().Length > 0)
        {
            ACTION_HP_END(isActionEnd);
        }
    }

    IEnumerator NextProcessed()
    {
        yield return new WaitForSeconds(.5f);
        objTitle.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        spriteMin.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        labelEndDesc.gameObject.SetActive(true);
        // yield return new WaitForSeconds(1.5f);
        StartCoroutine(IEEndCountDown());
    }

    


    bool isActionEnd = false;
    IEnumerator IEEndCountDown()
    {
        int count = 5;
        while (true)
        {
            if (count <= 0)
            {
                isActionEnd = true;
                break;
            }

            count -= 1;

            string endFormat = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eEndingHPEmpty);
            labelEndDesc.text = string.Format(endFormat, count);
            yield return new WaitForSeconds(1.0f);

        }
        Destroy(this.gameObject);

    }



}
