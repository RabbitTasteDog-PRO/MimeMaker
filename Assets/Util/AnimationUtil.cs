using UnityEngine;
using System.Collections;

public class AnimationUtil : MonoBehaviour
{

    static AnimationCurve animationCurveIn_Menu = new AnimationCurve(
    new Keyframe(0f, 0f),
    // new Keyframe(0.7f, 0.7f),
    //new Keyframe(0.7f, 2.2f),
    // new Keyframe(0.7f, 1.2f),
    // new Keyframe(0.7f, 1.0f),
    new Keyframe(1f, 1f));

    static AnimationCurve animationCurveIn = new AnimationCurve(
        new Keyframe(0f, 0f),
        // new Keyframe(0.7f, 1.0f),
        // new Keyframe(0.7f, 1.2f),
        //new Keyframe(0.7f, 2.2f),
        new Keyframe(1f, 1f));

    static AnimationCurve animationCurveOut = new AnimationCurve(
        new Keyframe(0f, 0),
        // new Keyframe(0.3f, -0.2f),
        new Keyframe(1f, 1f));

    static AnimationCurve animationCurveIn_Hard = new AnimationCurve(
        new Keyframe(0f, 0f),
        new Keyframe(0.7f, 1.2f),
        new Keyframe(1f, 1f));

    static AnimationCurve animationCurveOut_Hard = new AnimationCurve(
        new Keyframe(0f, 0),
        new Keyframe(0.3f, -0.2f),
        new Keyframe(1f, 1f));

    static AnimationCurve animationCurveIn_Mid = new AnimationCurve(
    new Keyframe(0f, 0f),
    new Keyframe(0.7f, 1.5f),
    new Keyframe(1f, 1f));

    static AnimationCurve animationCurveOut_Mid = new AnimationCurve(
        new Keyframe(0f, 0),
        new Keyframe(0.3f, -0.2f),
        new Keyframe(1f, 1f));

    static AnimationCurve animationFadeInOut = new AnimationCurve(
        new Keyframe(0.0f, 0.0f, 1.0f, 1.0f),
        new Keyframe(0.5f, 1.0f, 1.0f, 1.0f),
        new Keyframe(1.0f, 0.0f, 1.0f, 1.0f));

    static public void FadeInOutLoop(GameObject gameObject)
    {
        TweenAlpha tween = TweenAlpha.Begin(gameObject, 1.0f, 1.0f);
        tween.from = 0.0f;
        tween.animationCurve = animationFadeInOut;
        tween.style = UITweener.Style.Loop;

    }

    static public void FadeIn(GameObject gameObject, float interval, EventDelegate.Callback callback)
    {
        TweenAlpha tween = TweenAlpha.Begin(gameObject, interval, 1.0f);
        tween.from = 0.0f;
        tween.SetOnFinished(callback);
    }

    static public void FadeIn(GameObject gameObject, EventDelegate.Callback callback, float time = 1.0f)
    {
        TweenAlpha tween = TweenAlpha.Begin(gameObject, time, 1.0f);
        tween.from = 0.0f;
        tween.SetOnFinished(callback);
    }

    static public void FadeOut(GameObject gameObject, EventDelegate.Callback callback)
    {
        TweenAlpha tween = TweenAlpha.Begin(gameObject, 1.0f, 0f);
        tween.from = 1.0f;
        tween.SetOnFinished(callback);
    }

    static public void PopupScaleIn(GameObject gameObject, EventDelegate.Callback callback, float duration = 0.5f)
    {
        TweenScale tween = TweenScale.Begin(gameObject, duration, new Vector3(1.0f, 1.0f));
        tween.from = new Vector3(0.7f, 0.7f);
        tween.animationCurve = animationCurveIn;
        tween.SetOnFinished(callback);
    }


    static public void PopupScaleOut(GameObject gameObject, EventDelegate.Callback callback)
    {
        TweenScale tween = TweenScale.Begin(gameObject, 0.5f, new Vector3(0.5f, 0.5f));
        tween.from = new Vector3(1.0f, 1.0f);
        tween.animationCurve = animationCurveOut;
        tween.SetOnFinished(callback);
    }

    static public void ScaleIn_Hard(GameObject gameObject, EventDelegate.Callback callback, float duration = 0.5f)
    {
        TweenScale tween = TweenScale.Begin(gameObject, duration, new Vector3(1.0f, 1.0f));
        tween.from = new Vector3(0.7f, 0.7f);
        tween.animationCurve = animationCurveIn_Hard;
        tween.SetOnFinished(callback);
    }

        static public void ScaleIn(GameObject gameObject, EventDelegate.Callback callback, float _from , float _to, float duration = 0.5f)
    {
        TweenScale tween = TweenScale.Begin(gameObject, duration, new Vector3(1.0f, 1.0f));
        tween.from = new Vector3(_from, _from);
        tween.to = new Vector3(_to, _to);
        
        tween.animationCurve = animationCurveIn_Hard;
        tween.SetOnFinished(callback);
    }


    static public void ScaleOut_Hard(GameObject gameObject, EventDelegate.Callback callback, float duration = 0.5f)
    {
        TweenScale tween = TweenScale.Begin(gameObject, duration, new Vector3(0.5f, 0.5f));
        tween.from = new Vector3(1.0f, 1.0f);
        tween.animationCurve = animationCurveOut_Hard;
        tween.SetOnFinished(callback);
    }

    static public void ScaleIn_Mid(GameObject gameObject, EventDelegate.Callback callback, float duration = 0.5f)
    {
        TweenScale tween = TweenScale.Begin(gameObject, duration, new Vector3(1.0f, 1.0f));
        tween.from = new Vector3(0.7f, 0.7f);
        tween.animationCurve = animationCurveIn_Hard;
        tween.SetOnFinished(callback);
    }


    static public void ScaleOut_Mid(GameObject gameObject, EventDelegate.Callback callback)
    {
        TweenScale tween = TweenScale.Begin(gameObject, 0.5f, new Vector3(0.5f, 0.5f));
        tween.from = new Vector3(1.0f, 1.0f);
        tween.animationCurve = animationCurveOut_Hard;
        tween.SetOnFinished(callback);
    }

    static public void PopupAlphaIn(GameObject gameObject, EventDelegate.Callback callback, float time)
    {
        gameObject.SetActive(true);
        TweenAlpha tween = TweenAlpha.Begin(gameObject, time, 1.0f);
        tween.from = 0.0f;
        tween.to = 1.0f;
        //tween.animationCurve = animationCurveIn;
        tween.SetOnFinished(callback);
    }

    static public void PopupAlphaOut(GameObject gameObject, EventDelegate.Callback callback, float time)
    {
        gameObject.SetActive(true);
        TweenAlpha tween = TweenAlpha.Begin(gameObject, time, 0.0f);
        tween.from = 1.0f;
        tween.to = 0.0f;
        //tween.animationCurve = animationCurveOut;
        tween.SetOnFinished(callback);
    }

    public static void TweenPositionAmation(GameObject gameObject, EventDelegate.Callback callback, float crr_x, float crr_y, float mov_x, float mov_y, float time, string type)
    {

        if (gameObject.GetComponent<TweenPosition>() == null)
        {
            gameObject.AddComponent<TweenPosition>();
        }
        TweenPosition tween = TweenPosition.Begin(gameObject, time, new Vector3(0, 0, 0));
        tween.from = new Vector3(crr_x, crr_y, 0);
        tween.to = new Vector3(mov_x, mov_y, 0);

        if (type.Equals(Strings.CURVE_OUT))
        {
            tween.animationCurve = animationCurveOut;
        }
        else if (type.Equals(Strings.CURVE_IN))
        {
            //// 17.01.26
            //// 모니그이에서만 적용 중 
            //// 메뉴바 내려올때 커브 속도 다르게 하려고 
            if (gameObject.name.Equals("Menu"))
            {
                tween.animationCurve = animationCurveIn_Menu;
            }
            else
            {
                tween.animationCurve = animationCurveIn;
            }
        }
        else
        {
            tween.PlayForward();
        }

        tween.SetOnFinished(callback);
    }

    public static void TweenPositionAmation(GameObject gameObject, EventDelegate.Callback callback, float crr_x, float crr_y, float mov_x, float mov_y, float time)
    {

        if (gameObject.GetComponent<TweenPosition>() == null)
        {
            gameObject.AddComponent<TweenPosition>();
        }
        TweenPosition tween = TweenPosition.Begin(gameObject, time, new Vector3(0, 0, 0));

        tween.animationCurve = animationCurveIn;

        tween.from = new Vector3(crr_x, crr_y, 0);
        tween.to = new Vector3(mov_x, mov_y, 0);

        tween.SetOnFinished(callback);
    }

     public static void TweenPositionAmation_Hard(GameObject gameObject, EventDelegate.Callback callback, float crr_x, float crr_y, float mov_x, float mov_y, float time)
    {

        if (gameObject.GetComponent<TweenPosition>() == null)
        {
            gameObject.AddComponent<TweenPosition>();
        }
        TweenPosition tween = TweenPosition.Begin(gameObject, time, new Vector3(0, 0, 0));

        tween.animationCurve = animationCurveIn_Hard;

        tween.from = new Vector3(crr_x, crr_y, 0);
        tween.to = new Vector3(mov_x, mov_y, 0);

        tween.SetOnFinished(callback);
    }

    public static void TweenColorAnimation(GameObject obj, EventDelegate.Callback callback, float time)
    {
        if (obj.GetComponent<TweenColor>() == null)
        {
            obj.AddComponent<TweenColor>();
        }
        TweenColor tween = TweenColor.Begin(obj, time, new Color(255f / 255f, 255f / 255f, 255f / 255f, 45f / 255f));
        tween.animationCurve = animationCurveIn;

        tween.ResetToBeginning();
        tween.enabled = true;

        tween.from = new Color(255f / 255f, 255f / 255f, 255f / 255f, 45f / 255f);
        tween.to = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);

        tween.SetOnFinished(callback);

    }

}
