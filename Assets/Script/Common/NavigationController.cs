using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 레 이 어 의 타 입 정 의 
public enum eUILayerType
{
    LAYER = 0,
    VIEW,
    POPUP,
    COUNT
}


// 레 이 어 의 이 름 정 의 (= 프 리 팹 이 름 사 용)
public enum eUILayerId
{
    // RootView
    ViewNone = -1,
    Popup_Tutorial,
    COUNT
}


// 레 이 어 큐 잉 을 위 한 구 조 체 
public struct STEnqueueLayerInfo
{
    public eUILayerType layerType;
    public eUILayerId layerId;
    public eUILayerId groupId;
    public object param;
    public bool useAnimation;
    public bool isExternal;

    public STEnqueueLayerInfo(eUILayerId _layerId, object _param)
    {
        layerType = eUILayerType.LAYER;
        groupId = eUILayerId.ViewNone;
        layerId = _layerId;
        param = _param;
        useAnimation = true;
        isExternal = false;
    }

    public STEnqueueLayerInfo(eUILayerId _layerId, eUILayerId _groupId, object _param)
    {
        layerType = eUILayerType.LAYER;
        groupId = _groupId;
        layerId = _layerId;
        param = _param;
        useAnimation = true;
        isExternal = false;
    }

    public STEnqueueLayerInfo(eUILayerId _layerId, eUILayerId _groupId, object _param, bool _useAnimation)
    {
        layerType = eUILayerType.LAYER;
        groupId = _groupId;
        layerId = _layerId;
        param = _param;
        useAnimation = _useAnimation;
        isExternal = false;
    }
}


public class NavigationController : MonoBehaviour
{
    protected const string DIR = "Prefab/Popup/{0}";
    protected bool ISACTION_POPUP_ACTIVE = false;

    private Queue<STEnqueueLayerInfo> m_pLayerWaitingQueue;
    private List<eUILayerId> m_pWaitingQueueList;	// 중 복 검 사 용 

    public GameObject m_pRoot;
    public GameObject rootObject { get { return m_pRoot; } set { m_pRoot = value; } }

    #region 생성 / 소멸 
    void Awake()
    {
        initialize();
    }
    void OnDestroy()
    {
        release();
    }
    protected void initialize()
    {
        m_pLayerWaitingQueue = new Queue<STEnqueueLayerInfo>();
        m_pWaitingQueueList = new List<eUILayerId>();
    }
    protected void release()
    {
        if (m_pLayerWaitingQueue != null)
        {
            m_pLayerWaitingQueue.Clear();
            m_pLayerWaitingQueue = null;
        }


        if (m_pWaitingQueueList != null)
        {
            m_pWaitingQueueList.Clear();
        }
    }
    #endregion


    #region Popup Control

    // /// <summary>
    // /// 현 재 뷰 컨 트 롤 러 에 팝 업 추 가 
    // /// </summary>
    // public void AddPopup(eUILayerId _layerId, object _param = null)
    // {
    //     m_pOwner.TouchBlockOn();

    //     if (CanAdd(_layerId) == false)
    //     {
    //         m_pOwner.TouchBlockOff();
    //         return;
    //     }

    //     AddQueue(eUILayerType.POPUP, _layerId, eUILayerId.ViewNone, _param, false);

    //     if (m_pLayerWaitingQueue.Count == 1)
    //     {
    //         StartCoroutine(IE_AddPopup());
    //     }
    // }




    // /// <summary>
    // /// 현 재 뷰 컨 트 롤 러 에 팝 업 을 직 접 추 가 
    // /// </summary>
    // public void AddPopup(UILayer _layer)
    // {
    //     pushPopup(_layer);
    // }

    // /// <summary>
    // /// 팝 업 추 가 
    // /// </summary>
    // protected void pushPopup(UILayer _layer)
    // {
    //     int currentDepth = 0;

    //     UIViewController pViewController = tailViewController();
    //     if (pViewController != null)
    //     {
    //         currentDepth = pViewController.TopMost().depth + IHConstants.CONST_BASE_PANELDEPTH;
    //         // currentDepth = pViewController.TopMost ().depth + (IHConstants.CONST_BASE_PANELDEPTH + IHConstants.CONST_BASE_POPUP_DEPTH);
    //         _layer.depth = currentDepth;
    //         pViewController.PushSubLayer(_layer);
    //     }

    // }

    // /// <summary>
    // /// 팝 업 을 뷰 컨 트 롤 러 에 추 가 
    // /// </summary>
    // protected IEnumerator IE_AddPopup()
    // {

    //     while (m_pLayerWaitingQueue.Count > 0)
    //     {

    //         STEnqueueLayerInfo info = m_pLayerWaitingQueue.Peek();

    //         m_pLoadedPrefab = null;
    //         yield return StartCoroutine(IE_LoadPrefab(info.layerId));

    //         GameObject o = InstantiatePrefab();

    //         SettingLayer(eUILayerType.POPUP, info.layerId, o);

    //         RemoveQueue();

    //         yield return null;
    //     }

    //     m_pOwner.TouchBlockOff();
    // }



    // /// <summary>
    // /// 현 재 뷰 컨 트 롤 러 에 팝 업 을 대 기 시 간 없 이 바 로 추 가 
    // /// </summary>
    // public UILayer AddInstancePopup(eUILayerId _layerId, object _param = null)
    // {

    //     STEnqueueLayerInfo info = new STEnqueueLayerInfo(_layerId, _param);
    //     m_pLayerWaitingQueue.Enqueue(info);

    //     string path = string.Format(DIR, _layerId.ToString());


    //     GameObject prefab = Resources.Load(path) as GameObject;
    //     GameObject o = Instantiate(prefab) as GameObject;
    //     o.SetActive(false);

    //     UILayer _layer = SettingLayer(eUILayerType.POPUP, _layerId, o);

    //     m_pLayerWaitingQueue.Dequeue();
    //     return _layer;

    // }



    // /// <summary>
    // /// 특 정 팝 업 을 서 브 뷰 에 서 제 거
    // /// </summary>
    // public void RemovePopup(UILayer _layer)
    // {
    //     UIViewController vc = tailViewController();
    //     UILayer _l = vc.PopSubLayer(_layer);
    //     if (_l != null)
    //     {
    //         //p.OnRemoveAction ();
    //         Destroy(_l.gameObject);
    //     }
    //     SetPopupActive(false);
    //     Resources.UnloadUnusedAssets();

    // }

    // ///<summery>
    // /// 2018.11.06 지흠 
    // /// 메인로비에서 팝업만 켜질경우가 있어 팝업 상태값 추가 
    // ///<summery>
    // public void SetPopupActive(bool active)
    // {
    //     if (GetPopupCountInCurrentViewController() != 0)
    //     {
    //         ISACTION_POPUP_ACTIVE = true;
    //     }
    //     else
    //     {
    //         ISACTION_POPUP_ACTIVE = active;
    //     }
    // }

    // /// <summary>
    // /// 생 성 된 오 브 젝 트 로 부 터 UILayer 관 련 설 정 
    // /// </summary>
    // protected UILayer SettingLayer(eUILayerType _layerType, eUILayerId _layerId, GameObject _object)
    // {
    //     GameObject _root = null;

    //     UILayer pLayer = _object.GetComponent<UILayer>();
    //     pLayer.layerId = _layerId;
    //     pLayer.owner = m_pOwner;
    //     pLayer.navigationController = this;
    //     pLayer.layerType = _layerType;


    //     if (m_pRoot == null)
    //     {
    //         Debug.Log("##### m_pRoot is NULL");
    //     }

    //     switch (_layerType)
    //     {
    //         case eUILayerType.VIEW:
    //             pushViewController(pLayer);
    //             _root = m_pRoot;
    //             break;
    //         case eUILayerType.POPUP:
    //             pushPopup(pLayer);
    //             _root = tailViewController().rootView.gameObject;
    //             SetPopupActive(true);
    //             break;
    //     }

    //     STEnqueueLayerInfo info = m_pLayerWaitingQueue.Peek();

    //     if (info.isExternal)
    //     {
    //         pLayer.transform.SetParent(null);
    //     }
    //     else
    //     {
    //         pLayer.transform.SetParent(_root.transform);
    //     }

    //     pLayer.transform.localPosition = Vector3.zero;
    //     pLayer.transform.localScale = Vector3.one;
    //     pLayer.gameObject.SetActive(true);


    //     if (m_pLayerWaitingQueue.Count > 0)
    //     {

    //         pLayer.groupLayerId = info.groupId;
    //         if (info.param != null)
    //         {
    //             pLayer.SetData(info.param);
    //         }
    //     }

    //     return pLayer;
    // }



    /// <summary>
    /// 추 가 할 레 이 어 를 큐 에 등 록 한 다
    /// </summary>
    protected void AddQueue(eUILayerType _layerType, eUILayerId _layerId, eUILayerId _groupId, object _param, bool _useAnimation, bool _isExternal = false)
    {
        STEnqueueLayerInfo info = new STEnqueueLayerInfo(_layerId, _groupId, _param, _useAnimation);
        info.isExternal = _isExternal;
        info.layerType = _layerType;
        m_pLayerWaitingQueue.Enqueue(info);
        m_pWaitingQueueList.Add(_layerId);
    }


    /// <summary>
    /// 추 가 된 레 이 어 를 큐 에 서 제 거 한 다 
    /// </summary>
    protected void RemoveQueue()
    {
        STEnqueueLayerInfo info = m_pLayerWaitingQueue.Dequeue();
        m_pWaitingQueueList.Remove(info.layerId);
    }
    #endregion

}
