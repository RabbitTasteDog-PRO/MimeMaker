using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using StructuredDataType;

public class PopupMiniGame : JHPopup
{

    const int timer = 20;
    int countDown;
    int clapCount = 0;

    [Header("박수")]
    public GameObject objClap;
    public UIButton btnTouch;
    public UILabel labelClapCount;

    [Header("타이머")]
    public UILabel labelCountDown;

    [Header("밈 대사들")]
    public GameObject objMiniGameDesc;
    public UILabel labelMiniGameDesc;

    public GameObject objMemeTalk;
    public UILabel labelMemeTalk;

    public MinigameHandClap prefabClap;



    bool isActionStart = false;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    // void Awake()
    protected override void OnAwake()
    {
        base.OnAwake();
        btnTouch.onClick.Add(new EventDelegate(OnClickTouch));
        // string desc = SceneBase.Instance.dataManager.GetDicGlobalTextData(eGlobalTextKey.eMiniGameDesc);
        // labelMiniGameDesc.text = desc;
        labelCountDown.text = timer.ToString();
    }


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    IEnumerator Start()
    {
        Debug.LogError("2초 뒤 시작");
        yield return new WaitForSeconds(2.0f);
        isActionStart = true;
        StartCoroutine(IECountDown());
    }

    ///<summary>
    /// 타이머 카운트다운
    ///</summary>
    IEnumerator IECountDown()
    {
        while (true)
        {
            if (countDown >= timer)
            {
                SetEndMiniGameData();
                break;
            }

            yield return new WaitForSeconds(1.0f);
            countDown++;

            labelCountDown.text = (timer - countDown).ToString();


        }
    }



    ///<summary>
    /// 미니게임 터치 영역 온클릭
    ///</summary>
    void OnClickTouch()
    {
        if (isActionStart == true)
        {
            clapCount++;
            labelClapCount.text = clapCount.ToString();

            MinigameHandClap obj = Instantiate(prefabClap, btnTouch.transform) as MinigameHandClap;
            /// -212 321
            /// 256 -409
            obj.transform.localPosition = new Vector2(Random.RandomRange(-212, 256), Random.RandomRange(321, -409));
            obj.transform.localScale = Vector3.one;
            obj.gameObject.SetActive(true);
            
            obj.CreateHandClap();
        }
    }


    ///<summary>
    /// 미니게임 종료시 데이터 처리
    ///</summary>
    void SetEndMiniGameData()
    {
        Debug.LogError("박수미니게임 종료");

        isActionStart = false;
        //// TODO 20200833
        //// 박수량에 따라 가감 체크해야함 

        OnClosed();
    }



    protected override void OnClosed()
    {
        base.OnClosed();
    }




}
