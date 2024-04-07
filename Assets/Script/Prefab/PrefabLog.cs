using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabLog : MonoBehaviour
{

    Color32 COLOR_PLUS = new Color32(125, 189, 245, 255);
    Color32 COLOR_MINUS = new Color32(252, 94, 197, 255);
    Color32 COLOR_SUDDEN = new Color32(108, 68, 64, 255);


    public UILabel labelLog;
    public UISprite spriteDay;
    public UILabel labelDay;
    public BoxCollider2D colliderLog;


    const string KEY_SCHEDULE = "SCHEDULE";

    const string KEY_REST_HP = "REST_HP";

    const string KEY_REST_STATE = "REST_STATE";

    const string KEY_SUDDEN_START = "SUDDEN_START";

    const string KEY_SUDDEN_END = "SUDDEN_END";

    const string KEY_ITEM_EFFECT = "ITEM_EFFECT";
    const string KEY_MANAGER_EFFECT = "MANAGER_EFFECT";


    const string KEY_AWARENESS_UP = "AWARENESS_UP";
    const string KEY_HP_DOWN = "HP_DOWN";


    public void SetDayLog(string _type, int _value, string _day, string _message)
    {
        switch (_type)
        {
            case KEY_SCHEDULE:
                {
                    labelLog.color = _value > 0 ? COLOR_PLUS : COLOR_MINUS;
                    break;
                }
            case KEY_REST_HP:
            case KEY_AWARENESS_UP:
                {
                    labelLog.color = COLOR_PLUS;
                    break;
                }
            case KEY_REST_STATE:
            case KEY_HP_DOWN:
                {
                    labelLog.color = COLOR_MINUS;
                    break;
                }
            case KEY_SUDDEN_START:
                {
                    labelLog.color = COLOR_SUDDEN;
                    break;
                }
        }

        labelDay.text = _day;
        labelLog.text = _message;
        colliderLog.offset = new Vector2(150, 0);
        colliderLog.size = new Vector2(labelLog.width + 50, labelLog.height + 25);
    }

    public void SetDayLog(string _day, int _value, string _message)
    {

        labelLog.color = _value > 0 ? COLOR_PLUS : COLOR_MINUS;
        Debug.LogError("_value : " + _value);

        labelDay.text = _day;
        labelLog.text = _message;
        colliderLog.offset = new Vector2(150, 0);
        colliderLog.size = new Vector2(labelLog.width + 50, labelLog.height + 25);
    }

}
