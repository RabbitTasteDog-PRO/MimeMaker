using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : Ray_Singleton<PopupManager>
{

    void Awake()
    {
        gameObject.SetActive(true);

        if (RootPopup.childCount > 0)
        {
            RootPopup.DestroyChildren();
        }

        PopupManager.Instance.NoneTouchBlock(false);

        DontDestroyOnLoad(this);
    }

    public GameObject objectPopup;
    public Transform RootPopup;

    public GameObject objNoneTouchBlock;

    // public List<GameObject> listTest = new List<GameObject>();

    public int GetPopupCount()
    {
        return RootPopup.childCount;
    }


    public void NoneTouchBlock(bool enalbe = false)
    {
        StartCoroutine(IEDisableNoneTouchBlock(enalbe));
    }

    //// ㅎ시 안꺼질때 대비해서 ㄸ뜨
    public IEnumerator IEDisableNoneTouchBlock(bool enabled)
    {
        if (enabled == true)
        {
            yield return new WaitForSeconds(5.0f);
            objNoneTouchBlock.SetActive(false);
        }

    }

}
