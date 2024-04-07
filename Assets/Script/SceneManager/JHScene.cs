using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Scene 기본 클레스, 해당 클레스는 상속받아서 사용한다.
/// </summary>
public class JHScene : MonoBehaviour
{
    ///대기 레이어 사용 시 체크 시간. 다음 레이어의 뜨는 속도를 조절 하고 싶다면 수정한다.
    public float waitLayerTime = 0.1f;

    ///현재 활성화 중인 레이어를 담아둔다. Stack으로 하기에는 순서가 일정치 않을 수 있다..
    private List<JHPopup> _activeLayers = new List<JHPopup>();
    ///활성화 대기중인 레이어를 담아둔다. 순서대로 떠야 하므로 큐 처리..
    private List<JHPopup> _waitLayers = new List<JHPopup>();

    ///씬 생성 시 데이터를 저장하는 용도
    private Dictionary<string, object> _datas = null;

    ///활성화 대기 코르틴 제어 변수
    private Coroutine _waitLayerCoroutine;

    ///씬 생성 시 데이터를 받았다면 해당 함수가 호출된다. override 한다.
    public virtual void StartWithData(Dictionary<string, object> datas)
    {
        _datas = datas;
    }

    ///데이터 가져오기.
    public virtual object GetData(string key)
    {
        if (_datas == null) return null;

        if (_datas.ContainsKey(key))
        {
            return _datas[key];
        }

        return null;
    }

    ///활성화 대기 중인 레이어를 체크하고 활성화 된 레이어가 없다면 뛰워준다.
    IEnumerator StartWaitLayer()
    {
        ///너무 빠르게 레이어가 뜨는것을 방지하기 위해 일정 시간 기다린다.
        yield return new WaitForSeconds(waitLayerTime);

        ///활성화 대기 레이어를 한바퀴 돌면서 현재 레이어에 groupId가 없는것들만 순서대로 활성화 시킨다.
        for (int i = 0; i < _waitLayers.Count; i++)
        {

            ///i번째 레이어의 groupId가 현재 존재 하는지..
            bool haveGroupId = false;
            for (int j = 0; j < _activeLayers.Count; j++)
            {
                // if (_activeLayers[j].groupId == _waitLayers[i].groupId)
                // {
                //     haveGroupId = true;
                //     break;
                // }
            }

            ///존재 하지 않는다면 활성화!
            if (!haveGroupId)
            {
                AddLayer(_waitLayers[i], null);
                _waitLayers.Remove(_waitLayers[i]);

                yield return StartCoroutine(StartWaitLayer());
                break;
            }
        }

        _waitLayerCoroutine = null;
    }

    ///활성호 대기열에 레이어를 추가한다.
    public virtual void AddWaitLayer(JHPopup layer)
    {
        if (!_waitLayers.Contains(layer))
        {
            _waitLayers.Add(layer);

            if (_waitLayerCoroutine == null)
            {
                _waitLayerCoroutine = StartCoroutine(StartWaitLayer());
            }
        }
        else
        {
            Debug.LogError("duplicate the waitlayer : " + layer.name);
        }
    }

    ///레이어를 추가한다.
    public virtual void AddLayer(JHPopup layer, object data = null)
    {
        if (!_activeLayers.Contains(layer))
        {
            _activeLayers.Add(layer);

            // layer.parentScene = this;
            // layer.StartLayer();
        }
        else
        {
            Debug.LogError("duplicate the layer : " + layer.name);
        }
    }

    public virtual void AddLayer(JHPopup layer, object[] data = null)
    {
        if (!_activeLayers.Contains(layer))
        {
            _activeLayers.Add(layer);

            // layer.parentScene = this;
            // layer.StartLayer();
        }
        else
        {
            Debug.LogError("duplicate the layer : " + layer.name);
        }
    }

    ///레이어를 삭제한다.
    public virtual void RemoveLayer(JHPopup layer)
    {
        _activeLayers.Remove(layer);

        ///기존 레이어 삭제 시 활성화 대기 레이어를 체크한다.
        if (_waitLayerCoroutine == null)
        {
            _waitLayerCoroutine = StartCoroutine(StartWaitLayer());
        }
    }

    ///모든 레이어를 삭제한다.
    public void RemoveLayerAll()
    {
        if (_activeLayers.Count == 0) return;

        ///RemoveLayer의 호출로 인한 버그를 막기위해 복사 후 사용한다.
        JHPopup[] deleteLayers = new JHPopup[_activeLayers.Count];
        _activeLayers.CopyTo(deleteLayers);

        _activeLayers.Clear();

        for (int i = 0; i < deleteLayers.Length; i++)
        {
            // deleteLayers[i].EndLayer();
        }
    }

    /// <summary>
    /// 현재 레이어 갯수를 반환
    /// 씬에서 스크립트로 관리할경우 
    /// 백키를 눌렀을때 팝업이 떠있고 오브젝트 처리를 같이해야할 경우가 
    /// 생겨서 레이어 갯수 반환하는 함수 생성
    /// </summary>
    public int LayerCount()
    {
        return _activeLayers.Count;
    }

    /// <summary>
    /// 현재 레이어 갯수를 반환
    /// 씬에서 스크립트로 관리할경우 
    /// 백키를 눌렀을때 팝업이 떠있고 오브젝트 처리를 같이해야할 경우가 
    /// 생겨서 레이어 갯수 반환하는 함수 생성
    /// </summary>
    public string LayerName()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < _activeLayers.Count; i++)
        {
            // Debug.Log("_activeLayers [ " + i + " ] : " + _activeLayers[i].name);
            // return "";
            sb.AppendLine(_activeLayers[i].name);
        }

        return sb.ToString();
    }

    ///뒤로가기를 눌렀을 시 레이어가 있다면 먼저 삭제하고 없다면 씬을 종료한다.
    public virtual void BackPressed()
    {
        // if (_activeLayers.Count > 0)
        // {
        //     // if (_activeLayers[_activeLayers.Count - 1].backKeyEnable)
        //     // {
        //     //     _activeLayers[_activeLayers.Count - 1].EndLayer();
        //     // }
        // }
        // else
        // {
        //     EndScene();
        // }
    }

    ///씬을 종료한다.
    public virtual void EndScene()
    {
        JHSceneManager.Instance.Action(JHSceneManager.ACTION.ACTION_POP);
    }

    ///활성화 중인 레이어가 있는지 체크한다.
    public bool IsActiveLayer(JHPopup layer)
    {
        return _activeLayers.Contains(layer);
    }

    ///레이어가 몇개 있는지..
    public int GetLayerCount()
    {
        return _activeLayers.Count;
    }
}
