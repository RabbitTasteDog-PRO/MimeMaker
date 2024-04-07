using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class IOSSafeArea
{
//     public int addX = 0;
//     public int addY = 0;
//     public int addHeight = 0;

// #if UNITY_EDITOR || UNITY_ANDROID
//     private void GetSafeAreaImpl(out float x, out float y, out float w, out float h)
//     {
//         // x = 0f;
//         // y = 0f;
//         // w = Screen.width;
//         // h = Screen.height;

//         // 테스트용 SafeArea
//         x = 132f;
//         y = 63f;
//         w = 2172f;
//         h = 1062f;
//     }
// #elif UNITY_IOS
//         [DllImport("__Internal")]
//         private extern static void GetSafeAreaImpl(out float x, out float y, out float w, out float h);
// #endif

//     public IOSSafeArea()
//     {
//         float x, y, w, h;
//         GetSafeAreaImpl(out x, out y, out w, out h);

//         float defaultRatio = 16f / 9f;
//         float currentRatio = (float)Screen.width / (float)Screen.height;
//         float ratio = 1f;

//         if (defaultRatio < currentRatio)
//         {
//             ratio = 720f / Screen.height;
//         }
//         else
//         {
//             ratio = 1280f / Screen.width;
//         }

//         addX = (int)(x * ratio);
//         addY = (int)(y * ratio);
//         addHeight = (int)(2 * y * defaultRatio / currentRatio);
//     }
}
