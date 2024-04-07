using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class YieldHelper  {


	private static Dictionary <int, WaitForSeconds> m_pDicTimeYielder = new Dictionary<int, WaitForSeconds>(new IntComparer());
	private static WaitForEndOfFrame m_pEndOfFrameYielder = new WaitForEndOfFrame ();
	private static WaitForFixedUpdate m_pFixedUpdateYielder = new WaitForFixedUpdate();

	public static WaitForSeconds waitForSeconds (int miliSec )
	{
		WaitForSeconds pYielder = null;

		if (!m_pDicTimeYielder.TryGetValue (miliSec, out pYielder)) {
			pYielder = new WaitForSeconds (miliSec * 0.001f);
			m_pDicTimeYielder.Add (miliSec, pYielder);
		}
		return pYielder;
	}

	public static WaitForEndOfFrame waitForEndOfFrame ()
	{
		return m_pEndOfFrameYielder;
	}

	public static WaitForFixedUpdate waitForFixedUpdate ()
	{
		return m_pFixedUpdateYielder;
	}


}

//Dictionary 의 key 에 의 한 Boxing/UnBoxing 을 피 하 기 위 해 비 교 클 래 스 재 정 의 
public class IntComparer : IEqualityComparer<int>
{
    bool IEqualityComparer<int>.Equals(int x, int y)
    {
        return x == y;
    }
    int IEqualityComparer<int>.GetHashCode(int obj)
    {
        return obj.GetHashCode();
    }
}
