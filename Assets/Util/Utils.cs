using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RayUtils 
{
    public class Utils
    {
        /// 컨버트 
        public static T ConvertEnumData<T>(string _value)
        {
            return (T)Enum.Parse(typeof(T), _value);
        }
    }

}

