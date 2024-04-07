using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xmlSudenEvent
{
    public int index; // 순번
    public string event_title; // 이벤트명
    public string bgm; // 브금
    public string sfx; // 효과음
    public int type; // 대사 타입
    public int tag; // 선택지 타입 
    public string name; // 이름
    public string ch_img; // 이미지 
    public string message; // 대사
    public string choice_0; // 선택지 1번
    public string choice_1; // 선택지 2번
    public string choice_2; // 선택지 3번
    public string stateKey_0; // 가감될 수치의 키값  ( eHP,eGOLD... )
    public string stateValue_0; // 가감될 수치값 (0, -1, 1)
    public string stateKey_1;// 가감될 수치의 키값  ( eHP,eGOLD... )
    public string stateValue_1;// 가감될 수치값 (0, -1, 1)
    public string stateKey_2;// 가감될 수치의 키값  ( eHP,eGOLD... )
    public string stateValue_2;// 가감될 수치값 (0, -1, 1)

}
