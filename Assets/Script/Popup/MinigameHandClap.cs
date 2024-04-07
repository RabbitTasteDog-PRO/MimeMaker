using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameHandClap : MonoBehaviour
{

    public TweenRotation tween;
    public GameObject objectClap;

    void Awake()
    {

    }

    public void CreateHandClap()
    {
        objectClap.layer = LayerMask.NameToLayer("POPUP");
        tween.AddOnFinished(new EventDelegate(HandClapFinish));
    }


    void HandClapFinish()
    {
        StartCoroutine(IEDestroyObject());
    }


    IEnumerator IEDestroyObject()
    {
        yield return new WaitForSeconds(.5f);
        Destroy(this.gameObject);
    }
}
