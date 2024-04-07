using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeInOut : MonoBehaviour
{
    public GameObject mFadeInOut;
    protected bool ACTION_PROCESS = false;


    public IEnumerator IEFadeIn()
    {
        ACTION_PROCESS = true;
        PlayTweenAlpha(mFadeInOut, 0f, 0, 1, 1f);

        yield return new WaitForSeconds(1.1f);
        mFadeInOut.SetActive(false);
        ACTION_PROCESS = false;

    }

    public IEnumerator IEFadeOut()
    {
        ACTION_PROCESS = true;
        PlayTweenAlpha(mFadeInOut, 1f, 1, 0, 1f);

        yield return new WaitForSeconds(1.1f);
        mFadeInOut.SetActive(false);
        ACTION_PROCESS = false;
    }

    void PlayTweenAlpha(GameObject obj, float start, int from, int to, float duration)
    {
        TweenAlpha tween = TweenAlpha.Begin(obj, duration, start);
        tween.ResetToBeginning();
        tween.enabled = true;
        tween.from = from;
        tween.to = to;

        obj.SetActive(true);
        // tween.PlayForward();
    }

    ///<summery>
    /// 씬 이동 매니저 
    ///</summery>
    public IEnumerator IEFadeInOut()
    {
        mFadeInOut.GetComponent<UI2DSprite>().color = new Color32(0, 0, 0, 255);
        mFadeInOut.SetActive(true);
        AnimationUtil.PopupAlphaOut(mFadeInOut, null, 0.4f);
        yield return new WaitForSeconds(0.4f);
        mFadeInOut.SetActive(false);
        yield return new WaitForEndOfFrame();
        ACTION_PROCESS = false;
    }

    public bool GetActionFadeInOut()
    {
        return ACTION_PROCESS;
    }


}
