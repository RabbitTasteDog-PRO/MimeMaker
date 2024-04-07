using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Enums;

/// <summary>
/// JHScene 안에 들어가는 레이어, 해당 클레스는 상속받아서 사용한다.
/// </summary>
public class JHPopup : MonoBehaviour//, IPopupAbsoluteInheritance
{

    public GameObject PopupObject;
    public UIButton btnCloase;

    public bool isBackButton = true;

    void Update()
    {
        if (isBackButton == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnPopupClose();
            }
        }
    }

    ///<summary>
    /// 팝업 매니저 관계없이 게임오브젝트 삭제
    ///</summary>
    protected virtual void OnClosed()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        Destroy(this.gameObject);
        Transform root = PopupManager.Instance.RootPopup;
        SceneBase.Instance.ClosePopup(root);
    }

    ///<summary>
    /// 팝업 매니저를 이용한 팝업삭제
    ///</summary>
    protected virtual void OnPopupClose()
    {
        Transform root = PopupManager.Instance.RootPopup;
        SceneBase.Instance.ClosePopup(root);
    }


    void Awake()
    {
        OnAwake();

        PopupObject.GetComponent<UIPanel>().alpha = 0.0f;

        int depth = PopupObject.GetComponent<UIPanel>().depth;

        if (PopupManager.Instance.RootPopup.childCount > 0)
        {
            // Debug.LogError("################ : " + PopupManager.Instance.RootPopup.childCount);
            try
            {
                UIPanel panel = PopupManager.Instance.RootPopup.GetChild(Mathf.Max(0, PopupManager.Instance.RootPopup.childCount - 2)).gameObject.GetComponent<UIPanel>();
                string name = PopupManager.Instance.RootPopup.GetChild(PopupManager.Instance.RootPopup.childCount - 1).gameObject.name;
                PopupObject.GetComponent<UIPanel>().depth = (panel.depth + 10);
                // Debug.LogError("마지막꺼 네임 : " + name);
                // Debug.LogError(panel.gameObject.name + " depth : " + panel.gameObject.GetComponent<UIPanel>().depth);
                // Debug.LogError(PopupObject.name + " depth : " + PopupObject.GetComponent<UIPanel>().depth);

            }
            catch (System.Exception e)
            {
                Debug.LogError(" Depth Error :  " + e.ToString());
            }
        }


        if (btnCloase != null)
        {
            btnCloase.onClick.Add(new EventDelegate(OnClosed));
        }

        PopupManager.Instance.objNoneTouchBlock.SetActive(false);

    }
    void Start() { OnStart(); }
    void OnEnable() { OnEnabled(); }
    void OnDisable() { OnDisabled(); }
    void OnDestroy() { OnDestroied(); }


    protected virtual void OnAwake() { }
    protected virtual void OnStart() { }
    protected virtual void OnEnabled() { }
    protected virtual void OnDisabled() { }
    protected virtual void OnDestroied() { }

    public virtual void SetData() { }
    public virtual void SetData(object data) { }
    public virtual void SetData(object[] data) { }
    public virtual void SetData(List<object> data) { }

    public int GetDepth()
    {
        return PopupObject.GetComponent<UIPanel>().depth;
    }
}

