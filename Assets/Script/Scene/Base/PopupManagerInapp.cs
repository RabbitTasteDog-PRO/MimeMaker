using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupInappError : MonoBehaviour
{

    public GameObject objectPopup;
    public Transform RootPopup;

    private static PopupInappError _instance = null;

    public static PopupInappError Instance
    {
        ///중복 호출 방지
        // [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            if (_instance == null)
            {
                ///싱글톤 객체를 찾아서 넣는다.
                _instance = (PopupInappError)FindObjectOfType(typeof(PopupInappError));

                ///없다면 생성한다.
                if (_instance == null)
                {
                    string goName = typeof(PopupInappError).ToString();
                    GameObject go = GameObject.Find(goName);
                    if (go == null)
                    {
                        go = new GameObject();
                        go.name = goName;
                    }
                    _instance = go.AddComponent<PopupInappError>();
                }
            }
            return _instance;
        }
    }
}
