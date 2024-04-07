using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class JHDataReader : MonoBehaviour
{
    public struct STDataReadInfo
    {
        public string dataPath;
        public Action<string> ACTION_READ_LINE;
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void ReadTextData(string dataName, Action<string> _ACTION_READ_LINE)
    {
        /**********************************************************************************/
        /// 원본
        // // Debug.Log("############################");
        // TextAsset data = Resources.Load(string.Format("Data/metaData/{0}", dataName), typeof(TextAsset)) as TextAsset;
        // // Debug.Log("############################ : " + data.text);
        // string decryptData = data.text;

        // if (string.IsNullOrEmpty(decryptData))
        // {
        //     return;
        // }

        // using (StringReader sr = new StringReader(decryptData))
        // {
        //     string readLine;

        //     while ((readLine = sr.ReadLine()) != null)
        //     {
        //         if (!readLine.StartsWith("#") && !readLine.StartsWith("\t"))
        //         {
        //             _ACTION_READ_LINE(readLine);
        //         }
        //     }
        // }
        /**********************************************************************************/
        TextAsset targetFile = Resources.Load<TextAsset>(string.Format("Data/json/{0}", dataName));
        /// 패스 설정 
        string jsonFile = targetFile.text;

        // JSONObject json = new JSONObject(jsonFile);
        // Debug.LogError("################# : " + jsonFile );
        // Debug.LogError("################# jsonCount : " + json.Count);

        // JSONObject info = json[0];
        // mJson_ItemInfo = info["items"];

        if (string.IsNullOrEmpty(jsonFile))
        {
            return;
        }

        _ACTION_READ_LINE(jsonFile);

    }

    public IEnumerator IE_ReadData(Action _action)
    {
        _action();
        yield return YieldHelper.waitForEndOfFrame();
    }
}