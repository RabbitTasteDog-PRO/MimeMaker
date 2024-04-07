using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClass_A : MonoBehaviour
{
    protected int testClass_A;

    protected void TestClassAFnc(int idx_1, int idx_2)
    {
        Debug.Log(string.Format("{0} + {1} : {3}", idx_1, idx_2, (idx_2 + idx_2)));
    }

    protected void TESTAAA() { }

}


public class TestClass_B : TestClass_A
{

    private TestClass_B _instance = null;
    public TestClass_B Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new TestClass_B();
            }

            return _instance;
        }
    }


    public string testClass_B;
    public void TestClassBFnc()
    {
        TESTAAA();
        // classA.Instance.TestClassAFnc(3, 5);
    }

    void TEstBBB()
    {
        TestClassAFnc(3, 4);
    }


}


public class TestClass_C : MonoBehaviour
{


}