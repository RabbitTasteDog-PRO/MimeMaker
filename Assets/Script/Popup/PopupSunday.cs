using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSunday : JHPopup
{

    public UILabel labelSundayAct;
    public UI2DSprite spriteSundayBG;
    public UI2DSprite spriteMim;

    public UILabel labelSundayTip;

    public UISlider progress;


    protected override void OnAwake()
    {
        base.OnAwake();

        progress.value = 0;
    }

    protected override void OnDestroied()
    {
        base.OnDestroied();
    }

    protected override void OnStart()
    {
        base.OnStart();

        StartCoroutine(IEDestory());
    }


    public override void SetData()
    {

    }


    IEnumerator IEDestory()
    {
        while (true)
        {
            progress.value += 0.01f;

            if (progress.value >= 1)
            {
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(2.0f);
        Destroy(this.gameObject);

    }

}
