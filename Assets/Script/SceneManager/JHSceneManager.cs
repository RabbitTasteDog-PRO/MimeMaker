using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine.SceneManagement;

/// <summary>
/// BC scene manager.
/// 02.03 REFRESH기능 추가.
/// 05.30 AddLayerForCurrentScene API Add
/// </summary>
public class JHSceneManager : MonoBehaviour
{
    ///Scene Manager에서 사용하는 Scene의 저장단위
    public class SceneInfo
    {
        ///Scene의 이름
        public string name;

        ///Scene GameObject, 없다면 찾아서 반환한다.
        private GameObject _scene;
        public GameObject scene
        {
            get
            {
                if (_scene == null)
                {
                    _scene = GameObject.Find(name);
                }

                return _scene;
            }

            set
            {
                _scene = value;
            }
        }
    }

    ///싱글톤 객체
    private static JHSceneManager _instance = null;

    public static JHSceneManager Instance
    {
        ///중복 호출 방지
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            if (_instance == null)
            {
                ///싱글톤 객체를 찾아서 넣는다.
                _instance = (JHSceneManager)FindObjectOfType(typeof(JHSceneManager));

                ///없다면 생성한다.
                if (_instance == null)
                {
                    string goName = typeof(JHSceneManager).ToString();

                    GameObject go = GameObject.Find(goName);
                    if (go == null)
                    {
                        go = new GameObject();
                        go.name = goName;
                    }

                    _instance = go.AddComponent<JHSceneManager>();
                }
            }
            return _instance;
        }
    }

    ///Scene Manager에서 지원하는 기능
    public enum ACTION
    {
        ACTION_NONE,			///아무 행동 하지않는다.
		ACTION_BACKKEY,			///백키를 눌렀을 경우.
		ACTION_PUSH,			///Scene 하나를 추가한다.
		ACTION_PUSHFORADD,		///기존 Scene을 inactive시키고 Scene을 추가한다.
		ACTION_INSERT,			///특정 Scene앞에 Scene을 추가한다.
		ACTION_REPLACE,			///기존 Scene을 대체한다.
		ACTION_REFRESH,			///마지막 Scene을 삭제하고 다시 추가한다.
		ACTION_POP,				///Scene 하나를 삭제한다.
		ACTION_POPFORNAME		///추가 되어 있는 Scene 중 하나를 찾아서 돌아간다.
	};

    ///사용자가 설정 하는 Event들..

    ///back key를 눌러서 앱이 종료 될 상황에서 호출된다.
    public event System.Action Exit;
    ///Scene 전환이 시작 될 때 호출된다. 
    public event System.Action<string> SceneChageStart;
    ///Scene 전환이 종료 될 때 호출된다.
    public event System.Action<string> SceneChageEnd;

    ///Scene 전환이 시작되고 딜레이 시간. 애니메이션 등으로 잠시 딜레이다 필요할 시에 설정.
    public float sceneChageStartDelay = 1.0f;

    ///Scene 전환 시 자동으로 리소스를 언로드 한다..
    public bool resourceAutoUnLoad = true;

    ///Scene 매니징을 위한 스택 객체. (Scene 정보를 쌓아둠) 
    private Stack<SceneInfo> _sceneManager = new Stack<SceneInfo>();
    //public delegate void OnNotification();
    ///델리게이트 설정.
    public delegate IEnumerator OnNotificationCoroutine();
    ///페이드 인아웃을 델리게이트로 설정하여 연출하도록...
    public OnNotificationCoroutine onFadeIn { get; set; }
    public OnNotificationCoroutine onFadeOut { get; set; }

    ///현재 작업중인 ACTION
    private ACTION _currentAction;

    ///전환 할 Scene에 대한 Action, 이름, 데이터를 저장 해 둔다.
    private ACTION _nextAction;
    private string _oldSceneName;
    private string _nextSceneName;
    private Dictionary<string, object> _nextData;

    public Queue<GameObject> m_PopupQueue = new Queue<GameObject>();
    public Dictionary<string, GameObject> m_DicPopupQueue = new Dictionary<string, GameObject>();

    ///안드로이드의 경우 뒤로가기 버튼이 먹도록 설정.
    void Update()
    {
        // if(SceneBase.Instance.GetIsActionToast == false)
        {
            ///back 키 처리..
            if (Input.GetKeyDown(KeyCode.Escape) && _currentAction == ACTION.ACTION_NONE && _sceneManager.Count > 0)
            {
                ///백키에 대한 작업은 코르틴으로 진행한다.
                _nextAction = ACTION.ACTION_BACKKEY;
            }

            ///입력된 액션이 있다면 해당 액션을 처리한다..
            if (_nextAction != ACTION.ACTION_NONE && _currentAction == ACTION.ACTION_NONE)
            {
                _currentAction = _nextAction;
                _nextAction = ACTION.ACTION_NONE;

                StartCoroutine(ActionProcess());
            }
        }

    }

    ///새로운 Scene을 추가한다.
    SceneInfo PushScene(string sceneName)
    {
        SceneInfo newScene = new SceneInfo();
        newScene.name = sceneName;

        _sceneManager.Push(newScene);

        return newScene;
    }

    ///액션을 처리하는 코르틴..
    [MethodImpl(MethodImplOptions.Synchronized)]
    IEnumerator ActionProcess()
    {
        switch (_currentAction)
        {

            case ACTION.ACTION_BACKKEY:
                {
                    // if (m_PopupQueue.Count > 0)
                    // {
                    //     if (m_DicPopupQueue.ContainsKey(m_PopupQueue.Peek().name) == true)
                    //     {
                    //         if (m_DicPopupQueue[m_PopupQueue.Peek().name] != null)
                    //         {
                    //             Destroy(m_DicPopupQueue[m_PopupQueue.Peek().name]);
                    //         }
                    //         m_DicPopupQueue.Remove(m_PopupQueue.Peek().name);
                    //         m_PopupQueue.Dequeue();
                    //     }
                    //     _currentAction = ACTION.ACTION_NONE;
                    //     break;
                    // }

                    // ///back key의 경우 재귀호출 가능성이 있기 때문에 미리 초기화 해준다.
                    // _currentAction = ACTION.ACTION_NONE;

                    // ///마지막 Scene에 back key 함수를 호출
                    // SceneInfo sceneInfo = _sceneManager.Peek();
                    // sceneInfo.scene.GetComponent<JHScene>().BackPressed();
                    break;
                }
            case ACTION.ACTION_PUSH:
            case ACTION.ACTION_PUSHFORADD:
                {
                    ///현재 Scene과 지정한 Scene 이름이 같을 경우 메서드를 빠져나간다.
                    if (!IsContainsScene(_nextSceneName))
                    {
                        SceneInfo peekScene = null;
                        if (_sceneManager.Count > 0)
                        {
                            ///현재 Scene을 가져온다.
                            peekScene = _sceneManager.Peek();
                        }

                        /// 매니저에 Scene정보를 추가하고 가져온다. 
                        SceneInfo newScene = PushScene(_nextSceneName);

                        ///기존 Scene을 조건에 맞게 처리하고 새로운 Scene을 추가함.
                        yield return StartCoroutine(ChangeScene(peekScene, newScene, _nextData));
                    }
                    else
                    {
                        Debug.LogError(string.Format("[{0}] is already in use.", _nextSceneName));
                        _currentAction = ACTION.ACTION_NONE;
                    }

                    break;
                }
            case ACTION.ACTION_REPLACE:
                {
                    //현재 Scene과 지정한 Scene 이름이 같을 경우 메서드를 빠져나간다.
                    if (!IsContainsScene(_nextSceneName))
                    {
                        SceneInfo peekScene = null;
                        if (_sceneManager.Count > 0)
                        {
                            peekScene = _sceneManager.Pop();
                        }

                        /// 매니저에 Scene정보를 추가하고 가져온다. 
                        SceneInfo newScene = PushScene(_nextSceneName);

                        ///기존 Scene을 조건에 맞게 처리하고 새로운 Scene을 추가함.
                        yield return StartCoroutine(ChangeScene(peekScene, newScene, _nextData));
                    }
                    else
                    {
                        Debug.LogError(string.Format("[{0}] is already in use.", _nextSceneName));
                        _currentAction = ACTION.ACTION_NONE;
                    }

                    break;
                }
            case ACTION.ACTION_REFRESH:
                {
                    if (_sceneManager.Count > 0)
                    {
                        SceneInfo newScene = _sceneManager.Pop();

                        ///마지막 Scene을 삭제하고 같은정보로 다시 생성한다.
                        yield return StartCoroutine(ChangeScene(newScene, newScene, _nextData));
                    }
                    else
                    {
                        Debug.LogError("_sceneManager empty.");
                        _currentAction = ACTION.ACTION_NONE;
                    }
                    break;
                }
            case ACTION.ACTION_POP:
                {
                    //저장되어 있는 Scene이 있을 경우, 최종 1개 Scene은 삭제하지 않는다.
                    if (_sceneManager.Count > 1)
                    {
                        SceneInfo peekScene = null;
                        if (_sceneManager.Count > 0)
                        {
                            peekScene = _sceneManager.Pop();
                        }

                        ///현재 Scene은 삭제하고 그 전 Scene을 가져온다.
                        SceneInfo newScene = _sceneManager.Peek();

                        ///기존 Scene을 조건에 맞게 처리하고 새로운 Scene을 추가함.
                        yield return StartCoroutine(ChangeScene(peekScene, newScene, _nextData));
                    }
                    else
                    { //되돌아갈 Scene이 없는 경우
                      //어플리케이션 종료
                        if (Exit != null)
                        {
                            Exit();
                        }

                        _currentAction = ACTION.ACTION_NONE;
                    }

                    break;
                }
            case ACTION.ACTION_POPFORNAME:
                {
                    if (IsContainsScene(_nextSceneName) && !CurrentScene().name.Equals(_nextSceneName))
                    {
                        SceneInfo peekScene = _sceneManager.Pop();
                        SceneInfo newScene = null;

                        do
                        {
                            SceneInfo sceneInfo = _sceneManager.Peek();
                            if (sceneInfo.name.Equals(_nextSceneName))
                            {
                                newScene = sceneInfo;
                                break;
                            }
                            else
                            {
                                if (sceneInfo.scene != null)
                                {
                                    //Destroy(sceneInfo.scene);
#if (UNITY_5_0 || UNITY_5_1 || UNITY_5_2)
								Destroy(sceneInfo.scene);
#elif UNITY_5_3_OR_NEWER
                                    UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneInfo.name);
#endif
                                    sceneInfo.scene = null;
                                }

                                _sceneManager.Pop();
                            }
                        } while (_sceneManager.Count > 0);

                        ///기존 Scene을 조건에 맞게 처리하고 새로운 Scene을 추가함.
                        yield return StartCoroutine(ChangeScene(peekScene, newScene, _nextData));
                    }
                    else
                    {
                        Debug.LogError(string.Format("can not find [{0}].", _nextSceneName));
                        _currentAction = ACTION.ACTION_NONE;
                    }

                    break;
                }
            case ACTION.ACTION_INSERT:
                {
                    if (IsContainsScene(_oldSceneName))
                    {
                        SceneInfo peekScene = _sceneManager.Peek();

                        do
                        {
                            SceneInfo sceneInfo = _sceneManager.Peek();
                            if (sceneInfo.name.Equals(_oldSceneName))
                            {
                                break;
                            }
                            else
                            {
                                if (sceneInfo.scene != null)
                                {
#if (UNITY_5_0 || UNITY_5_1 || UNITY_5_2)
								Destroy(sceneInfo.scene);
#elif UNITY_5_3_OR_NEWER
                                    UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneInfo.name);
#endif
                                    sceneInfo.scene = null;
                                }

                                _sceneManager.Pop();
                            }
                        } while (_sceneManager.Count > 0);

                        /// 매니저에 Scene정보를 추가하고 가져온다. 
                        SceneInfo newScene = PushScene(_nextSceneName);

                        ///기존 Scene을 조건에 맞게 처리하고 새로운 Scene을 추가함.
                        yield return StartCoroutine(ChangeScene(peekScene, newScene, _nextData));
                    }
                    else
                    {
                        Debug.LogError(string.Format("can not find [{0}].", _nextSceneName));
                        _currentAction = ACTION.ACTION_NONE;
                    }

                    break;
                }
        }
    }

    // ///현재 Scene을 찾아서 Layer를 붙여준다.
    // ///주의! Start나 Awake에서 사용 시 정상작동 안함.
    // ///Scene 시작 시 사용하려면 StartWithData에서 사용바람.
    // public void AddLayerForCurrentScene(JHPopup layer)
    // {
    //     SceneInfo lastSceneInfo = _sceneManager.Peek();
    //     if (lastSceneInfo.scene != null)
    //     {
    //         lastSceneInfo.scene.GetComponent<JHScene>().AddLayer(layer, null);
    //     }
    // }

    ///현재 Scene의 클레스를 반환.
    public SceneInfo CurrentScene()
    {
        return _sceneManager.Peek();
    }

    ///현재 Scene의 이름을 반환.
    public string CurrentSceneName()
    {
        return _sceneManager.Peek().name.ToString();
    }

    ///현재 저장된 Scene의 갯수를 반환.
    public int SceneCount()
    {
        return _sceneManager.Count;
    }

    ///이미 추가되어 있는 Scene인가?
    public bool IsContainsScene(string sceneName)
    {
        foreach (SceneInfo sceneInfo in _sceneManager)
        {
            if (sceneInfo.name.Equals(sceneName))
            {
                return true;
            }
        }

        return false;
    }


    public void LogStack()
    {
        StringBuilder strings = new StringBuilder();
        foreach (SceneInfo sceneInfo in _sceneManager)
        {
            strings.Append(sceneInfo.name + " ");
        }

        Debug.Log(strings.ToString());
    }

    //------------------------------------------------------------------------------------

    ///여러가지 액션을 처리하도록 만들어진 함수..
    public bool Action(ACTION action, string sceneName = null, Dictionary<string, object> datas = null)
    {
        if (_currentAction == ACTION.ACTION_NONE)
        {
            _nextSceneName = sceneName;
            _nextData = datas;
            _nextAction = action;

            return true;
        }

        return false;
    }

    ///여러가지 액션을 처리하도록 만들어진 함수..
    public bool Action(ACTION action, string oldSceneName, string sceneName, Dictionary<string, object> datas = null)
    {
        if (_currentAction == ACTION.ACTION_NONE)
        {
            _oldSceneName = oldSceneName;
            _nextSceneName = sceneName;
            _nextData = datas;
            _nextAction = action;

            return true;
        }

        return false;
    }

    ///Scene 전환을 자연스럽게 처리하는 코루틴
	[MethodImpl(MethodImplOptions.Synchronized)]
    IEnumerator ChangeScene(SceneInfo peekScene, SceneInfo newScene, Dictionary<string, object> datas = null)
    {
        // Debug.Log("peekScene : " + peekScene + " // newScene : " + newScene);
        ///Scene 시작 이벤트
        if (SceneChageStart != null)
        {
            SceneChageStart(newScene.name);
        }

        //FadeIn 효과
        if (onFadeIn != null)
        {
            yield return StartCoroutine(onFadeIn());
        }

        ///시작 시 애니메이션이 있을 수 있으므로 약간의 딜레이를 설정 할 수 있다.
        yield return new WaitForSeconds(sceneChageStartDelay);

        if (peekScene != null)
        {
            ///각 타입에 맞게 기존 Scene을 처리한다.
            switch (_currentAction)
            {
                case ACTION.ACTION_PUSH:
                case ACTION.ACTION_REPLACE:
                case ACTION.ACTION_POP:
                case ACTION.ACTION_POPFORNAME:
                case ACTION.ACTION_INSERT:
                case ACTION.ACTION_REFRESH:
                    {

                        ///Scene 삭제.
                        // Destroy(peekScene.scene);
                        // peekScene.scene = null;

                        ///Scene 삭제 전 모든 레이어를 우선적으로 삭제한다.
                        if (peekScene.scene != null)
                        {

                            JHScene scene = peekScene.scene.GetComponent<JHScene>();

                            if (scene != null)
                            {
                                scene.RemoveLayerAll();
                            }
                            //                        Debug.Log("peekScene.scene.GetComponent<BCScene>() : " + peekScene.scene.GetComponent<BCScene>());
                        }


                        ///Scene 삭제.
#if (UNITY_5_0 || UNITY_5_1 || UNITY_5_2)
			Destroy(peekScene.scene);
#elif UNITY_5_3_OR_NEWER
                        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(peekScene.name);
#endif
                        peekScene.scene = null;
                        break;
                    }
                case ACTION.ACTION_PUSHFORADD:
                    {
                        ///Scene 비활성화.
                        peekScene.scene.SetActive(false);
                        break;
                    }
            }

            if (resourceAutoUnLoad)
            {
                ///사용하지 않는 리소스는 메모리 해제 한다.
                Resources.UnloadUnusedAssets();
            }
        }

        ///기존 씬이 완전히 삭제될때를 기다린다.
        if (peekScene != null)
        {
            do
            {
                yield return new WaitForEndOfFrame();
            } while (GameObject.Find(peekScene.name) != null);
        }

        ///신규 Scene이 비활성화만 되어있는 경우에는 활성화 시켜준다.
        if (newScene.scene != null)
        {
            newScene.scene.SetActive(true);
        }
        else
        {
            //Scene을 새로 생성해서 붙여준다.
#if (UNITY_5_0 || UNITY_5_1 || UNITY_5_2)
			AsyncOperation operation = Application.LoadLevelAdditiveAsync(newScene.name);
#elif UNITY_5_3_OR_NEWER
            AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(newScene.name, UnityEngine.SceneManagement.LoadSceneMode.Additive);
#endif
            //Scene 로드가 끝날 때까지 기다린다.
            while (!operation.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        ///Scene이 추가되었다면 데이터를 보내준다.
        newScene.scene.GetComponent<JHScene>().StartWithData(datas);

        ///Scene종료 이벤트
        if (SceneChageEnd != null)
        {
            SceneChageEnd(newScene.name);
        }

        ///초기화
        _nextData = null;
        _nextSceneName = "";
        _nextAction = ACTION.ACTION_NONE;
        _currentAction = ACTION.ACTION_NONE;

        //FadeOut 효과
        if (onFadeOut != null)
        {
            yield return StartCoroutine(onFadeOut());
        }
    }
    //------------------------------------------------------------------------------------

    // /Scene 전환을 자연스럽게 처리하는 코루틴
    // / StartDelay 활용할수 있어서 오버라이드 함 
    [MethodImpl(MethodImplOptions.Synchronized)]
    IEnumerator ChangeScene(SceneInfo peekScene, SceneInfo newScene, float StartDelay, Dictionary<string, object> datas = null)
    {
        ///Scene 시작 이벤트
        if (SceneChageStart != null)
        {
            SceneChageStart(newScene.name);
        }

        //FadeIn 효과
        if (onFadeIn != null)
        {
            yield return StartCoroutine(onFadeIn());
        }

        ///시작 시 애니메이션이 있을 수 있으므로 약간의 딜레이를 설정 할 수 있다.
        yield return new WaitForSeconds(StartDelay);

        if (peekScene != null)
        {
            ///각 타입에 맞게 기존 Scene을 처리한다.
            switch (_currentAction)
            {
                case ACTION.ACTION_PUSH:
                case ACTION.ACTION_REPLACE:
                case ACTION.ACTION_POP:
                case ACTION.ACTION_POPFORNAME:
                case ACTION.ACTION_INSERT:
                    {
                        ///Scene 삭제.
                        Destroy(peekScene.scene);
                        peekScene.scene = null;
                        break;
                    }
                case ACTION.ACTION_PUSHFORADD:
                    {
                        ///Scene 비활성화.
                        peekScene.scene.SetActive(false);
                        break;
                    }
            }

            if (resourceAutoUnLoad)
            {
                ///사용하지 않는 리소스는 메모리 해제 한다.
                Resources.UnloadUnusedAssets();
            }
        }

        ///신규 Scene이 비활성화만 되어있는 경우에는 활성화 시켜준다.
        if (newScene.scene != null)
        {
            newScene.scene.SetActive(true);
        }
        else
        {
            //Scene을 새로 생성해서 붙여준다.
            AsyncOperation operation = Application.LoadLevelAdditiveAsync(newScene.name);

            //Scene 로드가 끝날 때까지 기다린다.
            while (!operation.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        ///Scene이 추가되었다면 데이터를 보내준다.
        if (datas != null)
        {
            newScene.scene.GetComponent<JHScene>().StartWithData(datas);
        }

        ///Scene종료 이벤트
        if (SceneChageEnd != null)
        {
            SceneChageEnd(newScene.name);
        }

        ///초기화
        _nextData = null;
        _nextSceneName = "";
        _nextAction = ACTION.ACTION_NONE;
        _currentAction = ACTION.ACTION_NONE;

        //FadeOut 효과
        if (onFadeOut != null)
        {
            yield return StartCoroutine(onFadeOut());
        }
    }
}
