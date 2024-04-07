using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;

/**********************************************************************/
/// 아이템 관리용 싱글톤
/***********************************************************************/

public class JHItemManager : Ray_Singleton<JHItemManager>
{
    protected string _GOLD = "";
    public string GetGold { set { _GOLD = value; } get { return _GOLD; } }

    protected string _DIA = "";
    public string GetDia { set { GetDia = value; } get { return _DIA; } }

    ///<summary>
    /// 골드 사용 
    ///</summary>
    public bool SetGoldFormula(int gold)
    {
        int crrGold = int.Parse(UserInfoManager.Instance.GetGold());
        int calcGold = crrGold - gold;

        if (calcGold < 0)
        {
            Debug.LogError("골드가 모자릅니다.");
            return false;
        }

        UserInfoManager.Instance.SetGold(calcGold.ToString());

        GetGold = UserInfoManager.Instance.GetGold();
        return true;
    }

    ///<summary>
    /// 골드 얻음 
    ///</summary>
    public void GetGoldFormula(int gold)
    {
        int crrGold = int.Parse(UserInfoManager.Instance.GetGold());
        int calcGold = crrGold + gold;
        UserInfoManager.Instance.SetGold(calcGold.ToString());

        GetGold = UserInfoManager.Instance.GetGold();
    }



    ///<summary>
    /// 골드 사용 
    ///</summary>
    public bool SetDiadFormula(int dia)
    {
        int crrDia = int.Parse(UserInfoManager.Instance.GetDia());
        int calcDia = crrDia - dia;

        if (calcDia < 0)
        {
            Debug.LogError("골드가 모자릅니다.");
            return false;
        }

        UserInfoManager.Instance.SetDia(calcDia.ToString());
        GetDia = UserInfoManager.Instance.GetDia();
        return true;
    }

    ///<summary>
    /// 골드 얻음 
    ///</summary>
    public void GetDiadFormula(int dia)
    {
        int crrDia = int.Parse(UserInfoManager.Instance.GetDia());
        int calcDia = crrDia + dia;
        UserInfoManager.Instance.SetDia(calcDia.ToString());

        GetDia = UserInfoManager.Instance.GetDia();
    }


    ///<summary>
    /// 매니저 아이템 구입
    ///</summary>
    public void SetBuyMangaerItem(eManagerItem item, int price)
    {
        if (SetGoldFormula(price) == true)
        {
            UserInfoManager.Instance.SetSaveManagerItem(item.ToString(), true);
        }
    }

}
