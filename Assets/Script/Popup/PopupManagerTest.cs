using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Enums;


public class PopupManagerTest : JHPopup
{

    public UILabel labelManagerTest;
    public UILabel labelTestQuestion;


    public UIButton[] arrBtnChoice;
    public GameObject[] objBtnChoice_On;
    public GameObject[] objBtnChoice_Off;

    public UI2DSprite[] arrSpriteChoiceImage;
    public UILabel[] arrLabelChoiceText;



    public UIGrid gridButton;
    public UIButton btnPrev;
    public UILabel labelBtnPrev;
    public UIButton btnNext;
    public UILabel labelBtnNext;

    string DISABLE_NEXT_BTN = "test_btn_ok_off_211x112";
    string ENABLE_NEXT_BTN = "test_btn_ok_on_211x112";
    Color32 DISABLE_NEXT_COLOR = new Color32(127, 127, 127, 255);
    Color32 ENABLE_NEXT_COLOR = new Color32(80, 38, 21, 255);



    List<xmlManagerTest> listMngTest;
    int choiceCnt = 0;


    protected override void OnAwake()
    {
        base.OnAwake();


        btnPrev.gameObject.SetActive(false);
        gridButton.Reposition();

        btnPrev.onClick.Add(new EventDelegate(OnClickPrev));
        labelBtnPrev.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eBtnPrev);
        btnNext.onClick.Add(new EventDelegate(OnClickNext));
        labelBtnNext.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eBtnNext);

        btnNext.GetComponent<UISprite>().spriteName = DISABLE_NEXT_BTN;
        btnNext.normalSprite = DISABLE_NEXT_BTN;
        labelBtnNext.color = DISABLE_NEXT_COLOR;

        for (int i = 0; i < arrBtnChoice.Length; i++)
        {
            EventDelegate del = new EventDelegate(this, "OnClickChoice");
            EventDelegate.Parameter parm = new EventDelegate.Parameter();
            parm.value = i;
            parm.expectedType = typeof(int);
            del.parameters[0] = parm;
            EventDelegate.Add(arrBtnChoice[i].onClick, del);
        }
    }

    protected override void OnDestroied()
    {
        base.OnDestroied();

        if (isGetManager == true)
        {
            Transform _root = PopupManager.Instance.RootPopup;
            SceneBase.Instance.AddPopup(_root, Enums.ePopupLayer.PopupGetManager, returnData);
            returnData.Initialize();
        }
    }

    public override void SetData()
    {
        base.SetData();
    }

    protected override void OnStart()
    {
        base.OnStart();

        if (listMngTest == null)
        {
            listMngTest = XmlParser.Read<xmlManagerTest>("XML/MnagerTest/xmlManagerTest");
        }

        SetUIManagerTest(0);
        SetPrevNextButton(0);


    }

    int[] arrChoiceIndex = new int[]
    {
        -1,-1,-1,-1,-1
    };
    void SetUIManagerTest(int choiceIndex)
    {

        int _index = Mathf.Max(0, Mathf.Min(choiceIndex, (listMngTest.Count - 1)));

        string title = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eManagerTestTitle);
        labelManagerTest.text = string.Format(title, _index, (listMngTest.Count - 1));

        /// 질문 
        labelTestQuestion.text = listMngTest[_index].quest;

        string[] _arrImage = new string[]
        {
            listMngTest[_index].choice_img_0,listMngTest[_index].choice_img_1,
            listMngTest[_index].choice_img_2,listMngTest[_index].choice_img_3
        };
        string[] _arrChoice = new string[]
        {
            listMngTest[_index].choice_0,listMngTest[_index].choice_1,
            listMngTest[_index].choice_2,listMngTest[_index].choice_3
        };


        for (int i = 0; i < _arrImage.Length; i++)
        {
            arrSpriteChoiceImage[i].sprite2D = Resources.Load<Sprite>(string.Format("Image/ManagerTest/{0}", _arrImage[i]));
            arrLabelChoiceText[i].text = _arrChoice[i];
        }

    }

    protected override void OnClosed()
    {
        base.OnClosed();
    }


    void OnClickChoice(int choice)
    {

        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        arrChoiceIndex[choiceCnt] = choice;

        for (int i = 0; i < objBtnChoice_Off.Length; i++)
        {
            if (i == choice)
            {
                objBtnChoice_On[i].SetActive(true);
                objBtnChoice_Off[i].SetActive(false);
            }
            else
            {
                objBtnChoice_On[i].SetActive(false);
                objBtnChoice_Off[i].SetActive(true);
            }
        }

        SetPrevNextButton(choiceCnt);
    }

    void SetChoiceOnOff()
    {
        for (int i = 0; i < objBtnChoice_Off.Length; i++)
        {
            if (i == arrChoiceIndex[choiceCnt])
            {
                objBtnChoice_On[i].SetActive(true);
                objBtnChoice_Off[i].SetActive(false);
            }
            else
            {
                objBtnChoice_On[i].SetActive(false);
                objBtnChoice_Off[i].SetActive(true);
            }
        }

    }

    void SetPrevNextButton(int choiceIndex)
    {
        btnPrev.gameObject.SetActive(choiceIndex == 0 ? false : true);

        btnNext.GetComponent<UISprite>().spriteName = arrChoiceIndex[choiceIndex] <= -1 ? DISABLE_NEXT_BTN : ENABLE_NEXT_BTN;
        btnNext.normalSprite = arrChoiceIndex[choiceIndex] <= -1 ? DISABLE_NEXT_BTN : ENABLE_NEXT_BTN;
        labelBtnNext.color = arrChoiceIndex[choiceIndex] <= -1 ? DISABLE_NEXT_COLOR : ENABLE_NEXT_COLOR;

        if (choiceIndex >= (listMngTest.Count - 1))
        {
            labelBtnNext.text = SceneBase.Instance.dataManager.GetDicGlobalTextData(Enums.eGlobalTextKey.eBtnResult);
        }

        gridButton.Reposition();
    }



    void OnClickPrev()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);

        choiceCnt--;

        if (choiceCnt < 0)
        {
            choiceCnt = 0;
        }

        SetUIManagerTest(choiceCnt);
        SetPrevNextButton(choiceCnt);
        SetChoiceOnOff();
    }

    bool isGetManager = false;
    void OnClickNext()
    {
        SceneBase.Instance.PLAY_SFX(eSFX.SFX_4_touch1);
        

        if (arrChoiceIndex[choiceCnt] < 0)
        {
            return;
        }

        choiceCnt++;

        if (choiceCnt > (listMngTest.Count - 1))
        {
            choiceCnt--;
            /// 결과보기 이동
            isGetManager = true;

            SetReturnValue();
            OnClosed();

            return;
        }

        SetUIManagerTest(choiceCnt);
        SetPrevNextButton(choiceCnt);
        SetChoiceOnOff();

    }

    ///<summary>
    /// 매니저 테스트 벨류값 추출
    ///</summary>
    void SetReturnValue()
    {
        string[,] choiceValue = new string[,]
        {
            {listMngTest[0].choice_value_0,listMngTest[0].choice_value_1,listMngTest[0].choice_value_2,listMngTest[0].choice_value_3},
            {listMngTest[1].choice_value_0,listMngTest[1].choice_value_1,listMngTest[1].choice_value_2,listMngTest[1].choice_value_3},
            {listMngTest[2].choice_value_0,listMngTest[2].choice_value_1,listMngTest[2].choice_value_2,listMngTest[2].choice_value_3},
            {listMngTest[3].choice_value_0,listMngTest[3].choice_value_1,listMngTest[3].choice_value_2,listMngTest[3].choice_value_3},
            {listMngTest[4].choice_value_0,listMngTest[4].choice_value_1,listMngTest[4].choice_value_2,listMngTest[4].choice_value_3},

        };

        string[] resultValue = new string[arrChoiceIndex.Length];

        for (int i = 0; i < arrChoiceIndex.Length; i++)
        {
            int _choiceValue = arrChoiceIndex[i];

            string value = choiceValue[i, _choiceValue];
            Debug.LogError("SetReturnValue : " + value);
            resultValue[i] = value;
        }

        Dictionary<ePersonality_Type, int> dicVlaue = new Dictionary<ePersonality_Type, int>();
        for (int i = 0; i < resultValue.Length; i++)
        {
            string[] splite_ = resultValue[i].Split(',');

            for (int j = 0; j < splite_.Length; j++)
            {
                if (dicVlaue.ContainsKey((ePersonality_Type)j) == false)
                {
                    dicVlaue.Add((ePersonality_Type)j, int.Parse(splite_[j]));
                }
                else
                {
                    int beforValue = dicVlaue[(ePersonality_Type)j];
                    int plusValue = beforValue + int.Parse(splite_[j]);
                    dicVlaue[(ePersonality_Type)j] = plusValue;
                }
            }
        }


        /// 오름차순 정렬 
        var rannk = dicVlaue.OrderByDescending(num => num.Value);

        string[] _key = new string[2];
        int[] _value = new int[2];

        int number = 0;
        foreach (var r in rannk)
        {
            _key[number] = r.Key.ToString();
            _value[number] = r.Value;
            number++;
            if (number >= 2)
            {
                break;
            }
        }

        int select = 0;
        if (_value[0] == _value[1])
        {
            int _random = Random.Range(0, 10);
            // Debug.LogError("Random Value : " + _random);
            select = _random < 5 ? 0 : 1;
        }

        // Debug.LogError("####### key 0 : " + _key[0] + " Value 0 : " + _value[0]);
        // Debug.LogError("@@@@@@@ key 1 : " + _key[1] + " Value 1 : " + _value[1]);
        // Debug.LogError("####### 선택 키값 : " + select);

        string fixKey = _key[select];
        int fixValue = _value[select];

        returnData[0] = fixKey;
        returnData[1] = fixValue;


        _key.Initialize();
        _value.Initialize();

        Debug.LogError(string.Format("fixKey : {0} , FixValue : {1}", fixKey, fixValue));


    }

    object[] returnData = new object[2];


}
